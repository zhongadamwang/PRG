// @ts-ignore
import { existsSync, readFileSync, writeFileSync } from 'node:fs';
// @ts-ignore

// @ts-ignore
const process = globalThis.process;

// ─── Types (mirrors parse-domain-model output) ───────────────────────────────

interface EntityAttribute {
	name: string;
	type: string;
	isOptional: boolean;
	isArray: boolean;
	constraints?: string[];
}

interface Entity {
	id: string;
	name: string;
	type: 'entity' | 'actor' | 'enum' | 'system';
	description?: string;
	attributes: EntityAttribute[];
	methods: { name: string; returnType: string; parameters: { name: string; type: string }[] }[];
	category?: string;
}

interface Relationship {
	id: string;
	sourceEntity: string;
	targetEntity: string;
	type: 'association' | 'composition' | 'aggregation' | 'inheritance' | 'dependency';
	cardinality?: string;
	label?: string;
}

interface EnumValue {
	name: string;
	value?: string;
	description?: string;
}

interface DomainEnum {
	id: string;
	name: string;
	values: EnumValue[];
	description?: string;
}

interface DomainModelMetadata {
	version: string;
	generatedAt: string;
	sourceFile: string;
	entities: Entity[];
	relationships: Relationship[];
	enums: DomainEnum[];
	statistics: {
		totalEntities: number;
		totalRelationships: number;
		totalEnums: number;
		totalAttributes: number;
	};
}

// ─── String helpers ───────────────────────────────────────────────────────────

function toPascalCase(name: string): string {
	return name
		.split(/[_\s-]+/)
		.map(part => part.charAt(0).toUpperCase() + part.slice(1).toLowerCase())
		.join('');
}

// DataElement → data_element, ReviewPackage → review_package
function toSnakeCase(name: string): string {
	return name
		.replace(/([A-Z])/g, '_$1')
		.toLowerCase()
		.replace(/^_/, '');
}

// Column names are already snake_case from the domain model parser — pass through unchanged.
function columnName(attrName: string): string {
	return attrName;
}

// DataElement → data_elements, ReviewPackage → review_packages, Request → requests
function tableName(entityName: string): string {
	const base = toSnakeCase(entityName);
	if (base.endsWith('s') || base.endsWith('x') || base.endsWith('z') || base.endsWith('ch') || base.endsWith('sh')) {
		return base + 'es';
	}
	if (base.endsWith('y') && !/[aeiou]y$/.test(base)) {
		return base.slice(0, -1) + 'ies';
	}
	return base + 's';
}

// ─── Type mapping ─────────────────────────────────────────────────────────────

// Attributes whose column names suggest specific sizes
const WIDE_CONTENT_SUFFIXES = ['content', 'notes', 'summary', 'description', 'feedback', 'body', 'message', 'text'];
const EMAIL_SUFFIXES = ['email'];
const SHORT_ID_SUFFIXES = ['_id', 'id'];

function mapToSqlType(attrName: string, domainType: string): string {
	const lower = domainType.toLowerCase().replace('[]', '').trim();
	const lowerAttr = attrName.toLowerCase();

	// Guid
	if (lower === 'guid') return 'UNIQUEIDENTIFIER';

	// Bool
	if (lower === 'bool' || lower === 'boolean') return 'BIT';

	// Numeric
	if (lower === 'int' || lower === 'integer') return 'INT';
	if (lower === 'long' || lower === 'int64') return 'BIGINT';
	if (lower === 'short' || lower === 'int16') return 'SMALLINT';
	if (lower === 'float' || lower === 'single') return 'FLOAT';
	if (lower === 'double') return 'FLOAT';
	if (lower === 'decimal') return 'DECIMAL(18, 2)';

	// Date/Time
	if (lower === 'datetime' || lower === 'datetimeoffset') return 'DATETIME2';
	if (lower === 'date') return 'DATE';
	if (lower === 'timespan' || lower === 'time') return 'TIME';

	// Enum types → stored as NVARCHAR code value
	if (lower.endsWith('enum') || lower.endsWith('enumeration')) return 'INT';

	// Strings: choose width by attribute name
	if (lower === 'string' || lower === 'str' || lower === '') {
		if (WIDE_CONTENT_SUFFIXES.some(s => lowerAttr.endsWith(s) || lowerAttr.includes(s + '_'))) {
			return 'NVARCHAR(MAX)';
		}
		if (EMAIL_SUFFIXES.some(s => lowerAttr.endsWith(s))) return 'NVARCHAR(256)';
		if (SHORT_ID_SUFFIXES.some(s => lowerAttr.endsWith(s))) return 'NVARCHAR(64)';
		return 'NVARCHAR(256)';
	}

	// Fallback: treat unknown types as NVARCHAR
	return 'NVARCHAR(256)';
}

