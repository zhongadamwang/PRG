// @ts-ignore
import { existsSync, mkdirSync, readFileSync, writeFileSync } from 'node:fs';
// @ts-ignore  
import { join } from 'node:path';
// @ts-ignore
import { execSync } from 'node:child_process';

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

interface ConfigurationOptions {
	namespace: string;
	outputDirectory: string;
	generateComments: boolean;
	includeIndexes: boolean;
	includeRelationships: boolean;
}

// Convert snake_case to PascalCase for C# properties
function toPascalCase(snakeCase: string): string {
	return snakeCase
		.split('_')
		.map(word => word.charAt(0).toUpperCase() + word.slice(1).toLowerCase())
		.join('');
}

// Detect project path and generate appropriate configuration directory
function detectProjectConfigurationPath(): { outputDir: string; namespace: string } {
	// @ts-ignore
	let currentDir = process.cwd();
	console.log(`🔍 Detecting project structure from: ${currentDir}`);

	// Navigate up to find project root (where src/.slnx files are located)
	let projectRoot = currentDir;
	let foundSlnx = false;
	let slnxFile = '';

	while (projectRoot !== '/' && !foundSlnx) {
		try {
			// Check src subdirectory for .slnx files (fixed location)
			const srcDir = join(projectRoot, 'src');
			if (existsSync(srcDir)) {
				const slnxFiles = (execSync(`find "${srcDir}" -maxdepth 1 -name "*.slnx" -type f 2>/dev/null || true`, { encoding: 'utf-8' }) as string)
					.split('\n')
					.filter(line => line.trim())
					.map(line => line.trim());

				if (slnxFiles.length > 0) {
					slnxFile = slnxFiles[0];
					foundSlnx = true;
					break;
				}
			}

			// Move up one directory
			const parentDir = join(projectRoot, '..');
			if (parentDir === projectRoot) break; // Reached filesystem root
			projectRoot = parentDir;
		} catch (error) {
			// Continue searching up
			const parentDir = join(projectRoot, '..');
			if (parentDir === projectRoot) break;
			projectRoot = parentDir;
		}
	}

	if (!foundSlnx) {
		throw new Error('No .slnx file found in src directory. Unable to detect project structure. Please run from project root or provide paths manually.');
	}

	// Extract project name from .slnx file
	const projectName = slnxFile.split('/').pop()?.replace('.slnx', '') || 'Unknown';

	console.log(`📦 Detected project: ${projectName}`);
	console.log(`📂 Project root: ${projectRoot}`);
	console.log(`🎯 Found .slnx: ${slnxFile}`);

	// Construct configuration directory path (always in src subdirectory)
	const outputDir = join(projectRoot, `src/${projectName}.Core/Configuration`);
	const namespace = `${projectName}.Core.Configuration`;

	console.log(`📁 Target directory: ${outputDir}`);
	console.log(`📦 Target namespace: ${namespace}`);

	return { outputDir, namespace };
}

// Map domain types to EF Core configuration methods
function mapToEFCoreType(domainType: string): string {
	switch (domainType.toLowerCase()) {
		case 'string':
		case 'text':
			return 'HasMaxLength(255)'; // Default string length
		case 'int':
		case 'integer':
		case 'number':
			return 'HasColumnType("int")';
		case 'long':
			return 'HasColumnType("bigint")';
		case 'float':
		case 'decimal':
			return 'HasColumnType("decimal(18,2)")';
		case 'double':
			return 'HasColumnType("double")';
		case 'bool':
		case 'boolean':
			return 'HasColumnType("bit")';
		case 'datetime':
		case 'date':
			return 'HasColumnType("datetime2")';
		case 'guid':
		case 'uuid':
			return 'HasColumnType("uniqueidentifier")';
		default:
			return ''; // For custom types or navigation properties
	}
}

// Generate property configuration
function generatePropertyConfiguration(attribute: EntityAttribute): string[] {
	const configs: string[] = [];
	const propertyName = toPascalCase(attribute.name);

	// Basic property configuration
	configs.push(`builder.Property(e => e.${propertyName})`);

	// Column name mapping
	configs.push(`    .HasColumnName("${attribute.name}")`);

	// Type configuration
	const efCoreType = mapToEFCoreType(attribute.type);
	if (efCoreType) {
		configs.push(`    .${efCoreType}`);
	}

	// Required/Optional configuration
	if (!attribute.isOptional && attribute.type === 'string') {
		configs.push(`    .IsRequired()`);
	} else if (attribute.isOptional && attribute.type !== 'string') {
		configs.push(`    .IsRequired(false)`);
	}

	// Special handling for specific field types (avoid duplicating MaxLength)
	if (attribute.type === 'string' && !efCoreType.includes('HasMaxLength')) {
		if (attribute.name.toLowerCase().includes('email')) {
			configs.push(`    .HasMaxLength(255)`);
		} else if (attribute.name.toLowerCase().includes('description')) {
			configs.push(`    .HasMaxLength(500)`);
		} else if (attribute.name.toLowerCase().includes('name')) {
			configs.push(`    .HasMaxLength(100)`);
		}
	}

	return [configs.join('\n') + ';'];
}

