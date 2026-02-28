# Enum Generator

## Description
Generates C# enum definitions from parsed domain model metadata. This skill creates strongly-typed enumerations that are referenced by entity classes, ensuring type safety and consistent naming conventions across the codebase.

## When To Use
- When domain model contains enum definitions that need to be converted to C# enums
- After running `domain-model-parser` skill and before `entity-class-generator`
- When enum definitions have been added or modified in the domain model
- As part of the complete code generation workflow for new projects

## Input
- JSON metadata file from `domain-model-parser` skill containing enum definitions
- Optional: Target directory for enum files (auto-detected if not provided)
- Optional: C# namespace for enum classes (auto-detected if not provided)

## Output
- C# enum class files (.cs) in the project's entity directory
- Proper PascalCase naming conventions (e.g., `notificationtypeenum` → `NotificationTypeEnum`) 
- Enum values with appropriate underlying types and descriptions
- Files organized in the appropriate project structure

## Usage
This skill uses bun to run TypeScript scripts for enum code generation from domain model metadata.

## Script Execution
```bash
cd .github/skills/sanjel-drb-blazor/code-generation/enum-generator/scripts
bun run generate-enums.ts <metadata-file> [output-directory] [namespace]
```

### Parameters
- `<metadata-file>`: Path to the JSON metadata file from domain-model-parser
- `[output-directory]`: Optional path where enum files will be generated (auto-detected if not provided)
- `[namespace]`: Optional C# namespace for the enum classes (auto-detected if not provided)

### Examples
```bash
# Auto-detect project structure
bun run generate-enums.ts /path/to/domain-model-metadata.json

# Specify custom output location
bun run generate-enums.ts /path/to/domain-model-metadata.json ./src/MyProject.Core/Enums MyProject.Core.Enums
```

## Generated Files
For each enum defined in the domain model, creates:
- `{EnumName}.cs` - C# enum class file
- Proper using statements and namespace declarations
- XML documentation comments for enum values
- Standard C# enum patterns and conventions

## Dependencies
- Requires Node.js modules: `node:fs`, `node:path`, `node:child_process`
- Runs with bun TypeScript runtime
- Depends on output from `domain-model-parser` skill

## Integration
- Must run after `domain-model-parser` skill
- Must run before `entity-class-generator` skill
- Generated enums are referenced by entity classes
- Integrates with project auto-detection system