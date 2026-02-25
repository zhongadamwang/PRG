---
name: logging-creator
description: Generates Serilog logging configuration with structured logging, file/console sinks, enrichers, and middleware for request logging following eServiceCloud patterns.
---

# Logging Creator

## Overview

Generates Serilog logging configuration with structured logging, file/console sinks, enrichers, and middleware for request logging following eServiceCloud patterns.

## When to Use

This skill should be used when you need to create logging creator components.

**Complexity Level:** Intermediate

## Prerequisites

The following skills should be generated first:

- project-creator
- configuration-creator

## Quick Start

```bash
# Generate logging creator
bun run scripts/generate-logging-creator.ts --project prg --output .
```

## Capabilities

### Core Logging Creator Features

Provides comprehensive logging creator generation capabilities.

## Architecture Integration

This skill belongs to the **architecture** layer (Foundation layer that establishes the overall project structure and patterns.)

**Level 0** in the skill hierarchy - Foundation level - core architecture and infrastructure

## Dependencies

**Requires:**
- `project-creator`
- `configuration-creator`

**Used by:**
- `page-creator`
- `error-handler-creator`
- `service-creator`
- `repository-creator`



## Examples

### Basic Usage

```bash
# Generate basic logging creator
bun run scripts/generate-logging-creator.ts --name MyComponent

# Generate with custom options
bun run scripts/generate-logging-creator.ts --name MyComponent --advanced --template custom
```

See `references/examples.md` for complete usage examples.

## Advanced Usage

### Template Customization

Customize generation templates for project-specific needs:

```bash
# Use custom template
bun run scripts/generate-logging-creator.ts --template /path/to/template

# Override specific sections
bun run scripts/generate-logging-creator.ts --override section=value
```

### Batch Processing

Generate multiple items efficiently:

```bash
# Generate from configuration file
bun run scripts/generate-logging-creator.ts --config batch_config.json

# Generate with name patterns
bun run scripts/generate-logging-creator.ts --pattern "Feature*"
```

## Best Practices

1. **Follow Naming Conventions**: Use consistent naming patterns across all generated code
2. **Validate Dependencies**: Ensure prerequisite skills are available before generation	
3. **Test Generated Code**: Always validate generated code compiles and functions correctly
4. **Customize Templates**: Adapt templates to match project-specific requirements
5. **Document Changes**: Update skill documentation when modifying generation logic

## Resources

### Scripts
- `generate-logging-creator.ts` - Main generation script
- `validate-logging-creator.ts` - Validation utilities
- `test-logging-creator.ts` - Test generators

### References	
- `patterns.md` - Implementation patterns and examples
- `templates.md` - Template documentation
- `integration.md` - Integration guidelines

### Assets
- `templates/` - Code templates
- `examples/` - Example implementations
