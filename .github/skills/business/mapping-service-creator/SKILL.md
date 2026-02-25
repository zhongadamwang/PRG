---
name: mapping-service-creator
description: Creates mapping services for MDM collections with dependency data fetching, batch mapping, async operations, and performance optimization.
---

# Mapping Service Creator

## Overview

Creates mapping services for MDM collections with dependency data fetching, batch mapping, async operations, and performance optimization.

## When to Use

This skill should be used when you need to create mapping service creator components.

**Complexity Level:** Expert

## Prerequisites

The following skills should be generated first:

- entity-mapping-creator
- service-creator

## Quick Start

```bash
# Generate mapping service creator
bun run scripts/generate-mapping-service-creator.ts --project prg --output .
```

## Capabilities

### Core Mapping Service Creator Features

Provides comprehensive mapping service creator generation capabilities.

## Architecture Integration

This skill belongs to the **business** layer (Business logic layer containing domain rules and application orchestration.)

**Level 2** in the skill hierarchy - Business level - domain logic and services

## Dependencies

**Requires:**
- `entity-mapping-creator`
- `service-creator`



## Examples

### Basic Usage

```bash
# Generate basic mapping service creator
bun run scripts/generate-mapping-service-creator.ts --name MyComponent

# Generate with custom options
bun run scripts/generate-mapping-service-creator.ts --name MyComponent --advanced --template custom
```

See `references/examples.md` for complete usage examples.

## Advanced Usage

### Template Customization

Customize generation templates for project-specific needs:

```bash
# Use custom template
bun run scripts/generate-mapping-service-creator.ts --template /path/to/template

# Override specific sections
bun run scripts/generate-mapping-service-creator.ts --override section=value
```

### Batch Processing

Generate multiple items efficiently:

```bash
# Generate from configuration file
bun run scripts/generate-mapping-service-creator.ts --config batch_config.json

# Generate with name patterns
bun run scripts/generate-mapping-service-creator.ts --pattern "Feature*"
```

## Best Practices

1. **Follow Naming Conventions**: Use consistent naming patterns across all generated code
2. **Validate Dependencies**: Ensure prerequisite skills are available before generation	
3. **Test Generated Code**: Always validate generated code compiles and functions correctly
4. **Customize Templates**: Adapt templates to match project-specific requirements
5. **Document Changes**: Update skill documentation when modifying generation logic

## Resources

### Scripts
- `generate-mapping-service-creator.ts` - Main generation script
- `validate-mapping-service-creator.ts` - Validation utilities
- `test-mapping-service-creator.ts` - Test generators

### References	
- `patterns.md` - Implementation patterns and examples
- `templates.md` - Template documentation
- `integration.md` - Integration guidelines

### Assets
- `templates/` - Code templates
- `examples/` - Example implementations
