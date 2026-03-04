# Entity Class Generator

## Description
Generate basic C# entity classes from parsed domain model metadata. Creates clean, well-structured entity classes with proper Data Annotations, type mappings, and navigation properties for EF Core compatibility.

## When To Use
- After running `domain-model-parser` to extract structured metadata
- When starting a new project and need to create entity classes from domain models
- When domain model entities have been updated and need code regeneration
- As part of the automated workflow orchestration for new project setup
- When implementing EF Core entity framework structure
- Before running repository and service generation skills

## Usage
This skill uses bun to run TypeScript scripts for generating C# entity classes from JSON metadata.

## Input
- JSON metadata file path (output from domain-model-parser)
- Target output directory for generated entity classes
- Optional: namespace prefix (defaults to project namespace)
- Optional: specific entity names to generate (generates all if not specified)

## Output
- C# entity class files (.cs) with:
  - Proper class structure and naming conventions
  - Data Annotations ([Key], [Required], [MaxLength], etc.)
  - Correct C# type mappings (string, int, DateTime, bool, etc.)
  - Simple navigation properties for relationships
  - XML documentation comments
  - EF Core compatibility attributes
- Generation summary report
- Validation report for generated classes

## Script Execution
```bash
bun run scripts/generate-entities.ts
```

## Dependencies
- Node.js built-in modules only (fs, path, process)
- No external dependencies allowed  
- Compatible with bun runtime environment
- Requires domain model JSON metadata as input

## Error Handling
- Validates input JSON format and structure
- Reports missing or invalid entity definitions
- Handles type mapping errors gracefully
- Provides detailed error locations and suggestions
- Validates output directory permissions

## Generated Code Features
- **Type Safety**: Proper C# type mappings from domain model types
- **Data Annotations**: Automatic generation of validation attributes
- **Navigation Properties**: Simple properties for entity relationships
- **Naming Conventions**: PascalCase class and property names
- **Documentation**: XML comments for classes and properties
- **EF Core Ready**: Compatible with Entity Framework Core conventions

## Integration
- Works with output from `domain-model-parser` skill
- Integrates with `entity-configuration-generator` for Fluent API configs
- Compatible with `data-context-generator` for DbContext generation
- Supports incremental updates via `model-change-detector`

## UnitTests

- Should add unit tests for all the entities in Core.Tests project after Entity classes are generated. This will ensure that the generated entity classes are correct and can be used in the application without any issues.
