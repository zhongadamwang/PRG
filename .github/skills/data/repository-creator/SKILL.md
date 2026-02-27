---
name: repository-creator
description: Generate Repository classes for business entities using appropriate templates based on entity characteristics. Automatically detects if entity has ObjectVersion base class and selects correct repository pattern. Requires entity name input and uses entity-info-getter to retrieve entity details.
---

# Repository Creator

## Skill Purpose

This skill generates Repository classes for business entities in the Sanjel Request Management system. It automatically selects the appropriate repository template based on entity characteristics, particularly whether the entity inherits from ObjectVersion base classes.

## When to Use

**Primary Triggers:**
- When users need to create repository classes for new entities
- During development tasks requiring repository pattern implementation
- When adding new business entities to the system that need data access layer

**Usage Scenarios:**
- "Create repository for ProgramRequest entity"
- "Generate repository for StickDiagramTemplate"
- "I need a repository for Employee entity"
- "Create repository class for new entity"

## How It Works

### Template Selection Logic

The skill uses two different repository templates based on entity inheritance and characteristics:

#### 1. **Standard Repository** (`Repository.cs`)
**Used for entities that do NOT have ObjectVersion/Version tracking capabilities**

**Characteristics:**
- Inherits from `CommonRepository<Entity, IDataService>`
- Simple constructor with only `IDataService` dependency
- Used for regular business entities without versioning/auditing

**Entity Examples:**
- `ProgramRequest`: No version tracking, simple CRUD operations
- `CallSheet`: Standard business entity without audit trail
- `Employee`: Basic business entity without version management

**Generated Pattern:**
```csharp
public sealed class EntityRepository : CommonRepository<Entity, IDataService>, IEntityRepository
{
    public EntityRepository(IDataService dataService)
        : base(dataService)
    {
    }
}
```

#### 2. **Version Repository** (`VersionRepository.cs`)  
**Used for entities that DO have ObjectVersion/Version tracking capabilities**

**Characteristics:**
- Inherits from `CommonVersionRepository<Entity, IDataService>`
- Constructor with `IDataService` AND `ICurrentUserService` dependencies
- Used for entities requiring audit trails, versioning, and user tracking

**Entity Detection Criteria:**
The skill detects ObjectVersion entities by checking for **ALL** of these properties:
- `Version { get; set; }` - Version number tracking
- `ModifiedUserId { get; set; }` - User who made changes
- `EffectiveStartDateTime { get; set; }` - When version becomes effective
- `EffectiveEndDateTime { get; set; }` - When version expires

**Entity Examples:**
- `StickDiagramTemplate`: Has versioning, audit trail, and effective date management
- Other template entities with version control requirements

**Generated Pattern:**
```csharp  
public sealed class EntityRepository : CommonVersionRepository<Entity, IDataService>, IEntityRepository
{
    public EntityRepository(IDataService dataService, ICurrentUserService currentUserService)
        : base(dataService, currentUserService)
    {
    }
}
```

### ObjectVersion Detection Rules

The skill uses **strict detection rules** to ensure accuracy:

#### ✅ **Version Repository Required When:**
- Entity contains **ALL** ObjectVersion properties:
  ```
  ✓ Version { get; set; }
  ✓ ModifiedUserId { get; set; }  
  ✓ EffectiveStartDateTime { get; set; }
  ✓ EffectiveEndDateTime { get; set; }
  ```
- Additional version properties may include:
  - `ModifiedDateTime { get; set; }`
  - `SystemId { get; set; }`
  - `OperationType { get; set; }`

#### ❌ **Standard Repository Used When:**
- Entity lacks ANY of the required ObjectVersion properties
- Entity only has basic properties like `Id`, `Name`, `Description`
- No versioning or audit tracking capabilities

### Comparison Examples

#### Standard Entity (CallSheet):
```
Properties Found:
✓ CallSheetNumber { get; set; }
✓ Status { get; set; }
✓ CallDateTime { get; set; }
✓ ServicePoint { get; set; }
✓ Id { get; set; }
✓ Name { get; set; }

❌ Missing: Version, ModifiedUserId, EffectiveStartDateTime, EffectiveEndDateTime
→ Result: Standard Repository
```

#### Version Entity (StickDiagramTemplate):
```
Properties Found:
✓ TypeName { get; set; }
✓ Json { get; set; }
✓ Version { get; set; } ← ObjectVersion property
✓ ModifiedUserId { get; set; } ← ObjectVersion property  
✓ EffectiveStartDateTime { get; set; } ← ObjectVersion property
✓ EffectiveEndDateTime { get; set; } ← ObjectVersion property
✓ Id { get; set; }
✓ Name { get; set; }

✅ Has ALL ObjectVersion properties
→ Result: Version Repository
```

### Required Information

