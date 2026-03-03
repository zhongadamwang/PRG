// @ts-ignore
import { existsSync, writeFileSync } from 'node:fs';
// @ts-ignore
import { join } from 'node:path';

// Import shared utilities
import {
	constructDataPath,
	ensureDirectoryExists,
	findDomainModelMetadata,
	formatGeneratedCode,
	readJsonFile,
	toPascalCase
} from '../../../utilities/project-utilities/scripts/utilities';

// @ts-ignore
const process = globalThis.process;

interface Entity {
	id: string;
	name: string;
	type: 'entity' | 'actor' | 'enum' | 'system';
	description?: string;
	attributes: any[];
	methods: any[];
	category?: string;
}

interface DomainModelMetadata {
	version: string;
	generatedAt: string;
	sourceFile: string;
	entities: Entity[];
	relationships: any[];
	enums: any[];
	statistics: any;
}

interface DbContextOptions {
	namespace: string;
	outputDirectory: string;
	className: string;
	generateComments: boolean;
}



// Generate DbContext class
function generateDbContext(
	entities: Entity[],
	options: DbContextOptions
): string {
	const entityClasses = entities.filter(e => e.type === 'entity');
	const className = options.className;

	let content = '';

	// Using statements
	content += 'using Microsoft.EntityFrameworkCore;\n';
	content += `using ${options.namespace.replace('.Data', '.Entities')};\n`;
	if (entityClasses.length > 0) {
		content += `using ${options.namespace.replace('.Data', '.Configuration')};\n`;
	}
	content += '\n';

	// Namespace
	content += `namespace ${options.namespace};\n\n`;

	// Class documentation
	if (options.generateComments) {
		content += '/// <summary>\n';
		content += '/// EF Core DbContext for request management application with complete entity framework setup\n';
		content += '/// </summary>\n';
	}

	// Class declaration
	content += `public class ${className} : DbContext\n{\n`;

	// Constructor
	content += `\t/// <summary>\n`;
	content += `\t/// Initializes a new instance of the ${className} class\n`;
	content += `\t/// </summary>\n`;
	content += `\t/// <param name=\"options\">The DbContext options</param>\n`;
	content += `\tpublic ${className}(DbContextOptions<${className}> options)\n`;
	content += `\t\t: base(options)\n`;
	content += `\t{\n`;
	content += `\t}\n\n`;

	// DbSet properties for entities
	if (entityClasses.length > 0) {
		content += '\t// Entity DbSets\n\n';

		for (const entity of entityClasses) {
			const entityName = toPascalCase(entity.name);
			const pluralName = getPluralName(entityName);

			if (options.generateComments && entity.description) {
				content += `\t/// <summary>\n`;
				content += `\t/// Gets or sets the ${entityName} entities\n`;
				content += `\t/// ${entity.description}\n`;
				content += `\t/// </summary>\n`;
			} else if (options.generateComments) {
				content += `\t/// <summary>\n`;
				content += `\t/// Gets or sets the ${entityName} entities\n`;
				content += `\t/// </summary>\n`;
			}

			content += `\tpublic DbSet<${entityName}> ${pluralName} { get; set; }\n\n`;
		}
	}

	// OnModelCreating method
	content += '\t/// <summary>\n';
	content += '\t/// Configures the entity model using Fluent API configurations\n';
	content += '\t/// </summary>\n';
	content += '\t/// <param name=\"modelBuilder\">The model builder</param>\n';
	content += '\tprotected override void OnModelCreating(ModelBuilder modelBuilder)\n';
	content += '\t{\n';
	content += '\t\tbase.OnModelCreating(modelBuilder);\n\n';

	if (entityClasses.length > 0) {
		content += '\t\t// Apply all entity configurations from the Configuration assembly\n';
		content += '\t\tmodelBuilder.ApplyConfigurationsFromAssembly(typeof(' + className + ').Assembly);\n';
	} else {
		content += '\t\t// No entity configurations to apply yet\n';
		content += '\t\t// Configurations will be applied automatically when entity classes are added\n';
	}

	content += '\t}\n';

	// Additional helper methods (optional)
	content += '\n';
	content += '\t/// <summary>\n';
	content += '\t/// Configures the database connection and options\n';
	content += '\t/// </summary>\n';
	content += '\t/// <param name=\"optionsBuilder\">The options builder</param>\n';
	content += '\tprotected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)\n';
	content += '\t{\n';
	content += '\t\tbase.OnConfiguring(optionsBuilder);\n';
	content += '\t\t\n';
	content += '\t\t// Additional configuration can be added here if needed\n';
	content += '\t\t// Note: Connection string should be configured in dependency injection\n';
	content += '\t}\n';

	content += '}\n';

	return content;
}

