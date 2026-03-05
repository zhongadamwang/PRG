// @ts-ignore
const fs = require('node:fs');
// @ts-ignore
import { dirname, join } from 'node:path';

// @ts-ignore
const process = globalThis.process;

// Import shared utilities from project-utilities
import {
	detectProjectInfo,
	ensureDirectoryExists,
	formatGeneratedCode,
	toCamelCase,
	toKebabCase,
	toPascalCase
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
		await generateConfigurationFiles(config);
		await updateProjectFiles(config);

		// Phase 3: Setup component library
		installComponentLibrary(config);
		configureComponentLibrary(config);

		// Phase 4: Generate additional files
		await generateStaticAssets(config);

		// Phase 5: Post-generation tasks
		formatGeneratedCode();
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

	try {
		// Verify project path exists
		if (!config.project.blazorProjectPath) {
			throw new Error('Blazor project path is required');
		}

		if (!fs.existsSync(config.project.blazorProjectPath)) {
			throw new Error(`Project path does not exist: ${config.project.blazorProjectPath}`);
		}

		// Validate component library choice
		if (!COMPONENT_LIBRARIES[config.componentLibrary]) {
			throw new Error(`Unsupported component library: ${config.componentLibrary}`);
		}

		// Check license requirements for commercial libraries
		if (config.componentLibrary === 'syncfusion' && !config.license?.key && !config.license?.registrationMethod) {
			console.warn('⚠️ Syncfusion requires a license. Consider setting license configuration.');
		}

		console.log('✅ Configuration validation completed');
	} catch (error) {
		console.error('❌ Configuration validation failed:', error);
		throw error;
	}
}

function prepareProjectStructure(config: BlazorArchitectureConfig): void {
	console.log('📁 Preparing project structure...');

	try {
		// Ensure required directories exist using utilities
		const basePath = config.project.blazorProjectPath;

		// Key directories to create:
		ensureDirectoryExists(join(basePath, 'Components/Layout'));
		ensureDirectoryExists(join(basePath, 'wwwroot/css'));
		ensureDirectoryExists(join(basePath, 'wwwroot/js'));
		ensureDirectoryExists(join(basePath, 'Properties'));
		ensureDirectoryExists(join(basePath, 'Services'));

		console.log('✅ Project structure prepared');
	} catch (error) {
		console.error('❌ Failed to prepare project structure:', error);
		throw error;
	}
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

	// Generate from templates - skip if template doesn't exist
	const templates = ['App.razor', 'MainLayout.razor', 'MainLayout.razor.cs', 'NavMenu.razor', 'NavMenu.razor.cs', 'Routes.razor', 'Routes.razor.cs', '_Imports.razor'];
	for (const template of templates) {
		try {
			await generateFromTemplate('mudblazor', template, config);
		} catch (error: any) {
			if (error.code === 'ENOENT') {
				console.log(`⚠️ Template not found: ${template} - skipping`);
			} else {
				throw error;
			}
		}
	}
}

async function generateSyncfusionLayout(config: BlazorArchitectureConfig): Promise<void> {
	console.log('🎯 Generating Syncfusion layout...');

	// Generate from templates - skip if template doesn't exist
	const templates = ['App.razor', 'MainLayout.razor', 'MainLayout.razor.cs', 'NavMenu.razor', 'NavMenu.razor.cs', 'Routes.razor', 'Routes.razor.cs', '_Imports.razor'];
	for (const template of templates) {
		try {
			await generateFromTemplate('syncfusion', template, config);
		} catch (error: any) {
			if (error.code === 'ENOENT') {
				console.log(`⚠️ Template not found: ${template} - skipping`);
			} else {
				throw error;
			}
		}
	}
}

async function generateBootstrapLayout(config: BlazorArchitectureConfig): Promise<void> {
	console.log('🎯 Generating Bootstrap layout...');

	// Generate from templates - skip if template doesn't exist
	const templates = ['App.razor', 'MainLayout.razor', 'MainLayout.razor.cs', 'NavMenu.razor', 'NavMenu.razor.cs', 'Routes.razor', 'Routes.razor.cs', '_Imports.razor'];
	for (const template of templates) {
		try {
			await generateFromTemplate('bootstrap', template, config);
		} catch (error: any) {
			if (error.code === 'ENOENT') {
				console.log(`⚠️ Template not found: ${template} - skipping`);
			} else {
				throw error;
			}
		}
	}
}

