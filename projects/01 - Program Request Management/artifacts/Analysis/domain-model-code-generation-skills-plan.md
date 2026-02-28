# Domain Model-Driven Code Generation Skills Suite Planning

**Document ID**: ANA-02  
**Project**: 01 - Program Request Management  
**Created**: February 28, 2026  
**Status**: Planning Phase  

## Overview

This document outlines the planning for a comprehensive GitHub Skills suite that automates code generation from domain models. The skills are designed to be used within VS Code with Copilot integration, providing intelligent workflow orchestration and automated code generation capabilities.

## Skill Architecture

### Skill 1: `workflow-orchestrator`
**Responsibility**: Guide users to call related skills in the correct sequence to complete full workflows
**Input**: Current project state or user intent
**Output**: Recommended skill invocation sequence and detailed guidance

**Script**: `orchestrate-workflow.ts` (run with bun)
- Analyze current project state
- Detect domain model changes
- Generate recommended skill invocation sequence
- Provide detailed instructions for each step

### Skill 2: `domain-model-parser` ✅ **Finished**
**Responsibility**: Parse domain model documents and extract structured metadata
**Input**: Domain model markdown files
**Output**: JSON format parsing results

```json
{
  "entities": [...],
  "relationships": [...], 
  "enums": [...],
  "attributes": [...]
}
```

**Script**: `parse-domain-model.ts` (run with bun)
- Parse Mermaid class diagrams
- Extract entity definitions and attributes
- Analyze entity relationships
- Generate structured metadata

### Skill 3a: `entity-class-generator` ✅ **Finished**
**Responsibility**: Generate basic C# entity classes based on parsed metadata
**Input**: JSON output from domain-model-parser
**Output**: C# entity class files

**Script**: `generate-entities.ts` (run with bun)
- Generate basic entity classes with properties
- Handle attribute type mapping (string, int, DateTime, etc.)
- Add basic Data Annotations ([Key], [Required], [MaxLength])
- Generate simple navigation properties

### Skill 3b: `entity-configuration-generator`
**Responsibility**: Generate EF Core Fluent API configurations for entities
**Input**: JSON entity metadata + relationship metadata
**Output**: Entity configuration classes

**Script**: `generate-entity-configurations.ts` (run with bun)
- Generate EntityTypeConfiguration classes
- Configure complex relationships (one-to-many, many-to-many)
- Add indexes and constraints configuration
- Handle advanced EF Core features (owned types, value converters)

### Skill 4: `database-migration-generator`
**Responsibility**: Generate EF Core database migration scripts
**Input**: Entity metadata JSON
**Output**: EF Core Migration files

**Script**: `generate-migration.ts` (run with bun)
- Generate Migration classes
- Create table structure scripts
- Add indexes and constraints
- Handle foreign key relationships

### Skill 5a: `repository-interface-generator`
**Responsibility**: Generate Repository interface contracts
**Input**: Entity metadata JSON
**Output**: IRepository interface files

**Script**: `generate-repository-interfaces.ts` (run with bun)
- Generate IRepository<T> base interface
- Generate entity-specific repository interfaces
- Define CRUD method signatures
- Add performance annotation markers for Dapper optimization

### Skill 5b: `efcore-repository-generator`
**Responsibility**: Generate EF Core Repository implementations
**Input**: Entity metadata JSON + Repository interfaces
**Output**: EF Core Repository implementation classes

**Script**: `generate-efcore-repositories.ts` (run with bun)
- Generate EF Core-based repository implementations
- Implement LINQ-based query methods
- Add transaction management
- Include change tracking and lazy loading features

### Skill 5c: `dapper-repository-generator`
**Responsibility**: Generate Dapper Repository implementations for performance-critical operations
**Input**: Entity metadata JSON + Performance markers
**Output**: Dapper Repository implementation classes

**Script**: `generate-dapper-repositories.ts` (run with bun)
- Generate Dapper-based repository implementations
- Create optimized SQL queries
- Add parameter mapping
- Handle complex JOIN operations

### Skill 6a: `service-interface-generator`
**Responsibility**: Generate business service interface contracts
**Input**: Entity metadata JSON + Business rules
**Output**: Service interface files

**Script**: `generate-service-interfaces.ts` (run with bun)
- Generate IService interface definitions
- Define business operation method signatures
- Add validation and business rule placeholders
- Include async/await patterns

### Skill 6b: `service-implementation-generator`
**Responsibility**: Generate business service implementation classes
**Input**: Service interfaces + Repository interfaces
**Output**: Service implementation classes

**Script**: `generate-service-implementations.ts` (run with bun)
- Generate service implementation classes
- Add dependency injection for repositories
- Implement basic CRUD business operations
- Add transaction management and error handling

### Skill 7a: `blazor-list-component-generator`
**Responsibility**: Generate Blazor list/grid components for entities
**Input**: Entity metadata JSON
**Output**: Blazor list component files

**Script**: `generate-blazor-lists.ts` (run with bun)
- Generate data grid components
- Add sorting and filtering features
- Include pagination support
- Add action buttons (Edit, Delete, View)

