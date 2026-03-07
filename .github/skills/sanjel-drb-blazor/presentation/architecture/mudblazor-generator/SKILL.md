---
name: mudblazor-generator
description: Generate complete MudBlazor Material Design Blazor architecture. Creates a complete, working Blazor foundation with MudBlazor components, Material Design themes, icons, responsive layout system, and proper service configuration.
---

# MudBlazor Generator

**Responsibility**: Generate complete MudBlazor Material Design Blazor architecture
**Input**: Project configuration + Material Design preferences
**Output**: Complete Blazor foundation with MudBlazor components

- Install and configure MudBlazor NuGet packages
- Generate MainLayout.razor and NavMenu.razor with Material Design components
- Configure MudBlazor services in Program.cs
- Set up Material Design themes and icons
- Create responsive layout with MudBlazor responsive system
- **Output**: Complete, runnable Blazor application with MudBlazor

## Description
Generate Blazor application foundation specifically configured for MudBlazor Material Design component library. Creates a complete, working Blazor architecture with MudBlazor components, theming, and layout system.

## When To Use
- Setting up new Blazor application with MudBlazor Material Design components
- Need Material Design aesthetics and component library
- Want comprehensive component library with icons, charts, and advanced controls
- Prefer open-source component solution with MIT license
- Establishing MudBlazor-specific Blazor foundation

## Key Features
- **Material Design Implementation**: Full Material Design 3.0 compliance with MudBlazor
- **Complete Component Library**: Access to all MudBlazor components including data grids, charts, forms
- **Dark/Light Theme Support**: Built-in theme switching capabilities
- **Responsive Layout**: Material Design responsive layout system
- **Icon Integration**: Material Design Icons and Font Awesome icon support
- **Two-File Component Structure**: Strict `.razor` and `.razor.cs` separation

## Usage
This skill generates a complete Blazor application foundation with MudBlazor integration.

## Input
### Auto-Detected Parameters
- **Project Configuration**: Project name and Blazor project path (automatically detected from workspace)

### Optional Configuration
- **Dark Mode Support**: Enable dark/light theme switching (default: true)
- **Responsive Design**: Enable responsive layout features (default: true)
- **Icon Sets**: Choose icon libraries to include (Material Design Icons, Font Awesome)

## Output
### Generated Files
```
src/{Prg}.{ProjectName}.Blazor/
│── App.razor               # HTML host with MudBlazor CSS/JS references
│── App.razor.cs            # App component code-behind
│── Routes.razor            # Application routing with MudBlazor components
│── Routes.razor.cs         # Routes component code-behind
│── Layout/
│       ├── MainLayout.razor    # MudThemeProvider and MudDialogProvider layout
│       ├── MainLayout.razor.cs # Layout code-behind with theme management
│       ├── MainLayout.razor.css # MudBlazor-specific layout styles
│       ├── NavMenu.razor       # MudDrawer navigation with MudNavMenu
│       └── NavMenu.razor.cs    # Navigation code-behind with MudBlazor logic
├── Pages/
│   ├── Home/
│   │   └── Index.razor         # Default home with MudBlazor components
│   ├── Hello/
│   │   └── Index.razor         # Hello World demonstration page
│   ├── Counter/
│   │   └── Index.razor         # Example with MudButton and MudPaper
│   └── Error/
│       └── Index.razor         # MudAlert error handling
├── _Imports.razor             # MudBlazor namespace imports
├── appsettings.json           # MudBlazor theme configuration
├── Program.cs                 # MudServices registration
└── ProjectName.Blazor.csproj  # MudBlazor NuGet package references
```

### Package References
```xml
<PackageReference Include="MudBlazor" Version="6.11.2" />
```

### Service Configuration
```csharp
// Program.cs additions
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
});
```

### Generated Configuration
- **Theme System**: MudThemeProvider with Material Design 3.0 themes
- **Component Integration**: All MudBlazor components available project-wide
- **Icon Support**: Material Design Icons integrated
- **Responsive Breakpoints**: Material Design breakpoint system
- **Color Palette**: Full Material Design color system

## MudBlazor Features Included

### Layout Components
- **MudDrawer**: Responsive navigation drawer with Material Design behavior
- **MudAppBar**: Material Design app bar with proper elevation and theming
- **MudMainContent**: Content area with proper Material Design spacing
- **MudContainer**: Responsive container with Material Design grid system

### Theme System
- **MudThemeProvider**: Complete theme management system
- **Dark Mode**: Built-in dark/light theme switching
- **Color Customization**: Material Design 3.0 color system
- **Typography**: Material Design typography scale

### Interactive Components
- **MudButton**: Material Design button variants
- **MudTextField**: Material Design text inputs with validation
- **MudSelect**: Material Design select components
- **MudDialog**: Material Design modal dialogs
- **MudSnackbar**: Material Design notifications
- **MudTable**: Advanced data grid with sorting, filtering, pagination

### Development Features
- **Hot Reload Support**: Full support for Blazor hot reload
- **Component Explorer**: Easy component discovery and usage

