---
name: context-menu-creator
description: Generates right-click context menu components with dynamic menu items, action handling, and integration with grid and component interactions.
---

# Context Menu Creator

## Overview

Generates right-click context menu components with dynamic menu items, action handling, and integration with grid and component interactions.

## When to Use

This skill should be used when you need to create context menu creator components.

**Complexity Level:** Intermediate

## Prerequisites

The following skills should be generated first:

- component-creator

## Quick Start

```bash
# Generate context menu creator
bun run scripts/generate-context-menu-creator.ts --project prg --output .
```

## Capabilities

### Core Context Menu Creator Features

Provides comprehensive context menu creator generation capabilities.

## Architecture Integration

This skill belongs to the **presentation** layer (UI layer responsible for user interaction and display logic.)

**Level 1** in the skill hierarchy - Application level - user interface and interaction

## Dependencies

**Requires:**
- `component-creator`



## Examples

### Basic Usage

```bash
# Generate basic context menu creator
bun run scripts/generate-context-menu-creator.ts --name MyComponent

# Generate with custom options
bun run scripts/generate-context-menu-creator.ts --name MyComponent --advanced --template custom
```

See `references/examples.md` for complete usage examples.

## Advanced Usage

### Template Customization

Customize generation templates for project-specific needs:

```bash
# Use custom template
bun run scripts/generate-context-menu-creator.ts --template /path/to/template

# Override specific sections
bun run scripts/generate-context-menu-creator.ts --override section=value
```

### Batch Processing

Generate multiple items efficiently:

```bash
# Generate from configuration file
bun run scripts/generate-context-menu-creator.ts --config batch_config.json

# Generate with name patterns
bun run scripts/generate-context-menu-creator.ts --pattern "Feature*"
```

## Best Practices

1. **Follow Naming Conventions**: Use consistent naming patterns across all generated code
2. **Validate Dependencies**: Ensure prerequisite skills are available before generation	
3. **Test Generated Code**: Always validate generated code compiles and functions correctly
4. **Customize Templates**: Adapt templates to match project-specific requirements
5. **Document Changes**: Update skill documentation when modifying generation logic

## Resources

### Scripts
- `generate-context-menu-creator.ts` - Main generation script
- `validate-context-menu-creator.ts` - Validation utilities
- `test-context-menu-creator.ts` - Test generators

### References	
- `patterns.md` - Implementation patterns and examples
- `templates.md` - Template documentation
- `integration.md` - Integration guidelines

### Assets
- `templates/` - Code templates
- `examples/` - Example implementations
