# Domain Model-Driven Code Generation Skills Suite Planning

**Document ID**: ANA-02
**Project**: 01 - Program Request Management
**Created**: February 28, 2026
**Last Updated**: March 5, 2026
**Status**: Planning Phase

## 🆕 **Critical Architecture Update (March 5, 2026)**

**Major Shift: AI-Driven Skills Preferred Over Script-Driven Approach**

- **Primary Approach**: AI-driven consultation and guidance (🤖 AI-Driven)
- **Secondary Approach**: Script-driven automation (🔧 Script-Driven) for well-defined tasks only
- **Rationale**: AI-driven skills provide superior flexibility, educational value, and adaptation capabilities
- **Impact**: Enhanced user experience through consultative approach rather than rigid code generation

## Overview

This document outlines the planning for a comprehensive GitHub Skills suite that combines AI-driven consultation with selective automation for domain model-driven development. The skills are designed to be used within VS Code with Copilot integration, providing intelligent workflow orchestration and adaptive architectural guidance.

## Skill Architecture

### Skill 0: `project-creator` ✅ **Required - Currently Missing** **🔧 Script-Driven**
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

### Skill 0.5: `solution-code-formatter` **🔧 Script-Driven**
**Responsibility**: Format all code in the entire solution using dotnet format commands
**Input**: Solution file path (.slnx)
**Output**: Formatted code across entire solution

**Script**: `format-solution.ts` (run with bun)
- Execute `dotnet format` on the entire solution
- Execute `dotnet format style` for comprehensive style formatting
- Provide formatting status and results
- Handle formatting errors gracefully
- Support selective formatting by project if needed

### Skill 1: `workflow-orchestrator` **🤖 AI-Driven**
**Responsibility**: Intelligent workflow guidance and skill orchestration consultation
**Input**: Current project state + User intent + Project constraints
**Output**: Smart workflow recommendations and skill sequence guidance

**Approach**: **AI-Driven Workflow Intelligence**
- Analyzes current project state and requirements
- Provides intelligent skill sequence recommendations
- Adapts workflow based on project complexity and constraints
- Offers contextual guidance for each workflow step
- **Output**: Intelligent workflow orchestration + adaptive skill recommendations

### Skill 2: `domain-model-parser` ✅ **Finished** **🔧 Script-Driven**
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

### Skill 3: `enum-generator` ✅ **Finished** **🔧 Script-Driven**
**Responsibility**: Generate C# enum definitions from parsed metadata
**Input**: JSON output from domain-model-parser (enums array)
**Output**: C# enum class files

**Script**: `generate-enums.ts` (run with bun)
- Parse enum definitions from domain model metadata
- Generate C# enum classes with proper naming conventions
- Handle PascalCase conversion for compound words (notificationtype → NotificationType)
- Create enum files in appropriate project directory
- Support custom enum values and descriptions
- **Formatting handled by `solution-code-formatter` skill**

### Skill 4a: `entity-class-generator` ✅ **Finished** **🔧 Script-Driven**
**Responsibility**: Generate basic C# entity classes based on parsed metadata
**Input**: JSON output from domain-model-parser
**Output**: C# entity class files

**Script**: `generate-entities.ts` (run with bun)
- Generate basic entity classes with properties
- Handle attribute type mapping (string, int, DateTime, etc.)
- Add basic Data Annotations ([Key], [Required], [MaxLength])
- Generate simple navigation properties
- **Formatting handled by `solution-code-formatter` skill**

### Skill 4b: `entity-configuration-generator` ✅ **Finished** **🔧 Script-Driven**
**Responsibility**: Generate EF Core Fluent API configurations for entities
**Input**: JSON entity metadata + relationship metadata
**Output**: Entity configuration classes

**Script**: `generate-entity-configurations.ts` (run with bun)
- Generate EntityTypeConfiguration classes
- Configure complex relationships (one-to-many, many-to-many)
- Add indexes and constraints configuration
- Handle advanced EF Core features (owned types, value converters)
- **Formatting handled by `solution-code-formatter` skill**

### Skill 5: `database-migration-generator` ✅ **Finished** **🔧 Script-Driven**
**Responsibility**: Generate EF Core database migration scripts from domain model metadata
**Input**: Entity metadata JSON + Optional migration name and output directory
**Output**: EF Core Migration class files

**Script**: `generate-migration.ts` (run with bun)
- Generate Migration classes with proper timestamp naming (M{timestamp}_{name})
- Create complete table structure scripts with appropriate SQL types
- Add indexes and constraints for email fields and unique attributes
- Handle foreign key relationships with proper referential actions
- Generate both Up() and Down() methods for reversible migrations
- **Formatting handled by `solution-code-formatter` skill**

