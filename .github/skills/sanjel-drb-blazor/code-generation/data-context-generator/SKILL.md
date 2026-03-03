````skill
# Data Context Generator

## Description
Generate EF Core DbContext class from parsed domain model metadata. Creates a complete DbContext class with DbSet properties for all entities, configuration application, and proper setup for Entity Framework Core integration.

## When To Use
- After running `entity-class-generator` to create entity classes
- After running `entity-configuration-generator` to create Fluent API configurations
- When setting up the data access layer for a new project
- When entities have been added or modified and DbContext needs updates
- As part of the automated workflow orchestration for database layer setup
- When implementing the Repository pattern with EF Core

## Usage
This skill uses bun to run TypeScript scripts for generating EF Core DbContext classes from JSON metadata.

## Input  
- JSON metadata file path (output from domain-model-parser)
- Optional: target output directory for DbContext class
- Optional: namespace prefix (auto-detected from project structure)
- Optional: DbContext class name (defaults to {ProjectName}DbContext)

## Output
- DbContext class file (.cs) with:
  - Proper class inheritance from DbContext
  - Constructor with DbContextOptions parameter
  - DbSet<T> properties for all entity classes
  - OnModelCreating override with configuration application
  - XML documentation comments
  - Proper using statements and namespace declaration
- Generation summary report
- Validation report for generated DbContext

## Script Execution
```bash
bun run scripts/generate-data-context.ts
```

## Dependencies
- Node.js built-in modules only (fs, path, process)
- No external dependencies allowed
- Compatible with bun runtime environment
- Requires entity metadata JSON as input
- Works in conjunction with entity-class-generator and entity-configuration-generator output

## Error Handling
- Validates input JSON format and entity structure
- Reports missing or invalid entity definitions
- Handles namespace and class name conflicts gracefully
- Provides detailed error locations and suggestions
- Validates output directory permissions and structure

## Generated Code Features
- **Complete DbContext**: Full EF Core DbContext implementation
- **Entity Sets**: DbSet<T> properties for all domain entities
- **Configuration Integration**: Automatic application of entity configurations
- **Constructor Pattern**: Standard EF Core constructor with options injection
- **Namespace Management**: Proper using statements and namespace declarations
- **Documentation**: XML comments for class and properties

## Integration
- Works after `entity-class-generator` and `entity-configuration-generator`
- Integrates with dependency injection in application startup
- Compatible with `database-migration-generator` for database setup
- Supports `repository-interface-generator` and `efcore-repository-generator`
- Used by service layer generators for data access configuration

## Advanced Features
- **Auto-detection**: Automatically detects project structure and naming
- **Incremental Updates**: Supports adding new entities to existing DbContext
- **Configuration Discovery**: Finds and applies all IEntityTypeConfiguration implementations
- **Best Practices**: Follows EF Core naming conventions and patterns
````