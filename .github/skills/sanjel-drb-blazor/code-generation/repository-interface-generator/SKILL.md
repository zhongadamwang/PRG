---
name: repository-interface-generator
description: AI-driven architectural consultant for designing and generating repository interface contracts and their corresponding adapter implementations based on different data provider strategies (EF Core, SanjelData, or custom). Provides intelligent guidance on choosing the optimal data provider, generates repository interfaces, and produces concrete adapter classes that implement those interfaces for the selected provider.
license: MIT
---

# Repository Interface Generator

## Description

AI-driven architectural consultant for designing and generating repository interface contracts **and their corresponding adapter implementations** based on different data provider strategies. This skill provides intelligent guidance on choosing the optimal data provider (EF Core, SanjelData, or custom), generates repository interfaces adapted to the selected provider, and produces concrete adapter classes that implement those interfaces.

## When To Use

- **New Project Setup**: When initializing a new project and need to design repository architecture
- **Data Provider Decision**: When deciding between different data access technologies (EF Core vs SanjelData vs custom)
- **Architecture Planning**: When planning the repository layer for specific data access requirements
- **Technology Migration**: When migrating from one data provider to another
- **Provider Selection Uncertainty**: When unsure which data provider best fits project requirements

## Usage

This skill uses AI-driven consultation to guide you through repository interface design and generation. The AI will ask clarifying questions about your data access needs and recommend the optimal provider strategy.

## Input

**Required:**
- Domain model metadata (from `domain-model-parser` output or manual description)

**AI-Consulted Questions:**
- **Data Provider Preference**: 
  - EF Core (Entity Framework) - Modern OR mapper with LINQ support
  - SanjelData - Existing legacy service layer with rich query methods
  - Custom/Other - Other data access technologies (Dapper, ADO.NET, external APIs, etc.)
- **Custom Provider Details** (if "Custom/Other" selected):
  - Provider name and version
  - Async/sync nature
  - Primary query capabilities
  - Pagination approach
- **Repository Layer Goals**:
  - Standard CRUD operations
  - Specific entity queries
  - Business logic requirements
  - Transaction management needs
  - Performance optimization requirements
- **Integration Constraints**:
  - Existing code compatibility requirements
  - Team expertise levels
  - Project timeline constraints

## Output

**Generated Artifacts:**
- Repository interface files for each entity
- Base repository interface template (`IRepository<TEntity>`)
- **Adapter implementation classes** for each entity interface (concrete classes implementing the interfaces using the selected provider)
- Data provider configuration documentation
- Provider-specific implementation guidance
- DI registration recommendations

**Consultation Deliverables:**
- Data provider strategy recommendation
- Architectural pros/cons analysis
- Migration path (if applicable)
- Risk assessment and mitigation strategies

## Generated Code Features

### Standard Interface Methods (All Providers)

Every `IRepository<TEntity>` interface includes these 12 standard methods:

- `GetPagedListAsync` - Paginated list retrieval
- `GetListAsync` - List retrieval with optional filtering
- `GetByIdAsync` - Single entity by ID
- `GetByIdWithChildrenAsync` - Single entity with related entities
- `CreateAsync` - Create new entity
- `CreateWithChildrenAsync` - Create entity with related entities
- `UpdateAsync` - Update existing entity
- `UpdateWithChildrenAsync` - Update entity with related entities
- `DeleteAsync` - Delete entity
- `DeleteWithChildrenAsync` - Delete entity with related entities
- `GetListByIdsAsync` - List of entities by IDs
- `GetListWithChildrenByIdsAsync` - List with relations by IDs

### Provider-Specific Variations

**EF Core Interfaces:**
- Standard `IRepository<TEntity>` with LINQ-friendly signatures
- `Expression<Func<TEntity, bool>>` predicate support
- Native async/await patterns
- No additional wrapper interfaces needed

**SanjelData Interfaces:**
- Same standard `IRepository<TEntity>` interface
- Optional `IEntityQueryService` for accessing SanjelData's 30+ specific query methods
- Pagination conversion guidance (0-based to 1-based)
- Async wrapper recommendations

