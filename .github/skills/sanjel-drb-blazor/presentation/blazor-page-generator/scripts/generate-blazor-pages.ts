// @ts-ignore
import { existsSync, writeFileSync } from 'node:fs';
// @ts-ignore
import { join } from 'node:path';

// Import shared utilities
import {
	constructPagePath,
	detectProjectInfo,
	ensureDirectoryExists,
	findDomainModelMetadata,
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

interface DomainModelMetadata {
	version: string;
	generatedAt: string;
	sourceFile: string;
	entities: Entity[];
	relationships: any[];
	enums: any[];
	statistics: any;
}

interface PageGenerationOptions {
	namespace: string;
	outputDirectory: string;
	projectName: string;
	pageTypes: string[];
}

// Generate List page markup (.razor) for an entity
function generateListPageMarkup(entity: Entity, options: PageGenerationOptions): string {
	const entityName = toPascalCase(entity.name);
	const entityNameLower = entity.name.toLowerCase();

	const lines: string[] = [];

	// Page directive and usings
	lines.push(`@page "/${entityNameLower}"`);
	lines.push(`@page "/${entityNameLower}/list"`);
	lines.push(`@using ${options.projectName}.Core.Services`);
	lines.push(`@using ${options.projectName}.Core.Entities`);
	lines.push(`@inject I${entityName}Service ${entityName}Service`);
	lines.push(`@inject NavigationManager NavigationManager`);
	lines.push(`@inject ISnackbar Snackbar`);
	lines.push('');

	// Page title
	lines.push(`<PageTitle>${entityName} Management</PageTitle>`);
	lines.push('');

	// Main container
	lines.push('<MudContainer MaxWidth="MaxWidth.ExtraLarge" Class="mt-4">');
	lines.push(`    <MudText Typo="Typo.h4" Class="mb-4">${entityName} Management</MudText>`);
	lines.push('    ');
	lines.push('    <!-- Toolbar -->');
	lines.push('    <MudPaper Class="pa-4 mb-4">');
	lines.push('        <MudGrid AlignItems="Center">');
	lines.push('            <MudItem xs="8" md="10">');
	lines.push('                <MudTextField @bind-Value="searchTerm" ');
	lines.push(`                              Placeholder="Search ${entityNameLower}..." `);
	lines.push('                              Adornment="Adornment.Start" ');
	lines.push('                              AdornmentIcon="Icons.Material.Filled.Search" />');
	lines.push('            </MudItem>');
	lines.push('            <MudItem xs="4" md="2">');
	lines.push('                <MudButton Color="Color.Primary" ');
	lines.push('                           Variant="Variant.Filled" ');
	lines.push('                           StartIcon="Icons.Material.Filled.Add"');
	lines.push('                           OnClick="NavigateToCreate"');
	lines.push('                           FullWidth="true">');
	lines.push('                    Create');
	lines.push('                </MudButton>');
	lines.push('            </MudItem>');
	lines.push('        </MudGrid>');
	lines.push('    </MudPaper>');
	lines.push('    ');
	lines.push('    <!-- List Component -->');
	lines.push(`    <${entityName}ListComponent @ref="listComponent" `);
	lines.push('                          SearchTerm="@searchTerm"');
	lines.push('                          OnEditRequest="NavigateToEdit"');
	lines.push('                          OnViewRequest="NavigateToDetail"');
	lines.push('                          OnDeleteRequest="HandleDelete" />');
	lines.push('</MudContainer>');

	return lines.join('\n');
}

// Generate List page code-behind (.razor.cs) for an entity
function generateListPageCodeBehind(entity: Entity, options: PageGenerationOptions): string {
	const entityName = toPascalCase(entity.name);
	const entityNameLower = entity.name.toLowerCase();
	const entityId = entity.attributes.find(attr => attr.name.toLowerCase().includes('id'))?.name || 'id';

	const lines: string[] = [];

	// Using statements
	lines.push('using Microsoft.AspNetCore.Components;');
	lines.push('using MudBlazor;');
	lines.push(`using ${options.projectName}.Core.Services;`);
	lines.push(`using ${options.projectName}.Core.Entities;`);
	lines.push('');

	// Namespace and class
	lines.push(`namespace ${options.namespace}.${entityName};`);
	lines.push('');
	lines.push(`public partial class List`);
	lines.push('{');
	lines.push('    private string searchTerm = string.Empty;');
	lines.push(`    private ${entityName}ListComponent listComponent = default!;`);
	lines.push('    ');
	lines.push('    private void NavigateToCreate()');
	lines.push('    {');
	lines.push(`        NavigationManager.NavigateTo("/${entityNameLower}/create");`);
	lines.push('    }');
	lines.push('    ');
	lines.push(`    private void NavigateToEdit(string ${entityId})`);
	lines.push('    {');
	lines.push(`        NavigationManager.NavigateTo($"/${entityNameLower}/edit/{${entityId}}");`);
	lines.push('    }');
	lines.push('    ');
	lines.push(`    private void NavigateToDetail(string ${entityId})`);
	lines.push('    {');
	lines.push(`        NavigationManager.NavigateTo($"/${entityNameLower}/detail/{${entityId}}");`);
	lines.push('    }');
	lines.push('    ');
	lines.push(`    private async Task HandleDelete(string ${entityId})`);
	lines.push('    {');
	lines.push('        // TODO: Implement delete confirmation dialog');
	lines.push('        try');
	lines.push('        {');
	lines.push(`            await ${entityName}Service.DeleteAsync(${entityId});`);
	lines.push(`            Snackbar.Add("${entityName} deleted successfully", Severity.Success);`);
	lines.push('            await listComponent.RefreshAsync();');
	lines.push('        }');
	lines.push('        catch (Exception ex)');
	lines.push('        {');
	lines.push(`            Snackbar.Add($"Error deleting ${entityNameLower}: {ex.Message}", Severity.Error);`);
	lines.push('        }');
	lines.push('    }');
	lines.push('}');

	return lines.join('\n');
}

// Generate Create page markup (.razor) for an entity
function generateCreatePageMarkup(entity: Entity, options: PageGenerationOptions): string {
	const entityName = toPascalCase(entity.name);
	const entityNameLower = entity.name.toLowerCase();

	const lines: string[] = [];

	// Page directive and usings
	lines.push(`@page "/${entityNameLower}/create"`);
	lines.push(`@using ${options.projectName}.Core.Services`);
	lines.push(`@using ${options.projectName}.Core.Entities`);
	lines.push(`@inject I${entityName}Service ${entityName}Service`);
	lines.push(`@inject NavigationManager NavigationManager`);
	lines.push(`@inject ISnackbar Snackbar`);
	lines.push('');

	// Page title
	lines.push(`<PageTitle>Create ${entityName}</PageTitle>`);
	lines.push('');

	// Main container
	lines.push('<MudContainer MaxWidth="MaxWidth.Medium" Class="mt-4">');
	lines.push('    <MudBreadcrumbs Items="breadcrumbs" Class="mb-4" />');
	lines.push('    ');
	lines.push(`    <MudText Typo="Typo.h4" Class="mb-4">Create New ${entityName}</MudText>`);
	lines.push('    ');
	lines.push('    <!-- Form Component -->');
	lines.push(`    <${entityName}FormComponent @ref="formComponent"`);
	lines.push('                          OnSave="HandleSave"');
	lines.push('                          OnCancel="HandleCancel" />');
	lines.push('</MudContainer>');

	return lines.join('\n');
}

// Generate Create page code-behind (.razor.cs) for an entity
function generateCreatePageCodeBehind(entity: Entity, options: PageGenerationOptions): string {
	const entityName = toPascalCase(entity.name);
	const entityNameLower = entity.name.toLowerCase();

	const lines: string[] = [];

	// Using statements
	lines.push('using Microsoft.AspNetCore.Components;');
	lines.push('using MudBlazor;');
	lines.push(`using ${options.projectName}.Core.Services;`);
	lines.push(`using ${options.projectName}.Core.Entities;`);
	lines.push('');

	// Namespace and class
	lines.push(`namespace ${options.namespace}.${entityName};`);
	lines.push('');
	lines.push(`public partial class Create`);
	lines.push('{');
	lines.push(`    private ${entityName}FormComponent formComponent = default!;`);
	lines.push('    ');
	lines.push('    private List<BreadcrumbItem> breadcrumbs = new()');
	lines.push('    {');
	lines.push('        new BreadcrumbItem("Home", href: "/"),');
	lines.push(`        new BreadcrumbItem("${entityName}s", href: "/${entityNameLower}"),`);
	lines.push('        new BreadcrumbItem("Create", href: null, disabled: true)');
	lines.push('    };');
	lines.push('    ');
	lines.push(`    private async Task HandleSave(${entityName} ${entityNameLower})`);
	lines.push('    {');
	lines.push('        try');
	lines.push('        {');
	lines.push(`            await ${entityName}Service.CreateAsync(${entityNameLower});`);
	lines.push(`            Snackbar.Add("${entityName} created successfully", Severity.Success);`);
	lines.push(`            NavigationManager.NavigateTo("/${entityNameLower}");`);
	lines.push('        }');
	lines.push('        catch (Exception ex)');
	lines.push('        {');
	lines.push(`            Snackbar.Add($"Error creating ${entityNameLower}: {ex.Message}", Severity.Error);`);
	lines.push('        }');
	lines.push('    }');
	lines.push('    ');
	lines.push('    private void HandleCancel()');
	lines.push('    {');
	lines.push(`        NavigationManager.NavigateTo("/${entityNameLower}");`);
	lines.push('    }');
	lines.push('}');

	return lines.join('\n');
}

// Generate Edit page markup (.razor) for an entity
function generateEditPageMarkup(entity: Entity, options: PageGenerationOptions): string {
	const entityName = toPascalCase(entity.name);
	const entityNameLower = entity.name.toLowerCase();
	const entityId = entity.attributes.find(attr => attr.name.toLowerCase().includes('id'))?.name || 'id';

	const lines: string[] = [];

	// Page directive and usings
	lines.push(`@page "/${entityNameLower}/edit/{${entityId}}"`);
	lines.push(`@using ${options.projectName}.Core.Services`);
	lines.push(`@using ${options.projectName}.Core.Entities`);
	lines.push(`@inject I${entityName}Service ${entityName}Service`);
	lines.push(`@inject NavigationManager NavigationManager`);
	lines.push(`@inject ISnackbar Snackbar`);
	lines.push('');

	// Page title
	lines.push(`<PageTitle>Edit ${entityName}</PageTitle>`);
	lines.push('');

	// Main container with loading state
	lines.push('@if (isLoading)');
	lines.push('{');
	lines.push('    <MudContainer MaxWidth="MaxWidth.Medium" Class="mt-4">');
	lines.push('        <MudProgressCircular Indeterminate="true" />');
	lines.push('        <MudText Class="ml-2">Loading...</MudText>');
	lines.push('    </MudContainer>');
	lines.push('}');
	lines.push(`else if (current${entityName} == null)`);
	lines.push('{');
	lines.push('    <MudContainer MaxWidth="MaxWidth.Medium" Class="mt-4">');
	lines.push(`        <MudAlert Severity="Severity.Error">${entityName} not found.</MudAlert>`);
	lines.push('    </MudContainer>');
	lines.push('}');
	lines.push('else');
	lines.push('{');
	lines.push('    <MudContainer MaxWidth="MaxWidth.Medium" Class="mt-4">');
	lines.push('        <MudBreadcrumbs Items="breadcrumbs" Class="mb-4" />');
	lines.push('        ');
	lines.push(`        <MudText Typo="Typo.h4" Class="mb-4">Edit ${entityName}</MudText>`);
	lines.push('        ');
	lines.push('        <!-- Form Component -->');
	lines.push(`        <${entityName}FormComponent @ref="formComponent"`);
	lines.push(`                              Entity="current${entityName}"`);
	lines.push('                              OnSave="HandleSave"');
	lines.push('                              OnCancel="HandleCancel" />');
	lines.push('    </MudContainer>');
	lines.push('}');

	return lines.join('\n');
}

// Generate Edit page code-behind (.razor.cs) for an entity
function generateEditPageCodeBehind(entity: Entity, options: PageGenerationOptions): string {
	const entityName = toPascalCase(entity.name);
	const entityNameLower = entity.name.toLowerCase();
	const entityId = entity.attributes.find(attr => attr.name.toLowerCase().includes('id'))?.name || 'id';

	const lines: string[] = [];

	// Using statements
	lines.push('using Microsoft.AspNetCore.Components;');
	lines.push('using MudBlazor;');
	lines.push(`using ${options.projectName}.Core.Services;`);
	lines.push(`using ${options.projectName}.Core.Entities;`);
	lines.push('');

	// Namespace and class
	lines.push(`namespace ${options.namespace}.${entityName};`);
	lines.push('');
	lines.push(`public partial class Edit`);
	lines.push('{');
	lines.push(`    [Parameter] public string ${entityId} { get; set; } = string.Empty;`);
	lines.push('    ');
	lines.push(`    private ${entityName}FormComponent formComponent = default!;`);
	lines.push(`    private ${entityName}? current${entityName};`);
	lines.push('    private bool isLoading = true;');
	lines.push('    ');
	lines.push('    private List<BreadcrumbItem> breadcrumbs = new()');
	lines.push('    {');
	lines.push('        new BreadcrumbItem("Home", href: "/"),');
	lines.push(`        new BreadcrumbItem("${entityName}s", href: "/${entityNameLower}"),`);
	lines.push('        new BreadcrumbItem("Edit", href: null, disabled: true)');
	lines.push('    };');
	lines.push('    ');
	lines.push('    protected override async Task OnInitializedAsync()');
	lines.push('    {');
	lines.push('        await LoadEntity();');
	lines.push('    }');
	lines.push('    ');
	lines.push('    private async Task LoadEntity()');
	lines.push('    {');
	lines.push('        try');
	lines.push('        {');
	lines.push('            isLoading = true;');
	lines.push(`            current${entityName} = await ${entityName}Service.GetByIdAsync(${entityId});`);
	lines.push('        }');
	lines.push('        catch (Exception ex)');
	lines.push('        {');
	lines.push(`            Snackbar.Add($"Error loading ${entityNameLower}: {ex.Message}", Severity.Error);`);
	lines.push('        }');
	lines.push('        finally');
	lines.push('        {');
	lines.push('            isLoading = false;');
	lines.push('        }');
	lines.push('    }');
	lines.push('    ');
	lines.push(`    private async Task HandleSave(${entityName} ${entityNameLower})`);
	lines.push('    {');
	lines.push('        try');
	lines.push('        {');
	lines.push(`            await ${entityName}Service.UpdateAsync(${entityNameLower});`);
	lines.push(`            Snackbar.Add("${entityName} updated successfully", Severity.Success);`);
	lines.push(`            NavigationManager.NavigateTo("/${entityNameLower}");`);
	lines.push('        }');
	lines.push('        catch (Exception ex)');
	lines.push('        {');
	lines.push(`            Snackbar.Add($"Error updating ${entityNameLower}: {ex.Message}", Severity.Error);`);
	lines.push('        }');
	lines.push('    }');
	lines.push('    ');
	lines.push('    private void HandleCancel()');
	lines.push('    {');
	lines.push(`        NavigationManager.NavigateTo("/${entityNameLower}");`);
	lines.push('    }');
	lines.push('}');

	return lines.join('\n');
}

// Generate Detail page markup for an entity
function generateDetailPageMarkup(entity: Entity, options: PageGenerationOptions): string {
	const entityName = toPascalCase(entity.name);
	const entityNameLower = entity.name.toLowerCase();
	const entityId = entity.attributes.find(attr => attr.name.toLowerCase().includes('id'))?.name || 'id';

	const lines: string[] = [];

	// Page directive and usings
	lines.push(`@page "/${entityNameLower}/detail/{${entityId}}"`);
	lines.push(`@using ${options.projectName}.Core.Services`);
	lines.push(`@using ${options.projectName}.Core.Entities`);
	lines.push(`@inject I${entityName}Service ${entityName}Service`);
	lines.push(`@inject NavigationManager NavigationManager`);
	lines.push(`@inject ISnackbar Snackbar`);
	lines.push('');

	// Page title
	lines.push(`<PageTitle>${entityName} Details</PageTitle>`);
	lines.push('');

	// Main container with loading state
	lines.push('@if (isLoading)');
	lines.push('{');
	lines.push('    <MudContainer MaxWidth="MaxWidth.Medium" Class="mt-4">');
	lines.push('        <MudProgressCircular Indeterminate="true" />');
	lines.push('        <MudText Class="ml-2">Loading...</MudText>');
	lines.push('    </MudContainer>');
	lines.push('}');
	lines.push(`else if (current${entityName} == null)`);
	lines.push('{');
	lines.push('    <MudContainer MaxWidth="MaxWidth.Medium" Class="mt-4">');
	lines.push(`        <MudAlert Severity="Severity.Error">${entityName} not found.</MudAlert>`);
	lines.push('    </MudContainer>');
	lines.push('}');
	lines.push('else');
	lines.push('{');
	lines.push('    <MudContainer MaxWidth="MaxWidth.Medium" Class="mt-4">');
	lines.push('        <MudBreadcrumbs Items="breadcrumbs" Class="mb-4" />');
	lines.push('        ');
	lines.push(`        <MudText Typo="Typo.h4" Class="mb-4">${entityName} Details</MudText>`);
	lines.push('        ');
	lines.push('        <!-- Detail Component -->');
	lines.push(`        <${entityName}DetailComponent Entity="current${entityName}" />`);
	lines.push('        ');
	lines.push('        <!-- Action Buttons -->');
	lines.push('        <MudPaper Class="pa-4 mt-4">');
	lines.push('            <MudGrid>');
	lines.push('                <MudItem xs="12" md="6">');
	lines.push('                    <MudButton Color="Color.Primary" ');
	lines.push('                               Variant="Variant.Filled"');
	lines.push('                               StartIcon="Icons.Material.Filled.Edit"');
	lines.push('                               OnClick="NavigateToEdit"');
	lines.push('                               FullWidth="true">');
	lines.push('                        Edit');
	lines.push('                    </MudButton>');
	lines.push('                </MudItem>');
	lines.push('                <MudItem xs="12" md="6">');
	lines.push('                    <MudButton Color="Color.Secondary" ');
	lines.push('                               Variant="Variant.Outlined"');
	lines.push('                               StartIcon="Icons.Material.Filled.ArrowBack"');
	lines.push('                               OnClick="NavigateToList"');
	lines.push('                               FullWidth="true">');
	lines.push('                        Back to List');
	lines.push('                    </MudButton>');
	lines.push('                </MudItem>');
	lines.push('            </MudGrid>');
	lines.push('        </MudPaper>');
	lines.push('    </MudContainer>');
	lines.push('}');

	return lines.join('\n');
}

// Generate Detail page code-behind for an entity
function generateDetailPageCodeBehind(entity: Entity, options: PageGenerationOptions): string {
	const entityName = toPascalCase(entity.name);
	const entityNameLower = entity.name.toLowerCase();
	const entityId = entity.attributes.find(attr => attr.name.toLowerCase().includes('id'))?.name || 'id';

	const lines: string[] = [];

	// Usings
	lines.push(`using ${options.projectName}.Core.Services;`);
	lines.push(`using ${options.projectName}.Core.Entities;`);
	lines.push(`using Microsoft.AspNetCore.Components;`);
	lines.push(`using MudBlazor;`);
	lines.push('');

	// Namespace and class
	lines.push(`namespace ${options.projectName}.Components.Pages.${entityName}s;`);
	lines.push('');
	lines.push(`public partial class Detail : ComponentBase`);
	lines.push('{');
	lines.push(`    [Parameter] public string ${entityId} { get; set; } = string.Empty;`);
	lines.push('    ');
	lines.push(`    private ${entityName}? current${entityName};`);
	lines.push('    private bool isLoading = true;');
	lines.push('    ');
	lines.push('    private List<BreadcrumbItem> breadcrumbs = new()');
	lines.push('    {');
	lines.push('        new BreadcrumbItem("Home", href: "/"),');
	lines.push(`        new BreadcrumbItem("${entityName}s", href: "/${entityNameLower}"),`);
	lines.push('        new BreadcrumbItem("Details", href: null, disabled: true)');
	lines.push('    };');
	lines.push('    ');
	lines.push('    protected override async Task OnInitializedAsync()');
	lines.push('    {');
	lines.push('        await LoadEntity();');
	lines.push('    }');
	lines.push('    ');
	lines.push('    private async Task LoadEntity()');
	lines.push('    {');
	lines.push('        try');
	lines.push('        {');
	lines.push('            isLoading = true;');
	lines.push(`            current${entityName} = await ${entityName}Service.GetByIdAsync(${entityId});`);
	lines.push('        }');
	lines.push('        catch (Exception ex)');
	lines.push('        {');
	lines.push(`            Snackbar.Add($"Error loading ${entityNameLower}: {ex.Message}", Severity.Error);`);
	lines.push('        }');
	lines.push('        finally');
	lines.push('        {');
	lines.push('            isLoading = false;');
	lines.push('        }');
	lines.push('    }');
	lines.push('    ');
	lines.push('    private void NavigateToEdit()');
	lines.push('    {');
	lines.push(`        NavigationManager.NavigateTo($"/${entityNameLower}/edit/{${entityId}}");`);
	lines.push('    }');
	lines.push('    ');
	lines.push('    private void NavigateToList()');
	lines.push('    {');
	lines.push(`        NavigationManager.NavigateTo("/${entityNameLower}");`);
	lines.push('    }');
	lines.push('}');

	return lines.join('\n');
}

// Main generation function
function generateBlazorPages(metadata: DomainModelMetadata, options: PageGenerationOptions): void {
	console.log('🔧 Generating Blazor pages...');

	// Filter entities (exclude enums and systems)
	const pageEntities = metadata.entities.filter(entity =>
		entity.type === 'entity');

	console.log(`📝 Generating pages for ${pageEntities.length} entities...`);

	pageEntities.forEach(entity => {
		const entityName = toPascalCase(entity.name);

		// Create entity-specific directory
		const entityDir = join(options.outputDirectory, entityName);
		ensureDirectoryExists(entityDir);

		// Generate requested page types
		if (options.pageTypes.includes('List')) {
			const listPageMarkup = generateListPageMarkup(entity, options);
			const listPageCodeBehind = generateListPageCodeBehind(entity, options);
			writeFileSync(join(entityDir, 'List.razor'), listPageMarkup);
			writeFileSync(join(entityDir, 'List.razor.cs'), listPageCodeBehind);
			console.log(`   ✅ Generated ${entityName}/List.razor and List.razor.cs`);
		}

		if (options.pageTypes.includes('Create')) {
			const createPageMarkup = generateCreatePageMarkup(entity, options);
			const createPageCodeBehind = generateCreatePageCodeBehind(entity, options);
			writeFileSync(join(entityDir, 'Create.razor'), createPageMarkup);
			writeFileSync(join(entityDir, 'Create.razor.cs'), createPageCodeBehind);
			console.log(`   ✅ Generated ${entityName}/Create.razor and Create.razor.cs`);
		}

		if (options.pageTypes.includes('Edit')) {
			const editPageMarkup = generateEditPageMarkup(entity, options);
			const editPageCodeBehind = generateEditPageCodeBehind(entity, options);
			writeFileSync(join(entityDir, 'Edit.razor'), editPageMarkup);
			writeFileSync(join(entityDir, 'Edit.razor.cs'), editPageCodeBehind);
			console.log(`   ✅ Generated ${entityName}/Edit.razor and Edit.razor.cs`);
		}

		if (options.pageTypes.includes('Detail')) {
			const detailPageMarkup = generateDetailPageMarkup(entity, options);
			const detailPageCodeBehind = generateDetailPageCodeBehind(entity, options);
			writeFileSync(join(entityDir, 'Detail.razor'), detailPageMarkup);
			writeFileSync(join(entityDir, 'Detail.razor.cs'), detailPageCodeBehind);
			console.log(`   ✅ Generated ${entityName}/Detail.razor and Detail.razor.cs`);
		}
	});

	console.log(`🎉 Successfully generated Blazor pages in ${options.outputDirectory}`);
}

// Command line argument parsing and execution
function main(): void {
	try {
		console.log('🚀 Blazor Page Generator Starting...');

		// @ts-ignore
		const args = process.argv.slice(2) as string[];

		let metadataPath = '';
		let outputDir = '';
		let namespace = '';
		let projectName = '';
		let pageTypes: string[] = ['List', 'Create', 'Edit', 'Detail'];

		if (args.length === 0) {
			// Auto-detection mode
			console.log('📋 Auto-detection mode - finding metadata and paths...');

			// Find domain model metadata
			const foundMetadata = findDomainModelMetadata();
			if (!foundMetadata) {
				console.error('❌ No domain model metadata found. Please run domain-model-parser first or specify metadata file path.');
				// @ts-ignore
				process.exit(1);
			}
			metadataPath = foundMetadata;

			// Get page paths
			const pagePaths = constructPagePath();
			outputDir = pagePaths.outputDir;
			namespace = pagePaths.namespace;

			// Get project info
			const projectInfo = detectProjectInfo();
			projectName = projectInfo.projectName;
		} else if (args.length >= 1) {
			// Manual mode
			metadataPath = args[0];

			if (args.length >= 2) {
				pageTypes = args[1].split(',').map(type => type.trim());
			}

			if (args.length >= 3) {
				outputDir = args[2];
				// Derive namespace from output directory
				const projectInfo = detectProjectInfo();
				projectName = projectInfo.projectName;
				namespace = `${projectName}.Blazor.Pages`;
			} else {
				const pagePaths = constructPagePath();
				outputDir = pagePaths.outputDir;
				namespace = pagePaths.namespace;

				const projectInfo = detectProjectInfo();
				projectName = projectInfo.projectName;
			}
		} else {
			console.error('❌ Usage: bun run generate-blazor-pages.ts [metadata-file] [page-types] [output-dir]');
			// @ts-ignore
			process.exit(1);
		}

		// Validate metadata file exists
		if (!existsSync(metadataPath)) {
			console.error(`❌ Metadata file not found: ${metadataPath}`);
			// @ts-ignore
			process.exit(1);
		}

		console.log(`📂 Using metadata: ${metadataPath}`);
		console.log(`📁 Output directory: ${outputDir}`);
		console.log(`📦 Namespace: ${namespace}`);
		console.log(`📋 Page types: ${pageTypes.join(', ')}`);

		// Read and validate metadata
		const metadata = readJsonFile(metadataPath) as DomainModelMetadata;

		if (!metadata.entities || !Array.isArray(metadata.entities)) {
			console.error('❌ Invalid metadata: missing or invalid entities array');
			// @ts-ignore
			process.exit(1);
		}

		// Ensure output directory exists
		ensureDirectoryExists(outputDir);

		// Generation options
		const options: PageGenerationOptions = {
			namespace,
			outputDirectory: outputDir,
			projectName,
			pageTypes
		};

		// Generate Blazor pages
		generateBlazorPages(metadata, options);

		console.log('\n✅ Blazor page generation completed successfully!');

	} catch (error) {
		console.error('❌ Blazor page generation failed:', error);
		// @ts-ignore
		process.exit(1);
	}
}

// Execute if run directly
// @ts-ignore
if (import.meta.main) {
	main();
}