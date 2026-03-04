# Blazor Page Generator Skill

## Overview

The `blazor-page-generator` skill generates complete Blazor pages with separate markup (.razor) and code-behind (.razor.cs) files from domain model metadata. This skill creates entity-specific pages (List, Create, Edit, Detail) following MudBlazor layout patterns and modern Blazor development best practices with proper code separation.

## Functionality

### Generated Pages

Each page type generates two files:
- **Markup file (.razor)**: Contains Razor markup, page directives, and component structure
- **Code-behind file (.razor.cs)**: Contains C# logic, event handlers, and business logic

1. **List Pages**: 
   - Entity listing pages with search and filtering
   - Data grid integration with sorting and pagination
   - Action buttons (Create, Edit, Delete, View)
   - Toolbar with page-specific actions
   - Navigation integration

2. **Create Pages**:
   - Entity creation pages with form components
   - Validation integration and error handling
   - Save and cancel operations
   - Navigation breadcrumbs

3. **Edit Pages**:
   - Entity editing pages with pre-populated forms
   - Form validation and business rule integration
   - Update and cancel operations
   - Audit trail display

4. **Detail Pages**:
   - Read-only entity detail view
   - Formatted property display
   - Related entity navigation
   - Action buttons (Edit, Delete)

### Features

- **Code-Behind Separation**: Generates separate .razor.cs files following modern Blazor patterns
- **Auto Project Detection**: Automatically detects project root and Blazor page directory structure
- **MudBlazor Integration**: Uses MudBlazor components for consistent UI/UX
- **Routing Configuration**: Generates @page directives with proper route patterns
- **Layout Structure**: Implements standard page layout with containers and headers
- **Component Integration**: Designed to work with generated Blazor components
- **Navigation Support**: Includes navigation and breadcrumb generation
- **Responsive Design**: Mobile-first responsive layout patterns

## Usage

### Auto-Detection Mode (Recommended)
```bash
# Auto-detects metadata file and generates all entity pages
cd /path/to/.github/skills/sanjel-drb-blazor/presentation/blazor-page-generator/scripts
bun run generate-blazor-pages.ts
```

### Manual Mode
```bash
# Specify metadata file and optional page types
bun run generate-blazor-pages.ts ./path/to/domain-metadata.json [page-types] [output-dir]
```

### Parameters

- `metadata-file` (optional): Path to domain model metadata JSON file (auto-detected if not provided)
- `page-types` (optional): Comma-separated list of page types to generate (List,Create,Edit,Detail)
- `output-directory` (optional): Output directory for Blazor pages (auto-detected if not provided)

### Auto-Detection Behavior

1. **Metadata File**: Searches for `domain-model-metadata.json` in orgModel directories
2. **Output Directory**: Uses `src/{ProjectName}.Blazor/Pages/` based on project name detection
3. **Page Types**: Generates all page types (List, Create, Edit, Detail) by default
4. **Project Root**: Automatically finds project root by locating .slnx files

## Generated Code Structure

### List Page Example

#### List.razor (Markup)
```razor
@page "/requests"
@page "/requests/list"
@using Sanjel.RequestManagement.Core.Services
@using Sanjel.RequestManagement.Core.Entities
@inject IRequestService RequestService
@inject NavigationManager NavigationManager
@inject ISnackbar Snackbar

<PageTitle>Request Management</PageTitle>

<MudContainer MaxWidth="MaxWidth.ExtraLarge" Class="mt-4">
    <MudText Typo="Typo.h4" Class="mb-4">Request Management</MudText>
    
    <!-- Toolbar -->
    <MudPaper Class="pa-4 mb-4">
        <MudGrid AlignItems="Center">
            <MudItem xs="8" md="10">
                <MudTextField @bind-Value="searchTerm" 
                              Placeholder="Search requests..." 
                              Adornment="Adornment.Start" 
                              AdornmentIcon="Icons.Material.Filled.Search" />
            </MudItem>
            <MudItem xs="4" md="2">
                <MudButton Color="Color.Primary" 
                           Variant="Variant.Filled" 
                           StartIcon="Icons.Material.Filled.Add"
                           OnClick="NavigateToCreate"
                           FullWidth="true">
                    Create
                </MudButton>
            </MudItem>
        </MudGrid>
    </MudPaper>
    
    <!-- List Component -->
    <RequestListComponent @ref="listComponent" 
                          SearchTerm="@searchTerm"
                          OnEditRequest="NavigateToEdit"
                          OnViewRequest="NavigateToDetail"
                          OnDeleteRequest="HandleDelete" />
</MudContainer>
```

#### List.razor.cs (Code-Behind)
```csharp
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sanjel.RequestManagement.Core.Services;
using Sanjel.RequestManagement.Core.Entities;

namespace Sanjel.RequestManagement.Blazor.Pages.Request;

public partial class List
{
    private string searchTerm = string.Empty;
    private RequestListComponent listComponent = default!;
    
    private void NavigateToCreate()
    {
        NavigationManager.NavigateTo("/requests/create");
    }
    
    private void NavigateToEdit(string requestId)
    {
        NavigationManager.NavigateTo($"/requests/edit/{requestId}");
    }
    
    private void NavigateToDetail(string requestId)
    {
        NavigationManager.NavigateTo($"/requests/detail/{requestId}");
    }
    
    private async Task HandleDelete(string requestId)
    {
        // Delete confirmation and service call
    }
}
```

### Create Page Example

