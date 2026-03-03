# Domain Model-Driven Code Generation Skills Suite Planning

**Document ID**: ANA-02
**Project**: 01 - Program Request Management
**Created**: February 28, 2026
**Status**: Planning Phase

## Overview

This document outlines the planning for a comprehensive GitHub Skills suite that automates code generation from domain models. The skills are designed to be used within VS Code with Copilot integration, providing intelligent workflow orchestration and automated code generation capabilities.

## Skill Architecture

### Skill 0: `project-creator` ✅ **Required - Currently Missing**
**Responsibility**: Create and configure project architecture, folder structure, and basic setup files
**Input**: Project name, target framework, and architectural preferences
**Output**: Complete project structure with configured files

**Script**: `create-project.ts` (run with bun)
- Generate solution (.slnx) and project (.csproj) files
- Create standard folder structure (Core, Repositories, BusinessProcess, Blazor, Tests)
- Configure development environment settings (.editorconfig, .vscode/settings.json)
- Set up StyleCop analyzers and code formatting rules
- Configure XML documentation generation for all projects
- Add dependency injection and EF Core package references

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

### Skill 3: `enum-generator` ✅ **Finished + Auto-formatting**
**Responsibility**: Generate C# enum definitions from parsed metadata
**Input**: JSON output from domain-model-parser (enums array)
**Output**: C# enum class files with automatic code formatting

**Script**: `generate-enums.ts` (run with bun)
- Parse enum definitions from domain model metadata
- Generate C# enum classes with proper naming conventions
- Handle PascalCase conversion for compound words (notificationtype → NotificationType)
- Create enum files in appropriate project directory
- Support custom enum values and descriptions
- **Auto-format generated code with dotnet format**

### Skill 4a: `entity-class-generator` ✅ **Finished + Auto-formatting**
**Responsibility**: Generate basic C# entity classes based on parsed metadata
**Input**: JSON output from domain-model-parser
**Output**: C# entity class files with automatic code formatting

**Script**: `generate-entities.ts` (run with bun)
- Generate basic entity classes with properties
- Handle attribute type mapping (string, int, DateTime, etc.)
- Add basic Data Annotations ([Key], [Required], [MaxLength])
- Generate simple navigation properties
- **Auto-format generated code with dotnet format**

### Skill 4b: `entity-configuration-generator` ✅ **Finished + Auto-formatting**
**Responsibility**: Generate EF Core Fluent API configurations for entities
**Input**: JSON entity metadata + relationship metadata
**Output**: Entity configuration classes with automatic code formatting

**Script**: `generate-entity-configurations.ts` (run with bun)
- Generate EntityTypeConfiguration classes
- Configure complex relationships (one-to-many, many-to-many)
- Add indexes and constraints configuration
- Handle advanced EF Core features (owned types, value converters)
- **Auto-format generated code with dotnet format**

### Skill 5: `database-migration-generator` ✅ **Finished + Auto-formatting**
**Responsibility**: Generate EF Core database migration scripts from domain model metadata
**Input**: Entity metadata JSON + Optional migration name and output directory
**Output**: EF Core Migration class files with automatic code formatting

**Script**: `generate-migration.ts` (run with bun)
- Generate Migration classes with proper timestamp naming (M{timestamp}_{name})
- Create complete table structure scripts with appropriate SQL types
- Add indexes and constraints for email fields and unique attributes
- Handle foreign key relationships with proper referential actions
- Generate both Up() and Down() methods for reversible migrations
- **Auto-format generated code with dotnet format**

### Skill 6a: `repository-interface-generator` ✅ **Finished + Modern EF Core + Format Verified**
**Responsibility**: Generate Repository interface contracts using modern EF Core patterns
**Input**: Entity metadata JSON
**Output**: EF Core-based IRepository interface files

**Script**: `generate-repository-interfaces.ts` (run with bun)
- Generate modern IRepository<T> base interface with EF Core patterns
- Generate entity-specific repository interfaces with CancellationToken support
- Define modern CRUD method signatures (Query, GetByIdAsync, GetAllAsync, etc.)
- Add performance markers for EF Core query optimization
- Include PagedResult<T> class for pagination support
- **Completely modernized EF Core approach - no legacy dependencies**
- **Auto-format generated code with dotnet format**
- **✅ Confirmed: Calls shared `formatGeneratedCode()` method from project-utilities**