**Custom Interfaces:**
- Standard base with provider-specific extensions
- Adapter pattern guidance for provider quirks
- Custom query method recommendations

### Generated Adapter Implementations

For every generated interface, a corresponding adapter class is also generated using the naming convention `<Entity>Repository` (e.g., `RequestRepository : IRequestRepository`).

The adapter is a **thin wrapper** — it does not re-implement any data access logic. The underlying provider library (SanjelData, EF Core, etc.) already contains the full implementation. The adapter simply delegates each interface method call to the corresponding provider method:

**SanjelData Adapter** (`RequestRepository : IRequestRepository`):
- Injects the corresponding SanjelData service (e.g., `IProgramRequestService`)
- Each method delegates directly to the matching SanjelData service method
- No custom query logic is written inside the adapter

**EF Core Adapter** (`RequestRepository : IRequestRepository`):
- Injects `DbContext` and delegates to the appropriate `DbSet<TEntity>` and EF Core methods
- No custom query logic is written inside the adapter

**Custom Adapter** (`RequestRepository : IRequestRepository`):
- Injects the custom provider client or service
- Each method delegates directly to the provider's existing methods
- No bespoke logic is added inside the adapter itself

## AI-Driven Consultation Approach

### Phase 1: Needs Assessment

**AI Consultant Role:** Senior Data Access Architect

The AI will ask questions to understand:

```
Welcome! I'm here to help you design your repository layer architecture.

Let's start with understanding your data access needs:

1. **Data Provider Preference**
   What data provider do you intend to use?
   - Option A: EF Core (Entity Framework) - Recommended for new projects
   - Option B: SanjelData - Leverage existing services
   - Option C: Custom/Other - Different technology or approach
   
   Please describe your preference and any constraints.
```

### Phase 2: Provider Analysis

Based on your input, the AI analyzes:

- **For EF Core:**
  - LINQ query capabilities
  - Async/await patterns
  - Change tracking
  - Migration support
  - Recommended for: Greenfield projects, modern architectures, flexible querying needs

- **For SanjelData:**
  - Existing service methods (30+ per entity)
  - Sync/async patterns (may need adaptation)
  - Proven codebase
  - Recommended for: Integration with existing SanjelData, rapid development, leveraging proven queries

- **For Custom:**
  - Specific technology requirements
  - Adapter pattern needs
  - Integration complexity
  - Recommended for: External APIs, specialized databases, legacy systems

### Phase 3: Interface Design

The AI proposes repository interface design based on selected provider.

### Phase 4: Implementation Guidance

**For EF Core:**
- Use `DbContext` and `DbSet<T>`
- Implement LINQ-based queries
- Enable async/await patterns
- Configure entity relationships in `OnModelCreating`

**For SanjelData:**
- Inject `IProgramRequestService` (or similar)
- Wrap sync methods with `Task.Run()` if needed
- Convert 0-based paging to 1-based
- Handle enum type conversions if needed
- Create `ProgramRequestQueryService` for specific queries

**For Custom:**
- Define adapter interfaces
- Implement connection pooling
- Handle async patterns appropriately
- Create query abstractions

### Phase 5: Code Generation

After consultation agreement, the AI generates:

1. **Base Interface**: `IRepository<TEntity>` with standard methods
2. **Entity Interfaces**: `IRequestRepository`, `IDataElementRepository`, etc.
3. **Adapter Implementations**: Concrete adapter classes implementing each entity interface using the naming convention `<Entity>Repository` (e.g., `RequestRepository : IRequestRepository`) — thin wrappers that delegate to the selected provider's existing methods
4. **Base Adapter Class**: Shared `RepositoryBase<TEntity>` with common delegation logic where applicable
5. **Provider Documentation**: Setup and configuration guides
6. **DI Configuration**: Service registration patterns (registering adapters against their interfaces)
7. **Implementation Guidance**: Step-by-step implementation steps

## Integration with Other Skills

- **domain-model-parser**: Provides entity type information for interface generation
- **efcore-repository-generator**: Consumes interfaces for EF Core implementations (if EF Core chosen)
- **service-generator**: Repository interfaces are injected into generated services

