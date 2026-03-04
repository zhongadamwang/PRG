// @ts-ignore
import { existsSync, readFileSync, writeFileSync } from 'node:fs';
// @ts-ignore
import { join } from 'node:path';

// Import shared utilities
import {
	constructRepositoryPath,
	ensureDirectoryExists,
	findDomainModelMetadata,
	formatGeneratedCode,
	readJsonFile,
	toPascalCase
} from '../../../utilities/project-utilities/scripts/utilities';

// @ts-ignore
const process = globalThis.process;

// Domain model metadata types
interface DomainModelMetadata {
	entities: Entity[];
	relationships: Relationship[];
	enums: EnumDefinition[];
}

interface Entity {
	name: string;
	description?: string;
	type: string;
	attributes: Attribute[];
}

interface Attribute {
	name: string;
	type: string;
	required?: boolean;
	unique?: boolean;
	maxLength?: number;
	description?: string;
}

interface Relationship {
	from: string;
	to: string;
	type: string;
	description?: string;
}

interface EnumDefinition {
	name: string;
	type: string;
	values?: string[];
}

// Repository generation options
interface GenerationOptions {
	outputDir: string;
	namespace: string;
	generateComments: boolean;
	includePerformanceMarkers: boolean;
}

// Parse interface method from string
interface InterfaceMethod {
	name: string;
	returnType: string;
	parameters: string;
	description?: string;
	attributes?: string[];
}

// Parse repository interface file to extract method signatures
function parseRepositoryInterface(interfaceFilePath: string): InterfaceMethod[] {
	if (!existsSync(interfaceFilePath)) {
		console.log(`⚠️  Interface file not found: ${interfaceFilePath}`);
		return [];
	}

	console.log(`📖 Parsing interface file: ${interfaceFilePath}`);
	const content = readFileSync(interfaceFilePath, 'utf-8');
	const methods: InterfaceMethod[] = [];

	// Extract method signatures from interface
	const lines = content.split('\n');

	for (let i = 0; i < lines.length; i++) {
		const line = lines[i].trim();

		// Look for Task<...> method signatures (skip interface declaration line)
		// Handle both indented and non-indented lines
		if (line.includes('Task<') && line.includes('(') && line.includes(';') && !line.includes('interface')) {
			console.log(`🔍 Found potential method line: ${line}`);

			// Extract method name - handle indentation and attributes
			// Pattern: Task<...> MethodName(...)
			// Use a more robust regex to handle nested generics
			const taskMatch = line.match(/Task<.*?>\s+(\w+)\s*\(/);
			if (taskMatch) {
				const methodName = taskMatch[1];
				console.log(`✅ Extracted method name: ${methodName}`);

				// Extract return type - handle Entity alias
				const returnTypeMatch = line.match(/Task<([^>]+)>/);
				let returnType = returnTypeMatch ? returnTypeMatch[1] : 'void';

				// Replace Entity with actual entity name if needed
				if (returnType.includes('Entity')) {
					// Extract actual entity name from namespace or context
					const entityMatch = content.match(/using Entity = [^.]+\.Entities\.(\w+);/);
					if (entityMatch) {
						returnType = returnType.replace(/\bEntity\b/g, entityMatch[1]);
						console.log(`🔄 Replaced Entity with ${entityMatch[1]}: ${returnType}`);
					}
				}

				// Extract parameters (everything between parentheses)
				const paramMatch = line.match(/\(([^)]*)\)/);
				let parameters = paramMatch ? paramMatch[1] : '';

				// Replace Entity with actual entity name in parameters too
				if (parameters.includes('Entity')) {
					const entityMatch = content.match(/using Entity = [^.]+\.Entities\.(\w+);/);
					if (entityMatch) {
						parameters = parameters.replace(/\bEntity\b/g, entityMatch[1]);
						console.log(`🔄 Replaced Entity in parameters: ${parameters}`);
					}
				}

				// Look for description in previous lines
				let description = '';
				for (let j = i - 1; j >= 0; j--) {
					const prevLine = lines[j].trim();
					if (prevLine.includes('/// Gets ') || prevLine.includes('/// Searches ')) {
						description = prevLine.replace('///', '').trim();
						break;
					}
					if (!prevLine.startsWith('///') && !prevLine.startsWith('[') && prevLine !== '') break;
				}

				methods.push({
					name: methodName,
					returnType,
					parameters,
					description
				});

				console.log(`➕ Added method: ${methodName} -> ${returnType}`);
			} else {
				console.log(`❌ Could not extract method name from: ${line}`);
			}
		}
	}

	console.log(`📋 Parsed ${methods.length} methods from interface`);
	for (const method of methods) {
		console.log(`   - ${method.name}: ${method.returnType}`);
	}

	return methods;
}