// ─── PK / FK helpers ─────────────────────────────────────────────────────────

// Returns true if the attribute is the entity's own PK.
// Handles exact matches (request_id for Request) and suffix/prefix matches
// for compound entity names (element_id for DataElement, diagram_id for StateDiagram).
function isPrimaryKey(entityName: string, attrName: string): boolean {
	const lowerAttr = attrName.toLowerCase();
	const lowerEntity = entityName.toLowerCase();
	if (lowerAttr === 'id') return true;
	// Exact match: request_id or requestid
	if (lowerAttr === `${lowerEntity}_id` || lowerAttr === `${lowerEntity}id`) return true;
	// Suffix match: element_id for DataElement ("dataelement".endsWith("element"))
	if (lowerAttr.endsWith('_id')) {
		const prefix = lowerAttr.slice(0, -3); // strip '_id'
		if (lowerEntity.endsWith(prefix) || lowerEntity.startsWith(prefix)) return true;
	}
	return false;
}

// Returns true if the attribute is a FK referencing another table (but is not the PK)
function isForeignKey(entityName: string, attrName: string): boolean {
	const lowerAttr = attrName.toLowerCase();
	const lowerEntity = entityName.toLowerCase();
	if (isPrimaryKey(entityName, attrName)) return false;
	// Ends with _id but is NOT this entity's own id
	return lowerAttr.endsWith('_id') && lowerAttr !== `${lowerEntity}_id`;
}

// Best guess at what table the FK points to
function fkTargetTable(attrName: string): string {
	// strip trailing _id; input is already snake_case so pass directly to tableName
	const stripped = attrName.replace(/_id$/i, '');
	return tableName(stripped);
}

// ─── Enum collector ───────────────────────────────────────────────────────────

// Collect all unique enum names referenced in entity attributes
function collectReferencedEnums(entities: Entity[]): Set<string> {
	const refs = new Set<string>();
	for (const entity of entities) {
		for (const attr of entity.attributes) {
			const lower = attr.type.toLowerCase().replace('[]', '');
			if (lower.endsWith('enum') || lower.endsWith('enumeration')) {
				refs.add(attr.type.replace('[]', '').trim());
			}
		}
	}
	return refs;
}

// ─── Topological sort ─────────────────────────────────────────────────────────

// Build a dependency map: entity → set of entities it depends on (via FK attrs)
function buildDependencyGraph(entities: Entity[]): Map<string, Set<string>> {
	const graph = new Map<string, Set<string>>();
	for (const entity of entities) {
		graph.set(entity.name, new Set());
	}
	for (const entity of entities) {
		for (const attr of entity.attributes) {
			if (!attr.isArray && isForeignKey(entity.name, attr.name)) {
				const target = fkTargetTable(attr.name);
				// Find the entity whose table name matches the target
				const dep = entities.find(e => tableName(e.name) === target);
				if (dep && dep.name !== entity.name) {
					graph.get(entity.name)!.add(dep.name);
				}
			}
		}
	}
	return graph;
}

function topologicalSort(entities: Entity[]): Entity[] {
	const graph = buildDependencyGraph(entities);
	const visited = new Set<string>();
	const sorted: Entity[] = [];

	function visit(name: string): void {
		if (visited.has(name)) return;
		visited.add(name);
		const deps = graph.get(name) ?? new Set();
		for (const dep of deps) {
			visit(dep);
		}
		const entity = entities.find(e => e.name === name);
		if (entity) sorted.push(entity);
	}

	for (const entity of entities) {
		visit(entity.name);
	}
	return sorted;
}

// ─── SQL generators ───────────────────────────────────────────────────────────

const AUDIT_COLUMNS = `    -- Audit columns
    [created_at]          DATETIME2      NOT NULL  CONSTRAINT [DF_{TABLE}_created_at]  DEFAULT GETUTCDATE(),
    [created_by]          NVARCHAR(256)  NOT NULL  CONSTRAINT [DF_{TABLE}_created_by]  DEFAULT N'',
    [updated_at]          DATETIME2          NULL,
    [updated_by]          NVARCHAR(256)      NULL`;

