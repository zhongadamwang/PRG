---
name: project-creator
description: Creates complete .NET solution with Blazor Server, BusinessProcess, Core, and Repositories projects following clean architecture patterns. Includes project references, dependency injection setup, and basic structure.
---

# Project Creator

## Overview

Creates complete .NET solution with Blazor Server, BusinessProcess, Core, and Repositories projects following clean architecture patterns. Includes project references, dependency injection setup, and basic structure.

## When to Use

This skill should be used when you need to create a new .NET Blazor Server solution with clean architecture.

**Complexity Level:** Expert

## Prerequisites

No specific prerequisites required.

## Quick Start

```bash
# Generate project creator
bun run scripts/generate-project-creator.ts --project prg --output .
```

## Capabilities

### 1. Solution Structure Creation
- Multi-project .NET solution with proper references
- Clean architecture layers (Presentation, Business, Data)
- Test project structure with NUnit setup

### 2. Configuration Setup
- Dependency injection configuration
- Logging and error handling setup
- Development environment configuration

### 3. Blazor Server Setup
- Syncfusion component integration
- Authentication and authorization setup
- CSS and JavaScript integration

## Architecture Integration

This skill belongs to the **architecture** layer (Foundation layer that establishes the overall project structure and patterns.)

**Level 0** in the skill hierarchy - Foundation level - core architecture and infrastructure

## Dependencies

**Used by:**
- `dependency-injection-creator`
- `configuration-creator`
- `logging-creator`
- `page-creator`
- `component-creator`
- `repository-creator`
- `unit-test-creator`



## Examples

### Basic Usage

```bash
# Generate basic project creator
bun run scripts/generate-project-creator.ts --name MyComponent

# Generate with custom options
bun run scripts/generate-project-creator.ts --name MyComponent --advanced --template custom
```

See `references/examples.md` for complete usage examples.

## Advanced Usage

### Template Customization

Customize generation templates for project-specific needs:

```bash
# Use custom template
bun run scripts/generate-project-creator.ts --template /path/to/template

# Override specific sections
bun run scripts/generate-project-creator.ts --override section=value
```

### Batch Processing

Generate multiple items efficiently:

```bash
# Generate from configuration file
bun run scripts/generate-project-creator.ts --config batch_config.json

# Generate with name patterns
bun run scripts/generate-project-creator.ts --pattern "Feature*"
```

## Best Practices

1. **Follow Naming Conventions**: Use consistent naming patterns across all generated code
2. **Validate Dependencies**: Ensure prerequisite skills are available before generation	
3. **Test Generated Code**: Always validate generated code compiles and functions correctly
4. **Customize Templates**: Adapt templates to match project-specific requirements
5. **Document Changes**: Update skill documentation when modifying generation logic

## Resources

### Scripts
- `generate-project-creator.ts` - Main generation script
- `validate-project-creator.ts` - Validation utilities
- `test-project-creator.ts` - Test generators

### References	
- `patterns.md` - Implementation patterns and examples
- `templates.md` - Template documentation
- `integration.md` - Integration guidelines

### Assets
- `templates/` - Code templates
- `examples/` - Example implementations