// Generate method implementation based on interface signature and entity metadata
function generateMethodImplementation(method: InterfaceMethod, entity: Entity, metadata: DomainModelMetadata): string[] {
	const lines: string[] = [];

	// Fix return type to use actual entity name (handle incomplete generics)
	let fixedReturnType = method.returnType.replace(/\bEntity\b/g, entity.name);
	// Ensure generics are properly closed
	if (fixedReturnType.includes('<') && !fixedReturnType.includes('>')) {
		fixedReturnType += '>';
	}

	// Fix parameters to use actual entity name
	let fixedParameters = method.parameters.replace(/\bEntity\b/g, entity.name);

	// Add method signature
	lines.push(`\tpublic async Task<${fixedReturnType}> ${method.name}(${fixedParameters})`);
	lines.push('\t{');

	// Generate method body based on method name pattern
	if (method.name.startsWith('GetBy') && method.name.includes('Status')) {
		// Status-based query - use correct status field name
		const paramName = method.parameters.split(' ')[1].split(',')[0]; // Extract first parameter name
		const statusFieldName = extractStatusFieldFromMethod(method.name, entity);
		lines.push(`\t\treturn await this._dbSet`);
		lines.push(`\t\t\t.Where(e => e.${statusFieldName} == ${paramName})`);
		lines.push(`\t\t\t.ToListAsync(cancellationToken);`);

	} else if (method.name.includes('Range') && method.name.includes('Date')) {
		// Date range query
		const dateFieldName = extractDateFieldFromMethod(method.name, entity);
		lines.push(`\t\treturn await this._dbSet`);
		lines.push(`\t\t\t.Where(e => e.${dateFieldName} >= startDate && e.${dateFieldName} <= endDate)`);
		lines.push(`\t\t\t.ToListAsync(cancellationToken);`);

	} else if (method.name.startsWith('SearchBy')) {
		// Text search query
		const searchFieldName = extractSearchFieldFromMethod(method.name, entity);
		lines.push(`\t\treturn await this._dbSet`);
		lines.push(`\t\t\t.Where(e => e.${searchFieldName}.Contains(searchText))`);
		lines.push(`\t\t\t.ToListAsync(cancellationToken);`);

	} else if (method.name.includes('GetBy') && method.name.includes('IdAsync')) {
		// Foreign key relationship query - implement with navigation properties
		const relatedEntity = extractRelatedEntityFromMethod(method.name);
		if (relatedEntity) {
			const paramName = getFirstParameterName(method.parameters);

			// Generate implementation based on relationship analysis
			// Check if current entity has a direct foreign key to the related entity
			const directFkField = entity.attributes.find(attr =>
				attr.name.toLowerCase().includes(relatedEntity.toLowerCase()) &&
				attr.name.toLowerCase().includes('id')
			);

			if (directFkField) {
				// Direct foreign key relationship
				lines.push(`\t\t// Query by foreign key relationship`);
				lines.push(`\t\treturn await this._dbSet`);
				lines.push(`\t\t\t.Where(e => e.${toPascalCase(directFkField.name)} == ${paramName}.ToString())`);
				lines.push(`\t\t\t.ToListAsync(cancellationToken);`);
			} else {
				// No direct foreign key found - use conservative approach
				// Check if related entity exists in metadata and has back-reference
				const relatedEntityData = metadata.entities.find(e => e.name === relatedEntity);
				if (relatedEntityData) {
					// Check if related entity has a field pointing back to current entity
					const backReference = relatedEntityData.attributes.find(attr =>
						attr.name.toLowerCase().includes(entity.name.toLowerCase()) &&
						attr.name.toLowerCase().includes('id')
					);

					if (backReference) {
						// Generate conservative implementation - return empty for now
						lines.push(`\t\t// Relationship exists but requires EF Core navigation properties`);
						lines.push(`\t\t// TODO: Configure navigation properties between ${entity.name} and ${relatedEntity}`);
						lines.push(`\t\treturn new List<${entity.name}>();`);
					} else {
						// No relationship found
						lines.push(`\t\t// No direct relationship between ${entity.name} and ${relatedEntity}`);
						lines.push(`\t\t// Return empty list as this relationship does not exist in the domain model`);
						lines.push(`\t\treturn new List<${entity.name}>();`);
					}
				} else {
					// Related entity not found in metadata
					lines.push(`\t\t// Related entity ${relatedEntity} not found in domain model`);
					lines.push(`\t\treturn new List<${entity.name}>();`);
				}
			}
		}

	} else if (method.name === 'GetPagedAsync') {
		// Pagination implementation
		generatePagedMethod(lines, entity);

	} else {
		// Default implementation
		lines.push(`\t\t// TODO: Implement ${method.name}`);
		lines.push(`\t\tthrow new NotImplementedException(\"${method.name} implementation pending\");`);
	}

	lines.push('\t}');
	return lines;
}

