// @ts-ignore
import { existsSync, readFileSync, writeFileSync } from 'node:fs';

// @ts-ignore
const process = globalThis.process;

interface EntityAttribute {
	name: string;
	type: string;
	isOptional: boolean;
	isArray: boolean;
	constraints?: string[];
}

interface EntityMethod {
	name: string;
	returnType: string;
	parameters: { name: string; type: string }[];
}

interface Entity {
	id: string;
	name: string;
	type: 'entity' | 'actor' | 'enum' | 'system';
	description?: string;
	attributes: EntityAttribute[];
	methods: EntityMethod[];
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

interface ParseContext {
	entities: Entity[];
	relationships: Relationship[];
	enums: DomainEnum[];
}

// Utility functions
function mapClassType(type: string): 'entity' | 'actor' | 'enum' | 'system' {
	switch (type.toLowerCase()) {
		case 'actor': return 'actor';
		case 'enum': return 'enum';
		case 'ai': case 'system': return 'system';
		default: return 'entity';
	}
}

function mapDataType(type: string): string {
	const typeMap: Record<string, string> = {
		'String': 'string',
		'Integer': 'int',
		'Float': 'float',
		'Boolean': 'bool',
		'DateTime': 'DateTime',
		'Void': 'void'
	};
	return typeMap[type] || type.toLowerCase();
}

function parseAttribute(line: string): EntityAttribute | null {
	const attrMatch = line.match(/^[+\-#]\s*([a-zA-Z_]\w*):\s*([a-zA-Z_]\w+(?:\[\])?)\??$/);
	if (!attrMatch) return null;

	const name = attrMatch[1];
	const typeStr = attrMatch[2];
	const isOptional = line.includes('?');
	const isArray = typeStr.includes('[]');
	const type = typeStr.replace('[]', '');

	return {
		name,
		type: mapDataType(type),
		isOptional,
		isArray
	};
}

function parseMethod(line: string): EntityMethod | null {
	const methodMatch = line.match(/^[+\-#]\s*([a-zA-Z_]\w*)\((.*?)\):\s*([a-zA-Z_]\w+)$/);
	if (!methodMatch) return null;

	const name = methodMatch[1];
	const paramStr = methodMatch[2].trim();
	const returnType = methodMatch[3];

	const parameters = paramStr ? paramStr.split(',').map(param => {
		const paramTrimmed = param.trim();
		if (paramTrimmed.includes(':')) {
			const [paramName, paramType] = paramTrimmed.split(':').map(s => s.trim());
			return { name: paramName, type: mapDataType(paramType) };
		} else {
			return { name: paramTrimmed, type: 'string' };
		}
	}).filter(p => p.name) : [];

	return {
		name,
		returnType: mapDataType(returnType),
		parameters
	};
}

function parseClassContent(content: string, entity: Entity): void {
	const lines = content.split('\n').map(line => line.trim()).filter(line => line);

	console.log(`  📄 Parsing ${lines.length} lines for ${entity.name}`);

	for (const line of lines) {
		if (line.startsWith('+') || line.startsWith('-') || line.startsWith('#')) {
			if (line.includes('(') && line.includes(')')) {
				// Method
				const method = parseMethod(line);
				if (method) {
					console.log(`    🔧 Found method: ${method.name}`);
					entity.methods.push(method);
				}
			} else {
				// Attribute
				const attribute = parseAttribute(line);
				if (attribute) {
					console.log(`    📝 Found attribute: ${attribute.name}: ${attribute.type}`);
					entity.attributes.push(attribute);
				}
			}
		}
	}
}

function createEntity(className: string, classType: string, classContent: string): Entity {
	const entity: Entity = {
		id: className.toLowerCase(),
		name: className,
		type: mapClassType(classType),
		attributes: [],
		methods: [],
		category: classType
	};

	parseClassContent(classContent, entity);
	return entity;
}

function parseRelationship(line: string): Relationship | null {
	const relationshipPatterns = [
		{ pattern: /(\w+)\s*\*-->\s*(\w+)\s*:\s*(.+)/, type: 'composition' as const },
		{ pattern: /(\w+)\s*-->\s*(\w+)\s*:\s*(.+)/, type: 'association' as const },
		{ pattern: /(\w+)\s*\*-->\s*(\w+)/, type: 'composition' as const },
		{ pattern: /(\w+)\s*-->\s*(\w+)/, type: 'association' as const }
	];

	for (const { pattern, type } of relationshipPatterns) {
		const match = line.match(pattern);
		if (match) {
			return {
				id: `${match[1]}_to_${match[2]}`,
				sourceEntity: match[1],
				targetEntity: match[2],
				type,
				label: match[3] || undefined
			};
		}
	}
	return null;
}

function parseMermaidClassDiagram(content: string, context: ParseContext): void {
	const lines = content.split('\n');

	let inClass = false;
	let currentClass = '';
	let classType = '';
	let classContent = '';
	let braceCount = 0;

	for (let i = 0; i < lines.length; i++) {
		const line = lines[i];

		if (line.includes('class ') && line.includes('{')) {
			// Start of a class - use simple regex that we know works
			const classMatch = line.match(/class\s+(\w+)/);
			if (classMatch) {
				// Extract class type from :::type pattern
				const typeMatch = line.match(/:::(\w+)/);

				inClass = true;
				currentClass = classMatch[1];
				classType = typeMatch ? typeMatch[1] : 'entity';
				classContent = '';
				braceCount = 1;
				console.log(`🔍 Found class start: ${currentClass} (type: ${classType})`);
			}
		} else if (inClass) {
			if (line.includes('}')) {
				braceCount--;
				if (braceCount === 0) {
					// End of class - process the content
					console.log(`📄 Processing class content for ${currentClass}`);
					const entity = createEntity(currentClass, classType, classContent);
					context.entities.push(entity);
					inClass = false;
					currentClass = '';
					classContent = '';
				}
			} else {
				classContent += line + '\n';
			}
		}
	}

	// Parse relationships
	for (const line of lines) {
		const trimmedLine = line.trim();
		if (trimmedLine.includes('-->') || trimmedLine.includes('*-->') || trimmedLine.includes('--')) {
			const relationship = parseRelationship(trimmedLine);
			if (relationship) {
				context.relationships.push(relationship);
			}
		}
	}
}

function extractMermaidDiagrams(content: string, context: ParseContext): void {
	const mermaidRegex = /```mermaid\s*\n([\s\S]*?)\n```/g;
	let match;

	while ((match = mermaidRegex.exec(content)) !== null) {
		const diagramContent = match[1];
		parseMermaidClassDiagram(diagramContent, context);
	}
}

function extractDescription(text: string): string {
	const lines = text.split('\n').map(line => line.trim()).filter(line => line);
	const definitionMatch = lines.find(line => line.startsWith('- **Definition**:'));
	return definitionMatch ? definitionMatch.replace('- **Definition**:', '').trim() : '';
}

function extractEntityDescriptions(content: string, context: ParseContext): void {
	const entitySectionRegex = /###\s+(\w+)\s*\n([\s\S]*?)(?=\n###|\n##|\n#|$)/g;
	let match;

	while ((match = entitySectionRegex.exec(content)) !== null) {
		const entityName = match[1];
		const description = match[2];

		const entity = context.entities.find(e => e.name === entityName);
		if (entity) {
			entity.description = extractDescription(description);
		}
	}
}

function parseEnumValues(content: string): EnumValue[] {
	const lines = content.split('\n')
		.map(line => line.trim())
		.filter(line => line && !line.startsWith('<<') && !line.startsWith('%%'));

	return lines
		.filter(line => line.match(/^[A-Z_]+$/))
		.map(line => ({
			name: line,
			description: undefined
		}));
}

function extractEnumDefinitions(content: string, context: ParseContext): void {
	const enumRegex = /^\s*class\s+(\w*Enum)(?:::enum)?\s*\{([\s\S]*?<<enumeration>>[\s\S]*?)^\s*\}/gm;
	let match;

	while ((match = enumRegex.exec(content)) !== null) {
		const enumName = match[1];
		const enumContent = match[2];

		console.log(`🔍 Found enum: ${enumName}`);

		const enumDef: DomainEnum = {
			id: enumName.toLowerCase(),
			name: enumName,
			values: parseEnumValues(enumContent),
			description: `Enumeration for ${enumName.replace('Enum', '')} values`
		};

		context.enums.push(enumDef);
	}
}

function parseDomainModel(filePath: string): DomainModelMetadata {
	if (!existsSync(filePath)) {
		throw new Error(`Domain model file not found: ${filePath}`);
	}

	const content = readFileSync(filePath, 'utf-8');
	console.log('🔍 Parsing domain model...');

	const context: ParseContext = {
		entities: [],
		relationships: [],
		enums: []
	};

	extractMermaidDiagrams(content, context);
	extractEntityDescriptions(content, context);
	extractEnumDefinitions(content, context);

	const metadata: DomainModelMetadata = {
		version: '1.0.0',
		generatedAt: new Date().toISOString(),
		sourceFile: filePath,
		entities: context.entities,
		relationships: context.relationships,
		enums: context.enums,
		statistics: {
			totalEntities: context.entities.length,
			totalRelationships: context.relationships.length,
			totalEnums: context.enums.length,
			totalAttributes: context.entities.reduce((sum, entity) => sum + entity.attributes.length, 0)
		}
	};

	console.log(`✅ Parsed ${metadata.statistics.totalEntities} entities, ${metadata.statistics.totalRelationships} relationships, ${metadata.statistics.totalEnums} enums`);

	return metadata;
}

// Main execution
function main(): void {
	try {
		const args = process.argv.slice(2);

		if (args.length === 0) {
			console.error('❌ Usage: bun run parse-domain-model.ts <domain-model-file-path> [output-file-path]');
			process.exit(1);
		}

		const inputPath = args[0];
		const outputPath = args[1] || inputPath.replace('.md', '-metadata.json');

		console.log(`📖 Reading domain model from: ${inputPath}`);

		const metadata = parseDomainModel(inputPath);

		console.log(`💾 Writing metadata to: ${outputPath}`);
		writeFileSync(outputPath, JSON.stringify(metadata, null, 2), 'utf-8');

		console.log('🎉 Domain model parsing completed successfully!');
		console.log(`📊 Statistics:`);
		console.log(`   - Entities: ${metadata.statistics.totalEntities}`);
		console.log(`   - Relationships: ${metadata.statistics.totalRelationships}`);
		console.log(`   - Enums: ${metadata.statistics.totalEnums}`);
		console.log(`   - Attributes: ${metadata.statistics.totalAttributes}`);

	} catch (error) {
		console.error('❌ Error parsing domain model:', error);
		process.exit(1);
	}
}

// @ts-ignore
if (import.meta.main) {
	main();
}