**User Must Provide (via Interactive Prompt):**
- **EntityName**: The name of the business entity - **MUST be obtained through user interaction**
  - This skill will always prompt the user for the entity name if not provided
  - Can be provided as simple name or full namespace path
  - Examples: 
    - `ProgramRequest`
    - `Sesi.SanjelData.Entities.BusinessEntities.Engineering.ProgramDesign.ProgramRequest`
    - `StickDiagramTemplate`

**Automatically Derived:**
- **EntityClassFullPath**: Complete namespace path of the entity
- **EntityDataServiceInterfaceFullPath**: Corresponding DataService interface path
- **ProjectName**: Extracted from solution/project files
- **Template Selection**: Based on entity inheritance analysis

### DataService Interface Mapping

The skill automatically maps entity classes to their corresponding DataService interfaces using established patterns:

**Pattern Rules:**
- Entity: `Sesi.SanjelData.Entities.BusinessEntities.*`
- DataService: `Sesi.SanjelData.Services.Interfaces.BusinessEntities.*`
- Interface naming: `I[EntityName]Service`

**Examples:**
```
Entity: Sesi.SanjelData.Entities.BusinessEntities.Engineering.ProgramDesign.ProgramRequest
DataService: Sesi.SanjelData.Services.Interfaces.BusinessEntities.Engineering.ProgramDesign.IProgramRequestService

Entity: Sesi.SanjelData.Entities.BusinessEntities.Engineering.ProgramDesign.Template.StickDiagramTemplate
DataService: Sesi.SanjelData.Services.Interfaces.BusinessEntities.Engineering.ProgramDesign.Template.IStickDiagramTemplateService
```

## Implementation Process

### Step 1: Entity Name Validation
1. Prompt user for entity name if not provided
2. Use **entity-info-getter** skill to validate and retrieve entity details
3. Verify entity exists in Sesi.SanjelData.Entities namespace
4. Extract complete entity information including inheritance hierarchy

### Step 2: Template Selection
1. Analyze entity inheritance chain using entity-info-getter results
2. Check if entity inherits from ObjectVersion base classes
3. Select appropriate template:
   - ObjectVersion inheritance → `VersionRepository.cs`
   - Standard inheritance → `Repository.cs`

### Step 3: Variable Resolution
1. **EntityName**: Extract class name from full entity path
2. **EntityClassFullPath**: Use complete namespace from entity-info-getter
3. **EntityDataServiceInterfaceFullPath**: Apply mapping patterns to derive DataService interface
4. **ProjectName**: Extract from solution file name (e.g., "RequestManagement" from "Sanjel.RequestManagement.slnx")

### Step 4: File Generation
1. Load appropriate template file
2. Replace template variables with resolved values
3. Generate output file as `{EntityName}Repository.cs`
4. Save to appropriate repository directory

## Template Variables

All templates use these variables for content replacement:

- `{{EntityName}}`: Simple entity class name (e.g., "ProgramRequest")
- `{{EntityClassFullPath}}`: Complete entity namespace path
- `{{EntityDataServiceInterfaceFullPath}}`: Complete DataService interface path  
- `{{ProjectName}}`: Project name for namespace construction

## Output Structure

**Generated Repository Features:**
- Interface definition: `I{EntityName}Repository`
- Implementation class: `{EntityName}Repository`
- Proper namespace: `Sanjel.{ProjectName}.Repositories`
- Correct base class inheritance
- Appropriate constructor dependencies

**Standard Repository Output:**
```csharp
using Entity = Sesi.SanjelData.Entities.BusinessEntities.Engineering.ProgramDesign.ProgramRequest;
using IDataService = Sesi.SanjelData.Services.Interfaces.BusinessEntities.Engineering.ProgramDesign.IProgramRequestService;

namespace Sanjel.RequestManagement.Repositories
{
    public interface IProgramRequestRepository : Sanjel.RequestManagement.Repositories.Common.IRepository<Entity>
    {
    }

    public sealed class ProgramRequestRepository : Sanjel.RequestManagement.Repositories.Common.CommonRepository<Entity, IDataService>, IProgramRequestRepository
    {
        public ProgramRequestRepository(IDataService dataService)
            : base(dataService)
        {
        }
    }
}
```

**Version Repository Output:**
```csharp
using Entity = Sesi.SanjelData.Entities.BusinessEntities.Engineering.ProgramDesign.Template.StickDiagramTemplate;
using IDataService = Sesi.SanjelData.Services.Interfaces.BusinessEntities.Engineering.ProgramDesign.Template.IStickDiagramTemplateService;

namespace Sanjel.RequestManagement.Repositories
{
    public interface IStickDiagramTemplateRepository : Sanjel.RequestManagement.Repositories.Common.IRepository<Entity>
    {
    }

    public sealed class StickDiagramTemplateRepository : Sanjel.RequestManagement.Repositories.Common.CommonVersionRepository<Entity, IDataService>, IStickDiagramTemplateRepository
    {
        public StickDiagramTemplateRepository(IDataService dataService, Sanjel.RequestManagement.Core.Services.ICurrentUserService currentUserService)
            : base(dataService, currentUserService)
        {
        }
    }
}
```

