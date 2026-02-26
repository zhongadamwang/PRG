---
name: eServiceCloud Blazor Project Creator
description: Creates enterprise-grade .NET Blazor Server solutions following eServiceCloud clean architecture patterns with 4-layer structure (Blazor, Core, BusinessProcess, Repositories), dependency injection, comprehensive testing, and production-ready configuration.
---

# eServiceCloud Blazor Project Creator

## Overview

🏗️ **Enterprise Blazor Solution Creator** 🏗️

This skill creates complete .NET Blazor Server solutions following the proven eServiceCloud architecture patterns. It establishes a clean, maintainable, and scalable foundation with proper separation of concerns, dependency injection, and comprehensive testing infrastructure.

## When To Use

Use this skill when:
- **Starting new enterprise projects** - Need proven architecture foundation
- **Migrating legacy applications** - Modernizing with clean architecture 
- **Team standardization** - Establishing consistent enterprise patterns
- **Microservices development** - Creating service boundaries
- **Training and education** - Teaching clean architecture principles
- **Proof of concepts** - Quick setup with production-ready structure
- **Project creation/modification/initialization** - When users need to create, modify, or initialize projects with enterprise architecture patterns

## Architecture Overview

### Four-Layer Clean Architecture

**eServiceCloud follows a strict layered architecture with clear responsibilities:**

1. **Presentation Layer (Blazor)** - UI components, pages, user interaction
2. **Domain Services (Core)** - Models, interfaces, cross-cutting concerns  
3. **Business Process Layer** - Pure business logic, domain rules
4. **Data Access Layer (Repositories)** - Data persistence, external integrations

### Dependency Flow
```
Blazor → Core + Repositories
BusinessProcess → Core (pure, minimal external deps)
Repositories → Core + BusinessProcess
Tests → Corresponding layers
```

## Solution Structure

**Standard eServiceCloud Project Layout:**
```
src/
├── [Prefix].[ProjectName].slnx                     # Modern solution file format
└── app/
    ├── [Prefix].[ProjectName].Blazor/              # Presentation Layer
    ├── [Prefix].[ProjectName].Blazor.Tests/        # UI Tests  
    ├── [Prefix].[ProjectName].Core/                # Domain Services & Models
    ├── [Prefix].[ProjectName].Core.Tests/          # Core Tests
    ├── [Prefix].[ProjectName].BusinessProcess/     # Business Logic
    ├── [Prefix].[ProjectName].BusinessProcess.Tests/ # Business Tests
    ├── [Prefix].[ProjectName].Repositories/        # Data Access
    └── [Prefix].[ProjectName].Repositories.Tests/  # Repository Tests
.vscode/
├── tasks.json                                      # VS Code build/dev tasks
├── launch.json                                     # Debug configurations
├── settings.json                                   # Workspace settings
└── extensions.json                                 # Recommended extensions
```

> **Note:** The `.slnx` format is Microsoft's modern XML-based solution file format that provides better tooling support, enhanced portability across development environments, and improved integration with VS Code and other non-Visual Studio editors.

## Layer Details & Responsibilities

### 1. Blazor Presentation Layer

**Purpose:** User interface, interaction handling, display logic only

**Key Directories:**
```
[Prefix].[ProjectName].Blazor/
├── Program.cs                   # Application bootstrap & DI configuration
├── App.razor                    # Root application component
├── Pages/                       # Razor pages (.razor + .razor.cs + .razor.css)
│   ├── Index.razor             # Home page
│   └── [FeatureArea]/          # Feature-based organization
├── Components/                  # Reusable UI components  
├── Shared/                     # Shared layouts and components
│   ├── MainLayout.razor        # Primary application layout
│   └── NavMenu.razor           # Navigation component
├── Services/                   # Page/component-specific services
├── Extensions/                 # DI registration extensions
├── Policies/                   # Authorization policies
├── Styles/                     # SCSS/CSS styling
├── wwwroot/                    # Static assets (CSS, JS, images)
├── _Imports.razor              # Global using statements
├── appsettings.json            # Configuration (multiple environments)
└── libman.json                 # Client-side library management
```