// Helper function to extract date field name from method name
function extractDateFieldFromMethod(methodName: string, entity: Entity): string {
	const lowerMethodName = methodName.toLowerCase();

	// Find matching date attribute based on metadata attribute names
	for (const attr of entity.attributes) {
		if (attr.type.toLowerCase().includes('date') || attr.type.toLowerCase().includes('datetime')) {
			const attrName = attr.name.toLowerCase();
			const methodPart = lowerMethodName.replace('getby', '').replace('rangeasync', '').replace('async', '');

			// Check if method name matches attribute name pattern (more flexible matching)
			if (methodPart.includes(attrName.replace('_', '')) ||
				attrName.replace('_', '').includes(methodPart.replace('date', ''))) {
				// Convert attribute name to PascalCase directly (don't remove underscores first)
				return toPascalCase(attr.name);
			}
		}
	}

	// Enhanced fallback based on common patterns - try to find best matching date field
	let bestMatch = null;
	let bestScore = 0;

	// Score each date attribute based on method name similarity
	for (const attr of entity.attributes) {
		if (attr.type.toLowerCase().includes('date') || attr.type.toLowerCase().includes('datetime')) {
			const attrWords = attr.name.toLowerCase().replace('_', ' ').split(' ');
			let score = 0;

			// Check how many words from attribute name appear in method name
			for (const word of attrWords) {
				if (lowerMethodName.includes(word)) {
					score += word.length; // Longer words get higher score
				}
			}

			if (score > bestScore) {
				bestScore = score;
				bestMatch = attr.name;
			}
		}
	}

	if (bestMatch) {
		return toPascalCase(bestMatch);
	}

	// Static fallbacks only if no date fields found in entity
	if (lowerMethodName.includes('created')) return 'CreatedDate';
	if (lowerMethodName.includes('acknowledgment')) return 'AcknowledgmentDate';
	if (lowerMethodName.includes('completion')) return 'CompletionDate';
	if (lowerMethodName.includes('sent')) return 'SentDate';
	if (lowerMethodName.includes('import')) return 'ImportDate';
	if (lowerMethodName.includes('submission')) return 'SubmissionDate';

	return 'CreatedDate'; // Default
}

// Helper function to extract status field name from method name
function extractStatusFieldFromMethod(methodName: string, entity: Entity): string {
	const lowerMethodName = methodName.toLowerCase();

	// Find matching status/enum attribute by method name pattern
	for (const attr of entity.attributes) {
		if (attr.type.toLowerCase().includes('enum') || attr.name.toLowerCase().includes('status')) {
			const attrNameLower = attr.name.toLowerCase();
			// More precise matching: check if method name contains the exact attribute name
			if (lowerMethodName.includes(attrNameLower.replace('_', ''))) {
				// Convert attribute name to PascalCase directly (don't remove underscores first)
				return toPascalCase(attr.name);
			}
		}
	}

	// Try to find any status-related field as fallback
	for (const attr of entity.attributes) {
		if (attr.name.toLowerCase().includes('status')) {
			return toPascalCase(attr.name);
		}
	}

	// Try to find any enum field as final fallback
	for (const attr of entity.attributes) {
		if (attr.type.toLowerCase().includes('enum')) {
			return toPascalCase(attr.name);
		}
	}

	return 'Status'; // Default
}

