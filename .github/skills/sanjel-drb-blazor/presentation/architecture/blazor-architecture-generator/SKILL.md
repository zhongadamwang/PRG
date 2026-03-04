# Blazor Architecture Generator

## Description
Generate complete Blazor application foundation with a specific component library (direct implementation). Creates working Blazor architecture with concrete component library implementation, no abstraction layers.

## When To Use
- Initial Blazor project setup after basic project structure exists
- Need to establish foundational Blazor architecture with layout, navigation, and component library
- Setting up new Blazor application with specific component library choice
- Creating baseline Blazor setup before adding domain-specific pages and features

## Key Design Principles
- **Direct Implementation**: Generates concrete, working code with specific component library
- **No Abstraction Layers**: Code uses actual component library components directly
- **Component Library Switching**: Achieved through Skills re-generation, not code abstractions
- **Extensible Design**: Structured to easily support multiple component libraries
- **Two-File Component Structure**: All Blazor components follow strict separation:
  - `.razor` files contain only markup and UI structure
  - `.razor.cs` files contain all C# code logic and component behavior

## Usage
This skill uses bun to run TypeScript scripts for Blazor architecture generation.

## Input
### Required Parameters
- **Project Configuration**: Project name and Blazor project path (auto-detected)
- **Component Library**: Choice of component library (`mudblazor`, `syncfusion`, `bootstrap`, `none`)

### Optional Parameters
- **Authentication**: Enable authentication scaffolding (default: false)
- **Responsive Design**: Enable responsive layout features (default: true) 
- **Dark Mode Support**: Include dark/light theme switching (default: false)
- **License Configuration**: For commercial libraries (Syncfusion, Telerik, etc.)

## Output
### Generated Files
```
src/Sanjel.RequestManagement.Blazor/
├── Components/
│   ├── App.razor               # HTML host page with CSS/JS references
│   ├── App.razor.cs            # App component code-behind
│   ├── Routes.razor            # Application routing configuration  
│   ├── Routes.razor.cs         # Routes component code-behind
│   └── Layout/
│       ├── MainLayout.razor    # Main layout markup using selected component library
│       ├── MainLayout.razor.cs # Main layout code-behind with component logic
│       ├── MainLayout.razor.css # Layout-specific styles
│       ├── NavMenu.razor       # Navigation menu markup with component library components
│       └── NavMenu.razor.cs    # Navigation menu code-behind with component logic
├── Pages/
│   ├── Home.razor             # Default home page markup
│   ├── Home.razor.cs          # Default home page code-behind
│   ├── Counter.razor          # Example page markup (optional)
│   ├── Counter.razor.cs       # Example page code-behind (optional)
│   ├── Error.razor            # Error handling page markup
│   └── Error.razor.cs         # Error handling page code-behind
├── wwwroot/
│   ├── css/
│   │   └── site.css          # Base application styles
│   ├── js/
│   │   └── site.js           # Base application scripts
│   └── favicon.ico           # Application icon
├── _Imports.razor             # Component library namespace imports
├── appsettings.json           # Component library configuration
├── Program.cs                 # Service registrations for selected library
└── ProjectName.Blazor.csproj  # Project file with NuGet package references
```

### Installation & Configuration Files
```
Generated Installation Package/
├── installation-guide.md      # Complete setup instructions for selected library
├── package-installation.md    # NuGet package installation steps  
└── configuration-notes.md     # Library-specific configuration details
```

### Updated Files
- **.csproj**: NuGet package references for selected component library
- **Program.cs**: Service registrations and middleware configuration
- **appsettings.json**: Component library-specific configuration sections

## Supported Component Libraries

### MudBlazor (Default)
- Modern Material Design components
- Comprehensive component set
- Good documentation and community support
- MIT license (free)

### Syncfusion Blazor
- Enterprise-grade component library  
- Extensive controls including charts, grids, scheduling
- Commercial license required
- Advanced theming and customization

### Bootstrap (Basic)
- Standard Bootstrap styling
- Minimal component dependencies
- Basic layout and navigation only
- Free and lightweight

### None (Minimal)
- No component library dependencies
- Basic HTML/CSS layout structure
- Manual styling required
- Maximum flexibility

## Component Library Extensibility

### Adding New Component Libraries
The skill is designed with extensibility in mind:

1. **Library Configuration**: Add new library to `ComponentLibraryType` enum
2. **Template System**: Create library-specific template sets 
3. **Package Management**: Define NuGet packages and versions
4. **Service Registration**: Implement library-specific DI setup
5. **Style Integration**: Configure CSS/theme file generation

### Extension Points
```typescript
// Library-specific configurations
interface ComponentLibraryConfig {
  name: string;
  packageReferences: PackageReference[];
  serviceRegistrations: string[];
  namespaceImports: string[];
  styleFiles: string[];
  licenseConfig?: LicenseConfig;
}
```

## Script Execution
```bash
# From skill directory
bun run scripts/generate-blazor-architecture.ts

# With specific component library
bun run scripts/generate-blazor-architecture.ts --library=mudblazor
bun run scripts/generate-blazor-architecture.ts --library=syncfusion --license-key=your-key
bun run scripts/generate-blazor-architecture.ts --library=bootstrap
bun run scripts/generate-blazor-architecture.ts --library=none
```

