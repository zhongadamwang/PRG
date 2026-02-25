---
name: viewmodel-creator
description: Generates UI data models in Pages/{Feature}/_ViewModels/ with properties for data binding, display formatting, and form interaction.
---

# Viewmodel Creator

## Overview

Generates UI data models in Pages/{Feature}/_ViewModels/ with properties for data binding, display formatting, and form interaction.

## When to Use

This skill should be used when you need to create UI data models for form binding and display.

**Complexity Level:** Basic

## Prerequisites

No specific prerequisites required.

## Quick Start

```bash
# Generate viewmodel creator
bun run scripts/generate-viewmodel-creator.ts --project prg --output .
```

## Capabilities

### Core Viewmodel Creator Features

Provides comprehensive viewmodel creator generation capabilities.

## Architecture Integration

This skill belongs to the **business** layer (Business logic layer containing domain rules and application orchestration.)

**Level 2** in the skill hierarchy - Business level - domain logic and services

## Dependencies

**Used by:**
- `page-creator`
- `form-creator`
- `grid-creator`
- `validator-creator`
- `service-creator`
- `load-data-creator`
- `entity-mapping-creator`



## Examples

### Basic Usage

```bash
# Generate basic viewmodel creator
bun run scripts/generate-viewmodel-creator.ts --name MyComponent

# Generate with custom options
bun run scripts/generate-viewmodel-creator.ts --name MyComponent --advanced --template custom
```

See `references/examples.md` for complete usage examples.

## Advanced Usage

### Template Customization

Customize generation templates for project-specific needs:

```bash
# Use custom template
bun run scripts/generate-viewmodel-creator.ts --template /path/to/template

# Override specific sections
bun run scripts/generate-viewmodel-creator.ts --override section=value
```

### Batch Processing

Generate multiple items efficiently:

```bash
# Generate from configuration file
bun run scripts/generate-viewmodel-creator.ts --config batch_config.json

# Generate with name patterns
bun run scripts/generate-viewmodel-creator.ts --pattern "Feature*"
```

## Best Practices

1. **Follow Naming Conventions**: Use consistent naming patterns across all generated code
2. **Validate Dependencies**: Ensure prerequisite skills are available before generation	
3. **Test Generated Code**: Always validate generated code compiles and functions correctly
4. **Customize Templates**: Adapt templates to match project-specific requirements
5. **Document Changes**: Update skill documentation when modifying generation logic

## Resources

### Scripts
- `generate-viewmodel-creator.ts` - Main generation script
- `validate-viewmodel-creator.ts` - Validation utilities
- `test-viewmodel-creator.ts` - Test generators

### References	
- `patterns.md` - Implementation patterns and examples
- `templates.md` - Template documentation
- `integration.md` - Integration guidelines

### Assets
- `templates/` - Code templates
- `examples/` - Example implementations
