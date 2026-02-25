---
name: repository-creator
description: Creates data access repositories implementing IRepository<TEntity,TModel>, inheriting BaseRepository with CRUD operations and custom query methods.
---

# Repository Creator

## Overview

Creates data access repositories implementing IRepository<TEntity,TModel>, inheriting BaseRepository with CRUD operations and custom query methods.

## When to Use

This skill should be used when you need to create data access repositories following repository pattern.

**Complexity Level:** Advanced

## Prerequisites

The following skills should be generated first:

- project-creator
- entity-mapping-creator
- logging-creator

## Quick Start

```bash
# Generate repository creator
bun run scripts/generate-repository-creator.ts --project prg --output .
```

## Capabilities

### Core Repository Creator Features

Provides comprehensive repository creator generation capabilities.

## Architecture Integration

This skill belongs to the **data** layer (Data access layer providing abstraction over external systems.)

**Level 3** in the skill hierarchy - Data level - persistence and external integration

## Dependencies

**Requires:**
- `project-creator`
- `entity-mapping-creator`
- `logging-creator`

**Used by:**
- `service-creator`
- `business-process-creator`



## Examples

### Basic Usage

```bash
# Generate basic repository creator
bun run scripts/generate-repository-creator.ts --name MyComponent

# Generate with custom options
bun run scripts/generate-repository-creator.ts --name MyComponent --advanced --template custom
```

See `references/examples.md` for complete usage examples.

## Advanced Usage

### Template Customization

Customize generation templates for project-specific needs:

```bash
# Use custom template
bun run scripts/generate-repository-creator.ts --template /path/to/template

# Override specific sections
bun run scripts/generate-repository-creator.ts --override section=value
```

### Batch Processing

Generate multiple items efficiently:

```bash
# Generate from configuration file
bun run scripts/generate-repository-creator.ts --config batch_config.json

# Generate with name patterns
bun run scripts/generate-repository-creator.ts --pattern "Feature*"
```

## Best Practices

1. **Follow Naming Conventions**: Use consistent naming patterns across all generated code
2. **Validate Dependencies**: Ensure prerequisite skills are available before generation	
3. **Test Generated Code**: Always validate generated code compiles and functions correctly
4. **Customize Templates**: Adapt templates to match project-specific requirements
5. **Document Changes**: Update skill documentation when modifying generation logic

## Resources

### Scripts
- `generate-repository-creator.ts` - Main generation script
- `validate-repository-creator.ts` - Validation utilities
- `test-repository-creator.ts` - Test generators

### References	
- `patterns.md` - Implementation patterns and examples
- `templates.md` - Template documentation
- `integration.md` - Integration guidelines

### Assets
- `templates/` - Code templates
- `examples/` - Example implementations
