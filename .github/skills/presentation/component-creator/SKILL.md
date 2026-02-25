---
name: component-creator
description: Creates reusable Blazor components with three-file separation (.razor/.razor.cs/.razor.css), parameter binding, event callbacks, and CSS isolation.
---

# Component Creator

## Overview

Creates reusable Blazor components with three-file separation (.razor/.razor.cs/.razor.css), parameter binding, event callbacks, and CSS isolation.

## When to Use

This skill should be used when you need to create reusable UI components with proper separation of concerns.

**Complexity Level:** Intermediate

## Prerequisites

The following skills should be generated first:

- project-creator
- css-creator

## Quick Start

```bash
# Generate component creator
bun run scripts/generate-component-creator.ts --project prg --output .
```

## Capabilities

### Core Component Creator Features

Provides comprehensive component creator generation capabilities.

## Architecture Integration

This skill belongs to the **presentation** layer (UI layer responsible for user interaction and display logic.)

**Level 1** in the skill hierarchy - Application level - user interface and interaction

## Dependencies

**Requires:**
- `project-creator`
- `css-creator`

**Used by:**
- `dialog-creator`
- `form-creator`
- `filter-creator`
- `context-menu-creator`
- `error-handler-creator`



## Examples

### Basic Usage

```bash
# Generate basic component creator
bun run scripts/generate-component-creator.ts --name MyComponent

# Generate with custom options
bun run scripts/generate-component-creator.ts --name MyComponent --advanced --template custom
```

See `references/examples.md` for complete usage examples.

## Advanced Usage

### Template Customization

Customize generation templates for project-specific needs:

```bash
# Use custom template
bun run scripts/generate-component-creator.ts --template /path/to/template

# Override specific sections
bun run scripts/generate-component-creator.ts --override section=value
```

### Batch Processing

Generate multiple items efficiently:

```bash
# Generate from configuration file
bun run scripts/generate-component-creator.ts --config batch_config.json

# Generate with name patterns
bun run scripts/generate-component-creator.ts --pattern "Feature*"
```

## Best Practices

1. **Follow Naming Conventions**: Use consistent naming patterns across all generated code
2. **Validate Dependencies**: Ensure prerequisite skills are available before generation	
3. **Test Generated Code**: Always validate generated code compiles and functions correctly
4. **Customize Templates**: Adapt templates to match project-specific requirements
5. **Document Changes**: Update skill documentation when modifying generation logic

## Resources

### Scripts
- `generate-component-creator.ts` - Main generation script
- `validate-component-creator.ts` - Validation utilities
- `test-component-creator.ts` - Test generators

### References	
- `patterns.md` - Implementation patterns and examples
- `templates.md` - Template documentation
- `integration.md` - Integration guidelines

### Assets
- `templates/` - Code templates
- `examples/` - Example implementations
