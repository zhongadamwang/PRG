// @ts-ignore
import { existsSync } from 'node:fs';
// @ts-ignore
import { dirname } from 'node:path';
// @ts-ignore
import { execSync } from 'node:child_process';

// Import shared utilities
import {
	detectProjectInfo
} from '../../../utilities/project-utilities/scripts/utilities';

// @ts-ignore
const process = globalThis.process;

interface FormattingResult {
	success: boolean;
	command: string;
	output?: string;
	error?: string;
	duration: number;
}

interface FormattingSummary {
	totalCommands: number;
	successfulCommands: number;
	failedCommands: number;
	commands: FormattingResult[];
	overallSuccess: boolean;
	totalDuration: number;
}

/**
 * Execute a dotnet format command with error handling
 */
function executeFormatCommand(command: string, workingDir: string): FormattingResult {
	const startTime = Date.now();
	console.log(`🧹 Executing: ${command}`);

	try {
		const output = execSync(command, {
			cwd: workingDir,
			encoding: 'utf-8',
			timeout: 120000, // 2 minutes timeout
			stdio: ['ignore', 'pipe', 'pipe']
		});

		const duration = Date.now() - startTime;
		console.log(`✅ Command completed successfully in ${duration}ms`);

		return {
			success: true,
			command,
			output: output.toString(),
			duration
		};
	} catch (error: any) {
		const duration = Date.now() - startTime;
		console.log(`⚠️  Command failed after ${duration}ms: ${error.message}`);

		return {
			success: false,
			command,
			error: error.message,
			duration
		};
	}
}

/**
 * Format entire solution using dotnet format commands
 */
function formatSolution(solutionPath?: string): FormattingSummary {
	console.log('🚀 Starting solution code formatting...');

	const startTime = Date.now();
	let targetSolutionPath: string;
	let workingDirectory: string;

	// Detect solution path if not provided
	if (solutionPath && existsSync(solutionPath)) {
		targetSolutionPath = solutionPath;
		workingDirectory = dirname(solutionPath);
	} else {
		try {
			const { projectRoot, slnxFile } = detectProjectInfo();
			targetSolutionPath = slnxFile;
			workingDirectory = projectRoot;
		} catch (error) {
			console.log('❌ Failed to detect project structure');
			return {
				totalCommands: 0,
				successfulCommands: 0,
				failedCommands: 0,
				commands: [],
				overallSuccess: false,
				totalDuration: Date.now() - startTime
			};
		}
	}

	console.log(`📁 Working directory: ${workingDirectory}`);
	console.log(`📄 Solution file: ${targetSolutionPath}`);

	// Prepare formatting commands
	const formatCommands = [
		`dotnet build "${targetSolutionPath}"`,
		`dotnet format "${targetSolutionPath}"`,
		`dotnet format style "${targetSolutionPath}"`
	];

	const results: FormattingResult[] = [];

	// Execute each formatting command
	for (const command of formatCommands) {
		const result = executeFormatCommand(command, workingDirectory);
		results.push(result);
	}

	// Calculate summary
	const successfulCommands = results.filter(r => r.success).length;
	const failedCommands = results.filter(r => !r.success).length;
	const overallSuccess = failedCommands === 0;
	const totalDuration = Date.now() - startTime;

	// Print summary
	console.log('\n📊 Formatting Summary:');
	console.log(`├── Total commands: ${results.length}`);
	console.log(`├── Successful: ${successfulCommands}`);
	console.log(`├── Failed: ${failedCommands}`);
	console.log(`├── Overall status: ${overallSuccess ? '✅ Success' : '⚠️  Partial success'}`);
	console.log(`└── Total duration: ${totalDuration}ms\n`);

	// Print detailed results if there were failures
	if (failedCommands > 0) {
		console.log('⚠️  Failed commands:');
		results
			.filter(r => !r.success)
			.forEach((result, index) => {
				console.log(`   ${index + 1}. ${result.command}`);
				console.log(`      Error: ${result.error}`);
			});
		console.log('');
	}

	return {
		totalCommands: results.length,
		successfulCommands,
		failedCommands,
		commands: results,
		overallSuccess,
		totalDuration
	};
}

/**
 * Format specific projects (selective formatting)
 */
function formatProjects(projectPaths: string[]): FormattingSummary {
	console.log('🚀 Starting selective project formatting...');

	const startTime = Date.now();
	const results: FormattingResult[] = [];

	for (const projectPath of projectPaths) {
		if (!existsSync(projectPath)) {
			console.log(`⚠️  Project not found: ${projectPath}`);
			results.push({
				success: false,
				command: `format "${projectPath}"`,
				error: 'Project file not found',
				duration: 0
			});
			continue;
		}

		const projectDir = dirname(projectPath);
		const formatCommands = [
			`dotnet build "${projectPath}"`,
			`dotnet format "${projectPath}"`,
			`dotnet format style "${projectPath}"`
		];

		for (const command of formatCommands) {
			const result = executeFormatCommand(command, projectDir);
			results.push(result);
		}
	}

	// Calculate summary
	const successfulCommands = results.filter(r => r.success).length;
	const failedCommands = results.filter(r => !r.success).length;
	const overallSuccess = failedCommands === 0;
	const totalDuration = Date.now() - startTime;

	console.log('\n📊 Project Formatting Summary:');
	console.log(`├── Total commands: ${results.length}`);
	console.log(`├── Successful: ${successfulCommands}`);
	console.log(`├── Failed: ${failedCommands}`);
	console.log(`├── Overall status: ${overallSuccess ? '✅ Success' : '⚠️  Partial success'}`);
	console.log(`└── Total duration: ${totalDuration}ms\n`);

	return {
		totalCommands: results.length,
		successfulCommands,
		failedCommands,
		commands: results,
		overallSuccess,
		totalDuration
	};
}

/**
 * Main execution function
 */
function main() {
	console.log('🎨 Solution Code Formatter');
	console.log('==========================\n');

	const args = process.argv.slice(2);

	if (args.length === 0) {
		// Format entire solution
		const result = formatSolution();
		process.exit(result.overallSuccess ? 0 : 1);
	} else if (args[0] === '--help' || args[0] === '-h') {
		// Show help
		console.log('Usage:');
		console.log('  bun run format-solution.ts                    # Format entire solution');
		console.log('  bun run format-solution.ts [solution-path]    # Format specific solution');
		console.log('  bun run format-solution.ts --projects [paths] # Format specific projects');
		console.log('  bun run format-solution.ts --help             # Show this help\n');

		console.log('Examples:');
		console.log('  bun run format-solution.ts');
		console.log('  bun run format-solution.ts src/MyProject.slnx');
		console.log('  bun run format-solution.ts --projects src/Project1/Project1.csproj src/Project2/Project2.csproj');
		process.exit(0);
	} else if (args[0] === '--projects') {
		// Format specific projects
		const projectPaths = args.slice(1);
		if (projectPaths.length === 0) {
			console.log('❌ No project paths provided after --projects');
			process.exit(1);
		}

		const result = formatProjects(projectPaths);
		process.exit(result.overallSuccess ? 0 : 1);
	} else {
		// Format specific solution file
		const solutionPath = args[0];
		const result = formatSolution(solutionPath);
		process.exit(result.overallSuccess ? 0 : 1);
	}
}

// Run the script
// @ts-ignore
if (require.main === module) {
	main();
}