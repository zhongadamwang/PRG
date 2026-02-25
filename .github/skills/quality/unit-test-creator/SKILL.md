---
name: unit-test-creator
description: Generates NUnit test projects with FluentAssertions, Moq integration, test data factories, and layered testing patterns for all project components.
---

# Unit Test Creator

## Overview

Generates NUnit test projects with FluentAssertions, Moq integration, test data factories, and layered testing patterns for all project components.

## When to Use

This skill should be used when you need to create comprehensive unit tests for all layers.

**Complexity Level:** Intermediate

## Prerequisites

The following skills should be generated first:

- project-creator

## Quick Start

```bash
# Generate unit test creator
bun run scripts/generate-unit-test-creator.ts --project prg --output .
```

## Capabilities

### Core Unit Test Creator Features

Provides comprehensive unit test creator generation capabilities.

## Architecture Integration

This skill belongs to the **quality** layer (Quality assurance layer ensuring code reliability and maintainability.)

**Level 4** in the skill hierarchy - Quality level - testing and validation

## Dependencies

**Requires:**
- `project-creator`



## Examples

### Basic Usage

```bash
# Generate basic unit test creator
bun run scripts/generate-unit-test-creator.ts --name MyComponent

# Generate with custom options
bun run scripts/generate-unit-test-creator.ts --name MyComponent --advanced --template custom
```

See `references/examples.md` for complete usage examples.

## Advanced Usage

### Template Customization

Customize generation templates for project-specific needs:

```bash
# Use custom template
bun run scripts/generate-unit-test-creator.ts --template /path/to/template

# Override specific sections
bun run scripts/generate-unit-test-creator.ts --override section=value
```

### Batch Processing

Generate multiple items efficiently:

```bash
# Generate from configuration file
bun run scripts/generate-unit-test-creator.ts --config batch_config.json

# Generate with name patterns
bun run scripts/generate-unit-test-creator.ts --pattern "Feature*"
```

## Best Practices

1. **Follow Naming Conventions**: Use consistent naming patterns across all generated code
2. **Validate Dependencies**: Ensure prerequisite skills are available before generation	
3. **Test Generated Code**: Always validate generated code compiles and functions correctly
4. **Customize Templates**: Adapt templates to match project-specific requirements
5. **Document Changes**: Update skill documentation when modifying generation logic

## Resources

### Scripts
- `generate-unit-test-creator.ts` - Main generation script
- `validate-unit-test-creator.ts` - Validation utilities
- `test-unit-test-creator.ts` - Test generators

### References	
- `patterns.md` - Implementation patterns and examples
- `templates.md` - Template documentation
- `integration.md` - Integration guidelines

### Assets
- `templates/` - Code templates
- `examples/` - Example implementations
