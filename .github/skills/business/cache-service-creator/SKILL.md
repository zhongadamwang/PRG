---
name: cache-service-creator
description: Generates caching service implementations with memory and distributed cache support, cache-aside patterns, and invalidation strategies.
---

# Cache Service Creator

## Overview

Generates caching service implementations with memory and distributed cache support, cache-aside patterns, and invalidation strategies.

## When to Use

This skill should be used when you need to create cache service creator components.

**Complexity Level:** Intermediate

## Prerequisites

The following skills should be generated first:

- service-creator

## Quick Start

```bash
# Generate cache service creator
bun run scripts/generate-cache-service-creator.ts --project prg --output .
```

## Capabilities

### Core Cache Service Creator Features

Provides comprehensive cache service creator generation capabilities.

## Architecture Integration

This skill belongs to the **business** layer (Business logic layer containing domain rules and application orchestration.)

**Level 2** in the skill hierarchy - Business level - domain logic and services

## Dependencies

**Requires:**
- `service-creator`



## Examples

### Basic Usage

```bash
# Generate basic cache service creator
bun run scripts/generate-cache-service-creator.ts --name MyComponent

# Generate with custom options
bun run scripts/generate-cache-service-creator.ts --name MyComponent --advanced --template custom
```

See `references/examples.md` for complete usage examples.

## Advanced Usage

### Template Customization

Customize generation templates for project-specific needs:

```bash
# Use custom template
bun run scripts/generate-cache-service-creator.ts --template /path/to/template

# Override specific sections
bun run scripts/generate-cache-service-creator.ts --override section=value
```

### Batch Processing

Generate multiple items efficiently:

```bash
# Generate from configuration file
bun run scripts/generate-cache-service-creator.ts --config batch_config.json

# Generate with name patterns
bun run scripts/generate-cache-service-creator.ts --pattern "Feature*"
```

## Best Practices

1. **Follow Naming Conventions**: Use consistent naming patterns across all generated code
2. **Validate Dependencies**: Ensure prerequisite skills are available before generation	
3. **Test Generated Code**: Always validate generated code compiles and functions correctly
4. **Customize Templates**: Adapt templates to match project-specific requirements
5. **Document Changes**: Update skill documentation when modifying generation logic

## Resources

### Scripts
- `generate-cache-service-creator.ts` - Main generation script
- `validate-cache-service-creator.ts` - Validation utilities
- `test-cache-service-creator.ts` - Test generators

### References	
- `patterns.md` - Implementation patterns and examples
- `templates.md` - Template documentation
- `integration.md` - Integration guidelines

### Assets
- `templates/` - Code templates
- `examples/` - Example implementations