## Post-Generation Setup

### Automated Package Installation
The skill generates a complete `.csproj` file with all required package references:

**MudBlazor:**
```xml
<PackageReference Include="MudBlazor" Version="6.11.2" />
```

**Syncfusion:**
```xml
<PackageReference Include="Syncfusion.Blazor" Version="22.1.39" />
<PackageReference Include="Syncfusion.Blazor.Themes" Version="22.1.39" />
```

**Bootstrap/Minimal:**
```xml
<!-- Uses CDN resources, no additional packages -->
```

### Manual Installation Steps
1. **Restore Packages**: `dotnet restore`
2. **License Setup** (Syncfusion only): Add license key to `appsettings.json`
3. **Database Configuration**: Update connection string in `appsettings.json`
4. **Build & Run**: `dotnet build && dotnet run`

### Generated Installation Guide
Each execution creates a comprehensive `installation-guide.md` with:
- Step-by-step setup instructions
- Library-specific configuration details  
- License requirements and setup (where applicable)
- Troubleshooting common issues
- Links to official documentation

## Template Structure
The skill supports multiple component libraries through organized template directories:
```
templates/
├── mudblazor/          # MudBlazor Material Design components
│   ├── .csproj.template           # Project file with MudBlazor packages
│   ├── App.razor.template         # HTML host with Material Design resources
│   ├── Routes.razor.template      # MudBlazor routing with Material components
│   ├── MainLayout.razor.template  # Material Design layout structure
│   ├── NavMenu.razor.template     # MudBlazor navigation components
│   └── [additional templates...]
├── syncfusion/         # Syncfusion enterprise components  
│   ├── .csproj.template           # Project file with Syncfusion packages
│   ├── App.razor.template         # HTML host with Syncfusion resources
│   ├── Routes.razor.template      # Syncfusion routing configuration
│   └── [additional templates...]
├── bootstrap/          # Bootstrap styling components
│   ├── .csproj.template           # Project file with Bootstrap CDN setup
│   ├── App.razor.template         # HTML host with Bootstrap CDN links
│   └── [additional templates...]
├── minimal/            # No component library (pure HTML/CSS)
│   ├── .csproj.template           # Basic project file, no UI packages
│   ├── App.razor.template         # Minimal HTML host page
│   └── [additional templates...]
├── common/             # Shared page templates (all libraries)
│   ├── Home.razor.template        # Generic home page
│   ├── Counter.razor.template     # Universal counter example
│   ├── Error.razor.template       # Standard error page
│   ├── appsettings.json.template  # Configuration template
│   ├── site.js.template          # Base JavaScript utilities
│   └── installation-guide.md.template # Setup instructions
└── template-config.json # Library configurations and metadata
```

Each component library provides complete installation configuration:
- **NuGet Packages**: Automated package reference setup
- **Static Resources**: CSS/JS file references (CDN or local)
- **Service Registration**: Dependency injection configuration  
- **License Management**: Automated license key handling (where required)
- **Installation Guide**: Complete setup documentation

## Verification Steps
After execution, verify for each component library:

**Common Verification:**
1. ✅ Project builds successfully: `dotnet build`
2. ✅ Project runs without errors: `dotnet run`
3. ✅ Home page loads correctly
4. ✅ Navigation menu functions properly
5. ✅ Responsive layout works on different screen sizes

**Component Library Specific:**
- **MudBlazor**: Material Design theming, drawer sidebar, MudBlazor components render
- **Syncfusion**: Syncfusion controls load, sidebar navigation, proper licensing (if configured)
- **Bootstrap**: Bootstrap styling applied, collapsible navigation, responsive grid system
- **Minimal**: Custom CSS styling, basic HTML navigation, no external dependencies

## Integration with Other Skills
### Prerequisites
- `project-creator`: Basic project structure must exist
- `project-utilities`: Shared utilities for project detection and formatting

### Provides Foundation For
- `blazor-data-integration-generator`: Data layer integration
- `blazor-page-pattern-generator`: Domain-specific page generation
- `blazor-theme-generator`: Advanced styling and theming
- `blazor-*-adapter`: Component library switching/replacement

### Workflow Position
**Layer 1: Architecture Foundation Layer [Priority 1]**
- First UI skill to run in the Blazor development workflow
- Establishes foundation for all subsequent Blazor skills
- Creates baseline that other skills build upon

## Error Handling
- **Missing Project**: Skill will detect and report if Blazor project doesn't exist
- **Package Installation**: Handles NuGet package installation failures gracefully
- **File Conflicts**: Provides options for overwriting or preserving existing files
- **License Issues**: Clear guidance for commercial library license configuration

## Notes
- Generates concrete implementation code, not abstraction layers
- Component library switching handled by re-running skill with different library parameter
- Designed for single component library per application (no mixing)
- All generated code follows established formatting and naming conventions (strict .razor/.razor.cs separation)
- Integrates with project-utilities for consistent file structure and formatting
- **All four component libraries fully supported**: MudBlazor, Syncfusion, Bootstrap, and Minimal configurations