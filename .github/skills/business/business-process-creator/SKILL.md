---
name: business-process-creator
description: Creates business process classes with static methods, event handlers for data access, and complex business rule implementation.
---

# Business Process Creator

## Overview

Creates business process classes with static methods, event handlers for data access, and complex business rule implementation.

## When to Use

This skill should be used when you need to create complex business logic with event-driven architecture.

**Complexity Level:** Expert

## Prerequisites

The following skills should be generated first:

- repository-creator

## Quick Start

```bash
# Generate business process creator
bun run scripts/generate-business-process-creator.ts --project prg --output .
```

## Capabilities

### Core Business Process Creator Features

Provides comprehensive business process creator generation capabilities.

## Architecture Integration

This skill belongs to the **business** layer (Business logic layer containing domain rules and application orchestration.)

**Level 2** in the skill hierarchy - Business level - domain logic and services

## Dependencies

**Requires:**
- `repository-creator`



## Examples

### Basic Usage

```bash
# Generate basic business process creator
bun run scripts/generate-business-process-creator.ts --name MyComponent

# Generate with custom options
bun run scripts/generate-business-process-creator.ts --name MyComponent --advanced --template custom
```

See `references/examples.md` for complete usage examples.

## Advanced Usage

### Template Customization

Customize generation templates for project-specific needs:

```bash
# Use custom template
bun run scripts/generate-business-process-creator.ts --template /path/to/template

# Override specific sections
bun run scripts/generate-business-process-creator.ts --override section=value
```

### Batch Processing

Generate multiple items efficiently:

```bash
# Generate from configuration file
bun run scripts/generate-business-process-creator.ts --config batch_config.json

# Generate with name patterns
bun run scripts/generate-business-process-creator.ts --pattern "Feature*"
```

## Best Practices

1. **Follow Naming Conventions**: Use consistent naming patterns across all generated code
2. **Validate Dependencies**: Ensure prerequisite skills are available before generation	
3. **Test Generated Code**: Always validate generated code compiles and functions correctly
4. **Customize Templates**: Adapt templates to match project-specific requirements
5. **Document Changes**: Update skill documentation when modifying generation logic

## Resources

### Scripts
- `generate-business-process-creator.ts` - Main generation script
- `validate-business-process-creator.ts` - Validation utilities
- `test-business-process-creator.ts` - Test generators

### References	
- `patterns.md` - Implementation patterns and examples
- `templates.md` - Template documentation
- `integration.md` - Integration guidelines

### Assets
- `templates/` - Code templates
- `examples/` - Example implementations