**Implementation Patterns:**
- **Three-file separation:** `.razor` (markup), `.razor.cs` (code-behind), `.razor.css` (styling)
- **Component services:** Each page/major component has dedicated service
- **Async everywhere:** All operations use async/await with CancellationToken
- **Error boundaries:** Global exception handling and user-friendly error display
- **Loading states:** Comprehensive loading indicators for async operations
- **Authorization:** Declarative authorization with policies and attributes

### 2. Core Domain Layer

**Purpose:** Domain models, service interfaces, cross-cutting concerns

**Key Directories:**
```
[Prefix].[ProjectName].Core/
├── Models/                     # Domain entities and DTOs
│   ├── Entities/              # Core business entities
│   ├── DTOs/                  # Data transfer objects  
│   └── ViewModels/            # UI-specific models
├── Services/                  # Service interfaces and implementations
│   ├── Interfaces/            # Repository and service contracts
│   └── Implementations/       # Cross-cutting service implementations
├── Mappers/                   # Entity ↔ DTO mapping services
├── Validators/                # FluentValidation validators
├── Exceptions/                # Custom exception types
├── Common/                    # Shared utilities and extensions
│   ├── DependencyInjectionExtensions.cs # Service registration
│   ├── ServicesExtensions.cs  # Helper extensions
│   └── Result.cs              # Result pattern implementation
├── Caching/                   # Caching interfaces and configuration
└── Config/                    # Configuration models
```

**Implementation Patterns:**
- **Repository interfaces:** All data access through abstractions
- **Result pattern:** Return Result<T> for operation outcomes
- **FluentValidation:** Business rule validation
- **Mapping services:** Bidirectional entity-DTO conversion
- **Dependency injection:** Interface-based registration with proper lifetimes
- **Async with CancellationToken:** All public methods support cancellation

### 3. Business Process Layer

**Purpose:** Pure business logic, domain rules, business workflows

**Key Characteristics:**
```
[Prefix].[ProjectName].BusinessProcess/
├── [Entity]Process.cs          # Static business rule classes
├── [Workflow]Process.cs        # Business workflow implementations
└── [Domain]Process.cs          # Domain-specific logic
```

**Implementation Patterns:**
- **Static classes/methods:** Pure functions for business rules
- **Minimal external dependencies:** Only depends on Core models + NotificationLibrary
- **Single responsibility:** One process class per business domain
- **Unit test friendly:** Easy to test in isolation
- **Immutable operations:** Business rules don't modify state directly

**Dependencies:**
```xml
<PackageReference Include="NotificationLibrary" Version="1.2.2" />
<ProjectReference Include="..\[Prefix].[ProjectName].Core\[Prefix].[ProjectName].Core.csproj" />
```

### 4. Repositories Data Access Layer

**Purpose:** Data persistence, external system integration, data mapping

**Key Directories:**
```
[Prefix].[ProjectName].Repositories/
├── [Entity]Repository.cs       # Entity-specific repositories
├── I[Entity]Repository.cs      # Repository interfaces (in Core)
├── Common/                     # Shared repository infrastructure
│   └── DependencyInjectionExtensions.cs # Repository registration
├── MDM/                       # Master Data Management integration
├── Extensions.cs              # Entity framework extensions
└── Exceptions/                # Data access specific exceptions
```

**Implementation Patterns:**
- **Async repositories:** All data operations are asynchronous
- **Performance logging:** Automatic timing with interceptors
- **Interface segregation:** Specific interfaces for different concerns
- **External system adaptation:** Unified interface for third-party integration
- **Error handling:** Data-layer specific exception handling

## Template Files & Foundation Code

**Critical:** The following foundation files must be copied from eServiceCloud reference project to ensure proper functionality. These files provide essential base classes, interfaces, and utilities that all projects depend on.

### Project Root Configuration Files

**Required Configuration Files:**
```
src/
├── .editorconfig                       # Editor configuration for consistent formatting
├── .gitattributes                      # Git attributes configuration
├── .gitignore                          # Git ignore patterns for .NET projects
└── run.sh                              # Development server startup script
```

### Core Layer Template Files