### Skill 6a: `repository-interface-generator` ✅ **Finished + Modern EF Core** **🔧 Script-Driven**
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
- **Formatting handled by `solution-code-formatter` skill**

### Skill 6b: `efcore-repository-generator` ✅ **Finished** **🔧 Script-Driven**
**Responsibility**: Generate EF Core Repository implementations
**Input**: Entity metadata JSON + Repository interfaces
**Output**: EF Core Repository implementation classes

**Script**: `generate-efcore-repositories.ts` (run with bun)
- Generate EF Core-based repository implementations
- Implement LINQ-based query methods
- Add transaction management
- Include change tracking and lazy loading features

### Skill 7a: `service-interface-generator` **🔧 Script-Driven**
**Responsibility**: Generate business service interface contracts
**Input**: Entity metadata JSON + Business rules
**Output**: Service interface files

**Script**: `generate-service-interfaces.ts` (run with bun)
- Generate IService<T> base interface with business operations
- Generate entity-specific service interfaces (IRequestService, IDataElementService, etc.)
- Add validation and business rule placeholders with ValidationResult support
- Include async/await patterns with CancellationToken support
- **Formatting handled by `solution-code-formatter` skill**

## 🆕 Blazor Layered Architecture Design - Supporting Flexible Component Library Selection

### 🎯 Critical Design Principle: Skills Layering vs Code Layering

**Important Clarification (Added March 4, 2026):**

