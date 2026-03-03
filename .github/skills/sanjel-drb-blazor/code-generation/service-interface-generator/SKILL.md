# Service Interface Generator Skill

## Overview

The `service-interface-generator` skill generates C# service interface contracts from domain model metadata. This skill creates a base IService<T> interface and entity-specific service interfaces following business layer patterns and best practices.

## Functionality

### Generated Interfaces

1. **Base Service Interface**: 
   - Generic `IService<T>` interface with business operations
   - Async/await patterns with CancellationToken support
   - CRUD operations with business validation
   - Batch operations and transaction support
   - Search and filtering capabilities

2. **Entity-Specific Service Interfaces**:
   - Interface per entity (e.g., `IRequestService`, `IDataElementService`)
   - Inherits from base `IService<T>` interface
   - Domain-specific business method signatures
   - Validation and business rule placeholders
   - Custom business operations based on entity roles

### Features

- **Auto Project Detection**: Automatically detects project root and service directory structure
- **Code Formatting**: Integrates with `dotnet format` for consistent code style
- **Metadata Parsing**: Reads domain model metadata JSON files
- **Business Patterns**: Implements service layer patterns with validation
- **Modern Async**: Full async/await support with CancellationToken for cancellation
- **Repository Integration**: Designed to work with repository pattern interfaces

## Usage

### Auto-Detection Mode (Recommended)
```bash
# Auto-detects metadata file and generates all service interfaces
cd /path/to/.github/skills/sanjel-drb-blazor/code-generation/service-interface-generator/scripts
bun run generate-service-interfaces.ts
```

### Manual Mode
```bash
# Specify metadata file and optional output directory
bun run generate-service-interfaces.ts ./path/to/domain-metadata.json [output-dir] [namespace]
```

### Parameters

- `metadata-file` (optional): Path to domain model metadata JSON file (auto-detected if not provided)
- `output-directory` (optional): Output directory for service interfaces (auto-detected if not provided) 
- `namespace` (optional): Target C# namespace (auto-detected if not provided)

### Auto-Detection Behavior

1. **Metadata File**: Searches for `domain-model-metadata.json` in orgModel directories
2. **Output Directory**: Uses `src/{ProjectName}.Core/Services/` based on project name detection
3. **Namespace**: Uses `{ProjectName}.Core.Services` pattern
4. **Project Root**: Automatically finds project root by locating .slnx files

## Generated Code Structure

### Base IService<T> Interface

```csharp
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Sanjel.RequestManagement.Core.Common;

namespace Sanjel.RequestManagement.Core.Services.Common;

/// <summary>
/// Base interface for all service classes providing business operations
/// </summary>
/// <typeparam name="T">The entity type this service manages</typeparam>
public interface IService<T> where T : class
{
    // Core CRUD Operations
    Task<T> GetByIdAsync(object id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<T>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    
    // Business Operations
    Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(object id, CancellationToken cancellationToken = default);
    
    // Validation and Business Rules
    Task<ValidationResult> ValidateAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(object id, CancellationToken cancellationToken = default);
    
    // Search Operations
    Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<int> CountAsync(Expression<Func<T, bool>> predicate = null, CancellationToken cancellationToken = default);
}
```

### Entity-Specific Service Interfaces

```csharp
using System.Threading;
using System.Threading.Tasks;
using Sanjel.RequestManagement.Core.Services.Common;
using Sanjel.RequestManagement.Core.Entities;

namespace Sanjel.RequestManagement.Core.Services;

/// <summary>
/// Service interface for Request business operations
/// </summary>
public interface IRequestService : IService<Request>
{
    // Business-specific operations for Request entity
    Task<Request> SubmitRequestAsync(Request request, CancellationToken cancellationToken = default);
    Task<Request> AssignEngineerAsync(string requestId, string engineerId, CancellationToken cancellationToken = default);
    Task<Request> UpdateStatusAsync(string requestId, StatusEnum newStatus, CancellationToken cancellationToken = default);
    Task<Request> SetPriorityAsync(string requestId, PriorityEnum priority, CancellationToken cancellationToken = default);
    
    // Validation and business rules
    Task<ValidationResult> ValidateRequestSubmissionAsync(Request request, CancellationToken cancellationToken = default);
    Task<bool> CanAssignEngineerAsync(string requestId, string engineerId, CancellationToken cancellationToken = default);
}
```

## Integration Points

### Dependencies

This skill integrates with:

- **Repository Interfaces**: Service methods call repository operations
- **Domain Entities**: Services operate on domain entity types  
- **Validation Framework**: Defines validation result contracts
- **Business Rules**: Placeholder methods for business logic implementation

### Output Locations

Generated service interfaces are placed in:
- **Base Interface**: `src/{ProjectName}.Core/Services/Common/IService.cs`
- **Entity Services**: `src/{ProjectName}.Core/Services/I{EntityName}Service.cs`
- **Validation Support**: `src/{ProjectName}.Core/Services/Common/ValidationResult.cs`

### Project Integration

The generated interfaces integrate with the project:
- Use project namespace patterns
- Reference existing entity classes
- Support dependency injection patterns
- Follow established coding standards

## Error Handling

The skill includes comprehensive error handling:

- **File System Errors**: Graceful handling of permission and path issues
- **Metadata Validation**: Validates domain model metadata structure
- **Code Generation**: Handles edge cases in entity names and attributes
- **Formatting Integration**: Continues if dotnet format fails

## Dependencies

### Required Utilities

Uses shared functions from `project-utilities` skill:
- `detectProjectInfo()` - Project root and name detection
- `constructServicePath()` - Service directory path construction  
- `findDomainModelMetadata()` - Metadata file location
- `formatGeneratedCode()` - Code formatting integration
- `toPascalCase()` - Name conversion utilities
- `ensureDirectoryExists()` - Safe directory creation

### External Dependencies

- **bun**: TypeScript runtime for execution
- **dotnet format**: Code formatting (optional enhancement)
- **Domain Model Metadata**: Requires parsed domain model JSON

## Workflow Integration

### Skill Orchestration

This skill is typically called:

1. **After Repository Generation**: Services depend on repository interfaces
2. **Before Service Implementation**: Generates contracts for implementation
3. **During Domain Model Updates**: Regenerates when entities change
4. **In Complete Generation**: Part of full project generation workflow

### Related Skills

- **`domain-model-parser`**: Provides input metadata
- **`repository-interface-generator`**: Generates dependency interfaces  
- **`service-implementation-generator`**: Implements these service interfaces
- **`entity-class-generator`**: Provides entity types used by services

## Quality Standards

### Code Quality

- Follows C# naming conventions and coding standards
- Includes comprehensive XML documentation comments
- Uses modern async/await patterns throughout
- Implements proper separation of concerns

### Performance Considerations

- Includes CancellationToken support for operation cancellation
- Supports efficient pagination with PagedResult<T>
- Uses Expression<Func<T, bool>> for efficient filtering
- Designed for lazy loading and deferred execution

### Maintainability

- Single responsibility per interface
- Clear separation between base and entity-specific operations
- Extensible design for adding business operations
- Consistent patterns across all generated interfaces

## Testing Support

Generated interfaces support testing through:
- Clear contracts for mocking and stubbing
- Async patterns compatible with testing frameworks
- Separation of business logic for unit testing
- Validation methods that can be tested independently

## Future Extensions

The skill can be extended to support:
- Custom business operation templates based on entity metadata
- Integration with validation frameworks (FluentValidation)
- Audit and logging method signatures
- Event-driven architecture patterns
- Advanced business rule definition parsing