**Required Foundation Files:**
```
[Prefix].[ProjectName].Core/
├── Common/
│   ├── Pager.cs                    # Pagination support
│   ├── PagerResult.cs              # Paged query results
│   ├── CoreConstants.cs            # Application constants
│   ├── ServicesExtensions.cs       # Service registration helpers
│   ├── DependencyInjectionExtensions.cs # Core DI configuration
│   └── TimingInterceptor.cs        # Performance monitoring
├── Models/
│   ├── IModel.cs                   # Base model interface
│   └── Model.cs                    # Generic base model class
└── Services/
    ├── IMappingService.cs          # Entity-model mapping interface
    ├── IValidationService.cs       # Business validation interface
    ├── ICurrentUserService.cs      # User context interface
    └── IDependentDataService.cs    # Dependent data interface
```

### Repositories Layer Template Files

**Required Foundation Files:**
```
[Prefix].[ProjectName].Repositories/
└── Common/
    ├── IRepository.cs              # Generic repository interface
    ├── CommonRepository.cs         # Base repository implementation
    ├── CommonVersionRepository.cs  # Versioned entity repository base
    ├── DependencyInjectionExtensions.cs # Repository DI configuration
    ├── RepositoryExceptionHelper.cs # Error handling utilities
    └── BusinessProcessEventBinder.cs # Business process integration
```

### Blazor Layer Template Files

**Required Foundation Files:**
```
[Prefix].[ProjectName].Blazor/
├── Extensions/
│   └── DependencyInjectionExtensions.cs # Blazor DI configuration
├── App.razor                       # Root application component
├── _Imports.razor                  # Global using statements
└── Shared/
    ├── MainLayout.razor            # Primary layout template
    └── NavMenu.razor               # Navigation component template
```

### Template File Mapping Configuration

**Implementation Pattern:** Use a mapping configuration to specify source and destination paths:

```json
{
  "templateFiles": {
    "ProjectRoot": {
      "sourceProject": "eServiceCloud",
      "files": [
        {
          "source": ".editorconfig",
          "target": ".editorconfig",
          "replaceNamespace": false
        },
        {
          "source": ".gitattributes", 
          "target": ".gitattributes",
          "replaceNamespace": false
        },
        {
          "source": ".gitignore",
          "target": ".gitignore", 
          "replaceNamespace": false
        },
        {
          "source": "run.sh",
          "target": "run.sh",
          "replaceProjectName": true
        }
      ]
    },
    "Core": {
      "sourceProject": "Sanjel.eServiceCloud.Core",
      "files": [
        {
          "source": "Common/Pager.cs",
          "target": "Common/Pager.cs",
          "replaceNamespace": true
        },
        {
          "source": "Common/PagerResult.cs", 
          "target": "Common/PagerResult.cs",
          "replaceNamespace": true
        },
        {
          "source": "Models/IModel.cs",
          "target": "Models/IModel.cs", 
          "replaceNamespace": true
        },
        {
          "source": "Models/Model.cs",
          "target": "Models/Model.cs",
          "replaceNamespace": true
        }
      ]
    },
    "Repositories": {
      "sourceProject": "Sanjel.eServiceCloud.Repositories",
      "files": [
        {
          "source": "Common/IRepository.cs",
          "target": "Common/IRepository.cs",
          "replaceNamespace": true
        },
        {
          "source": "Common/CommonRepository.cs",
          "target": "Common/CommonRepository.cs", 
          "replaceNamespace": true
        },
        {
          "source": "Common/CommonVersionRepository.cs",
          "target": "Common/CommonVersionRepository.cs",
          "replaceNamespace": true
        }
      ]
    }
  }
}
```

**Namespace Replacement Rules:**
- Replace `Sanjel.eServiceCloud` with `[Prefix].[ProjectName]`
- Maintain relative namespace structure (e.g., `.Core.Common`, `.Repositories.Common`)
- Update using statements and type references accordingly

**Extension Mechanism:**
To add new template files, extend the configuration:
1. Add entry to appropriate project section
2. Specify source and target paths
3. Set `replaceNamespace: true` if namespace substitution needed
4. Optionally specify `replaceContent` patterns for additional substitutions

## Framework & Technology Stack

