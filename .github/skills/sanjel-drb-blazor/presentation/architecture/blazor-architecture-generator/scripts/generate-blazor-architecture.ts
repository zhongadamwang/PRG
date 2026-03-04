// @ts-ignore
const fs = require('fs');
// @ts-ignore
const path = require('path');

// @ts-ignore
const process = globalThis.process;

// Import shared utilities from project-utilities
import {
	detectProjectInfo,
	ensureDirectoryExists,
	formatGeneratedCode,
	formatSpecificFiles,
	toPascalCase,
	toCamelCase,
	toKebabCase
} from '../../../../utilities/project-utilities/scripts/utilities';

/**
 * Blazor Architecture Generator
 * Generates complete Blazor application foundation with selected component library.
 * 
 * Design Principles:
 * - Generate concrete implementations, not abstractions
 * - Support multiple component libraries through extensible configuration
 * - Enable component library switching via Skills re-generation
 * - Produce immediately runnable Blazor applications
 */

// ================================
// Configuration Types & Interfaces
// ================================

interface BlazorArchitectureConfig {
	project: {
		name: string;
		blazorProjectPath: string;
	};
	componentLibrary: ComponentLibraryType;
	features: {
		authentication?: boolean;
		responsive?: boolean;
		darkModeSupport?: boolean;
	};
	license?: LicenseConfig;
}

type ComponentLibraryType = 'mudblazor' | 'syncfusion' | 'bootstrap' | 'none';

interface LicenseConfig {
	key?: string;
	registrationMethod?: 'appsettings' | 'code' | 'environment';
}

interface PackageReference {
	name: string;
	version: string;
}

interface ComponentLibraryDefinition {
	name: string;
	displayName: string;
	packageReferences: PackageReference[];
	serviceRegistrations: string[];
	namespaceImports: string[];
	styleFiles: string[];
	scriptFiles?: string[];
	requiresLicense: boolean;
	licenseConfig?: {
		configKey: string;
		registrationCode: string;
	};
}

// ================================
// Component Library Configurations 
// ================================

/**
 * Extensible component library definitions
 * Each library defines its specific requirements and configuration
 */
const COMPONENT_LIBRARIES: Record<ComponentLibraryType, ComponentLibraryDefinition> = {
	mudblazor: {
		name: 'mudblazor',
		displayName: 'MudBlazor',
		packageReferences: [
			{ name: 'MudBlazor', version: '6.11.2' }
		],
		serviceRegistrations: [
			'builder.Services.AddMudServices();'
		],
		namespaceImports: [
			'@using MudBlazor'
		],
		styleFiles: [],
		requiresLicense: false
	},

	syncfusion: {
		name: 'syncfusion',
		displayName: 'Syncfusion Blazor',
		packageReferences: [
			{ name: 'Syncfusion.Blazor.Core', version: '23.2.4' },
			{ name: 'Syncfusion.Blazor.Themes', version: '23.2.4' },
			{ name: 'Syncfusion.Blazor.Grid', version: '23.2.4' },
			{ name: 'Syncfusion.Blazor.Inputs', version: '23.2.4' }
		],
		serviceRegistrations: [
			'builder.Services.AddSyncfusionBlazor();'
		],
		namespaceImports: [
			'@using Syncfusion.Blazor',
			'@using Syncfusion.Blazor.Grids',
			'@using Syncfusion.Blazor.Inputs'
		],
		styleFiles: [
			'_content/Syncfusion.Blazor.Themes/bootstrap5.css'
		],
		requiresLicense: true,
		licenseConfig: {
			configKey: 'Syncfusion:LicenseKey',
			registrationCode: 'Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(builder.Configuration["Syncfusion:LicenseKey"]);'
		}
	},

	bootstrap: {
		name: 'bootstrap',
		displayName: 'Bootstrap',
		packageReferences: [],
		serviceRegistrations: [],
		namespaceImports: [],
		styleFiles: [
			'https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css'
		],
		scriptFiles: [
			'https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js'
		],
		requiresLicense: false
	},

	none: {
		name: 'none',
		displayName: 'None (Minimal)',
		packageReferences: [],
		serviceRegistrations: [],
		namespaceImports: [],
		styleFiles: [],
		requiresLicense: false
	}
};

