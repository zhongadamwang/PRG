---
name: blazor-list-filter-generator
description: Generate comprehensive search, sort, and filter functionality for Blazor list pages with advanced query building and real-time filtering.
---

# Blazor List Filter Generator

**Responsibility**: Implement complete search, sort, and filter functionality for list pages in Blazor applications
**Input**: List page structure from blazor-list-pattern-generator + Entity metadata + Search/filter requirements
**Output**: Complete search/filter implementation with query building, real-time filtering, and advanced search capabilities

**Approach**: **AI-Driven Search & Filter Implementation**
- Takes existing list page structure with TODO placeholders
- Implements search input handlers with debouncing and real-time filtering
- Builds dynamic filter UI components based on entity properties
- Generates query building logic for complex search conditions
- Handles sort operations with multi-column sorting support
- **Output**: Working search/filter functionality with performance optimization

## Description

This skill acts as a **Search & Filter Operations Specialist** for Blazor list pages. It implements the complete search and filter functionality by:
- Taking the structural framework from `blazor-list-pattern-generator`
- Implementing the TODO-marked search and filter operation placeholders
- Adding real-time search with debouncing and performance optimization
- Creating dynamic filter UI based on entity property types
- Building efficient query logic for backend data filtering
- Supporting advanced search patterns and saved search functionality

**DEPENDENCY**: This skill **REQUIRES** output from `blazor-list-pattern-generator` as input. It cannot work in isolation.

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

### Required Inputs
- **List Page Structure**: Complete list page structure from `blazor-list-pattern-generator`
- **Entity Metadata**: JSON metadata containing searchable properties and data types
- **Service Interface**: Service interface containing search/filter method signatures
- **Search Requirements**: Global search fields, filter types, sort options

### Optional Inputs
- **Advanced Search Requirements**: Complex query builders, saved searches, export capabilities
- **Performance Constraints**: Expected data volume, search response time requirements
- **Internationalization**: Multi-language search support requirements
- **Analytics Requirements**: Search behavior tracking and reporting needs
- **UI Customization**: Custom filter controls, search result highlighting preferences

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

### Scenario 1: Simple Global Search
**Input**: Basic entity with text properties for simple searching  
**Output**: Global search box with real-time filtering and highlighting  
**Features**: 
- ✅ Debounced search input with loading indicator
- ✅ Search across multiple text fields (title, description, etc.)
- ✅ Clear search button and search term display
- ✅ Result count and "no results" messaging

### Scenario 2: Advanced Multi-Filter Interface
**Input**: Complex entity with various data types and relationships  
**Output**: Multiple filter controls with complex query building  
**Features**:
- ✅ Property-specific filter controls (date ranges, dropdowns, etc.)
- ✅ Filter combination logic (AND/OR between filters)
- ✅ Active filter display with individual clear buttons
- ✅ Filter preset options for common search scenarios

### Scenario 3: High-Performance Large Dataset Search
**Input**: Entity with 10K+ records requiring efficient search  
**Output**: Optimized search with server-side processing and caching  
**Features**:
- ✅ Server-side search processing with pagination
- ✅ Search result caching and intelligent prefetching
- ✅ Progressive loading and virtual scrolling integration
- ✅ Search performance monitoring and optimization

### Scenario 4: Advanced Query Builder
**Input**: Power users needing complex search capabilities  
**Output**: Visual query builder with saved searches and export  
**Features**:
- ✅ Drag-and-drop query builder interface
- ✅ Nested AND/OR condition groups
- ✅ Saved search management with naming and sharing
- ✅ Query export and import functionality

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

### Search functionality Testing
```csharp
[Test]
public async Task SearchAsync_WithValidTerm_ReturnsFilteredResults()
{
    // Arrange
    var searchTerm = "test";
    var expectedResults = CreateTestSearchResults();
    _service.Setup(s => s.SearchAsync(It.IsAny<SearchRequest>()))
           .ReturnsAsync(expectedResults);
    
    // Act
    _component.SearchTerm = searchTerm;
    await _component.ExecuteSearchAsync();
    
    // Assert
    Assert.That(_component.Items.Count, Is.EqualTo(expectedResults.Items.Count));
    _service.Verify(s => s.SearchAsync(It.Is<SearchRequest>(r => 
        r.Term == searchTerm)), Times.Once);
}

[Test]
public async Task SearchAsync_WithDebouncing_DelaysSearchExecution()
{
    // Arrange
    var component = CreateComponentWithDebouncing(300); // 300ms delay
    
    // Act
    component.SearchTerm = "a";
    await Task.Delay(100);
    component.SearchTerm = "ab";
    await Task.Delay(100);  
    component.SearchTerm = "abc";
    await Task.Delay(400); // Wait for debounce
    
    // Assert
    _service.Verify(s => s.SearchAsync(It.IsAny<SearchRequest>()), Times.Once);
    _service.Verify(s => s.SearchAsync(It.Is<SearchRequest>(r => 
        r.Term == "abc")), Times.Once);
}
```

### Filter Integration Testing
- **Filter Combination Testing**: Test AND/OR combinations between different filters
- **Filter Performance Testing**: Test filter performance with large datasets
- **Filter State Persistence**: Test save/restore of filter states
- **Filter Validation**: Test filter input validation and error handling

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
- **Service Dependency**: Requires service layer with search/filter methods
- **Component Library**: Limited to available filter components in the project
- **Browser Performance**: Complex searches may impact browser performance
- **API Rate Limiting**: Must respect API rate limits for search operations

### Business Constraints
- **Data Privacy**: Must respect data privacy rules for searchable fields
- **Performance Requirements**: Must meet specified search response time requirements
- **Search Accuracy**: Must provide relevant and accurate search results
- **User Permissions**: Must respect user permissions for searchable data

### Integration Constraints
- **List Structure Dependency**: Can only work with list pages from blazor-list-pattern-generator
- **Entity Metadata Dependency**: Requires comprehensive entity metadata for filter generation
- **Service Interface Constraints**: Must work with existing service method signatures
- **State Management**: Must integrate with existing application state management

## Integration Workflow

### Phase 1: Structure Analysis
1. **Analyze List Structure**: Review list page structure from blazor-list-pattern-generator
2. **Identify Search Placeholders**: Find all search/filter-related TODO markers
3. **Review Entity Metadata**: Understand searchable properties and data types
4. **Assess Search Requirements**: Determine complexity and performance needs

### Phase 2: Implementation
1. **Replace TODO Methods**: Implement all search/filter-related placeholder methods
2. **Generate Filter UI**: Create dynamic filter controls based on entity properties
3. **Implement Query Building**: Add query building logic for backend integration
4. **Add Performance Optimization**: Implement debouncing, caching, and progressive loading

### Phase 3: Integration Testing
1. **Test Search Workflows**: Verify complete search operation workflows
2. **Test Filter Combinations**: Verify complex filter logic and combinations
3. **Test Performance**: Verify search performance with various data volumes
4. **Test State Management**: Verify filter state persistence and restoration

## Key Principles
1. **User-First Design**: Design search and filter experiences around user needs
2. **Performance Conscious**: Optimize for fast search response times
3. **Flexible and Extensible**: Create extensible search architecture for future needs
4. **Error Resilient**: Handle search errors gracefully with clear user feedback
5. **Data-Driven**: Generate filter UI dynamically based on entity metadata