**Skills Layering ≠ Code Layering**
- **Skills Layering**: Organization and execution sequence of AI skills (this document's layer design)
- **Code Layering**: Architecture of the generated Blazor application code
- **Key Understanding**: Do NOT push Skills layer abstractions into generated code

**Correct Approach:**
- Skills should generate **concrete, working code** with specific component library implementations
- Component library switching achieved through **Skills re-generation**, not code-level abstractions
- Each skill produces **simple, direct implementations** without unnecessary abstraction layers
- **Skills orchestration** handles complexity, **generated code** remains straightforward

**Example:**
- ❌ Wrong: Generate IComponentAdapter interfaces and adapter pattern code
- ✅ Correct: Generate MainLayout.razor directly using MudBlazor components
- ✅ Switching: Run `blazor-syncfusion-adapter` skill to replace MudBlazor with Syncfusion

### Layer 1: Architecture Foundation Layer

#### Skill 8a: `blazor-architecture-generator` ✅ **Finished** [Priority 1] **🤖 AI-Driven**
**Responsibility**: Simple component library selector and friendly architecture guidance
**Input**: Project preferences + Basic requirements
**Output**: Component library selection guidance + Automatic generator launch

**Approach**: **AI-Driven Simple Selection**
- Acts as friendly Blazor architecture guide
- Presents 4 clear component library options (MudBlazor, Syncfusion, Bootstrap, Minimal)
- Provides conversational guidance and answers questions
- Launches selected specialized generator automatically
- **Output**: User-friendly selection process + automated skill orchestration

**Component Library Switching Strategy:**
- Switching = Re-run different component library generator (e.g., `syncfusion-generator`)
- Component library generators replace/update concrete implementations in existing files
- No code-level abstraction layers - switching handled by Skills orchestration

### Layer 1.5: Component Library Generator Layer 🆕 **Updated Architecture**

#### Skill 8a1: `mudblazor-generator` ✅ **Finished** [Priority 1.1] **🔧 Script-Driven**
**Responsibility**: Generate complete MudBlazor Material Design Blazor architecture
**Input**: Project configuration + Material Design preferences
**Output**: Complete Blazor foundation with MudBlazor components

**Script**: `generate-mudblazor-architecture.ts` (run with bun)
- Install and configure MudBlazor NuGet packages
- Generate MainLayout.razor and NavMenu.razor with Material Design components
- Configure MudBlazor services in Program.cs
- Set up Material Design themes and icons
- Create responsive layout with MudBlazor responsive system
- **Output**: Complete, runnable Blazor application with MudBlazor

#### Skill 8a2: `syncfusion-generator` ✅ **Finished** [Priority 1.2] **🤖 AI-Driven**
**Responsibility**: Enterprise architecture consultant for Syncfusion Blazor integration
**Input**: Enterprise requirements + Syncfusion license + Performance needs
**Output**: Strategic Syncfusion architecture guidance and implementation recommendations

**Approach**: **AI-Driven Enterprise Consultation**
- Acts as Senior Enterprise Architect with Syncfusion expertise
- Provides license optimization and ROI maximization guidance
- Recommends optimal component selection for business requirements
- Guides performance optimization for large-scale enterprise data
- **Output**: Expert enterprise architecture guidance + implementation strategies

#### Skill 8a3: `bootstrap-generator` ✅ **Finished** [Priority 1.3] **🤖 AI-Driven**
**Responsibility**: CSS Framework Architecture consultant for Bootstrap 5 Blazor integration
**Input**: Responsive design requirements + Bootstrap customization needs + Performance constraints
**Output**: Strategic Bootstrap architecture guidance and responsive design recommendations

**Approach**: **AI-Driven CSS Framework Consultation**
- Acts as Senior CSS Framework Architect with Bootstrap expertise
- Provides utility-first design strategies and responsive implementation guidance
- Recommends optimal Bootstrap component usage and customization approaches
- Guides performance optimization and CSS architecture best practices
- **Output**: Expert CSS framework guidance + responsive design strategies

#### Skill 8a4: `minimal-generator` ✅ **Finished** [Priority 1.4] **🤖 AI-Driven**
**Responsibility**: Senior Frontend Architect for zero-dependency Blazor applications
**Input**: Performance requirements + Brand specifications + Accessibility needs
**Output**: Consultative guidance for lean, high-performance Blazor architecture

**Approach**: **AI-Driven Performance Consultation**
- Acts as Senior Frontend Architect specializing in minimal dependencies
- Provides consultative guidance for custom design systems
- Recommends progressive enhancement and accessibility strategies
- Guides performance optimization and bundle size reduction
- **Output**: Architectural mentorship + performance-focused guidance

#### Skill 8b: `blazor-data-integration-generator` [Priority 2] **🔧 Script-Driven**
**Responsibility**: Generate data integration and state management layer
**Input**: Entity metadata JSON + Service interfaces
**Output**: Data access layer and state management components

**Script**: `generate-data-integration.ts` (run with bun)
- Generate page-level data services (PageDataService<TEntity>)
- Create state management components (State containers)
- Add data loading and error handling mechanisms
- Generate caching and performance optimization code

### Layer 2: Page Pattern Layer

#### Skill 8c: `blazor-page-pattern-generator` 🔄 **Enhanced - Component Library Agnostic** [Priority 3]
**Responsibility**: Generate concrete page files using current component library (not abstractions)
**Input**: Entity metadata JSON + Current project component library detection
**Output**: Concrete .razor page files using detected component library

**Script**: `generate-page-patterns.ts` (run with bun)
- Detect current component library in project (MudBlazor, Syncfusion, etc.)
- Generate concrete List.razor, Create.razor, Edit.razor, Detail.razor pages
- Use specific component library components directly (no abstraction layer)
- Configure routing with @page directives
- Implement navigation and parameter passing using concrete components
- **Output**: Working page files with direct component implementations

#### Skill 8d: `blazor-list-pattern-generator` [Priority 4]
**Responsibility**: Generate business logic patterns for list pages
**Input**: Entity metadata JSON + Sorting, filtering, pagination requirements
**Output**: List page business logic and interaction patterns

**Script**: `generate-list-patterns.ts` (run with bun)
- Generate list data management logic (search, sort, filter)
- Add pagination and virtual scrolling support
- Create batch operation functionality (batch delete, export, etc.)
- Generate list item interaction event handling

#### Skill 8e: `blazor-form-pattern-generator` [Priority 5]
**Responsibility**: Generate form page business logic and validation patterns
**Input**: Entity metadata JSON + Validation rules + Form layout requirements
**Output**: Form business logic and validation mechanisms

**Script**: `generate-form-patterns.ts` (run with bun)
- Generate form data binding and validation logic
- Add dynamic form field management
- Create form state tracking (dirty data detection, auto-save)
- Generate complex form interactions (cascading selection, conditional display)

#### Skill 8f: `blazor-detail-pattern-generator` [Priority 6]
**Responsibility**: Generate detail page display logic and related data processing
**Input**: Entity metadata JSON + Related entity information
**Output**: Detail page business logic and related data management

**Script**: `generate-detail-patterns.ts` (run with bun)
- Generate detail data display logic
- Add lazy loading for related data
- Create detail page action button logic
- Generate audit log and change history display

### Layer 3: Component Library Adapter Layer

#### Skill 8g: `blazor-component-adapter-base` [Priority 7] **⚠️ DEPRECATED**
**Status**: This skill concept has been revised - no longer needed
**Reason**: Component library switching handled by direct replacement skills, not abstraction layers

#### Skill 8h: `blazor-syncfusion-adapter` 🎯 **Current Priority** [Priority 8]
**Responsibility**: Replace existing component library implementation with Syncfusion Blazor components
**Input**: Existing Blazor project + Syncfusion configuration preferences
**Output**: Updated project files with Syncfusion component implementations

**Script**: `generate-syncfusion-adapter.ts` (run with bun)
- Replace NuGet package references (remove old library, add Syncfusion packages)
- Update Program.cs service registrations for Syncfusion
- Replace _Imports.razor namespaces with Syncfusion imports  
- Update MainLayout.razor and NavMenu.razor to use Syncfusion components (SfSidebar, SfAppBar, etc.)
- Replace CSS/theme files with Syncfusion theme configuration
- Update appsettings.json with Syncfusion license and configuration
- **Result**: Direct Syncfusion implementation (no abstraction layers)

#### Skill 8i: `blazor-material-adapter` 📋 **Future Extension** [Optional]
**Responsibility**: Replace existing component library implementation with Material Design Blazor components  
**Input**: Existing Blazor project + Material Design preferences
**Output**: Updated project files with Material Design component implementations

**Script**: `generate-material-adapter.ts` (run with bun)
- Replace component library with MudBlazor or other Material Design library
- Update all layout and configuration files for Material Design components
- **Result**: Direct Material Design implementation (no abstraction layers)

### Layer 4: Styling and Theme Layer

#### Skill 8j: `blazor-theme-generator` [Priority 9] **🤖 AI-Driven**
**Responsibility**: Design system consultation and brand customization guidance
**Input**: Brand requirements + Design system needs + Accessibility standards + Performance constraints
**Output**: Consultative guidance for theme architecture and implementation strategies

**Approach**: **AI-Driven Design System Consultant**
- Acts as brand design system architect and consultant
- Provides adaptive theme strategies based on brand identity
- Guides responsive design implementation approaches
- Recommends accessibility-first theme architectures
- Offers performance-optimized styling strategies
- **Output**: Flexible design system guidance + adaptive theme recommendations

### Skill 7b: `page-driven-service-generator` 🔄 **Redesigned - Based on Page Operations** **🤖 AI-Driven**
**Responsibility**: Generate service implementation classes based on actual page operations and business requirements
**Input**: Generated Blazor pages + Service interfaces + Repository interfaces
**Output**: Service implementation classes with page-specific business methods

**Script**: `generate-page-driven-services.ts` (run with bun)
- Analyze generated pages for actual business operations (Create, Edit, Delete, Search, etc.)
- Generate service implementations with only required methods
- Add dependency injection for repositories
- Implement business validation based on page form requirements
- Add transaction management for multi-step page operations
- Generate custom business methods based on page-specific workflows

### Skill 9: `data-context-generator` **🔧 Script-Driven**
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

### Skill 12: `test-code-generator`
**Responsibility**: Generate unit tests for generated code (entities, repositories, services, pages)
**Input**: Entity metadata JSON + Generated code files
**Output**: Unit test files in corresponding test projects

**Script**: `generate-tests.ts` (run with bun)
- Generate unit tests for entity classes
- Generate repository tests (CRUD, query, transaction)
- Generate service layer tests (business logic, validation)
- Generate page/component tests for Blazor UI
- Support for test data generation and mocking

### Skill 13: `configuration-file-manager`
**Responsibility**: Manage and generate environment configuration files and application settings
**Input**: Environment specifications + Feature requirements
**Output**: Complete appsettings.json, logging, middleware, and DI configuration files

**Script**: `generate-configurations.ts` (run with bun)
- Generate appsettings.json for all environments (Development, Production, etc.)
- Generate logging configuration
- Generate middleware configuration
- Update DI container setup
- Support for custom feature flags and secrets

## Directory Structure

```
/.github/skills/sanjel-drb-blazor/
├── utilities/
│   ├── project-utilities/
│   │   ├── SKILL.md
│   │   └── scripts/
│   │       └── utilities.ts
│   └── solution-code-formatter/
│       ├── SKILL.md
│       └── scripts/
│           └── format-solution.ts
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
│   └── service-interface-generator/
│       ├── SKILL.md
│       └── scripts/
│           └── generate-service-interfaces.ts
├── database/
│   ├── database-migration-generator/
│   │   ├── SKILL.md
│   │   └── scripts/
│   │       └── generate-migration.ts
│   └── data-context-generator/
│       ├── SKILL.md
│       └── scripts/
│           └── generate-context.ts
├── presentation/
│   ├── architecture/
│   │   └── blazor-architecture-generator/
│   │       └── SKILL.md
│   ├── mudblazor-generator/
│   │   ├── SKILL.md
│   │   └── scripts/
│   │       └── generate-mudblazor-architecture.ts
│   ├── syncfusion-generator/
│   │   └── SKILL.md
│   ├── bootstrap-generator/
│   │   └── SKILL.md
│   ├── minimal-generator/
│   │   └── SKILL.md
│   ├── patterns/
│   │   ├── blazor-page-pattern-generator/
│   │   │   ├── SKILL.md
│   │   │   └── scripts/
│   │   │       └── generate-page-patterns.ts
│   │   ├── blazor-list-pattern-generator/
│   │   │   ├── SKILL.md
│   │   │   └── scripts/
│   │   │       └── generate-list-patterns.ts
│   │   ├── blazor-form-pattern-generator/
│   │   │   ├── SKILL.md
│   │   │   └── scripts/
│   │   │       └── generate-form-patterns.ts
│   │   └── blazor-detail-pattern-generator/
│   │       ├── SKILL.md
│   │       └── scripts/
│   │           └── generate-detail-patterns.ts
│   └── styling/
│       └── blazor-theme-generator/
│           └── SKILL.md
├── business-logic/
│   └── page-driven-service-generator/
│       ├── SKILL.md
│       └── scripts/
│           └── generate-page-driven-services.ts
└── change-management/
    └── incremental-update-generator/
        ├── SKILL.md
        └── scripts/
            └── generate-updates.ts
```

## Technical Standards

### Skill Implementation Approaches 🆕 **Critical Architecture Decision**

**Skills can be implemented using two distinct approaches, chosen based on flexibility requirements:**

#### AI-Driven Skills (Preferred Approach)
**Characteristics:**
- No TypeScript scripts or automation
- Uses AI persona and consultative guidance
- Provides flexible, context-aware recommendations
- Adapts to specific project requirements and constraints
- Educational and mentoring approach

**When To Use AI-Driven Approach:**
- **Architectural Consulting**: Where flexibility and adaptation are crucial
- **Design System Creation**: Custom solutions that reflect unique brand identity
- **Performance Optimization**: Context-specific recommendations 
- **Accessibility Guidance**: Adaptive compliance strategies
- **Learning and Mentoring**: Educational value and skill transfer
- **Complex Decision Making**: Trade-offs require human-like reasoning

**Examples:**
- ✅ `minimal-generator` - AI architect persona for zero-dependency Blazor guidance
- ✅ `blazor-theme-generator` - Consultative theme and design system guidance
- ✅ `workflow-orchestrator` - Intelligent skill sequence recommendations

#### Script-Driven Skills (Legacy/Specific Use Cases)
**Characteristics:**
- Uses bun + TypeScript automation scripts
- Generates consistent, standardized code structures
- Follows rigid templates and patterns
- Optimal for repetitive, well-defined tasks

**When To Use Script-Driven Approach:**
- **Data Structure Generation**: Entities, enums, configurations (well-defined patterns)
- **Database Operations**: Migrations, contexts (standardized EF Core patterns)
- **Repository Pattern**: CRUD operations with consistent interfaces
- **Boilerplate Code**: Repetitive code that follows strict conventions
- **Integration Tasks**: File manipulation, project structure setup

**Examples:**
- ✅ `enum-generator` - Standardized C# enum class generation
- ✅ `entity-class-generator` - Consistent entity class structure
- ✅ `database-migration-generator` - EF Core migration scripts
- ✅ `repository-interface-generator` - Standardized repository contracts

#### Decision Matrix

| Factor             | AI-Driven                | Script-Driven           |
| ------------------ | ------------------------ | ----------------------- |
| **Flexibility**    | ✅ Maximum adaptation     | ❌ Fixed templates       |
| **Consistency**    | ⚠️ Varies by context      | ✅ Always consistent     |
| **Learning Value** | ✅ Educational guidance   | ❌ Black box output      |
| **Customization**  | ✅ Infinite possibilities | ❌ Template limitations  |
| **Speed**          | ⚠️ Interactive process    | ✅ Instant generation    |
| **Maintenance**    | ✅ Self-improving AI      | ❌ Manual script updates |

#### **Current Preference: AI-Driven Approach**
**Rationale**: AI-driven skills provide superior flexibility, educational value, and adaptation capabilities. Script-driven approach reserved only for highly repetitive, well-standardized code generation tasks.

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

### Code Formatting Requirements ✅ **Updated Architecture**

**Code formatting is handled by dedicated `solution-code-formatter` skill:**

- Uses `dotnet format` and `dotnet format style` commands on entire solution
- Called after code generation skills complete their work
- Provides comprehensive formatting across all generated files
- Handles formatting errors gracefully with status reporting

**Formatting Workflow:**
- Code generation skills focus purely on code generation
- `solution-code-formatter` skill called at appropriate workflow points
- Provides consistent code style across entire solution
- Reduces merge conflicts and maintains code quality standards

**Code Architecture:**
- Skills communicate through Copilot orchestration, not direct imports
- Main generation functions do not export (removed unnecessary exports)
- Only `project-utilities` exports functions for shared use across skills
- Formatting logic centralized in dedicated skill

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

### Scenario 1: New Project (Layered Architecture Development)
0. `project-creator` → Create project structure, configure .csproj files, set up development environment
1. `workflow-orchestrator` → Analyze project state, recommend complete generation process
2. `domain-model-parser` → Parse domain models
3. `enum-generator` → Generate enum definitions
4. `entity-class-generator` → Generate basic entity classes
5. `entity-configuration-generator` → Generate EF Core configurations
6. `data-context-generator` → Generate DbContext class
7. `database-migration-generator` → Generate database scripts
8. `solution-code-formatter` → Format all generated code (data layer)
9. `repository-interface-generator` → Generate Repository interfaces
10. `efcore-repository-generator` → Generate EF Core Repository implementations
11. `service-interface-generator` → Generate service interfaces (contracts only)
12. `solution-code-formatter` → Format all generated code (service layer)
**[Layered Architecture UI Development Phase]**
13. `blazor-architecture-generator` → Intelligent component library selection and architectural guidance
**[Component Library Generation Phase]**
14a. **Choose One**: `mudblazor-generator` | `syncfusion-generator` | `bootstrap-generator` | `minimal-generator`
14b. `blazor-data-integration-generator` → Generate data integration layer and state management
**[Page Pattern Generation Phase]**
15. `blazor-page-pattern-generator` → Generate abstract page patterns and routing
16. `blazor-list-pattern-generator` → Generate list page business logic patterns
17. `blazor-form-pattern-generator` → Generate form page business logic and validation
18. `blazor-detail-pattern-generator` → Generate detail page logic and related data handling
19. `solution-code-formatter` → Format all generated code (UI layer)
**[Theming and Business Logic Phase]**
20. `blazor-theme-generator` → Enterprise theme consultation and styling guidance
21. `page-driven-service-generator` → Generate service implementations based on page operations
22. `solution-code-formatter` → Format all generated code (final formatting)

### Scenario 2: Component Library Migration (MudBlazor → Syncfusion) **🔄 UPDATED**
1. `workflow-orchestrator` → Detect component library migration intent
2. `syncfusion-generator` → **AI-Driven**: Provide consultation for migration strategy and implementation approach
   - Analyze existing MudBlazor implementation
   - Recommend optimal Syncfusion component mappings
   - Guide migration path and resource optimization
   - Provide enterprise-specific implementation recommendations
3. **Implementation Approach**: Manual implementation guided by AI consultant
4. **No data layer changes needed** - Only UI layer files need updates
5. **Result**: Strategic migration plan with expert guidance (consultative approach)

### Scenario 3: Domain Model Modification
1. `workflow-orchestrator` → Detect model changes, recommend incremental update process
2. `model-change-detector` → Analyze specific changes
3. `incremental-update-generator` → Generate update scripts based on changes
4. Selectively call affected generators:
   - **Entity changes**: `enum-generator` + `entity-class-generator` + `entity-configuration-generator`
   - **New entities**: 数据层 + 页面模式层 (only abstract patterns, adapters remain)
   - **Relationship changes**: `entity-configuration-generator` + `data-context-generator` + `blazor-data-integration-generator`
   - **UI changes**: Only regenerate affected page patterns, adapters auto-adapt
5. `solution-code-formatter` → Format all modified code
6. `test-code-generator` → Generate/Update unit tests for changed code
7. `configuration-file-manager` → Update configuration files if required by changes

### Scenario 4: Add New Component Library (Maintain Multi-Library Coexistence)
1. `workflow-orchestrator` → Based on user intent, recommend adapter-only generation
2. `blazor-component-adapter-base` → Extend base framework if needed
3. **New Component Library Adapter** → Create adapter for new component library (e.g., AntDesign, DevExpress)
4. `blazor-theme-generator` → Generate themes compatible with new component library
5. **No business logic changes** - Page pattern layer and data layer remain unchanged

### Scenario 5: Update Specific Layer Only
1. `workflow-orchestrator` → Based on user intent, recommend specific skill combinations
2. Call relevant specific generators:
   **Entity Layer**: `enum-generator` + `entity-class-generator` + `entity-configuration-generator`
   **Data Layer**: `repository-interface-generator` + `efcore-repository-generator` + `blazor-data-integration-generator`
   **Business Layer**: `service-interface-generator` + `page-driven-service-generator`
   **UI Architecture Layer**: `blazor-architecture-generator` + `blazor-data-integration-generator`
   **UI Pattern Layer**: `blazor-page-pattern-generator` + `blazor-list-pattern-generator` + `blazor-form-pattern-generator` + `blazor-detail-pattern-generator`
   **UI Adapter Layer**: Any specific component library adapter (Syncfusion/Material/etc.)
   **Styling Layer**: `blazor-theme-generator`
   **Database**: `database-migration-generator` + `data-context-generator`

## Integration Points

### Target Code Locations
- `src/Sanjel.RequestManagement.Core/` - Entity classes and services
- `src/Sanjel.RequestManagement.Repositories/` - Repository classes  
- `src/Sanjel.RequestManagement.Blazor/Components/` - Layout components (MainLayout.razor, NavMenu.razor)
- `src/Sanjel.RequestManagement.Blazor/Pages/` - Page files (List, Create, Edit, Detail pages)
- `src/Sanjel.RequestManagement.Blazor/wwwroot/` - Static assets (CSS, JS, theme files)
- `src/Sanjel.RequestManagement.Blazor/_Imports.razor` - Component library namespace imports
- `src/Sanjel.RequestManagement.Blazor/Program.cs` - Service registrations and configuration

### Configuration Integration
- Update `.csproj` file dependencies
- Modify `appsettings.json` database connections
- Integrate with existing DI container configuration

### Testing Support
- Generate unit tests for generated code
- Integrate with existing test projects

## Benefits

1. **Layered Architecture Flexibility**: Component libraries can be replaced without affecting business logic
2. **Technology Stack Adaptation**: Supports multiple component libraries like Syncfusion, Material Design, AntDesign, etc.
3. **Progressive Migration**: Gradual component library migration without requiring complete rewrites
4. **Consistency**: Ensures all generated code follows the same patterns and standards
5. **Efficiency**: Reduces manual coding time and potential errors
6. **Maintainability**: Keeps code synchronized with domain model changes
7. **Quality**: Enforces best practices and design patterns
8. **Traceability**: Maintains clear connection between domain models and implementation
9. **Modularity**: Fine-grained skills allow selective updates and easier debugging
10. **Flexibility**: Uses modern EF Core implementation with performance optimization
11. **Scalability**: Individual skills can be enhanced without affecting others
12. **Enterprise Scalability**: Supports parallel use of multiple component libraries and dynamic switching
13. **Development Efficiency**: Complete separation of business logic and UI, enabling parallel development

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
10. Implement `test-code-generator` for automated unit test generation
11. Implement `configuration-file-manager` for environment and appsettings management

## Implementation Priority

**Phase 0 (Foundation)**:
- `project-utilities` ✅ **Implemented** - Shared utility functions for all skills
- `solution-code-formatter` ✅ **Required** - Dedicated code formatting skill
- `project-creator` ❌ **Missing** - Project structure and configuration setup

**Phase 1 (Foundation)**: Skills 1, 2, 3, 4a, 4b, 5, 9 - Basic enum, entity and database generation ✅ **Complete**
**Phase 2 (Data Access)**: Skills 6a, 6b - Repository pattern implementation ✅ **Complete (6a Complete, 6b Implemented)**
**Phase 3 (Service Contracts)**: Skill 7a - Service interface generation ✅ **Complete**
**Phase 4 (Component Library Architecture)**: Skills 8a, 8a1-8a4 - Component library selection and generation ✅ **Complete (Redesigned)**
  - `blazor-architecture-generator` ✅ - Intelligent selector and consultation
  - `mudblazor-generator` ✅ - Material Design Blazor generation
  - `syncfusion-generator` ✅ - Enterprise Syncfusion consultation 
  - `bootstrap-generator` ✅ - Bootstrap 5 CSS framework consultation
  - `minimal-generator` ✅ - Zero-dependency performance consultation
**Phase 5 (Data Integration)**: Skill 8b - Blazor data integration layer 📋 **NEW PRIORITY**
**Phase 6 (Page Patterns)**: Skills 8c, 8d, 8e, 8f - Abstract page pattern generation 📋 **PENDING**
**Phase 7 (Theme Styling)**: Skill 8j - Theme and styling consultation ✅ **Complete (AI-Driven)**
**Phase 8 (Business Integration)**: Skill 7b - Service implementation based on page operations 📋 **REDESIGNED (AI-Driven)**
**Phase 9 (Change Management)**: Skills 10, 11 - Model change detection and updates 📋 **PENDING**

## New Architecture Advantages Comparison

### 📊 Old Architecture vs New Architecture

| Aspect                        | Old Architecture (Direct Component Generation) | New Architecture (Layered Adaptation)                  |
| ----------------------------- | ---------------------------------------------- | ------------------------------------------------------ |
| **Component Library Binding** | ✅ Tightly bound to MudBlazor/Syncfusion        | ✅ Flexible adaptation to different component libraries |
| **Migration Cost**            | ❌ Complete code rewrite required               | ✅ Only adapter replacement needed                      |
| **Business Logic Reuse**      | ❌ Tightly coupled with UI                      | ✅ Completely separated and reusable                    |
| **Development Maintenance**   | ❌ Changes require modifying multiple skills    | ✅ Independent maintenance per layer                    |
| **Technology Stack Choice**   | ❌ Limited to initial selection                 | ✅ Support for parallel multi-stack                     |
| **Enterprise Adaptation**     | ❌ Difficult to meet different team needs       | ✅ Flexible adaptation to enterprise standards          |

## Notes

- All skills designed for VS Code Copilot integration
- **Primary Approach**: AI-driven consultation and guidance (preferred for flexibility)
- **Secondary Approach**: Script-driven automation (for well-defined, repetitive tasks only)
- Script-driven skills use bun runtime for TypeScript execution
- No third-party dependencies allowed in scripts (Node.js modules only)
- Follows single responsibility principle (one skill = one focused task)
- **Architecture Principle**: AI-first approach for maximum adaptability and learning value

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

**Next Implementation:** Continue with `blazor-list-component-generator`, `blazor-form-component-generator`, and `blazor-detail-component-generator` to complete the UI layer, then implement page-driven service implementations

**Phase 4 Progress: `blazor-page-generator` ✅ COMPLETED**

Successfully implemented and tested the `blazor-page-generator` skill:

**Generated Pages:** 28 complete Blazor pages across 7 entities
- **Request Pages**: List, Create, Edit, Detail (.razor files with full routing)
- **DataElement Pages**: List, Create, Edit, Detail  
- **Engineer Pages**: List, Create, Edit, Detail
- **Manager Pages**: List, Create, Edit, Detail
- **Notification Pages**: List, Create, Edit, Detail
- **ReviewPackage Pages**: List, Create, Edit, Detail
- **StateDiagram Pages**: List, Create, Edit, Detail

**Key Features Implemented:**
- ✅ Auto-detection of project structure and metadata
- ✅ MudBlazor integration with responsive layouts
- ✅ Route generation with @page directives
- ✅ Service injection and dependency integration
- ✅ Navigation breadcrumbs and user-friendly URLs
- ✅ Error handling and loading states
- ✅ Action buttons and toolbar integration
- ✅ Component composition (references to List/Form/Detail components)

**Architecture Benefits:**
- **Page-First Development**: Establishes UI structure before component implementation
- **Service Integration**: Pages reference generated service interfaces
- **Consistent Patterns**: All pages follow the same architectural patterns
- **Component Ready**: Pages are prepared for integration with component generators

## 🔄 Architecture Shift: Service-First → Page-First Development

**Planning Adjustment Made**: March 3, 2026

### 📋 Key Changes

**1. Reordered Skill Execution Priority:**
- **Before**: Service Interface → Service Implementation → UI Components
- **After**: Service Interface → UI Pages → UI Components → Page-Driven Service Implementation

**2. New Skill Added: `blazor-page-generator`**
- Generates complete `.razor` page files with routing
- Creates page-level structure and navigation
- Establishes the foundation for component integration

**3. Redesigned: `service-implementation-generator` → `page-driven-service-generator`**
- Analyzes actual page operations to determine required service methods
- Generates only necessary business logic based on UI requirements
- Prevents over-engineering and ensures UI-Service alignment

### 🎯 Benefits of Page-First Approach

**1. Requirements-Driven Development:**
- Service methods generated based on actual UI operations
- Eliminates unused or speculative business logic
- Ensures tight coupling between user requirements and implementation

**2. Iterative Development Support:**
- Pages can be developed and tested independently
- Service logic follows proven UI workflows
- Faster feedback cycles through UI-first prototyping

**3. Better Architecture Alignment:**
- Follows modern front-end development patterns
- Supports MVP (Minimum Viable Product) approach
- Enables user testing before backend complexity

**4. Reference Implementation Compliance:**
- Aligns with `/sanjel/eServiceCloud/docs/实现模式-008-Service.md` patterns
- Follows `/sanjel/eServiceCloud/docs/实现模式-002-Page.md` page structure guidelines
- Maintains organizational development standards

### 🚀 Implementation Flow

```
Domain Model → Entities → Data Layer → Service Contracts → 
Pages → Components → Page-Driven Services → Testing
```

This approach ensures that the generated code directly supports user workflows while maintaining clean architecture principles.