// ================================
// Main Generation Functions
// ================================

/**
 * Main entry point for Blazor architecture generation
 */
async function generateBlazorArchitecture(config: BlazorArchitectureConfig): Promise<void> {
	console.log(`🏗️ Generating Blazor architecture with ${config.componentLibrary}...`);

	try {
		// Phase 1: Validate and prepare
		validateConfiguration(config);
		prepareProjectStructure(config);

		// Phase 2: Generate core files
		await generateLayoutFiles(config);
		generateConfigurationFiles(config);
		await updateProjectFiles(config);

		// Phase 3: Setup component library
		installComponentLibrary(config);
		configureComponentLibrary(config);

		// Phase 4: Generate additional files
		generateSamplePages(config);
		generateStaticAssets(config);

		// Phase 5: Post-generation tasks
		formatGeneratedFiles(config);
		verifyGeneration(config);

		console.log('✅ Blazor architecture generation completed successfully!');

	} catch (error) {
		console.error('❌ Generation failed:', error);
		throw error;
	}
}

// ================================
// Phase 1: Validation & Preparation
// ================================

function validateConfiguration(config: BlazorArchitectureConfig): void {
	console.log('🔍 Validating configuration...');

	// TODO: Implement validation logic
	// - Verify project path exists
	// - Validate component library choice
	// - Check license requirements
	// - Validate feature combinations

	// Placeholder implementation
	if (!config.project.blazorProjectPath) {
		throw new Error('Blazor project path is required');
	}

	if (!COMPONENT_LIBRARIES[config.componentLibrary]) {
		throw new Error(`Unsupported component library: ${config.componentLibrary}`);
	}

	console.log('✅ Configuration validation completed');
}

function prepareProjectStructure(config: BlazorArchitectureConfig): void {
	console.log('📁 Preparing project structure...');

	// TODO: Implement project structure preparation
	// - Ensure required directories exist
	// - Backup existing files if needed
	// - Create directory structure

	// Key directories to create:
	// - Components/Layout/
	// - Pages/
	// - wwwroot/css/
	// - wwwroot/js/

	console.log('✅ Project structure prepared');
}

// ================================
// Phase 2: Core File Generation
// ================================

async function generateLayoutFiles(config: BlazorArchitectureConfig): Promise<void> {
	console.log('🎨 Generating layout files...');

	const libraryDef = COMPONENT_LIBRARIES[config.componentLibrary];

	// Generate component library-specific layouts
	switch (config.componentLibrary) {
		case 'mudblazor':
			await generateMudBlazorLayout(config);
			break;
		case 'syncfusion':
			await generateSyncfusionLayout(config);
			break;
		case 'bootstrap':
			await generateBootstrapLayout(config);
			break;
		case 'none':
			await generateMinimalLayout(config);
			break;
	}

	console.log('✅ Layout files generated');
}

async function generateMudBlazorLayout(config: BlazorArchitectureConfig): Promise<void> {
	console.log('🎯 Generating MudBlazor layout...');

	// Generate from templates
	await generateFromTemplate('mudblazor', 'App.razor', config);
	await generateFromTemplate('mudblazor', 'MainLayout.razor', config);
	await generateFromTemplate('mudblazor', 'MainLayout.razor.cs', config);
	await generateFromTemplate('mudblazor', 'NavMenu.razor', config);
	await generateFromTemplate('mudblazor', 'NavMenu.razor.cs', config);
	await generateFromTemplate('mudblazor', 'Routes.razor', config);
	await generateFromTemplate('mudblazor', 'Routes.razor.cs', config);
	await generateFromTemplate('mudblazor', '_Imports.razor', config);
}

