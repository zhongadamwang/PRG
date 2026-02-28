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

### Skill 2: `domain-model-parser`
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

### Skill 3: `entity-class-generator` 
**Responsibility**: Generate C# entity classes based on parsed metadata
**Input**: JSON output from domain-model-parser
**Output**: C# entity class files

**Script**: `generate-entities.ts` (run with bun)
- Generate EF Core entity classes
- Handle attribute type mapping
- Add Data Annotations
- Generate navigation properties

### Skill 4: `database-migration-generator`
**Responsibility**: Generate EF Core database migration scripts
**Input**: Entity metadata JSON
**Output**: EF Core Migration files

**Script**: `generate-migration.ts` (run with bun)
- Generate Migration classes
- Create table structure scripts
- Add indexes and constraints
- Handle foreign key relationships

### Skill 5: `repository-generator`
**Responsibility**: Generate Repository pattern code
**Input**: Entity metadata JSON
**Output**: Repository interfaces and implementation classes

**Script**: `generate-repositories.ts` (run with bun)
- Generate IRepository interfaces
- Generate Repository implementation classes
- Add common CRUD methods
- Handle complex query requirements

### Skill 6: `service-layer-generator`
**Responsibility**: Generate business service layer code
**Input**: Entity metadata JSON
**Output**: Service interfaces and implementation classes

**Script**: `generate-services.ts` (run with bun)
- Generate business service interfaces
- Generate service implementation classes
- Add business logic methods
- Handle transaction management

### Skill 7: `blazor-component-generator`
**Responsibility**: Generate basic Blazor CRUD components
**Input**: Entity metadata JSON
**Output**: Blazor component files

**Script**: `generate-blazor-components.ts` (run with bun)
- Generate list components
- Generate edit form components
- Generate detail display components
- Add basic data binding

### Skill 8: `model-change-detector`
**Responsibility**: Detect changes in domain model files
**Input**: Current and historical versions of domain models
**Output**: Change difference report

**Script**: `detect-changes.ts` (run with bun)
- Compare model file changes
- Identify added/deleted/modified entities
- Analyze attribute and relationship changes
- Generate change summary

### Skill 9: `incremental-update-generator`
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
│   ├── repository-generator/
│   │   ├── SKILL.md
│   │   └── scripts/
│   │       └── generate-repositories.ts
│   └── service-layer-generator/
│       ├── SKILL.md
│       └── scripts/
│           └── generate-services.ts
├── database/
│   └── database-migration-generator/
│       ├── SKILL.md
│       └── scripts/
│           └── generate-migration.ts
├── ui-generation/
│   └── blazor-component-generator/
│       ├── SKILL.md
│       └── scripts/
│           └── generate-blazor-components.ts
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
3. `entity-class-generator` → Generate entity classes
4. `database-migration-generator` → Generate database scripts
5. `repository-generator` → Generate Repository layer
6. `service-layer-generator` → Generate service layer
7. `blazor-component-generator` → Generate UI components

### Scenario 2: Domain Model Modification
1. `workflow-orchestrator` → Detect model changes, recommend incremental update process
2. `model-change-detector` → Analyze specific changes
3. `incremental-update-generator` → Generate update scripts based on changes
4. Selectively call related generators based on change impact

### Scenario 3: Update Specific Layer Only
1. `workflow-orchestrator` → Based on user intent, recommend specific skill combinations
2. Call relevant specific generators

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

## Next Steps

1. Create the skill directory structure
2. Implement the workflow-orchestrator skill first
3. Develop individual generator skills incrementally
4. Test with existing domain model
5. Refine based on real-world usage

## Notes

- All skills designed for VS Code Copilot integration
- Uses bun runtime for TypeScript execution
- No third-party dependencies allowed (Node.js modules only)
- Follows single responsibility principle (one skill = one focused task)