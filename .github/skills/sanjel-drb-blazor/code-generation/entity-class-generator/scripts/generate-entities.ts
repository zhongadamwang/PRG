// @ts-ignore
import { existsSync, writeFileSync } from 'node:fs';
// @ts-ignore
import { join } from 'node:path';
// @ts-ignore

// Import shared utilities
import {
	constructEntityPath,
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
	includeNavigationProperties: boolean;
}

// PascalCase conversion - USE UTILITY VERSION
// function toPascalCase() - now imported from utilities

// Enhanced enum name conversion (same as enum-generator)
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

// Project path detection - USE UTILITY VERSION
// function detectProjectEntityPath() - now replaced by constructEntityPath() from utilities

// Type mapping from domain model types to C# types
function mapToCSharpType(domainType: string, isOptional: boolean = false, isArray: boolean = false): string {
	let csharpType: string;

	switch (domainType.toLowerCase()) {
		case 'string':
		case 'text':
			csharpType = 'string';
			break;
		case 'int':
		case 'integer':
		case 'number':
			csharpType = 'int';
			break;
		case 'long':
			csharpType = 'long';
			break;
		case 'float':
		case 'decimal':
			csharpType = 'decimal';
			break;
		case 'double':
			csharpType = 'double';
			break;
		case 'bool':
		case 'boolean':
			csharpType = 'bool';
			break;
		case 'datetime':
		case 'date':
			csharpType = 'DateTime';
			break;
		case 'guid':
		case 'uuid':
			csharpType = 'Guid';
			break;
		case 'void':
			csharpType = 'void';
			break;
		default:
			// Check if it's an enum type (ends with 'enum')
			if (domainType.toLowerCase().endsWith('enum')) {
				// Use enhanced enum name conversion
				csharpType = convertEnumName(domainType);
			} else if (domainType.toLowerCase() === 'engineer' || domainType.toLowerCase() === 'manager') {
				// For actor types, use string representation (ID reference)
				csharpType = 'string';
			} else {
				// For other custom types, assume they are entity classes
				csharpType = toPascalCase(domainType);
			}
			break;
	}

	// Handle arrays/collections
	if (isArray) {
		csharpType = `ICollection<${csharpType}>`;
	}

	// Handle nullable types (except for string and collections which are reference types)
	if (isOptional && !isArray && csharpType !== 'string' && csharpType !== 'void') {
		if (['int', 'long', 'decimal', 'double', 'bool', 'DateTime', 'Guid'].includes(csharpType)) {
			csharpType += '?';
		}
	}

	return csharpType;
}

// Generate Data Annotations based on attribute properties
function generateDataAnnotations(attribute: EntityAttribute): string[] {
	const annotations: string[] = [];

	// Always add Column attribute to map to database column name
	annotations.push(`[Column("${attribute.name}")]`);

	// Handle constraints
	if (attribute.constraints) {
		for (const constraint of attribute.constraints) {
			switch (constraint.toLowerCase()) {
				case 'required':
				case 'notnull':
					if (attribute.type !== 'string' || !attribute.isOptional) {
						annotations.push('[Required]');
					}
					break;
				case 'key':
				case 'primarykey':
					annotations.push('[Key]');
					break;
				case 'unique':
					annotations.push('[Index(IsUnique = true)]');
					break;
			}
		}
	}

	// Auto-detect primary key by naming convention
	if (attribute.name.toLowerCase() === 'id' ||
		attribute.name.toLowerCase().endsWith('id') &&
		attribute.name.toLowerCase().indexOf('id') === attribute.name.length - 2) {
		if (!annotations.some(a => a.includes('[Key]'))) {
			annotations.push('[Key]');
		}
	}

	// Add Required annotation for non-nullable reference types
	if (!attribute.isOptional && attribute.type === 'string' && !annotations.some(a => a.includes('[Required]'))) {
		annotations.push('[Required]');
	}

	// Add MaxLength for strings (default reasonable length)
	if (attribute.type === 'string' && !annotations.some(a => a.includes('[MaxLength]'))) {
		const maxLength = attribute.name.toLowerCase().includes('name') ? '100' :
			attribute.name.toLowerCase().includes('description') ? '500' :
				attribute.name.toLowerCase().includes('email') ? '255' : '255';
		annotations.push(`[MaxLength(${maxLength})]`);
	}

	return annotations;
}

