---
name: edps-skill-navigator
description: Integrate with Copilot to provide natural language navigation and orchestration of EDPS (Evolutionary Development Process System) skills. Acts as an intelligent skill discovery and workflow orchestration assistant within the Copilot framework.
license: MIT
---

# EDPS Skill Navigator

An intelligent assistant that seamlessly integrates with GitHub Copilot to provide natural language navigation, discovery, and orchestration of the Evolutionary Development Process System (EDPS) skills ecosystem.

## Core Function

**Purpose**: Transform natural language requests into optimal skill invocation patterns and workflows
**Input**: User intent expressed in natural language via Copilot
**Output**: Skill recommendations, orchestrated workflows, and guided execution paths
**Integration**: Native Copilot skill that understands user context and available skill capabilities

## Core Capabilities

### 1. Intelligent Skill Discovery
- **Natural Language Parsing**: Interprets user requests like "help me process requirements" or "analyze my domain model"
- **Context Awareness**: Considers project stage, available artifacts, and previous work to recommend appropriate skills
- **Capability Mapping**: Matches user intent to skill capabilities across the entire EDPS toolkit

### 2. Workflow Orchestration
- **Multi-Skill Coordination**: Sequences multiple skills for complex workflows (e.g., requirements → domain analysis → collaboration diagrams)
- **Dependency Management**: Ensures prerequisite skills are executed before dependent skills
- **Progress Tracking**: Monitors workflow completion and suggests next steps

### 3. Copilot Integration Optimization
- **Natural Invocation**: Enables users to say "analyze requirements using EDPS" instead of remembering specific skill names
- **Context Preservation**: Maintains conversation flow while invoking skills behind the scenes
- **Interactive Guidance**: Provides real-time assistance and suggestions during skill execution

## Usage Patterns

### Direct Skill Navigation
```
User: "I have requirements that need processing"
Navigator: Invokes requirements-ingest skill automatically, then suggests next steps

User: "Help me understand my domain model"
Navigator: Guides through domain-extractconcepts → domain-alignentities → domain-proposenewconcepts workflow
```

### Project Stage Recognition
```
User: "I'm starting a new project with these requirements"
Navigator: Orchestrates complete initialization workflow:
1. project-document-management (structure setup)
2. requirements-ingest (requirement processing)
3. goals-extract (objective clarification)
4. process-w5h (comprehensive analysis)
```

### Intelligent Recommendations
```
User: "What should I do next?"
Navigator: Analyzes current project state and suggests:
- Missing analysis steps
- Available next actions
- Optimal skill sequences
- Quality checkpoints
```

## EDPS Methodology Integration

### Evolutionary Development Principles
- **Iterative Refinement**: Guides users through iterative improvement cycles
- **Continuous Integration**: Ensures skills work together harmoniously
- **Adaptive Planning**: Adjusts recommendations based on project evolution
- **Knowledge Accumulation**: Builds understanding progressively through skill interactions

### Process Awareness
- **Stage Recognition**: Identifies current development phase (discovery, analysis, design, implementation)
- **Transition Guidance**: Smoothly guides users between development stages
- **Quality Gates**: Ensures completeness before advancing to next phase
- **Artifact Dependencies**: Tracks and manages inter-skill dependencies

## Copilot Integration Strategies

### Natural Language Processing
```copilot-pattern
# User Intent Recognition
"analyze requirements" → requirements-ingest + goals-extract
"merge requirements" → requirements-merge
"understand domain" → domain-extractconcepts + domain-alignentities  
"create diagrams" → diagram-generatecollaboration
"plan project" → plan-derivetasks + plan-estimateeffort + plan-buildschedule
"estimate effort" → plan-estimateeffort
"create tasks" → plan-derivetasks
"build schedule" → plan-buildschedule
"track project" → project-planning-tracking + process-scopemin
"track changes" → change-management
"update organization model" → orgmodel-update
"integrate models" → model-integration
"update top requirements" → process-findtopandupdate
"test integration" → integration-testing
"validate workflows" → integration-testing
```

### Conversational Flows
```copilot-interaction
User: "I need to start working on requirements analysis"
Navigator: 
1. "I'll help you with requirements analysis. First, let me set up your project structure..."
2. [Invokes project-document-management]
3. "Great! Now let's process your requirements. Please provide your requirements document..."
4. [Invokes requirements-ingest]
5. "Excellent! I've processed your requirements. Would you like me to extract goals and success criteria next?"

User: "I need to plan my project and estimate effort"
Navigator:
1. "I'll help you create a comprehensive project plan. Let me start by deriving tasks from your requirements..."
2. [Invokes plan-derivetasks]
3. "Perfect! I've identified the key tasks. Now let me estimate effort for each task..."
4. [Invokes plan-estimateeffort]
5. "Great estimates! Now I'll build a detailed schedule with dependencies..."
6. [Invokes plan-buildschedule]
7. "Your project plan is ready! Would you like me to integrate this with your organizational model?"

User: "I need to integrate new processes with our existing organizational model"
Navigator:
1. "I'll help you integrate your processes safely. Let me start by merging the process models..."
2. [Invokes process-merge]
3. "Process merge complete. Now I'll find and update top-level requirements that may be affected..."
4. [Invokes process-findtopandupdate]
5. "Updates identified. Now I'll integrate the changes into your organizational model..."
6. [Invokes model-integration]
7. "Integration complete! Let me update the organizational documentation..."
8. [Invokes orgmodel-update]
9. "Everything is updated. Would you like me to run integration tests to validate the changes?"
```

