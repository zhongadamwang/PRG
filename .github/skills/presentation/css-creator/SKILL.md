---
name: css-creator
description: Generates CSS files with Blazor CSS isolation (.razor.css), CSS custom properties for theming, BEM naming conventions, and Syncfusion component overrides.
---

# Css Creator

## Overview

Generates CSS files with Blazor CSS isolation (.razor.css), CSS custom properties for theming, BEM naming conventions, and Syncfusion component overrides.

## When to Use

This skill should be used when you need to create CSS stylesheets with proper isolation and theming.

**Complexity Level:** Basic

## Prerequisites

No specific prerequisites required.

## Quick Start

```bash
# Generate css creator
bun run scripts/generate-css-creator.ts --project prg --output .
```

## Capabilities

### Core Css Creator Features

Provides comprehensive css creator generation capabilities.

## Architecture Integration

This skill belongs to the **presentation** layer (UI layer responsible for user interaction and display logic.)

**Level 1** in the skill hierarchy - Application level - user interface and interaction

## Dependencies

**Used by:**
- `component-creator`



## Examples

### Basic Usage

```bash
# Generate basic css creator
bun run scripts/generate-css-creator.ts --name MyComponent

# Generate with custom options
bun run scripts/generate-css-creator.ts --name MyComponent --advanced --template custom
```

See `references/examples.md` for complete usage examples.

## Advanced Usage

### Template Customization

Customize generation templates for project-specific needs:

```bash
# Use custom template
bun run scripts/generate-css-creator.ts --template /path/to/template

# Override specific sections
bun run scripts/generate-css-creator.ts --override section=value
```

### Batch Processing

Generate multiple items efficiently:

```bash
# Generate from configuration file
bun run scripts/generate-css-creator.ts --config batch_config.json

# Generate with name patterns
bun run scripts/generate-css-creator.ts --pattern "Feature*"
```

## Best Practices

1. **Follow Naming Conventions**: Use consistent naming patterns across all generated code
2. **Validate Dependencies**: Ensure prerequisite skills are available before generation	
3. **Test Generated Code**: Always validate generated code compiles and functions correctly
4. **Customize Templates**: Adapt templates to match project-specific requirements
5. **Document Changes**: Update skill documentation when modifying generation logic

## Resources

### Scripts
- `generate-css-creator.ts` - Main generation script
- `validate-css-creator.ts` - Validation utilities
- `test-css-creator.ts` - Test generators

### References	
- `patterns.md` - Implementation patterns and examples
- `templates.md` - Template documentation
- `integration.md` - Integration guidelines

### Assets
- `templates/` - Code templates
- `examples/` - Example implementations