### Target Framework
- **.NET 10.0** (as per eServiceCloud standard)
- **C# 10** with nullable reference types disabled (Blazor/Core) or enabled (BusinessProcess/Repositories)
- **Implicit usings** enabled for cleaner code
- **EnforceCodeStyleInBuild** enabled for consistent formatting

### Core Dependencies

**Blazor Application:**
```xml
<PackageReference Include="Microsoft.AspNetCore.Authentication.OpenIdConnect" Version="10.0.0" />
<PackageReference Include="Microsoft.Identity.Web" Version="3.2.0" />
<PackageReference Include="Microsoft.Identity.Web.UI" Version="3.2.0" />
<PackageReference Include="Blazored.FluentValidation" Version="2.2.0" />
<PackageReference Include="Syncfusion.Blazor" Version="26.2.14" />
<PackageReference Include="Syncfusion.Blazor.Themes" Version="26.2.14" />
```

**Core Layer:**
```xml
<PackageReference Include="Castle.Core" Version="5.1.1" />
<PackageReference Include="Castle.Core.AsyncInterceptor" Version="2.1.0" />
<PackageReference Include="FluentValidation" Version="11.10.0" />
<PackageReference Include="Microsoft.Extensions.DependencyInjection.Abstractions" Version="10.0.0" />
<PackageReference Include="Microsoft.Extensions.Logging.Abstractions" Version="10.0.0" />
<PackageReference Include="SanjelData.Daos" Version="1.25.0-rc.10" />
<PackageReference Include="SanjelData.Entities" Version="1.25.0-rc.10" />
```

**Repositories Layer:**
```xml
<PackageReference Include="SanjelData.Daos" Version="1.25.0-rc.10" />
<PackageReference Include="SanjelData.Services" Version="1.25.0-rc.10" />
<PackageReference Include="BusinessProcessLibrary" Version="1.0.3-rc.6" />
<PackageReference Include="Microsoft.VisualStudio.Threading" Version="17.12.19" />
```

**Logging Infrastructure:**
```xml
<PackageReference Include="Serilog.AspNetCore" Version="8.0.2" />
<PackageReference Include="Serilog.Enrichers.Environment" Version="3.1.0" />
<PackageReference Include="Serilog.Enrichers.Process" Version="3.0.0" />
<PackageReference Include="Serilog.Enrichers.Thread" Version="4.0.0" />
<PackageReference Include="Serilog.Expressions" Version="5.0.0" />
<PackageReference Include="Serilog.Sinks.Seq" Version="8.0.0" />
```

**Testing Framework:**
```xml
<PackageReference Include="NUnit" Version="4.2.2" />
<PackageReference Include="FluentAssertions" Version="7.0.0" />
<PackageReference Include="Moq" Version="4.20.72" />
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.12.0" />
<PackageReference Include="NUnit3TestAdapter" Version="4.6.0" />
<PackageReference Include="AutoFixture" Version="4.18.1" />
```

### Development Environment Configuration

**VS Code Integration:**
The skill creates complete VS Code workspace configuration for optimal development experience:

**.vscode/tasks.json** - Build system integration:
- `build` - Compile solution with error reporting
- `clean` - Clean build artifacts
- `restore` - Restore NuGet packages
- `test` - Run unit tests with build dependency
- `watch` - Hot-reload development server
- `publish` - Production build and deployment

**.vscode/launch.json** - Debug configurations:
- `Launch Blazor Server (Development)` - Standard development debugging
- `Launch Blazor Server (HTTPS)` - HTTPS development with custom ports
- `Attach to Process` - Attach debugger to running process

**.vscode/settings.json** - Workspace optimization:
- Default solution targeting the `.slnx` file
- C# formatting and code organization settings
- Inlay hints for enhanced code readability
- Exclude patterns for build artifacts

**.vscode/extensions.json** - Recommended extensions:
- C# Dev Kit for comprehensive .NET development
- Blazor WASM Companion for Blazor-specific features  
- .NET Interactive for enhanced development experience
- Additional productivity extensions for C# development

## Dependency Injection Configuration

### Service Registration Pattern