// Generate relationship configurations
function generateRelationshipConfigurations(entity: Entity, relationships: Relationship[], metadata: DomainModelMetadata): string[] {
	const configs: string[] = [];

	// Find relationships involving this entity
	const entityRelationships = relationships.filter(rel =>
		rel.sourceEntity === entity.name || rel.targetEntity === entity.name
	);

	for (const rel of entityRelationships) {
		if (rel.sourceEntity === entity.name) {
			// This entity is the source
			const targetEntity = rel.targetEntity;

			// Find target entity in metadata to check its type
			const targetEntityDef = metadata.entities.find(e => e.name === targetEntity);
			if (!targetEntityDef || targetEntityDef.type === 'actor' || targetEntityDef.type === 'system') {
				// Skip Actor and System types - they are not generated as entity classes
				continue;
			}

			const navigationProperty = targetEntity;

			if (rel.cardinality?.includes('*') || rel.type === 'composition') {
				// One-to-many relationship
				configs.push(`// One-to-many relationship with ${targetEntity}`);
				configs.push(`builder.HasMany(d => d.${navigationProperty})`);
				configs.push(`    .WithOne()`);
				configs.push(`    .HasForeignKey("${entity.name}Id")`);
				configs.push(`    .OnDelete(DeleteBehavior.Cascade);`);
			} else {
				// One-to-one relationship
				configs.push(`// One-to-one relationship with ${targetEntity}`);
				configs.push(`builder.HasOne(d => d.${navigationProperty})`);
				configs.push(`    .WithOne()`);
				configs.push(`    .HasForeignKey<${targetEntity}>("${entity.name}Id");`);
			}
		} else if (rel.targetEntity === entity.name) {
			// This entity is the target - check if source is an Actor/System
			const sourceEntity = rel.sourceEntity;

			// Find source entity in metadata to check its type
			const sourceEntityDef = metadata.entities.find(e => e.name === sourceEntity);
			if (!sourceEntityDef || sourceEntityDef.type === 'actor' || sourceEntityDef.type === 'system') {
				// Skip Actor and System types - but we might still need foreign key properties
				// Only add FK if the property actually exists in the entity
				const fkPropertyName = `${sourceEntity}Id`;
				const hasProperty = entity.attributes.some(attr =>
					toPascalCase(attr.name) === fkPropertyName
				);

				if (hasProperty) {
					configs.push(`// Foreign key reference to ${sourceEntity}`);
					configs.push(`builder.Property(e => e.${fkPropertyName})`);
					configs.push(`    .HasColumnName("${sourceEntity.toLowerCase()}_id");`);
				}
				continue;
			}

			// Add foreign key property reference for entity types
			// Only if the entity actually has the foreign key property
			const fkPropertyName = `${sourceEntity}Id`;
			const hasProperty = entity.attributes.some(attr =>
				toPascalCase(attr.name) === fkPropertyName
			);

			if (hasProperty) {
				configs.push(`// Foreign key reference to ${sourceEntity}`);
				configs.push(`builder.Property(e => e.${fkPropertyName})`);
				configs.push(`    .HasColumnName("${sourceEntity.toLowerCase()}_id");`);
			}
		}
	}

	return configs;
}

// Generate index configurations
function generateIndexConfigurations(entity: Entity): string[] {
	const indexes: string[] = [];

	for (const attr of entity.attributes) {
		if (attr.constraints) {
			const propertyName = toPascalCase(attr.name);

			for (const constraint of attr.constraints) {
				if (constraint.toLowerCase() === 'unique') {
					indexes.push(`// Unique index on ${propertyName}`);
					indexes.push(`builder.HasIndex(e => e.${propertyName})`);
					indexes.push(`    .IsUnique()`);
					indexes.push(`    .HasDatabaseName("IX_${entity.name}_${propertyName}");`);
				}
			}
		}

		// Auto-generate indexes for common patterns
		if (attr.name.toLowerCase().includes('email')) {
			const propertyName = toPascalCase(attr.name);
			indexes.push(`// Index on ${propertyName} for performance`);
			indexes.push(`builder.HasIndex(e => e.${propertyName})`);
			indexes.push(`    .HasDatabaseName("IX_${entity.name}_${propertyName}");`);
		}
	}

	return indexes;
}

