// @ts-ignore
import { existsSync, mkdirSync, readFileSync, writeFileSync } from 'node:fs';
// @ts-ignore
import { join } from 'node:path';
// @ts-ignore
import { execSync } from 'node:child_process';

// @ts-ignore
const process = globalThis.process;

// Project structure information
interface ProjectInfo {
	projectRoot: string;
	projectName: string;
	slnxFile: string;
	srcRoot: string;
}

// Detect project root directory by finding .slnx files
export function detectProjectRoot(): string {
	// @ts-ignore
	let currentDir = process.cwd();
	console.log(`🔍 Detecting project root from: ${currentDir}`);

	// Navigate up to find project root (where src/.slnx files are located)
	let projectRoot = currentDir;
	let foundSlnx = false;

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

	console.log(`📂 Project root detected: ${projectRoot}`);
	return projectRoot;
}

// Get comprehensive project information
export function detectProjectInfo(): ProjectInfo {
	const projectRoot = detectProjectRoot();
	const srcRoot = join(projectRoot, 'src');

	// Find .slnx file
	const slnxFiles = (execSync(`find "${srcRoot}" -maxdepth 1 -name "*.slnx" -type f 2>/dev/null || true`, { encoding: 'utf-8' }) as string)
		.split('\n')
		.filter(line => line.trim())
		.map(line => line.trim());

	if (slnxFiles.length === 0) {
		throw new Error('No .slnx file found in src directory.');
	}

	const slnxFile = slnxFiles[0];
	const projectName = slnxFile.split('/').pop()?.replace('.slnx', '') || 'Unknown';

	console.log(`📦 Detected project: ${projectName}`);
	console.log(`🎯 Found .slnx: ${slnxFile}`);

	return {
		projectRoot,
		projectName,
		slnxFile,
		srcRoot
	};
}

// Construct standard paths for different types of generated code
export function constructMigrationPath(): { outputDir: string; namespace: string } {
	const { projectRoot, projectName } = detectProjectInfo();

	const outputDir = join(projectRoot, `src/${projectName}.Core/Migrations`);
	const namespace = `${projectName}.Core.Migrations`;

	console.log(`📁 Migration directory: ${outputDir}`);
	console.log(`📦 Migration namespace: ${namespace}`);

	return { outputDir, namespace };
}

export function constructEntityPath(): { outputDir: string; namespace: string } {
	const { projectRoot, projectName } = detectProjectInfo();

	const outputDir = join(projectRoot, `src/${projectName}.Core/Entities`);
	const namespace = `${projectName}.Core.Entities`;

	console.log(`📁 Entity directory: ${outputDir}`);
	console.log(`📦 Entity namespace: ${namespace}`);

	return { outputDir, namespace };
}

export function constructRepositoryPath(): { outputDir: string; namespace: string } {
	const { projectRoot, projectName } = detectProjectInfo();

	const outputDir = join(projectRoot, `src/${projectName}.Repositories`);
	const namespace = `${projectName}.Repositories`;

	console.log(`📁 Repository directory: ${outputDir}`);
	console.log(`📦 Repository namespace: ${namespace}`);

	return { outputDir, namespace };
}

export function constructServicePath(): { outputDir: string; namespace: string } {
	const { projectRoot, projectName } = detectProjectInfo();

	const outputDir = join(projectRoot, `src/${projectName}.Core/Services`);
	const namespace = `${projectName}.Core.Services`;

	console.log(`📁 Service directory: ${outputDir}`);
	console.log(`📦 Service namespace: ${namespace}`);

	return { outputDir, namespace };
}

export function constructPagePath(): { outputDir: string; namespace: string } {
	const { projectRoot, projectName } = detectProjectInfo();

	const outputDir = join(projectRoot, `src/${projectName}.Blazor/Pages`);
	const namespace = `${projectName}.Blazor.Pages`;

	console.log(`📁 Page directory: ${outputDir}`);
	console.log(`📦 Page namespace: ${namespace}`);

	return { outputDir, namespace };
}

export function constructDataPath(): { outputDir: string; namespace: string; projectName: string } {
	const { projectRoot, projectName } = detectProjectInfo();

	const outputDir = join(projectRoot, `src/${projectName}.Core/Data`);
	const namespace = `${projectName}.Core.Data`;

	console.log(`📁 Data directory: ${outputDir}`);
	console.log(`📦 Data namespace: ${namespace}`);

	return { outputDir, namespace, projectName };
}

// Format generated C# code using dotnet format
export function formatGeneratedCode(outputDir: string): void {
	try {
		console.log('🎨 Formatting generated code with dotnet format...');

		const projectInfo = detectProjectInfo();

		// Run dotnet format on the specific .slnx file and include the output directory
		const formatCommand = `dotnet format style "${projectInfo.slnxFile}" --include "${outputDir}/**/*.cs" --verbosity minimal`;
		execSync(formatCommand, { cwd: projectInfo.projectRoot, encoding: 'utf-8' });

		console.log('✅ Code formatting completed successfully!');
	} catch (error) {
		console.warn('⚠️  Code formatting failed, but generation was successful:', error);
		// Don't fail the generation if formatting fails
	}
}

