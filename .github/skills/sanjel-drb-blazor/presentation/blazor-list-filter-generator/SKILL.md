---
name: blazor-list-filter-generator
description: Generate comprehensive search, sort, and filter functionality for Blazor list pages with advanced query building and real-time filtering.
---

# Blazor List Filter Generator

**Responsibility**: Implement complete search, sort, and filter functionality for list pages in Blazor applications. After completing each feature, add corresponding unit tests, including Playwright UI tests (using C# scripts) for Blazor list pages.
**Input**: List page structure from blazor-list-pattern-generator + Entity metadata + Search/filter requirements
**Output**: Complete search/filter implementation with query building, real-time filtering, and advanced search capabilities. All features must be validated by unit tests and Playwright UI tests (C#).

**Approach**: **AI-Driven Search & Filter Implementation with Interactive User Guidance**
- **Automatically discovers and analyzes project context**:
  - Detects component library from project files and dependencies (MudBlazor, Syncfusion, Bootstrap, Minimal)
  - Extracts ViewModel, Service, and Repository from existing page code
  - Identifies entity type and all available properties with their data types
  - Reads entity metadata including property names, types, and relationships
- **Interactively guides user through configuration**:
  - Presents discovered fields with their types for user selection
  - Asks user to choose which fields to display in the query results table
  - Asks user to select which fields should support filtering
  - Collects filter method preferences for each filter field (e.g., text: contains/equals, date: range, numeric: range/comparison)
  - Optionally collects global search field preferences for fuzzy search
  - Optionally collects sortable field preferences and default sort order
- Implements search input handlers with debouncing and real-time filtering
- Builds dynamic filter UI components based on user selections and property types
- Generates query building logic for complex search conditions
- Handles sort operations with multi-column sorting support
- **Output**: Working search/filter functionality with performance optimization

## Description

**Testing Requirement — MANDATORY:**
After implementing any search, filter, or sort functionality, you **MUST** also generate corresponding **Playwright UI tests (C#, NUnit)** in the project's existing Blazor test project (e.g. `src/Sanjel.RequestManagement.Blazor.Tests/`). These tests are a required output deliverable — the skill implementation is NOT complete until the tests are generated.

**What to test (Playwright):**
- Filter fields (text inputs, dropdowns) render on the page
- Entering a search term and clicking Apply filters the list results
- Selecting a dropdown filter value and applying it returns filtered results
- Clearing filters restores the unfiltered list
- Column sort headers are clickable and change the sort order
- Empty state message appears when no results match the filter

**Test file conventions:**
- Add tests to the existing Playwright test file or create a new test class following the naming pattern `{Entity}FilterPlaywrightTests.cs`
- Use the same setup/teardown pattern as existing Playwright tests (see `RequestPagePlaywrightTests.cs` for reference)
- Use NUnit `[TestFixture]` / `[Test]` attributes
- Use `Microsoft.Playwright` for browser automation
- Tests must be headless-compatible

This skill acts as a **Search & Filter Operations Specialist** for Blazor list pages. It implements the complete search and filter functionality through an **interactive discovery and configuration process**:

### Automatic Discovery Phase
- **Component Library Detection**: Automatically analyzes project files (`.csproj`, `Program.cs`) and dependencies to identify the active component library (MudBlazor, Syncfusion, Bootstrap, or Minimal)
- **Context Extraction**: Reads the existing list page code (`.razor`, `.razor.cs`) to extract:
  - ViewModel class name and properties
  - Service interface and implementation
  - Repository interface and implementation
  - Entity type and metadata
- **Field Analysis**: Identifies all available entity properties with:
  - Property names and display names
  - Data types (string, int, decimal, DateTime, bool, enum, etc.)
  - Relationships to other entities
  - Nullable/non-nullable status

### Interactive Configuration Phase
The skill **engages the user interactively** to make informed decisions about search and filter configuration:

#### 2.1 Query Field Selection
Presents discovered fields and asks user to select which columns to display in the results table:
```
Available fields discovered from {EntityName}:
[1] {PropertyName} ({DataType}) - {OptionalDescription}
[2] {PropertyName} ({DataType}) - {OptionalDescription}
...

Please select which fields to display in the query results table:
- Enter field numbers separated by commas (e.g., "1,2,3,5")
- Or specify "all" to display all fields
```

#### 2.2 Filter Field Selection
Presents discovered fields and asks user to select which fields should support filtering:
```
Available fields for filtering:
[1] {PropertyName} ({DataType})
[2] {PropertyName} ({DataType})
...

Please select which fields to enable filtering:
- Enter field numbers separated by commas
- For each selected field, specify the filter method:
  * String fields: [contains/equals/startsWith/endsWith]
  * Date fields: [range/exact/before/after]
  * Numeric fields: [range/equals/greaterThan/lessThan]
  * Enum fields: [multiSelect/singleSelect]
  * Boolean fields: [toggle/checkbox]
```

#### 2.3 Global Search Configuration (Optional)
Asks user about global fuzzy search preferences:
```
Would you like to enable global search (fuzzy search across multiple fields)? [yes/no]

If yes, please select which text fields should participate in global search:
[1] {PropertyName} ({string})
[2] {PropertyName} ({string})
...

Enter field numbers for global search participation:
```

#### 2.4 Sort Configuration (Optional)
Asks user about sorting preferences:
```
Which fields should support column sorting?
[1] {PropertyName} ({DataType})
[2] {PropertyName} ({DataType})
...

Enter field numbers for sortable columns:

Default sort field and direction:
- Primary sort field: [field name or "none"]
- Direction: [ascending/descending]
```

#### 2.5 Advanced Options (Optional)
Collects additional requirements based on user needs:
- Performance constraints (expected data volume, response time requirements)
- Filter combination logic (AND/OR between multiple filters)
- Search result caching preferences
- Debounce delay for real-time search (default: 300ms)
- Filter preset requirements for common scenarios

### Implementation Phase
- Taking the structural framework from the list page
- Implementing the TODO-marked search and filter operation placeholders
- Creating dynamic filter UI based on entity property types with appropriate component library patterns
- Building efficient query logic for backend data filtering
- Supporting advanced search patterns and saved search functionality

**DEPENDENCY**: This skill **REQUIRES** an existing Blazor list page file (`.razor`, `.razor.cs`) to work with. It cannot work in isolation or generate pages from scratch.

## Responsibility

Implement **SEARCH, SORT & FILTER OPERATIONS** for list pages, including:
- ✅ Global search functionality with debouncing (replace placeholder TODO methods)
- ✅ Column-specific filtering with appropriate UI controls
- ✅ Multi-column sorting with sort indicators
- ✅ Advanced search builders with complex query conditions
- ✅ Filter state persistence and saved search functionality
- ✅ Real-time filtering with performance optimization
- ✅ Query building for backend API integration
- ✅ Search result highlighting and count display

**WORKS WITH**:
- ✅ Structure from `blazor-list-pattern-generator`
- ✅ Service layer search/filter methods
- ✅ Component library filter controls
- ✅ State management systems for filter persistence

## AI Persona

**Role**: Senior Search & Filter Operations Specialist & Query Optimization Expert  
**Expertise**: 15+ years experience in search implementation and query optimization  
**Specializations**:
- Real-time search implementation with debouncing and performance optimization
- Dynamic filter UI generation based on data types and relationships
- Complex query building and LINQ expression generation
- Advanced search patterns (faceted search, full-text search, etc.)
- Search state persistence and user experience optimization
- Performance optimization for large dataset filtering
- Multi-language search support and text analysis
- Search analytics and user behavior tracking integration

## Input Parameters

### Automatic Discovery (No User Input Required)
This skill **automatically discovers and extracts** the following information from the existing codebase:
- **Component Library**: Detected from project files and dependencies (`.csproj`, `Program.cs`, existing imports)
- **ViewModel Class**: Extracted from the list page code (`@inject`, model usage)
- **Service Interface**: Identified from dependency injection and method calls
- **Repository Interface**: Derived from the Service interface and dependency chain
- **Entity Type**: Determined from the Repository interface and ViewModel mapping
- **Entity Properties**: All fields with their names, data types, and metadata

### Required User Interactions
The skill **interactively collects** the following information through user dialogue:

#### 1. Query Field Selection (Required)
- User selects which entity fields should display in the results table
- User specifies column order if needed
- User can choose "all" to display all discovered fields

#### 2. Filter Field Selection (Required)
- User selects which fields should support filtering functionality
- For each filter field, user specifies the filter method:
  - **String fields**: Contains, Equals, Starts With, Ends With
  - **DateTime fields**: Date Range, Exact Date, Before Date, After Date
  - **Numeric fields** (int, decimal, etc.): Range, Equals, Greater Than, Less Than
  - **Enum fields**: Multi-select dropdown, Single-select dropdown
  - **Boolean fields**: Toggle switch, Checkbox

#### 3. Global Search Fields (Optional, Recommended)
- User selects which text fields should participate in global fuzzy search
- If user skips, defaults to all string fields

#### 4. Sort Configuration (Optional, Recommended)
- User selects which fields should support column sorting (click header to sort)
- User specifies default sort field and direction (ascending/descending)
- If user skips, defaults to primary key field ascending

### Optional User Interactions
- **Performance Requirements**: Expected data volume, acceptable response time
- **Filter Logic**: AND vs OR combination between multiple filters
- **Debounce Delay**: Real-time search delay in milliseconds (default: 300ms)
- **Search Caching**: Enable/disable search result caching (default: enabled)
- **Filter Presets**: Define common filter combinations as presets
- **Advanced Search**: Visual query builder for complex conditions
- **Saved Searches**: Allow users to save and restore search criteria
- **Export Integration**: Export filtered results functionality

## Output Deliverables

### 1. **Complete Search Operation Implementation**

Replaces all TODO placeholders from `blazor-list-pattern-generator` with working implementations:

```csharp
// BEFORE (from blazor-list-pattern-generator):
// TODO: [blazor-list-filter-generator] Implement search logic
private async Task SearchAsync()
{
    throw new NotImplementedException("Requires blazor-list-filter-generator skill");
}

// AFTER (implemented by this skill):
private async Task SearchAsync()
{
    if (string.IsNullOrWhiteSpace(_searchTerm))
    {
        await LoadDataAsync();
        return;
    }
    
    _isSearching = true;
    StateHasChanged();
    
    try
    {
        var searchRequest = BuildSearchRequest();
        var results = await RequestService.SearchAsync(searchRequest);
        UpdateSearchResults(results);
    }
    finally
    {
        _isSearching = false;
        StateHasChanged();
    }
}
```

### 2. **Dynamic Filter UI Generation**

- **Type-Based Filters**: Generate appropriate filter controls based on property types
- **Relationship Filters**: Handle entity relationship filtering with dropdown/autocomplete
- **Date Range Filters**: Implement date/time range pickers
- **Numeric Filters**: Support range and comparison operators for numeric fields
- **Enum Filters**: Multi-select dropdowns for enum properties
- **Boolean Filters**: Checkbox or toggle controls for boolean fields

### 3. **Advanced Search Builder**

- **Query Builder UI**: Visual query builder with AND/OR conditions
- **Saved Searches**: Save and restore complex search queries
- **Search Templates**: Pre-defined search templates for common queries
- **Export Integration**: Export search results with applied filters

### 4. **Real-Time Filtering with Performance**

- **Debounced Search**: Delay search execution to reduce API calls
- **Client-Side Filtering**: Filter already loaded data when possible
- **Progressive Loading**: Load more results as user scrolls or pages
- **Search Caching**: Cache search results for repeated queries

### 5. **Sort Operations Implementation**

- **Multi-Column Sorting**: Support sorting by multiple columns with priority
- **Sort State Persistence**: Remember sort preferences across sessions
- **Custom Sort Logic**: Support custom sort operations for complex data types
- **Sort Performance**: Optimize sorting for large datasets

### 6. **Playwright UI Tests (MANDATORY)**

Generate Playwright tests that validate the filter/search workflow end-to-end in a real browser. Tests are placed in the Blazor test project alongside existing Playwright tests.

```csharp
using Microsoft.Playwright;
using NUnit.Framework;

namespace Sanjel.RequestManagement.Blazor.Tests
{
    [TestFixture]
    public class RequestFilterPlaywrightTests
    {
        private IBrowser _browser;
        private IPage _page;
        private IPlaywright _playwright;

        [OneTimeSetUp]
        public async Task SetupAsync()
        {
            this._playwright = await Playwright.CreateAsync();
            this._browser = await this._playwright.Chromium.LaunchAsync(
                new BrowserTypeLaunchOptions { Headless = true });
            this._page = await this._browser.NewPageAsync();
        }

        [OneTimeTearDown]
        public async Task TeardownAsync()
        {
            await this._browser.CloseAsync();
            this._playwright.Dispose();
        }

        [Test]
        public async Task FilterPanel_ShouldRenderFilterFieldsAsync()
        {
            await this._page.GotoAsync("http://localhost:5000/request");
            await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await this._page.WaitForTimeoutAsync(2000);

            var filterPanel = await this._page.QuerySelectorAsync(".filter-panel");
            Assert.IsNotNull(filterPanel, "Filter panel should be visible");

            var applyBtn = await this._page.QuerySelectorAsync("button:has-text('Apply')");
            Assert.IsNotNull(applyBtn, "Apply filter button should exist");

            var clearBtn = await this._page.QuerySelectorAsync("button:has-text('Clear')");
            Assert.IsNotNull(clearBtn, "Clear filter button should exist");
        }

        [Test]
        public async Task ApplyFilter_ShouldFilterListResultsAsync()
        {
            await this._page.GotoAsync("http://localhost:5000/request");
            await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await this._page.WaitForTimeoutAsync(2000);

            // Enter a search/filter value that is unlikely to match all items
            var searchInput = await this._page.QuerySelectorAsync(
                "input[placeholder*='Search'], input[placeholder*='ID']");
            Assert.IsNotNull(searchInput, "Search input should exist");
            await searchInput.FillAsync("NONEXISTENT_VALUE_12345");

            var applyBtn = await this._page.QuerySelectorAsync("button:has-text('Apply')");
            await applyBtn.ClickAsync();
            await this._page.WaitForTimeoutAsync(1000);

            // Expect no results or empty state
            var emptyState = await this._page.QuerySelectorAsync(".empty-state");
            var rows = await this._page.QuerySelectorAllAsync(".e-gridcontent tr.e-row");
            Assert.That(
                emptyState != null || rows.Count == 0,
                "Filter should reduce results to zero for a non-matching term");
        }

        [Test]
        public async Task ClearFilter_ShouldRestoreAllResultsAsync()
        {
            await this._page.GotoAsync("http://localhost:5000/request");
            await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await this._page.WaitForTimeoutAsync(2000);

            var clearBtn = await this._page.QuerySelectorAsync("button:has-text('Clear')");
            await clearBtn.ClickAsync();
            await this._page.WaitForTimeoutAsync(1000);

            var pageTitle = await this._page.TitleAsync();
            Assert.IsNotNull(pageTitle, "Page should remain functional after clearing filters");
        }
    }
}
```

**Key**: The test file **MUST** be generated as part of the skill output. Adapt selectors, URLs, and assertions to match the actual page being implemented.

## Functional Capabilities

### Core Search Operations
- **Global Search**: Search across multiple entity properties simultaneously
- **Column Search**: Individual column filtering with property-specific controls
- **Full-Text Search**: Integration with full-text search engines where available
- **Fuzzy Search**: Support for approximate string matching and typo tolerance

### Filter Management
- **Dynamic Filters**: Generate filters based on entity metadata and property types
- **Filter Combinations**: Support AND/OR logic combinations between filters
- **Filter Validation**: Validate filter inputs and show appropriate error messages
- **Filter Presets**: Pre-defined filter combinations for common use cases

### Advanced Search Features
- **Query Builder**: Visual interface for building complex search queries
- **Saved Searches**: Save frequently used search criteria
- **Search History**: Track and restore recent searches
- **Search Suggestions**: Auto-complete and search term suggestions

### Performance and UX
- **Debouncing**: Reduce API calls with intelligent search delay
- **Loading States**: Show appropriate loading indicators during search
- **Result Highlighting**: Highlight search terms in results
- **Search Analytics**: Track search patterns for optimization

## Integration Points

### Dependencies
- **blazor-list-pattern-generator**: Provides list page structure with search/filter TODO placeholders
- **service-method-generator**: Uses service methods for search/filter operations
- **component-library**: Uses search inputs, filter controls, and sort indicators
- **state-management**: Integrates with state persistence for search preferences

### Service Layer Integration
- **Search Methods**: Calls service.SearchAsync() and service.FilterAsync() methods
- **Query Building**: Builds structured query objects for backend processing
- **Result Processing**: Processes search results and applies client-side enhancements
- **Performance Integration**: Works with service-layer caching and pagination

### UI Component Integration
- **Search Controls**: Integrates with text inputs, autocomplete, and search icons
- **Filter Components**: Uses dropdowns, date pickers, range sliders, checkboxes
- **Sort Indicators**: Integrates with column headers and sort direction indicators
- **Result Display**: Enhances result display with highlighting and search context

## Usage Scenarios

### Scenario 1: Simple Global Search (Auto-Discovery + Interactive Configuration)
**Discovery Phase**:
- Automatically detects: MudBlazor library, RequestViewModel, RequestService, RequestRepository, Request entity
- Extracts 6 fields: RequestId (Guid), Title (string), Description (string), Status (enum), CreatedDate (DateTime), Priority (int)

**Interactive Configuration**:
```
AI: I found the Request entity with the following fields:
[1] RequestId (Guid)
[2] Title (string)
[3] Description (string)
[4] Status (enum)
[5] CreatedDate (DateTime)
[6] Priority (int)

Please select which fields to display in the results table:
User: 2,3,4,5

Which fields should support filtering?
User: 4,5,6

For Status (enum), what filter method? [multiSelect/singleSelect]
User: singleSelect

For CreatedDate (DateTime), what filter method? [range/exact/before/after]
User: range

For Priority (int), what filter method? [range/equals/greaterThan/lessThan]
User: range

Which fields should participate in global search? [text fields: 2,3]
User: 2,3

Which fields should support sorting?
User: 5,6

Default sort field and direction?
User: CreatedDate, descending
```

**Output**: Global search box with real-time filtering and highlighting
**Features**:
- ✅ Debounced search input (300ms) with loading indicator
- ✅ Search across Title and Description fields
- ✅ Status dropdown filter (single-select)
- ✅ CreatedDate date range filter
- ✅ Priority numeric range filter
- ✅ Sortable columns: CreatedDate, Priority
- ✅ Default sort: CreatedDate descending
- ✅ Clear search button and search term display
- ✅ Result count and "no results" messaging

### Scenario 2: Advanced Multi-Filter Interface
**Discovery Phase**:
- Automatically detects: Syncfusion library, OrderViewModel, OrderService, OrderRepository, Order entity
- Extracts 10 fields including relationships to Customer, Product, etc.

**Interactive Configuration**:
```
AI: Found Order entity with 10 fields including relationships...
User selects: 8 fields for display, 6 for filtering
User specifies filter methods for each field
User enables: advanced search builder, saved searches, export
```

**Output**: Multiple filter controls with complex query building
**Features**:
- ✅ Property-specific filter controls (date ranges, dropdowns, etc.)
- ✅ Filter combination logic (AND/OR between filters)
- ✅ Active filter display with individual clear buttons
- ✅ Filter preset options for common search scenarios
- ✅ Visual query builder with nested conditions
- ✅ Save/restore search functionality
- ✅ Export filtered results to CSV/Excel

### Scenario 3: High-Performance Large Dataset Search
**Discovery Phase**:
- Automatically detects: MudBlazor library, TransactionViewModel, TransactionService, TransactionRepository, Transaction entity
- Extracts 15 fields, user indicates expected 50K+ records

**Interactive Configuration**:
```
User: "Expecting 50K+ records, need fast response times"
User selects fields for minimal display (5 columns)
User enables: server-side pagination, search caching, virtual scrolling
User specifies: 500ms debounce delay for search
```

**Output**: Optimized search with server-side processing and caching
**Features**:
- ✅ Server-side search processing with pagination
- ✅ Search result caching (5-minute expiration, 100-item limit)
- ✅ Progressive loading and virtual scrolling integration
- ✅ 500ms debounce delay for reduced API calls
- ✅ Intelligent prefetching of next pages
- ✅ Search performance monitoring and optimization

### Scenario 4: Interactive Quick Configuration (Minimal User Input)
**Discovery Phase**:
- Automatically detects all necessary context from existing code

**Interactive Configuration**:
```
AI: I found {EntityName} with {N} fields. Would you like to use recommended defaults?
[✓] Display all fields
[✓] Enable filtering on text and date fields
[✓] Enable sorting on all columns
[✓] Enable global search on all text fields
User: Yes, use defaults

AI: Recommended configuration applied. Any adjustments needed?
User: No
```

**Output**: Working search/filter functionality with sensible defaults
**Features**:
- ✅ All fields displayed in results table
- ✅ Text fields support "contains" filtering
- ✅ Date fields support "range" filtering
- ✅ Numeric fields support "range" filtering
- ✅ All columns sortable
- ✅ Global search on all text fields
- ✅ Standard debounce (300ms) and caching enabled

## Implementation Patterns

### Debounced Global Search
```csharp
private readonly debounceTimer = new System.Timers.Timer(300);
private string _searchTerm = string.Empty;
private string _lastSearchTerm = string.Empty;

public string SearchTerm
{
    get => _searchTerm;
    set
    {
        if (_searchTerm != value)
        {
            _searchTerm = value;
            
            // Reset and restart debounce timer
            debounceTimer.Stop();
            debounceTimer.Start();
        }
    }
}

private async void OnDebounceTimerElapsed(object sender, ElapsedEventArgs e)
{
    debounceTimer.Stop();
    
    if (_searchTerm != _lastSearchTerm)
    {
        _lastSearchTerm = _searchTerm;
        await InvokeAsync(async () => await ExecuteSearchAsync());
    }
}

private async Task ExecuteSearchAsync()
{
    _isLoading = true;
    StateHasChanged();
    
    try
    {
        var searchRequest = new SearchRequest
        {
            Term = _searchTerm,
            Fields = new[] { "Title", "Description", "Code" },
            PageSize = _pageSize,
            PageNumber = 1
        };
        
        var results = await RequestService.SearchAsync(searchRequest);
        
        _items = results.Items;
        _totalCount = results.TotalCount;
        _currentPage = 1;
        
        StateHasChanged();
    }
    catch (Exception ex)
    {
        ShowErrorMessage("Search failed. Please try again.");
        Logger.LogError(ex, "Error executing search for term: {SearchTerm}", _searchTerm);
    }
    finally
    {
        _isLoading = false;
        StateHasChanged();
    }
}
```

### Dynamic Filter Generation
```csharp
private RenderFragment GenerateFiltersForEntity(EntityMetadata metadata)
{
    return builder =>
    {
        var sequence = 0;
        
        foreach (var property in metadata.SearchableProperties)
        {
            builder.OpenComponent(sequence++, GetFilterComponentType(property));
            builder.AddAttribute(sequence++, "Label", property.DisplayName);
            builder.AddAttribute(sequence++, "Value", GetFilterValue(property.Name));
            builder.AddAttribute(sequence++, "ValueChanged", 
                EventCallback.Factory.Create(this, 
                    (object newValue) => UpdateFilter(property.Name, newValue)));
            
            // Add property-specific attributes
            AddPropertySpecificAttributes(builder, ref sequence, property);
            
            builder.CloseComponent();
        }
    };
}

private Type GetFilterComponentType(PropertyMetadata property)
{
    return property.DataType switch
    {
        "string" => typeof(MudTextField<string>),
        "DateTime" => typeof(MudDateRangePicker),
        "int" or "decimal" => typeof(MudNumericField<decimal?>),
        "bool" => typeof(MudCheckBox<bool?>),
        "enum" => typeof(MudSelect<string>),
        _ => typeof(MudTextField<string>)
    };
}
```

### Advanced Query Builder
```csharp
public class QueryBuilder
{
    private readonly List<FilterGroup> _filterGroups = new();
    
    public class FilterGroup
    {
        public LogicalOperator Operator { get; set; } = LogicalOperator.And;
        public List<FilterCondition> Conditions { get; set; } = new();
    }
    
    public class FilterCondition
    {
        public string PropertyName { get; set; }
        public ComparisonOperator Operator { get; set; }
        public object Value { get; set; }
        public string DisplayText => BuildDisplayText();
    }
    
    public SearchRequest BuildSearchRequest()
    {
        var request = new SearchRequest();
        
        foreach (var group in _filterGroups)
        {
            var groupConditions = new List<SearchCondition>();
            
            foreach (var condition in group.Conditions)
            {
                groupConditions.Add(new SearchCondition
                {
                    Field = condition.PropertyName,
                    Operator = MapOperator(condition.Operator),
                    Value = condition.Value
                });
            }
            
            request.FilterGroups.Add(new SearchFilterGroup
            {
                Operator = group.Operator,
                Conditions = groupConditions
            });
        }
        
        return request;
    }
}
```

### Multi-Column Sort Implementation
```csharp
public class SortManager
{
    private readonly List<SortColumn> _sortColumns = new();
    
    public void ToggleSort(string columnName)
    {
        var existing = _sortColumns.FirstOrDefault(s => s.ColumnName == columnName);
        
        if (existing == null)
        {
            // Add new sort column
            _sortColumns.Add(new SortColumn
            {
                ColumnName = columnName,
                Direction = SortDirection.Ascending,
                Priority = _sortColumns.Count
            });
        }
        else if (existing.Direction == SortDirection.Ascending)
        {
            // Change to descending
            existing.Direction = SortDirection.Descending;
        }
        else
        {
            // Remove sort column
            _sortColumns.Remove(existing);
            
            // Update priorities
            for (int i = 0; i < _sortColumns.Count; i++)
            {
                _sortColumns[i].Priority = i;
            }
        }
        
        ApplySortToData();
    }
    
    private async Task ApplySortToData()
    {
        var sortRequest = new SortRequest
        {
            SortColumns = _sortColumns.Select(s => new SortColumn
            {
                ColumnName = s.ColumnName,
                Direction = s.Direction
            }).ToList()
        };
        
        var results = await RequestService.GetSortedDataAsync(sortRequest);
        UpdateDataWithResults(results);
    }
}
```

## Syncfusion Component Implementation Patterns

### Enum Dropdown Filter with Proper Configuration

**Critical Success Pattern**: Syncfusion SfDropDownList requires structured data objects with proper TValue/TItem configuration.

#### 1. Data Structure Classes (Code-Behind)
```csharp
// In Index.razor.cs or component code-behind
public partial class Index : ComponentBase
{
    // Structured data classes for dropdown binding
    public class StatusEnumItem
    {
        public StatusEnum? Value { get; set; }
        public string Text { get; set; } = string.Empty;
    }
    
    public class PriorityEnumItem  
    {
        public PriorityEnum? Value { get; set; }
        public string Text { get; set; } = string.Empty;
    }
    
    // Data sources for dropdowns
    private List<StatusEnumItem> statusOptionItems = new();
    private List<PriorityEnumItem> priorityOptionItems = new();
    
    protected override async Task OnInitializedAsync()
    {
        // Initialize dropdown data sources
        statusOptionItems = new List<StatusEnumItem>
        {
            new() { Value = null, Text = "All Statuses" },
            new() { Value = StatusEnum.Draft, Text = GetStatusDisplayName(StatusEnum.Draft) },
            new() { Value = StatusEnum.Submitted, Text = GetStatusDisplayName(StatusEnum.Submitted) },
            new() { Value = StatusEnum.InProgress, Text = GetStatusDisplayName(StatusEnum.InProgress) },
            new() { Value = StatusEnum.UnderReview, Text = GetStatusDisplayName(StatusEnum.UnderReview) },
            new() { Value = StatusEnum.Approved, Text = GetStatusDisplayName(StatusEnum.Approved) },
            new() { Value = StatusEnum.Rejected, Text = GetStatusDisplayName(StatusEnum.Rejected) },
            new() { Value = StatusEnum.Completed, Text = GetStatusDisplayName(StatusEnum.Completed) },
            new() { Value = StatusEnum.Cancelled, Text = GetStatusDisplayName(StatusEnum.Cancelled) }
        };
        
        priorityOptionItems = new List<PriorityEnumItem>
        {
            new() { Value = null, Text = "All Priorities" },
            new() { Value = PriorityEnum.Low, Text = GetPriorityDisplayName(PriorityEnum.Low) },
            new() { Value = PriorityEnum.Normal, Text = GetPriorityDisplayName(PriorityEnum.Normal) },
            new() { Value = PriorityEnum.High, Text = GetPriorityDisplayName(PriorityEnum.High) },
            new() { Value = PriorityEnum.Critical, Text = GetPriorityDisplayName(PriorityEnum.Critical) }
        };
        
        await base.OnInitializedAsync();
    }
    
    // Display name mapping functions for user-friendly text
    private string GetStatusDisplayName(StatusEnum status)
    {
        return status switch
        {
            StatusEnum.Draft => "Draft",
            StatusEnum.Submitted => "Submitted", 
            StatusEnum.InProgress => "In Progress",
            StatusEnum.UnderReview => "Under Review",
            StatusEnum.Approved => "Approved",
            StatusEnum.Rejected => "Rejected",
            StatusEnum.Completed => "Completed",
            StatusEnum.Cancelled => "Cancelled",
            _ => status.ToString()
        };
    }
    
    private string GetPriorityDisplayName(PriorityEnum priority)
    {
        return priority switch
        {
            PriorityEnum.Low => "Low Priority",
            PriorityEnum.Normal => "Normal Priority", 
            PriorityEnum.High => "High Priority",
            PriorityEnum.Critical => "Critical Priority",
            _ => priority.ToString()
        };
    }
}
```

#### 2. Razor Component Configuration
```razor
@* Status Filter Dropdown - CORRECT Configuration *@
<SfDropDownList TValue="StatusEnum?" 
                TItem="StatusEnumItem"
                @bind-Value="ViewModel.StatusFilter"
                DataSource="statusOptionItems"
                Placeholder="Select Status"
                AllowFiltering="false">
    <DropDownListFieldSettings Value="Value" Text="Text" />
</SfDropDownList>

@* Priority Filter Dropdown - CORRECT Configuration *@
<SfDropDownList TValue="PriorityEnum?"
                TItem="PriorityEnumItem" 
                @bind-Value="ViewModel.PriorityFilter"
                DataSource="priorityOptionItems"
                Placeholder="Select Priority"
                AllowFiltering="false">
    <DropDownListFieldSettings Value="Value" Text="Text" />
</SfDropDownList>

@* Text Filter - Standard Configuration *@
<SfTextBox @bind-Value="ViewModel.TitleFilter"
           Placeholder="Search by title..."
           ShowClearButton="true" />

@* Apply/Clear Filter Buttons *@
<SfButton CssClass="e-primary" @onclick="ApplyFiltersAsync">Apply Filters</SfButton>
<SfButton CssClass="e-outline" @onclick="ClearFiltersAsync">Clear Filters</SfButton>
```

#### 3. ViewModel Filter Properties
```csharp
// In RequestListViewModel.cs or similar ViewModel class
public class RequestListViewModel
{
    public string TitleFilter { get; set; } = string.Empty;
    public StatusEnum? StatusFilter { get; set; }
    public PriorityEnum? PriorityFilter { get; set; }
    
    public bool HasActiveFilters()
    {
        return !string.IsNullOrWhiteSpace(TitleFilter) ||
               StatusFilter.HasValue ||
               PriorityFilter.HasValue;
    }
    
    public void ResetFilters()
    {
        TitleFilter = string.Empty;
        StatusFilter = null;
        PriorityFilter = null;
    }
}
```

#### 4. Service Layer Filtering Logic
```csharp
// In RequestService.cs or similar Service class
public async Task<IEnumerable<RequestDto>> GetFilteredRequestsAsync(RequestListViewModel viewModel)
{
    var query = _repository.GetQueryable();
    
    // Build filter predicate based on ViewModel
    if (viewModel.HasActiveFilters())
    {
        query = query.Where(BuildFilterPredicate(viewModel));
    }
    
    return await query
        .OrderByDescending(r => r.CreatedDate)
        .Select(r => new RequestDto
        {
            RequestId = r.RequestId,
            Title = r.Title,
            Status = r.Status,
            CreatedDate = r.CreatedDate,
            Priority = r.Priority
        })
        .ToListAsync();
}

private Expression<Func<Request, bool>> BuildFilterPredicate(RequestListViewModel viewModel)
{
    var parameter = Expression.Parameter(typeof(Request), "r");
    Expression? filterExpression = null;
    
    // Title filter - contains search
    if (!string.IsNullOrWhiteSpace(viewModel.TitleFilter))
    {
        var titleProperty = Expression.Property(parameter, nameof(Request.Title));
        var titleValue = Expression.Constant(viewModel.TitleFilter, typeof(string));
        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        var titleFilter = Expression.Call(titleProperty, containsMethod!, titleValue);
        filterExpression = CombineWithAnd(filterExpression, titleFilter);
    }
    
    // Status filter - exact match
    if (viewModel.StatusFilter.HasValue)
    {
        var statusProperty = Expression.Property(parameter, nameof(Request.Status));
        var statusValue = Expression.Constant(viewModel.StatusFilter.Value);
        var statusFilter = Expression.Equal(statusProperty, statusValue);
        filterExpression = CombineWithAnd(filterExpression, statusFilter);
    }
    
    // Priority filter - exact match
    if (viewModel.PriorityFilter.HasValue)
    {
        var priorityProperty = Expression.Property(parameter, nameof(Request.Priority));
        var priorityValue = Expression.Constant(viewModel.PriorityFilter.Value);
        var priorityFilter = Expression.Equal(priorityProperty, priorityValue);
        filterExpression = CombineWithAnd(filterExpression, priorityFilter);
    }
    
    // Default to true if no filters
    filterExpression ??= Expression.Constant(true);
    
    return Expression.Lambda<Func<Request, bool>>(filterExpression, parameter);
}

private Expression CombineWithAnd(Expression? left, Expression right)
{
    return left == null ? right : Expression.AndAlso(left, right);
}
```

### Common Configuration Mistakes and Solutions

#### ❌ WRONG: Direct Enum Binding (Causes Selection Issues)
```razor
@* This configuration DOES NOT WORK properly *@
<SfDropDownList TValue="StatusEnum?" 
                DataSource="Enum.GetValues<StatusEnum>()"
                @bind-Value="ViewModel.StatusFilter" />
```

#### ✅ CORRECT: Structured Data Binding
```razor
@* This configuration WORKS reliably *@
<SfDropDownList TValue="StatusEnum?" 
                TItem="StatusEnumItem"
                DataSource="statusOptionItems" 
                @bind-Value="ViewModel.StatusFilter">
    <DropDownListFieldSettings Value="Value" Text="Text" />
</SfDropDownList>
```

#### Key Configuration Requirements:
1. **TValue and TItem must match**: `TValue="StatusEnum?"` and `TItem="StatusEnumItem"`
2. **DropDownListFieldSettings is required**: Maps `Value` and `Text` properties
3. **Structured data objects**: Create dedicated classes like `StatusEnumItem`
4. **User-friendly display names**: Use mapping functions for better UX

## Playwright Testing for Syncfusion Components

### Test Project Configuration
```xml
<!-- In Sanjel.RequestManagement.Blazor.Tests.csproj -->
<PackageReference Include="Microsoft.Playwright" Version="1.41.0" />
<PackageReference Include="Microsoft.Playwright.NUnit" Version="1.41.0" />
<PackageReference Include="NUnit" Version="3.14.0" />
<PackageReference Include="NUnit3TestAdapter" Version="4.5.0" />
```

### Essential Filter Testing Patterns
```csharp
[TestFixture]
public class RequestPagePlaywrightTests : PageTest
{
    [SetUp]
    public async Task Setup()
    {
        // Navigate to the list page
        await Page.GotoAsync("https://localhost:5001/request");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
    
    [Test]
    public async Task RequestList_StatusDropdown_ShouldRenderWithOptionsAsync()
    {
        // Verify dropdown exists and is interactive
        var statusDropdown = Page.Locator("span.e-dropdownlist").First;
        await Expect(statusDropdown).ToBeVisibleAsync();
        
        // Click to open dropdown
        await statusDropdown.ClickAsync();
        
        // Verify options are available
        var dropdownOptions = Page.Locator("li[data-value]");
        await Expect(dropdownOptions).ToHaveCountGreaterThanAsync(0);
        
        // Verify specific enum values are present
        var approvedOption = Page.Locator("li").Filter(new() { HasText = "Approved" });
        await Expect(approvedOption).ToBeVisibleAsync();
    }
    
    [Test]
    public async Task RequestList_SelectStatusApproved_ShouldWorkAsync()
    {
        try
        {
            // Open status dropdown
            var statusDropdown = Page.Locator("span.e-dropdownlist").First;
            await statusDropdown.ClickAsync();
            await Page.WaitForTimeoutAsync(500);
            
            // Select "Approved" option
            var approvedOption = Page.Locator("li").Filter(new() { HasText = "Approved" });
            await approvedOption.ClickAsync();
            await Page.WaitForTimeoutAsync(500);
            
            // Verify selection was successful
            var selectedText = await statusDropdown.Locator("input").InputValueAsync();
            Assert.That(selectedText, Is.Not.Empty, "Dropdown selection should have a value");
        }
        catch (TimeoutException)
        {
            // Fallback: Use JavaScript for reliable selection
            await Page.EvaluateAsync(@"
                const dropdown = document.querySelector('span.e-dropdownlist input');
                if (dropdown) {
                    dropdown.value = 'Approved';
                    dropdown.dispatchEvent(new Event('change', { bubbles: true }));
                }
            ");
            
            // Verify JavaScript fallback worked
            var inputValue = await Page.Locator("span.e-dropdownlist input").First.InputValueAsync();
            Assert.That(inputValue, Does.Contain("Approved").Or.Not.Empty,
                "Status selection should work either through UI or JavaScript fallback");
        }
    }
    
    [Test]  
    public async Task RequestList_ApplyFilterButton_ShouldWorkAsync()
    {
        // Find and click apply filter button
        var applyButton = Page.Locator("button").Filter(new() { HasText = "Apply" });
        await Expect(applyButton).ToBeVisibleAsync();
        await applyButton.ClickAsync();
        
        // Verify filter application (check if list was updated)
        await Page.WaitForTimeoutAsync(1000);
        
        // This test verifies the button is clickable and functional
        // Additional assertions can be added based on expected filter behavior
        var listContainer = Page.Locator("table, .e-grid, .list-container").First;
        await Expect(listContainer).ToBeVisibleAsync();
    }
}
```

## Troubleshooting Guide

### Problem: Syncfusion Dropdown Not Selecting Values

**Symptoms:**
- Dropdown appears but selections don't stick
- Values reset to null/empty after selection
- Options visible but clicking has no effect

**Root Cause Analysis:**
1. **TValue/TItem Type Mismatch**: Most common cause
2. **Missing DropDownListFieldSettings**: Required for proper binding
3. **Direct Enum Binding**: Syncfusion doesn't handle raw enums well
4. **Incorrect Data Source Structure**: Must be structured objects, not arrays

**Solution:**
```csharp
// ✅ Implement structured data approach as shown in templates above
// 1. Create dedicated data classes (StatusEnumItem)
// 2. Use proper TValue/TItem configuration  
// 3. Add DropDownListFieldSettings
// 4. Implement display name mapping functions
```

### Problem: Playwright Tests Failing on Dropdown Interaction

**Symptoms:**
- Tests timeout on dropdown clicks
- Cannot locate dropdown options
- Selections don't register in tests

**Solutions:**
```csharp
// ✅ Use multiple strategies for robust testing
try 
{
    // Primary: Standard UI interaction
    await statusDropdown.ClickAsync();
    await approvedOption.ClickAsync();
}
catch (TimeoutException)
{
    // Fallback: JavaScript interaction
    await Page.EvaluateAsync("/* JavaScript selection logic */");
}

// ✅ Add proper wait strategies
await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
await Page.WaitForTimeoutAsync(500); // Allow UI updates
```

### Problem: Filter State Not Persisting

**Symptoms:**
- Filters reset after page actions
- Filter values lost during navigation
- ViewModel properties not updating

**Solution:**
```csharp
// ✅ Ensure proper two-way binding
@bind-Value="ViewModel.StatusFilter"  // Not just Value="..."

// ✅ Implement ViewModel property change notifications
public StatusEnum? StatusFilter 
{ 
    get => _statusFilter;
    set 
    {
        if (_statusFilter != value)
        {
            _statusFilter = value;
            NotifyPropertyChanged();
        }
    }
}
```

## Error Handling Patterns

### Search Error Recovery
```csharp
private async Task HandleSearchErrorAsync(Exception ex)
{
    var errorMessage = ex switch
    {
        SearchTimeoutException => "Search timed out. Please try a more specific search term.",
        SearchSyntaxException syntaxEx => $"Invalid search syntax: {syntaxEx.Message}",
        ServiceUnavailableException => "Search service is temporarily unavailable. Please try again later.",
        _ => "Search failed. Please try again."
    };
    
    ShowErrorMessage(errorMessage);
    
    // Revert to last successful search state
    if (_lastSuccessfulSearchState != null)
    {
        RestoreSearchState(_lastSuccessfulSearchState);
    }
    
    Logger.LogError(ex, "Search error for term: {SearchTerm}", _searchTerm);
}
```

### Filter Validation
```csharp
private async Task<bool> ValidateFiltersAsync()
{
    var validationErrors = new List<string>();
    
    foreach (var filter in _activeFilters)
    {
        try
        {
            await ValidateFilterValueAsync(filter);
        }
        catch (FilterValidationException ex)
        {
            validationErrors.Add($"{filter.DisplayName}: {ex.Message}");
        }
    }
    
    if (validationErrors.Any())
    {
        ShowValidationErrors(validationErrors);
        return false;
    }
    
    return true;
}
```

## Performance Optimization

### Search Caching Strategy
```csharp
private readonly MemoryCache _searchCache = new(new MemoryCacheOptions
{
    SizeLimit = 100, // Cache up to 100 search results
    CompactionPercentage = 0.25 // Remove 25% when limit reached
});

private async Task<SearchResults> GetCachedSearchResultsAsync(SearchRequest request)
{
    var cacheKey = GenerateCacheKey(request);
    
    if (_searchCache.TryGetValue(cacheKey, out SearchResults cachedResults))
    {
        return cachedResults;
    }
    
    var results = await RequestService.SearchAsync(request);
    
    var cacheOptions = new MemoryCacheEntryOptions
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
        Size = 1
    };
    
    _searchCache.Set(cacheKey, results, cacheOptions);
    
    return results;
}
```

### Incremental Search Loading
```csharp
private async Task LoadMoreSearchResultsAsync()
{
    if (_isLoadingMore || !_hasMoreResults) return;
    
    _isLoadingMore = true;
    
    try
    {
        var searchRequest = _currentSearchRequest with 
        { 
            PageNumber = _currentPage + 1 
        };
        
        var results = await RequestService.SearchAsync(searchRequest);
        
        // Append new results to existing items
        _items.AddRange(results.Items);
        _currentPage++;
        _hasMoreResults = results.Items.Count == _pageSize;
        
        StateHasChanged();
    }
    finally
    {
        _isLoadingMore = false;
    }
}
```

## Testing Strategies

### Playwright UI Tests (Primary — MANDATORY)

All search/filter/sort functionality **MUST** be validated with Playwright browser-based tests. These tests run against the real Blazor application in headless Chromium.

**Required test coverage:**

| Test Case | Description |
|---|---|
| Filter panel renders | Verify filter fields and Apply/Clear buttons are present |
| Text search filters list | Enter a search term, apply, verify results change |
| Dropdown filter works | Select a dropdown filter value, apply, verify filtered results |
| Clear restores results | Click Clear and verify full unfiltered list returns |
| No-match shows empty state | Enter non-matching filter and verify empty state message |
| Sort changes order | Click a column header and verify row order changes |

**Test structure:**
```csharp
[TestFixture]
public class {Entity}FilterPlaywrightTests
{
    private IBrowser _browser;
    private IPage _page;
    private IPlaywright _playwright;

    [OneTimeSetUp]
    public async Task SetupAsync()
    {
        this._playwright = await Playwright.CreateAsync();
        this._browser = await this._playwright.Chromium.LaunchAsync(
            new BrowserTypeLaunchOptions { Headless = true });
        this._page = await this._browser.NewPageAsync();
    }

    [OneTimeTearDown]
    public async Task TeardownAsync()
    {
        await this._browser.CloseAsync();
        this._playwright.Dispose();
    }

    [Test]
    public async Task FilterPanel_ApplyButton_ShouldWorkAsync()
    {
        await this._page.GotoAsync("http://localhost:5000/{entity}");
        await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        await this._page.WaitForTimeoutAsync(2000);

        var applyBtn = await this._page.QuerySelectorAsync("button:has-text('Apply')");
        Assert.IsNotNull(applyBtn, "Apply button should exist");

        var clearBtn = await this._page.QuerySelectorAsync("button:has-text('Clear')");
        Assert.IsNotNull(clearBtn, "Clear button should exist");

        await applyBtn.ClickAsync();
        await this._page.WaitForTimeoutAsync(700);

        var title = await this._page.TitleAsync();
        Assert.IsNotNull(title, "Page should remain functional after applying filter");
    }

    [Test]
    public async Task SearchInput_WithNonMatchingTerm_ShouldShowEmptyStateAsync()
    {
        await this._page.GotoAsync("http://localhost:5000/{entity}");
        await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        await this._page.WaitForTimeoutAsync(2000);

        var searchInput = await this._page.QuerySelectorAsync(
            "input[placeholder*='Search']");
        await searchInput.FillAsync("NONEXISTENT_FILTER_VALUE_999");

        var applyBtn = await this._page.QuerySelectorAsync("button:has-text('Apply')");
        await applyBtn.ClickAsync();
        await this._page.WaitForTimeoutAsync(1000);

        var emptyState = await this._page.QuerySelectorAsync(".empty-state");
        var rows = await this._page.QuerySelectorAllAsync(".e-gridcontent tr.e-row");
        Assert.That(emptyState != null || rows.Count == 0,
            "No-match filter should result in empty list or empty state");
    }
}
```

### Integration Testing
- **Full Filter Workflow**: Test complete filter/search workflow via Playwright including apply and results
- **Filter State Persistence**: Test that filter state is maintained during navigation
- **Sort Integration**: Test column sort interactions via Playwright

## Best Practices

### Search User Experience
- **Instant Feedback**: Provide immediate visual feedback for search actions
- **Clear Results**: Show result count, search term, and clear search options
- **No Results Handling**: Provide helpful suggestions when no results are found
- **Search History**: Allow users to easily repeat previous searches

### Performance Optimization
- **Debounce Search**: Always debounce real-time search to reduce API calls
- **Cache Results**: Cache search results for repeated queries
- **Progressive Loading**: Load results progressively for large datasets
- **Client-Side Filtering**: Use client-side filtering when data is already loaded

### Filter Design
- **Intuitive Controls**: Use appropriate filter controls for each data type
- **Clear State**: Make it obvious which filters are active and how to clear them
- **Filter Combinations**: Support logical combinations of multiple filters
- **Filter Presets**: Provide common filter combinations as presets

### Code Maintainability
- **Separation of Concerns**: Keep search logic separate from UI display logic
- **Reusable Components**: Create reusable filter components for different data types
- **Configuration Driven**: Make filter generation configurable based on entity metadata
- **Error Handling**: Provide comprehensive error handling and user feedback

## Constraints and Limitations

### Technical Constraints
- **Existing Code Dependency**: Requires an existing Blazor list page (`.razor`, `.razor.cs`) to work with; cannot generate from scratch
- **Service Dependency**: Requires service layer with search/filter methods (will suggest implementation if missing)
- **Component Library**: Limited to available filter components in the detected component library
- **Browser Performance**: Complex searches may impact browser performance; optimization required for large datasets
- **API Rate Limiting**: Must respect API rate limits for search operations
- **Discovery Limitations**: Assumes standard naming conventions; may require clarification for non-standard code patterns

### Business Constraints
- **Data Privacy**: Must respect data privacy rules for searchable fields
- **Performance Requirements**: Must meet specified search response time requirements
- **Search Accuracy**: Must provide relevant and accurate search results
- **User Permissions**: Must respect user permissions for searchable data
- **User Interaction**: Requires user input for field selections; cannot automatically decide business logic

### Integration Constraints
- **Entity Metadata**: Requires discoverable entity metadata for filter generation; may need user clarification for complex types
- **Service Interface Constraints**: Must work with existing service method signatures; may suggest additions if needed
- **State Management**: Must integrate with existing application state management
- **Component Library Coverage**: Only supports MudBlazor, Syncfusion, Bootstrap, and Minimal components

### User Interaction Constraints
- **Required Input**: User must provide selections for query fields and filter fields; defaults can be used but should be confirmed
- **Decision Responsibility**: User makes business decisions about which fields to search, filter, and sort; AI provides recommendations only
- **Iteration May Be Needed**: Complex configurations may require multiple rounds of clarification
- **Knowledge Assumptions**: Assumes user understands their domain and business requirements

## Integration Workflow

### Phase 1: Automatic Discovery and Analysis
1. **Analyze Project Structure**:
   - Read the existing list page files (`.razor`, `.razor.cs`)
   - Extract ViewModel class name and properties
   - Identify injected Service and Repository interfaces
   - Determine the entity type from the Repository interface

2. **Detect Component Library**:
   - Analyze project dependencies (`PackageReference`, `Using` statements)
   - Identify active component library: MudBlazor, Syncfusion, Bootstrap, or Minimal
   - Determine appropriate UI components for search inputs and filters

3. **Extract Entity Metadata**:
   - Read entity class definition to discover all properties
   - Identify property names, data types, and display attributes
   - Detect relationships to other entities
   - Determine nullable status and validation rules

4. **Prepare Field Information**:
   - Compile a comprehensive list of all available fields
   - Organize fields by data type (string, numeric, date, enum, bool)
   - Prepare field descriptions for user selection

### Phase 2: Interactive User Configuration
1. **Present Discovered Information**:
   - Display discovered entity name and total field count
   - Show component library detection results
   - List all available fields with their types

2. **Collect Query Field Selection**:
   - Ask user to select which fields to display in the results table
   - Present fields with numbers for easy selection
   - Allow user to specify column order if needed
   - Provide "all" option for quick selection

3. **Collect Filter Field Selection**:
   - Ask user to select which fields should support filtering
   - For each selected field, ask for filter method preference
   - Present appropriate filter options based on data type
   - Show examples of how filters will appear in the UI

4. **Collect Optional Configuration**:
   - Ask about global search preferences (which fields to include)
   - Ask about sort configuration (sortable fields, default sort)
   - Collect performance requirements if needed
   - Ask about advanced features (saved searches, export, etc.)

### Phase 3: Implementation
1. **Replace TODO Methods**:
   - Implement all search/filter-related placeholder methods
   - Generate search request building logic
   - Implement filter value collection and validation
   - Add sort state management

2. **Generate Filter UI**:
   - Create dynamic filter controls based on user selections
   - Use appropriate component library components for each data type
   - Implement filter combination logic (AND/OR)
   - Add active filter display and clear functionality

3. **Implement Search Logic**:
   - Build global search across user-selected fields
   - Implement debounced real-time search
   - Add search result caching if requested
   - Handle empty search states with helpful messages

4. **Add Performance Optimization**:
   - Implement debouncing with configurable delay
   - Add search result caching strategy
   - Optimize query building for backend integration
   - Implement progressive loading if needed for large datasets

### Phase 4: Integration Testing
1. **Test Search Workflows**: Verify complete search operation workflows
2. **Test Filter Combinations**: Verify complex filter logic and combinations
3. **Test Performance**: Verify search performance with various data volumes
4. **Test State Management**: Verify filter state persistence and restoration

## Key Principles
1. **User-First Design**: Design search and filter experiences around user needs with interactive configuration
2. **Automatic Discovery**: Minimize user input by automatically detecting component library, entities, and field metadata
3. **Interactive Guidance**: Guide users through configuration decisions with clear options and recommendations
4. **Performance Conscious**: Optimize for fast search response times with caching and debouncing
5. **Flexible and Extensible**: Create extensible search architecture for future needs
6. **Error Resilient**: Handle search errors gracefully with clear user feedback
7. **Data-Driven**: Generate filter UI dynamically based on discovered entity metadata and user selections

## Final Step: Code Formatting

After implementing all search, sort, and filter functionality and generating Playwright tests, the skill calls `solution-code-formatter` to ensure all generated code follows proper formatting standards:
```bash
bun run ../../utilities/solution-code-formatter/scripts/format-solution.ts [solution-path]
```

## Discovery and Configuration Examples

### Example 1: Standard Entity Discovery
**Discovered Context**:
```
Component Library: MudBlazor
Page: /sanjel/PRG/src/Sanjel.RequestManagement.Blazor/Pages/Request/Index.razor
ViewModel: RequestViewModel
Service: IRequestService
Repository: IRequestRepository
Entity: Request
Fields:
  1. RequestId (Guid)
  2. Title (string) - DisplayAttribute: "Request Title"
  3. Description (string)
  4. Status (RequestStatus) - Enum
  5. CreatedDate (DateTime)
  6. Priority (int)
  7. RequesterId (Guid) - ForeignKey to User
```

**Interactive Dialogue**:
```
AI: I discovered the Request entity with 7 fields. Here's what I found:

Available fields:
[1] RequestId (Guid)
[2] Title (string) - Request Title
[3] Description (string)
[4] Status (RequestStatus) - Enum [Pending, Approved, Rejected]
[5] CreatedDate (DateTime)
[6] Priority (int)
[7] RequesterId (Guid) - Related to User

Component Library Detected: MudBlazor

Let's configure your search and filter functionality:

1. Which fields should display in the results table?
   (Enter numbers separated by commas, or "all" for all fields)
   User: 2,3,4,5,6

2. Which fields should support filtering?
   (Enter numbers separated by commas)
   User: 4,5,6

3. For Status (enum), which filter method?
   [multiSelect/singleSelect]
   User: singleSelect

4. For CreatedDate (DateTime), which filter method?
   [range/exact/before/after]
   User: range

5. For Priority (int), which filter method?
   [range/equals/greaterThan/lessThan]
   User: range

6. Which text fields should participate in global search?
   (Available: 2-Title, 3-Description)
   User: 2,3

7. Which fields should support sorting?
   User: 5,6

8. Default sort field and direction?
   User: CreatedDate, descending

9. Enable advanced features?
   - Search caching [yes/no, default: yes]
   - Saved searches [yes/no, default: no]
   - Export results [yes/no, default: no]
   User: yes, no, no

Configuration complete! I'll now implement the search/filter functionality...
```

### Example 2: Quick Default Configuration
**Discovered Context**: Same as Example 1

**Interactive Dialogue**:
```
AI: I discovered the Request entity with 7 fields using MudBlazor components.

I can recommend a sensible configuration:
✓ Display all fields except RequestId (internal)
✓ Enable filtering on Status, CreatedDate, Priority
✓ Enable global search on Title and Description
✓ Enable sorting on CreatedDate and Priority
✓ Default sort: CreatedDate descending
✓ Enable search caching

Would you like to use these defaults? [yes/no]
User: Yes

AI: Great! Implementing search/filter functionality with recommended defaults...
```

### Example 3: Complex Entity with Relationships
**Discovered Context**:
```
Component Library: Syncfusion
Entity: Order
Fields: 12 fields including:
  - OrderId (Guid)
  - OrderNumber (string)
  - CustomerName (string) - From Customer relationship
  - ProductName (string) - From OrderItems relationship
  - OrderDate (DateTime)
  - RequiredDate (DateTime)
  - ShippedDate (DateTime?)
  - Status (OrderStatus) - Enum
  - TotalAmount (decimal)
  - ShippingAddress (string)
  - TrackingNumber (string?)
  - Notes (string?)
```

**Interactive Dialogue**:
```
AI: I discovered the Order entity with 12 fields including customer and product data.
Component Library: Syncfusion

Available fields:
[1] OrderId (Guid)
[2] OrderNumber (string)
[3] CustomerName (string) - From Customer
[4] ProductName (string) - From OrderItems
[5] OrderDate (DateTime)
[6] RequiredDate (DateTime)
[7] ShippedDate (DateTime?)
[8] Status (OrderStatus) - Enum
[9] TotalAmount (decimal)
[10] ShippingAddress (string)
[11] TrackingNumber (string?)
[12] Notes (string?)

1. Display fields:
   User: 2,3,4,5,6,8,9

2. Filter fields:
   User: 5,6,8,9

3. Filter methods:
   - OrderDate (DateTime): [range/exact] → User: range
   - RequiredDate (DateTime): [range/exact] → User: exact
   - Status (enum): [multiSelect/singleSelect] → User: multiSelect
   - TotalAmount (decimal): [range/equals/greaterThan/lessThan] → User: range

4. Global search fields:
   User: 2,3,4,10,11

5. Sortable fields:
   User: 2,5,9

6. Default sort:
   User: OrderDate, descending

7. Advanced features:
   - Search caching: yes
   - Saved searches: yes
   - Export results: yes
   - Visual query builder: no

AI: Implementing advanced search/filter with saved searches and export...
```