## AI Agent Guidelines

### User Interaction Requirements

**🔴 CRITICAL: Always ask for EntityName if not provided**
Repository creation **CANNOT** proceed without a valid entity name. The AI must:
- Clearly prompt: *"Please provide the entity name for which you want to create a repository (e.g., ProgramRequest, CallSheet, or full namespace path)"*
- Accept partial names but validate through entity-info-getter
- Display entity details and template selection reasoning for user confirmation
- **NEVER** assume or guess entity names

**Entity Validation Process:**
1. Use entity-info-getter skill to retrieve complete entity information
2. Analyze ObjectVersion properties using strict detection rules
3. Display entity details AND detection reasoning for user confirmation
4. Explain template selection logic before proceeding

### Implementation Steps

1. **Validate Input**
   ```
   If EntityName not provided:
       Ask user: "Please provide the entity name (e.g., ProgramRequest or full path)"
   ```

2. **Get Entity Information**
   ```
   Use entity-info-getter skill:
   cd tools/TypeInfoRetriever && dotnet run --project TypeInfoRetriever.csproj "{entityFullPath}"
   ```

3. **Analyze ObjectVersion Detection**
   ```
   Analyze entity properties for ObjectVersion characteristics:
   - Check for Version { get; set; }
   - Check for ModifiedUserId { get; set; }
   - Check for EffectiveStartDateTime { get; set; }
   - Check for EffectiveEndDateTime { get; set; }
   - Explain detection results and reasoning to user
   ```

4. **Confirm Template Selection**
   ```
   Display analysis results:
   - Entity: {EntityName}
   - ObjectVersion Properties Found: {foundCount}/{requiredCount}
   - Template Selected: {Standard/Version} Repository
   - Reasoning: {why this template was selected}
   Ask for user confirmation before proceeding
   ```

5. **Execute Repository Generation**
   ```
   Run create-repository.ts with confirmed parameters
   Verify successful generation with correct template
   Display output location and validate contents
   ```

6. **Post-Generation Validation**
   ```
   Confirm generated file uses correct:
   - Base class (CommonRepository vs CommonVersionRepository)
   - Constructor parameters (IDataService vs IDataService + ICurrentUserService)
   - Template selection reasoning was accurate
   ```

## Best Practices

### For Developers

**🎯 Entity Naming**
- Use consistent entity naming conventions
- Prefer full namespace paths when dealing with ambiguous names
- Verify entity exists before starting repository creation

**📁 File Organization**
- Generated files go to appropriate repository directory
- Follow existing project structure patterns
- Maintain consistent namespace organization

**🔄 Dependency Management**
- Ensure proper dependency injection setup for generated repositories
- Update registration if using DI containers
- Test repository functionality after generation

### For AI Agents

**🤖 Validation Requirements**
- ALWAYS validate entity existence using entity-info-getter
- NEVER proceed without user confirmation of entity details
- Ensure clear communication about template selection reasoning

**📊 Error Handling**
- Handle entity not found scenarios gracefully
- Provide clear error messages for resolution steps
- Suggest alternative entity names when exact match fails

**🔄 Follow-up Actions**
- Confirm successful file generation
- Provide file location and next steps
- Suggest dependency injection registration if needed

## Troubleshooting

### Common Issues

**❌ Entity Not Found**
```
Solutions:
- Verify entity name spelling and case sensitivity
- Use entity-info-getter to list available entities
- Check if entity is in correct namespace (Sesi.SanjelData.Entities)
- Try with full namespace path
```

**❌ Template Selection Errors**
```
Solutions:
- Verify entity inheritance analysis results
- Check entity-info-getter output for ObjectVersion references
- Manually inspect entity structure if needed
- Use appropriate template override if necessary
```

**❌ DataService Interface Mapping**
```
Solutions:
- Verify naming pattern compliance
- Check if DataService interface actually exists
- Review namespace mapping accuracy against existing examples
- Adjust mapping patterns if entity structure differs
```

**❌ File Generation Failures**
```
Solutions:
- Ensure proper write permissions to repository directory
- Verify template file existence and format
- Check variable replacement completeness
- Validate output directory structure
```

---

**Supported Languages**: English, Chinese
**Version Requirements**: .NET 6.0+, TypeScript/Node.js
**Required Tools**: entity-info-getter skill, TypeInfoRetriever, create-repository.ts
**Dependencies**: Existing repository directory structure, entity templates