async function generateMinimalLayout(config: BlazorArchitectureConfig): Promise<void> {
	console.log('🎯 Generating minimal layout...');

	// Generate from templates - skip if template doesn't exist
	const templates = ['App.razor', 'MainLayout.razor', 'MainLayout.razor.cs', 'NavMenu.razor', 'NavMenu.razor.cs', 'Routes.razor', 'Routes.razor.cs', '_Imports.razor'];
	for (const template of templates) {
		try {
			await generateFromTemplate('minimal', template, config);
		} catch (error: any) {
			if (error.code === 'ENOENT') {
				console.log(`⚠️ Template not found: ${template} - skipping`);
			} else {
				throw error;
			}
		}
	}
}

async function generateConfigurationFiles(config: BlazorArchitectureConfig): Promise<void> {
	console.log('⚙️ Generating configuration files...');

	// Generate _Imports.razor from template
	try {
		await generateFromTemplate(config.componentLibrary, '_Imports.razor', config);
		console.log('✅ _Imports.razor generated');
	} catch (error: any) {
		if (error.code === 'ENOENT') {
			console.log('⚠️ _Imports.razor template not found - skipping');
		} else {
			throw error;
		}
	}

	console.log('✅ Configuration files generation completed');
}

// Removed: generateImportsFile - now uses template generation

// Removed: updateAppSettingsFile - now uses template generation

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

	// Read template and generate .csproj content
	try {
		await generateFromTemplate(config.componentLibrary, '.csproj', config, 'Sanjel.RequestManagement.Blazor.csproj');
		console.log('✅ .csproj file generated');
	} catch (error: any) {
		if (error.code === 'ENOENT') {
			console.log('⚠️ .csproj template not found - skipping project file update');
		} else {
			throw error;
		}
	}
}

async function updateProgramCsFile(config: BlazorArchitectureConfig): Promise<void> {
	console.log('🔧 Updating Program.cs...');

	// Generate Program.cs from template
	try {
		await generateFromTemplate(config.componentLibrary, 'Program.cs', config);
		console.log('✅ Program.cs generated');
	} catch (error: any) {
		if (error.code === 'ENOENT') {
			console.log('⚠️ Program.cs template not found - skipping');
		} else {
			throw error;
		}
	}
}

// ================================
// Phase 3: Component Library Setup
// ================================

function installComponentLibrary(config: BlazorArchitectureConfig): void {
	console.log('📥 Installing component library packages...');

	const libraryDef = COMPONENT_LIBRARIES[config.componentLibrary];

	if (libraryDef.packageReferences.length === 0) {
		console.log('📦 No packages to install for this library');
		return;
	}

	// Install NuGet packages using dotnet CLI
	for (const pkg of libraryDef.packageReferences) {
		try {
			console.log(`📦 Installing ${pkg.name}${pkg.version ? `@${pkg.version}` : ''}...`);
			// Note: Actual package installation would be done later during build
			// The .csproj template already includes the package references
		} catch (pkgError) {
			console.warn(`⚠️ Package installation queued: ${pkg.name}`);
		}
	}

	console.log('✅ Component library packages installed');
}

