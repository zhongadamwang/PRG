---
name: load-data-creator
description: Generates data loading components with async data fetching, parameter handling, loading states, and integration with ViewModels and services.
---

# Load Data Creator

## Overview

Generates data loading components with async data fetching, parameter handling, loading states, and integration with ViewModels and services.

## When to Use

This skill should be used when you need to create load data creator components.

**Complexity Level:** Advanced

## Prerequisites

The following skills should be generated first:

- service-creator
- viewmodel-creator

## Quick Start

```bash
# Generate load data creator
bun run scripts/generate-load-data-creator.ts --project prg --output .
```

## Capabilities

### Core Load Data Creator Features

Provides comprehensive load data creator generation capabilities.

## Architecture Integration

This skill belongs to the **business** layer (Business logic layer containing domain rules and application orchestration.)

**Level 2** in the skill hierarchy - Business level - domain logic and services

## Dependencies

**Requires:**
- `service-creator`
- `viewmodel-creator`

**Used by:**
- `page-creator`
- `grid-creator`



## Examples

### Basic Usage

```bash
# Generate basic load data creator
bun run scripts/generate-load-data-creator.ts --name MyComponent

# Generate with custom options
bun run scripts/generate-load-data-creator.ts --name MyComponent --advanced --template custom
```

See `references/examples.md` for complete usage examples.

## Advanced Usage

### Template Customization

Customize generation templates for project-specific needs:

```bash
# Use custom template
bun run scripts/generate-load-data-creator.ts --template /path/to/template

# Override specific sections
bun run scripts/generate-load-data-creator.ts --override section=value
```

### Batch Processing

Generate multiple items efficiently:

```bash
# Generate from configuration file
bun run scripts/generate-load-data-creator.ts --config batch_config.json

# Generate with name patterns
bun run scripts/generate-load-data-creator.ts --pattern "Feature*"
```

## Best Practices

1. **Follow Naming Conventions**: Use consistent naming patterns across all generated code
2. **Validate Dependencies**: Ensure prerequisite skills are available before generation	
3. **Test Generated Code**: Always validate generated code compiles and functions correctly
4. **Customize Templates**: Adapt templates to match project-specific requirements
5. **Document Changes**: Update skill documentation when modifying generation logic

## Resources

### Scripts
- `generate-load-data-creator.ts` - Main generation script
- `validate-load-data-creator.ts` - Validation utilities
- `test-load-data-creator.ts` - Test generators

### References	
- `patterns.md` - Implementation patterns and examples
- `templates.md` - Template documentation
- `integration.md` - Integration guidelines

### Assets
- `templates/` - Code templates
- `examples/` - Example implementations
