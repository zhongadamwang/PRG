---
name: grid-creator
description: Generates Syncfusion SfGrid components with columns, sorting, filtering, paging, and master-detail relationships for data display and manipulation.
---

# Grid Creator

## Overview

Generates Syncfusion SfGrid components with columns, sorting, filtering, paging, and master-detail relationships for data display and manipulation.

## When to Use

This skill should be used when you need to create data grids with advanced features like filtering and sorting.

**Complexity Level:** Advanced

## Prerequisites

The following skills should be generated first:

- viewmodel-creator
- service-creator
- load-data-creator

## Quick Start

```bash
# Generate grid creator
bun run scripts/generate-grid-creator.ts --project prg --output .
```

## Capabilities

### Core Grid Creator Features

Provides comprehensive grid creator generation capabilities.

## Architecture Integration

This skill belongs to the **presentation** layer (UI layer responsible for user interaction and display logic.)

**Level 1** in the skill hierarchy - Application level - user interface and interaction

## Dependencies

**Requires:**
- `viewmodel-creator`
- `service-creator`
- `load-data-creator`

**Used by:**
- `filter-creator`



## Examples

### Basic Usage

```bash
# Generate basic grid creator
bun run scripts/generate-grid-creator.ts --name MyComponent

# Generate with custom options
bun run scripts/generate-grid-creator.ts --name MyComponent --advanced --template custom
```

See `references/examples.md` for complete usage examples.

## Advanced Usage

### Template Customization

Customize generation templates for project-specific needs:

```bash
# Use custom template
bun run scripts/generate-grid-creator.ts --template /path/to/template

# Override specific sections
bun run scripts/generate-grid-creator.ts --override section=value
```

### Batch Processing

Generate multiple items efficiently:

```bash
# Generate from configuration file
bun run scripts/generate-grid-creator.ts --config batch_config.json

# Generate with name patterns
bun run scripts/generate-grid-creator.ts --pattern "Feature*"
```

## Best Practices

1. **Follow Naming Conventions**: Use consistent naming patterns across all generated code
2. **Validate Dependencies**: Ensure prerequisite skills are available before generation	
3. **Test Generated Code**: Always validate generated code compiles and functions correctly
4. **Customize Templates**: Adapt templates to match project-specific requirements
5. **Document Changes**: Update skill documentation when modifying generation logic

## Resources

### Scripts
- `generate-grid-creator.ts` - Main generation script
- `validate-grid-creator.ts` - Validation utilities
- `test-grid-creator.ts` - Test generators

### References	
- `patterns.md` - Implementation patterns and examples
- `templates.md` - Template documentation
- `integration.md` - Integration guidelines

### Assets
- `templates/` - Code templates
- `examples/` - Example implementations