function generateEnumLookupTable(enumTypeName: string, enumDef: DomainEnum | undefined): string {
	const tName = `${toPascalCase(enumTypeName)}_Lookup`;
	const lines: string[] = [];
	lines.push(`-- Lookup table for enum: ${enumTypeName}`);
	lines.push(`CREATE TABLE [dbo].[${tName}] (`);
	lines.push(`    [Code]        NVARCHAR(64)   NOT NULL,`);
	lines.push(`    [Description] NVARCHAR(256)      NULL,`);
	lines.push(`    CONSTRAINT [PK_${tName}] PRIMARY KEY ([Code])`);
	lines.push(`);`);

	// Seed data if enum values are known
	const values = enumDef?.values ?? [];
	if (values.length > 0) {
		lines.push('');
		lines.push(`INSERT INTO [dbo].[${tName}] ([Code], [Description]) VALUES`);
		const valueRows = values.map((v, i) => {
			const comma = i < values.length - 1 ? ',' : '';
			const desc = v.description ? v.description : v.name;
			return `    (N'${v.name}', N'${desc}')${comma}`;
		});
		lines.push(...valueRows);
		lines.push(';');
	}

	return lines.join('\n');
}

function generateArrayValueTable(entity: Entity, attr: EntityAttribute): string {
	const parentTable = tableName(entity.name);
	const pkAttr = entity.attributes.find(a => isPrimaryKey(entity.name, a.name));
	const pkCol = pkAttr ? columnName(pkAttr.name) : toPascalCase(entity.name) + 'Id';
	const pkType = pkAttr ? mapToSqlType(pkAttr.name, pkAttr.type) : 'NVARCHAR(64)';

	const valueTableName = `${toPascalCase(entity.name)}_${toPascalCase(attr.name)}`;
	const valueColName = toPascalCase(attr.name.replace(/s$/, '')); // naïve singularise
	const valueType = mapToSqlType(attr.name, attr.type);

	const lines: string[] = [];
	lines.push(`-- Array values table for ${entity.name}.${attr.name}`);
	lines.push(`CREATE TABLE [dbo].[${valueTableName}] (`);
	lines.push(`    [Id]              INT             NOT NULL  IDENTITY(1,1),`);
	lines.push(`    [${pkCol}]        ${pkType.padEnd(14)} NOT NULL,`);
	lines.push(`    [${valueColName}] ${valueType.padEnd(14)} NOT NULL,`);
	lines.push(`    CONSTRAINT [PK_${valueTableName}] PRIMARY KEY ([Id]),`);
	lines.push(`    CONSTRAINT [FK_${valueTableName}_${parentTable}]`);
	lines.push(`        FOREIGN KEY ([${pkCol}]) REFERENCES [dbo].[${parentTable}] ([${pkCol}])`);
	lines.push(`        ON DELETE CASCADE`);
	lines.push(`);`);
	return lines.join('\n');
}

function generateCreateTable(entity: Entity): string {
	const tName = tableName(entity.name);
	const lines: string[] = [];

	if (entity.description) {
		lines.push(`-- ${entity.description}`);
	}
	lines.push(`CREATE TABLE [dbo].[${tName}] (`);

	const colLines: string[] = [];
	let pkCol = '';

	for (const attr of entity.attributes) {
		if (attr.isArray) continue; // Array attrs → separate table

		const col = columnName(attr.name);
		const sqlType = mapToSqlType(attr.name, attr.type);
		const nullable = attr.isOptional ? '    NULL' : 'NOT NULL';

		const padding = Math.max(0, 20 - col.length);
		colLines.push(`    [${col}]${' '.repeat(padding)} ${sqlType.padEnd(14)} ${nullable},`);

		if (isPrimaryKey(entity.name, attr.name)) {
			pkCol = col;
		}
	}

	// Append audit columns
	const auditBlock = AUDIT_COLUMNS.replace(/\{TABLE\}/g, tName);
	colLines.push(auditBlock + ',');

	// Append PK constraint
	if (pkCol) {
		colLines.push(`    CONSTRAINT [PK_${tName}] PRIMARY KEY ([${pkCol}])`);
	} else {
		// Fallback – add a surrogate key
		console.warn(`  ⚠️  No primary key found for ${entity.name}. Adding surrogate [Id] INT IDENTITY.`);
		colLines.unshift(`    [Id]                 INT             NOT NULL  IDENTITY(1,1),`);
		colLines.push(`    CONSTRAINT [PK_${tName}] PRIMARY KEY ([Id])`);
	}

	// Remove trailing comma from last real column before constraints block
	// The audit block already has comma in it; fix up the last line before the PK line
	lines.push(...colLines);
	lines.push(');');

	// FK indexes
	const indexLines: string[] = [];
	for (const attr of entity.attributes) {
		if (!attr.isArray && isForeignKey(entity.name, attr.name)) {
			const col = columnName(attr.name);
			indexLines.push(`CREATE NONCLUSTERED INDEX [IX_${tName}_${col}]`);
			indexLines.push(`    ON [dbo].[${tName}] ([${col}]);`);
		}
	}

	if (indexLines.length > 0) {
		lines.push('');
		lines.push(...indexLines);
	}

	return lines.join('\n');
}

// ─── FK constraint DDL (emitted after all tables are created) ─────────────────

