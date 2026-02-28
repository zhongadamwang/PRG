#!/usr/bin/env bun

/**
 * Repository Creator Script
 * Generates repository files based on entity characteristics and inheritance patterns
 * 
 * Usage: bun run create-repository.ts <entityName> [outputPath]
 * 
 * The script:
 * 1. Gets entity information using TypeInfoRetriever
 * 2. Determines template based on ObjectVersion inheritance
 * 3. Maps entity to corresponding DataService interface
 * 4. Generates repository file with appropriate pattern
 */

// @ts-ignore
import { spawn } from 'node:child_process';
// @ts-ignore
import { promises as fs } from 'node:fs';
// @ts-ignore
import { join } from 'node:path';

// Configuration
const SKILLS_PATH = '.github/skills/data/repository-creator';
const TEMPLATES_PATH = join(SKILLS_PATH, 'templates');
const STANDARD_TEMPLATE = 'Repository.cs';
const VERSION_TEMPLATE = 'VersionRepository.cs';
const DEFAULT_OUTPUT_PATH = 'src/Sanjel.RequestManagement.Repositories';

interface EntityInfo {
	name: string;
	fullPath: string;
	hasObjectVersion: boolean;
	baseClasses: string[];
}

interface RepositoryConfig {
	entityName: string;
	entityClassFullPath: string;
	entityDataServiceInterfaceFullPath: string;
	projectName: string;
	templateFile: string;
	outputPath: string;
}

/**
 * Get entity information using TypeInfoRetriever
 */
async function getEntityInfo(entityName: string): Promise<EntityInfo> {
	console.log(`🔍 Getting entity information for: ${entityName}`);

	try {
		// Try with provided name first
		let fullEntityPath = entityName;

		// If not a full path, try to construct it with common patterns
		if (!entityName.includes('Sesi.SanjelData.Entities')) {
			// Try common business entities paths in order of likelihood
			const commonPaths = [
				`Sesi.SanjelData.Entities.BusinessEntities.Engineering.ProgramDesign.${entityName}`,
				`Sesi.SanjelData.Entities.BusinessEntities.Engineering.ProgramDesign.Template.${entityName}`,
				`Sesi.SanjelData.Entities.BusinessEntities.${entityName}`,
				`Sesi.SanjelData.Entities.Common.BusinessEntities.${entityName}`,
				`Sesi.SanjelData.Entities.Common.BusinessEntities.HumanResources.${entityName}`,
				`Sesi.SanjelData.Entities.BusinessEntities.Engineering.${entityName}`,
				`Sesi.SanjelData.Entities.BusinessEntities.Operations.${entityName}`
			];

			for (const path of commonPaths) {
				try {
					const result = await executeTypeInfoRetriever(path);
					if (result.success) {
						fullEntityPath = path;
						break;
					}
				} catch (e) {
					// Continue trying other paths
					continue;
				}
			}
		}

		const result = await executeTypeInfoRetriever(fullEntityPath);
		if (!result.success) {
			// Try to provide helpful error message with suggestions
			console.log(`❌ Entity '${entityName}' not found at path: ${fullEntityPath}`);
			console.log('💡 Tip: Try providing the full namespace path, for example:');
			console.log('   - Sesi.SanjelData.Entities.BusinessEntities.Engineering.ProgramDesign.ProgramRequest');
			console.log('   - Sesi.SanjelData.Entities.BusinessEntities.Engineering.ProgramDesign.Template.StickDiagramTemplate');

			throw new Error(`Entity not found: ${entityName}. Please verify the entity exists in Sesi.SanjelData.Entities namespace or provide the full path.`);
		}

		// Parse the output to extract inheritance information
		// ObjectVersion entities must have ALL these properties
		const requiredObjectVersionProps = [
			'Version { get; set; }',
			'ModifiedUserId { get; set; }',
			'EffectiveStartDateTime { get; set; }',
			'EffectiveEndDateTime { get; set; }'
		];

		const foundProps = requiredObjectVersionProps.filter(prop => result.output.includes(prop));
		const hasObjectVersion = foundProps.length === requiredObjectVersionProps.length ||
			result.output.toLowerCase().includes('objectversion') ||
			result.output.toLowerCase().includes('versionentity');

		console.log(`🔍 ObjectVersion Detection:
  - Required Properties: ${requiredObjectVersionProps.length}
  - Found Properties: ${foundProps.length} (${foundProps.join(', ')})
  - Has ObjectVersion: ${hasObjectVersion}
  - Detection Reason: ${hasObjectVersion ? (foundProps.length === requiredObjectVersionProps.length ? 'All ObjectVersion properties found' : 'ObjectVersion keyword found') : 'Missing required ObjectVersion properties'}`);
		// Extract base classes (simplified parsing - could be enhanced)
		const baseClasses = extractBaseClasses(result.output);

		// Extract simple entity name from full path
		const simpleEntityName = fullEntityPath.split('.').pop() || entityName;

		return {
			name: simpleEntityName,
			fullPath: fullEntityPath,
			hasObjectVersion,
			baseClasses
		};

	} catch (error) {
		console.error(`❌ Failed to get entity info: ${error}`);
		throw error;
	}
}