### Skill 7b: `blazor-form-component-generator`
**Responsibility**: Generate Blazor form components for entity editing
**Input**: Entity metadata JSON + Validation rules
**Output**: Blazor form component files

**Script**: `generate-blazor-forms.ts` (run with bun)
- Generate create/edit form components
- Add form validation
- Include input components for different data types
- Add save/cancel functionality

### Skill 7c: `blazor-detail-component-generator`
**Responsibility**: Generate Blazor detail view components
**Input**: Entity metadata JSON
**Output**: Blazor detail component files

**Script**: `generate-blazor-details.ts` (run with bun)
- Generate read-only detail view components
- Format display for different data types
- Add navigation to related entities
- Include action buttons

### Skill 8: `data-context-generator` 
**Responsibility**: Generate EF Core DbContext class
**Input**: Entity metadata JSON + Connection configuration
**Output**: DbContext class file

**Script**: `generate-context.ts` (run with bun)
- Generate ApplicationDbContext class
- Configure entity relationships using Fluent API
- Add DbSet properties for all entities
- Configure database connection and options
- Add audit fields and soft delete configurations

### Skill 9: `model-change-detector`
**Responsibility**: Detect changes in domain model files
**Input**: Current and historical versions of domain models
**Output**: Change difference report

**Script**: `detect-changes.ts` (run with bun)
- Compare model file changes
- Identify added/deleted/modified entities
- Analyze attribute and relationship changes
- Generate change summary

### Skill 10: `incremental-update-generator`
**Responsibility**: Generate incremental update scripts based on changes
**Input**: Change difference report
**Output**: Incremental update code and scripts

**Script**: `generate-updates.ts` (run with bun)
- Generate database migration scripts
- Update existing entity classes
- Add new Repository methods
- Update service layer code

## Directory Structure

```
/.github/skills/sanjel-drb-blazor/
├── workflow/
│   └── workflow-orchestrator/
│       ├── SKILL.md
│       └── scripts/
│           └── orchestrate-workflow.ts
├── domain-modeling/
│   ├── domain-model-parser/
│   │   ├── SKILL.md
│   │   └── scripts/
│   │       └── parse-domain-model.ts
│   └── model-change-detector/
│       ├── SKILL.md
│       └── scripts/
│           └── detect-changes.ts
├── code-generation/
│   ├── entity-class-generator/
│   │   ├── SKILL.md
│   │   └── scripts/
│   │       └── generate-entities.ts
│   ├── entity-configuration-generator/
│   │   ├── SKILL.md
│   │   └── scripts/
│   │       └── generate-entity-configurations.ts
│   ├── repository-interface-generator/
│   │   ├── SKILL.md
│   │   └── scripts/
│   │       └── generate-repository-interfaces.ts
│   ├── efcore-repository-generator/
│   │   ├── SKILL.md
│   │   └── scripts/
│   │       └── generate-efcore-repositories.ts
│   ├── dapper-repository-generator/
│   │   ├── SKILL.md
│   │   └── scripts/
│   │       └── generate-dapper-repositories.ts
│   ├── service-interface-generator/
│   │   ├── SKILL.md
│   │   └── scripts/
│   │       └── generate-service-interfaces.ts
│   └── service-implementation-generator/
│       ├── SKILL.md
│       └── scripts/
│           └── generate-service-implementations.ts
├── database/
│   ├── database-migration-generator/
│   │   ├── SKILL.md
│   │   └── scripts/
│   │       └── generate-migration.ts
│   └── data-context-generator/
│       ├── SKILL.md
│       └── scripts/
│           └── generate-context.ts
├── ui-generation/
│   ├── blazor-list-component-generator/
│   │   ├── SKILL.md
│   │   └── scripts/
│   │       └── generate-blazor-lists.ts
│   ├── blazor-form-component-generator/
│   │   ├── SKILL.md
│   │   └── scripts/
│   │       └── generate-blazor-forms.ts
│   └── blazor-detail-component-generator/
│       ├── SKILL.md
│       └── scripts/
│           └── generate-blazor-details.ts
└── change-management/
    └── incremental-update-generator/
        ├── SKILL.md
        └── scripts/
            └── generate-updates.ts
```

## Technical Standards

### TypeScript Script Requirements

All scripts must follow these standards:

```typescript
// @ts-ignore
import { readFileSync, writeFileSync } from 'node:fs';
// @ts-ignore  
import { join, dirname } from 'node:path';
// @ts-ignore
import { execSync } from 'node:child_process';

// @ts-ignore
const process = globalThis.process;

// Script logic...
```

### Skill Documentation Template

Each SKILL.md file must include:

```markdown
# [Skill Name]

## Description
[Brief description of what the skill does]

## When To Use
- [Specific scenarios when this skill should be used]
- [Trigger conditions]
- [Context requirements]

## Usage
This skill uses bun to run TypeScript scripts for [specific purpose].

## Input
- [Required input parameters]
- [Optional parameters]

## Output
- [Expected output files/data]
- [Generated artifacts]

## Script Execution
```bash
bun run scripts/[script-name].ts
```
```