// Build the set of table names that will actually exist in the output schema.
function buildExistingTableNames(entities: Entity[]): Set<string> {
	const names = new Set<string>();
	for (const e of entities) names.add(tableName(e.name));
	return names;
}

function generateForeignKeyConstraints(entities: Entity[]): string[] {
	const statements: string[] = [];
	const existingTables = buildExistingTableNames(entities);

	for (const entity of entities) {
		const tName = tableName(entity.name);
		for (const attr of entity.attributes) {
			if (!attr.isArray && isForeignKey(entity.name, attr.name)) {
				const col = columnName(attr.name);
				const target = fkTargetTable(attr.name);

				// Only emit FK if the target table actually exists in this schema
				if (!existingTables.has(target)) {
					statements.push(
						`-- NOTE: FK for [${tName}].[${col}] → [${target}] skipped` +
						` (target table not in this schema — define manually if needed)`
					);
					continue;
				}

				// Find the referenced table's PK column
				const targetEntity = entities.find(e => tableName(e.name) === target)!;
				const targetPkAttr = targetEntity.attributes.find(a => isPrimaryKey(targetEntity.name, a.name));
				const targetPkCol = targetPkAttr ? columnName(targetPkAttr.name) : col;

				const constraintName = `FK_${tName}_${col}_${target}`;
				statements.push(
					`ALTER TABLE [dbo].[${tName}]\n` +
					`    ADD CONSTRAINT [${constraintName}]\n` +
					`    FOREIGN KEY ([${col}]) REFERENCES [dbo].[${target}] ([${targetPkCol}]);`
				);
			}
		}
	}

	return statements;
}

// ─── Main ─────────────────────────────────────────────────────────────────────

function main(): void {
	const args = process.argv.slice(2);

	if (args.length === 0) {
		console.error('❌ Usage: bun run generate-sql-schema.ts <metadata-json-path>');
		process.exit(1);
	}

	const inputPath = args[0] as string;
	const outputPath = inputPath.replace(/\.json$/i, '.sql').replace('metadata', 'schema');

	if (!existsSync(inputPath)) {
		console.error(`❌ Metadata file not found: ${inputPath}`);
		process.exit(1);
	}

	let metadata = {} as DomainModelMetadata;
	try {
		const raw = readFileSync(inputPath, 'utf-8');
		metadata = JSON.parse(raw) as DomainModelMetadata;
	} catch (e) {
		console.error('❌ Failed to parse metadata JSON:', e);
		process.exit(1);
	}

	console.log(`📖 Loaded metadata: ${metadata.statistics.totalEntities} entities, ${metadata.statistics.totalEnums} enums`);

	// ── Filter entities: only 'entity' and 'actor' types get tables ──
	const tableEntities = metadata.entities.filter(
		e => e.type === 'entity' || e.type === 'actor'
	);

	// De-duplicate by name (metadata may have repeated inferred enums)
	const seenEntityNames = new Set<string>();
	const uniqueEntities: Entity[] = [];
	for (const e of tableEntities) {
		if (!seenEntityNames.has(e.name.toLowerCase())) {
			seenEntityNames.add(e.name.toLowerCase());
			uniqueEntities.push(e);
		}
	}

	console.log(`🗂️  Generating tables for ${uniqueEntities.length} entities`);

	// ── Sort entities by dependency ──
	const sortedEntities = topologicalSort(uniqueEntities);

	// ── Build SQL output ──
	const sections: string[] = [];

	// Header
	sections.push([
		'-- ============================================================',
		`-- SQL Server DDL Script`,
		`-- Generated by domain-model-sql-generator`,
		`-- Source: ${inputPath}`,
		`-- Generated: ${new Date().toISOString()}`,
		'-- ============================================================',
		'',
		'USE [$(DatabaseName)];',
		'GO',
		'',
		'SET NOCOUNT ON;',
		'GO',
	].join('\n'));

	// Entity tables
	sections.push('-- ============================================================');
	sections.push('-- ENTITY TABLES');
	sections.push('-- ============================================================');

	for (const entity of sortedEntities) {
		console.log(`  📋 Generating table: ${tableName(entity.name)}`);
		sections.push(generateCreateTable(entity));
	}

	// Footer
	sections.push([
		'',
		'-- ============================================================',
		'-- END OF DDL SCRIPT',
		'-- ============================================================',
	].join('\n'));

	const output = sections.join('\n\n') + '\n';

	writeFileSync(outputPath, output, 'utf-8');

	console.log(`\n✅ SQL schema script written to: ${outputPath}`);
	console.log(`📊 Summary:`);
	console.log(`   - Entity tables: ${sortedEntities.length}`);
}

// @ts-ignore
if (import.meta.main) {
	main();
}
