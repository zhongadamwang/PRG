---
name: project-document-management
description: Initialize and manage project documentation structures following hierarchical folder guidelines. Creates consistent project trees with requirements, artifacts, and organizational modeling documents.
license: MIT
---

# Project Document Management

Creates and manages standardized project documentation structures using established hierarchical folder guidelines. Ensures consistency across projects and proper organization of requirements, artifacts, and modeling documents.

## Core Function

**Input**: Project name, type, initial requirements
**Output**: Complete project folder structure with template files

## Usage Scenarios

1. **New Project Initialization**: Create complete folder structure for greenfield projects
2. **Project Migration**: Restructure existing projects to follow standards  
3. **Structure Validation**: Verify project follows documentation guidelines
4. **Template Generation**: Create project templates with standard files

## Project Initialization Workflow

### 1. Create Base Project Structure

```
orgDocument/projects/[NN] - [project name]/
├── artifacts/             # Analysis outputs and supporting materials
│   ├── Analysis/         # Analysis documents
│   ├── Requirements/     # Detailed requirement specifications  
│   ├── Sample Data/      # Test data and examples
│   └── UI Mockups/       # Design mockups and wireframes
├── tasks/                # Individual task files in GitHub issue format
│   ├── T##-task-name.md  # Task files aligned with project plan numbering
│   ├── task-tracking.md  # Overall task progress tracking
│   └── README.md         # Task management workflow documentation
├── main.md               # Project overview and navigation
├── project-plan.md   # Detailed project plan with PERT analysis
├── README.md             # Project documentation guide
└── [other planning docs] # Additional project documents as needed
```

### 2. Update Project Registry

Add entry to `orgDocument/projects/projects.md`:
```markdown
| NN | [Project Name] | [Brief Description] | [Status] | [Start Date] |
```

### 3. Initialize Core Process Modeling (if applicable)

```
orgDocument/orgModel/[NN] - [Process Name]/
├── main.md               # Process overview  
├── process.md            # Mermaid Activity Diagram
├── collaboration.md      # Mermaid sequence diagram
├── domain-model.md       # Actors, systems, entities
├── vocabulary.md         # Canonical names mapping
├── test-case-list.md     # Test case master list
└── test-cases/           # Individual test case files
    └── tc-[identifier]-[3-digit-sequence].md
```

## Standard File Templates

### Project main.md Template
```markdown
# [NN] - [Project Name]

## Overview
[Brief project description and objectives]

## Structure
- `artifacts/` - Supporting materials and analysis outputs
  - `Analysis/` - Technical analysis documents
  - `Requirements/` - Requirements and specifications  
  - `Sample Data/` - Test data and examples
  - `UI Mockups/` - Design mockups and wireframes
- `tasks/` - Individual task files in GitHub issue format
  - Task files can be imported to GitHub Issues for team collaboration
  - Feedback added as issue comments in GitHub
  - Issues can be exported from GitHub to update project documents

## Key Documents
- [Project Plan](project-plan.md) - Detailed MVP planning with PERT analysis
- [Tasks Folder](tasks/) - Individual task files in GitHub issue format for team collaboration
- [Analysis artifacts (artifacts/Analysis/)](artifacts/Analysis/) - Technical analysis documents
- [Requirements artifacts (artifacts/Requirements/)](artifacts/Requirements/) - Project requirements and specifications
- [Related Process Model](../../orgModel/[NN]%20-%20[Process%20Name]/main.md) - Linked organizational process

## Status
- **Phase**: [Current project phase]
- **Last Updated**: [Date]
- **Next Milestone**: [Upcoming milestone]
```

### Process main.md Template  
```markdown
# [NN] - [Process Name]

## Business Model Overview
[Description of the business model at this process level]

## Requirements Source
[Overview of requirements driving this process level]

## Process Scope
[Specific scope and boundaries for this process granularity]

## Business Context
[Business context and rationale for this process level]

## Key Stakeholders
[Primary stakeholders involved at this process level]

## Process Flow
See [process.md](process.md) for detailed activity diagram.

## Collaborations
See [collaboration.md](collaboration.md) for entity interactions.

## Domain Model
See [domain-model.md](domain-model.md) for actors and entities.

## Sub-Processes
[List of sub-process folders if any]

## Test Coverage
See [test-case-list.md](test-case-list.md) for verification test cases.
```

## Quick Commands

### Initialize New Project
**Parameters**: project_number, project_name, description, project_type
**Actions**:
1. Create numbered project folder
2. Generate standard subfolder structure (artifacts/, tasks/)
3. Create template main.md and README.md files
4. Initialize project-plan.md with PERT analysis template
5. Update projects.md registry
6. Initialize artifact subfolders
7. Create tasks/ folder with README.md and task tracking template

### Initialize Process Model
**Parameters**: process_number, process_name, scope_description  
**Actions**:
1. Create numbered process folder
2. Generate core modeling files (main.md, process.md, collaboration.md, etc.)
3. Create test-cases folder structure
4. Initialize vocabulary.md for canonical naming

### Structure Validation
**Actions**:
1. Verify folder naming conventions
2. Check required files presence
3. Validate cross-references
4. Report missing elements

## Naming Conventions

### Folders
- Projects: `[NN] - [Project Name]` (e.g., "01 - Customer Portal")
- Processes: `[NN] - [Process Name]` (e.g., "02 - User Authentication")
- Artifacts: Descriptive names without numbering (e.g., "Analysis", "UI Mockups")

### Files  
- Main documentation: `main.md`
- Process diagrams: `process.md`  
- Collaboration diagrams: `collaboration.md`
- Domain models: `domain-model.md`
- Vocabulary: `vocabulary.md`
- Test case lists: `test-case-list.md`
- Individual test cases: `tc-[identifier]-[3-digit-sequence].md`

## Integration Points

- References document tree guidelines in `orgDocument/instructions.md`
- Coordinates with `requirements-ingest` skill for requirement processing
- Supports `goals-extract` skill output for project initialization
- Integrates with test case management workflows

## Best Practices

1. **Consistent Numbering**: Use sequential numbering for projects and processes
2. **Clear Naming**: Use descriptive, business-friendly names  
3. **Complete Templates**: Initialize all core files even if initially empty
4. **Cross-linking**: Maintain navigation links between related documents
5. **Registry Updates**: Always update central registries when creating new items
6. **Hierarchical Organization**: Follow parent-child relationships in process breakdowns