## What This Skill DOES

- Provide AI-driven consultation on data provider selection
- Analyze project requirements and recommend optimal provider strategy
- Generate standard repository interface contracts (`IRepository<TEntity>`)
- Generate entity-specific repository interfaces (e.g., `IRequestRepository`)
- **Generate concrete adapter implementations** for each interface using `<Entity>Repository` naming (e.g., `RequestRepository : IRequestRepository`) — thin wrappers that delegate to the existing provider library
- **Generate a shared base adapter class** (`RepositoryBase<TEntity>`) with common delegation logic
- Create provider-specific implementation guidance
- Document provider-specific patterns and considerations
- Provide DI registration recommendations (mapping adapters to interfaces)
- Explain architectural trade-offs and decision rationale

## What This Skill DOES NOT DO

- Does NOT generate database schema migrations
- Does NOT generate `DbContext` configuration code
- Does NOT generate business service layer code (handled by `service-generator`)
- Does NOT generate UI or API layer code

## Data Provider Strategies Overview

### EF Core Strategy (Recommended for New Projects)

**Best for:** Greenfield projects, modern architectures, flexible querying needs

**Key Benefits:** LINQ expressiveness, native async/await, change tracking, migration support

### SanjelData Strategy (Recommended for Integration)

**Best for:** Integration with existing SanjelData, rapid development, leveraging proven code

**Key Benefits:** 30+ proven query methods per entity, tested codebase, business logic included

### Custom/Other Strategy

**Best for:** External APIs, specialized databases, legacy system integration

**Key Benefits:** Full control, optimized for specific needs, no framework constraints

## Key Principles

1. **Standard Interface Contract**: All providers use the same 12 standard methods
2. **Provider Abstraction**: Implementation details hidden behind interface
3. **AI-Driven Decision Making**: Consultation ensures optimal provider choice
4. **Educational Approach**: Understand the "why" behind recommendations
5. **Flexibility**: Interfaces remain consistent across provider changes

## Example Scenarios

### Scenario 1: New Project with EF Core
User selects EF Core → AI generates standard interfaces **and** `<Entity>Repository` adapter classes (e.g., `RequestRepository`) that delegate to EF Core's existing `DbSet<TEntity>` methods → DI registration maps adapters to interfaces

### Scenario 2: SanjelData Integration
User selects SanjelData → AI generates standard interfaces **and** `<Entity>Repository` adapter classes that delegate directly to SanjelData service methods (no re-implementation of query logic)

### Scenario 3: External API Integration
User selects Custom provider → AI generates standard interfaces **and** `<Entity>Repository` adapter classes that delegate to the existing provider's methods

## Dependencies

- Domain model metadata from `domain-model-parser` (optional but recommended)
- Project namespace conventions
- Repository project structure

## Best Practices

- Start with AI consultation before any code generation
- Base provider decision on project context, not just technology preference
- Keep standard methods consistent across all repository interfaces
- Add entity-specific methods only when truly needed
- Document provider-specific patterns in the generated documentation
- Use the generated DI registration patterns for consistency

## Troubleshooting

**Q: Can I change data providers later?**
A: Yes! The interfaces remain the same. Just regenerate implementations with the appropriate provider-specific skill.

**Q: Should I mix providers in one project?**
A: Generally no. Choose one primary provider for consistency. Use adapters only if absolutely necessary.

**Q: What if I need provider-specific queries?**
A: Add them to entity-specific interfaces (e.g., `IRequestRepository`) or create separate query service interfaces.

**Q: How do I handle async/sync differences?**
A: The AI consultation will address this. SanjelData adapters may need `Task.Run()` wrappers, which will be documented.

## Notes

- This is a **pure AI-driven skill** with no scripts or automation
- Focus is on **consultation and architectural guidance**
- Code generation happens only after user agreement
- Educational approach helps you understand the decisions
- Interfaces are provider-agnostic, implementations are provider-specific
- Use this skill when starting a project or reconsidering data access strategy