// Generate primary key configuration
function generateKeyConfiguration(entity: Entity): string[] {
	const keyConfigs: string[] = [];

	// Find primary key properties
	const keyProperties = entity.attributes.filter(attr =>
		attr.constraints?.some(c => c.toLowerCase() === 'key' || c.toLowerCase() === 'primarykey') ||
		attr.name.toLowerCase() === 'id'
	);

	if (keyProperties.length === 1) {
		const keyProp = toPascalCase(keyProperties[0].name);
		keyConfigs.push(`// Primary key configuration`);
		keyConfigs.push(`builder.HasKey(e => e.${keyProp});`);
	} else if (keyProperties.length > 1) {
		// Composite key
		const keyProps = keyProperties.map(attr => `e.${toPascalCase(attr.name)}`).join(', ');
		keyConfigs.push(`// Composite primary key configuration`);
		keyConfigs.push(`builder.HasKey(e => new { ${keyProps} });`);
	}

	return keyConfigs;
}

// Generate complete entity configuration class
function generateEntityConfiguration(entity: Entity, metadata: DomainModelMetadata, options: ConfigurationOptions): string {
	const lines: string[] = [];

	// Using statements
	lines.push('using Microsoft.EntityFrameworkCore;');
	lines.push('using Microsoft.EntityFrameworkCore.Metadata.Builders;');
	lines.push(`using ${options.namespace.replace('.Configuration', '.Entities')};`);
	lines.push('');

	// Namespace
	lines.push(`namespace ${options.namespace};`);
	lines.push('');

	// Class documentation
	if (options.generateComments && entity.description) {
		lines.push('/// <summary>');
		lines.push(`/// Configuration for ${entity.name} entity`);
		lines.push(`/// ${entity.description}`);
		lines.push('/// </summary>');
	}

	// Class declaration
	lines.push(`public class ${entity.name}Configuration : IEntityTypeConfiguration<${entity.name}>`);
	lines.push('{');

	// Configure method
	lines.push(`    public void Configure(EntityTypeBuilder<${entity.name}> builder)`);
	lines.push('    {');

	// Table configuration
	lines.push(`        // Table configuration`);
	lines.push(`        builder.ToTable("${entity.name.toLowerCase()}s");`);
	lines.push('');

	// Primary key configuration
	const keyConfigs = generateKeyConfiguration(entity);
	if (keyConfigs.length > 0) {
		for (const config of keyConfigs) {
			lines.push(`        ${config}`);
		}
		lines.push('');
	}

	// Property configurations
	lines.push('        // Property configurations');
	for (const attr of entity.attributes) {
		if (!attr.isArray) { // Skip navigation properties for now
			const propConfigs = generatePropertyConfiguration(attr);
			for (const config of propConfigs) {
				lines.push(`        ${config}`);
			}
			lines.push('');
		}
	}

	// Index configurations
	if (options.includeIndexes) {
		const indexConfigs = generateIndexConfigurations(entity);
		if (indexConfigs.length > 0) {
			lines.push('        // Index configurations');
			for (const config of indexConfigs) {
				lines.push(`        ${config}`);
			}
			lines.push('');
		}
	}

	// Relationship configurations
	if (options.includeRelationships) {
		const relationshipConfigs = generateRelationshipConfigurations(entity, metadata.relationships, metadata);
		if (relationshipConfigs.length > 0) {
			lines.push('        // Relationship configurations');
			for (const config of relationshipConfigs) {
				lines.push(`        ${config}`);
			}
			lines.push('');
		}
	}

	lines.push('    }');
	lines.push('}');

	return lines.join('\n');
}