// Helper function to extract search field name from method name
function extractSearchFieldFromMethod(methodName: string, entity: Entity): string {
	const lowerMethodName = methodName.toLowerCase();

	// Find matching string attribute
	for (const attr of entity.attributes) {
		if (attr.type.toLowerCase().includes('string') || attr.type.toLowerCase().includes('email')) {
			const attrNameLower = attr.name.toLowerCase();
			if (lowerMethodName.includes(attrNameLower.replace('_', ''))) {
				// Convert attribute name to PascalCase directly (don't remove underscores first)
				return toPascalCase(attr.name);
			}
		}
	}

	// Common search fields
	if (lowerMethodName.includes('email')) return 'SourceEmail';
	if (lowerMethodName.includes('description')) return 'Description';
	if (lowerMethodName.includes('name')) return 'Name';

	return 'Description'; // Default
}

// Helper function to extract related entity from method name
function extractRelatedEntityFromMethod(methodName: string): string | null {
	const match = methodName.match(/GetBy(\w+)IdAsync/);
	return match ? match[1] : null;
}

// Helper function to get first parameter name
function getFirstParameterName(parameters: string): string {
	const parts = parameters.split(',')[0].trim().split(' ');
	return parts[parts.length - 1];
}

// Generate paged method implementation
function generatePagedMethod(lines: string[], entity: Entity): void {
	lines.push('\t\tvar query = this._dbSet.AsQueryable();');
	lines.push('');
	lines.push('\t\tif (filter != null)');
	lines.push('\t\t{');
	lines.push('\t\t\tquery = query.Where(filter);');
	lines.push('\t\t}');
	lines.push('');
	lines.push('\t\tvar totalCount = await query.CountAsync(cancellationToken);');
	lines.push('\t\tvar skip = (pageNumber - 1) * pageSize;');
	lines.push('');

	// Find a suitable ordering field (prefer date fields, then ID fields)
	let orderField = 'Id'; // Fallback default

	// First, try to find a date field for ordering
	for (const attr of entity.attributes) {
		if (attr.type.toLowerCase().includes('date')) {
			// Convert attribute name to PascalCase directly (don't remove underscores first)
			orderField = toPascalCase(attr.name);
			break;
		}
	}

	// If no date field found, look for ID field (usually ends with '_id')
	if (orderField === 'Id') {
		for (const attr of entity.attributes) {
			if (attr.name.toLowerCase().endsWith('_id') && attr.name.toLowerCase().includes(entity.name.toLowerCase())) {
				orderField = toPascalCase(attr.name);
				break;
			}
		}
	}

	// Final fallback - try to find any ID field
	if (orderField === 'Id') {
		for (const attr of entity.attributes) {
			if (attr.name.toLowerCase().includes('id')) {
				orderField = toPascalCase(attr.name);
				break;
			}
		}
	}

	lines.push('\t\tvar items = await query');
	lines.push(`\t\t\t.OrderBy(e => e.${orderField}) // Default ordering`);
	lines.push('\t\t\t.Skip(skip)');
	lines.push('\t\t\t.Take(pageSize)');
	lines.push('\t\t\t.ToListAsync(cancellationToken);');
	lines.push('');
	lines.push(`\t\treturn new PagedResult<${entity.name}>`);
	lines.push('\t\t{');
	lines.push('\t\t\tItems = items,');
	lines.push('\t\t\tTotalCount = totalCount,');
	lines.push('\t\t\tPageNumber = pageNumber,');
	lines.push('\t\t\tPageSize = pageSize,');
	lines.push('\t\t};');
}

// Generate repository implementation class
function generateRepositoryImplementation(entity: Entity, metadata: DomainModelMetadata, options: GenerationOptions): string {
	const lines: string[] = [];

	// Using statements
	lines.push('using System.Linq.Expressions;');
	lines.push('using Microsoft.EntityFrameworkCore;');
	lines.push('using Sanjel.RequestManagement.Core.Data;');
	lines.push('using Sanjel.RequestManagement.Core.Entities;');
	lines.push('using Sanjel.RequestManagement.Repositories.Common;');
	lines.push('');

	// Namespace
	lines.push(`namespace ${options.namespace};`);
	lines.push('');

	// Class documentation
	if (options.generateComments) {
		lines.push('/// <summary>');
		lines.push(`/// Repository implementation for ${entity.name} entities using EF Core.`);
		if (entity.description) {
			// Ensure description ends with a period for StyleCop compliance
			const description = entity.description.trim();
			const formattedDescription = description.endsWith('.') ? description : `${description}.`;
			lines.push(`/// ${formattedDescription}`);
		}
		lines.push('/// </summary>');
	}

	// Class declaration
	const interfaceName = `I${entity.name}Repository`;
	lines.push(`public class ${entity.name}Repository : BaseRepository<${entity.name}>, ${interfaceName}`);
	lines.push('{');

	// Constructor
	lines.push(`\tpublic ${entity.name}Repository(RequestManagementDbContext context)`);
	lines.push('\t\t: base(context)');
	lines.push('\t{');
	lines.push('\t}');

	// Find and parse the corresponding interface file
	const interfaceFileName = `I${entity.name}Repository.cs`;
	const interfaceFilePath = join(options.outputDir, interfaceFileName);
	const interfaceMethods = parseRepositoryInterface(interfaceFilePath);

	if (interfaceMethods.length > 0) {
		lines.push('');
		lines.push('\t// Implement entity-specific methods');

		for (let i = 0; i < interfaceMethods.length; i++) {
			const method = interfaceMethods[i];

			// Add method implementation
			const methodLines = generateMethodImplementation(method, entity, metadata);
			lines.push(...methodLines);
		}
	} else {
		lines.push('');
		lines.push('\t// No entity-specific methods found in interface');
		lines.push('\t// All standard CRUD operations are available through BaseRepository<T>');
	}

	lines.push('}');

	return lines.join('\n');
}

