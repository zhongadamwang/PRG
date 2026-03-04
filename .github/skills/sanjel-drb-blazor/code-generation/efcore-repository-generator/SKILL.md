# EF Core Repository Generator

## Description
Generates EF Core Repository implementation classes based on entity metadata and repository interface contracts. Creates concrete repository classes that inherit from BaseRepository<T> and implement entity-specific interfaces using modern EF Core patterns.

## When To Use
- After generating repository interfaces with `repository-interface-generator`
- When implementing data access layer for domain entities
- During initial project setup or when adding new entities
- When migrating from legacy data access patterns to modern EF Core

## Usage
This skill uses bun to run TypeScript scripts for EF Core repository implementation generation.

## Input
- **Required**: Entity metadata JSON (from `domain-model-parser`)
- **Required**: Repository interface files (from `repository-interface-generator`)
- **Optional**: Output directory path (auto-detected if not provided)
- **Optional**: Namespace (auto-detected if not provided)

## Output
- EF Core Repository implementation classes (.cs files)
- Concrete implementations of all interface methods
- Proper dependency injection patterns
- LINQ-based query implementations
- Transaction management support
- Error handling patterns

## Generated Features
- **BaseRepository Inheritance**: All repositories inherit from `BaseRepository<TEntity>`
- **Interface Implementation**: Implements corresponding `I{Entity}Repository` interface
- **Constructor Injection**: Accepts `RequestManagementDbContext` via dependency injection
- **Entity-Specific Methods**: Status queries, date range queries, text searches
- **Pagination Support**: Implements pagination with `PagedResult<T>`
- **Async/Await Patterns**: All methods use modern async patterns with CancellationToken
- **LINQ Queries**: Uses EF Core LINQ for database operations
- **Performance Optimization**: Includes query optimization hints

## Dependencies
- Domain model metadata from `domain-model-parser`
- Repository interfaces from `repository-interface-generator`
- BaseRepository and IRepository from Common folder
- EF Core DbContext implementation

## Script Execution
```bash
bun run scripts/generate-efcore-repositories.ts
```

## Examples

### Auto-Detection (Recommended)
```bash
bun run scripts/generate-efcore-repositories.ts
```

### With Custom Parameters
```bash
bun run scripts/generate-efcore-repositories.ts /path/to/metadata.json /output/directory Namespace.Name
```

## Output Location
- **Auto-detected**: `src/Sanjel.RequestManagement.Repositories/`
- **Custom**: User-specified directory

## Generated File Pattern
- `{EntityName}Repository.cs` - Implementation class
- Implements: `I{EntityName}Repository` interface
- Inherits: `BaseRepository<{EntityName}>`

## Integration
- Works with existing EF Core DbContext
- Uses shared utilities from `project-utilities`
- Includes automatic code formatting with `dotnet format`
- Follows established project conventions


## UnitTests

- Should add unit tests for all the entities in Repositories.Tests project after Repository classes are generated. This will ensure that the generated repository classes and interfaces are correct and can be used in the application without any issues.
