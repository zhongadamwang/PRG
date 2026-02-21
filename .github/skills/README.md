# EDPS Skills Documentation

A comprehensive guide to using the Evolutionary Development Process System (EDPS) skills with GitHub Copilot.

## Quick Start 🚀

**Essential Prompt Pattern:**
```
@workspace Use the [skill-name] skill to [specific action] on [input description].

[Your content here]

Project ID: [YOUR-PROJECT-ID]
```

## Skills Overview

### 📋 Requirements Processing
| Skill | Purpose | Input | Output | Next Step |
|-------|---------|-------|---------|-----------|
| `requirements-ingest` | Normalize any format to structured requirements | Raw docs (PDF/Word/MD) | requirements.json/md | → `goals-extract` |
| `goals-extract` | Extract business goals and success criteria | requirements.json | goals.json/md | → `process-w5h` |
| `process-w5h` | WHO/What/When/Where/Why/How analysis | requirements.json | w5h-analysis.json/md | → `domain-extractconcepts` |

### 🎯 Domain Modeling
| Skill | Purpose | Input | Output | Next Step |
|-------|---------|-------|---------|-----------|
| `domain-extractconcepts` | Identify domain entities and concepts | requirements.json | domain-concepts.json/md | → `domain-alignentities` |
| `domain-alignentities` | Align concepts with existing domain models | domain-concepts.json | domain-alignment.json/md | → `domain-proposenewconcepts` |
| `domain-proposenewconcepts` | Propose new concepts for gaps | domain-alignment.json | domain-newconcepts.json/md | → `diagram-generatecollaboration` |

### 📊 Analysis & Visualization
| Skill | Purpose | Input | Output | Next Step |
|-------|---------|-------|---------|-----------|
| `process-scopemin` | Define MVP and minimal scope | requirements.json | scope-analysis.json/md | → Planning |
| `diagram-generatecollaboration` | Generate system interaction diagrams | domain artifacts | collaboration-diagrams.json/md | → Documentation |

### 🔄 Process Management
| Skill | Purpose | Input | Output | Next Step |
|-------|---------|-------|---------|-----------|
| `process-merge` | Merge multiple requirement sources | Multiple requirement documents | Unified requirements.json/md | → Analysis |
| `change-management` | Track and document requirement changes | Change requests | Change documentation | → Update artifacts |

### 📁 Project Management
| Skill | Purpose | Input | Output | Next Step |
|-------|---------|-------|---------|-----------|
| `project-document-management` | Initialize project structure | Project details | Project folder tree | → Requirements |
| `project-planning-tracking` | Create project plans and timelines | Project scope | Project plans and tracking | → Execution |
| `project-status-reporting` | Generate status reports | Project artifacts | Status reports and dashboards | → Stakeholder review |
| `plan-derivetasks` | Convert requirements into actionable tasks | requirements.json, goals.json | task-breakdown.json/md | → `plan-estimateeffort` |
| `plan-estimateeffort` | Generate effort estimates with confidence levels | task-breakdown.json | effort-estimates.json/md | → `plan-buildschedule` |
| `plan-buildschedule` | Generate project schedules with dependencies | task-breakdown.json, effort-estimates.json | project-schedule.json/md | → Resource allocation |

### 🧠 Meta Skills
| Skill | Purpose | Input | Output | Next Step |
|-------|---------|-------|---------|-----------|
| `edps-skill-navigator` | Navigate and orchestrate skills | Natural language task description | Skill execution plan | → Execute recommended skills |
| `skill-creator` | Create new EDPS skills | Skill requirements | New skill structure | → Implement skill |

## Common Workflows

### 🏁 New Project Setup (5 minutes)
```markdown
@workspace Use project-document-management skill to initialize a new project structure:

Project Name: Customer Portal Redesign
Project ID: CPR-2024
Description: Modernize customer portal with improved UX and mobile support
Stakeholders: Product, Engineering, Design, Customer Success
```

### 📑 Requirements Analysis Pipeline (15 minutes)
```markdown
@workspace Execute this requirements analysis workflow:

1. Use requirements-ingest skill on attached requirements document
2. Use goals-extract skill on the resulting requirements
3. Use process-w5h skill for comprehensive analysis
4. Use domain-extractconcepts skill to identify key entities

Project ID: CPR-2024

[Attach your requirements document here]
```