**Program.cs Bootstrap:**
```csharp
// Core services registration
builder.Services.AddCoreDependencies(options);

// Repository layer registration  
builder.Services.AddRepositoryDependencies(options);

// Presentation layer services
builder.Services.AddBlazorDependencies();
```

**Extension Method Pattern:**
Each layer provides extension methods for clean service registration:

- `AddCoreDependencies()` - Core/Common/DependencyInjectionExtensions.cs
- `AddRepositoryDependencies()` - Repositories/Common/DependencyInjectionExtensions.cs  
- `AddBlazorDependencies()` - Blazor/Extensions/DependencyInjectionExtensions.cs

**Service Lifetimes:**
- **Singleton:** Stateless services (mapping, caching configuration)  
- **Scoped:** Repository implementations, application services
- **Transient:** Validators, lightweight stateless services

**Performance Monitoring:**
```csharp
// Automatic performance logging for repositories
services.AddScopedWithTimeLogging<IProductHaulRepository, ProductHaulRepository>();
```

## Authentication & Authorization

**Authentication Setup:**
- **Azure AD/OpenID Connect** integration
- **Microsoft Identity Web** for seamless Azure AD integration
- **Policy-based authorization** with custom policies

**Authorization Patterns:**
- **Declarative:** `[Authorize(Policy = "PolicyName")]` on controllers/pages
- **Component level:** `<AuthorizeView Policy="PolicyName">` in Blazor components
- **Service level:** Authorization checks in application services

## Configuration Management

**Multi-Environment Configuration:**
- appsettings.json (base configuration)
- appsettings.Development.json
- appsettings.Staging.json  
- appsettings.Production.json
- appsettings.Test.json

**Configuration Patterns:**
- **Strongly-typed options:** Configuration bound to C# classes
- **Environment-specific:** Override settings per environment
- **Secret management:** User secrets for development, Key Vault for production

## Testing Strategy

### Test Project Structure
Each main project has corresponding test project with same folder structure:

**Test Categories:**
- **Unit Tests:** Business logic, validation, mapping
- **Integration Tests:** Repository implementations, database operations
- **Component Tests:** Blazor component rendering and interaction

**Testing Patterns:**
- **AAA Pattern:** Arrange-Act-Assert structure
- **Descriptive naming:** `Method_Scenario_ExpectedOutcome`
- **Parameterized tests:** `[TestCase]` for multiple scenarios
- **Mock dependencies:** Moq for external dependencies
- **FluentAssertions:** Readable assertion syntax

**Test Infrastructure:**
- **Test categories:** `[Category("Unit")]`, `[Category("Integration")]`
- **Test fixtures:** Shared setup for related tests
- **Object mothers:** Test data builders for complex entities
- **Async testing:** Proper async/await patterns in tests

## Implementation Guidelines

### Code Organization Principles

1. **Feature-based folders:** Group related functionality together
2. **Single responsibility:** Each class has one reason to change
3. **Interface segregation:** Small, focused interfaces
4. **Dependency inversion:** Depend on abstractions, not implementations

### Naming Conventions

**Projects:** `[CompanyPrefix].[ProjectName].[Layer]`  
**Namespaces:** Match folder structure
**Files:** PascalCase, descriptive names
**Methods:** Verb-noun patterns (GetProductById, SaveCustomer)

### Error Handling Strategy

**Layered Error Handling:**
- **Global exception middleware** (Blazor layer)
- **Result pattern** for service operations  
- **Structured logging** with Serilog
- **User-friendly error messages** in UI

### Performance Considerations

**Async Operations:**
- `ConfigureAwait(false)` in library code
- CancellationToken support throughout
- `Task.WhenAll()` for parallel operations

**Caching Strategy:**
- **Cache-aside pattern** for frequently accessed data
- **Memory caching** for session data
- **Distributed caching** for scalability

## Build and Deployment

### Development Workflow

