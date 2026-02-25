---
name: form-creator
description: Creates data entry forms with EditForm binding, FluentValidation integration, submit/cancel handling, and error display following eServiceCloud patterns.
---

# Form Creator

## Overview

Creates data entry forms with EditForm binding, FluentValidation integration, submit/cancel handling, and error display following eServiceCloud patterns.

## When to Use

This skill should be used when you need to create data entry forms with validation integration.

**Complexity Level:** Intermediate

## Prerequisites

The following skills should be generated first:

- component-creator
- viewmodel-creator
- validator-creator
- save-data-creator

## Quick Start

```bash
# Generate form creator
bun run scripts/generate-form-creator.ts --project prg --output .
```

## Capabilities

### Core Form Creator Features

Provides comprehensive form creator generation capabilities.

## Architecture Integration

This skill belongs to the **presentation** layer (UI layer responsible for user interaction and display logic.)

**Level 1** in the skill hierarchy - Application level - user interface and interaction

## Dependencies

**Requires:**
- `component-creator`
- `viewmodel-creator`
- `validator-creator`
- `save-data-creator`

**Used by:**
- `dialog-creator`



## Examples

### Basic Usage

```bash
# Generate basic form creator
bun run scripts/generate-form-creator.ts --name MyComponent

# Generate with custom options
bun run scripts/generate-form-creator.ts --name MyComponent --advanced --template custom
```

See `references/examples.md` for complete usage examples.

## Advanced Usage

### Template Customization

Customize generation templates for project-specific needs:

```bash
# Use custom template
bun run scripts/generate-form-creator.ts --template /path/to/template

# Override specific sections
bun run scripts/generate-form-creator.ts --override section=value
```

### Batch Processing

Generate multiple items efficiently:

```bash
# Generate from configuration file
bun run scripts/generate-form-creator.ts --config batch_config.json

# Generate with name patterns
bun run scripts/generate-form-creator.ts --pattern "Feature*"
```

## Best Practices

1. **Follow Naming Conventions**: Use consistent naming patterns across all generated code
2. **Validate Dependencies**: Ensure prerequisite skills are available before generation	
3. **Test Generated Code**: Always validate generated code compiles and functions correctly
4. **Customize Templates**: Adapt templates to match project-specific requirements
5. **Document Changes**: Update skill documentation when modifying generation logic

## Resources

### Scripts
- `generate-form-creator.ts` - Main generation script
- `validate-form-creator.ts` - Validation utilities
- `test-form-creator.ts` - Test generators

### References	
- `patterns.md` - Implementation patterns and examples
- `templates.md` - Template documentation
- `integration.md` - Integration guidelines

### Assets
- `templates/` - Code templates
- `examples/` - Example implementations
