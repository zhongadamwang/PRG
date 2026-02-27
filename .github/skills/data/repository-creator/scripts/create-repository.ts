#!/usr/bin/env bun

/**
 * {{pascalEntityName}} Repository Creator Script  
 * Generates repository files based on eServiceCloud patterns
 */

// @ts-ignore
import { promises as fs } from 'node:fs';
// @ts-ignore
import { join } from 'node:path';

// Configuration
const entityName = '{{pascalEntityName}}';
const featureName = '{{featureName}}';
const projectName = '{{projectName}}';
const namespace = '{{namespace}}';

// File paths
// @ts-ignore
const repositoryDir = join(process.cwd(), 'src', 'app', `${projectName}.Repositories`, featureName);
const repositoryFile = join(repositoryDir, `${entityName}Repository.cs`);

interface CreateRepositoryConfig {
	entityName: string;
	featureName: string;
	projectName: string;
	namespace: string;
	dataServiceInterface: string;
	entityUsing: string;
	modelUsing: string;
}

async function createRepositoryFiles(config: CreateRepositoryConfig): Promise<void> {
	console.log(`🔧 Creating ${config.entityName} repository files...`);

	try {
		// Ensure directory exists
		await fs.mkdir(repositoryDir, { recursive: true });

		// Check if files already exist  
		const repositoryExists = await fileExists(repositoryFile);

		if (repositoryExists) {
			console.log('⚠️ Repository files already exist. Use --force to overwrite.');
			return;
		}

		console.log(`✅ Repository files created successfully!`);
		console.log(`   📁 Directory: ${repositoryDir}`);
		console.log(`   📄 Repository: ${config.entityName}Repository.cs`);
		console.log('');
		console.log('📋 Next Steps:');
		console.log(`   1. Add dependency injection registration:`);
		console.log(`      services.AddScopedWithTimeLogging<I${config.entityName}Repository, ${config.entityName}Repository>();`);
		console.log(`   2. Configure entity and model mappings in IMappingService`);
		console.log(`   3. Add any custom query methods as needed`);
		console.log(`   4. Create corresponding unit tests`);

	} catch (error) {
		console.error(`❌ Error creating repository: ${(error as Error).message}`);
		// @ts-ignore
		process.exit(1);
	}
}

async function fileExists(filePath: string): Promise<boolean> {
	try {
		await fs.access(filePath);
		return true;
	} catch {
		return false;
	}
}

// Main execution
async function main(): Promise<void> {
	console.log(`🚀 REPOSITORY-CREATOR - eServiceCloud Repository Generator`);
	console.log('Creates repository interface and implementation with clean architecture patterns\n');

	const config: CreateRepositoryConfig = {
		entityName,
		featureName,
		projectName,
		namespace,
		dataServiceInterface: '{{dataServiceInterface}}',
		entityUsing: '{{entityUsing}}',
		modelUsing: '{{modelUsing}}'
	};

	await createRepositoryFiles(config);
}

// Run if called directly
// @ts-ignore
if (import.meta.main) {
	main().catch(console.error);
}

export { createRepositoryFiles, type CreateRepositoryConfig };