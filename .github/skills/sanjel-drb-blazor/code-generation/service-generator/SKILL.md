---
name: service-generator
description: Generate application service architecture shells for Blazor pages and components following Single Service per Page principle. Creates minimal service class structure with dependency injection setup without generating specific methods, providing foundation for method extension by other skills.
---

# Service Generator **🤖 AI-Driven**

**Responsibility**: Generate application service architecture shells for Blazor pages/components
**Input**: Entity metadata + Repository interfaces + Feature/Page context
**Output**: Minimal service class files with DI setup (no methods)

**Approach**: **AI-Driven Architecture Shell Generation**
- Creates minimal service architecture following Single Service per Page principle
- Generates service class file with proper namespace and naming conventions
- Sets up constructor with repository dependency injection
- Provides DI registration guidance for Program.cs
- **No methods generated** - only the architectural foundation
- Methods are added by separate, focused skills
- **Output**: Service architecture shell ready for method extensions

## Description
Generate application service architecture shells for Blazor pages and components. This AI-driven skill creates the foundational service class structure with dependency injection setup, following the Single Service per Page principle without generating specific methods.

## When To Use
- Creating service layer architecture for Blazor applications
- Setting up dependency injection structure for services
- Establishing service foundations before method implementation
- Following Single Service per Page architectural pattern

## Key Features
- **AI-Driven Architecture**: Intelligent service architecture design
- **Single Service per Page**: Follows architectural best practices
- **DI Setup**: Automatic dependency injection configuration
- **Clean Architecture**: Separation of concerns and proper layering
- **Method Extension Ready**: Foundation for focused method generation

## Usage
This skill generates service architecture shells that are ready for method implementation by other focused skills.

## Input
- **Entity Metadata**: JSON output from domain-model-parser
- **Repository Interfaces**: Generated repository interface files
- **Feature/Page Context**: Specific page or component requirements

## Output
- C# service class files (.cs) located at `src/[ProjectName].Blazor/Pages/[Feature]/Services/[Feature]Service.cs`
- Minimal service architecture with:
  - Proper namespace and class naming
  - Scoped lifecycle configuration guidance
  - Constructor with dependency injection setup
  - Basic service structure ready for method additions
  - XML documentation comments
- Service registration guidance for Program.cs

## Generated Code Features
- **Single Responsibility**: One service per page/component
- **Dependency Injection**: Scoped lifecycle with proper DI registration
- **Repository Injection**: Accepts repository interfaces via constructor
- **Minimal Structure**: Empty service class with architecture foundation
- **No Methods**: By default, service has no methods - only the architectural shell
- **Ready for Extension**: Structured to accept method additions via separate skills

## Service Architecture Template

```csharp
namespace [ProjectName].Blazor.Pages.[Feature].Services;

/// <summary>
/// Application service for [Feature] page.
/// Acts as the single coordinator between presentation and data access layers.
/// </summary>
public class [Feature]Service
{
    private readonly [IRepositoryInterface] _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="[Feature]Service"/> class.
    /// </summary>
    /// <param name="repository">The repository for data access operations.</param>
    public [Feature]Service([IRepositoryInterface] repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
}
```

## Dependency Injection Registration

```csharp
// In Program.cs
builder.Services.AddScoped<Pages.[Feature].Services.[Feature]Service>();
```

## Integration with Other Skills
- **entity-class-generator**: Provides entity type information
- **repository-interface-generator**: Provides repository interfaces to inject
- **efcore-repository-generator**: Provides concrete repository implementations
- **blazor-page-generator**: Consumes this service for page/component data access
- **service-method-adder** (future skill): Adds methods to existing service classes

## Key Principles
1. **One Service Per Page**: Each page/component has exactly one corresponding service
2. **Minimal Foundation**: Generated services are architectural shells without business logic
3. **Method-Free by Default**: No CRUD or query methods are generated - those come from separate, targeted skills
4. **AI-Driven**: Generation relies on AI analysis of requirements, not deterministic scripts
5. **Extension-Ready**: Structure is designed to accept method additions incrementally

## What This Skill DOES
- Create the service class file in the correct location
- Set up proper namespace and naming conventions
- Add constructor with repository dependency injection
- Add XML documentation comments
- Provide DI registration guidance
- Establish the architectural foundation

## What This Skill DOES NOT DO
- Does NOT generate CRUD methods (Create, Read, Update, Delete)
- Does NOT generate query methods
- Does NOT generate business logic
- Does NOT generate ViewModel mapping methods
- Does NOT generate validation methods
- Does NOT generate data loading methods
- These method additions are handled by separate, focused skills

## Future Skill Integration
The generated service class is designed to be extended by future skills:
- **service-crud-generator**: Add CreateAsync, UpdateAsync, DeleteAsync methods
- **service-query-generator**: Add LoadAsync, GetByIdAsync methods
- **service-viewmodel-generator**: Add ViewModel mapping methods
- **service-dependence-generator**: Add LoadDependencesAsync methods for dropdown data

## Example Scenarios

### Scenario 1: New Feature Page
User requests: "Create a service for the ProgramRequest feature"
AI generates: `src/{Prg}.{ProjectName}.Blazor/Pages/ProgramRequest/Services/ProgramRequestService.cs` with minimal architecture

### Scenario 2: Existing Entity Integration
User requests: "Generate a service for handling Request entities in the Assignment page"
AI generates: `src/{Prg}.{ProjectName}.Blazor/Pages/Assignment/Services/AssignmentService.cs` with IRequestRepository injection

## Error Handling
- Validates that entity metadata is available
- Checks that repository interfaces exist
- Validates output directory structure
- Provides clear error messages with suggestions

## Dependencies
- Domain model metadata from `domain-model-parser`
- Repository interfaces from `repository-interface-generator`
- Project namespace conventions
- Existing page/component structure

## Best Practices
- Keep services minimal and focused on coordination
- Add methods only through dedicated, scoped skills
- Maintain one-to-one relationship with pages/components
- Use Result<T> pattern for method returns when implementing methods
- Always include CancellationToken in async methods (when methods are added)
- Document service purpose and responsibilities clearly

## UnitTests
- Unit tests should be added when methods are added to the service
- For the base service architecture, focus on DI registration and constructor validation
- Tests should be in the `src/[ProjectName].Blazor.Tests` project