### Skill 6b: `efcore-repository-generator` ✅ **Finished**
**Responsibility**: Generate EF Core Repository implementations
**Input**: Entity metadata JSON + Repository interfaces
**Output**: EF Core Repository implementation classes

**Script**: `generate-efcore-repositories.ts` (run with bun)
- Generate EF Core-based repository implementations
- Implement LINQ-based query methods
- Add transaction management
- Include change tracking and lazy loading features

### Skill 7a: `service-interface-generator`
**Responsibility**: Generate business service interface contracts
**Input**: Entity metadata JSON + Business rules
**Output**: Service interface files

**Script**: `generate-service-interfaces.ts` (run with bun)
- Generate IService interface definitions
- Define business operation method signatures
- Add validation and business rule placeholders
- Include async/await patterns

### Skill 7b: `service-implementation-generator`
**Responsibility**: Generate business service implementation classes
**Input**: Service interfaces + Repository interfaces
**Output**: Service implementation classes

**Script**: `generate-service-implementations.ts` (run with bun)
- Generate service implementation classes
- Add dependency injection for repositories
- Implement basic CRUD business operations
- Add transaction management and error handling

### Skill 8a: `blazor-list-component-generator`
**Responsibility**: Generate Blazor list/grid components for entities
**Input**: Entity metadata JSON
**Output**: Blazor list component files

**Script**: `generate-blazor-lists.ts` (run with bun)
- Generate data grid components
- Add sorting and filtering features
- Include pagination support
- Add action buttons (Edit, Delete, View)

### Skill 8b: `blazor-form-component-generator`
**Responsibility**: Generate Blazor form components for entity editing
**Input**: Entity metadata JSON + Validation rules
**Output**: Blazor form component files

**Script**: `generate-blazor-forms.ts` (run with bun)
- Generate create/edit form components
- Add form validation
- Include input components for different data types
- Add save/cancel functionality

### Skill 8c: `blazor-detail-component-generator`
**Responsibility**: Generate Blazor detail view components
**Input**: Entity metadata JSON
**Output**: Blazor detail component files

**Script**: `generate-blazor-details.ts` (run with bun)
- Generate read-only detail view components
- Format display for different data types
- Add navigation to related entities
- Include action buttons

### Skill 9: `data-context-generator`
**Responsibility**: Generate EF Core DbContext class
**Input**: Entity metadata JSON + Connection configuration
**Output**: DbContext class file

**Script**: `generate-context.ts` (run with bun)
- Generate ApplicationDbContext class
- Configure entity relationships using Fluent API
- Add DbSet properties for all entities
- Configure database connection and options
- Add audit fields and soft delete configurations

### Skill 10: `model-change-detector`
**Responsibility**: Detect changes in domain model files
**Input**: Current and historical versions of domain models
**Output**: Change difference report

**Script**: `detect-changes.ts` (run with bun)
- Compare model file changes
- Identify added/deleted/modified entities
- Analyze attribute and relationship changes
- Generate change summary

### Skill 11: `incremental-update-generator`
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
├── utilities/
│   └── project-utilities/
│       ├── SKILL.md
│       └── scripts/
│           └── utilities.ts
├── architecture/
│   └── project-creator/
│       ├── SKILL.md
│       └── scripts/
│           └── create-project-structure.ts
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
│   ├── enum-generator/
│   │   ├── SKILL.md
│   │   └── scripts/
│   │       └── generate-enums.ts
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

### Shared Utilities Skill ✅ **Implemented**

**All code generation skills use centralized utilities from `project-utilities` skill:**

Located at: `/.github/skills/sanjel-drb-blazor/utilities/project-utilities/`

**Available Utility Functions:**
- `detectProjectRoot()` - Find project root by locating .slnx files
- `detectProjectInfo()` - Get project name, root path, and solution info
- `constructMigrationPath()` - Build standard migration directory paths
- `constructEntityPath()` - Build standard entity directory paths
- `constructRepositoryPath()` - Build standard repository directory paths
- `constructServicePath()` - Build standard service directory paths
- `formatGeneratedCode(outputDir)` - Format C# code using dotnet format
- `formatSpecificFiles(filePaths)` - Format specific files only
- `toPascalCase()` - Convert to PascalCase format
- `toCamelCase()` - Convert to camelCase format
- `toKebabCase()` - Convert to kebab-case format
- `pluralize()` / `singularize()` - Handle singular/plural conversions
- `ensureDirectoryExists()` - Create directories safely
- `readJsonFile()` / `writeJsonFile()` - Safe JSON file operations
- `findDomainModelMetadata()` - Locate domain model metadata file