// Main generation function
function generateRepositoryImplementations(metadataFile?: string, outputDir?: string, namespace?: string): void {
	console.log('🚀 Starting EF Core repository implementation generation...');

	// Use provided metadata file or auto-detect
	const finalMetadataFile = metadataFile || findDomainModelMetadata();
	console.log(`📋 Using metadata: ${finalMetadataFile}`);

	// Load and parse domain model metadata
	const metadata = readJsonFile<DomainModelMetadata>(finalMetadataFile);
	console.log(`📋 Found ${metadata.entities.length} entities to analyze`);

	// Auto-detect output directory and namespace if not provided
	const { outputDir: finalOutputDir, namespace: finalNamespace } = outputDir && namespace
		? { outputDir, namespace }
		: constructRepositoryPath();

	console.log(`🔍 Using auto-detected paths:`);
	console.log(`   📁 Output: ${finalOutputDir}`);
	console.log(`   📦 Namespace: ${finalNamespace}`);

	// Ensure output directory exists
	ensureDirectoryExists(finalOutputDir);

	const options: GenerationOptions = {
		outputDir: finalOutputDir,
		namespace: finalNamespace,
		generateComments: true,
		includePerformanceMarkers: true
	};

	// Generate repository implementations for domain entities
	let generatedCount = 0;
	for (const entity of metadata.entities) {
		// Skip system and enum types, but include entities and actors
		if (entity.type === 'system' || entity.type === 'enum') {
			console.log(`⏭️  Skipping ${entity.name} (type: ${entity.type})`);
			continue;
		}

		console.log(`🔧 Generating ${entity.name}Repository...`);

		// Generate repository implementation
		const implementationContent = generateRepositoryImplementation(entity, metadata, options);

		// Write to file
		const fileName = `${entity.name}Repository.cs`;
		const filePath = join(finalOutputDir, fileName);

		try {
			writeFileSync(filePath, implementationContent, 'utf-8');
			console.log(`✅ Generated: ${fileName}`);
			generatedCount++;
		} catch (error) {
			console.error(`❌ Failed to generate ${fileName}:`, error);
		}
	}

	console.log(`🎉 Repository implementation generation complete! Generated ${generatedCount} implementation classes.`);

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
			console.log('Usage: bun run generate-efcore-repositories.ts [metadata-file] [output-directory] [namespace]');
			console.log('');
			console.log('Arguments:');
			console.log('  metadata-file    Path to the JSON metadata file from domain-model-parser (auto-detected if not provided)');
			console.log('  output-directory Optional: Path to the directory where repository implementations will be generated (auto-detected if not provided)');
			console.log('  namespace        Optional: C# namespace for the repository implementations (auto-detected if not provided)');
			console.log('');
			console.log('Examples:');
			console.log('  bun run generate-efcore-repositories.ts');
			console.log('  bun run generate-efcore-repositories.ts ./domain-model-metadata.json');
			console.log('  bun run generate-efcore-repositories.ts ./domain-model-metadata.json ./src/Repositories MyProject.Repositories');
			console.log('');
			console.log('Error:', error);
			return;
		}
	} else {
		metadataFile = args[0];
	}

	const outputDir = args[1];
	const namespace = args[2];

	generateRepositoryImplementations(metadataFile, outputDir, namespace);
}

// Run the main function
main();