## Code Generation Constraints & Scope Control

**STRICT MVP (Minimum Viable Product) Approach:**
- **ONLY ONE Component**: Generate exactly ONE basic component per session
- **NO Complex Directory Structure**: Do NOT create multiple folders or complex hierarchies
- **NO Supporting Services**: Do NOT generate service classes, configuration classes, or helper classes
- **Update Existing Files Only**: Prefer updating existing files over creating new ones

**ABSOLUTE Component Generation Limits:**
- **MAXIMUM 1 Component Per Session**: Generate only ONE simple component
- **MAXIMUM 3 Files Total**: One component file + minimal usage example only
- **NO Subdirectories**: Place components in existing directories only
- **NO Custom Services**: Use built-in Blazor and MudBlazor services only

**CRITICAL RESTRAINT RULES:**

**Rule 1 - ABSOLUTE MINIMAL SCOPE (绝不可过多发挥):**
- ✅ **ONLY THREE CORE TASKS**: Install packages + Configure basic component + Create simple page
- ❌ **NO EXTRA CREATIVITY**: Do NOT add fancy features, styling, services, or enhancements
- ❌ **NO ARCHITECTURAL COMPLEXITY**: Avoid enterprise patterns, design systems, or advanced abstractions
- ❌ **STICK TO BASICS**: Only what's absolutely required to make MudBlazor work
- ⚠️ **RESIST TEMPTATION**: Do not showcase expertise by adding "nice-to-have" features

**STRICT PAGE GENERATION RULES (页面生成严格约束):**
- ✅ **ONLY ONE HELLO WORLD PAGE**: Generate exactly ONE basic "Hello World" demonstration page
- ❌ **NO COMPLEX EXAMPLES**: Do NOT create complex data examples, business logic, or realistic scenarios  
- ❌ **NO MULTIPLE PAGES**: Only create ONE single demonstration page
- ❌ **NO ADVANCED FEATURES**: Page must only show basic component rendering, no interactions
- ✅ **HELLO WORLD CONTENT**: Page should display "Hello World" text + minimal component usage
- ❌ **NO DATA MODELS**: Do NOT create custom classes, data lists, or mock data structures
- ❌ **NO BUSINESS LOGIC**: Page should have zero business logic or complex scenarios
- ✅ **SIMPLE ROUTING**: Only add basic @page directive, no complex routing setup
- ✅ **REQUIRED: ROUTE COMPONENT**: All pages must include @page directive for routing
- ✅ **REQUIRED: PAGES DIRECTORY**: All pages must be placed in Pages/ directory

**MANDATORY PAGE PLACEMENT:**
- **Directory Path**: `Pages/` (required for all generated pages)
- **Route Components**: All pages must include `@page` directive for proper routing
- **File Structure**: Follow Blazor convention with pages in Pages/ directory

**HELLO WORLD PAGE TEMPLATE:**
```razor
@page "/mudblazor-hello"

<h3>Hello World</h3>
<p>Basic MudBlazor component test</p>

<SimpleComponent />
```

**PAGE FILE LOCATION:**
- **Full Path**: `src/{Prg}.{ProjectName}.Blazor/Pages/Hello/Index.razor`
- **Required Directory**: `Pages/` (mandatory placement)

**Rule 2 - COMPILATION SUCCESS GUARANTEE (确保编译成功):**
- ✅ **COMPILATION FIRST**: Every generated file MUST compile without errors
- ✅ **MINIMAL BUT COMPLETE**: Use the absolute minimum code that still works
- ✅ **TEST BUILD**: Ensure all references, imports, and syntax are correct
- ❌ **NO BROKEN CODE**: Avoid incomplete implementations or missing dependencies
- ⚠️ **QUALITY OVER QUANTITY**: Better to generate 2 working files than 5 broken ones
- **Theme Editor**: Runtime theme customization capabilities
- **Validation Integration**: Seamless form validation with MudBlazor components

## Installation & Setup

### Automated Setup
The skill automatically configures:
1. **NuGet Package Installation**: MudBlazor package reference in `.csproj`
2. **Service Registration**: MudServices configuration in `Program.cs`
3. **CSS Resources**: MudBlazor CSS files linked in `App.razor`
4. **JavaScript**: MudBlazor JavaScript files integrated
5. **Theme Configuration**: Default Material Design theme setup

### Post-Generation Steps
1. **Restore Packages**: `dotnet restore` (automatically includes MudBlazor)
2. **Build Project**: `dotnet build`
3. **Run Application**: `dotnet run`
4. **Verify Setup**: Check Material Design components render correctly

### Verification Checklist
- ✅ Project builds successfully with MudBlazor references
- ✅ Material Design theming applied correctly
- ✅ MudDrawer navigation functions properly
- ✅ MudBlazor components render with proper styling
- ✅ Dark mode toggle works (if enabled)
- ✅ Responsive layout adapts to screen sizes
- ✅ Material Design icons display correctly

## Customization Options