**Import Pattern:**
```typescript
import {
    detectProjectInfo,
    formatGeneratedCode,
    toPascalCase,
    constructEntityPath
} from '../../../utilities/project-utilities/scripts/utilities';
```

### Code Formatting Requirements ✅ **Implemented**

**All code generation skills MUST include automatic code formatting using `dotnet format`:**

- Each generation script calls `dotnet format` after code generation completes
- Formatting targets only the generated output directory to optimize performance
- Formatting failures do not fail the generation process (warning only)
- Formatting provides consistent code style and reduces merge conflicts

**Implementation Standard:**
```typescript
// Format generated C# code using dotnet format
function formatGeneratedCode(outputDir: string): void {
	try {
		console.log('🎨 Formatting generated code with dotnet format...');

		// Get the project root (where .slnx file is located)
		let projectRoot = outputDir;
		while (projectRoot && projectRoot !== '/') {
			const files = execSync(`ls "${projectRoot}"`, { encoding: 'utf-8' }).split('\n');
			const hasSlnx = files.some(file => file.endsWith('.slnx'));
			if (hasSlnx) {
				break;
			}
			// Continue searching up
			const parentDir = join(projectRoot, '..');
			if (parentDir === projectRoot) break;
			projectRoot = parentDir;
		}

		// Run dotnet format on the specific directory
		const formatCommand = `dotnet format "${projectRoot}" --include "${outputDir}/**/*.cs"`;
		execSync(formatCommand, { cwd: projectRoot, encoding: 'utf-8' });

		console.log('✅ Code formatting completed successfully!');
	} catch (error) {
		console.warn('⚠️  Code formatting failed, but generation was successful:', error);
		// Don't fail the generation if formatting fails
	}
}
```

**Applied to Skills:**
- ✅ `enum-generator`: Auto-formats generated enum classes
- ✅ `entity-class-generator`: Auto-formats generated entity classes
- ✅ `entity-configuration-generator`: Auto-formats generated configuration classes
- ✅ `database-migration-generator`: Auto-formats generated migration classes

**Code Architecture:**
- Skills communicate through Copilot orchestration, not direct imports
- Main generation functions do not export (removed unnecessary exports)
- Only `project-utilities` exports functions for shared use across skills

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
0. `project-creator` → Create project structure, configure .csproj files, set up development environment
1. `workflow-orchestrator` → Analyze project state, recommend complete generation process
2. `domain-model-parser` → Parse domain models
3. `enum-generator` → Generate enum definitions
4. `entity-class-generator` → Generate basic entity classes
5. `entity-configuration-generator` → Generate EF Core configurations
6. `data-context-generator` → Generate DbContext class
7. `database-migration-generator` → Generate database scripts
8. `repository-interface-generator` → Generate Repository interfaces
9. `efcore-repository-generator` → Generate EF Core Repository implementations
10. `service-interface-generator` → Generate service interfaces
12. `service-implementation-generator` → Generate service implementations
13. `blazor-list-component-generator` → Generate list components
14. `blazor-form-component-generator` → Generate form components
15. `blazor-detail-component-generator` → Generate detail components

### Scenario 2: Domain Model Modification
1. `workflow-orchestrator` → Detect model changes, recommend incremental update process
2. `model-change-detector` → Analyze specific changes
3. `incremental-update-generator` → Generate update scripts based on changes
4. Selectively call affected generators:
   - Entity changes: `enum-generator` + `entity-class-generator` + `entity-configuration-generator`
   - New entities: Full generation sequence for new entities only
   - Relationship changes: `entity-configuration-generator` + `data-context-generator`
   - Repository changes: `repository-interface-generator` + `efcore-repository-generator`
   - UI changes: Relevant Blazor component generators

