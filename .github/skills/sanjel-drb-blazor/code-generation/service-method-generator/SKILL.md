---
name: service-method-generator
description: AI-driven service method generator that analyzes Repository interfaces and generates appropriate service methods. Maps business requirements to repository capabilities, implements business logic, and provides intelligent feedback when repository methods are insufficient.
---

## Description
AI-driven service method generator that analyzes repository interfaces and generates appropriate service methods. This skill intelligently maps business requirements to repository capabilities and implements service methods with proper business logic.

## When To Use
- When a Blazor page/component needs specific data operations
- When implementing CRUD operations for entities
- When adding custom business logic methods to services
- When existing service methods need enhancement
- During iterative development of service capabilities

## Usage
This skill is **AI-Driven** and does not use scripts. The AI analyzes repository interfaces and requirements to generate appropriate service methods through intelligent consultation.

## Input
- **Required**: Service class file path (from `service-generator`)
- **Required**: Repository interface file(s) to analyze
- **Required**: Method requirements (what the service method should accomplish)
- **Optional**: Domain model documentation for business logic context
- **Optional**: ViewModel definitions (if mapping is required)
- **Optional**: Business process documentation (if business logic is involved)

## Output
- **Primary Output**: Service method implementation(s) added to the service class
- **Method Features**:
  - Proper async/await patterns with CancellationToken
  - Repository method calls with appropriate parameters
  - ViewModel-to-Model mapping (if applicable)
  - Result<T> return patterns for operations that can fail
  - XML documentation comments
- **Feedback**:
  - Analysis of available repository methods
  - Identification of repository capabilities
  - Recommendations if repository methods are insufficient
  - Suggestions for repository interface extensions

## Repository Analysis Process

### Step 1: Repository Interface Discovery
The AI will:
1. Locate and read repository interface files (I[Entity]Repository)
2. Parse all method signatures and return types
3. Identify available CRUD operations (Query, GetByIdAsync, CreateAsync, UpdateAsync, DeleteAsync, etc.)
4. Document specialized methods (e.g., FindByStatusAsync, GetByDateRangeAsync)
5. Note any business-specific query methods

### Step 2: Method Capability Mapping
For each required service method, the AI will:
1. Analyze the business requirement
2. Identify necessary repository operations
3. Map requirement to available repository methods
4. Determine if sufficient methods exist

### Step 3: Gap Analysis
If repository methods are insufficient, the AI will:
1. Identify missing capabilities
2. Suggest additional repository methods needed
3. Provide code snippets for suggested repository interface additions
4. Explain why current methods are inadequate

## Generated Method Patterns

### Pattern 1: LoadAsync (Data Retrieval)
```csharp
/// <summary>
/// Loads paginated data for [Entity] with optional filtering.
/// </summary>
/// <param name="pager">Pagination and filtering parameters.</param>
/// <param name="ct">Cancellation token.</param>
/// <returns>Paged result with [Entity] data.</returns>
public async Task<(Pager pager, IReadOnlyList<ViewModelType> list)> LoadAsync(
    Pager pager, 
    CancellationToken ct = default)
{
    var (pg, entities) = await _repository.LoadAsync(pager, ct).ConfigureAwait(false);
    var viewModels = entities.Select(e => ViewModelConverter.Model2ViewModel(e)).ToList();
    return (pg, viewModels);
}
```

### Pattern 2: GetByIdAsync (Single Entity)
```csharp
/// <summary>
/// Retrieves a single [Entity] by its identifier.
/// </summary>
/// <param name="id">Entity identifier.</param>
/// <param name="ct">Cancellation token.</param>
/// <returns>ViewModel representation of the entity.</returns>
public async Task<ViewModelType?> GetByIdAsync(int id, CancellationToken ct = default)
{
    var entity = await _repository.GetByIdAsync(id, ct).ConfigureAwait(false);
    return entity != null ? ViewModelConverter.Model2ViewModel(entity) : null;
}
```

