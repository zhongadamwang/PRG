---
name: repository-mock-data-generator
description: AI-driven consultant for generating realistic mock data and mock repository implementations. Provides intelligent guidance on creating diverse, business-realistic test data that supports development and testing workflows without requiring database dependencies.
license: MIT
---

# Repository Mock Data Generator

## Description

AI-driven consultant for generating realistic mock data and mock repository implementations. Provides intelligent guidance on creating diverse, business-realistic test data that supports development and testing workflows without requiring database dependencies.

## When To Use

- **Development Environment Setup**: When setting up development environment without database connection
- **Unit Testing**: When creating unit tests with realistic test data
- **Integration Testing**: When conducting integration tests without database dependencies
- **UI/UX Prototyping**: When building UI/UX prototypes with sample data
- **Performance Testing**: When conducting performance testing with configurable datasets
- **Repository Testing**: When testing repository pattern implementations with in-memory data

## Usage

This skill uses AI-driven consultation to guide you through creating realistic mock data and mock repository implementations. The AI will ask clarifying questions about your entity structure and recommend optimal mock data strategies.

## Input

**Required:**
- Entity metadata (entity class file or domain model parser output)
- Repository interface to implement (e.g., `IRepository<Request>`)

**AI-Consulted Questions:**
- **Entity Selection**: Which entity needs mock data generation?
  - Entity name and class location
  - Key properties and their data types
  - Enum definitions and valid values
  - Foreign key relationships to other entities
- **Data Volume Requirements**: How many mock records do you need?
  - Small: 10-20 records (quick testing)
  - Medium: 50-100 records (typical development)
  - Large: 500-1000+ records (performance testing)
- **Use Case**: What is the primary use case?
  - Development environment seeding
  - Unit testing
  - Integration testing
  - UI/UX prototyping
  - Performance testing
- **Business Context**: What business rules should mock data reflect?
  - Status distribution (e.g., more Draft than Cancelled)
  - Date ranges (e.g., last 6 months, next year)
  - Value ranges (e.g., budget amounts, priorities)
  - Relationship requirements (e.g., parent-child relationships)

## Output

**Generated Artifacts:**
- MockDataGenerator class with entity-specific generation methods
- Mock repository implementation (e.g., `MockRequestRepository`)
- Sample realistic mock data with proper relationships
- DI registration recommendations for development/testing
- Data configuration documentation

**Consultation Deliverables:**
- Mock data strategy recommendations
- Realistic value generation patterns
- Enum distribution and weighting strategies
- Relationship handling and referential integrity guidance
- Performance optimization for large datasets

## Mock Data Generation Principles

### Realistic Over Random

**❌ Avoid**: "Test1", "Test2", "Test3" or random dates across entire century

**✅ Use**: Business-realistic values with appropriate context

```csharp
// Realistic business strings
private static string GenerateRequestName()
{
    var prefixes = new[] { "System", "Data", "Infrastructure", "Cloud" };
    var suffixes = new[] { "Upgrade", "Migration", "Deployment", "Optimization" };
    var year = random.Next(2024, 2027);
    var number = random.Next(100, 999);
    return $"{prefixes[random.Next(prefixes.Length)]} {suffixes[random.Next(suffixes.Length)]} {year}-{number}";
}

// Business-appropriate date ranges
private static DateTime GenerateRequestDate(RequestStatus status)
{
    var now = DateTime.Now;
    return status switch
    {
        RequestStatus.Draft => now.AddDays(-random.Next(1, 30)),
        RequestStatus.Approved => now.AddDays(-random.Next(7, 365)),
        _ => now.AddDays(-random.Next(1, 30))
    };
}
```

### Weighted Enum Distribution

**❌ Avoid**: Equal probability for all enum values

**✅ Use**: Weighted distribution based on business reality

