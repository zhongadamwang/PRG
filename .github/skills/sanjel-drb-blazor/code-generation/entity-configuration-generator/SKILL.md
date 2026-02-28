# Entity Configuration Generator

## Description
Generate EF Core Fluent API configuration classes for entity-to-database mapping. Creates EntityTypeConfiguration classes that define complex relationships, constraints, indexes, and advanced EF Core features using the Fluent API instead of Data Annotations.

## When To Use
- After running `entity-class-generator` to create basic entity classes
- When complex relationships need precise configuration beyond Data Annotations
- When database schema requires specific constraints, indexes, or custom mappings
- When implementing advanced EF Core features like owned types, value converters, or custom conventions
- As part of the automated workflow orchestration for database layer setup
- When entities have many-to-many relationships or complex inheritance hierarchies

## Usage
This skill uses bun to run TypeScript scripts for generating EF Core Fluent API configuration classes from JSON metadata.

## Input
- JSON metadata file path (output from domain-model-parser)
- Optional: target output directory for configuration classes
- Optional: namespace prefix (auto-detected from project structure)
- Optional: specific entity names to generate configurations for

## Output
- EntityTypeConfiguration class files (.cs) with:
  - Table name mapping and schema configuration
  - Property constraints and data types configuration
  - Index definitions and unique constraints
  - Relationship configurations (one-to-one, one-to-many, many-to-many)
  - Foreign key definitions and cascade behaviors
  - Advanced features (owned types, value converters, etc.)
- Configuration registration methods for DbContext
- Generation summary report

## Script Execution
```bash
bun run scripts/generate-entity-configurations.ts
```

## Dependencies
- Node.js built-in modules only (fs, path, process)
- No external dependencies allowed  
- Compatible with bun runtime environment
- Requires entity metadata JSON as input
- Works in conjunction with entity-class-generator output

## Error Handling
- Validates input JSON format and entity structure
- Reports missing or invalid relationship definitions
- Handles complex relationship mapping errors gracefully
- Provides detailed error locations and suggestions
- Validates output directory permissions and structure

## Generated Code Features
- **Fluent API Configuration**: Complete EF Core configuration using ModelBuilder
- **Relationship Mapping**: Precise one-to-many, many-to-many, and inheritance configurations
- **Constraint Definitions**: Database constraints, indexes, and validation rules
- **Performance Optimizations**: Proper indexing and query optimization configurations
- **Standards Compliance**: Follows EF Core best practices and naming conventions
- **Extensibility**: Generated configurations are easily customizable and extensible

## Integration
- Complements `entity-class-generator` for complete entity framework setup
- Integrates with `data-context-generator` for DbContext configuration registration
- Compatible with `database-migration-generator` for database schema creation
- Supports incremental updates via `model-change-detector`