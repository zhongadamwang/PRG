// @ts-ignore
import { existsSync, readFileSync, writeFileSync } from 'node:fs';
// @ts-ignore
import { join } from 'node:path';
// @ts-ignore

// Import shared utilities
import {
	constructMigrationPath,
	detectProjectInfo,
	ensureDirectoryExists,
	findDomainModelMetadata,
	formatGeneratedCode,
	toPascalCase
} from '../../../utilities/project-utilities/scripts/utilities';

// @ts-ignore
const process = globalThis.process;

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
	methods: any[];
	category?: string;
}

interface Relationship {
	id: string;
	sourceEntity: string;
	targetEntity: string;
	type: 'association' | 'composition' | 'aggregation' | 'inheritance' | 'dependency';
	cardinality?: string;
	label?: string;
	description?: string;
}

interface DomainModelMetadata {
	version: string;
	generatedAt: string;
	sourceFile: string;
	entities: Entity[];
	relationships: Relationship[];
	enums: any[];
	statistics: any;
}

interface MigrationOptions {
	migrationName: string;
	namespace: string;
	outputDirectory: string;
	generateComments: boolean;
}

// Convert snake_case to PascalCase for C# properties - USE UTILITY VERSION
// function toPascalCase() - now imported from utilities

// Map domain model types to SQL Server data types
function mapToSqlType(domainType: string, constraints?: string[]): string {
	// Handle array types
	if (domainType.endsWith('[]')) {
		return 'nvarchar(max)';
	}

	const baseType = domainType.replace('?', '');

	const typeMap: Record<string, string> = {
		'string': 'nvarchar(255)',
		'int': 'int',
		'float': 'real',
		'bool': 'bit',
		'DateTime': 'datetime2',
		'decimal': 'decimal(18,2)',
		'guid': 'uniqueidentifier',
		'Guid': 'uniqueidentifier'
	};

	// Handle email constraint
	if (constraints && constraints.includes('email')) {
		return 'nvarchar(320)';
	}

	// Handle enum types
	if (domainType.toLowerCase().endsWith('enum')) {
		return 'int';
	}

	return typeMap[baseType] || 'nvarchar(max)';
}

// Project path detection - USE UTILITY VERSION
// function detectMigrationPath() - now replaced by constructMigrationPath() from utilities

// Generate migration timestamp using system timezone or specified timezone
function generateMigrationTimestamp(timezone: string = 'Asia/Shanghai'): string {
	const now = new Date();

	// Method 1: Using Intl.DateTimeFormat with timezone (most modern and reliable)
	try {
		const formatter = new Intl.DateTimeFormat('en-CA', {
			timeZone: timezone,
			year: 'numeric',
			month: '2-digit',
			day: '2-digit',
			hour: '2-digit',
			minute: '2-digit',
			second: '2-digit',
			hour12: false
		});

		const parts = formatter.formatToParts(now);
		const partsMap = Object.fromEntries(parts.map(part => [part.type, part.value]));

		return `${partsMap.year}${partsMap.month}${partsMap.day}${partsMap.hour}${partsMap.minute}${partsMap.second}`;

	} catch (error) {
		// Fallback: Method 2 - Using toLocaleString with timezone 
		console.warn(`⚠️  Timezone ${timezone} not supported, falling back to locale-based formatting`);

		try {
			const timeStr = now.toLocaleString('zh-CN', {
				timeZone: timezone,
				year: 'numeric',
				month: '2-digit',
				day: '2-digit',
				hour: '2-digit',
				minute: '2-digit',
				second: '2-digit',
				hour12: false
			});

			// Parse the formatted string (format: YYYY/MM/DD HH:mm:ss)
			const match = timeStr.match(/(\d{4})\/(\d{2})\/(\d{2})\s+(\d{2}):(\d{2}):(\d{2})/);
			if (match) {
				const [, year, month, day, hours, minutes, seconds] = match;
				return `${year}${month}${day}${hours}${minutes}${seconds}`;
			}
		} catch (localeError) {
			console.warn('⚠️  Locale-based formatting failed, using manual offset calculation');
		}

		// Fallback: Method 3 - Manual UTC offset for China Standard Time (UTC+8)
		const chinaOffset = 8 * 60 * 60 * 1000; // 8 hours in milliseconds
		const chinaTime = new Date(now.getTime() + chinaOffset);

		const year = chinaTime.getUTCFullYear();
		const month = String(chinaTime.getUTCMonth() + 1).padStart(2, '0');
		const day = String(chinaTime.getUTCDate()).padStart(2, '0');
		const hours = String(chinaTime.getUTCHours()).padStart(2, '0');
		const minutes = String(chinaTime.getUTCMinutes()).padStart(2, '0');
		const seconds = String(chinaTime.getUTCSeconds()).padStart(2, '0');

		return `${year}${month}${day}${hours}${minutes}${seconds}`;
	}
}