### Pattern 3: CreateAsync (Data Insertion)
```csharp
/// <summary>
/// Creates a new [Entity] from the provided ViewModel.
/// </summary>
/// <param name="viewModel">ViewModel with entity data.</param>
/// <param name="ct">Cancellation token.</param>
/// <returns>Result indicating success or failure.</returns>
public async Task<Result<ViewModelType>> CreateAsync(
    ViewModelType viewModel, 
    CancellationToken ct = default)
{
    try
    {
        var model = ViewModelConverter.ViewModel2Model(viewModel);
        var createdEntity = await _repository.CreateAsync(model, ct).ConfigureAwait(false);
        return Result.Success(ViewModelConverter.Model2ViewModel(createdEntity));
    }
    catch (Exception ex)
    {
        // Log error here
        return Result.Failure<ViewModelType>($"Failed to create entity: {ex.Message}");
    }
}
```

### Pattern 4: UpdateAsync (Data Modification)
```csharp
/// <summary>
/// Updates an existing [Entity] from the provided ViewModel.
/// </summary>
/// <param name="viewModel">ViewModel with updated entity data.</param>
/// <param name="ct">Cancellation token.</param>
/// <returns>Result indicating success or failure.</returns>
public async Task<Result<ViewModelType>> UpdateAsync(
    ViewModelType viewModel, 
    CancellationToken ct = default)
{
    try
    {
        var model = ViewModelConverter.ViewModel2Model(viewModel);
        var updatedEntity = await _repository.UpdateAsync(model, ct).ConfigureAwait(false);
        return Result.Success(ViewModelConverter.Model2ViewModel(updatedEntity));
    }
    catch (Exception ex)
    {
        // Log error here
        return Result.Failure<ViewModelType>($"Failed to update entity: {ex.Message}");
    }
}
```

### Pattern 5: DeleteAsync (Data Deletion)
```csharp
/// <summary>
/// Deletes an [Entity] by its identifier.
/// </summary>
/// <param name="id">Entity identifier.</param>
/// <param name="ct">Cancellation token.</param>
/// <returns>Result indicating success or failure.</returns>
public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
{
    try
    {
        await _repository.DeleteAsync(id, ct).ConfigureAwait(false);
        return Result.Success();
    }
    catch (Exception ex)
    {
        // Log error here
        return Result.Failure($"Failed to delete entity: {ex.Message}");
    }
}
```

## AI-Driven Business Logic Integration

The AI can analyze domain model documentation and business requirements to implement more complex service methods:

### Example: Business Logic Method
```csharp
/// <summary>
/// Submits a [Entity] for review with validation and status updates.
/// </summary>
/// <param name="viewModel">ViewModel with submission data.</param>
/// <param name="ct">Cancellation token.</param>
/// <returns>Result indicating success or failure.</returns>
public async Task<Result<ViewModelType>> SubmitForReviewAsync(
    ViewModelType viewModel, 
    CancellationToken ct = default)
{
    // AI-generated business logic based on domain model analysis
    var entity = ViewModelConverter.ViewModel2Model(viewModel);
    
    // Validate business rules based on domain model
    if (entity.Status != EntityStatus.Draft)
    {
        return Result.Failure<ViewModelType>(
            "Only draft entities can be submitted for review.");
    }
    
    // Check required fields based on domain model requirements
    if (string.IsNullOrEmpty(entity.RequiredField))
    {
        return Result.Failure<ViewModelType>(
            "Required field must be provided before submission.");
    }
    
    entity.Status = EntityStatus.PendingReview;
    entity.SubmissionDate = DateTime.UtcNow;
    
    var updatedEntity = await _repository.UpdateAsync(entity, ct).ConfigureAwait(false);
    return Result.Success(ViewModelConverter.Model2ViewModel(updatedEntity));
}
```

## Insufficient Repository Capability Feedback

When the AI identifies that repository methods cannot fulfill requirements, it will provide structured feedback:

### Feedback Format
```markdown
## Repository Capability Analysis

### Available Methods Found:
- `GetByIdAsync(int id, CancellationToken ct = default)` - Returns Entity
- `LoadAsync(Pager pager, CancellationToken ct = default)` - Returns (Pager, IReadOnlyList<Entity>)
- `CreateAsync(Entity entity, CancellationToken ct = default)` - Returns Entity
- `UpdateAsync(Entity entity, CancellationToken ct = default)` - Returns Entity
- `DeleteAsync(int id, CancellationToken ct = default)` - Returns void

### Required Operation:
[Description of what the service method needs to accomplish]

### Capability Gap:
The current repository interface does not have a method that supports [specific requirement].

### Suggested Repository Interface Addition:
```csharp
/// <summary>
/// [Description of what the method should do]
/// </summary>
Task<[ReturnType]> [MethodName]([Parameters], CancellationToken ct = default);
```

### Recommended Action:
1. Add the suggested method to the repository interface
2. Implement the method in the repository implementation
3. Re-run this skill to generate the service method
```

## Domain Model Context Analysis

The AI will intelligently analyze available context to implement business logic:
- **Domain model documentation**: Entity definitions, relationships, enums, business rules
- **Existing code patterns**: Similar methods in other services for consistency
- **Business process documentation**: Workflow rules, state transitions, validation requirements
- **ViewModel definitions**: Mapping requirements and validation attributes

## Integration with Other Skills
- **service-generator**: Provides the service architecture shell to add methods to
- **repository-interface-generator**: Provides repository interfaces to analyze
- **efcore-repository-generator**: Provides repository implementations
- **entity-class-generator**: Provides entity type information
- **domain-model-parser**: Provides domain model metadata for business logic

## Key Principles
1. **Repository-Aware**: Only generates methods based on actual repository capabilities
2. **Gap Detection**: Identifies and reports missing repository methods
3. **AI-Driven**: Uses AI to analyze requirements and implement business logic
4. **No Assumptions**: Does not assume repository methods exist - analyzes actual code
5. **Standards Compliant**: Follows `/sanjel/eServiceCloud/docs/实现模式-008-Service.md` principles
6. **Single Responsibility**: Each generated method has one clear purpose
7. **Async-First**: All methods are async with CancellationToken support

## What This Skill DOES
- Analyze repository interfaces to understand available methods
- Generate service methods that call repository methods appropriately
- Implement ViewModel/Model mapping when needed
- Add Result<T> error handling patterns
- Implement business logic based on domain model analysis
- Provide intelligent feedback when repository methods are insufficient
- Follow Service implementation pattern standards

## What This Skill DOES NOT DO
- Does NOT modify repository interfaces (provides suggestions instead)
- Does NOT implement repository methods
- Does NOT generate new repository interface files
- Does NOT assume repository methods exist without verification
- Does NOT bypass repository to access DbContext directly

## Error Handling
- Validates that repository interfaces exist and are readable
- Checks that service class exists
- Provides detailed error messages when repository capabilities are missing
- Offers actionable suggestions for resolving capability gaps

## Dependencies
- Service class from `service-generator`
- Repository interfaces from `repository-interface-generator`
- Domain model documentation
- ViewModel definitions (for mapping scenarios)

## Best Practices
- Always analyze actual repository interface files, don't assume capabilities
- Provide clear, actionable feedback for missing methods
- Follow async/await patterns with CancellationToken
- Use Result<T> for operations that can fail
- Add XML documentation for all generated methods
- Consider business rules from domain model when implementing logic

## Example Usage Scenarios

### Scenario 1: Basic CRUD Operations
User: "Add CreateAsync, UpdateAsync, and DeleteAsync methods for Request entity"
AI: Analyzes IRequestRepository, generates three methods using available CRUD methods

### Scenario 2: Custom Query Method
User: "Add a method to load requests by status and date range"
AI: Analyzes IRequestRepository, finds LoadAsync, suggests adding FindByStatusAndDateRangeAsync

### Scenario 3: Business Logic Method
User: "Add a method to submit requests for review with validation"
AI: Analyzes domain model, implements validation rules, uses UpdateAsync for status change

## UnitTests
- Unit tests should be generated for each added service method
- Tests should verify repository method calls with correct parameters
- Tests should verify Result<T> return patterns
- Tests should validate business logic implementation
- Tests should be in the `src/[ProjectName].Blazor.Tests` project