### Context-Aware Assistance
- **Progressive Disclosure**: Reveals relevant skills as user progresses
- **Intelligent Defaults**: Pre-configures skills based on project context
- **Error Recovery**: Guides users when skills encounter issues
- **Learning Loop**: Improves recommendations based on user patterns

## Skill Ecosystem Navigation

### Available Skills Catalog
```
Requirements Processing:
├── requirements-ingest       # Normalize and structure requirements
├── requirements-merge        # Combine multiple requirement sources
├── goals-extract            # Extract business goals and success criteria
└── process-w5h             # Comprehensive requirements analysis

Domain Analysis:
├── domain-extractconcepts   # Identify domain entities and relationships
├── domain-alignentities    # Align concepts with organizational standards
└── domain-proposenewconcepts # Suggest domain extensions

Process & Planning:
├── process-merge           # Integrate process models with organizational models
├── process-findtopandupdate # Update top-level requirements based on analysis
├── process-scopemin        # Identify minimum viable scope
├── plan-derivetasks        # Convert requirements into actionable tasks
├── plan-estimateeffort     # Provide effort estimates for development tasks
├── plan-buildschedule      # Generate project schedules with dependencies
├── project-planning-tracking # Plan and track project milestones
└── project-status-reporting # Generate status reports

Visualization & Documentation:
├── diagram-generatecollaboration # Create Mermaid collaboration diagrams  
├── project-document-management   # Manage project documentation structure
└── change-management            # Track and document changes

Model & Integration Management:
├── model-integration       # Integrate new models into existing structures
├── orgmodel-update        # Update organizational model documents
└── integration-testing     # Validate end-to-end skill workflows

Quality & Development:
└── skill-creator          # Create new skills when needed
```

### Workflow Templates
```
Complete Project Initiation:
project-document-management → requirements-ingest → goals-extract → process-w5h → domain-extractconcepts

Requirements Analysis Deep Dive:
requirements-ingest → goals-extract → process-w5h → process-scopemin → requirements-merge (if multiple sources)

Domain Modeling Workflow:
domain-extractconcepts → domain-alignentities → domain-proposenewconcepts → diagram-generatecollaboration

Project Planning Workflow:
goals-extract → process-scopemin → plan-derivetasks → plan-estimateeffort → plan-buildschedule

Process Integration Workflow:
process-merge → process-findtopandupdate → model-integration → orgmodel-update

Change Management Cycle:
change-management → [affected skill execution] → orgmodel-update → project-status-reporting

End-to-End Organization Integration:
requirements-ingest → domain-extractconcepts → model-integration → orgmodel-update → integration-testing

Complete Development Lifecycle:
project-document-management → requirements-ingest → goals-extract → process-w5h → 
domain-extractconcepts → plan-derivetasks → plan-estimateeffort → plan-buildschedule → 
diagram-generatecollaboration → integration-testing
```

## Implementation Guidelines

### For Copilot Users
1. **Natural Interaction**: Simply describe what you want to accomplish in plain language
2. **Trust the Navigator**: Let it guide you through complex workflows
3. **Provide Context**: Mention your project stage and available artifacts
4. **Iterate Safely**: Use the navigator to experiment with different analysis approaches

### For Skill Developers
1. **Register Skills**: Ensure new skills are catalogued in the navigator's knowledge base
2. **Define Dependencies**: Clearly specify what inputs your skill requires
3. **Provide Metadata**: Include skill capabilities and use cases for intelligent matching
4. **Enable Chaining**: Design skills to work well in orchestrated workflows

### For System Administrators
1. **Monitor Usage**: Track which skill combinations are most effective
2. **Optimize Patterns**: Refine workflow templates based on user success
3. **Update Navigator**: Keep skill catalog and capabilities current
4. **Performance Tuning**: Ensure smooth orchestration without latency

## Advanced Features

### Adaptive Learning
- **Pattern Recognition**: Learns common user workflow patterns
- **Personalization**: Adapts recommendations to individual user preferences  
- **Success Metrics**: Tracks which skill combinations produce best outcomes
- **Continuous Improvement**: Refines orchestration logic over time

### Integration Hooks
- **External Tools**: Connects with VS Code extensions and external APIs
- **Custom Workflows**: Allows users to define and save custom skill sequences
- **Automation Triggers**: Enables automated skill execution based on project events
- **Quality Assurance**: Implements checkpoints and validation between skills

### Error Handling & Recovery
- **Graceful Degradation**: Continues workflow even if individual skills fail
- **Alternative Paths**: Suggests alternative approaches when preferred skills unavailable
- **Debug Assistance**: Helps troubleshoot skill execution issues
- **Rollback Support**: Enables reverting to previous state when needed

---

**Version**: 1.1.0
**Last Updated**: 2026-02-21
**Compatibility**: GitHub Copilot, VS Code, EDPS v1.x
**Maintainer**: EDPS Development Team