// Generate table creation statements for Up() method
function generateTableCreations(entities: Entity[], relationships: Relationship[]): string[] {
	const statements: string[] = [];

	// Filter only entity types
	const entityTypes = entities.filter(e => e.type === 'entity');

	for (const entity of entityTypes) {
		statements.push(`        // Create ${entity.name} table`);
		statements.push(`        migrationBuilder.CreateTable(`);
		statements.push(`            name: "${entity.name}s",`);
		statements.push(`            columns: table => new`);
		statements.push(`            {`);

		// Generate columns
		const columns: string[] = [];

		// Primary key (assume first attribute or add Id if none)
		const firstAttr = entity.attributes[0];
		const hasIdAttribute = entity.attributes.some(attr => attr.name.toLowerCase().includes('id'));

		if (!hasIdAttribute) {
			columns.push(`                Id = table.Column<int>(type: "int", nullable: false)`);
			columns.push(`                    .Annotation("SqlServer:Identity", "1, 1")`);
		}

		// Entity attributes
		for (const attr of entity.attributes) {
			const propertyName = toPascalCase(attr.name);
			const sqlType = mapToSqlType(attr.type, attr.constraints);
			const nullable = attr.isOptional ? 'true' : 'false';

			let columnDef = `                ${propertyName} = table.Column<`;

			// Determine C# type
			if (attr.type === 'string') {
				columnDef += `string>(type: "${sqlType}", nullable: ${nullable}`;
			} else if (attr.type === 'int') {
				columnDef += `int>(type: "${sqlType}", nullable: ${nullable}`;
			} else if (attr.type === 'DateTime') {
				columnDef += `DateTime>(type: "${sqlType}", nullable: ${nullable}`;
			} else if (attr.type === 'bool') {
				columnDef += `bool>(type: "${sqlType}", nullable: ${nullable}`;
			} else if (attr.type === 'float') {
				columnDef += `float>(type: "${sqlType}", nullable: ${nullable}`;
			} else if (attr.type.toLowerCase().endsWith('enum')) {
				columnDef += `int>(type: "${sqlType}", nullable: ${nullable}`;
			} else {
				columnDef += `string>(type: "${sqlType}", nullable: ${nullable}`;
			}

			// Add identity for ID columns
			if (attr.name.toLowerCase().includes('id') && attr.type === 'int' && !attr.isOptional) {
				columnDef += ')\n                    .Annotation("SqlServer:Identity", "1, 1")';
			} else {
				columnDef += ')';
			}

			columns.push(columnDef);
		}

		// Add foreign key columns for relationships
		const incomingRels = relationships.filter(rel =>
			rel.targetEntity === entity.name &&
			entities.some(e => e.name === rel.sourceEntity && e.type === 'entity')
		);

		for (const rel of incomingRels) {
			const fkColumnName = `${rel.sourceEntity}Id`;
			// Only add if not already exists as an attribute
			const existingAttr = entity.attributes.find(attr =>
				toPascalCase(attr.name) === fkColumnName
			);

			if (!existingAttr) {
				columns.push(`                ${fkColumnName} = table.Column<int>(type: "int", nullable: true)`);
			}
		}

		// Join columns with commas
		statements.push(columns.join(',\n'));

		statements.push(`            },`);

		// Constraints
		statements.push(`            constraints: table =>`);
		statements.push(`            {`);

		// Primary key
		const pkColumn = hasIdAttribute ? toPascalCase(firstAttr.name) : 'Id';
		statements.push(`                table.PrimaryKey("PK_${entity.name}s", x => x.${pkColumn});`);

		// Foreign key constraints
		for (const rel of incomingRels) {
			const fkColumnName = `${rel.sourceEntity}Id`;
			const existingAttr = entity.attributes.find(attr =>
				toPascalCase(attr.name) === fkColumnName
			);

			if (!existingAttr) {
				statements.push(`                table.ForeignKey(`);
				statements.push(`                    name: "FK_${entity.name}s_${rel.sourceEntity}s_${fkColumnName}",`);
				statements.push(`                    column: x => x.${fkColumnName},`);
				statements.push(`                    principalTable: "${rel.sourceEntity}s",`);
				statements.push(`                    principalColumn: "Id",`);
				statements.push(`                    onDelete: ReferentialAction.Restrict);`);
			}
		}

		statements.push(`            });`);
		statements.push(``);
	}

	return statements;
}