// Main generation function
function generateEntityConfigurations(metadataFilePath: string, outputDir?: string, namespace?: string): void {
	console.log('🚀 Starting entity configuration generation...');

	// Validate input file
	if (!existsSync(metadataFilePath)) {
		throw new Error(`Metadata file not found: ${metadataFilePath}`);
	}

	// Read and parse metadata
	const metadataContent = readFileSync(metadataFilePath, 'utf-8');
	const metadata: DomainModelMetadata = JSON.parse(metadataContent);

	console.log(`📋 Found ${metadata.entities.length} entities to configure`);

	// Determine output directory and namespace
	let finalOutputDir: string;
	let finalNamespace: string;

	if (outputDir && namespace) {
		finalOutputDir = outputDir;
		finalNamespace = namespace;
		console.log('📝 Using provided paths:');
	} else {
		const detected = detectProjectConfigurationPath();
		finalOutputDir = detected.outputDir;
		finalNamespace = detected.namespace;
		console.log('🔍 Using auto-detected paths:');
	}

	console.log(`   📁 Output: ${finalOutputDir}`);
	console.log(`   📦 Namespace: ${finalNamespace}`);

	// Create output directory if it doesn't exist
	if (!existsSync(finalOutputDir)) {
		mkdirSync(finalOutputDir, { recursive: true });
		console.log(`📁 Created output directory: ${finalOutputDir}`);
	}

	const options: ConfigurationOptions = {
		namespace: finalNamespace,
		outputDirectory: finalOutputDir,
		generateComments: true,
		includeIndexes: true,
		includeRelationships: true
	};

	let generatedCount = 0;

	// Generate configuration classes
	for (const entity of metadata.entities) {
		// Only generate for actual entities
		if (entity.type !== 'entity') {
			console.log(`⏭️  Skipping ${entity.name} (type: ${entity.type})`);
			continue;
		}

		console.log(`🔧 Generating ${entity.name}Configuration...`);

		// Generate configuration content
		const configContent = generateEntityConfiguration(entity, metadata, options);

		// Write to file
		const fileName = `${entity.name}Configuration.cs`;
		const filePath = join(finalOutputDir, fileName);

		try {
			writeFileSync(filePath, configContent, 'utf-8');
			console.log(`✅ Generated: ${fileName}`);
			generatedCount++;
		} catch (error) {
			console.error(`❌ Failed to generate ${fileName}:`, error);
		}
	}

	console.log(`🎉 Configuration generation complete! Generated ${generatedCount} configuration classes.`);

	// Format generated code using dotnet format
	formatGeneratedCode(finalOutputDir);
}

// Command line interface
function main(): void {
	const args = process.argv.slice(2);

	if (args.length < 1) {
		console.log('Usage: bun run generate-entity-configurations.ts <metadata-file> [output-directory] [namespace]');
		console.log('');
		console.log('Arguments:');
		console.log('  metadata-file    Path to the JSON metadata file from domain-model-parser');
		console.log('  output-directory Optional: Path to the directory where configuration classes will be generated (auto-detected if not provided)');
		console.log('  namespace        Optional: C# namespace for the configuration classes (auto-detected if not provided)');
		console.log('');
		console.log('Examples:');
		console.log('  bun run generate-entity-configurations.ts ./domain-metadata.json');
		console.log('  bun run generate-entity-configurations.ts ./domain-metadata.json ./custom/path Custom.Namespace');
		process.exit(1);
	}

	const metadataFile = args[0];

	// Auto-detect project structure if not provided
	let outputDir: string | undefined;
	let namespace: string | undefined;

	if (args.length >= 2) {
		// Manual override
		outputDir = args[1];
		namespace = args[2];
		console.log('📝 Using provided paths:');
	} else {
		console.log('🔍 Using auto-detected paths:');
	}

	console.log(`   📄 Metadata: ${metadataFile}`);

	try {
		generateEntityConfigurations(metadataFile, outputDir, namespace);
	} catch (error) {
		console.error('❌ Configuration generation failed:', error);
		process.exit(1);
	}
}

// Format generated C# code using dotnet format
function formatGeneratedCode(outputDir: string): void {
	try {
		console.log('🎨 Formatting generated code with dotnet format...');

		// Get the project root (where .slnx file is located)
		let projectRoot = outputDir;
		while (projectRoot && projectRoot !== '/') {
			const files = (execSync(`ls "${projectRoot}"`, { encoding: 'utf-8' }) as string).split('\n');
			const hasSlnx = files.some(file => file.endsWith('.slnx'));
			if (hasSlnx) {
				break;
			}
			// Continue searching up
			const parentDir = join(projectRoot, '..');
			if (parentDir === projectRoot) break;
			projectRoot = parentDir;
		}

		// Run dotnet format on the specific directory
		const formatCommand = `dotnet format "${projectRoot}" --include "${outputDir}/**/*.cs"`;
		execSync(formatCommand, { cwd: projectRoot, encoding: 'utf-8' });

		console.log('✅ Code formatting completed successfully!');
	} catch (error) {
		console.warn('⚠️  Code formatting failed, but generation was successful:', error);
		// Don't fail the generation if formatting fails
	}
}

// Run if called directly
// @ts-ignore
if (import.meta.main) {
	main();
}

// Export for use by other skills
export { detectProjectConfigurationPath, generateEntityConfigurations, toPascalCase };
