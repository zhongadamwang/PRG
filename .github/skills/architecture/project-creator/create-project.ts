#!/usr/bin/env bun

// @ts-ignore
import * as fs from 'node:fs';
// @ts-ignore
import * as path from 'node:path';
// @ts-ignore
import process from 'node:process';

interface ProjectConfig {
	projectName: string;
	projectPrefix: string;
	targetDirectory: string;
	fullProjectName: string;
}

class ProjectCreator {
	// @ts-ignore
	private readonly templatesDir = path.join(__dirname, 'templates');

	constructor() {
		console.log('\n🏗️  eServiceCloud Blazor Project Creator 🏗️\n');
	}

	async createProject(projectName: string, projectPrefix: string, targetDirectory: string) {
		try {
			// Validate templates directory exists
			if (!fs.existsSync(this.templatesDir)) {
				throw new Error(`Templates directory not found: ${this.templatesDir}`);
			}

			// Parse arguments into config
			const config = this.parseArguments(projectName, projectPrefix, targetDirectory);

			// Validate input
			this.validateConfig(config);

			// Create project
			console.log('\n📋 Creating project structure...');
			await this.copyAndTransformTemplates(config);

			console.log('\n✅ Project created successfully!');
			console.log(`\n📁 Project location: ${config.targetDirectory}`);
			console.log(`\n🚀 Next steps:`);
			console.log(`   cd ${config.targetDirectory}`);
			console.log(`   bun run ./run.sh`);

		} catch (error) {
			console.error('\n❌ Error creating project:', (error as Error).message);
			// @ts-ignore
			process.exit(1);
		}
	}

	private parseArguments(projectName: string, projectPrefix: string, targetDirectory: string): ProjectConfig {
		return {
			projectName: projectName.trim(),
			projectPrefix: projectPrefix.trim(),
			targetDirectory: path.resolve(targetDirectory.trim()),
			fullProjectName: `${projectPrefix.trim()}.${projectName.trim()}`
		};
	}

	private validateConfig(config: ProjectConfig): void {
		if (!config.projectName) {
			throw new Error('Project name is required');
		}

		if (!config.projectPrefix) {
			throw new Error('Project prefix is required');
		}

		if (!config.targetDirectory) {
			throw new Error('Target directory is required');
		}

		// Validate project name format
		if (!/^[A-Za-z][A-Za-z0-9]*$/.test(config.projectName)) {
			throw new Error('Project name must start with a letter and contain only letters and numbers');
		}

		// Validate prefix format
		if (!/^[A-Za-z][A-Za-z0-9]*$/.test(config.projectPrefix)) {
			throw new Error('Project prefix must start with a letter and contain only letters and numbers');
		}
	}

	private async copyAndTransformTemplates(config: ProjectConfig): Promise<void> {
		// Create target directory
		fs.mkdirSync(config.targetDirectory, { recursive: true });

		// Process templates directory
		await this.processDirectory(this.templatesDir, config.targetDirectory, config);

		console.log('✅ Template processing completed');
	}

	private async processDirectory(sourceDir: string, targetDir: string, config: ProjectConfig): Promise<void> {
		const items = fs.readdirSync(sourceDir);

		for (const item of items) {
			const sourcePath = path.join(sourceDir, item);
			const targetPath = path.join(targetDir, this.transformName(item, config));

			const stat = fs.statSync(sourcePath);

			if (stat.isDirectory()) {
				// Create directory and process contents
				fs.mkdirSync(targetPath, { recursive: true });
				await this.processDirectory(sourcePath, targetPath, config);
			} else {
				// Copy and transform file
				await this.processFile(sourcePath, targetPath, config);
			}
		}
	}

	private transformName(name: string, config: ProjectConfig): string {
		// Replace Prg.ProjectName with actual project name
		return name.replace(/Prg\.ProjectName/g, config.fullProjectName);
	}

	private async processFile(sourcePath: string, targetPath: string, config: ProjectConfig): Promise<void> {
		try {
			// Read source file
			const content = fs.readFileSync(sourcePath, 'utf8');

			// Transform content
			const transformedContent = this.transformContent(content, config);

			// Write to target
			fs.writeFileSync(targetPath, transformedContent, 'utf8');

			// Copy file permissions
			const stats = fs.statSync(sourcePath);
			fs.chmodSync(targetPath, stats.mode);

		} catch (error) {
			// For binary files or files that can't be read as UTF-8, just copy them
			try {
				fs.copyFileSync(sourcePath, targetPath);
			} catch (copyError) {
				console.warn(`Warning: Could not process file ${sourcePath}: ${(copyError as Error).message}`);
			}
		}
	}

	private transformContent(content: string, config: ProjectConfig): string {
		// Replace all instances of Prg.ProjectName with the actual project name
		let transformed = content.replace(/Prg\.ProjectName/g, config.fullProjectName);

		// Also replace any path separators in project references
		transformed = transformed.replace(/Prg\.ProjectName/g, config.fullProjectName);

		return transformed;
	}
}

// Main execution
async function main(): Promise<void> {
	// @ts-ignore
	const args = process.argv.slice(2) as string[];

	if (args.length !== 3) {
		console.error('Usage: bun run create-project.ts <projectName> <projectPrefix> <targetDirectory>');
		console.error('Example: bun run create-project.ts RequestManagement Prg /path/to/projects');
		// @ts-ignore
		process.exit(1);
	}

	const [projectName, projectPrefix, targetDirectory] = args;

	const creator = new ProjectCreator();
	await creator.createProject(projectName, projectPrefix, targetDirectory);
}

// Run if this file is executed directly
// @ts-ignore
if (import.meta.main) {
	main().catch(error => {
		console.error('Fatal error:', (error as Error).message);
		// @ts-ignore
		process.exit(1);
	});
}