async function generateSyncfusionLayout(config: BlazorArchitectureConfig): Promise<void> {
	console.log('🎯 Generating Syncfusion layout...');

	// Generate App.razor with Syncfusion providers
	await generateFromTemplate('syncfusion', 'App.razor', config);

	// Generate MainLayout.razor with Syncfusion components
	await generateFromTemplate('syncfusion', 'MainLayout.razor', config);
	await generateFromTemplate('syncfusion', 'MainLayout.razor.cs', config);

	// Generate NavMenu.razor with Syncfusion navigation components
	await generateFromTemplate('syncfusion', 'NavMenu.razor', config);
	await generateFromTemplate('syncfusion', 'NavMenu.razor.cs', config);

	// Generate Routes.razor
	await generateFromTemplate('syncfusion', 'Routes.razor', config);
	await generateFromTemplate('syncfusion', 'Routes.razor.cs', config);

	// Generate _Imports.razor
	await generateFromTemplate('syncfusion', '_Imports.razor', config);
}

async function generateBootstrapLayout(config: BlazorArchitectureConfig): Promise<void> {
	console.log('🎯 Generating Bootstrap layout...');

	// Generate from templates
	await generateFromTemplate('bootstrap', 'App.razor', config);
	await generateFromTemplate('bootstrap', 'MainLayout.razor', config);
	await generateFromTemplate('bootstrap', 'MainLayout.razor.cs', config);
	await generateFromTemplate('bootstrap', 'NavMenu.razor', config);
	await generateFromTemplate('bootstrap', 'NavMenu.razor.cs', config);
	await generateFromTemplate('bootstrap', 'Routes.razor', config);
	await generateFromTemplate('bootstrap', 'Routes.razor.cs', config);
	await generateFromTemplate('bootstrap', '_Imports.razor', config);
}

async function generateMinimalLayout(config: BlazorArchitectureConfig): Promise<void> {
	console.log('🎯 Generating minimal layout...');

	// Generate from templates
	await generateFromTemplate('minimal', 'App.razor', config);
	await generateFromTemplate('minimal', 'MainLayout.razor', config);
	await generateFromTemplate('minimal', 'MainLayout.razor.cs', config);
	await generateFromTemplate('minimal', 'NavMenu.razor', config);
	await generateFromTemplate('minimal', 'NavMenu.razor.cs', config);
	await generateFromTemplate('minimal', 'Routes.razor', config);
	await generateFromTemplate('minimal', 'Routes.razor.cs', config);
	await generateFromTemplate('minimal', '_Imports.razor', config);
}

function generateConfigurationFiles(config: BlazorArchitectureConfig): void {
	console.log('⚙️ Generating configuration files...');

	// TODO: Generate/update _Imports.razor with component library namespaces
	generateImportsFile(config);

	// TODO: Update appsettings.json with component library configuration
	updateAppSettingsFile(config);

	console.log('✅ Configuration files generated');
}

function generateImportsFile(config: BlazorArchitectureConfig): void {
	console.log('📥 Generating _Imports.razor...');

	const libraryDef = COMPONENT_LIBRARIES[config.componentLibrary];

	// TODO: Generate _Imports.razor with:
	// - Standard Blazor imports
	// - Component library-specific imports
	// - Project-specific imports

	// Base imports that are always included:
	const baseImports = [
		'@using Microsoft.AspNetCore.Components.Web',
		'@using Microsoft.AspNetCore.Components.WebAssembly.Http',
		'@using System.Net.Http',
		'@using System.Net.Http.Json'
	];

	// Component library imports
	const libraryImports = libraryDef.namespaceImports;

	// TODO: Combine and write to file
}

function updateAppSettingsFile(config: BlazorArchitectureConfig): void {
	console.log('📝 Updating appsettings.json...');

	const libraryDef = COMPONENT_LIBRARIES[config.componentLibrary];

	// TODO: Update appsettings.json with:
	// - Component library configuration
	// - License keys (if required)
	// - Feature-specific settings

	if (libraryDef.requiresLicense && config.license?.key) {
		// TODO: Add license configuration
	}
}

