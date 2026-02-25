---
name: service-creator
description: Generates application services in Pages/{Feature}/Services/ with ViewModel mapping, business orchestration, and Result<T> pattern integration.
---

# Service Creator

## Overview

Generates application services in Pages/{Feature}/Services/ with ViewModel mapping, business orchestration, and Result<T> pattern integration.

## When to Use

This skill should be used when you need to create application services that orchestrate business operations.

**Complexity Level:** Advanced

## Prerequisites

The following skills should be generated first:

- viewmodel-creator
- repository-creator
- entity-mapping-creator
- logging-creator

## Quick Start

```bash
# Generate service creator
bun run scripts/generate-service-creator.ts --project prg --output .
```

## Capabilities

### Core Service Creator Features

Provides comprehensive service creator generation capabilities.

## Architecture Integration

This skill belongs to the **business** layer (Business logic layer containing domain rules and application orchestration.)

**Level 2** in the skill hierarchy - Business level - domain logic and services

## Dependencies

**Requires:**
- `viewmodel-creator`
- `repository-creator`
- `entity-mapping-creator`
- `logging-creator`

**Used by:**
- `page-creator`
- `grid-creator`
- `load-data-creator`
- `save-data-creator`
- `mapping-service-creator`
- `cache-service-creator`



## Examples

### Basic Usage

```bash
# Generate basic service creator
bun run scripts/generate-service-creator.ts --name MyComponent

# Generate with custom options
bun run scripts/generate-service-creator.ts --name MyComponent --advanced --template custom
```

See `references/examples.md` for complete usage examples.

## Advanced Usage

### Template Customization

Customize generation templates for project-specific needs:

```bash
# Use custom template
bun run scripts/generate-service-creator.ts --template /path/to/template

# Override specific sections
bun run scripts/generate-service-creator.ts --override section=value
```

### Batch Processing

Generate multiple items efficiently:

```bash
# Generate from configuration file
bun run scripts/generate-service-creator.ts --config batch_config.json

# Generate with name patterns
bun run scripts/generate-service-creator.ts --pattern "Feature*"
```

## Best Practices

1. **Follow Naming Conventions**: Use consistent naming patterns across all generated code
2. **Validate Dependencies**: Ensure prerequisite skills are available before generation	
3. **Test Generated Code**: Always validate generated code compiles and functions correctly
4. **Customize Templates**: Adapt templates to match project-specific requirements
5. **Document Changes**: Update skill documentation when modifying generation logic

## Resources

### Scripts
- `generate-service-creator.ts` - Main generation script
- `validate-service-creator.ts` - Validation utilities
- `test-service-creator.ts` - Test generators

### References	
- `patterns.md` - Implementation patterns and examples
- `templates.md` - Template documentation
- `integration.md` - Integration guidelines

### Assets
- `templates/` - Code templates
- `examples/` - Example implementations
