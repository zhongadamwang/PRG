# Database Migration Generator

## Description
Generate EF Core database migration scripts from domain model metadata to create the initial database schema and handle subsequent schema changes.

## When To Use
- Creating initial database schema from domain model
- Generating migration scripts for database deployment
- Setting up new development environments
- Preparing database structure for CI/CD pipelines
- Converting domain model entities to database tables

## Usage
This skill uses bun to run TypeScript scripts for generating EF Core migration files.

## Input
- Entity metadata JSON file from domain-model-parser
- Optional: Migration name (defaults to "InitialMigration")
- Optional: Output directory (auto-detected if not provided)

## Output
- EF Core Migration class files (.cs)
- Database table creation scripts
- Index and constraint definitions
- Foreign key relationship configurations
- Migration metadata files

## Script Execution
```bash
bun run scripts/generate-migration.ts <metadata-file> [migration-name] [output-directory]
```

## Features
- **Complete Schema Generation**: Creates tables, columns, and relationships
- **Index Management**: Generates optimal database indexes
- **Constraint Handling**: Implements foreign keys and unique constraints
- **Data Type Mapping**: Maps C# types to appropriate SQL types
- **Migration Versioning**: Supports EF Core migration timestamp naming
- **Auto-formatting**: Automatically formats generated C# code with dotnet format

## Implementation Notes
- Follows EF Core migration patterns and conventions
- Generates both Up() and Down() methods for reversible migrations
- Handles entity relationships and navigation properties
- Supports entity inheritance and complex type configurations
- Integrates with existing project structure and namespaces