## Workflow Scenarios

### Scenario 1: New Project
1. `workflow-orchestrator` → Analyze project state, recommend complete generation process
2. `domain-model-parser` → Parse domain models
3. `entity-class-generator` → Generate basic entity classes
4. `entity-configuration-generator` → Generate EF Core configurations
5. `data-context-generator` → Generate DbContext class
6. `database-migration-generator` → Generate database scripts
7. `repository-interface-generator` → Generate Repository interfaces
8. `efcore-repository-generator` → Generate EF Core Repository implementations
9. `dapper-repository-generator` → Generate optimized Dapper implementations
10. `service-interface-generator` → Generate service interfaces
11. `service-implementation-generator` → Generate service implementations
12. `blazor-list-component-generator` → Generate list components
13. `blazor-form-component-generator` → Generate form components
14. `blazor-detail-component-generator` → Generate detail components

### Scenario 2: Domain Model Modification
1. `workflow-orchestrator` → Detect model changes, recommend incremental update process
2. `model-change-detector` → Analyze specific changes
3. `incremental-update-generator` → Generate update scripts based on changes
4. Selectively call affected generators:
   - Entity changes: `entity-class-generator` + `entity-configuration-generator`
   - New entities: Full generation sequence for new entities only
   - Relationship changes: `entity-configuration-generator` + `data-context-generator`
   - Repository changes: `repository-interface-generator` + implementation generators
   - UI changes: Relevant Blazor component generators

### Scenario 3: Update Specific Layer Only
1. `workflow-orchestrator` → Based on user intent, recommend specific skill combinations
2. Call relevant specific generators:
   **Entity Layer**: `entity-class-generator` + `entity-configuration-generator`
   **Data Layer**: `repository-interface-generator` + `efcore-repository-generator` + `dapper-repository-generator`
   **Business Layer**: `service-interface-generator` + `service-implementation-generator`
   **UI Layer**: `blazor-list-component-generator` + `blazor-form-component-generator` + `blazor-detail-component-generator`
   **Database**: `database-migration-generator` + `data-context-generator`

## Integration Points

### Target Code Locations
- `src/Sanjel.RequestManagement.Core/` - Entity classes and services
- `src/Sanjel.RequestManagement.Repositories/` - Repository classes
- `src/Sanjel.RequestManagement.Blazor/Components/` - UI components

### Configuration Integration
- Update `.csproj` file dependencies
- Modify `appsettings.json` database connections
- Integrate with existing DI container configuration

### Testing Support
- Generate unit tests for generated code
- Integrate with existing test projects

## Benefits

1. **Consistency**: Ensures all generated code follows the same patterns and standards
2. **Efficiency**: Reduces manual coding time and potential errors
3. **Maintainability**: Keeps code synchronized with domain model changes
4. **Quality**: Enforces best practices and design patterns
5. **Traceability**: Maintains clear connection between domain models and implementation
6. **Modularity**: Fine-grained skills allow selective updates and easier debugging
7. **Flexibility**: Can generate EF Core or Dapper implementations as needed
8. **Scalability**: Individual skills can be enhanced without affecting others

## Skill Architecture Benefits

After splitting larger skills into focused components, we achieve:

- **Single Responsibility**: Each skill has one clear, focused task
- **Easier Testing**: Individual skills can be tested in isolation  
- **Incremental Development**: Skills can be implemented and refined independently
- **Selective Updates**: Only affected skills need to run when models change
- **Parallel Development**: Multiple developers can work on different skills simultaneously
- **Reduced Complexity**: Smaller scripts are easier to understand and maintain

## Next Steps

1. Create the skill directory structure with all 15+ individual skills
2. Implement the workflow-orchestrator skill first to coordinate other skills
3. Develop core skills in this order:
   - `domain-model-parser` (foundation for all others)
   - `entity-class-generator` + `entity-configuration-generator` (entity layer)
   - `data-context-generator` (database context)
   - `repository-interface-generator` (contracts)
   - `efcore-repository-generator` (primary implementation)
4. Add service layer skills: `service-interface-generator` + `service-implementation-generator`
5. Implement UI generation skills: Blazor list, form, and detail generators
6. Add optimization skills: `dapper-repository-generator` for performance
7. Implement change management: `model-change-detector` + `incremental-update-generator`
8. Test with existing domain model and refine based on real-world usage

## Implementation Priority

**Phase 1 (Core)**: Skills 1, 2, 3a, 3b, 4, 8 - Basic entity and database generation
**Phase 2 (Data Access)**: Skills 5a, 5b, 5c - Repository pattern implementation  
**Phase 3 (Business Logic)**: Skills 6a, 6b - Service layer generation
**Phase 4 (UI)**: Skills 7a, 7b, 7c - Blazor component generation
**Phase 5 (Change Management)**: Skills 9, 10 - Model change detection and updates

## Notes

- All skills designed for VS Code Copilot integration
- Uses bun runtime for TypeScript execution
- No third-party dependencies allowed (Node.js modules only)
- Follows single responsibility principle (one skill = one focused task)