**Initial Setup:**
```bash
# Create directory structure
mkdir -p src/app src/resources/images
cd src

# Create solution
dotnet new sln -n [CompanyPrefix].[ProjectName]

# Create projects in app folder
cd app
dotnet new blazorserver -n [Prefix].[ProjectName].Blazor
dotnet new classlib -n [Prefix].[ProjectName].Core  
dotnet new classlib -n [Prefix].[ProjectName].BusinessProcess
dotnet new classlib -n [Prefix].[ProjectName].Repositories

# Create test projects
dotnet new nunit -n [Prefix].[ProjectName].Blazor.Tests
dotnet new nunit -n [Prefix].[ProjectName].Core.Tests
dotnet new nunit -n [Prefix].[ProjectName].BusinessProcess.Tests
dotnet new nunit -n [Prefix].[ProjectName].Repositories.Tests

# Add projects to solution
cd ..
dotnet sln add app/**/*.csproj

# Setup project references
dotnet add app/[Prefix].[ProjectName].Blazor reference app/[Prefix].[ProjectName].Core
dotnet add app/[Prefix].[ProjectName].Blazor reference app/[Prefix].[ProjectName].Repositories
dotnet add app/[Prefix].[ProjectName].BusinessProcess reference app/[Prefix].[ProjectName].Core
dotnet add app/[Prefix].[ProjectName].Repositories reference app/[Prefix].[ProjectName].Core
dotnet add app/[Prefix].[ProjectName].Repositories reference app/[Prefix].[ProjectName].BusinessProcess
```

**Template File Setup:**
```bash
# CRITICAL: Copy foundation files from eServiceCloud reference project

# Project root configuration files
cp /reference/eServiceCloud/.editorconfig .
cp /reference/eServiceCloud/.gitattributes .
cp /reference/eServiceCloud/.gitignore .
cp /reference/eServiceCloud/run.sh .

# Core layer foundation files
cp /reference/Sanjel.eServiceCloud.Core/Common/Pager.cs \
   app/[Prefix].[ProjectName].Core/Common/Pager.cs

cp /reference/Sanjel.eServiceCloud.Core/Common/PagerResult.cs \
   app/[Prefix].[ProjectName].Core/Common/PagerResult.cs

cp /reference/Sanjel.eServiceCloud.Core/Models/IModel.cs \
   app/[Prefix].[ProjectName].Core/Models/IModel.cs

cp /reference/Sanjel.eServiceCloud.Core/Models/Model.cs \
   app/[Prefix].[ProjectName].Core/Models/Model.cs

# Repositories layer foundation files  
cp /reference/Sanjel.eServiceCloud.Repositories/Common/IRepository.cs \
   app/[Prefix].[ProjectName].Repositories/Common/IRepository.cs

cp /reference/Sanjel.eServiceCloud.Repositories/Common/CommonRepository.cs \
   app/[Prefix].[ProjectName].Repositories/Common/CommonRepository.cs

cp /reference/Sanjel.eServiceCloud.Repositories/Common/CommonVersionRepository.cs \
   app/[Prefix].[ProjectName].Repositories/Common/CommonVersionRepository.cs

# Update namespaces in copied files
find app/[Prefix].[ProjectName].Core -name "*.cs" \
  -exec sed -i 's/Sanjel\.eServiceCloud/[Prefix].[ProjectName]/g' {} +

find app/[Prefix].[ProjectName].Repositories -name "*.cs" \
  -exec sed -i 's/Sanjel\.eServiceCloud/[Prefix].[ProjectName]/g' {} +

# Update project references in run.sh
sed -i 's/Sanjel\.eServiceCloud\.Blazor/[Prefix].[ProjectName].Blazor/g' run.sh
```

**Build Commands:**
```bash
dotnet restore                    # Restore NuGet packages
dotnet build                     # Build entire solution  
dotnet test                      # Run all tests
dotnet run --project app/[Prefix].[ProjectName].Blazor  # Run application
```

### Production Deployment

**Container Support:**
- Dockerfile for containerized deployment
- Docker Compose for multi-container scenarios
- Health checks for monitoring

**Environment Configuration:**
- Environment-specific appsettings files
- Azure Key Vault integration for secrets
- Application Insights for telemetry

## Global Configuration Files

**Global.json:**
```json
{
    "sdk": {
        "version": "6.0.407",
        "rollForward": "latestFeature", 
        "allowPrerelease": false
    }
}
```

