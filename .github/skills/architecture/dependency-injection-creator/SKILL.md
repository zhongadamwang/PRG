---
name: dependency-injection-creator
description: Generates dependency injection configuration with extension methods, service registration patterns, and lifetime management (Singleton/Scoped/Transient) following eServiceCloud conventions.
---

# Dependency Injection Creator

## Overview

Generates dependency injection configuration with extension methods, service registration patterns, and lifetime management (Singleton/Scoped/Transient) following eServiceCloud conventions.

## When to Use

This skill should be used when you need to create dependency injection creator components.

**Complexity Level:** Expert

## Prerequisites

The following skills should be generated first:

- project-creator

## Quick Start

```bash
# Generate dependency injection creator
bun run scripts/generate-dependency-injection-creator.ts --project prg --output .
```

## Capabilities

### Core Dependency Injection Creator Features

Provides comprehensive dependency injection creator generation capabilities.

## Architecture Integration

This skill belongs to the **architecture** layer (Foundation layer that establishes the overall project structure and patterns.)

**Level 0** in the skill hierarchy - Foundation level - core architecture and infrastructure

## Dependencies

**Requires:**
- `project-creator`



## Examples

### Basic Usage

```bash
# Generate basic dependency injection creator
bun run scripts/generate-dependency-injection-creator.ts --name MyComponent

# Generate with custom options
bun run scripts/generate-dependency-injection-creator.ts --name MyComponent --advanced --template custom
```

See `references/examples.md` for complete usage examples.

## Advanced Usage

### Template Customization

Customize generation templates for project-specific needs:

```bash
# Use custom template
bun run scripts/generate-dependency-injection-creator.ts --template /path/to/template

# Override specific sections
bun run scripts/generate-dependency-injection-creator.ts --override section=value
```

### Batch Processing

Generate multiple items efficiently:

```bash
# Generate from configuration file
bun run scripts/generate-dependency-injection-creator.ts --config batch_config.json

# Generate with name patterns
bun run scripts/generate-dependency-injection-creator.ts --pattern "Feature*"
```

## Best Practices

1. **Follow Naming Conventions**: Use consistent naming patterns across all generated code
2. **Validate Dependencies**: Ensure prerequisite skills are available before generation	
3. **Test Generated Code**: Always validate generated code compiles and functions correctly
4. **Customize Templates**: Adapt templates to match project-specific requirements
5. **Document Changes**: Update skill documentation when modifying generation logic

## Resources

### Scripts
- `generate-dependency-injection-creator.ts` - Main generation script
- `validate-dependency-injection-creator.ts` - Validation utilities
- `test-dependency-injection-creator.ts` - Test generators

### References	
- `patterns.md` - Implementation patterns and examples
- `templates.md` - Template documentation
- `integration.md` - Integration guidelines

### Assets
- `templates/` - Code templates
- `examples/` - Example implementations
