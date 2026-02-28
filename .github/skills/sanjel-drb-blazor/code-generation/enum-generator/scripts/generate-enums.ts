// @ts-ignore
import { existsSync, mkdirSync, readFileSync, writeFileSync } from 'node:fs';
// @ts-ignore  
import { join } from 'node:path';
// @ts-ignore
import { execSync } from 'node:child_process';

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

interface EnumGenerationOptions {
	namespace: string;
	outputDirectory: string;
	generateComments: boolean;
	includeToString: boolean;
}

// Convert snake_case to proper PascalCase for compound words
function toPascalCase(snakeCase: string): string {
	return snakeCase
		.split('_')
		.map(word => word.charAt(0).toUpperCase() + word.slice(1).toLowerCase())
		.join('');
}

// Enhanced enum name conversion
function convertEnumName(enumId: string): string {
	// Remove 'enum' suffix and convert to PascalCase
	let baseName = enumId.replace(/enum$/i, '');
	
	// Handle special compound words
	const compoundWords: Record<string, string> = {
		'notificationtype': 'NotificationType',
		'elementtype': 'ElementType',
		'diagramtype': 'DiagramType',
		'reviewstatus': 'ReviewStatus'
	};
	
	if (compoundWords[baseName.toLowerCase()]) {
		return compoundWords[baseName.toLowerCase()] + 'Enum';
	}
	
	// Standard PascalCase conversion
	return toPascalCase(baseName) + 'Enum';
}

// Detect project path and generate appropriate enum directory
function detectProjectEnumPath(): { outputDir: string; namespace: string } {
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
	
	// Construct enum directory path (same as entities for now)
	const outputDir = join(projectRoot, `src/${projectName}.Core/Entities`);
	const namespace = `${projectName}.Core.Entities`;
	
	console.log(`📁 Target directory: ${outputDir}`);
	console.log(`📦 Target namespace: ${namespace}`);
	
	return { outputDir, namespace };
}

// Generate standard enum values based on common patterns
function generateEnumValues(enumEntity: Entity): { name: string; value: number; description?: string }[] {
	const enumName = enumEntity.name;
	
	// Standard enum value patterns based on enum type
	const standardEnums: Record<string, { name: string; value: number; description?: string }[]> = {
		'StatusEnum': [
			{ name: 'Draft', value: 0, description: 'Request is in draft state' },
			{ name: 'Submitted', value: 1, description: 'Request has been submitted' },
			{ name: 'InProgress', value: 2, description: 'Request is being processed' },
			{ name: 'UnderReview', value: 3, description: 'Request is under review' },
			{ name: 'Approved', value: 4, description: 'Request has been approved' },
			{ name: 'Rejected', value: 5, description: 'Request has been rejected' },
			{ name: 'Completed', value: 6, description: 'Request has been completed' },
			{ name: 'Cancelled', value: 7, description: 'Request has been cancelled' }
		],
		'PriorityEnum': [
			{ name: 'Low', value: 0, description: 'Low priority request' },
			{ name: 'Normal', value: 1, description: 'Normal priority request' },
			{ name: 'High', value: 2, description: 'High priority request' },
			{ name: 'Critical', value: 3, description: 'Critical priority request' }
		],
		'NotificationTypeEnum': [
			{ name: 'Email', value: 0, description: 'Email notification' },
			{ name: 'SMS', value: 1, description: 'SMS notification' },
			{ name: 'InApp', value: 2, description: 'In-application notification' },
			{ name: 'System', value: 3, description: 'System notification' }
		],
		'DeliveryEnum': [
			{ name: 'Pending', value: 0, description: 'Notification pending delivery' },
			{ name: 'Sent', value: 1, description: 'Notification sent successfully' },
			{ name: 'Delivered', value: 2, description: 'Notification delivered' },
			{ name: 'Failed', value: 3, description: 'Notification delivery failed' }
		],
		'RecipientEnum': [
			{ name: 'Engineer', value: 0, description: 'Engineering staff member' },
			{ name: 'Manager', value: 1, description: 'Management staff member' },
			{ name: 'Client', value: 2, description: 'External client' },
			{ name: 'System', value: 3, description: 'System notification' }
		],
		'ReviewStatusEnum': [
			{ name: 'NotStarted', value: 0, description: 'Review not started' },
			{ name: 'InProgress', value: 1, description: 'Review in progress' },
			{ name: 'Completed', value: 2, description: 'Review completed' },
			{ name: 'Approved', value: 3, description: 'Review approved' },
			{ name: 'Rejected', value: 4, description: 'Review rejected' }
		],
		'ElementTypeEnum': [
			{ name: 'Text', value: 0, description: 'Text input element' },
			{ name: 'Number', value: 1, description: 'Numeric input element' },
			{ name: 'Date', value: 2, description: 'Date input element' },
			{ name: 'Selection', value: 3, description: 'Selection input element' }
		],
		'DiagramTypeEnum': [
			{ name: 'ProcessFlow', value: 0, description: 'Process flow diagram' },
			{ name: 'PipingInstrumentation', value: 1, description: 'P&ID diagram' },
			{ name: 'Schematic', value: 2, description: 'Schematic diagram' },
			{ name: 'Layout', value: 3, description: 'Layout diagram' }
		],
		'ValidationEnum': [
			{ name: 'NotValidated', value: 0, description: 'Not validated' },
			{ name: 'Valid', value: 1, description: 'Validation passed' },
			{ name: 'Invalid', value: 2, description: 'Validation failed' },
			{ name: 'Pending', value: 3, description: 'Validation pending' }
		],
		'AvailabilityEnum': [
			{ name: 'Available', value: 0, description: 'Available for work' },
			{ name: 'Busy', value: 1, description: 'Currently busy' },
			{ name: 'Away', value: 2, description: 'Away from office' },
			{ name: 'Unavailable', value: 3, description: 'Unavailable' }
		]
	};
	
	return standardEnums[enumName] || [
		{ name: 'Unknown', value: 0, description: 'Unknown value' }
	];
}

