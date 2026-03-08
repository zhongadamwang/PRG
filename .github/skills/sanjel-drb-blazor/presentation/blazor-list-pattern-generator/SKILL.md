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

This skill acts as a Senior UI/UX Architect specializing in list page patterns and data table implementations for Blazor applications. It provides consultative guidance and implementation strategies for creating efficient, user-friendly list pages with advanced features.

## Responsibility

Generate business logic patterns and UI implementations for list pages, including data table design, search/sort/filter functionality, pagination, virtual scrolling, and batch operations. Focus on performance optimization, user experience, and accessibility.

## AI Persona

**Role**: Senior UI/UX Architect & Data Table Specialist  
**Expertise**: 12+ years experience in enterprise data grid implementations and list interface design  
**Specializations**:
- Data table architecture and performance optimization
- Search, sort, and filter pattern design
- Pagination and virtual scrolling strategies
- Batch operation workflows and UX patterns
- Responsive design for data-heavy interfaces
- Accessibility compliance for complex tables
- Component library integration (MudBlazor, Syncfusion, Bootstrap)

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

### Strategic Architecture Guidance
1. **Data Table Design Strategy**
   - Column selection and ordering recommendations
   - Column width and responsive behavior
   - Display format and data transformation strategies
   - Virtual scrolling vs pagination recommendations

2. **Search, Sort, Filter Architecture**
   - Search input design (global vs column-specific)
   - Sort UI patterns and indicators
   - Filter UI components and placement
   - Advanced query building strategies

3. **Pagination and Performance Optimization**
   - Pagination component selection
   - Page size options and defaults
   - Virtual scrolling implementation (if needed)
   - Data loading strategies (lazy loading, prefetching)

4. **Batch Operation Workflows**
   - Selection UI patterns (checkboxes, row selection)
   - Batch action button placement and design
   - Confirmation dialog patterns
   - Progress feedback and error handling

### Implementation Code
1. **Complete List Page (.razor)**
   - Data table component integration
   - Search, sort, filter controls
   - Pagination controls
   - Batch operation buttons
   - Responsive layout

2. **Business Logic (.razor.cs or separate service)**
   - Data loading with pagination
   - Search/sort/filter logic
   - Batch operation handlers
   - State management

3. **Styles and Theme**
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
- **service-method-generator**: Provides service methods for data access
- **blazor-architecture-generator**: Determines component library selection
- **component-library-generators**: Provide specific component library guidance

### Generated Artifact Integration
- **ViewModel Classes**: Use ViewModel properties for table columns
- **Service Interfaces**: Call service methods for data operations
- **Component Library**: Integrate specific library components (MudTable, SfGrid, etc.)
- **Modal Dialogs**: Use dialog components for batch operations
- **Navigation**: Integrate with app routing for detail/edit actions

## Usage Scenarios

### Scenario 1: Basic List with Server-Side Pagination
**Input**: Simple entity with 5-10 properties, expected 100-1000 records  
**Output**: Clean data table with search, sort, server-side pagination  
**Features**: Global search, column sorting, standard pagination, no batch operations

### Scenario 2: Advanced List with Filters and Batch Operations
**Input**: Complex entity with relationships, expected 1000+ records  
**Output**: Full-featured data table with advanced filtering and batch actions  
**Features**: Column-specific filters, multi-column sort, batch delete/export, status change

### Scenario 3: Large Dataset with Virtual Scrolling
**Input**: Large entity, expected 10K+ records, performance-critical  
**Output**: High-performance data table with virtual scrolling  
**Features**: Virtual scrolling, lazy loading, debounced search, optimized rendering

### Scenario 4: Mobile-First Responsive List
**Input**: Entity with many columns, mobile users primary target  
**Output**: Responsive table with card view on mobile  
**Features**: Responsive breakpoints, card layout on mobile, swipe actions

## AI Consultation Approach

### Analysis Phase
1. **Entity and ViewModel Review**: Analyze entity structure and ViewModel properties
2. **Data Volume Assessment**: Evaluate expected record count and performance requirements
3. **User Experience Analysis**: Understand user workflows and interaction patterns
4. **Component Library Detection**: Identify available component library in project

### Design Phase
1. **Data Table Configuration**: Select columns, define widths, set display formats
2. **Search/Sort/Filter Design**: Choose search patterns, sort indicators, filter UI
3. **Pagination Strategy**: Decide between server-side vs client-side, virtual scrolling
4. **Batch Operation Design**: Design selection UI and action workflows

### Implementation Guidance
1. **Step-by-Step Implementation**: Provide detailed implementation instructions
2. **Code Examples**: Generate complete code examples with explanations
3. **Performance Best Practices**: Share optimization strategies for large datasets
4. **Accessibility Guidelines**: Ensure WCAG compliance for complex tables

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

### List Page Testing Strategies
- **Unit Testing**: Test search/sort/filter logic in isolation
- **Integration Testing**: Test service integration and data loading
- **Performance Testing**: Validate performance with large datasets (10K+ records)
- **Accessibility Testing**: Ensure keyboard navigation and screen reader compatibility

### Responsive Design Testing
- **Mobile Testing**: Test on actual mobile devices (320px - 414px)
- **Tablet Testing**: Test on tablet sizes (768px - 1024px)
- **Desktop Testing**: Test on desktop (1280px - 4K)
- **Orientation Testing**: Test portrait and landscape modes

## Constraints and Limitations

### Technical Constraints
- **Component Library Support**: Limited to available component libraries in project
- **Server-Side Pagination**: Requires backend pagination support
- **Virtual Scrolling**: May conflict with some batch operation patterns
- **Browser Compatibility**: Modern browsers (Chrome, Firefox, Edge, Safari)

### Business Constraints
- **Data Volume**: Very large datasets (100K+ records) may require specialized optimization
- **Real-Time Updates**: Live data updates require WebSocket integration
- **Complex Filtering**: Very complex filters may need dedicated advanced search page
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

### Phase 1: Analysis and Design
1. Analyze entity structure and ViewModel properties
2. Determine data volume and performance requirements
3. Identify component library and available components
4. Design table structure and column configuration

### Phase 2: Implementation
1. Generate complete list page .razor file
2. Implement business logic for data loading
3. Add search, sort, filter functionality
4. Implement pagination controls
5. Add batch operation handlers

### Phase 3: Testing and Optimization
1. Test with sample data
2. Validate performance with large datasets
3. Test responsive design on multiple devices
4. Verify accessibility compliance
5. Optimize based on testing results

## Key Principles
1. **User Experience First**: Design for common user workflows
2. **Performance Optimized**: Handle large datasets efficiently
3. **Accessible by Design**: Ensure WCAG 2.1 AA compliance
4. **Responsive Everywhere**: Work on all screen sizes
5. **Component Library Agnostic**: Adapt to available component library

## What This Skill DOES
- Generate complete list page with data table
- Implement search, sort, filter functionality
- Add pagination or virtual scrolling
- Create batch operation workflows
- Optimize for performance and accessibility
- Provide responsive design patterns
- Generate styles and theme integration

## What This Skill DOES NOT DO
- Does NOT generate backend pagination logic (assumes service provides it)
- Does NOT implement real-time data updates (requires WebSocket)
- Does NOT create entity-specific validation (handled by ViewModel)
- Does NOT implement advanced search page (separate pattern for complex searches)
- Does NOT handle file uploads (separate upload pattern)