// Generate index creation statements
function generateIndexCreations(entities: Entity[]): string[] {
	const statements: string[] = [];

	const entityTypes = entities.filter(e => e.type === 'entity');

	for (const entity of entityTypes) {
		// Generate indexes for email fields, foreign keys, and unique constraints
		for (const attr of entity.attributes) {
			const propertyName = toPascalCase(attr.name);

			// Email fields
			if (attr.name.toLowerCase().includes('email')) {
				statements.push(`        // Index on ${entity.name}.${propertyName} for performance`);
				statements.push(`        migrationBuilder.CreateIndex(`);
				statements.push(`            name: "IX_${entity.name}s_${propertyName}",`);
				statements.push(`            table: "${entity.name}s",`);
				statements.push(`            column: "${propertyName}");`);
				statements.push(``);
			}

			// Unique constraints
			if (attr.constraints?.includes('unique')) {
				statements.push(`        // Unique index on ${entity.name}.${propertyName}`);
				statements.push(`        migrationBuilder.CreateIndex(`);
				statements.push(`            name: "IX_${entity.name}s_${propertyName}_Unique",`);
				statements.push(`            table: "${entity.name}s",`);
				statements.push(`            column: "${propertyName}",`);
				statements.push(`            unique: true);`);
				statements.push(``);
			}

			// Foreign key indexes (only for actual foreign key fields ending with 'Id')
			if (attr.name.toLowerCase().endsWith('_id') ||
				(attr.name.toLowerCase().includes('id') && attr.name.toLowerCase() !== entity.id.toLowerCase() + '_id')) {
				const propertyName = toPascalCase(attr.name);
				// Check if this is likely a foreign key (not the entity's own primary key)
				if (propertyName !== toPascalCase(entity.id + '_id') && propertyName.endsWith('Id')) {
					statements.push(`        // Index on ${entity.name}.${propertyName} foreign key`);
					statements.push(`        migrationBuilder.CreateIndex(`);
					statements.push(`            name: "IX_${entity.name}s_${propertyName}",`);
					statements.push(`            table: "${entity.name}s",`);
					statements.push(`            column: "${propertyName}");`);
					statements.push(``);
				}
			}
		}
	}

	return statements;
}

// Generate Down() method statements
function generateDropStatements(entities: Entity[]): string[] {
	const statements: string[] = [];

	const entityTypes = entities.filter(e => e.type === 'entity');

	// Drop tables in reverse order to handle dependencies
	for (const entity of [...entityTypes].reverse()) {
		statements.push(`        migrationBuilder.DropTable(`);
		statements.push(`            name: "${entity.name}s");`);
		statements.push(``);
	}

	return statements;
}

// Generate migration class content
function generateMigrationClass(entities: Entity[], relationships: Relationship[], options: MigrationOptions): string {
	const lines: string[] = [];
	const timestamp = generateMigrationTimestamp();
	const className = `M${timestamp}_${options.migrationName}`; // Add 'M' prefix to avoid starting with number

	// Using statements
	lines.push('using Microsoft.EntityFrameworkCore.Migrations;');
	lines.push('');

	// Namespace
	lines.push(`namespace ${options.namespace};`);
	lines.push('');

	// Class documentation
	if (options.generateComments) {
		lines.push('/// <summary>');
		lines.push(`/// Database migration: ${options.migrationName}`);
		lines.push('/// Generated from domain model metadata');
		lines.push('/// </summary>');
	}

	// Migration class declaration
	lines.push(`public partial class ${className} : Migration`);
	lines.push('{');

	// Up method
	if (options.generateComments) {
		lines.push('    /// <summary>');
		lines.push('    /// Create database schema from domain model');
		lines.push('    /// </summary>');
	}
	lines.push('    protected override void Up(MigrationBuilder migrationBuilder)');
	lines.push('    {');

	// Table creations
	const tableStatements = generateTableCreations(entities, relationships);
	for (const stmt of tableStatements) {
		lines.push(stmt);
	}

	// Index creations
	const indexStatements = generateIndexCreations(entities);
	if (indexStatements.length > 0) {
		lines.push('        // Create performance indexes');
		for (const stmt of indexStatements) {
			lines.push(stmt);
		}
	}

	lines.push('    }');
	lines.push('');

	// Down method
	if (options.generateComments) {
		lines.push('    /// <summary>');
		lines.push('    /// Rollback database schema changes');
		lines.push('    /// </summary>');
	}
	lines.push('    protected override void Down(MigrationBuilder migrationBuilder)');
	lines.push('    {');

	// Drop statements
	const dropStatements = generateDropStatements(entities);
	for (const stmt of dropStatements) {
		lines.push(stmt);
	}

	lines.push('    }');
	lines.push('}');

	return lines.join('\n');
}