// Generate navigation properties based on relationships
function generateNavigationProperties(entity: Entity, relationships: Relationship[], metadata: DomainModelMetadata): string[] {
	const navigationProps: string[] = [];

	// Find relationships where this entity is involved
	const entityRelationships = relationships.filter(rel =>
		rel.sourceEntity === entity.name || rel.targetEntity === entity.name
	);

	for (const rel of entityRelationships) {
		if (rel.sourceEntity === entity.name) {
			// This entity is the source - add navigation to target
			const targetEntity = rel.targetEntity;

			// Find target entity in metadata to check its type
			const targetEntityDef = metadata.entities.find(e => e.name === targetEntity);
			if (!targetEntityDef || targetEntityDef.type === 'actor') {
				// Skip Actor types - they are not generated as entity classes
				continue;
			}

			const propName = targetEntity;

			if (rel.cardinality?.includes('*') || rel.type === 'composition') {
				navigationProps.push(`    public virtual ICollection<${targetEntity}> ${propName} { get; set; } = new List<${targetEntity}>();`);
			} else {
				navigationProps.push(`    public virtual ${targetEntity}? ${propName} { get; set; }`);
			}
		} else if (rel.targetEntity === entity.name) {
			// This entity is the target - add navigation to source
			const sourceEntity = rel.sourceEntity;

			// Find source entity in metadata to check its type
			const sourceEntityDef = metadata.entities.find(e => e.name === sourceEntity);
			if (!sourceEntityDef || sourceEntityDef.type === 'actor') {
				// Skip Actor types - they are not generated as entity classes
				continue;
			}

			const propName = sourceEntity;

			if (rel.cardinality?.startsWith('*') || rel.type === 'aggregation') {
				navigationProps.push(`    public virtual ICollection<${sourceEntity}> ${propName} { get; set; } = new List<${sourceEntity}>();`);
			} else {
				navigationProps.push(`    public virtual ${sourceEntity}? ${propName} { get; set; }`);
			}
		}
	}

	return navigationProps;
}

// Generate a complete C# entity class
function generateEntityClass(entity: Entity, metadata: DomainModelMetadata, options: GenerationOptions): string {
	const lines: string[] = [];

	// Using statements
	lines.push('using System;');
	lines.push('using System.Collections.Generic;');
	lines.push('using System.ComponentModel.DataAnnotations;');
	lines.push('using System.ComponentModel.DataAnnotations.Schema;');
	lines.push('using Microsoft.EntityFrameworkCore;');
	lines.push('');

	// Namespace
	lines.push(`namespace ${options.namespace};`);
	lines.push('');

	// Class documentation
	if (options.generateComments && entity.description) {
		lines.push('/// <summary>');
		lines.push(`/// ${entity.description}`);
		lines.push('/// </summary>');
	}

	// Class declaration
	lines.push(`public class ${entity.name}`);
	lines.push('{');

	// Properties
	for (const attr of entity.attributes) {
		lines.push('');

		// Property documentation
		if (options.generateComments) {
			lines.push('    /// <summary>');
			lines.push(`    /// ${attr.name} property`);
			lines.push('    /// </summary>');
		}

		// Data annotations
		const annotations = generateDataAnnotations(attr);
		for (const annotation of annotations) {
			lines.push(`    ${annotation}`);
		}

		// Property declaration
		const csharpType = mapToCSharpType(attr.type, attr.isOptional, attr.isArray);
		const propertyName = toPascalCase(attr.name); // Convert snake_case to PascalCase

		if (attr.isArray) {
			lines.push(`    public ${csharpType} ${propertyName} { get; set; } = new List<${csharpType.replace('ICollection<', '').replace('>', '')}>();`);
		} else {
			lines.push(`    public ${csharpType} ${propertyName} { get; set; }`);
		}
	}

	// Navigation properties
	if (options.includeNavigationProperties) {
		const navProps = generateNavigationProperties(entity, metadata.relationships, metadata);
		if (navProps.length > 0) {
			lines.push('');
			lines.push('    // Navigation Properties');
			for (const navProp of navProps) {
				lines.push('');
				lines.push(navProp);
			}
		}
	}

	lines.push('}');

	return lines.join('\n');
}

