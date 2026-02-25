---
name: page-creator
description: Generates Blazor pages with Pages/{Feature}/Index.razor structure, including routing, authorization policies, lifecycle management, and code-behind separation.
---

# Page Creator

## Overview

Generates Blazor pages with Pages/{Feature}/Index.razor structure, including routing, authorization policies, lifecycle management, and code-behind separation.

## When to Use

This skill should be used when you need to create Blazor pages with proper routing and lifecycle management.

**Complexity Level:** Advanced

## Prerequisites

The following skills should be generated first:

- project-creator
- viewmodel-creator
- service-creator
- load-data-creator
- logging-creator

## Quick Start

```bash
# Generate page creator
bun run scripts/generate-page-creator.ts --project prg --output .
```

## Capabilities

### 1. Page Structure Generation
- Three-file separation (.razor, .razor.cs, .razor.css)
- Routing configuration with parameters
- Authorization policy integration

### 2. Lifecycle Management
- Proper async initialization patterns
- State management and cleanup
- Event subscription and disposal

### 3. Service Integration
- Dependency injection setup
- Error handling and loading states
- Navigation and page coordination

## Architecture Integration

This skill belongs to the **presentation** layer (UI layer responsible for user interaction and display logic.)

**Level 1** in the skill hierarchy - Application level - user interface and interaction

## Dependencies

**Requires:**
- `project-creator`
- `viewmodel-creator`
- `service-creator`
- `load-data-creator`
- `logging-creator`



## Examples

### Basic Usage

```bash
# Generate basic page creator
bun run scripts/generate-page-creator.ts --name MyComponent

# Generate with custom options
bun run scripts/generate-page-creator.ts --name MyComponent --advanced --template custom
```

See `references/examples.md` for complete usage examples.

## Advanced Usage

### Template Customization

Customize generation templates for project-specific needs:

```bash
# Use custom template
bun run scripts/generate-page-creator.ts --template /path/to/template

# Override specific sections
bun run scripts/generate-page-creator.ts --override section=value
```

### Batch Processing

Generate multiple items efficiently:

```bash
# Generate from configuration file
bun run scripts/generate-page-creator.ts --config batch_config.json

# Generate with name patterns
bun run scripts/generate-page-creator.ts --pattern "Feature*"
```

## Best Practices

1. **Follow Naming Conventions**: Use consistent naming patterns across all generated code
2. **Validate Dependencies**: Ensure prerequisite skills are available before generation	
3. **Test Generated Code**: Always validate generated code compiles and functions correctly
4. **Customize Templates**: Adapt templates to match project-specific requirements
5. **Document Changes**: Update skill documentation when modifying generation logic

## Resources

### Scripts
- `generate-page-creator.ts` - Main generation script
- `validate-page-creator.ts` - Validation utilities
- `test-page-creator.ts` - Test generators

### References	
- `patterns.md` - Implementation patterns and examples
- `templates.md` - Template documentation
- `integration.md` - Integration guidelines

### Assets
- `templates/` - Code templates
- `examples/` - Example implementations