// Generate enum class content
function generateEnumClass(enumEntity: Entity, options: EnumGenerationOptions): string {
	const lines: string[] = [];
	const enumName = convertEnumName(enumEntity.id);
	const enumValues = generateEnumValues({ ...enumEntity, name: enumName });
	
	// Using statements
	lines.push('using System;');
	lines.push('');
	
	// Namespace
	lines.push(`namespace ${options.namespace};`);
	lines.push('');
	
	// Class documentation
	if (options.generateComments) {
		lines.push('/// <summary>');
		lines.push(`/// ${enumEntity.description || `${enumName} enumeration`}`);
		lines.push('/// </summary>');
	}
	
	// Enum declaration
	lines.push(`public enum ${enumName}`);
	lines.push('{');
	
	// Enum values
	for (let i = 0; i < enumValues.length; i++) {
		const enumValue = enumValues[i];
		
		if (options.generateComments && enumValue.description) {
			lines.push(`    /// <summary>`);
			lines.push(`    /// ${enumValue.description}`);
			lines.push(`    /// </summary>`);
		}
		
		const isLast = i === enumValues.length - 1;
		const suffix = isLast ? '' : ',';
		lines.push(`    ${enumValue.name} = ${enumValue.value}${suffix}`);
		
		if (!isLast) {
			lines.push('');
		}
	}
	
	lines.push('}');
	
	return lines.join('\n');
}

// Main generation function
function generateEnums(metadataFilePath: string, outputDir?: string, namespace?: string): void {
	console.log('🚀 Starting enum generation...');
	
	// Validate input file
	if (!existsSync(metadataFilePath)) {
		throw new Error(`Metadata file not found: ${metadataFilePath}`);
	}
	
	// Read and parse metadata
	const metadataContent = readFileSync(metadataFilePath, 'utf-8');
	const metadata: DomainModelMetadata = JSON.parse(metadataContent);
	
	// Filter enum entities
	const enumEntities = metadata.entities.filter(entity => entity.type === 'enum');
	console.log(`📋 Found ${enumEntities.length} enums to generate`);
	
	if (enumEntities.length === 0) {
		console.log('⚠️  No enum entities found in metadata. Nothing to generate.');
		return;
	}
	
	// Determine output directory and namespace
	let finalOutputDir: string;
	let finalNamespace: string;
	
	if (outputDir && namespace) {
		finalOutputDir = outputDir;
		finalNamespace = namespace;
		console.log('📝 Using provided paths:');
	} else {
		const detected = detectProjectEnumPath();
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
	
	const options: EnumGenerationOptions = {
		namespace: finalNamespace,
		outputDirectory: finalOutputDir,
		generateComments: true,
		includeToString: true
	};
	
	let generatedCount = 0;
	
	// Generate enum classes
	for (const enumEntity of enumEntities) {
		const enumName = convertEnumName(enumEntity.id);
		console.log(`🔧 Generating ${enumName}...`);
		
		// Generate enum content
		const enumContent = generateEnumClass(enumEntity, options);
		
		// Write to file
		const fileName = `${enumName}.cs`;
		const filePath = join(finalOutputDir, fileName);
		
		try {
			writeFileSync(filePath, enumContent, 'utf-8');
			console.log(`✅ Generated: ${fileName}`);
			generatedCount++;
		} catch (error) {
			console.error(`❌ Failed to generate ${fileName}:`, error);
		}
	}
	
	console.log(`🎉 Enum generation complete! Generated ${generatedCount} enum classes.`);
}

// Command line interface
function main(): void {
	const args = process.argv.slice(2);

	if (args.length < 1) {
		console.log('Usage: bun run generate-enums.ts <metadata-file> [output-directory] [namespace]');
		console.log('');
		console.log('Arguments:');
		console.log('  metadata-file    Path to the JSON metadata file from domain-model-parser');
		console.log('  output-directory Optional: Path to the directory where enum classes will be generated (auto-detected if not provided)');
		console.log('  namespace        Optional: C# namespace for the enum classes (auto-detected if not provided)');
		console.log('');
		console.log('Examples:');
		console.log('  bun run generate-enums.ts ./domain-metadata.json');
		console.log('  bun run generate-enums.ts ./domain-metadata.json ./custom/path Custom.Namespace');
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
		generateEnums(metadataFile, outputDir, namespace);
	} catch (error) {
		console.error('❌ Enum generation failed:', error);
		process.exit(1);
	}
}

// Run if called directly
// @ts-ignore
if (import.meta.main) {
	main();
}

// Export for use by other skills
export { convertEnumName, detectProjectEnumPath, generateEnums, toPascalCase };