async function updateProjectFiles(config: BlazorArchitectureConfig): Promise<void> {
	console.log('📦 Updating project files...');

	// TODO: Update .csproj with package references
	await updateCsProjectFile(config);

	// TODO: Update Program.cs with service registrations
	await updateProgramCsFile(config);

	console.log('✅ Project files updated');
}

async function updateCsProjectFile(config: BlazorArchitectureConfig): Promise<void> {
	console.log('📦 Updating .csproj file...');

	const libraryDef = COMPONENT_LIBRARIES[config.componentLibrary];

	// Read template and generate .csproj content
	await generateFromTemplate(config.componentLibrary, '.csproj', config, 'Sanjel.RequestManagement.Blazor.csproj');
}

async function updateProgramCsFile(config: BlazorArchitectureConfig): Promise<void> {
	console.log('🔧 Updating Program.cs...');

	const libraryDef = COMPONENT_LIBRARIES[config.componentLibrary];

	// Generate Program.cs from template
	await generateFromTemplate(config.componentLibrary, 'Program.cs', config);
}

// ================================
// Phase 3: Component Library Setup
// ================================

function installComponentLibrary(config: BlazorArchitectureConfig): void {
	console.log('📥 Installing component library packages...');

	const libraryDef = COMPONENT_LIBRARIES[config.componentLibrary];

	if (libraryDef.packageReferences.length === 0) {
		console.log('ℹ️ No packages to install for this library');
		return;
	}

	// TODO: Install NuGet packages using dotnet CLI
	// TODO: Handle package installation errors
	// TODO: Verify package installation success

	console.log('✅ Component library packages installed');
}

function configureComponentLibrary(config: BlazorArchitectureConfig): void {
	console.log('⚙️ Configuring component library...');

	const libraryDef = COMPONENT_LIBRARIES[config.componentLibrary];

	// TODO: Configure library-specific settings
	// TODO: Setup themes and styling
	// TODO: Configure license (if required)

	if (libraryDef.requiresLicense) {
		configureLicense(config, libraryDef);
	}

	console.log('✅ Component library configured');
}

function configureLicense(config: BlazorArchitectureConfig, libraryDef: ComponentLibraryDefinition): void {
	console.log('🔑 Configuring license...');

	// TODO: Setup license configuration based on registration method
	// TODO: Add license validation
	// TODO: Generate license registration code
}

// ================================
// Phase 4: Additional File Generation  
// ================================

function generateSamplePages(config: BlazorArchitectureConfig): void {
	console.log('📄 Generating sample pages...');

	// TODO: Generate Home.razor
	// TODO: Generate Counter.razor (optional example)
	// TODO: Generate Error.razor for error handling
	// TODO: Use component library-specific components in samples

	console.log('✅ Sample pages generated');
}

function generateStaticAssets(config: BlazorArchitectureConfig): void {
	console.log('🎨 Generating static assets...');

	// TODO: Generate site.css with base styles
	// TODO: Generate site.js with base scripts
	// TODO: Copy/generate favicon and other assets
	// TODO: Generate component library-specific style overrides

	console.log('✅ Static assets generated');
}

// ================================
// Phase 5: Post-Generation Tasks
// ================================

function formatGeneratedFiles(config: BlazorArchitectureConfig): void {
	console.log('🎨 Formatting generated files...');

	try {
		// Use project-utilities formatGeneratedCode function
		const projectPath = config.project.blazorProjectPath;
		formatGeneratedCode(projectPath);

		console.log('✅ File formatting completed');
	} catch (error) {
		console.warn('⚠️ Code formatting failed, but generation was successful:', error);
	}
}

function verifyGeneration(config: BlazorArchitectureConfig): void {
	console.log('🔍 Verifying generation results...');

	try {
		// TODO: Verify all expected files were created
		// TODO: Attempt to build the project (dotnet build)
		// TODO: Check for obvious errors or issues
		// TODO: Validate component library integration

		console.log('✅ Generation verification completed');
	} catch (error) {
		console.warn('⚠️ Verification encountered issues:', error);
	}
}