### 🔄 Iterative Planning (10 minutes)
```markdown
@workspace Help me scope this project for MVP delivery:

1. Use process-scopemin skill to identify core vs optional features
2. Use project-planning-tracking skill to create a delivery timeline
3. Generate initial task breakdown

Project: Customer Portal Redesign (CPR-2024)
Timeline: 3 months
Team Size: 5 developers, 1 designer
```

### 📊 Complete Planning Pipeline (20 minutes)
```markdown
@workspace Execute full planning workflow from requirements to schedule:

1. Use plan-derivetasks skill to break down requirements into actionable tasks
2. Use plan-estimateeffort skill to estimate effort for all tasks
3. Use plan-buildschedule skill to generate project schedule with critical path

Project: Customer Portal Redesign (CPR-2024)
Team: 5 developers (3 senior, 2 junior), 1 designer
Deadline: 12 weeks from start
```

## Advanced Usage Patterns

### Skill Chaining
```markdown
@workspace Execute this complete requirements-to-design workflow:

1. Use requirements-ingest → goals-extract → process-w5h → domain-extractconcepts → domain-alignentities → diagram-generatecollaboration

Input: [Your requirements document]
Project: [PROJECT-ID]

Please chain these skills automatically and provide final collaboration diagrams.
```

### Change Management
```markdown
@workspace Handle this requirement change:

1. Use change-management skill to document this change request
2. Update affected artifacts using process-merge skill
3. Generate impact analysis with project-status-reporting skill

Change: Add real-time notifications to the customer portal
Project: CPR-2024
```

## File Structure Convention

All skills output to standardized locations:
```
projects/[PROJECT-ID]/
├── artifacts/
│   ├── Analysis/          # Core analysis outputs
│   │   ├── requirements.json/md
│   │   ├── goals.json/md  
│   │   ├── w5h-analysis.json/md
│   │   ├── domain-concepts.json/md
│   │   ├── domain-alignment.json/md
│   │   ├── scope-analysis.json/md
│   │   ├── collaboration-diagrams.json/md
│   │   ├── task-breakdown.json/md      # Planning outputs
│   │   ├── effort-estimates.json/md
│   │   ├── project-schedule.json/md
│   │   └── critical-path-analysis.md
│   ├── Changes/           # Change management
│   │   └── [DATE]-[TYPE]-CHG-[NUM]-[description].md
│   ├── Requirements/      # Input documents
│   └── UI Mockups/        # Design artifacts
└── tasks/                 # Project planning outputs
    ├── project-plan.md
    ├── task-tracking.md
    └── T[NN]-[task-name].md
```

## Tips for Success 💡

### Effective Prompting
- **Be specific**: Include project IDs and clear input descriptions
- **Chain related skills**: Use workflows instead of individual skills when possible
- **Validate outputs**: Check generated artifacts before proceeding to next steps
- **Iterate**: Refine inputs based on skill outputs for better results

### Project Organization
- **Start with structure**: Always use `project-document-management` first
- **Follow the pipeline**: Requirements → Goals → Analysis → Domain → Planning
- **Version control**: Skills create timestamped outputs for change tracking
- **Review outputs**: Each skill generates both JSON (machine) and MD (human) formats

### Common Patterns
```markdown
# Quick Analysis
@workspace Quick requirements analysis using requirements-ingest and goals-extract skills:
[Content]

# Full Domain Analysis  
@workspace Complete domain analysis pipeline (requirements-ingest → goals-extract → process-w5h → domain-extractconcepts):
[Content]

# Change Request
@workspace Process this change using change-management skill and update project status:
[Change description]
```

## Next Steps

1. **Browse individual skill guides**: Each skill has detailed documentation in its folder
2. **Try the examples**: Start with simple single-skill invocations
3. **Build workflows**: Chain skills together for comprehensive analysis
4. **Customize**: Adapt prompt patterns to your specific domain and needs

See `/examples/` folder for complete project walkthroughs and `/workflows/` for advanced skill combination patterns.