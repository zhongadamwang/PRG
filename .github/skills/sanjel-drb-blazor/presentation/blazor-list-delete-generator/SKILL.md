---
name: blazor-list-delete-generator
description: Generate delete operation handlers with confirmation dialogs, batch delete support, and proper error handling for Blazor list pages.
---

# Blazor List Delete Generator

**Responsibility**: Implement complete delete operation functionality for list pages in Blazor applications
**Input**: List page structure from blazor-list-pattern-generator + Entity metadata + Delete operation requirements
**Output**: Complete delete implementation with confirmation dialogs, batch operations, and safety mechanisms

**Approach**: **AI-Driven Delete Operation Implementation**
- Takes existing list page structure with TODO placeholders
- Implements delete button handlers with confirmation dialogs
- Supports both single and batch delete operations
- Handles soft delete vs hard delete patterns
- Manages optimistic updates and error recovery
- **Output**: Working delete functionality with comprehensive safety measures

## Description

**Testing Requirement:**
After implementing any delete functionality, add corresponding unit tests and Playwright UI tests (C#) to validate the delete operations. Playwright tests should cover delete button interactions, confirmation dialogs, batch delete operations, error handling, and list refresh functionality for Blazor pages.

This skill acts as a **Delete Operations Specialist** for Blazor list pages. It implements the complete delete functionality by:
- **Automatically detecting and adapting to the current project's component library** (MudBlazor, Syncfusion, Bootstrap, etc.)
- Taking the structural framework from `blazor-list-pattern-generator`
- Implementing the TODO-marked delete operation placeholders
- Adding confirmation dialogs and safety mechanisms using appropriate component library patterns
- Supporting batch delete operations with progress tracking
- Handling delete validation and constraint checking
- Managing list refresh and optimistic updates

**DEPENDENCY**: This skill **REQUIRES** output from `blazor-list-pattern-generator` as input. It cannot work in isolation.

## Responsibility

Implement **DELETE OPERATIONS** for list pages, including:
- ✅ Delete button click handlers (replace placeholder TODO methods)
- ✅ Confirmation dialogs with delete impact information
- ✅ Single item delete with validation and constraints
- ✅ Batch delete operations with progress tracking
- ✅ Soft delete vs hard delete implementation
- ✅ Optimistic UI updates with rollback capability
- ✅ Error handling and recovery mechanisms
- ✅ Delete validation and dependency checking

**WORKS WITH**:
- ✅ Structure from `blazor-list-pattern-generator`
- ✅ Service layer delete methods
- ✅ Component library confirmation dialogs
- ✅ Audit logging and compliance systems

## AI Persona

**Role**: Senior Delete Operations Specialist & Data Safety Expert  
**Expertise**: 12+ years experience in CRUD operations with focus on data safety and deletion patterns  
**Specializations**:
- Delete operation safety patterns and confirmation workflows
- Batch operation implementation with progress tracking
- Soft delete vs hard delete pattern implementation
- Constraint validation and dependency checking
- Optimistic UI updates with proper error recovery
- Audit trail and compliance logging for delete operations
- User experience for deletion workflows and error prevention
- Performance optimization for batch delete operations

## Input Parameters

### Required Inputs
- **List Page Structure**: Complete list page structure from `blazor-list-pattern-generator`
- **Entity Metadata**: JSON metadata containing entity relationships and constraints
- **Service Interface**: Service interface containing delete method signatures
- **Delete Strategy**: Soft delete vs hard delete, constraint checking requirements

### Optional Inputs
- **Confirmation Requirements**: Custom confirmation dialog content and validation
- **Batch Operation Limits**: Maximum batch size, progress reporting requirements
- **Audit Requirements**: Logging and compliance requirements for delete operations
- **Permission Requirements**: Role-based access control for delete operations
- **Constraint Validation**: Custom validation for delete dependencies

## Output Deliverables

### 1. **Complete Delete Operation Implementation**

Replaces all TODO placeholders from `blazor-list-pattern-generator` with working implementations:

```csharp
// BEFORE (from blazor-list-pattern-generator):
// TODO: [blazor-list-delete-generator] Implement delete logic
private async Task DeleteAsync(int id)
{
    throw new NotImplementedException("Requires blazor-list-delete-generator skill");
}

// AFTER (implemented by this skill):
private async Task DeleteAsync(int id)
{
    var entity = _items.FirstOrDefault(x => x.Id == id);
    if (entity == null) return;

    var confirmed = await ShowDeleteConfirmationAsync(entity);
    if (!confirmed) return;

    try
    {
        await ProcessDeleteAsync(entity);
    }
    catch (DeleteConstraintException ex)
    {
        await ShowConstraintErrorAsync(ex.Message);
    }
}
```

### 2. **Confirmation Dialog Implementation**

- **Smart Confirmation**: Show relevant information about what will be deleted
- **Impact Assessment**: Display consequences of deletion (related records, etc.)
- **Safety Checks**: Prevent accidental deletion with clear confirmation requirements
- **Custom Messages**: Entity-specific confirmation messages

### 3. **Batch Delete Implementation**

- **Selection Management**: Handle multi-select checkboxes and "Select All" functionality
- **Progress Tracking**: Show progress for large batch operations
- **Partial Success Handling**: Handle cases where some deletes succeed and others fail
- **Performance Optimization**: Batch API calls efficiently

### 4. **Delete Validation and Safety**

- **Constraint Checking**: Validate foreign key constraints before deletion
- **Dependency Analysis**: Check for dependent records that would be affected
- **Permission Validation**: Verify user has permission to delete specific records
- **Business Rule Validation**: Apply business-specific deletion rules

### 5. **UI State Management**

- **Optimistic Updates**: Remove items from UI immediately, rollback on failure
- **Loading States**: Show loading indicators during delete operations
- **Error Recovery**: Handle errors gracefully with clear user feedback
- **List Refresh**: Update list state after successful deletions

## Functional Capabilities

### Core Delete Operations
- **Single Item Delete**: Delete individual items with confirmation
- **Batch Delete Operations**: Delete multiple selected items efficiently
- **Conditional Delete**: Delete only items meeting certain criteria
- **Cascade Delete Handling**: Handle related record deletion appropriately

### Confirmation and Safety
- **Multi-Level Confirmation**: Different confirmation levels based on delete impact
- **Impact Preview**: Show what will be deleted (including related records)
- **Undo Capability**: Provide undo for soft deletes where applicable
- **Safety Timeouts**: Prevent accidental rapid-fire deletions

### Validation and Constraints
- **Foreign Key Validation**: Check for constraint violations before deletion
- **Business Rule Validation**: Apply entity-specific deletion rules
- **Permission Checking**: Verify delete permissions at record level
- **Audit Trail**: Log all delete operations for compliance

### Performance and UX
- **Optimistic Updates**: Update UI immediately for better user experience
- **Background Processing**: Handle large batch operations in background
- **Progress Feedback**: Show progress for long-running delete operations
- **Error Recovery**: Proper error handling and rollback mechanisms

## Integration Points

### Dependencies
- **blazor-list-pattern-generator**: Provides list page structure with delete TODO placeholders
- **service-method-generator**: Uses service methods for delete operations
- **component-library**: Uses confirmation dialogs and progress indicators
- **audit-system**: Integrates with audit logging if required

### Service Layer Integration
- **Delete Methods**: Calls service.DeleteAsync() and service.BatchDeleteAsync() methods
- **Validation**: Integrates with service-layer constraint validation
- **Error Handling**: Processes delete exceptions and converts to user messages
- **Transaction Management**: Handles transactional delete operations

### UI Component Integration
- **Confirmation Dialogs**: Integrates with MudBlazor dialogs, Bootstrap modals, etc.
- **Progress Indicators**: Uses component library progress bars and spinners
- **Notification Systems**: Integrates with Snackbar, Toast, or Alert systems
- **Icon Libraries**: Uses appropriate delete icons and visual indicators

## Usage Scenarios

### Scenario 1: Simple Item Delete
**Input**: Basic entity with no complex relationships  
**Output**: Single-click delete with simple confirmation  
**Features**: 
- ✅ Delete button with confirmation dialog
- ✅ Optimistic UI update (immediate removal from list)
- ✅ Success/error notifications
- ✅ Rollback on failure

### Scenario 2: Complex Entity Deletion
**Input**: Entity with foreign key relationships and business constraints  
**Output**: Multi-step confirmation with impact analysis  
**Features**:
- ✅ Constraint checking before confirmation
- ✅ Impact preview (shows related records that will be affected)
- ✅ Multi-level confirmation for high-impact deletions
- ✅ Detailed error messages for constraint violations

### Scenario 3: Batch Delete Operations
**Input**: Multiple selected items for bulk deletion  
**Output**: Efficient batch processing with progress tracking  
**Features**:
- ✅ Multi-select checkboxes with "Select All"
- ✅ Batch delete button with count display
- ✅ Progress bar for large batch operations
- ✅ Partial success handling and error reporting

### Scenario 4: Soft Delete Implementation
**Input**: Entity requiring soft delete (audit trail preservation)  
**Output**: Soft delete with option to restore  
**Features**:
- ✅ Mark as deleted instead of physical deletion
- ✅ Filter out soft-deleted items from normal views
- ✅ Admin interface to view and restore deleted items
- ✅ Permanent delete option for authorized users

## Implementation Patterns

### Single Delete with Confirmation
```csharp
private async Task DeleteItemAsync(RequestEntity entity)
{
    var confirmed = await ShowDeleteConfirmationAsync(
        title: "Delete Request",
        message: $"Are you sure you want to delete request '{entity.Title}'?",
        confirmButtonText: "Delete",
        entity: entity
    );
    
    if (!confirmed) return;
    
    try
    {
        // Optimistic update
        RemoveFromUI(entity);
        
        // Perform delete
        await RequestService.DeleteAsync(entity.Id);
        
        ShowSuccessMessage("Request deleted successfully");
    }
    catch (DeleteConstraintException ex)
    {
        // Rollback UI change
        AddBackToUI(entity);
        
        await ShowConstraintErrorDialog(ex.Message, ex.RelatedEntities);
    }
    catch (Exception ex)
    {
        // Rollback UI change
        AddBackToUI(entity);
        
        ShowErrorMessage("Failed to delete request. Please try again.");
        Logger.LogError(ex, "Error deleting request {RequestId}", entity.Id);
    }
}
```

### Batch Delete Implementation
```csharp
private async Task DeleteSelectedItemsAsync()
{
    var selectedItems = _items.Where(x => x.IsSelected).ToList();
    
    if (!selectedItems.Any())
    {
        ShowWarningMessage("No items selected for deletion");
        return;
    }
    
    var confirmed = await ShowBatchDeleteConfirmationAsync(selectedItems.Count);
    if (!confirmed) return;
    
    var progress = new Progress<BatchDeleteProgress>(OnDeleteProgress);
    
    try
    {
        // Start batch delete with progress tracking
        var result = await RequestService.BatchDeleteAsync(
            selectedItems.Select(x => x.Id).ToList(), 
            progress
        );
        
        // Remove successfully deleted items from UI
        foreach (var id in result.SuccessfulIds)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            if (item != null) _items.Remove(item);
        }
        
        // Handle partial failures
        if (result.FailedItems.Any())
        {
            await ShowBatchErrorsAsync(result.FailedItems);
        }
        
        ShowSuccessMessage($"{result.SuccessfulIds.Count} items deleted successfully");
    }
    catch (Exception ex)
    {
        ShowErrorMessage("Batch delete operation failed. Please try again.");
        Logger.LogError(ex, "Error in batch delete operation");
    }
    finally
    {
        ClearSelection();
        StateHasChanged();
    }
}
```

### Soft Delete Pattern
```csharp
private async Task SoftDeleteItemAsync(RequestEntity entity)
{
    var confirmed = await ShowSoftDeleteConfirmationAsync(entity);
    if (!confirmed) return;
    
    try
    {
        // Mark as deleted in service
        await RequestService.SoftDeleteAsync(entity.Id);
        
        // Update UI to show deleted state or remove from list
        if (_showDeleted)
        {
            entity.IsDeleted = true;
            entity.DeletedDate = DateTime.UtcNow;
        }
        else
        {
            _items.Remove(entity);
        }
        
        ShowSuccessMessage("Request moved to trash. You can restore it from the admin panel.");
        StateHasChanged();
    }
    catch (Exception ex)
    {
        ShowErrorMessage("Failed to delete request. Please try again.");
        Logger.LogError(ex, "Error soft deleting request {RequestId}", entity.Id);
    }
}
```

## Error Handling Patterns

### Constraint Violation Handling
```csharp
private async Task HandleDeleteConstraintAsync(DeleteConstraintException ex)
{
    var dialogContent = new StringBuilder();
    dialogContent.AppendLine($"Cannot delete this item because it is referenced by:");
    
    foreach (var related in ex.RelatedEntities)
    {
        dialogContent.AppendLine($"• {related.Count} {related.EntityType}(s)");
    }
    
    dialogContent.AppendLine();
    dialogContent.AppendLine("To delete this item, you must first remove the related records.");
    
    await DialogService.ShowMessageBox(
        title: "Delete Not Allowed",
        message: dialogContent.ToString(),
        yesText: "OK"
    );
}
```

### Batch Operation Error Handling
```csharp
private async Task HandleBatchDeleteErrorsAsync(List<BatchDeleteError> errors)
{
    var errorGroups = errors.GroupBy(e => e.ErrorType).ToList();
    
    var dialogContent = new StringBuilder();
    dialogContent.AppendLine($"{errors.Count} items could not be deleted:");
    
    foreach (var group in errorGroups)
    {
        dialogContent.AppendLine($"\n{group.Key}:");
        foreach (var error in group.Take(5)) // Show max 5 examples
        {
            dialogContent.AppendLine($"• {error.ItemName}: {error.Message}");
        }
        
        if (group.Count() > 5)
        {
            dialogContent.AppendLine($"  ... and {group.Count() - 5} more");
        }
    }
    
    await DialogService.ShowMessageBox(
        title: "Partial Delete Failure",
        message: dialogContent.ToString(),
        yesText: "OK"
    );
}
```

## Performance Optimization

### Optimistic Updates
```csharp
private async Task OptimisticDeleteAsync(RequestEntity entity)
{
    // Store original state for rollback
    var originalIndex = _items.IndexOf(entity);
    
    // Immediate UI update
    _items.Remove(entity);
    StateHasChanged();
    
    try
    {
        // Perform actual delete
        await RequestService.DeleteAsync(entity.Id);
    }
    catch (Exception ex)
    {
        // Rollback: restore item to original position
        _items.Insert(originalIndex, entity);
        StateHasChanged();
        
        // Show error
        ShowErrorMessage("Delete failed. Item has been restored.");
        throw;
    }
}
```

### Batch Operation Optimization
- **Chunked Processing**: Process large batches in smaller chunks
- **Parallel Execution**: Use parallel processing where safe
- **Progress Reporting**: Report progress to keep UI responsive
- **Memory Management**: Clean up resources during long operations

## Testing Strategies

### Unit Testing
```csharp
[Test]
public async Task DeleteAsync_WithValidId_RemovesItemFromList()
{
    // Arrange
    var entity = CreateTestEntity();
    _service.Setup(s => s.DeleteAsync(entity.Id)).Returns(Task.CompletedTask);
    
    // Act
    await _component.DeleteAsync(entity.Id);
    
    // Assert
    Assert.That(_component.Items.Contains(entity), Is.False);
    _service.Verify(s => s.DeleteAsync(entity.Id), Times.Once);
}

[Test]
public async Task DeleteAsync_WithConstraintViolation_ShowsErrorAndRestoresItem()
{
    // Arrange
    var entity = CreateTestEntity();
    _service.Setup(s => s.DeleteAsync(entity.Id))
           .ThrowsAsync(new DeleteConstraintException("Referenced by other entities"));
    
    // Act
    await _component.DeleteAsync(entity.Id);
    
    // Assert
    Assert.That(_component.Items.Contains(entity), Is.True);
    Assert.That(_component.LastErrorMessage, Contains.Substring("Referenced by"));
}
```

### Integration Testing
- **Full Delete Workflow**: Test complete delete workflow including confirmations
- **Batch Operations**: Test batch delete with various selection scenarios
- **Error Scenarios**: Test constraint violations, network errors, permissions
- **UI State Management**: Test optimistic updates and rollback scenarios

## Best Practices

### Safety First
- **Always Confirm**: Never delete without explicit user confirmation
- **Show Impact**: Display what will be deleted including related records
- **Soft Delete Default**: Use soft delete by default unless hard delete is required
- **Audit Everything**: Log all delete operations for compliance and debugging

### User Experience
- **Clear Feedback**: Provide immediate feedback for all delete operations
- **Undo Options**: Provide undo capability where possible (especially soft deletes)
- **Progress Indication**: Show progress for long-running batch operations
- **Error Recovery**: Provide clear paths to resolve delete errors

### Performance
- **Optimistic Updates**: Update UI immediately for better perceived performance
- **Batch Efficiency**: Use efficient batch operations for multiple deletes
- **Memory Management**: Clean up resources properly during long operations
- **Network Optimization**: Minimize API calls through proper batching

### Security and Compliance
- **Permission Checks**: Always verify delete permissions before operations
- **Audit Logging**: Log all delete operations with user, timestamp, and reason
- **Data Retention**: Respect data retention policies and legal requirements
- **Constraint Enforcement**: Always check and respect database constraints

## Constraints and Limitations

### Technical Constraints
- **Service Dependency**: Requires service layer with delete methods
- **Database Constraints**: Must respect foreign key constraints and database rules
- **Transaction Limits**: Limited by database transaction size and timeout limits
- **UI Performance**: Large batch operations may impact UI responsiveness

### Business Constraints
- **Permission Requirements**: Must integrate with authorization systems
- **Audit Requirements**: Must support compliance and audit logging requirements
- **Data Retention Policies**: Must respect legal and business data retention rules
- **Workflow Integration**: Must integrate with business approval workflows if required

### Integration Constraints
- **List Structure Dependency**: Can only work with list pages that have proper structure
- **Component Library Limits**: Limited to available components for confirmations and progress
- **Service Interface Constraints**: Must work with existing service method signatures
- **State Management**: Must integrate with existing state management patterns

## Integration Workflow

### Phase 1: Structure Analysis
1. **Analyze List Structure**: Review list page structure from blazor-list-pattern-generator
2. **Identify Delete Placeholders**: Find all delete-related TODO markers
3. **Review Entity Constraints**: Understand foreign key relationships and constraints
4. **Assess Delete Requirements**: Determine soft vs hard delete, batch requirements

### Phase 2: Implementation
1. **Replace TODO Methods**: Implement all delete-related placeholder methods
2. **Add Confirmation Dialogs**: Implement appropriate confirmation patterns
3. **Implement Batch Operations**: Add multi-select and batch delete capabilities
4. **Add Error Handling**: Implement comprehensive error handling and recovery

### Phase 3: Integration Testing
1. **Test Delete Workflows**: Verify complete delete operation workflows
2. **Test Error Scenarios**: Verify constraint violations and error handling
3. **Test Batch Operations**: Verify batch delete performance and partial failures
4. **Test State Management**: Verify optimistic updates and rollback functionality

## Key Principles
1. **Safety First**: Always prioritize data safety and user confirmation
2. **Clear Feedback**: Provide immediate and clear feedback for all operations
3. **Error Resilience**: Handle all error scenarios gracefully with recovery options
4. **Performance Conscious**: Optimize for responsive delete operations
5. **Audit Compliance**: Ensure all delete operations are properly logged and trackable