### Theme Customization
The generated setup includes theme customization capabilities:
- **Primary Colors**: Material Design color palette
- **Typography**: Material Design type scale
- **Spacing**: Material Design spacing system
- **Elevation**: Material Design shadow system
- **Border Radius**: Material Design shape system

### Component Variants
- **Button Styles**: Text, Contained, Outlined button variants
- **Input Types**: Filled, Outlined text field variants
- **Navigation**: Permanent, Temporary, Mini drawer variants
- **Layout**: Fixed, Flexible layout options

## Integration Points
### Prerequisites
- Basic Blazor project structure must exist
- .NET 6.0 or later runtime

### Integrates With
- `blazor-data-integration-generator`: MudTable data integration
- `blazor-page-pattern-generator`: MudBlazor page templates
- `blazor-theme-generator`: Advanced MudBlazor theme customization
- `project-utilities`: Shared project management utilities

### Provides Foundation For
- Material Design page development
- Advanced MudBlazor component usage
- Data-driven MudTable implementations
- Form development with MudForm components

## Error Handling
- **Missing Project**: Detects and reports missing Blazor project structure
- **Package Conflicts**: Handles existing MudBlazor package references
- **Version Compatibility**: Ensures compatible MudBlazor and .NET versions
- **Template Errors**: Provides detailed error reporting for template generation failures

## Notes
- Uses latest stable MudBlazor version (6.11.2)
- Generates production-ready Material Design components
- Includes comprehensive MudBlazor service configuration
- Follows Material Design 3.0 guidelines
- Supports all modern browsers with MudBlazor compatibility
- All components follow strict `.razor`/`.razor.cs` code separation
````

## Code Generation Constraints & Scope Control

**STRICT MVP (Minimum Viable Product) Approach:**
- **ONLY ONE Component**: Generate exactly ONE basic component per session
- **NO Complex Directory Structure**: Do NOT create multiple folders or complex hierarchies
- **NO Supporting Services**: Do NOT generate service classes, configuration classes, or helper classes
- **Update Existing Files Only**: Prefer updating existing files over creating new ones

**ABSOLUTE Component Generation Limits:**
- **MAXIMUM 1 Component Per Session**: Generate only ONE simple component
- **MAXIMUM 3 Files Total**: One component file + minimal usage example only
- **NO Subdirectories**: Place components in existing directories only
- **NO Custom Services**: Use built-in Blazor and MudBlazor services only

**CRITICAL RESTRAINT RULES:**

**Rule 1 - ABSOLUTE MINIMAL SCOPE (绝不可过多发挥):**
- ✅ **ONLY THREE CORE TASKS**: Install packages + Configure basic component + Create simple page
- ❌ **NO EXTRA CREATIVITY**: Do NOT add fancy features, styling, services, or enhancements
- ❌ **NO ARCHITECTURAL COMPLEXITY**: Avoid enterprise patterns, design systems, or advanced abstractions
- ❌ **STICK TO BASICS**: Only what's absolutely required to make MudBlazor work
- ⚠️ **RESIST TEMPTATION**: Do not showcase expertise by adding "nice-to-have" features

**STRICT PAGE GENERATION RULES (页面生成严格约束):**
- ✅ **ONLY ONE HELLO WORLD PAGE**: Generate exactly ONE basic "Hello World" demonstration page
- ❌ **NO COMPLEX EXAMPLES**: Do NOT create complex data examples, business logic, or realistic scenarios  
- ❌ **NO MULTIPLE PAGES**: Only create ONE single demonstration page
- ❌ **NO ADVANCED FEATURES**: Page must only show basic component rendering, no interactions
- ✅ **HELLO WORLD CONTENT**: Page should display "Hello World" text + minimal component usage
- ❌ **NO DATA MODELS**: Do NOT create custom classes, data lists, or mock data structures
- ❌ **NO BUSINESS LOGIC**: Page should have zero business logic or complex scenarios
- ✅ **SIMPLE ROUTING**: Only add basic @page directive, no complex routing setup

**HELLO WORLD PAGE TEMPLATE:**
```razor
@page "/mudblazor-hello"

<h3>Hello World</h3>
<p>Basic MudBlazor component test</p>

<SimpleComponent />
```

**Rule 2 - COMPILATION SUCCESS GUARANTEE (确保编译成功):**
- ✅ **COMPILATION FIRST**: Every generated file MUST compile without errors
- ✅ **MINIMAL BUT COMPLETE**: Use the absolute minimum code that still works
- ✅ **TEST BUILD**: Ensure all references, imports, and syntax are correct
- ❌ **NO BROKEN CODE**: Avoid incomplete implementations or missing dependencies
- ⚠️ **QUALITY OVER QUANTITY**: Better to generate 2 working files than 5 broken ones

**FINAL VERIFICATION CHECKLIST:**
1. Can the project build with `dotnet build`?
2. Are there only the essential 3 files (package refs + component + example)?
3. Does the component do exactly ONE thing (display data with MudBlazor)?
4. Is there ZERO unnecessary complexity?