```csharp
private static RequestStatus GetRandomRequestStatus()
{
    var weightedStatuses = new List<(RequestStatus Status, double Weight)>
    {
        (RequestStatus.Draft, 30),
        (RequestStatus.Submitted, 25),
        (RequestStatus.Approved, 30),
        (RequestStatus.Rejected, 10),
        (RequestStatus.Cancelled, 5)
    };

    var totalWeight = weightedStatuses.Sum(w => w.Weight);
    var randomValue = random.NextDouble() * totalWeight;
    var cumulativeWeight = 0.0;

    foreach (var (status, weight) in weightedStatuses)
    {
        cumulativeWeight += weight;
        if (randomValue <= cumulativeWeight) return status;
    }

    return RequestStatus.Draft;
}
```

## Mock Repository Implementation

### Standard Template

```csharp
public class MockRequestRepository : IRepository<Request>
{
    private readonly List<Request> _data;
    private static readonly Random _random = new Random(42);

    public MockRequestRepository(int count = 100)
    {
        _data = MockDataGenerator.GenerateRequests(count).ToList();
    }

    public async Task<Request?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        await Task.CompletedTask;
        return _data.FirstOrDefault(e => e.Id == id);
    }

    public async Task<IEnumerable<Request>> GetAllAsync(CancellationToken ct = default)
    {
        await Task.CompletedTask;
        return _data.AsReadOnly();
    }

    public async Task<PagedResult<Request>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default)
    {
        await Task.CompletedTask;
        var query = _data.AsQueryable();
        var totalCount = query.Count();
        var items = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return new PagedResult<Request>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        };
    }

    public async Task<IQueryable<Request>> QueryAsync(CancellationToken ct = default)
    {
        await Task.CompletedTask;
        return _data.AsQueryable();
    }

    public async Task<Request> CreateAsync(Request entity, CancellationToken ct = default)
    {
        entity.Id = Guid.NewGuid();
        entity.CreatedAt = DateTime.Now;
        _data.Add(entity);
        await Task.CompletedTask;
        return entity;
    }

    public async Task UpdateAsync(Request entity, CancellationToken ct = default)
    {
        var existing = _data.FirstOrDefault(e => e.Id == entity.Id);
        if (existing != null)
        {
            var index = _data.IndexOf(existing);
            entity.UpdatedAt = DateTime.Now;
            _data[index] = entity;
        }
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = _data.FirstOrDefault(e => e.Id == id);
        if (entity != null) _data.Remove(entity);
        await Task.CompletedTask;
    }
}
```

## Complex Relationship Handling

```csharp
public static class MockDataGenerator
{
    private static readonly Random _random = new Random(42);

    // Generate parent entities first
    public static List<Program> GeneratePrograms(int count = 20)
    {
        return Enumerable.Range(0, count)
            .Select(_ => new Program
            {
                Id = Guid.NewGuid(),
                Name = GenerateProgramName(),
                StartDate = DateTime.Now.AddDays(_random.Next(30, 365)),
                EndDate = DateTime.Now.AddDays(_random.Next(366, 730)),
                IsActive = _random.Next(100) < 80
            })
            .ToList();
    }

    // Generate child entities with valid parent references
    public static List<Request> GenerateRequests(List<Program> programs, int count = 100)
    {
        return Enumerable.Range(0, count)
            .Select(_ => new Request
            {
                Id = Guid.NewGuid(),
                Name = GenerateRequestName(),
                ProgramId = programs[_random.Next(programs.Count)].Id,
                Status = GetRandomRequestStatus(),
                RequestDate = GenerateRequestDate(GetRandomRequestStatus()),
                CreatedAt = DateTime.Now.AddDays(-_random.Next(1, 365))
            })
            .ToList();
    }
}
```

## AI-Driven Consultation Approach

### Phase 1: Entity Analysis

Ask user to provide:
- Entity class name or file location
- Key properties and data types
- Enum definitions with valid values
- Foreign key relationships
- Business rules and constraints

### Phase 2: Strategy Recommendation

Based on entity analysis, recommend:
- Realistic string generation patterns
- Weighted enum distributions
- Business-appropriate date ranges
- Relationship handling strategy
- Data volume recommendations

### Phase 3: Implementation Guidance

Provide:
- MockDataGenerator class structure
- Mock repository implementation template
- DI registration instructions
- Usage examples

### Phase 4: Validation and Refinement

Ask validation questions:
- Does mock data reflect real business scenarios?
- Are all relationships valid?
- Is data volume appropriate?
- Any adjustments needed?