// Main generation function
function generateMigration(metadataFilePath: string, migrationName?: string, outputDir?: string, namespace?: string): void {
	console.log('🚀 Starting migration generation...');

	// Validate input file
	if (!existsSync(metadataFilePath)) {
		throw new Error(`Metadata file not found: ${metadataFilePath}`);
	}

	// Read and parse metadata
	const metadataContent = readFileSync(metadataFilePath, 'utf-8');
	const metadata: DomainModelMetadata = JSON.parse(metadataContent);

	// Filter entity and relationship data
	const entities = metadata.entities || [];
	const relationships = metadata.relationships || [];
	const entityCount = entities.filter(e => e.type === 'entity').length;

	console.log(`📋 Found ${entityCount} entities and ${relationships.length} relationships to migrate`);

	// Determine output directory and namespace
	let finalOutputDir: string;
	let finalNamespace: string;
	let projectName: string;

	if (outputDir && namespace) {
		finalOutputDir = outputDir;
		finalNamespace = namespace;
		projectName = 'Unknown';
		console.log('📝 Using provided paths:');
	} else {
		const migrationPaths = constructMigrationPath();
		const projectInfo = detectProjectInfo();
		finalOutputDir = migrationPaths.outputDir;
		finalNamespace = migrationPaths.namespace;
		projectName = projectInfo.projectName;
		console.log('🔍 Using auto-detected paths:');
	}

	console.log(`   📁 Output: ${finalOutputDir}`);
	console.log(`   📦 Namespace: ${finalNamespace}`);

	// Create output directory if it doesn't exist using utility
	ensureDirectoryExists(finalOutputDir);

	const options: MigrationOptions = {
		migrationName: migrationName || 'InitialMigration',
		namespace: finalNamespace,
		outputDirectory: finalOutputDir,
		generateComments: true
	};

	// Generate migration class
	console.log(`🔧 Generating migration: ${options.migrationName}...`);
	const timestamp = generateMigrationTimestamp();
	const migrationClass = generateMigrationClass(entities, relationships, options);

	// Write migration file
	const fileName = `M${timestamp}_${options.migrationName}.cs`;
	const filePath = join(finalOutputDir, fileName);

	try {
		writeFileSync(filePath, migrationClass, 'utf-8');
		console.log(`✅ Generated migration: ${fileName}`);

		// Format generated code using dotnet format
		formatGeneratedCode();

		console.log(`🎉 Migration generation complete!`);
		console.log(`📊 Generated migration with ${entityCount} tables and ${relationships.length} relationships`);

	} catch (error) {
		console.error(`❌ Failed to generate migration:`, error);
		throw error;
	}
}

// Command line interface
function main(): void {
	const args = process.argv.slice(2);

	// Default to auto-detection if no metadata file provided or empty string
	let metadataFile = '';
	if (args.length === 0 || !args[0] || args[0].trim() === '') {
		try {
			metadataFile = findDomainModelMetadata();
			console.log('🔍 Auto-detected metadata file, proceeding with generation...');
		} catch (error) {
			console.log('Usage: bun run generate-migration.ts [metadata-file] [migration-name] [output-directory] [namespace]');
			console.log('');
			console.log('Arguments:');
			console.log('  metadata-file    Path to the JSON metadata file from domain-model-parser (auto-detected if not provided)');
			console.log('  migration-name   Optional: Name of the migration (defaults to "InitialMigration")');
			console.log('  output-directory Optional: Path where migration files will be generated (auto-detected if not provided)');
			console.log('  namespace        Optional: C# namespace for migration classes (auto-detected if not provided)');
			console.log('');
			console.log('Examples:');
			console.log('  bun run generate-migration.ts                                      # Auto-detect all');
			console.log('  bun run generate-migration.ts ./domain-metadata.json');
			console.log('  bun run generate-migration.ts ./domain-metadata.json InitialSchema');
			console.log('  bun run generate-migration.ts ./domain-metadata.json AddUserTables ./migrations Custom.Migrations');
			console.error('');
			console.error(error);
			process.exit(1);
		}
	} else {
		metadataFile = args[0];
	}

	const migrationName = args[1];
	const outputDir = args[2];
	const namespace = args[3];

	console.log(`   📄 Metadata: ${metadataFile}`);
	if (migrationName) console.log(`   🏷️  Migration: ${migrationName}`);

	try {
		generateMigration(metadataFile, migrationName, outputDir, namespace);
	} catch (error) {
		console.error('❌ Migration generation failed:', error);
		process.exit(1);
	}
}

// Run if called directly
// @ts-ignore
if (import.meta.main) {
	main();
}

// Note: No exports needed - skills communicate through Copilot, not direct imports