/**
 * Execute TypeInfoRetriever tool
 */
async function executeTypeInfoRetriever(entityPath: string): Promise<{ success: boolean; output: string }> {
	return new Promise((resolve, reject) => {
		const process = spawn('dotnet', [
			'run',
			'--project',
			'TypeInfoRetriever.csproj',
			entityPath,
			'simple'
		], {
			cwd: 'tools/TypeInfoRetriever',
			stdio: ['ignore', 'pipe', 'pipe']
		});

		let stdout = '';
		let stderr = '';

		process.stdout?.on('data', (data: string) => {
			stdout += data.toString();
		});

		process.stderr?.on('data', (data: string) => {
			stderr += data.toString();
		});

		process.on('close', (code: number) => {
			if (code === 0 && stdout.trim()) {
				resolve({ success: true, output: stdout });
			} else {
				resolve({ success: false, output: stderr || stdout });
			}
		});

		process.on('error', (error: Error) => {
			reject(error);
		});
	});
}

/**
 * Extract base classes from TypeInfoRetriever output (simplified parsing)
 */
function extractBaseClasses(output: string): string[] {
	const baseClasses: string[] = [];
	const lines = output.split('\n');

	for (const line of lines) {
		if (line.includes('Base Class:') || line.includes('Inherits:')) {
			const match = line.split(':')[1]?.trim();
			if (match) {
				baseClasses.push(match);
			}
		}
	}

	return baseClasses;
}

/**
 * Map entity namespace to corresponding DataService interface namespace
 */
function mapToDataServiceInterface(entityFullPath: string): string {
	// Replace Entities with Services.Interfaces
	let servicePath = entityFullPath.replace(
		'Sesi.SanjelData.Entities.BusinessEntities',
		'Sesi.SanjelData.Services.Interfaces.BusinessEntities'
	);

	// Extract entity name and add I prefix + Service suffix
	const entityName = servicePath.split('.').pop();
	if (!entityName) {
		throw new Error(`Cannot extract entity name from path: ${entityFullPath}`);
	}

	// Replace entity name with interface name
	const interfaceName = `I${entityName}Service`;
	servicePath = servicePath.replace(entityName, interfaceName);

	return servicePath;
}

/**
 * Extract project name from solution file
 */
async function getProjectName(): Promise<string> {
	try {
		// Look for .slnx files in src directory
		const srcFiles = await fs.readdir('src') as string[];
		const solutionFile = srcFiles.find(file => file.endsWith('.slnx'));

		if (solutionFile) {
			// Extract project name from solution file
			// e.g., "Sanjel.RequestManagement.slnx" -> "RequestManagement"
			const match = solutionFile.match(/Sanjel\.(.+)\.slnx/);
			if (match) {
				return match[1];
			}
		}

		// Fallback to directory-based detection
		const repositoryDirs = await fs.readdir('src') as string[];
		const repoDir = repositoryDirs.find(dir => dir.includes('Repositories') && !dir.includes('Tests'));

		if (repoDir) {
			// e.g., "Sanjel.RequestManagement.Repositories" -> "RequestManagement"
			const match = repoDir.match(/Sanjel\.(.+)\.Repositories/);
			if (match) {
				return match[1];
			}
		}

		// Ultimate fallback
		return 'RequestManagement';

	} catch (error) {
		console.warn(`⚠️  Could not determine project name, using default: ${error}`);
		return 'RequestManagement';
	}
}

/**
 * Load and process template file
 */
async function loadTemplate(templateFile: string): Promise<string> {
	const templatePath = join(TEMPLATES_PATH, templateFile);

	try {
		const templateContent = await fs.readFile(templatePath, 'utf-8');
		return templateContent;
	} catch (error) {
		throw new Error(`Failed to load template ${templateFile}: ${error}`);
	}
}