### Scenario 3: Update Specific Layer Only
1. `workflow-orchestrator` → Based on user intent, recommend specific skill combinations
2. Call relevant specific generators:
   **Entity Layer**: `enum-generator` + `entity-class-generator` + `entity-configuration-generator`
   **Data Layer**: `repository-interface-generator` + `efcore-repository-generator`
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
5. **Flexibility**: Uses modern EF Core implementation with performance optimization
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

1. Create the skill directory structure with all 16+ individual skills
2. **Priority: Implement missing `project-creator` skill first** - Essential for project scaffolding
3. Implement the workflow-orchestrator skill to coordinate other skills
4. Develop core skills in this order:
   - `domain-model-parser` (foundation for all others)
   - `entity-class-generator` + `entity-configuration-generator` (entity layer)
   - `data-context-generator` (database context)
   - `repository-interface-generator` (contracts)
   - `efcore-repository-generator` (primary implementation)
5. Add service layer skills: `service-interface-generator` + `service-implementation-generator`
6. Implement UI generation skills: Blazor list, form, and detail generators
7. Implement change management: `model-change-detector` + `incremental-update-generator`
9. Test with existing domain model and refine based on real-world usage

## Implementation Priority

**Phase 0 (Foundation)**:
- `project-utilities` ✅ **Implemented** - Shared utility functions for all skills
- `project-creator` ❌ **Missing** - Project structure and configuration setup

**Phase 1 (Core)**: Skills 1, 2, 3, 4a, 4b, 5, 9 - Basic enum, entity and database generation ✅ **Complete**
**Phase 2 (Data Access)**: Skills 6a, 6b - Repository pattern implementation ✅ **Started (6a Complete)**
**Phase 3 (Business Logic)**: Skills 7a, 7b - Service layer generation
**Phase 4 (UI)**: Skills 8a, 8b, 8c - Blazor component generation
**Phase 5 (Change Management)**: Skills 10, 11 - Model change detection and updates

## Notes

- All skills designed for VS Code Copilot integration
- Uses bun runtime for TypeScript execution
- No third-party dependencies allowed (Node.js modules only)
- Follows single responsibility principle (one skill = one focused task)

## Project Utilities Integration ✅ **Completed**

Successfully extracted common functionality into a centralized `project-utilities` skill:

**Benefits Achieved:**
- ✅ Eliminated code duplication across 4+ skills
- ✅ Centralized project structure detection logic
- ✅ Standardized code formatting integration
- ✅ Improved maintainability and consistency
- ✅ Added auto-detection for domain model metadata files
- ✅ Fixed formatting issues with proper .slnx file references
- ✅ Removed unnecessary exports - skills communicate via Copilot, not direct imports

**Refactored Skills:**
- ✅ `database-migration-generator` - fully refactored, tested, and cleaned of exports
- ✅ `enum-generator` - fully refactored, tested, and cleaned of exports
- ✅ `entity-class-generator` - fully refactored and cleaned of exports
- ✅ `entity-configuration-generator` - fully refactored and cleaned of exports

**Phase 2 Skills:**
- ✅ `repository-interface-generator` - Generated modern EF Core repository interfaces with 16 methods, CancellationToken support, and PagedResult<T> pagination

**Modern EF Core Architecture:** ✅ **Implemented**
Successfully migrated from legacy data access patterns to modern EF Core:

**Architecture Changes:**
- ✅ Created `RequestManagementDbContext` with DbSet<T> properties
- ✅ Modern `IRepository<TEntity>` interface with 16 standardized methods
- ✅ `BaseRepository<TEntity>` implementation using EF Core DbContext
- ✅ `PagedResult<T>` class for advanced pagination with metadata
- ✅ CancellationToken support throughout all async operations
- ✅ IQueryable<T> exposure for advanced LINQ queries
- ✅ Performance markers changed from "Dapper" to "EF Core Query" annotations

**Generated Interface Features:**
- Query() method for direct IQueryable<T> access
- Modern async/await patterns with CancellationToken
- Flexible pagination with PagedResult<T>
- Expression<Func<T, bool>> predicate support
- Batch operations (AddRangeAsync, UpdateRange, RemoveRange)
- Automatic SaveChanges integration

**Next Implementation:** Continue with `efcore-repository-generator` and complete Phase 2 - Data Access