**Directory.Build.props:** (Shared properties across projects)
```xml
<Project>
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>
  </PropertyGroup>
</Project>
```

**Project-Specific Properties:**
- **Blazor & Core:** `<Nullable>disable</Nullable>`
- **BusinessProcess & Repositories:** `<Nullable>enable</Nullable>` 
- **Test Projects:** `<IsPackable>false</IsPackable>` and `<IsPublishable>false</IsPublishable>`

## Success Criteria

After creating the project structure, verify:

1. **Solution builds successfully** - `dotnet build` completes without errors
2. **All tests pass** - `dotnet test` runs successfully  
3. **Application runs** - Blazor app starts and loads correctly
4. **Dependencies resolved** - All project references and NuGet packages restore
5. **Folder structure matches** - eServiceCloud architectural patterns implemented
6. **Services registered** - Dependency injection configured correctly
7. **Template files copied** - All foundation files present with correct namespaces:
   - Root: `.editorconfig`, `.gitattributes`, `.gitignore`, `run.sh` 
   - Core: `Pager.cs`, `PagerResult.cs`, `IModel.cs`, `Model.cs`, `ServicesExtensions.cs`
   - Repositories: `IRepository.cs`, `CommonRepository.cs`, `CommonVersionRepository.cs`
   - All files contain correct `[Prefix].[ProjectName]` namespace references
   - `run.sh` contains correct project references
8. **Authentication works** - Login/logout functionality if auth enabled
9. **Database connection** - Entity Framework context configured (if applicable)

**Template File Validation:**
```bash
# Verify root configuration files exist
ls -la .editorconfig .gitattributes .gitignore run.sh

# Verify foundation files exist and have correct namespaces
grep -r "[Prefix].[ProjectName]" app/*/Common/*.cs
grep -r "[Prefix].[ProjectName]" app/*/Models/*.cs

# Verify run.sh has correct project references
grep "[Prefix].[ProjectName].Blazor" run.sh

# Ensure no old eServiceCloud references remain
! grep -r "Sanjel.eServiceCloud" app/*/
! grep "Sanjel.eServiceCloud" run.sh
```

## Related Skills Integration

This skill establishes the foundation for other eServiceCloud development skills:

- **Page Creator** - Add new Blazor pages with proper service integration
- **Component Creator** - Create reusable UI components  
- **Service Creator** - Add new application services and repositories
- **Repository Creator** - Implement data access for new entities
- **Dialog Creator** - Create modal dialogs and forms
- **Grid Creator** - Implement data grids with Syncfusion components
- **Form Creator** - Build data entry forms with validation

## Template Files Management

**Foundation Files Location:**
```
templates/
├── template-manifest.json      # Configuration for template processing
├── .editorconfig               # Editor configuration  
├── .gitattributes             # Git attributes
├── .gitignore                 # Git ignore patterns
├── run.sh                     # Development server script
├── Core/
│   ├── Common/
│   │   ├── Pager.cs           # Pagination support
│   │   ├── PagerResult.cs     # Paged results container
│   │   └── ServicesExtensions.cs # DI performance logging
│   └── Models/
│       ├── IModel.cs          # Base model interface
│       └── Model.cs           # Generic model base class
└── Repositories/
    └── Common/
        ├── IRepository.cs              # Repository interface
        ├── CommonRepository.cs         # Repository base implementation
        └── CommonVersionRepository.cs  # Versioned repository base
```

**Template Processing:**
1. Read `template-manifest.json` for file mappings
2. Copy files from templates to target project locations
3. Apply namespace replacements: `Sanjel.eServiceCloud` → `[Prefix].[ProjectName]`
4. Apply additional content replacements as specified
5. Validate all files compile successfully

**Adding New Template Files:**
1. Copy the new file to appropriate `templates/[Layer]/` directory
2. Add entry to `template-manifest.json` under correct layer
3. Specify source path, target path, and replacement rules
4. Test with a sample project creation to ensure it works

**Maintenance:**
- Template files should be kept in sync with eServiceCloud reference implementation
- When eServiceCloud foundation files are updated, corresponding template files must be updated
- Version the template files along with the skill to maintain compatibility