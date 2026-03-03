# Repository Interface Generator Skill

## Overview

The `repository-interface-generator` skill generates C# repository interface contracts from domain model metadata. This skill creates a base IRepository<T> interface and entity-specific repository interfaces following the established project patterns.

## Functionality

### Generated Interfaces

1. **Base Repository Interface**: 
   - Generic `IRepository<T>` interface with CRUD operations
   - Async/await patterns with Task return types  
   - Paging support with PagerResult<T>
   - Expression-based filtering
   - Batch operations for performance optimization

2. **Entity-Specific Repository Interfaces**:
   - Interface per entity (e.g., `IRequestRepository`, `IDataElementRepository`)
   - Inherits from base `IRepository<T>` interface
   - Domain-specific method signatures
   - Performance annotations for Dapper optimization

### Features

- **Auto Project Detection**: Automatically detects project root and repository directory structure
- **Code Formatting**: Integrates with `dotnet format` for consistent code style
- **Metadata Parsing**: Reads domain model metadata JSON files
- **Performance Optimization**: Adds markers for Dapper-optimized implementations
- **Repository Pattern**: Follows established repository pattern conventions

## Usage

### Auto-Detection Mode (Recommended)
```bash
# Auto-detects metadata file and generates all repository interfaces
cd /path/to/.github/skills/sanjel-drb-blazor/code-generation/repository-interface-generator/scripts
bun run generate-repository-interfaces.ts
```

### Manual Mode
```bash
# Specify metadata file and optional output directory
bun run generate-repository-interfaces.ts ./path/to/domain-metadata.json [output-dir] [namespace]
```

### Parameters

- `metadata-file` (optional): Path to domain model metadata JSON file (auto-detected if not provided)
- `output-directory` (optional): Output directory for repository interfaces (auto-detected if not provided) 
- `namespace` (optional): C# namespace for generated interfaces (auto-detected if not provided)

## Output Structure

Generated repository interfaces are placed in the project's Repository directory:
```
src/ProjectName.Repositories/
├── Common/
│   └── IRepository.cs           # Base repository interface
├── IRequestRepository.cs        # Entity-specific repository interface
├── IDataElementRepository.cs    # Entity-specific repository interface  
└── ...                          # Other entity repository interfaces
```

## Integration

This skill integrates with:

- **Shared Utilities**: Uses project-utilities for project detection and formatting
- **Domain Model Metadata**: Reads JSON output from domain-model-parser skill
- **EF Core/Dapper**: Generated interfaces support both EF Core and Dapper implementations
- **VS Code**: Automatic code formatting through dotnet format integration

## Dependencies

- **Bun Runtime**: TypeScript execution environment
- **Domain Model Metadata**: JSON file from domain-model-parser skill  
- **Project Utilities**: Shared utility functions for project operations
- **Dotnet SDK**: For code formatting (dotnet format command)

## Generated Interface Pattern

### Base Repository Interface
```csharp
public interface IRepository<TEntity> 
    where TEntity : Common, new()
{
    Task<PagerResult<TEntity>> GetPagedListAsync(Pager pager, Expression<Func<TEntity, bool>> expression);
    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression);
    Task<TEntity> GetByIdAsync(int id);
    Task<TEntity> GetByIdWithChildrenAsync(int id);
    Task<bool> CreateAsync(TEntity entity);
    Task<bool> UpdateAsync(TEntity entity);
    Task<bool> DeleteAsync(TEntity entity);
    // ... additional methods
}
```

### Entity-Specific Repository Interface  
```csharp
public interface IRequestRepository : IRepository<Request>
{
    // Entity-specific operations can be added here
    // Performance markers for Dapper optimization
    [DapperOptimized]
    Task<List<Request>> GetRequestsByStatusAsync(Status status);
}
```

## Architecture Notes

- **Repository Pattern**: Follows clean architecture repository pattern
- **Dependency Injection**: Generated interfaces support DI containers  
- **Performance Markers**: Attributes indicate operations suitable for Dapper optimization
- **Async Operations**: All methods use async/await patterns for scalability
- **Expression Trees**: LINQ expressions enable flexible filtering

## Skill Communication

This skill communicates through GitHub Copilot orchestration:
- No direct exports or imports between skills
- Input: Domain model metadata JSON files
- Output: Generated C# repository interface files
- Integration: VS Code workspace file operations

This skill is part of the comprehensive code generation pipeline and works in coordination with other domain-driven code generation skills.