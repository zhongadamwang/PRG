---
name: save-data-creator
description: Creates data saving mechanisms with form submission handling, validation, async operations, success/error feedback, and transaction management.
---

# Save Data Creator

## Overview

Creates data saving mechanisms with form submission handling, validation, async operations, success/error feedback, and transaction management.

## When to Use

This skill should be used when you need to create save data creator components.

**Complexity Level:** Advanced

## Prerequisites

The following skills should be generated first:

- service-creator
- validator-creator

## Quick Start

```bash
# Generate save data creator
bun run scripts/generate-save-data-creator.ts --project prg --output .
```

## Capabilities

### Core Save Data Creator Features

Provides comprehensive save data creator generation capabilities.

## Architecture Integration

This skill belongs to the **business** layer (Business logic layer containing domain rules and application orchestration.)

**Level 2** in the skill hierarchy - Business level - domain logic and services

## Dependencies

**Requires:**
- `service-creator`
- `validator-creator`

**Used by:**
- `form-creator`



## Examples

### Basic Usage

```bash
# Generate basic save data creator
bun run scripts/generate-save-data-creator.ts --name MyComponent

# Generate with custom options
bun run scripts/generate-save-data-creator.ts --name MyComponent --advanced --template custom
```

See `references/examples.md` for complete usage examples.

## Advanced Usage

### Template Customization

Customize generation templates for project-specific needs:

```bash
# Use custom template
bun run scripts/generate-save-data-creator.ts --template /path/to/template

# Override specific sections
bun run scripts/generate-save-data-creator.ts --override section=value
```

### Batch Processing

Generate multiple items efficiently:

```bash
# Generate from configuration file
bun run scripts/generate-save-data-creator.ts --config batch_config.json

# Generate with name patterns
bun run scripts/generate-save-data-creator.ts --pattern "Feature*"
```

## Best Practices

1. **Follow Naming Conventions**: Use consistent naming patterns across all generated code
2. **Validate Dependencies**: Ensure prerequisite skills are available before generation	
3. **Test Generated Code**: Always validate generated code compiles and functions correctly
4. **Customize Templates**: Adapt templates to match project-specific requirements
5. **Document Changes**: Update skill documentation when modifying generation logic

## Resources

### Scripts
- `generate-save-data-creator.ts` - Main generation script
- `validate-save-data-creator.ts` - Validation utilities
- `test-save-data-creator.ts` - Test generators

### References	
- `patterns.md` - Implementation patterns and examples
- `templates.md` - Template documentation
- `integration.md` - Integration guidelines

### Assets
- `templates/` - Code templates
- `examples/` - Example implementations