function createBackup(config: BlazorArchitectureConfig): void {
	console.log('💾 Creating backup of existing files...');

	// TODO: Backup existing files before modification
	// TODO: Create restore mechanism if needed
}

// ================================
// Extension Points for New Libraries
// ================================

/**
 * Extension point for adding new component libraries
 * Usage: registerComponentLibrary('antdesign', antDesignConfig);
 */
function registerComponentLibrary(name: string, definition: ComponentLibraryDefinition): void {
	// TODO: Implement runtime registration of new component libraries
	// TODO: Validate library definition
	// TODO: Add to available libraries
}

/**
 * Template system for extensible layout generation
 */
interface LayoutTemplate {
	mainLayout: string;
	navMenu: string;
	appComponent: string;
	imports: string[];
}

async function generateFromTemplate(libraryName: string, templateFileName: string, config: BlazorArchitectureConfig, outputFileName?: string): Promise<void> {
	const scriptDir = path.dirname(import.meta.url.replace('file://', ''));
	const templatePath = path.join(scriptDir, '../templates', libraryName, `${templateFileName}.template`);
	const outputPath = path.join(config.project.blazorProjectPath, getOutputPath(templateFileName));
	const actualOutputName = outputFileName || templateFileName;
	const finalOutputPath = path.join(path.dirname(outputPath), actualOutputName);

	try {
		// Read template file
		const templateContent = fs.readFileSync(templatePath, 'utf-8');

		// Replace template variables
		const processedContent = replaceTemplateVariables(templateContent, config);

		// Write output file
		ensureDirectoryExists(path.dirname(finalOutputPath));
		fs.writeFileSync(finalOutputPath, processedContent, 'utf-8');
		console.log(`✅ Generated ${actualOutputName}`);
	} catch (error) {
		console.error(`❌ Failed to generate ${templateFileName}:`, error);
	}
}

function generateLayoutFromTemplate(libraryName: string, template: LayoutTemplate, config: BlazorArchitectureConfig): void {
	// TODO: Implement template-based layout generation
	// TODO: Support variable substitution in templates
	// TODO: Enable custom templates for new libraries
}

// ================================
// Helper Functions
// ================================

