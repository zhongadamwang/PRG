# Data Access Layer Migration Guide

## Overview

This document describes the migration of data access logic from the `Repositories` layer to the `Entities.Data` layer. The `Repositories` layer now acts as a pure adapter that delegates all operations to the `Entities.Data` layer.

## Architecture

### Entities.Data Layer (Business Logic + Data Access)

Located in `src/Sanjel.RequestManagement.Entities/Data/`, this layer contains:

- **Base Classes**:
  - `IDataAccess<TEntity>` - Base interface for all data access operations
  - `BaseDataAccess<TEntity>` - Base implementation with common CRUD operations

- **Entity-Specific Data Access**:
  - `IRequestDataAccess` / `RequestDataAccess`
  - `IStateDiagramDataAccess` / `StateDiagramDataAccess`
  - `IReviewPackageDataAccess` / `ReviewPackageDataAccess`
  - `INotificationDataAccess` / `NotificationDataAccess`
  - `IDataElementDataAccess` / `DataElementDataAccess`

- **Standard Methods**:
  - `GetPagedListAsync` - Get paginated list of entities
  - `GetListAsync` - Get list of entities with optional filter
  - `GetByIdAsync` - Get entity by ID
  - `GetByIdWithChildrenAsync` - Get entity by ID with related entities
  - `CreateAsync` - Create new entity
  - `CreateWithChildrenAsync` - Create entity with related entities
  - `UpdateAsync` - Update entity
  - `UpdateWithChildrenAsync` - Update entity with related entities
  - `DeleteAsync` - Delete entity
  - `DeleteWithChildrenAsync` - Delete entity with related entities
  - `GetListByIdsAsync` - Get entities by list of IDs
  - `GetListWithChildrenByIdsAsync` - Get entities with relations by list of IDs

### Repositories Layer (Adapter Only)

Located in `src/Sanjel.RequestManagement.Repositories/`, this layer now:

- **Acts as an adapter** that simply delegates to the Entities.Data layer
- **Provides the same interface** for external consumers
- **Contains no business logic**

- **Base Classes**:
  - `IRepository<TEntity>` - Base interface with standard methods
  - `BaseRepository<TEntity>` - Base implementation that delegates to IDataAccess

- **Entity-Specific Repositories**:
  - `IRequestRepository` / `RequestRepository`
  - `IStateDiagramRepository` / `StateDiagramRepository`
  - `IReviewPackageRepository` / `ReviewPackageRepository`
  - `INotificationRepository` / `NotificationRepository`
  - `IDataElementRepository` / `DataElementRepository`

## Migration Details

### What Was Migrated

All entity-specific query methods have been moved from Repositories to Entities.Data:

**Request → RequestDataAccess**:
- `GetByStatusAsync`
- `GetByCreatedDateRangeAsync`
- `GetByAcknowledgmentDateRangeAsync`
- `GetByCompletionDateRangeAsync`
- `GetByReviewPackageIdAsync`
- `GetByDataElementIdAsync`
- `GetByNotificationIdAsync`
- `GetPagedAsync`

**StateDiagram → StateDiagramDataAccess**:
- `GetByImportDateRangeAsync`
- `SearchByDiagramNameAsync`
- `GetByDataElementIdAsync`
- `GetByRequestIdAsync`
- `GetPagedAsync`

**ReviewPackage → ReviewPackageDataAccess**:
- `GetByReviewStatusAsync`
- `GetBySubmissionDateRangeAsync`
- `GetByReviewCompletionDateRangeAsync`
- `GetPagedAsync`

**Notification → NotificationDataAccess**:
- `GetBySentDateRangeAsync`
- `GetPagedAsync`

**DataElement → DataElementDataAccess**:
- `GetByValidationStatusAsync`
- `GetPagedAsync`

### What Changed in Repositories

**Before**:
- Repositories contained business logic and EF Core queries
- Repositories directly used `RequestManagementDbContext`
- Repository interfaces defined custom query methods

**After**:
- Repositories are simple adapters
- Repositories delegate to IDataAccess implementations
- Repository interfaces only extend `IRepository<TEntity>`
- No EF Core dependencies in repository implementations

## Usage

### For Existing Code

If your code currently uses repositories, **no changes are required**. The repository interfaces maintain the same public API.

### For New Code

When you need entity-specific queries:

1. **Use the Repository for standard operations**:
   ```csharp
   var request = await _requestRepository.GetByIdAsync(requestId);
   var pagedRequests = await _requestRepository.GetPagedListAsync(1, 20);
   ```

2. **Inject the DataAccess for entity-specific queries**:
   ```csharp
   public class MyService
   {
       private readonly IRequestDataAccess _requestDataAccess;

       public MyService(IRequestDataAccess requestDataAccess)
       {
           _requestDataAccess = requestDataAccess;
       }

       public async Task<List<Request>> GetPendingRequestsAsync()
       {
           return await _requestDataAccess.GetByStatusAsync(StatusEnum.Pending);
       }
   }
   ```

## Dependency Injection Configuration

You need to register both layers in your DI container:

```csharp
// Register Entities.Data layer (business logic)
services.AddScoped<IRequestDataAccess, RequestDataAccess>();
services.AddScoped<IStateDiagramDataAccess, StateDiagramDataAccess>();
services.AddScoped<IReviewPackageDataAccess, ReviewPackageDataAccess>();
services.AddScoped<INotificationDataAccess, NotificationDataAccess>();
services.AddScoped<IDataElementDataAccess, DataElementDataAccess>();

// Register Repositories layer (adapters)
services.AddScoped<IRequestRepository, RequestRepository>();
services.AddScoped<IStateDiagramRepository, StateDiagramRepository>();
services.AddScoped<IReviewPackageRepository, ReviewPackageRepository>();
services.AddScoped<INotificationRepository, NotificationRepository>();
services.AddScoped<IDataElementRepository, DataElementRepository>();
```

## Benefits

1. **Separation of Concerns**: Business logic is in Entities.Data, Repositories are pure adapters
2. **Flexibility**: Can swap data access implementations without affecting repository layer
3. **Testability**: Easier to mock and test data access separately from repository adapters
4. **Clean Architecture**: Follows dependency inversion and single responsibility principles
5. **Reusability**: DataAccess layer can be used independently without repository wrapper

## Future Considerations

- Consider adding specifications or query objects for complex queries
- Evaluate if Repository layer is still needed or if DataAccess can be used directly
- Consider moving caching logic to DataAccess layer
- Add unit tests for DataAccess implementations