// Main generation function
function generateEntities(metadataFilePath: string, outputDir?: string, namespace?: string): void {
	console.log('🚀 Starting entity class generation...');

	// Validate input file
	if (!existsSync(metadataFilePath)) {
		throw new Error(`Metadata file not found: ${metadataFilePath}`);
	}

	// Read and parse metadata using utility function
	const metadata: DomainModelMetadata = readJsonFile<DomainModelMetadata>(metadataFilePath);

	console.log(`📋 Found ${metadata.entities.length} entities to generate`);

	// Determine output directory and namespace
	let finalOutputDir: string;
	let finalNamespace: string;

	if (outputDir && namespace) {
		finalOutputDir = outputDir;
		finalNamespace = namespace;
		console.log('📝 Using provided paths:');
	} else {
		const entityPaths = constructEntityPath();
		finalOutputDir = entityPaths.outputDir;
		finalNamespace = entityPaths.namespace;
		console.log('🔍 Using auto-detected paths:');
	}

	console.log(`   📁 Output: ${finalOutputDir}`);
	console.log(`   📦 Namespace: ${finalNamespace}`);

	// Create output directory if it doesn't exist using utility
	ensureDirectoryExists(finalOutputDir);

	const options: GenerationOptions = {
		namespace: finalNamespace,
		outputDirectory: finalOutputDir,
		generateComments: true,
		includeNavigationProperties: true
	};

	let generatedCount = 0;

	// Generate entity classes
	for (const entity of metadata.entities) {
		// Skip non-entity types for now
		if (entity.type !== 'entity') {
			console.log(`⏭️  Skipping ${entity.name} (type: ${entity.type})`);
			continue;
		}

		console.log(`🔧 Generating ${entity.name}...`);

		// Generate class content
		const classContent = generateEntityClass(entity, metadata, options);

		// Write to file
		const fileName = `${entity.name}.cs`;
		const filePath = join(finalOutputDir, fileName);

		try {
			writeFileSync(filePath, classContent, 'utf-8');
			console.log(`✅ Generated: ${fileName}`);
			generatedCount++;
		} catch (error) {
			console.error(`❌ Failed to generate ${fileName}:`, error);
		}
	}

	console.log(`🎉 Entity generation complete! Generated ${generatedCount} entity classes.`);

	// Format generated code using dotnet format
	formatGeneratedCode(finalOutputDir);
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
			console.log('Usage: bun run generate-entities.ts [metadata-file] [output-directory] [namespace]');
			console.log('');
			console.log('Arguments:');
			console.log('  metadata-file    Path to the JSON metadata file from domain-model-parser (auto-detected if not provided)');
			console.log('  output-directory Optional: Path to the directory where entity classes will be generated (auto-detected if not provided)');
			console.log('  namespace        Optional: C# namespace for the entity classes (auto-detected if not provided)');
			console.log('');
			console.log('Examples:');
			console.log('  bun run generate-entities.ts                                               # Auto-detect all');
			console.log('  bun run generate-entities.ts ./domain-metadata.json');
			console.log('  bun run generate-entities.ts ./domain-metadata.json ./custom/path Custom.Namespace');
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
		generateEntities(metadataFile, outputDir, namespace);
	} catch (error) {
		console.error('❌ Generation failed:', error);
		process.exit(1);
	}
}

// Run if called directly
// @ts-ignore
if (import.meta.main) {
	main();
}

// Code formatting - USE UTILITY VERSION
// function formatGeneratedCode() - now imported from utilities

// Note: No exports needed - skills communicate through Copilot, not direct imports