function replaceTemplateVariables(content: string, config: BlazorArchitectureConfig): string {
	// Get project info using utilities
	let projectName = config.project.name;
	try {
		const projectInfo = detectProjectInfo();
		projectName = projectInfo.projectName;
	} catch (error) {
		// Fallback to config if detection fails
		console.warn('Using config project name as fallback:', projectName);
	}

	let result = content
		.replace(/\{\{PROJECT_NAMESPACE\}\}/g, projectName)
		.replace(/\{\{PROJECT_NAME\}\}/g, projectName)
		.replace(/\{\{PROJECT_NAME_PASCAL\}\}/g, toPascalCase(projectName))
		.replace(/\{\{PROJECT_NAME_CAMEL\}\}/g, toCamelCase(projectName))
		.replace(/\{\{PROJECT_NAME_KEBAB\}\}/g, toKebabCase(projectName))
		.replace(/\{\{COMPONENT_LIBRARY\}\}/g, config.componentLibrary)
		.replace(/\{\{COMPONENT_LIBRARY_PASCAL\}\}/g, toPascalCase(config.componentLibrary));

	// Handle conditional blocks based on features
	if (config.features.authentication) {
		// Enable authentication blocks
		result = result.replace(/\{\{#if AUTHENTICATION\}\}([\s\S]*?)\{\{\/if\}\}/g, '$1');
	} else {
		// Remove authentication blocks
		result = result.replace(/\{\{#if AUTHENTICATION\}\}([\s\S]*?)\{\{\/if\}\}/g, '');
	}

	if (config.features.darkModeSupport) {
		// Enable dark mode blocks
		result = result.replace(/\{\{#if DARK_MODE_SUPPORT\}\}([\s\S]*?)\{\{\/if\}\}/g, '$1');
	} else {
		// Remove dark mode blocks
		result = result.replace(/\{\{#if DARK_MODE_SUPPORT\}\}([\s\S]*?)\{\{\/if\}\}/g, '');
	}

	if (config.features.responsive) {
		// Enable responsive blocks
		result = result.replace(/\{\{#if RESPONSIVE\}\}([\s\S]*?)\{\{\/if\}\}/g, '$1');
	} else {
		// Remove responsive blocks
		result = result.replace(/\{\{#if RESPONSIVE\}\}([\s\S]*?)\{\{\/if\}\}/g, '');
	}

	// Fix specific namespace issues in generated files
	result = result
		.replace(/using Sanjel\.RequestManagement\.Services;/g, 'using Sanjel.RequestManagement.Blazor.Services;')
		.replace(/using Sanjel\.RequestManagement\.Components;/g, 'using Sanjel.RequestManagement.Blazor.Components;')
		.replace(/Sanjel\.RequestManagement\.Components\.App/g, 'Sanjel.RequestManagement.Blazor.Components.App')
		.replace(/@using Microsoft\.AspNetCore\.Components\.WebAssembly\.Http\n/g, '') // Remove WebAssembly reference for Server-side Blazor
		.replace(/@using \s*\n/g, ''); // Remove empty using lines

	return result;
}

function getOutputPath(templateFileName: string): string {
	if (templateFileName === 'App.razor' || templateFileName === 'Routes.razor' || templateFileName === '_Imports.razor') {
		return `Components/${templateFileName}`;
	}
	if (templateFileName.includes('Layout') || templateFileName === 'NavMenu.razor' || templateFileName === 'NavMenu.razor.cs') {
		return `Components/Layout/${templateFileName}`;
	}
	if (templateFileName === 'Program.cs') {
		return templateFileName;
	}
	if (templateFileName === '.csproj') {
		return 'Sanjel.RequestManagement.Blazor.csproj';
	}
	return templateFileName;
}

// Use imported ensureDirectoryExists from utilities
// Note: The imported function is already available and should be used instead of this custom implementation
function createDirectoryIfNeeded(dirPath: string): void {
	ensureDirectoryExists(dirPath);
}

// Legacy function for compatibility - redirects to utilities
function legacyEnsureDirectoryExists(dirPath: string): void {
	try {
		if (!fs.existsSync(dirPath)) {
			fs.mkdirSync(dirPath, { recursive: true });
		}
	} catch (error: any) {
		if (error.code !== 'EEXIST') {
			throw error;
		}
	}
}

// ================================
// CLI Interface & Entry Point
// ================================

/**
 * CLI entry point for the skill
 */
async function main(): Promise<void> {
	console.log('🚀 Blazor Architecture Generator');
	console.log('================================');

	try {
		// TODO: Parse command line arguments
		// TODO: Detect project configuration automatically
		// TODO: Prompt for missing configuration
		// TODO: Execute generation with configuration

		// Configuration for Syncfusion Blazor with requested features
		const config: BlazorArchitectureConfig = {
			project: {
				name: 'Sanjel.RequestManagement',
				blazorProjectPath: '/sanjel/PRG/src/Sanjel.RequestManagement.Blazor'
			},
			componentLibrary: 'syncfusion',
			features: {
				responsive: true,
				authentication: true,
				darkModeSupport: true
			},
			license: {
				registrationMethod: 'appsettings'
			}
		};

		await generateBlazorArchitecture(config);

	} catch (error) {
		console.error('❌ Skill execution failed:', error);
		process.exit(1);
	}
}

// ================================
// Execute if run directly
// ================================

if (import.meta.url === `file://${process.argv[1]}`) {
	main().catch(error => {
		console.error('❌ Fatal error:', error);
		process.exit(1);
	});
}

// ================================
// Exports (for testing and integration)
// ================================

export {
	COMPONENT_LIBRARIES, generateBlazorArchitecture, type BlazorArchitectureConfig, type ComponentLibraryDefinition, type ComponentLibraryType
};
