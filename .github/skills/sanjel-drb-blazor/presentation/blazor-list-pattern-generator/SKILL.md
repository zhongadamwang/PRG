---
name: blazor-list-pattern-generator
description: AI-driven list pattern generator for Blazor pages with comprehensive data table design, search, sort, filter, pagination, and batch operations support.
---

# Blazor List Pattern Generator

**Responsibility**: Generate business logic patterns and UI implementations for list pages in Blazor applications
**Input**: Entity metadata + ViewModel definitions + UI/UX requirements + Performance constraints
**Output**: Complete list page implementation with data table, search, sort, filter, pagination, and batch operations

**Approach**: **AI-Driven List Pattern Architecture**
- Analyzes entity structure and ViewModel properties for optimal data table design
- Recommends appropriate component library components (MudBlazor, Syncfusion, etc.)
- Implements comprehensive search, sort, and filter functionality
- Designs efficient pagination and virtual scrolling for large datasets
- Provides batch operation patterns (batch delete, export, etc.)
- Ensures responsive design and accessibility compliance
- **Output**: Strategic list pattern guidance + complete implementation code

## Description

**Testing Requirement — MANDATORY:**
After generating any list page structure, you **MUST** also generate corresponding **Playwright UI tests (C#, NUnit)** in the project's existing Blazor test project (e.g. `src/Sanjel.RequestManagement.Blazor.Tests/`). These tests are a required output deliverable — the skill implementation is NOT complete until the tests are generated.

**What to test (Playwright):**
- List page loads and renders without errors
- Data table columns are visible with correct headers
- Pagination controls (page numbers, page size selector) are present
- Filter panel and its input fields render on the page
- Action buttons (New, Edit, Delete) are visible in the correct locations
- Loading state is shown while data is being fetched

**Test file conventions:**
- Add tests to the existing Playwright test file or create a new test class following the naming pattern `{Entity}ListPlaywrightTests.cs`
- Use the same setup/teardown pattern as existing Playwright tests (see `RequestPagePlaywrightTests.cs` for reference)
- Use NUnit `[TestFixture]` / `[Test]` attributes
- Use `Microsoft.Playwright` for browser automation
- Tests must be headless-compatible

This skill acts as a **List Page Structure Architect** specializing in list page layout and component integration for Blazor applications. It provides **STRUCTURAL ONLY** guidance for creating list pages with data tables, search/filter UI placeholders, pagination controls, and action button frameworks.

**Component Library Auto-Detection**: This skill **automatically detects and adapts to the current project's component library** (MudBlazor, Syncfusion, Bootstrap, etc.) by analyzing project dependencies and existing code patterns to generate appropriate component usage.

**IMPORTANT**: This skill does **NOT** implement business logic for search, filter, create, edit, or delete operations. These require specialized skills:
- Search/Filter Logic → `blazor-list-filter-generator`
- Create/Add Logic → `blazor-list-add-generator`
- Edit/Modify Logic → `blazor-list-modify-generator`
- Delete Logic → `blazor-list-delete-generator`

## Responsibility

Generate **STRUCTURAL FRAMEWORK ONLY** for list pages, including:
- ✅ Data table layout and component integration
- ✅ Search/filter UI placeholders (NO logic implementation)
- ✅ Pagination controls structure
- ✅ Action button frameworks (Create/Edit/Delete buttons without handlers)
- ✅ Batch operation UI structure
- ✅ Responsive design and accessibility compliance

**EXPLICITLY EXCLUDED**:
- ❌ Search/filter business logic implementation (requires `blazor-list-filter-generator`)
- ❌ Create/add operation implementation (requires `blazor-list-add-generator`)
- ❌ Edit/modify operation implementation (requires `blazor-list-modify-generator`)
- ❌ Delete operation implementation (requires `blazor-list-delete-generator`)

## AI Persona

**Role**: Senior List Page Structure Architect & Component Integration Specialist  
**Expertise**: 12+ years experience in enterprise list page layout design and component architecture  
**Specializations**:
- Data table structure and responsive layout design
- UI component placement and visual hierarchy
- Search/filter/sort control **UI frameworks** (NO logic)
- Pagination and action button **structure** (NO handlers)
- Batch operation **UI patterns** (NO implementation)
- Responsive design for data-heavy interfaces
- Accessibility compliance for complex tables
- Component library integration (MudBlazor, Syncfusion, Bootstrap)

**CRITICAL LIMITATION**: 
This persona provides **STRUCTURAL ARCHITECTURE ONLY**. All business logic implementations must be delegated to specialized skills:
- 🚫 **Search/Filter Logic** → Refer to `blazor-list-filter-generator` skill
- 🚫 **Create/Add Logic** → Refer to `blazor-list-add-generator` skill
- 🚫 **Edit/Modify Logic** → Refer to `blazor-list-modify-generator` skill
- 🚫 **Delete Logic** → Refer to `blazor-list-delete-generator` skill

## Input Parameters

### Required Inputs
- **Entity Metadata**: JSON metadata from domain-model-parser containing entity structure
- **ViewModel Definition**: ViewModel class with display properties and validation rules
- **Service Interface**: Service interface for data access operations
- **Component Library**: Target component library (MudBlazor, Syncfusion, Bootstrap, Minimal)

### Optional Inputs
- **Search Requirements**: Search field specifications (which properties to search, search types)
- **Sort Requirements**: Default sort order, sortable columns, multi-sort support
- **Filter Requirements**: Filter columns, filter types (dropdown, date range, etc.)
- **Pagination Requirements**: Page size options, pagination style
- **Batch Operations**: Required batch operations (delete, export, status change, etc.)
- **Performance Constraints**: Expected data volume, response time requirements
- **Responsive Design Needs**: Mobile-first or desktop-first, breakpoint strategies

## Output Deliverables

### ⚠️ IMPORTANT: Structure-Only Output

This skill provides **STRUCTURAL FRAMEWORK ONLY**. All business logic implementations are marked as TODO placeholders requiring specialized skills.

### Structural Architecture Guidance (PROVIDED)

1. **Data Table Design Structure**
   - Column selection and ordering recommendations
   - Column width and responsive behavior guidelines
   - Display format and component selection strategies
   - Virtual scrolling vs pagination structure recommendations
   - **NOTE**: Actual data loading logic requires service layer implementation

2. **Search, Sort, Filter UI Framework**
   - Search input UI placement and design patterns
   - Sort UI indicators and button placement
   - Filter UI components and layout strategies
   - **EXPLICIT LIMITATION**: NO search/filter logic implementation
   - **REQUIRED**: `blazor-list-filter-generator` for actual functionality

3. **Pagination and Performance Structure**
   - Pagination component selection and placement
   - Page size options UI framework
   - Virtual scrolling structure guidelines
   - **EXPLICIT LIMITATION**: NO data loading or pagination logic
   - **REQUIRED**: Service layer must provide pagination support

4. **Batch Operation UI Framework**
   - Row selection UI patterns (checkboxes, row selection)
   - Batch action button placement and design
   - Confirmation dialog UI structure
   - **EXPLICIT LIMITATION**: NO batch operation handlers
   - **REQUIRED**: `blazor-list-delete-generator` for delete operations

### Implementation Code (STRUCTURE ONLY)

1. **Complete List Page (.razor)** - STRUCTURAL ONLY
   - Data table component integration
   - Search, sort, filter control **placeholders**
   - Pagination controls **structure**
   - Batch operation buttons **without handlers**
   - Responsive layout
   - **ALL ACTION HANDLERS ARE EMPTY STUBS WITH TODO COMMENTS**

2. **Business Logic (.razor.cs)** - ARCHITECTURE PATTERNS ONLY
   - Method stubs with `NotImplementedException`
   - TODO comments indicating required skills
   - Service injection structure
   - **NO ACTUAL BUSINESS LOGIC IMPLEMENTATION**
   - **EXAMPLE**:
   ```csharp
   // TODO: [blazor-list-filter-generator] Implement search logic
   private async Task SearchAsync()
   {
       throw new NotImplementedException("Requires blazor-list-filter-generator skill");
   }
   
   // TODO: [blazor-list-add-generator] Implement create navigation
   private async Task CreateAsync()
   {
       throw new NotImplementedException("Requires blazor-list-add-generator skill");
   }
   
   // TODO: [blazor-list-modify-generator] Implement edit logic
   private async Task EditAsync(int id)
   {
       throw new NotImplementedException("Requires blazor-list-modify-generator skill");
   }
   
   // TODO: [blazor-list-delete-generator] Implement delete logic
   private async Task DeleteAsync(int id)
   {
       throw new NotImplementedException("Requires blazor-list-delete-generator skill");
   }
   ```

3. **Styles and Theme** - PROVIDED
   - Table styling
   - Status badge styles
   - Responsive breakpoints
   - Accessibility enhancements

## Functional Capabilities

### Core List Pattern Architecture
- **Data Table Integration**: Select optimal data table component based on requirements
- **Column Configuration**: Configure columns with appropriate display formats and validation
- **Responsive Design**: Ensure table works across all screen sizes
- **Accessibility**: Support keyboard navigation, screen readers, ARIA labels

### Search, Sort, Filter Patterns
- **Global Search**: Single search box across multiple columns
- **Column-Specific Search**: Individual column filters
- **Multi-Column Sort**: Support sorting by multiple columns
- **Advanced Filtering**: Date ranges, dropdown filters, custom filters
- **Query Persistence**: Save and restore search/filter states

### Pagination and Performance
- **Server-Side Pagination**: Efficient data loading for large datasets
- **Client-Side Pagination**: Fast navigation for smaller datasets
- **Virtual Scrolling**: Handle 10K+ rows efficiently
- **Infinite Scroll**: Load more data as user scrolls
- **Prefetching**: Load next page before user navigates

### Batch Operations
- **Row Selection**: Checkboxes for multi-row selection
- **Select All**: Select all visible or all records
- **Batch Actions**: Delete, Export, Status Change, etc.
- **Progress Indicators**: Show operation progress
- **Error Handling**: Partial success handling and rollback

## Integration Points

### Dependencies
- **domain-model-parser**: Provides entity metadata for column configuration
- **blazor-viewmodel-generator**: Provides ViewModel with display properties
- **service-method-generator**: Provides service methods for data access (**structure only - this skill does NOT implement logic**)
- **blazor-architecture-generator**: Determines component library selection
- **component-library-generators**: Provide specific component library guidance

### Generated Artifact Integration (STRUCTURAL INTEGRATION)

**IMPORTANT**: This skill generates **STRUCTURAL INTEGRATION POINTS ONLY**. Actual business logic integration requires specialized skills.

- **ViewModel Classes**: Use ViewModel properties for table column **display definitions**
- **Service Interfaces**: Inject service interfaces for **future** data operations
  - Method calls are placeholders with `NotImplementedException`
  - All handlers require subsequent skill implementations
- **Component Library**: Integrate specific library components (MudTable, SfGrid, etc.) for **layout only**
- **Modal Dialogs**: Use dialog components for batch operation **UI structure**
- **Navigation**: Integrate with app routing for detail/edit actions (**placeholders only**)
- **Skill Handoff Points**: Clear markers for:
  - `blazor-list-filter-generator` → Search/filter method implementations
  - `blazor-list-add-generator` → Create action handlers
  - `blazor-list-modify-generator` → Edit action handlers
  - `blazor-list-delete-generator` → Delete action handlers

## Usage Scenarios

### Scenario 1: Basic List Structure (Structure Only)
**Input**: Simple entity with 5-10 properties, expected 100-1000 records  
**Output**: Clean data table **structure** with search/sort/pagination **placeholders**  
**Features**: 
- ✅ Global search UI placeholder (NO logic - requires `blazor-list-filter-generator`)
- ✅ Column sorting UI framework (NO logic - requires `blazor-list-filter-generator`)
- ✅ Standard pagination structure (NO logic - requires service layer)
- ❌ NO batch operations (requires `blazor-list-delete-generator`)

**NOTE**: This output is a structural template requiring additional skills for functionality.

### Scenario 2: Advanced List Structure (Structure Only)
**Input**: Complex entity with relationships, expected 1000+ records  
**Output**: Full-featured data table **UI framework** with advanced filtering and batch action placeholders  
**Features**:
- ✅ Column-specific filter UI placeholders (NO logic - requires `blazor-list-filter-generator`)
- ✅ Multi-column sort UI indicators (NO logic - requires `blazor-list-filter-generator`)
- ✅ Batch delete/export UI buttons (NO handlers - requires `blazor-list-delete-generator`)
- ✅ Status change buttons (NO handlers - requires `blazor-list-modify-generator`)

**NOTE**: All functionality requires subsequent skill implementations.

### Scenario 3: Large Dataset Structure (Structure Only)
**Input**: Large entity, expected 10K+ records, performance-critical  
**Output**: High-performance data table **layout** with virtual scrolling **framework**  
**Features**:
- ✅ Virtual scrolling structure (implementation requires service support)
- ✅ Lazy loading framework (logic requires service layer)
- ✅ Debounced search UI (logic requires `blazor-list-filter-generator`)
- ✅ Optimized rendering patterns

**NOTE**: Performance optimization requires service layer collaboration.

### Scenario 4: Mobile-First Responsive Structure (Structure Only)
**Input**: Entity with many columns, mobile users primary target  
**Output**: Responsive table **layout** with card view on mobile  
**Features**:
- ✅ Responsive breakpoints implementation
- ✅ Card layout on mobile
- ✅ Swipe action UI placeholders (logic requires other skills)
- ✅ Touch-friendly button placement

**NOTE**: Interaction logic requires additional skill implementations.

## AI Consultation Approach

### Analysis Phase (STRUCTURE FOCUS)
1. **Entity and ViewModel Review**: Analyze entity structure for **layout planning**
2. **Data Volume Assessment**: Evaluate expected record count for **component selection**
3. **User Experience Analysis**: Understand user workflows for **UI placement**
4. **Component Library Detection**: Identify available component library in project
5. **Skill Dependency Mapping**: Identify which specialized skills are needed for:
   - Search/filter complexity → `blazor-list-filter-generator`
   - Create operation needs → `blazor-list-add-generator`
   - Edit operation needs → `blazor-list-modify-generator`
   - Delete operation needs → `blazor-list-delete-generator`

### Design Phase (ARCHITECTURAL PATTERNS ONLY)
1. **Data Table Configuration**: Select columns, define widths, set display formats
2. **Search/Sort/Filter UI Design**: Choose UI patterns and control placement (**NO logic design**)
3. **Pagination Strategy**: Decide between server-side vs client-side **structure**
4. **Batch Operation UI Design**: Design selection UI and action button frameworks (**NO handler design**)
5. **Skill Integration Planning**: Define clear integration points for subsequent skills

### Implementation Guidance (STRUCTURAL INSTRUCTIONS)
1. **Step-by-Step Structure Implementation**: Provide detailed layout implementation instructions
2. **Code Examples with TODO Markers**: Generate structural code examples with clear skill dependency comments
3. **Performance Best Practices**: Share optimization strategies for large datasets (**structural only**)
4. **Accessibility Guidelines**: Ensure WCAG compliance for complex tables
5. **Skill Handoff Documentation**: Provide clear instructions for next-phase skills

## Best Practices Enforcement

### Data Table Design Standards
- **Performance-First**: Optimize for large datasets (1000+ records)
- **User-Centric**: Design around common user workflows and patterns
- **Accessibility-First**: Ensure keyboard navigation and screen reader support
- **Responsive-Design**: Test on all screen sizes and orientations

### Search, Sort, Filter Best Practices
- **Intuitive UX**: Use familiar patterns (magnifying glass for search, arrows for sort)
- **Real-Time Feedback**: Show loading states and result counts
- **Query Persistence**: Save search/sort state for back navigation
- **Debounced Search**: Delay search to reduce server load

### Pagination Best Practices
- **Consistent Page Sizes**: Use standard page sizes (10, 25, 50, 100)
- **Jump Navigation**: Allow direct page number input for large datasets
- **Total Count Display**: Show total records and current position
- **Prefetching**: Load next page before user navigates

## Performance Optimization Strategies

### Data Loading Optimization
- **Server-Side Processing**: Offload sorting/filtering to database for large datasets
- **Lazy Loading**: Load data only when needed (pagination, virtual scrolling)
- **Debouncing**: Delay search/filter operations to reduce API calls
- **Caching**: Cache frequently accessed data and query results

### Rendering Optimization
- **Virtual Scrolling**: Render only visible rows (use MudBlazor's MudTable)
- **ShouldRender Override**: Prevent unnecessary re-renders
- **Memoization**: Cache expensive computations
- **Async Rendering**: Use async patterns to prevent UI blocking

### Memory Management
- **Dispose Subscriptions**: Clean up event subscriptions properly
- **Weak References**: Use weak references for large cached data
- **Pagination Limits**: Limit max page size to prevent memory issues

## Component Library Integration

### MudBlazor Integration (Recommended)
- **MudTable**: Primary data table component with built-in pagination
- **MudDataGrid**: Advanced grid with filtering, sorting, grouping
- **MudTextField**: Search input with icon and clear button
- **MudSelect**: Dropdown filters for enum columns
- **MudDatePicker**: Date range filters
- **MudCheckBox**: Row selection checkboxes
- **MudButton**: Batch operation buttons
- **MudMenu**: Action menu with dropdown options

### Syncfusion Integration (Enterprise)
- **SfGrid**: Enterprise-grade data grid with advanced features
- **SfTextBox**: Search input with autocomplete
- **SfDatePicker**: Date range picker
- **SfDropDownList**: Enum and dropdown filters
- **SfCheckBox**: Row selection
- **SfButton**: Batch operation buttons

### Bootstrap Integration (Simple)
- **Bootstrap Table**: Basic table with Bootstrap styling
- **Form Input**: Search and filter inputs
- **Pagination**: Bootstrap pagination component
- **Modal**: Batch operation confirmations

## Error Handling and User Experience

### Loading States
- **Initial Load**: Show skeleton loader or spinner
- **Pagination Load**: Show loading indicator in footer
- **Search/Filter**: Show debouncing indicator
- **Batch Operations**: Show progress bar for long operations

### Error States
- **Data Load Error**: Show friendly error message with retry button
- **Network Error**: Show offline indicator
- **Validation Error**: Highlight invalid filters
- **Batch Operation Error**: Show partial success details with retry options

### Empty States
- **No Records**: Show friendly illustration and "Create First" button
- **No Search Results**: Show "No results found for [query]" and clear filters button
- **Empty Selection**: Disable batch operation buttons with tooltip

## Testing and Quality Assurance

### List Page Testing Strategies (Playwright — MANDATORY)

All generated list page structure **MUST** be validated with Playwright browser-based tests. These tests run against the real Blazor application in headless Chromium.

**Required test coverage:**

| Test Case | Description |
|---|---|
| Page loads | Verify list page navigates and renders without errors |
| Table columns visible | Verify all expected column headers are present |
| Filter panel renders | Verify filter inputs and Apply/Clear buttons exist |
| Action buttons present | Verify New/Create button is visible |
| Pagination controls present | Verify page controls are rendered |

**Test structure:**
```csharp
[TestFixture]
public class {Entity}ListPlaywrightTests
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
    public async Task ListPage_ShouldLoadWithDataGridAsync()
    {
        await this._page.GotoAsync("http://localhost:5000/{entity}");
        await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        await this._page.WaitForTimeoutAsync(2000);

        var grid = await this._page.QuerySelectorAsync(
            ".e-grid, table, [role='grid']");
        Assert.IsNotNull(grid, "Data grid should be visible on the list page");
    }

    [Test]
    public async Task ListPage_ShouldRenderFilterPanelAsync()
    {
        await this._page.GotoAsync("http://localhost:5000/{entity}");
        await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        await this._page.WaitForTimeoutAsync(2000);

        var filterPanel = await this._page.QuerySelectorAsync(".filter-panel");
        Assert.IsNotNull(filterPanel, "Filter panel should be visible");
    }

    [Test]
    public async Task ListPage_ShouldHaveCreateButtonAsync()
    {
        await this._page.GotoAsync("http://localhost:5000/{entity}");
        await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        await this._page.WaitForTimeoutAsync(2000);

        var createBtn = await this._page.QuerySelectorAsync(
            ".e-primary, button:has-text('New'), button:has-text('Create')");
        Assert.IsNotNull(createBtn, "Create/New button should be visible");
    }
}
```

### Responsive Design Testing
- **Mobile Testing**: Test on actual mobile devices (320px - 414px)
- **Tablet Testing**: Test on tablet sizes (768px - 1024px)
- **Desktop Testing**: Test on desktop (1280px - 4K)
- **Orientation Testing**: Test portrait and landscape modes

## Constraints and Limitations

### ⚠️ **MANDATORY CONSTRAINTS - DO NOT VIOLATE**

#### 1. **Scope Limitations - Structure Only**
This skill is **STRICTLY LIMITED** to generating list page structure and architecture patterns. It does **NOT** implement detailed business logic for specific operations.

**PROHIBITED OPERATIONS - This skill DOES NOT implement:**

##### ❌ Search/Filter Logic Implementation
- **Cannot implement**: Actual search/filter business logic, query building, or data filtering algorithms
- **Cannot implement**: Filter condition validation, complex query composition, or dynamic LINQ generation
- **Reason**: These operations require specialized business logic handling
  
**REQUIRED DEPENDENCY**: 
- Must use **`blazor-list-filter-generator`** skill to implement actual search/filter functionality
- This skill only provides the UI structure and component placement for filters
- All filter logic implementation must be delegated to the filter generator skill

##### ❌ Create/Add Operation Implementation  
- **Cannot implement**: Create form validation, entity creation logic, or data persistence workflows
- **Cannot implement**: Navigation to create pages, form submission handlers, or success/error handling
- **Reason**: Create operations require dedicated form handling and validation expertise

**REQUIRED DEPENDENCY**:
- Must use **`blazor-list-add-generator`** skill to implement create/add functionality
- This skill only provides the "Create" button structure and navigation placeholder
- All create logic implementation must be delegated to the add generator skill

##### ❌ Edit/Modify Operation Implementation
- **Cannot implement**: Edit form loading, entity update logic, change tracking, or concurrency handling
- **Cannot implement**: Navigation to edit pages, form pre-population, or update confirmation
- **Reason**: Edit operations require dedicated state management and update pattern expertise

**REQUIRED DEPENDENCY**:
- Must use **`blazor-list-modify-generator`** skill to implement edit/modify functionality  
- This skill only provides the "Edit" action button structure and navigation placeholder
- All edit logic implementation must be delegated to the modify generator skill

##### ❌ Delete Operation Implementation
- **Cannot implement**: Delete confirmation dialogs, cascade delete logic, or soft-delete patterns
- **Cannot implement**: Batch delete processing, deletion validation, or audit trail logging
- **Reason**: Delete operations require dedicated safety checks and transaction handling

**REQUIRED DEPENDENCY**:
- Must use **`blazor-list-delete-generator`** skill to implement delete functionality
- This skill only provides the "Delete" action button structure and placeholder
- All delete logic implementation must be delegated to the delete generator skill

#### 2. **What This Skill PROVIDES**

This skill **ONLY** generates:

✅ **UI Structure Components**:
- Data table layout and column definitions
- Search/filter control placeholders (UI only, no logic)
- Pagination controls structure
- Action button placeholders (Create/Edit/Delete buttons without handlers)
- Batch operation UI framework

✅ **Architecture Patterns**:
- Component library integration (MudBlazor, Syncfusion, etc.)
- Responsive design patterns
- Accessibility compliance structure
- Visual design and styling

✅ **Integration Points**:
- Service interface injection structure
- Method call placeholders (marked with TODO comments)
- Event handler stubs (empty implementations)
- Navigation framework setup

✅ **Documentation and Guidance**:
- Comments indicating where to integrate other skills
- TODO markers for filter/add/modify/delete implementations
- Architecture diagrams showing skill dependencies
- Integration instructions for specialized skills

#### 3. **Skill Dependency Requirements**

**MANDATORY SKILL SEQUENCE**:

```
1. blazor-list-pattern-generator    → Generate list page STRUCTURE ONLY
2. blazor-list-filter-generator     → Implement search/filter LOGIC
3. blazor-list-add-generator        → Implement create/add LOGIC
4. blazor-list-modify-generator     → Implement edit/update LOGIC
5. blazor-list-delete-generator     → Implement delete/remove LOGIC
```

**Integration Markers**:
All generated code MUST include clear TODO comments indicating missing implementations:

``csharp
// TODO: [blazor-list-filter-generator] Implement search logic here
// Current: Placeholder only - requires filter generator skill
private async Task SearchAsync()
{
    throw new NotImplementedException("Requires blazor-list-filter-generator skill");
}

// TODO: [blazor-list-add-generator] Implement create navigation and logic
// Current: Placeholder only - requires add generator skill  
private async Task NavigateToCreateAsync()
{
    throw new NotImplementedException("Requires blazor-list-add-generator skill");
}

// TODO: [blazor-list-modify-generator] Implement edit navigation and logic
// Current: Placeholder only - requires modify generator skill
private async Task NavigateToEditAsync(int id)
{
    throw new NotImplementedException("Requires blazor-list-modify-generator skill");
}

// TODO: [blazor-list-delete-generator] Implement delete logic
// Current: Placeholder only - requires delete generator skill
private async Task DeleteAsync(int id)
{
    throw new NotImplementedException("Requires blazor-list-delete-generator skill");
}
```

#### 4. **File Modification Restrictions**

**ALLOWED FILES** (This skill can ONLY modify):
- ✅ List page `.razor` files (structure and layout only)
- ✅ List page `.razor.cs` files (architecture patterns only, NO business logic)
- ✅ List page `.razor.css` files (styling only)

**PROHIBITED FILES** (This skill CANNOT modify):
- ❌ Service classes or interfaces
- ❌ Repository implementations
- ❌ Entity or ViewModel classes
- ❌ Filter/search logic files
- ❌ Form validation logic files
- ❌ Delete operation handlers
- ❌ Any business logic implementation files

#### 5. **Code Generation Limitations**

**GENERATED CODE MUST**:
- Contain ONLY structural and architectural code
- Include TODO comments for all business logic placeholders
- Have empty method stubs or `NotImplementedException` for operations
- Clearly mark sections requiring other skills
- Provide integration guidance in comments

**GENERATED CODE MUST NOT**:
- Contain working search/filter algorithms
- Implement create/edit/delete handlers
- Include form validation logic
- Have data persistence code
- Contain navigation implementation details
- Include error handling for business operations

### Technical Constraints
- **Component Library Support**: Limited to available component libraries in project
- **Server-Side Pagination**: Requires backend pagination support from service layer
- **Virtual Scrolling**: May conflict with some batch operation patterns
- **Browser Compatibility**: Modern browsers (Chrome, Firefox, Edge, Safari)

### Business Constraints
- **Data Volume**: Very large datasets (100K+ records) may require specialized optimization
- **Real-Time Updates**: Live data updates require WebSocket integration
- **Complex Filtering**: Very complex filters need dedicated advanced search page
- **Export Limits**: Large exports may require background job processing

## Documentation and Knowledge Transfer

### Generated Documentation
- **List Page Architecture Diagrams**: Visual representation of page structure
- **Component Configuration Guide**: Detailed column and control configuration
- **Performance Benchmarks**: Document expected performance characteristics
- **Accessibility Compliance**: WCAG compliance checklist and test results

### Knowledge Transfer Materials
- **Implementation Tutorials**: Step-by-step implementation guide
- **Best Practices Guide**: Data table design and optimization best practices
- **Pattern Library**: Reusable list page patterns for different scenarios
- **Troubleshooting Guide**: Solutions for common list page issues

## Implementation Workflow

### Phase 1: Analysis and Design (STRUCTURE FOCUS)
1. Analyze entity structure and ViewModel properties
2. Determine data volume and performance requirements
3. Identify component library and available components
4. Design table structure and column configuration
5. **OUTPUT**: Structural design document with clear skill dependency markers

### Phase 2: Structure Implementation (NO BUSINESS LOGIC)
1. Generate complete list page `.razor` file (**layout only**)
2. Implement method stubs in `.razor.cs` (**with TODO comments, NO logic**)
3. Add search, sort, filter **UI controls** (**NO logic implementation**)
4. Implement pagination controls **structure**
5. Add batch operation **UI buttons** (**NO handlers**)
6. **MARK ALL PLACEHOLDERS**: Clear TODO comments indicating required skills

### Phase 3: Integration Preparation (HANDOFF TO OTHER SKILLS)
1. Verify all TODO markers are in place
2. Document skill dependencies clearly
3. Provide integration instructions for:
   - `blazor-list-filter-generator` → Search/filter logic
   - `blazor-list-add-generator` → Create/add operations
   - `blazor-list-modify-generator` → Edit/modify operations
   - `blazor-list-delete-generator` → Delete operations
4. Validate structure is ready for next-phase skills

### ⚠️ CRITICAL WORKFLOW REQUIREMENT

This skill is **PHASE 1 OF 5** in the complete list page implementation:

```
PHASE 1: blazor-list-pattern-generator (THIS SKILL)
  └─ Output: List page STRUCTURE with TODO placeholders
  
PHASE 2: blazor-list-filter-generator
  └─ Input: Structure from Phase 1
  └─ Output: Working search/filter logic implementation
  
PHASE 3: blazor-list-add-generator
  └─ Input: Structure from Phase 1
  └─ Output: Working create/add operation handlers
  
PHASE 4: blazor-list-modify-generator
  └─ Input: Structure from Phase 1
  └─ Output: Working edit/modify operation handlers
  
PHASE 5: blazor-list-delete-generator
  └─ Input: Structure from Phase 1
  └─ Output: Working delete operation handlers
  
FINAL RESULT: Complete, functional list page
```

**IMPORTANT**: This skill's output is **INCOMPLETE** without subsequent phases. All generated code will have `NotImplementedException` until other skills are applied.

## Key Principles
1. **User Experience First**: Design for common user workflows
2. **Performance Optimized**: Handle large datasets efficiently
3. **Accessible by Design**: Ensure WCAG 2.1 AA compliance
4. **Responsive Everywhere**: Work on all screen sizes
5. **Component Library Agnostic**: Adapt to available component library

## What This Skill DOES

✅ Generate complete list page **STRUCTURE** with data table
✅ Implement search, sort, filter **UI placeholders** (NO logic)
✅ Add pagination **controls structure** (NO data loading logic)
✅ Create batch operation **UI framework** (NO handlers)
✅ Optimize for performance and accessibility **patterns**
✅ Provide responsive design **layouts**
✅ Generate styles and theme integration
✅ Include TODO markers for all business logic
✅ Provide clear integration points for specialized skills

## What This Skill DOES NOT DO

❌ **Does NOT implement search/filter logic** - Requires `blazor-list-filter-generator` skill
❌ **Does NOT implement create/add operations** - Requires `blazor-list-add-generator` skill
❌ **Does NOT implement edit/modify operations** - Requires `blazor-list-modify-generator` skill
❌ **Does NOT implement delete operations** - Requires `blazor-list-delete-generator` skill
❌ Does NOT generate backend pagination logic (assumes service provides it)
❌ Does NOT implement real-time data updates (requires WebSocket)
❌ Does NOT create entity-specific validation (handled by ViewModel)
❌ Does NOT implement advanced search page (separate pattern for complex searches)
❌ Does NOT handle file uploads (separate upload pattern)
❌ Does NOT contain working business logic in method handlers