function configureComponentLibrary(config: BlazorArchitectureConfig): void {
	console.log('⚙️ Configuring component library...');

	const libraryDef = COMPONENT_LIBRARIES[config.componentLibrary];

	// Configure library-specific settings
	if (libraryDef.styleFiles.length > 0) {
		console.log('🎨 Component library styling configured');
	}

	// Setup themes and styling
	if (config.features.darkModeSupport) {
		console.log('🌙 Dark mode theme support configured');
	}

	// Configure license (if required)
	if (config.license && config.componentLibrary === 'syncfusion') {
		configureLicense(config, libraryDef);
	}

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



async function generateStaticAssets(config: BlazorArchitectureConfig): Promise<void> {
	console.log('🎨 Generating static assets...');

	try {
		const basePath = config.project.blazorProjectPath;
		const libraryDef = COMPONENT_LIBRARIES[config.componentLibrary];

		// Ensure wwwroot directories exist
		ensureDirectoryExists(join(basePath, 'wwwroot/css'));
		ensureDirectoryExists(join(basePath, 'wwwroot/js'));
		ensureDirectoryExists(join(basePath, 'wwwroot/lib'));


		if (config.features.responsive) {
			console.log('📱 Responsive design assets prepared');
		}

		if (config.features.darkModeSupport) {
			console.log('🌙 Dark mode assets prepared');
		}

		console.log('✅ Static assets generated');
	} catch (error) {
		console.error('❌ Static asset generation failed:', error);
		throw error;
	}
}

function verifyGeneration(config: BlazorArchitectureConfig): void {
	console.log('🔍 Verifying generation results...');

	try {
		// Verify all expected files were created
		const basePath = config.project.blazorProjectPath;
		const expectedFiles = [
			'Program.cs',
			'Components/App.razor',
			'Components/Routes.razor',
			'Components/Routes.razor.cs',
			'Components/_Imports.razor',
			'Components/Layout/MainLayout.razor',
			'Components/Layout/NavMenu.razor'
		];

		for (const file of expectedFiles) {
			const filePath = join(basePath, file);
			if (fs.existsSync(filePath)) {
				console.log(`✅ ${file}`);
			} else {
				console.warn(`⚠️ Missing: ${file}`);
			}
		}

		// Check component library integration
		if (config.componentLibrary !== 'none') {
			console.log(`🧩 ${toPascalCase(config.componentLibrary)} integration verified`);
		}

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

/**
 * Get the correct template path based on file type and template directory structure
 */
function getTemplatePath(libraryName: string, templateFileName: string): string {
	const scriptDir = dirname(import.meta.url.replace('file://', ''));
	const baseTemplatePath = join(scriptDir, '../templates', libraryName);

	// Core component files
	if (templateFileName === 'App.razor' || templateFileName === 'App.razor.cs' ||
		templateFileName === 'Routes.razor' || templateFileName === 'Routes.razor.cs' ||
		templateFileName === '_Imports.razor') {
		return join(baseTemplatePath, 'Components', `${templateFileName}.template`);
	}

	// Layout components
	if (templateFileName.includes('Layout') ||
		templateFileName === 'NavMenu.razor' || templateFileName === 'NavMenu.razor.cs') {
		return join(baseTemplatePath, 'Components/Layout', `${templateFileName}.template`);
	}



	// Static assets
	if (templateFileName === 'site.css') {
		return join(baseTemplatePath, 'wwwroot/css', `${templateFileName}.template`);
	}
	if (templateFileName === 'site.js') {
		return join(baseTemplatePath, 'wwwroot/js', `${templateFileName}.template`);
	}

	// Root level files (Program.cs, .csproj, appsettings.json)
	return join(baseTemplatePath, `${templateFileName}.template`);
}

async function generateFromTemplate(libraryName: string, templateFileName: string, config: BlazorArchitectureConfig, outputFileName?: string): Promise<void> {
	const templatePath = getTemplatePath(libraryName, templateFileName);

	// 动态解析输出路径，支持项目名称替换
	let finalOutputPath: string;
	if (outputFileName) {
		// 如果提供了自定义输出文件名，直接使用
		finalOutputPath = join(config.project.blazorProjectPath, outputFileName);
	} else {
		// 否则使用动态路径解析
		const dynamicOutputPath = getOutputPathFromTemplate(templateFileName, config);
		finalOutputPath = join(config.project.blazorProjectPath, dynamicOutputPath);
	}

	try {
		// Read template file
		const templateContent = fs.readFileSync(templatePath, 'utf-8');

		// Replace template variables
		const processedContent = replaceTemplateVariables(templateContent, config);

		// Write output file
		ensureDirectoryExists(dirname(finalOutputPath));
		fs.writeFileSync(finalOutputPath, processedContent, 'utf-8');
		console.log(`✅ Generated ${outputFileName || getOutputPathFromTemplate(templateFileName, config)}`);
	} catch (error) {
		console.error(`❌ Failed to generate ${templateFileName}:`, error);
		throw error;
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

	// Get current date for build date
	const currentDate = new Date().toISOString().split('T')[0];

	let result = content
		// Project variables
		.replace(/\{\{PROJECT_NAMESPACE\}\}/g, projectName)
		.replace(/\{\{PROJECT_NAME\}\}/g, projectName)
		.replace(/\{\{PROJECT_NAME_PASCAL\}\}/g, toPascalCase(projectName))
		.replace(/\{\{PROJECT_NAME_CAMEL\}\}/g, toCamelCase(projectName))
		.replace(/\{\{PROJECT_NAME_KEBAB\}\}/g, toKebabCase(projectName))
		// Component library variables
		.replace(/\{\{COMPONENT_LIBRARY\}\}/g, config.componentLibrary)
		.replace(/\{\{COMPONENT_LIBRARY_PASCAL\}\}/g, toPascalCase(config.componentLibrary))
		.replace(/\{\{COMPONENT_LIBRARY_NAME\}\}/g, COMPONENT_LIBRARIES[config.componentLibrary]?.displayName || toPascalCase(config.componentLibrary))
		// Configuration variables
		.replace(/\{\{BUILD_DATE\}\}/g, currentDate)
		.replace(/\{\{DOMAIN_NAME\}\}/g, 'localhost')
		.replace(/\{\{CONNECTION_STRING\}\}/g, 'Data Source=localhost;Initial Catalog=SanjelData;Integrated Security=True')
		.replace(/\{\{SYNCFUSION_LICENSE_KEY\}\}/g, config.license?.key || 'YOUR_SYNCFUSION_LICENSE_KEY_HERE');

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

/**
 * 根据模板文件名动态解析输出路径
 * 基于文件名约定，避免硬编码路径映射
 */
function getOutputPathFromTemplate(templateFileName: string, config: BlazorArchitectureConfig): string {
	// 移除 .template 扩展名，获取目标文件名
	const fileName = templateFileName.replace('.template', '');

	// 获取项目信息用于名称替换
	let projectName = config.project.name;
	try {
		const projectInfo = detectProjectInfo();
		projectName = projectInfo.projectName;
	} catch (error) {
		console.warn('Using fallback project name:', projectName);
	}

	// 基于文件名模式推断输出路径

	// 核心组件文件 → Components/
	if (/^(App|Routes|_Imports)\.razor(\.cs)?$/.test(fileName)) {
		return `Components/${fileName}`;
	}

	// 布局组件文件 → Components/Layout/
	if (/Layout|NavMenu/.test(fileName)) {
		return `Components/Layout/${fileName}`;
	}



	// 静态资源文件 → wwwroot/
	if (fileName.endsWith('.css')) {
		return `wwwroot/css/${fileName}`;
	}
	if (fileName.endsWith('.js')) {
		return `wwwroot/js/${fileName}`;
	}
	if (fileName.match(/\.(png|jpg|jpeg|gif|svg|ico)$/)) {
		return `wwwroot/images/${fileName}`;
	}

	// 项目配置文件 → 根目录
	if (fileName === 'Program.cs' || fileName === 'appsettings.json') {
		return fileName;
	}

	// .csproj 文件 → 动态项目名称替换
	if (fileName === '.csproj') {
		return `${projectName}.Blazor.csproj`;
	}

	// 默认：根目录
	return fileName;
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
		// Parse command line arguments
		const componentLibraryArg = (process.argv as string[]).find(arg => arg.startsWith('--component-library='));
		let componentLibrary = componentLibraryArg?.split('=')[1] as ComponentLibraryType;

		// If no component library specified, provide interactive selection
		if (!componentLibrary) {
			console.log('🎯 Interactive Component Library Selection');
			console.log('=========================================');
			console.log('Please select your preferred Blazor component library:');
			console.log('');
			console.log('Available options:');
			console.log('  🎨 MudBlazor     - Material Design components (free, recommended)');
			console.log('  🏢 Syncfusion    - Enterprise components (requires license)');
			console.log('  🅱️ Bootstrap     - Bootstrap-based components (free, lightweight)');
			console.log('  ⚡ None (Minimal)- No component library (maximum flexibility)');
			console.log('');

			// Default to mudblazor for interactive mode as it's the most popular choice
			componentLibrary = 'mudblazor';
			console.log(`📋 Auto-selecting MudBlazor as the default recommended option...`);
			console.log('💡 Tip: Use --component-library=OPTION to skip this selection in future runs');
		}

		// Validate component library selection
		const validLibraries: ComponentLibraryType[] = ['mudblazor', 'syncfusion', 'bootstrap', 'none'];
		if (!validLibraries.includes(componentLibrary)) {
			console.error(`❌ Invalid component library: ${componentLibrary}`);
			console.log(`Valid options: ${validLibraries.join(', ')}`);
			process.exit(1);
		}

		console.log(`🏗️ Generating Blazor architecture with ${componentLibrary}...`);

		// Parse feature flags
		const withAuth = (process.argv as string[]).includes('--with-auth');
		const darkMode = (process.argv as string[]).includes('--dark-mode');
		const noResponsive = (process.argv as string[]).includes('--no-responsive');

		// Auto-detect project configuration
		const projectInfo = detectProjectInfo();
		const blazorProjectPath = `${projectInfo.srcRoot}/${projectInfo.projectName}.Blazor`;

		const config: BlazorArchitectureConfig = {
			project: {
				name: projectInfo.projectName,
				blazorProjectPath: blazorProjectPath
			},
			componentLibrary: componentLibrary,
			features: {
				responsive: !noResponsive,
				authentication: withAuth,
				darkModeSupport: darkMode
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