## Integration with Other Skills

- **domain-model-parser**: Provides entity structure information
- **repository-interface-generator**: Supplies repository interface to implement
- **service-generator**: Mock repositories can be injected into generated services
- **blazor-page-generator**: Provides realistic data for UI development
- **test-code-generator**: Supports unit testing with realistic test data

## What This Skill DOES

- Provide AI-driven consultation on mock data generation strategies
- Analyze entity structure and recommend realistic value patterns
- Generate MockDataGenerator class with entity-specific methods
- Create mock repository implementations
- Handle complex relationships with referential integrity
- Provide weighted enum distributions based on business reality
- Support configurable data volumes for different use cases
- Offer DI registration and integration guidance

## What This Skill DOES NOT DO

- Does NOT generate real production data
- Does NOT validate database schema and constraints
- Does NOT create database-specific code (migrations, stored procedures)
- Does NOT handle complex database transactions
- Does NOT support real-time data synchronization
- Does NOT replace actual database in production

## Key Principles

1. **Realistic Over Random**: Use business-realistic values, not random test data
2. **Weighted Distributions**: Enum values should reflect business probabilities
3. **Referential Integrity**: All foreign key references must exist
4. **Business Logic Validation**: Mock data should respect business rules
5. **Scalability**: Support different data volumes for different use cases
6. **Maintainability**: Clear, well-documented code with meaningful method names

## Best Practices

### Data Generation
- Use static Random instance with fixed seed for reproducibility
- Generate parent entities before child entities
- Implement weighted random selection for enums
- Use business-appropriate ranges for numeric values
- Validate relationships maintain referential integrity

### Mock Repository
- Implement all IRepository<TEntity> methods
- Use async patterns consistent with real repository
- Return read-only collections where appropriate
- Support IQueryable for flexible querying
- Handle edge cases (null values, empty lists)

### Performance
- Pre-generate data at repository initialization
- Use LINQ for efficient in-memory queries
- Support lazy loading for large datasets
- Consider caching for frequently accessed data

## Dependencies

- Entity class definition or domain model metadata
- Repository interface (IRepository<TEntity>)
- Related entity classes (for relationship handling)
- PagedResult<T> class (from repository-interface-generator)

## Usage Example

### Program.cs Registration

```csharp
#if DEBUG
builder.Services.AddScoped<IRepository<Request>>(sp => new MockRequestRepository(count: 100));
builder.Services.AddScoped<IRepository<Program>>(sp => new MockProgramRepository(count: 20));
#endif

#if !DEBUG
builder.Services.AddScoped<IRepository<Request>, RequestRepository>();
builder.Services.AddScoped<IRepository<Program>, ProgramRepository>();
#endif
```

### Service Usage

```csharp
public class RequestService
{
    private readonly IRepository<Request> _requestRepository;

    public RequestService(IRepository<Request> requestRepository)
    {
        _requestRepository = requestRepository;
    }

    public async Task<Request?> GetRequestAsync(Guid id)
    {
        return await _requestRepository.GetByIdAsync(id);
    }
}
```

## Troubleshooting

**Q: Can I use mock data in production?**
A: No, mock repositories are for development and testing only.

**Q: How do I handle complex relationships?**
A: Generate parent entities first, then child entities with valid foreign key references.

**Q: Can I customize random seed for reproducibility?**
A: Yes, use a fixed seed value in Random constructor.

**Q: How do I adjust data volume?**
A: Pass desired count parameter to MockRepository constructor.

**Q: Can I create specific test scenarios?**
A: Yes, create additional generation methods for specific scenarios.

## Notes

- This is a **pure AI-driven skill** with no scripts or automation
- Focus is on **consultation and architectural guidance** for realistic mock data
- Mock data generation happens only after user agreement on strategy
- Educational approach helps understand best practices for test data
- Supports both development environment and testing scenarios
- Ensure mock repositories are only used in non-production environments

## Final Step: Code Formatting

After generating all mock data and repository implementations, the skill calls `solution-code-formatter` to ensure all generated code follows proper formatting standards:
```bash
bun run ../../utilities/solution-code-formatter/scripts/format-solution.ts [solution-path]
```