```razor
@page "/requests/create"
@using Sanjel.RequestManagement.Core.Services
@using Sanjel.RequestManagement.Core.Entities
@inject IRequestService RequestService
@inject NavigationManager NavigationManager
@inject ISnackbar Snackbar

<PageTitle>Create Request</PageTitle>

<MudContainer MaxWidth="MaxWidth.Medium" Class="mt-4">
    <MudBreadcrumbs Items="breadcrumbs" Class="mb-4" />
    
    <MudText Typo="Typo.h4" Class="mb-4">Create New Request</MudText>
    
    <!-- Form Component -->
    <RequestFormComponent @ref="formComponent"
                          OnSave="HandleSave"
                          OnCancel="HandleCancel" />
</MudContainer>

@code {
    private RequestFormComponent formComponent = default!;
    
    private List<BreadcrumbItem> breadcrumbs = new()
    {
        new BreadcrumbItem("Home", href: "/"),
        new BreadcrumbItem("Requests", href: "/requests"),
        new BreadcrumbItem("Create", href: null, disabled: true)
    };
    
    private async Task HandleSave(Request request)
    {
        try
        {
            await RequestService.CreateAsync(request);
            Snackbar.Add("Request created successfully", Severity.Success);
            NavigationManager.NavigateTo("/requests");
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error creating request: {ex.Message}", Severity.Error);
        }
    }
    
    private void HandleCancel()
    {
        NavigationManager.NavigateTo("/requests");
    }
}
```

## Integration Points

### Dependencies

This skill integrates with:

- **MudBlazor Components**: Uses MudBlazor for consistent UI components
- **Service Interfaces**: Injects entity-specific service interfaces
- **Blazor Components**: References generated list, form, and detail components
- **Navigation Manager**: Handles page navigation and routing
- **Snackbar Service**: Provides user feedback and notifications

### Output Locations

Generated Blazor pages are placed in:
- **List Pages**: 
  - `src/{ProjectName}.Blazor/Pages/{EntityName}/List.razor`
  - `src/{ProjectName}.Blazor/Pages/{EntityName}/List.razor.cs` 
- **Create Pages**: 
  - `src/{ProjectName}.Blazor/Pages/{EntityName}/Create.razor`
  - `src/{ProjectName}.Blazor/Pages/{EntityName}/Create.razor.cs`
- **Edit Pages**: 
  - `src/{ProjectName}.Blazor/Pages/{EntityName}/Edit.razor`
  - `src/{ProjectName}.Blazor/Pages/{EntityName}/Edit.razor.cs`
- **Detail Pages**: 
  - `src/{ProjectName}.Blazor/Pages/{EntityName}/Detail.razor`
  - `src/{ProjectName}.Blazor/Pages/{EntityName}/Detail.razor.cs`

### Project Integration

The generated pages integrate with the project:
- Use project namespace patterns
- Reference existing service interfaces
- Support dependency injection patterns
- Follow established routing conventions
- Include proper page titles and metadata

## Error Handling

The skill includes comprehensive error handling:

- **File System Errors**: Graceful handling of permission and path issues
- **Metadata Validation**: Validates domain model metadata structure
- **Code Generation**: Handles edge cases in entity names and attributes
- **Template Processing**: Robust template rendering with fallbacks

## Dependencies

### Required Utilities

Uses shared functions from `project-utilities` skill:
- `detectProjectInfo()` - Project root and name detection
- `constructPagePath()` - Blazor page directory path construction  
- `findDomainModelMetadata()` - Metadata file location
- `toPascalCase()` - Name conversion utilities
- `ensureDirectoryExists()` - Safe directory creation

### External Dependencies

- **bun**: TypeScript runtime for execution
- **MudBlazor**: UI component library (referenced in templates)
- **Domain Model Metadata**: Requires parsed domain model JSON

## Workflow Integration

### Skill Orchestration

This skill is typically called:

1. **After Service Interface Generation**: Pages depend on service contracts
2. **Before Component Generation**: Pages provide structure for components
3. **During Full UI Generation**: Part of complete page + component workflow
4. **In Page-First Development**: Primary skill in new development approach

### Related Skills

- **`domain-model-parser`**: Provides input metadata
- **`service-interface-generator`**: Provides service dependencies  
- **`blazor-list-component-generator`**: Generates components used by pages
- **`blazor-form-component-generator`**: Generates form components for create/edit pages
- **`blazor-detail-component-generator`**: Generates detail components for detail pages

## Quality Standards

### Code Quality

- Follows Blazor naming conventions and coding standards
- Uses modern Blazor patterns (async/await, component parameters)
- Implements proper error handling and user feedback
- Includes responsive design and accessibility considerations

### Performance Considerations

- Supports lazy loading and component optimization
- Uses efficient data binding patterns
- Implements proper component lifecycle management
- Includes pagination for large data sets

### Maintainability

- Clear separation between page and component responsibilities
- Consistent patterns across all generated pages
- Extensible design for adding custom page features
- Standard navigation and routing patterns

## Testing Support

Generated pages support testing through:
- Clear component references for integration testing
- Separation of concerns for unit testing
- Standard navigation patterns for end-to-end testing
- Consistent error handling for test scenarios

## Future Extensions

The skill can be extended to support:
- Custom page templates based on entity metadata
- Advanced routing patterns (nested routes, parameters)
- Integration with state management libraries
- Custom layout and theming support
- Multi-language and localization features