// Simple pluralization (English rules)
function getPluralName(entityName: string): string {
	// Handle special cases
	const specialCases: Record<string, string> = {
		'Request': 'Requests',
		'DataElement': 'DataElements',
		'StateDiagram': 'StateDiagrams',
		'ReviewPackage': 'ReviewPackages',
		'Notification': 'Notifications'
	};

	if (specialCases[entityName]) {
		return specialCases[entityName];
	}

	// Basic pluralization rules
	if (entityName.endsWith('s') || entityName.endsWith('x') || entityName.endsWith('ch') || entityName.endsWith('sh')) {
		return entityName + 'es';
	} else if (entityName.endsWith('y') && !['a', 'e', 'i', 'o', 'u'].includes(entityName[entityName.length - 2])) {
		return entityName.substring(0, entityName.length - 1) + 'ies';
	} else {
		return entityName + 's';
	}
}

// Main generation function
function generateDataContext(metadataFilePath: string, outputDir?: string, namespace?: string, className?: string): void {
	console.log('🚀 Starting DbContext generation...');

	// Validate input file
	if (!existsSync(metadataFilePath)) {
		throw new Error(`Metadata file not found: ${metadataFilePath}`);
	}

	// Read metadata
	const metadata: DomainModelMetadata = readJsonFile(metadataFilePath);
	console.log(`📋 Found ${metadata.entities.length} entities in metadata`);

	// Auto-detect project structure if not provided
	if (!outputDir || !namespace) {
		const detected = constructDataPath();
		outputDir = outputDir || detected.outputDir;
		namespace = namespace || detected.namespace;
		className = className || `${detected.projectName.split('.').pop()}DbContext`;
	}

	console.log('🔍 Using paths:');
	console.log(`   📁 Output: ${outputDir}`);
	console.log(`   📦 Namespace: ${namespace}`);
	console.log(`   🏷️  Class: ${className}`);

	// Ensure output directory exists
	ensureDirectoryExists(outputDir);

	// Filter entities
	const entityClasses = metadata.entities.filter(e => e.type === 'entity');
	console.log(`📊 Found ${entityClasses.length} entity classes to include in DbContext`);

	if (entityClasses.length === 0) {
		console.log('⚠️  No entity classes found. Creating empty DbContext for future use.');
	}

	// Generate DbContext
	const options: DbContextOptions = {
		namespace: namespace!,
		outputDirectory: outputDir!,
		className: className!,
		generateComments: true
	};

	const dbContextContent = generateDbContext(metadata.entities, options);

	// Write DbContext file
	const outputFile = join(outputDir, `${className}.cs`);
	writeFileSync(outputFile, dbContextContent);

	console.log(`✅ Generated: ${className}.cs`);
	console.log(`🎉 DbContext generation complete!`);

	// Format generated code using unified utility
	formatGeneratedCode(outputDir);
}

// Parse command line arguments
function parseArguments(): {
	metadataFile: string;
	outputDir?: string;
	namespace?: string;
	className?: string;
} {
	const args = process.argv.slice(2);

	if (args.length === 0) {
		// Auto-detect metadata file
		console.log('🔍 No metadata file specified, attempting auto-detection...');
		try {
			const metadataFile = findDomainModelMetadata();
			return { metadataFile };
		} catch (error) {
			console.error('❌ Could not auto-detect metadata file.');
			console.error('');
			console.error('Usage:');
			console.error('  bun run generate-data-context.ts <metadata-file> [output-directory] [namespace] [class-name]');
			console.error('');
			console.error('Arguments:');
			console.error('  metadata-file    Path to the JSON metadata file from domain-model-parser');
			console.error('  output-directory Optional: Path to the directory where DbContext will be generated');
			console.error('  namespace        Optional: C# namespace for the DbContext class');
			console.error('  class-name       Optional: Name for the DbContext class');
			console.error('');
			console.error('Examples:');
			console.error('  bun run generate-data-context.ts ./domain-metadata.json');
			console.error('  bun run generate-data-context.ts ./domain-metadata.json ./Data MyProject.Data MyProjectDbContext');
			process.exit(1);
		}
	}

	return {
		metadataFile: args[0],
		outputDir: args[1],
		namespace: args[2],
		className: args[3]
	};
}

// Main function
function main(): void {
	console.log('🏗️ Starting DbContext generation...');

	const { metadataFile, outputDir, namespace, className } = parseArguments();

	console.log('🔍 Using paths:');
	console.log(`   📄 Metadata: ${metadataFile}`);
	if (outputDir) console.log(`   📁 Output: ${outputDir}`);
	if (namespace) console.log(`   📦 Namespace: ${namespace}`);
	if (className) console.log(`   🏷️  Class: ${className}`);

	try {
		generateDataContext(metadataFile, outputDir, namespace, className);
	} catch (error) {
		console.error('❌ DbContext generation failed:', error);
		process.exit(1);
	}
}

// Run if called directly
// @ts-ignore
if (import.meta.main) {
	main();
}

// Note: No exports needed - skills communicate through Copilot, not direct imports