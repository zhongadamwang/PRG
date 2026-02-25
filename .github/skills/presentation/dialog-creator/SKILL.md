---
name: dialog-creator
description: Generates modal dialog components using Syncfusion SfDialog with event handling, data binding, and proper lifecycle management for popup interactions.
---

# Dialog Creator

## Overview

Generates modal dialog components using Syncfusion SfDialog with event handling, data binding, and proper lifecycle management for popup interactions.

## When to Use

This skill should be used when you need to create modal dialogs for user interactions.

**Complexity Level:** Advanced

## Prerequisites

The following skills should be generated first:

- component-creator
- form-creator

## Quick Start

```bash
# Generate dialog creator
bun run scripts/generate-dialog-creator.ts --project prg --output .
```

## Capabilities

### Core Dialog Creator Features

Provides comprehensive dialog creator generation capabilities.

## Architecture Integration

This skill belongs to the **presentation** layer (UI layer responsible for user interaction and display logic.)

**Level 1** in the skill hierarchy - Application level - user interface and interaction

## Dependencies

**Requires:**
- `component-creator`
- `form-creator`



## Examples

### Basic Usage

```bash
# Generate basic dialog creator
bun run scripts/generate-dialog-creator.ts --name MyComponent

# Generate with custom options
bun run scripts/generate-dialog-creator.ts --name MyComponent --advanced --template custom
```

See `references/examples.md` for complete usage examples.

## Advanced Usage

### Template Customization

Customize generation templates for project-specific needs:

```bash
# Use custom template
bun run scripts/generate-dialog-creator.ts --template /path/to/template

# Override specific sections
bun run scripts/generate-dialog-creator.ts --override section=value
```

### Batch Processing

Generate multiple items efficiently:

```bash
# Generate from configuration file
bun run scripts/generate-dialog-creator.ts --config batch_config.json

# Generate with name patterns
bun run scripts/generate-dialog-creator.ts --pattern "Feature*"
```

## Best Practices

1. **Follow Naming Conventions**: Use consistent naming patterns across all generated code
2. **Validate Dependencies**: Ensure prerequisite skills are available before generation	
3. **Test Generated Code**: Always validate generated code compiles and functions correctly
4. **Customize Templates**: Adapt templates to match project-specific requirements
5. **Document Changes**: Update skill documentation when modifying generation logic

## Resources

### Scripts
- `generate-dialog-creator.ts` - Main generation script
- `validate-dialog-creator.ts` - Validation utilities
- `test-dialog-creator.ts` - Test generators

### References	
- `patterns.md` - Implementation patterns and examples
- `templates.md` - Template documentation
- `integration.md` - Integration guidelines

### Assets
- `templates/` - Code templates
- `examples/` - Example implementations