// Format generated C# code for a specific project (used during project creation)
export function formatGeneratedCodeForProject(projectDir: string, slnxFile: string): void {
	try {
		console.log('🎨 Formatting generated code with dotnet format...');

		// Check if solution file exists
		if (!existsSync(slnxFile)) {
			console.warn('⚠️  Solution file not found, skipping code formatting:', slnxFile);
			return;
		}

		// Run dotnet format on the specific .slnx file and include the project directory
		const formatCommand = `dotnet format "${slnxFile}" --include "${projectDir}/**/*.cs"`;
		execSync(formatCommand, { cwd: projectDir, encoding: 'utf-8' });

		console.log('✅ Code formatting completed successfully!');
	} catch (error) {
		console.warn('⚠️  Code formatting failed, but generation was successful:', error);
		// Don't fail the generation if formatting fails
	}
}

// Format specific files only
export function formatSpecificFiles(filePaths: string[]): void {
	try {
		console.log(`🎨 Formatting ${filePaths.length} specific files with dotnet format...`);

		const projectInfo = detectProjectInfo();

		// Format each file individually using the .slnx file
		for (const filePath of filePaths) {
			const formatCommand = `dotnet format "${projectInfo.slnxFile}" --include "${filePath}"`;
			execSync(formatCommand, { cwd: projectInfo.projectRoot, encoding: 'utf-8' });
		}

		console.log('✅ Specific files formatting completed successfully!');
	} catch (error) {
		console.warn('⚠️  Code formatting failed, but generation was successful:', error);
		// Don't fail the generation if formatting fails
	}
}

// String manipulation utilities
export function toPascalCase(str: string): string {
	if (!str) return '';

	// Handle compound words and camelCase
	return str
		.replace(/([a-z])([A-Z])/g, '$1 $2') // Split camelCase
		.split(/[\s_-]+/) // Split on spaces, underscores, hyphens
		.filter(word => word.length > 0)
		.map(word => word.charAt(0).toUpperCase() + word.slice(1).toLowerCase())
		.join('');
}

export function toCamelCase(str: string): string {
	const pascalCase = toPascalCase(str);
	if (!pascalCase) return '';
	return pascalCase.charAt(0).toLowerCase() + pascalCase.slice(1);
}

export function toKebabCase(str: string): string {
	if (!str) return '';

	return str
		.replace(/([a-z])([A-Z])/g, '$1-$2') // Split camelCase
		.replace(/[\s_]+/g, '-') // Replace spaces and underscores
		.toLowerCase()
		.replace(/-+/g, '-') // Remove duplicate hyphens
		.replace(/^-|-$/g, ''); // Remove leading/trailing hyphens
}

// Simple pluralization (can be enhanced with more rules)
export function pluralize(str: string): string {
	if (!str) return '';

	// Basic English pluralization rules
	if (str.endsWith('y')) {
		return str.slice(0, -1) + 'ies';
	} else if (str.endsWith('s') || str.endsWith('sh') || str.endsWith('ch') || str.endsWith('x') || str.endsWith('z')) {
		return str + 'es';
	} else {
		return str + 's';
	}
}

// Simple singularization
export function singularize(str: string): string {
	if (!str) return '';

	// Basic English singularization rules
	if (str.endsWith('ies')) {
		return str.slice(0, -3) + 'y';
	} else if (str.endsWith('es') && (str.endsWith('ses') || str.endsWith('shes') || str.endsWith('ches') || str.endsWith('xes') || str.endsWith('zes'))) {
		return str.slice(0, -2);
	} else if (str.endsWith('s') && !str.endsWith('ss')) {
		return str.slice(0, -1);
	}

	return str;
}

// File system utilities
export function ensureDirectoryExists(dirPath: string): void {
	if (!existsSync(dirPath)) {
		mkdirSync(dirPath, { recursive: true });
		console.log(`📁 Created directory: ${dirPath}`);
	} else {
		console.log(`📁 Directory exists: ${dirPath}`);
	}
}

export function findFiles(pattern: string, directory?: string): string[] {
	const searchDir = directory || process.cwd();
	try {
		const result = execSync(`find "${searchDir}" -name "${pattern}" -type f 2>/dev/null || true`, { encoding: 'utf-8' }) as string;
		return result
			.split('\n')
			.filter(line => line.trim())
			.map(line => line.trim());
	} catch (error) {
		console.warn(`Warning: Could not search for files with pattern '${pattern}':`, error);
		return [];
	}
}

export function readJsonFile<T>(filePath: string): T {
	try {
		const content = readFileSync(filePath, 'utf-8');
		return JSON.parse(content) as T;
	} catch (error) {
		throw new Error(`Failed to read JSON file '${filePath}': ${error}`);
	}
}

export function writeJsonFile(filePath: string, data: any): void {
	try {
		const content = JSON.stringify(data, null, 2);
		writeFileSync(filePath, content, 'utf-8');
		console.log(`💾 Written JSON file: ${filePath}`);
	} catch (error) {
		throw new Error(`Failed to write JSON file '${filePath}': ${error}`);
	}
}

// Metadata file location detection
export function findDomainModelMetadata(): string {
	const { projectRoot } = detectProjectInfo();

	// Common locations for domain model metadata
	const possiblePaths = [
		join(projectRoot, 'orgModel/01 - Program Request Management/domain-model-metadata.json'),
		join(projectRoot, 'projects/01 - Program Request Management/artifacts/Analysis/domain-model-metadata.json'),
		join(projectRoot, 'domain-model-metadata.json')
	];

	for (const path of possiblePaths) {
		if (existsSync(path)) {
			console.log(`📋 Found domain model metadata: ${path}`);
			return path;
		}
	}

	throw new Error('Could not find domain-model-metadata.json file in expected locations');
}