/**
 * Replace template variables with actual values
 */
function replaceTemplateVariables(template: string, config: RepositoryConfig): string {
	return template
		.replace(/\{\{EntityName\}\}/g, config.entityName)
		.replace(/\{\{EntityClassFullPath\}\}/g, config.entityClassFullPath)
		.replace(/\{\{EntityDataServiceInterfaceFullPath\}\}/g, config.entityDataServiceInterfaceFullPath)
		.replace(/\{\{ProjectName\}\}/g, config.projectName);
}

/**
 * Write generated repository file
 */
async function writeRepositoryFile(content: string, config: RepositoryConfig): Promise<string> {
	const fileName = `${config.entityName}Repository.cs`;
	const outputFilePath = join(config.outputPath, fileName);

	try {
		// Ensure output directory exists
		await fs.mkdir(config.outputPath, { recursive: true });

		// Write the file
		await fs.writeFile(outputFilePath, content, 'utf-8');

		console.log(`✅ Successfully created: ${outputFilePath}`);
		return outputFilePath;

	} catch (error) {
		throw new Error(`Failed to write repository file: ${error}`);
	}
}

/**
 * Main function to create repository
 */
async function createRepository(entityName: string, outputPath?: string): Promise<void> {
	try {
		console.log(`🚀 Starting repository creation for entity: ${entityName}`);

		// Step 1: Get entity information
		const entityInfo = await getEntityInfo(entityName);
		console.log(`📋 Entity Info:
  - Name: ${entityInfo.name}
  - Full Path: ${entityInfo.fullPath}
  - Has ObjectVersion: ${entityInfo.hasObjectVersion}
  - Base Classes: ${entityInfo.baseClasses.join(', ') || 'None'}`);

		// Step 2: Determine template based on ObjectVersion inheritance
		const templateFile = entityInfo.hasObjectVersion ? VERSION_TEMPLATE : STANDARD_TEMPLATE;
		console.log(`📄 Selected template: ${templateFile}`);

		// Step 3: Map to DataService interface
		const dataServiceInterface = mapToDataServiceInterface(entityInfo.fullPath);
		console.log(`🔗 DataService Interface: ${dataServiceInterface}`);

		// Step 4: Get project name
		const projectName = await getProjectName();
		console.log(`📦 Project Name: ${projectName}`);

		// Step 5: Build configuration
		const config: RepositoryConfig = {
			entityName: entityInfo.name,
			entityClassFullPath: entityInfo.fullPath,
			entityDataServiceInterfaceFullPath: dataServiceInterface,
			projectName: projectName,
			templateFile: templateFile,
			outputPath: outputPath || DEFAULT_OUTPUT_PATH
		};

		// Step 6: Load and process template
		console.log(`📝 Loading template: ${templateFile}`);
		const template = await loadTemplate(templateFile);
		const processedContent = replaceTemplateVariables(template, config);

		// Step 7: Write repository file
		console.log(`💾 Writing repository file...`);
		const outputFilePath = await writeRepositoryFile(processedContent, config);

		console.log(`🎉 Repository creation completed successfully!
📁 Output: ${outputFilePath}
📋 Summary:
  - Entity: ${config.entityName}
  - Template: ${config.templateFile}
  - Pattern: ${entityInfo.hasObjectVersion ? 'Version Repository' : 'Standard Repository'}`);

	} catch (error) {
		console.error(`💥 Repository creation failed: ${error}`);
		// @ts-ignore
		process.exit(1);
	}
}

// Main execution
async function main() {
	// @ts-ignore
	const args = process.argv.slice(2);

	if (args.length === 0) {
		console.error(`❌ Usage: bun run create-repository.ts <entityName> [outputPath]
    
Examples:
  bun run create-repository.ts ProgramRequest
  bun run create-repository.ts StickDiagramTemplate
  bun run create-repository.ts "Sesi.SanjelData.Entities.BusinessEntities.Engineering.ProgramDesign.ProgramRequest"
  bun run create-repository.ts Employee src/Custom.Repositories`);
		// @ts-ignore
		process.exit(1);
	}

	const entityName = args[0];
	const outputPath = args[1];

	await createRepository(entityName, outputPath);
}

// Execute if run directly
// @ts-ignore
if (import.meta.main) {
	main().catch(console.error);
}
