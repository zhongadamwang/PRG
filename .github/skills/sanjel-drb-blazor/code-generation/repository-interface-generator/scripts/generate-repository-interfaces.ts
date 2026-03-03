// @ts-ignore
import { existsSync, writeFileSync } from 'node:fs';
// @ts-ignore
import { join } from 'node:path';
// @ts-ignore

// Import shared utilities
import {
	constructRepositoryPath,
	detectProjectInfo,
	ensureDirectoryExists,
	findDomainModelMetadata,
	formatGeneratedCode,
	readJsonFile,
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

interface GenerationOptions {
	namespace: string;
	outputDirectory: string;
	generateComments: boolean;
	includePerformanceMarkers: boolean;
}

// Generate base IRepository<T> interface (check if update is needed)
function generateBaseRepositoryInterface(options: GenerationOptions): string {
	const lines: string[] = [];

	// Using statements
	lines.push('using System;');
	lines.push('using System.Collections.Generic;');
	lines.push('using System.Linq;');
	lines.push('using System.Linq.Expressions;');
	lines.push('using System.Threading;');
	lines.push('using System.Threading.Tasks;');
	lines.push('');

	// Namespace
	lines.push(`namespace ${options.namespace}.Common;`);
	lines.push('');

	// Interface documentation
	if (options.generateComments) {
		lines.push('/// <summary>');
		lines.push('/// Generic repository interface for EF Core-based CRUD operations and querying.');
		lines.push('/// </summary>');
		lines.push('/// <typeparam name="TEntity">Entity type that inherits from a base entity class.</typeparam>');
	}

	// Interface declaration
	lines.push('public interface IRepository<TEntity> where TEntity : class');
	lines.push('{');

	// Modern EF Core method signatures
	const methods = [
		{
			signature: 'IQueryable<TEntity> Query()',
			comment: 'Gets a queryable collection of entities for advanced querying'
		},
		{
			signature: 'Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default)',
			comment: 'Gets an entity by its primary key'
		},
		{
			signature: 'Task<TEntity?> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)',
			comment: 'Gets a single entity matching the predicate'
		},
		{
			signature: 'Task<TEntity?> GetFirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)',
			comment: 'Gets the first entity matching the predicate'
		},
		{
			signature: 'Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)',
			comment: 'Gets all entities matching the predicate'
		},
		{
			signature: 'Task<PagedResult<TEntity>> GetPagedAsync(int skip, int take, Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, CancellationToken cancellationToken = default)',
			comment: 'Gets a paged list of entities'
		},
		{
			signature: 'Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)',
			comment: 'Checks if any entities match the predicate'
		},
		{
			signature: 'Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)',
			comment: 'Count entities matching the predicate'
		},
		{
			signature: 'Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)',
			comment: 'Adds a new entity'
		},
		{
			signature: 'Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)',
			comment: 'Adds multiple entities'
		},
		{
			signature: 'TEntity Update(TEntity entity)',
			comment: 'Updates an existing entity'
		},
		{
			signature: 'void UpdateRange(IEnumerable<TEntity> entities)',
			comment: 'Updates multiple entities'
		},
		{
			signature: 'void Remove(TEntity entity)',
			comment: 'Removes an entity'
		},
		{
			signature: 'void RemoveRange(IEnumerable<TEntity> entities)',
			comment: 'Removes multiple entities'
		},
		{
			signature: 'Task<bool> RemoveByIdAsync(object id, CancellationToken cancellationToken = default)',
			comment: 'Removes an entity by its primary key'
		},
		{
			signature: 'Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)',
			comment: 'Saves all changes to the database'
		}
	];

	for (let i = 0; i < methods.length; i++) {
		const method = methods[i];

		if (options.generateComments) {
			lines.push('\t/// <summary>');
			lines.push(`\t/// ${method.comment}`);
			lines.push('\t/// </summary>');
		}

		lines.push(`\t${method.signature};`);

		if (i < methods.length - 1) {
			lines.push('');
		}
	}

	lines.push('}');

	return lines.join('\n');
}

// Map attribute names to correct enum type names based on metadata
function mapAttributeToEnumType(attributeName: string, attributeType: string, entityName: string, metadata: DomainModelMetadata): string {
	// If it's already a proper enum type, return it
	if (attributeType.endsWith('Enum')) {
		return attributeType;
	}

	// Look for matching enum in the entities section (enum entities)
	const enumEntity = metadata.entities.find(entity => {
		if (entity.type !== 'enum') return false;

		// Normalize names for comparison
		const entityName = entity.name.toLowerCase();
		const attrType = attributeType.toLowerCase().replace('enum', '');
		const attrName = attributeName.toLowerCase().replace(/_/g, '');

		// Check various matching patterns
		return entityName === attrType ||
			entityName === attrName ||
			entityName === `${attrName}enum` ||
			entityName === `${attrType}enum` ||
			entity.id === attributeType.toLowerCase();
	});

	if (enumEntity) {
		return enumEntity.name;
	}

	// Look for matching enum in the enums section if available
	if (metadata.enums && metadata.enums.length > 0) {
		const matchingEnum = metadata.enums.find(enumDef => {
			if (!enumDef.name) return false;

			// Normalize names for comparison
			const enumName = enumDef.name.toLowerCase();
			const attrType = attributeType.toLowerCase().replace('enum', '');
			const attrName = attributeName.toLowerCase().replace(/_/g, '');

			// Check various matching patterns
			return enumName === attrType ||
				enumName === attrName ||
				enumName === `${attrName}enum` ||
				enumName === `${attrType}enum` ||
				enumDef.id === attributeType.toLowerCase();
		});

		if (matchingEnum && matchingEnum.name) {
			// Ensure proper PascalCase and 'Enum' suffix
			let enumName = toPascalCase(matchingEnum.name);
			if (!enumName.endsWith('Enum')) {
				enumName += 'Enum';
			}
			return enumName;
		}
	}

	// Fallback: construct enum name from attribute type/name
	if (attributeType.toLowerCase().includes('enum')) {
		let baseName = attributeType.replace(/enum$/i, '');
		return toPascalCase(baseName) + 'Enum';
	}

	// Last resort: use attribute name
	return toPascalCase(attributeName) + 'Enum';
}

// Generate entity-specific repository interface
function generateEntityRepositoryInterface(entity: Entity, metadata: DomainModelMetadata, options: GenerationOptions): string {
	const lines: string[] = [];

	// Using statements - simplified like backup version
	lines.push('using System.ComponentModel;');
	lines.push('using System.Linq.Expressions;');
	lines.push('using Sanjel.RequestManagement.Core.Entities;');
	lines.push(`using ${options.namespace}.Common;`);
	lines.push(`using Entity = ${detectProjectInfo().projectName}.Core.Entities.${entity.name};`);
	lines.push('');

	// Namespace
	lines.push(`namespace ${options.namespace};`);
	lines.push('');

	// Interface documentation
	if (options.generateComments) {
		lines.push('/// <summary>');
		lines.push(`/// Repository interface for ${entity.name} entity operations.`);
		if (entity.description) {
			// Ensure description ends with a period for StyleCop compliance
			const description = entity.description.endsWith('.') ? entity.description : entity.description + '.';
			lines.push(`/// ${description}`);
		}
		lines.push('/// </summary>');
	}

	// Interface declaration
	const interfaceName = `I${entity.name}Repository`;
	lines.push(`public interface ${interfaceName} : IRepository<Entity>`);
	lines.push('{');

	// Add entity-specific methods based on attributes and relationships
	const entitySpecificMethods = generateModernEntitySpecificMethods(entity, metadata, options);

	if (entitySpecificMethods.length > 0) {
		// Add methods with smart spacing - only add empty lines between complete method blocks
		let previousWasMethod = false;
		for (let i = 0; i < entitySpecificMethods.length; i++) {
			const currentLine = entitySpecificMethods[i];

			// Detect if current line is a method signature (ends with semicolon)
			const isMethod = currentLine.trim().endsWith(';');

			// Add empty line before XML comment if we just finished a method
			if (currentLine.trim().startsWith('/// <summary>') && previousWasMethod) {
				lines.push('');
			}

			lines.push(currentLine);
			previousWasMethod = isMethod;
		}
	} else {
		// Add a comment if no specific methods are needed
		lines.push('\t// Entity-specific repository methods can be added here as needed');
		lines.push('\t// Use the base IRepository<T> methods for standard CRUD operations');
	}

	lines.push('}');

	return lines.join('\n');
}

// Generate modern EF Core entity-specific methods based on attributes and relationships
function generateModernEntitySpecificMethods(entity: Entity, metadata: DomainModelMetadata, options: GenerationOptions): string[] {
	const methods: string[] = [];

	// Look for common patterns in entity attributes that might need specific repository methods

	// 1. Status-based queries (if entity has a status field)
	const statusAttribute = entity.attributes.find(attr =>
		attr.name.toLowerCase().includes('status') && attr.type.toLowerCase().includes('enum')
	);

	if (statusAttribute) {
		if (options.generateComments) {
			methods.push('\t/// <summary>');
			methods.push(`\t/// Gets ${entity.name} entities by status.`);
			methods.push('\t/// </summary>');
			methods.push(`\t/// <param name="${statusAttribute.name.toLowerCase()}">The status to filter by.</param>`);
			methods.push('\t/// <param name="cancellationToken">The cancellation token.</param>');
			methods.push(`\t/// <returns>A task that represents the asynchronous operation. The task result contains a list of ${entity.name} entities.</returns>`);
		}
		if (options.includePerformanceMarkers) {
			methods.push('\t[Description("EF Core Query - High frequency query, consider indexing")]');
		}

		// Map to correct enum type name based on metadata
		let statusType = mapAttributeToEnumType(statusAttribute.name, statusAttribute.type, entity.name, metadata);
		const methodName = `GetBy${toPascalCase(statusAttribute.name)}Async`;
		methods.push(`\tTask<List<Entity>> ${methodName}(${statusType} ${statusAttribute.name.toLowerCase()}, CancellationToken cancellationToken = default);`);
	}

	// 2. Date-based range queries (if entity has date fields)
	const dateAttributes = entity.attributes.filter(attr =>
		attr.type.toLowerCase().includes('datetime') || attr.type.toLowerCase().includes('date')
	);

	for (const dateAttr of dateAttributes) {
		if (options.generateComments) {
			methods.push('\t/// <summary>');
			methods.push(`\t/// Gets ${entity.name} entities within a date range for ${dateAttr.name}.`);
			methods.push('\t/// </summary>');
			methods.push('\t/// <param name="startDate">The start date of the range.</param>');
			methods.push('\t/// <param name="endDate">The end date of the range.</param>');
			methods.push('\t/// <param name="cancellationToken">The cancellation token.</param>');
			methods.push(`\t/// <returns>A task that represents the asynchronous operation. The task result contains a list of ${entity.name} entities.</returns>`);
		}
		if (options.includePerformanceMarkers) {
			methods.push('\t[Description("EF Core Query - Date range query, consider indexing")]');
		}

		const methodName = `GetBy${toPascalCase(dateAttr.name)}RangeAsync`;
		methods.push(`\tTask<List<Entity>> ${methodName}(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);`);
	}

	// 3. Search methods for string fields
	const searchableAttributes = entity.attributes.filter(attr =>
		attr.type === 'string' &&
		(attr.name.toLowerCase().includes('name') ||
			attr.name.toLowerCase().includes('title') ||
			attr.name.toLowerCase().includes('description'))
	);

	for (const searchAttr of searchableAttributes) {
		if (options.generateComments) {
			methods.push('\t/// <summary>');
			methods.push(`\t/// Searches ${entity.name} entities by ${searchAttr.name} containing the specified text.`);
			methods.push('\t/// </summary>');
			methods.push('\t/// <param name="searchText">The text to search for.</param>');
			methods.push('\t/// <param name="cancellationToken">The cancellation token.</param>');
			methods.push(`\t/// <returns>A task that represents the asynchronous operation. The task result contains a list of ${entity.name} entities.</returns>`);
		}
		if (options.includePerformanceMarkers) {
			methods.push('\t[Description("EF Core Query - Text search, consider full-text indexing")]');
		}

		const methodName = `SearchBy${toPascalCase(searchAttr.name)}Async`;
		methods.push(`\tTask<List<Entity>> ${methodName}(string searchText, CancellationToken cancellationToken = default);`);
	}

	// 4. Foreign key relationship queries
	const relationships = metadata.relationships.filter(rel =>
		rel.sourceEntity === entity.name || rel.targetEntity === entity.name
	);

	for (const rel of relationships) {
		if (rel.sourceEntity === entity.name) {
			// This entity is the source - add method to get by target entity
			const targetEntity = rel.targetEntity;

			// Find target entity to check if it's an actual entity (not actor/enum/system)
			const targetEntityDef = metadata.entities.find(e => e.name === targetEntity);
			if (targetEntityDef && targetEntityDef.type === 'entity') {
				if (options.generateComments) {
					methods.push('\t/// <summary>');
					methods.push(`\t/// Gets ${entity.name} entities related to a specific ${targetEntity}.`);
					methods.push('\t/// </summary>');
					methods.push(`\t/// <param name="${targetEntity.toLowerCase()}Id">The ID of the ${targetEntity.toLowerCase()}.</param>`);
					methods.push('\t/// <param name="cancellationToken">The cancellation token.</param>');
					methods.push(`\t/// <returns>A task that represents the asynchronous operation. The task result contains a list of ${entity.name} entities.</returns>`);
				}

				if (options.includePerformanceMarkers) {
					methods.push('\t[Description("EF Core Query - Foreign key relationship")]');
				}

				methods.push(`\tTask<List<Entity>> GetBy${targetEntity}IdAsync(int ${targetEntity.toLowerCase()}Id, CancellationToken cancellationToken = default);`);
			}
		}
	}

	// 5. Pagination methods for high-volume entities
	if (entity.attributes.length > 5) { // Assume entities with many attributes might have high volume
		if (options.generateComments) {
			methods.push('\t/// <summary>');
			methods.push(`\t/// Gets a paged list of ${entity.name} entities with custom ordering.`);
			methods.push('\t/// </summary>');
			methods.push('\t/// <param name="pageNumber">The page number.</param>');
			methods.push('\t/// <param name="pageSize">The page size.</param>');
			methods.push('\t/// <param name="filter">Optional filter expression.</param>');
			methods.push('\t/// <param name="cancellationToken">The cancellation token.</param>');
			methods.push(`\t/// <returns>A task that represents the asynchronous operation. The task result contains a paged result of ${entity.name} entities.</returns>`);
		}
		if (options.includePerformanceMarkers) {
			methods.push('\t[Description("EF Core Query - Optimized pagination")]');
		}

		methods.push(`\tTask<PagedResult<Entity>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<Entity, bool>>? filter = null, CancellationToken cancellationToken = default);`);
	}

	return methods;
}

// Main generation function
function generateRepositoryInterfaces(metadataFilePath?: string, outputDir?: string, namespace?: string): void {
	console.log('🚀 Starting repository interface generation...');

	// Handle auto-detection
	let actualMetadataFile: string;
	if (!metadataFilePath || metadataFilePath.trim() === '') {
		actualMetadataFile = findDomainModelMetadata();
		console.log('🔍 Auto-detected metadata file');
	} else {
		actualMetadataFile = metadataFilePath;
		if (!existsSync(actualMetadataFile)) {
			throw new Error(`Metadata file not found: ${actualMetadataFile}`);
		}
	}

	// Read and parse metadata using utility function
	const metadata: DomainModelMetadata = readJsonFile<DomainModelMetadata>(actualMetadataFile);

	console.log(`📋 Found ${metadata.entities.length} entities to analyze`);

	// Determine output directory and namespace
	let finalOutputDir: string;
	let finalNamespace: string;

	if (outputDir && namespace) {
		finalOutputDir = outputDir;
		finalNamespace = namespace;
		console.log('📝 Using provided paths:');
	} else {
		const repoParams = constructRepositoryPath();
		finalOutputDir = repoParams.outputDir;
		finalNamespace = repoParams.namespace;
		console.log('🔍 Using auto-detected paths:');
	}

	console.log(`   📁 Output: ${finalOutputDir}`);
	console.log(`   📦 Namespace: ${finalNamespace}`);

	// Create output directory if it doesn't exist using utility
	ensureDirectoryExists(finalOutputDir);

	// Ensure Common subdirectory exists
	const commonDir = join(finalOutputDir, 'Common');
	ensureDirectoryExists(commonDir);

	const options: GenerationOptions = {
		namespace: finalNamespace,
		outputDirectory: finalOutputDir,
		generateComments: true,
		includePerformanceMarkers: true
	};

	let generatedCount = 0;

	// Generate base repository interface (check if it needs to be updated)
	const baseInterfacePath = join(commonDir, 'IRepository.cs');
	const pagedResultPath = join(commonDir, 'PagedResult.cs');

	if (!existsSync(baseInterfacePath)) {
		console.log('🔧 Generating base IRepository<T> interface...');

		const baseInterfaceContent = generateBaseRepositoryInterface(options);

		try {
			writeFileSync(baseInterfacePath, baseInterfaceContent, 'utf-8');
			console.log('✅ Generated: IRepository.cs');
			generatedCount++;
		} catch (error) {
			console.error('❌ Failed to generate IRepository.cs:', error);
		}
	} else {
		console.log('⏭️  Base IRepository<T> interface already exists, skipping...');
	}

	// Generate PagedResult class if it doesn't exist
	if (!existsSync(pagedResultPath)) {
		console.log('🔧 Generating PagedResult<T> class...');

		const pagedResultContent = generatePagedResultClass(options);

		try {
			writeFileSync(pagedResultPath, pagedResultContent, 'utf-8');
			console.log('✅ Generated: PagedResult.cs');
			generatedCount++;
		} catch (error) {
			console.error('❌ Failed to generate PagedResult.cs:', error);
		}
	} else {
		console.log('⏭️  PagedResult<T> class already exists, skipping...');
	}

	// Generate entity-specific repository interfaces
	for (const entity of metadata.entities) {
		// Only generate interfaces for entity types
		if (entity.type !== 'entity') {
			console.log(`⏭️  Skipping ${entity.name} (type: ${entity.type})`);
			continue;
		}

		console.log(`🔧 Generating I${entity.name}Repository...`);

		// Generate interface content
		const interfaceContent = generateEntityRepositoryInterface(entity, metadata, options);

		// Write to file
		const fileName = `I${entity.name}Repository.cs`;
		const filePath = join(finalOutputDir, fileName);

		try {
			writeFileSync(filePath, interfaceContent, 'utf-8');
			console.log(`✅ Generated: ${fileName}`);
			generatedCount++;
		} catch (error) {
			console.error(`❌ Failed to generate ${fileName}:`, error);
		}
	}

	console.log(`🎉 Repository interface generation complete! Generated ${generatedCount} interface files.`);

	// Format generated code using dotnet format
	formatGeneratedCode(finalOutputDir);
}

// Command line interface
function main(): void {
	const args = process.argv.slice(2);

	// Default to auto-detection if no metadata file provided or empty string
	let metadataFile: string | undefined;
	if (args.length === 0 || !args[0] || args[0].trim() === '') {
		try {
			metadataFile = findDomainModelMetadata();
			console.log('🔍 Auto-detected metadata file, proceeding with generation...');
		} catch (error) {
			console.log('Usage: bun run generate-repository-interfaces.ts [metadata-file] [output-directory] [namespace]');
			console.log('');
			console.log('Arguments:');
			console.log('  metadata-file    Path to the JSON metadata file from domain-model-parser (auto-detected if not provided)');
			console.log('  output-directory Optional: Path to the directory where repository interfaces will be generated (auto-detected if not provided)');
			console.log('  namespace        Optional: C# namespace for the repository interfaces (auto-detected if not provided)');
			console.log('');
			console.log('Examples:');
			console.log('  bun run generate-repository-interfaces.ts                                               # Auto-detect all');
			console.log('  bun run generate-repository-interfaces.ts ./domain-metadata.json');
			console.log('  bun run generate-repository-interfaces.ts ./domain-metadata.json ./custom/path Custom.Namespace');
			console.error('');
			console.error(error);
			process.exit(1);
		}
	} else {
		metadataFile = args[0];
	}

	// Auto-detect project structure if not provided
	let outputDir: string | undefined;
	let namespace: string | undefined;

	if (args.length >= 2 && args[1]) {
		// Manual override
		outputDir = args[1];
		namespace = args[2];
		console.log('📝 Using provided paths:');
	} else {
		// Auto-detect
		console.log('🔍 Using auto-detected paths:');
	}

	console.log(`   📄 Metadata: ${metadataFile}`);
	if (outputDir) console.log(`   📁 Output: ${outputDir}`);
	if (namespace) console.log(`   📦 Namespace: ${namespace}`);

	try {
		generateRepositoryInterfaces(metadataFile, outputDir, namespace);
	} catch (error) {
		console.error('❌ Generation failed:', error);
		process.exit(1);
	}
}

// Generate PagedResult class
function generatePagedResultClass(options: GenerationOptions): string {
	const lines: string[] = [];

	// Using statements
	lines.push('using System;');
	lines.push('using System.Collections.Generic;');
	lines.push('');

	// Namespace
	lines.push(`namespace ${options.namespace}.Common;`);
	lines.push('');

	// Class documentation
	if (options.generateComments) {
		lines.push('/// <summary>');
		lines.push('/// Represents a paged result set with entities and pagination metadata.');
		lines.push('/// </summary>');
		lines.push('/// <typeparam name="T">Entity type</typeparam>');
	}

	// Class declaration
	lines.push('public class PagedResult<T>');
	lines.push('{');

	// Properties with documentation
	const properties = [
		{ name: 'Items', type: 'IList<T>', init: 'new List<T>()', comment: 'The entities in the current page' },
		{ name: 'TotalCount', type: 'int', init: '', comment: 'Total number of entities available (across all pages)' },
		{ name: 'PageNumber', type: 'int', init: '', comment: 'Current page number (1-based)' },
		{ name: 'PageSize', type: 'int', init: '', comment: 'Number of items per page' }
	];

	for (const prop of properties) {
		lines.push('');
		if (options.generateComments) {
			lines.push('\t/// <summary>');
			lines.push(`\t/// ${prop.comment}`);
			lines.push('\t/// </summary>');
		}
		const initializer = prop.init ? ` = ${prop.init}` : '';
		lines.push(`\tpublic ${prop.type} ${prop.name} { get; set; }${initializer};`);
	}

	// Computed properties
	lines.push('');
	if (options.generateComments) {
		lines.push('\t/// <summary>');
		lines.push('\t/// Total number of pages available.');
		lines.push('\t/// </summary>');
	}
	lines.push('\tpublic int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;');

	lines.push('');
	if (options.generateComments) {
		lines.push('\t/// <summary>');
		lines.push('\t/// Whether there is a previous page available.');
		lines.push('\t/// </summary>');
	}
	lines.push('\tpublic bool HasPreviousPage => PageNumber > 1;');

	lines.push('');
	if (options.generateComments) {
		lines.push('\t/// <summary>');
		lines.push('\t/// Whether there is a next page available.');
		lines.push('\t/// </summary>');
	}
	lines.push('\tpublic bool HasNextPage => PageNumber < TotalPages;');

	lines.push('}');

	return lines.join('\n');
}

// Run if called directly
// @ts-ignore
if (import.meta.main) {
	main();
}

// Note: No exports needed - skills communicate through Copilot, not direct imports
