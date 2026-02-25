---
name: javascript-creator
description: Creates JavaScript files for browser interop, DOM manipulation, and third-party library integration with proper JS Interop patterns.
---

# Javascript Creator

## Overview

Creates JavaScript files for browser interop, DOM manipulation, and third-party library integration with proper JS Interop patterns.

## When to Use

This skill should be used when you need to create JavaScript files for browser interop.

**Complexity Level:** Basic

## Prerequisites

No specific prerequisites required.

## Quick Start

```bash
# Generate javascript creator
bun run scripts/generate-javascript-creator.ts --project prg --output .
```

## Capabilities

### Core Javascript Creator Features

Provides comprehensive javascript creator generation capabilities.

## Architecture Integration

This skill belongs to the **presentation** layer (UI layer responsible for user interaction and display logic.)

**Level 1** in the skill hierarchy - Application level - user interface and interaction

## Dependencies

No direct dependencies.



## Examples

### Basic Usage

```bash
# Generate basic javascript creator
bun run scripts/generate-javascript-creator.ts --name MyComponent

# Generate with custom options
bun run scripts/generate-javascript-creator.ts --name MyComponent --advanced --template custom
```

See `references/examples.md` for complete usage examples.

## Advanced Usage

### Template Customization

Customize generation templates for project-specific needs:

```bash
# Use custom template
bun run scripts/generate-javascript-creator.ts --template /path/to/template

# Override specific sections
bun run scripts/generate-javascript-creator.ts --override section=value
```

### Batch Processing

Generate multiple items efficiently:

```bash
# Generate from configuration file
bun run scripts/generate-javascript-creator.ts --config batch_config.json

# Generate with name patterns
bun run scripts/generate-javascript-creator.ts --pattern "Feature*"
```

## Best Practices

1. **Follow Naming Conventions**: Use consistent naming patterns across all generated code
2. **Validate Dependencies**: Ensure prerequisite skills are available before generation	
3. **Test Generated Code**: Always validate generated code compiles and functions correctly
4. **Customize Templates**: Adapt templates to match project-specific requirements
5. **Document Changes**: Update skill documentation when modifying generation logic

## Resources

### Scripts
- `generate-javascript-creator.ts` - Main generation script
- `validate-javascript-creator.ts` - Validation utilities
- `test-javascript-creator.ts` - Test generators

### References	
- `patterns.md` - Implementation patterns and examples
- `templates.md` - Template documentation
- `integration.md` - Integration guidelines

### Assets
- `templates/` - Code templates
- `examples/` - Example implementations
