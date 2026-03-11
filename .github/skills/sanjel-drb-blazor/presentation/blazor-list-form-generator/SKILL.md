---
name: blazor-list-form-generator
description: Generate create/add and edit/modify operation handlers for Blazor list pages using popup/modal dialogs, with inline editing, validation, and Playwright tests.
---

# Blazor List Form Generator

**Responsibility**: Implement complete create and edit operation functionality for list pages in Blazor applications using popup/modal dialogs
**Input**: List page structure from blazor-list-pattern-generator + Entity metadata + ViewModel validation rules + Edit operation requirements
**Output**: Complete create and edit implementations using MudBlazor dialog popups, with inline editing, validation, and Playwright UI tests

**Approach**: **AI-Driven Add & Edit Operation Implementation (Modal-Only)**
- Takes existing list page structure with TODO placeholders for both create and edit operations
- Implements create button handlers and edit button handlers that open **popup/modal dialogs**
- All create and edit interactions happen in modal dialogs — no page navigation
- Handles validation, concurrency checking, error handling, and success notifications within dialogs
- Manages state refresh after successful create or edit operations
- Generates Playwright UI tests for both create and edit workflows
- **Output**: Working create and edit functionality integrated into the list page using popup dialogs

## Description

**Testing Requirement — MANDATORY:**
After implementing any create/add or edit/modify functionality, you **MUST** also generate corresponding **Playwright UI tests (C#, NUnit)** in the project's existing Blazor test project (e.g. `src/Sanjel.RequestManagement.Blazor.Tests/`). These tests are a required output deliverable — the skill implementation is NOT complete until the tests are generated.

**What to test — Create/Add:**
- Create/New button is visible and clickable
- Clicking create navigates to the correct URL or opens the expected modal/dialog
- Form fields render and accept input
- Form validation prevents invalid submissions
- Successful creation shows a success notification (e.g. toast)
- After creation the list page refreshes and displays the new item
- Cancel/back navigation returns to the list without creating

**What to test — Edit/Modify:**
- Edit button is visible and clickable on each list row
- Clicking edit navigates to the edit form page or opens the edit dialog
- Edit form is pre-populated with the item's existing data
- Form validation prevents saving invalid data
- Saving valid changes updates the item in the list and shows a success toast
- Cancel returns to the list without saving changes

**Test file conventions:**
- Add tests for add operations to `{Entity}AddPlaywrightTests.cs`
- Add tests for edit operations to `{Entity}EditPlaywrightTests.cs`
- Use the same setup/teardown pattern as existing Playwright tests (see `RequestPagePlaywrightTests.cs` for reference)
- Use NUnit `[TestFixture]` / `[Test]` attributes
- Use `Microsoft.Playwright` for browser automation
- Tests must be headless-compatible

This skill acts as a **Create & Edit Operations Specialist** for Blazor list pages. It implements both create and edit functionality by:
- **Using popup/modal dialogs exclusively** — all create and edit operations open in dialogs, never navigate to a new page
- **Automatically detecting and adapting to the current project's component library** (MudBlazor, Syncfusion, Bootstrap, etc.)
- Taking the structural framework from `blazor-list-pattern-generator`
- Implementing all TODO-marked create and edit operation placeholders with dialog-based patterns
- Adding inline editing capabilities directly in the data table as an alternative lightweight option
- Handling success/error states, concurrency conflicts, and list refresh logic within dialogs
- Integrating proper validation and user feedback with library-specific dialog components

**DEPENDENCY**: This skill **REQUIRES** output from `blazor-list-pattern-generator` as input. It cannot work in isolation.

## Prerequisites and Skill Dependencies

### Required Pre-execution Skills
**Before using this skill, you must execute:**

1. **blazor-viewmodel-generator** (MANDATORY)
   - **Purpose**: Generate ViewModel classes with Data Annotations validation for the target entity
   - **When to run**: Once per entity before implementing create/edit forms
   - **Example Query**:
     > Use blazor-viewmodel-generator skill to generate a complete ViewModel class (including Data Annotations validation) and corresponding NUnit unit tests for the `ProgramRequest` entity based on domain-model-metadata.json in the project, and place them in the `Sanjel.RequestManagement.Core.Tests` project.

   **Why required**: This skill requires ViewModel classes with validation rules to implement form binding and validation logic. Forms cannot be properly implemented without these ViewModels.

2. **blazor-list-pattern-generator** (MANDATORY)
   - **Purpose**: Generate list page structure with TODO placeholders for create/edit operations
   - **When to run**: Before implementing create/edit operations
   - **Why required**: This skill replaces TODO placeholders in the list page structure

### Integration Workflow
```
1. blazor-viewmodel-generator → Generate ViewModels with validation
2. blazor-list-pattern-generator → Generate list structure with TODO placeholders
3. blazor-list-form-generator → Implement create/edit using ViewModels
```

### Auto-Detection Pattern
When invoked, this skill will:
1. **Check for existing ViewModels**: Search for `*ViewModel.cs` files in the project
2. **If missing**: Recommend executing `blazor-viewmodel-generator` first with specific instructions
3. **If found**: Proceed with form implementation using existing ViewModels

## Responsibility

Implement **CREATE AND EDIT OPERATIONS** for list pages, including:

**Create Operations:**
- ✅ Create button click handlers (replace placeholder TODO methods)
- ✅ Open create form in a **modal/popup dialog** (no page navigation)
- ✅ Form submission integration and validation handling inside the dialog
- ✅ Success notifications and error handling
- ✅ List data refresh after dialog closes on success
- ✅ State management during create operations
- ✅ Loading states and user feedback within the dialog

**Edit Operations:**
- ✅ Edit button click handlers (replace placeholder TODO methods)
- ✅ Open edit form in a **modal/popup dialog** pre-populated with entity data
- ✅ Inline editing directly in data table rows (lightweight alternative for simple fields)
- ✅ Form pre-population with existing entity data
- ✅ Real-time validation and error handling inside the dialog
- ✅ Optimistic updates with concurrency conflict resolution
- ✅ Bulk edit operations for multiple selected records via dialog
- ✅ Change tracking and dirty state management

**WORKS WITH**:
- ✅ Structure from `blazor-list-pattern-generator`
- ✅ Form pages generated by `blazor-form-generator`
- ✅ Modal dialogs from component libraries
- ✅ Service layer create and update methods

## AI Persona

**Role**: Senior Create & Edit Operations Specialist  
**Expertise**: 12+ years experience in CRUD operation implementation, form integration, and data integrity  
**Specializations**:
- Create operation handlers and form navigation patterns
- Inline editing with real-time validation
- Edit form integration patterns (modal, page-based, inline)
- Concurrency conflict resolution and optimistic locking
- Bulk edit operations with progress tracking and error handling
- Change tracking and dirty state management
- User experience for creation and edit workflows
- State management and list refresh strategies

## Input Parameters

### Required Inputs
- **List Page Structure**: Complete list page structure from `blazor-list-pattern-generator`
- **Entity Metadata**: JSON metadata containing entity structure, editable properties, and validation rules
- **Service Interface**: Service interface containing create and update method signatures
- **Form Strategy**: Modal dialog (default) vs inline editing for simple fields — no page navigation

### Optional Inputs
- **Concurrency Strategy**: Optimistic vs pessimistic locking, conflict resolution patterns
- **Validation Rules**: Custom validation requirements beyond entity metadata
- **Bulk Edit Requirements**: Multi-select editing capabilities and constraints
- **Change Tracking**: Audit trail and change history requirements
- **Success Actions**: Custom actions after successful create or edit (navigation, notifications)
- **Permission Requirements**: Role-based and field-level access control

## Output Deliverables

### 1. **Complete Create Operation Implementation**

Replaces all create-related TODO placeholders from `blazor-list-pattern-generator`:

```csharp
// BEFORE (from blazor-list-pattern-generator):
// TODO: [blazor-list-form-generator] Implement create navigation and logic
private async Task NavigateToCreateAsync()
{
    throw new NotImplementedException("Requires blazor-list-form-generator skill");
}

// AFTER:
private async Task NavigateToCreateAsync()
{
    // All create operations open a dialog — no page navigation
    await ShowCreateModalAsync();
}
```

### 2. **Complete Edit Operation Implementation**

Replaces all edit-related TODO placeholders from `blazor-list-pattern-generator`:

```csharp
// BEFORE (from blazor-list-pattern-generator):
// TODO: [blazor-list-form-generator] Implement edit logic
private async Task EditAsync(int id)
{
    throw new NotImplementedException("Requires blazor-list-form-generator skill");
}

// AFTER:
private async Task EditAsync(int id)
{
    var entity = _items.FirstOrDefault(x => x.Id == id);
    if (entity == null) return;

    // All edit operations open a dialog — no page navigation
    await ShowEditModalAsync(entity);
}
```

### 3. **Dialog Form Handlers**

> ⚠️ **Page navigation is NOT used.** All create and edit interactions happen inside popup/modal dialogs on the same list page.

- **Modal Create Dialog**: Open a create form in a `MudDialog` (or equivalent) when the Create button is clicked
- **Modal Edit Dialog**: Open an edit form in a `MudDialog` pre-populated with the selected entity's data
- **Slide-Over Panel (alternative)**: Use a drawer/slide-over panel for richer forms when a full-width layout is preferred
- **Inline Editing (simple fields only)**: Switch individual table cells to edit mode for lightweight, single-field edits
- **Inline Row Creation (simple entities only)**: Insert an editable row at the top of the table for quick-add scenarios
- **Context Preservation**: Because there is no navigation, list state (search, filters, page) is always preserved automatically

### 4. **Form Integration Logic**

- **Pre-population**: Load existing entity data into edit forms; set defaults for create forms
- **Validation Integration**: Handle client-side and server-side validation for both create and edit
- **Submission Handling**: Process form submissions and API calls (CreateAsync / UpdateAsync)
- **Error Recovery**: Handle validation errors, concurrency conflicts, and network failures

### 5. **State Management**

- **Loading States**: Show loading indicators during create/edit operations
- **Success Handling**: Refresh list data and show success notifications
- **Error Handling**: Display error messages and recovery options
- **Navigation State**: Maintain list position and filters when returning from forms
- **Dirty State Tracking**: Track which records have unsaved changes during inline editing
- **Optimistic Updates**: Update UI immediately, rollback on failure

### 6. **Bulk Edit Operations**

- **Multi-Select Editing**: Edit multiple records simultaneously
- **Field-Level Bulk Updates**: Update specific fields across selected records
- **Validation for Bulk Operations**: Validate bulk changes before applying
- **Progress Tracking**: Show progress for large bulk update operations

### 7. **Playwright UI Tests (MANDATORY)**

Generate Playwright tests covering both Create and Edit workflows.

**Create tests:**

```csharp
using Microsoft.Playwright;
using NUnit.Framework;

namespace Sanjel.RequestManagement.Blazor.Tests
{
    [TestFixture]
    public class RequestAddPlaywrightTests
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
        public async Task CreateButton_ShouldBeVisibleAndClickableAsync()
        {
            await this._page.GotoAsync("http://localhost:5000/request");
            await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await this._page.WaitForTimeoutAsync(2000);

            var createButton = await this._page.QuerySelectorAsync(
                "button:has-text('New Request')");
            Assert.IsNotNull(createButton, "New Request button should exist on the list page");

            await createButton.ClickAsync();
            await this._page.WaitForTimeoutAsync(500);
            // Verify navigation occurred or modal opened
        }

        [Test]
        public async Task CreateForm_EmptySubmit_ShouldShowValidationAsync()
        {
            await this._page.GotoAsync("http://localhost:5000/request/create");
            await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await this._page.WaitForTimeoutAsync(2000);

            var submitBtn = await this._page.QuerySelectorAsync("button:has-text('Save')");
            await submitBtn.ClickAsync();
            await this._page.WaitForTimeoutAsync(500);

            var validationErrors = await this._page.QuerySelectorAllAsync(".validation-message, .e-error");
            Assert.That(validationErrors.Count, Is.GreaterThan(0),
                "Validation errors should appear for empty submission");
        }

        [Test]
        public async Task CreateForm_SubmitValid_ShouldShowSuccessAsync()
        {
            await this._page.GotoAsync("http://localhost:5000/request/create");
            await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await this._page.WaitForTimeoutAsync(2000);

            // Fill required form fields and submit
            // Assert success toast or navigation back to list
        }
    }
}
```

**Edit tests:**

```csharp
using Microsoft.Playwright;
using NUnit.Framework;

namespace Sanjel.RequestManagement.Blazor.Tests
{
    [TestFixture]
    public class RequestEditPlaywrightTests
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
        public async Task EditButton_ShouldNavigateToEditFormAsync()
        {
            await this._page.GotoAsync("http://localhost:5000/request");
            await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await this._page.WaitForTimeoutAsync(2000);

            var editBtn = await this._page.QuerySelectorAsync(
                ".action-buttons button[title='Edit'], .e-edit");
            Assert.IsNotNull(editBtn, "Edit button should exist on list rows");

            await editBtn.ClickAsync();
            await this._page.WaitForTimeoutAsync(500);

            Assert.That(this._page.Url, Does.Contain("/edit/"));
        }

        [Test]
        public async Task EditForm_ShouldBePrepopulatedAsync()
        {
            await this._page.GotoAsync("http://localhost:5000/request");
            await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await this._page.WaitForTimeoutAsync(2000);

            var editBtn = await this._page.QuerySelectorAsync(
                ".action-buttons button[title='Edit']");
            await editBtn.ClickAsync();
            await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await this._page.WaitForTimeoutAsync(1000);

            var inputs = await this._page.QuerySelectorAllAsync("input[type='text']:not([value=''])");
            Assert.That(inputs.Count, Is.GreaterThan(0),
                "Edit form fields should be pre-populated with existing data");
        }
    }
}
```

**Key**: Both test files **MUST** be generated as part of the skill output. Adapt selectors, URLs, and assertions to match the actual page being implemented.

## Functional Capabilities

### Core Create Operations
- **Button Handler Implementation**: Replace placeholder create button handlers with working logic
- **Service Integration**: Call service layer `CreateAsync()` methods with proper error handling
- **Validation Handling**: Integrate with form validation and display errors
- **Success Processing**: Handle successful creation and update UI accordingly

### Core Edit Operations
- **Single Record Edit**: Edit individual records with various UI patterns
- **Bulk Record Edit**: Edit multiple records simultaneously
- **Field-Level Updates**: Update specific fields without full record replacement
- **Conditional Updates**: Apply updates based on business rules and constraints

### Dialog / Interaction Patterns
> Page navigation is **not used**. All create and edit operations stay on the list page.
- **Modal Dialog (primary)**: Open create or edit forms in `MudDialog` popups
- **Slide-Over / Drawer (alternative)**: Use a side drawer for forms that need more space
- **Inline Editing (simple fields)**: Edit individual cells or rows directly in the table

### Validation and Integrity
- **Real-Time Validation**: Validate fields as the user types with immediate feedback
- **Cross-Field Validation**: Validate relationships between multiple fields
- **Business Rule Validation**: Apply business-specific validation rules
- **Concurrency Validation**: Detect and resolve concurrent edit conflicts

### State and Performance
- **Optimistic Updates**: Update UI before server confirmation; rollback on failure
- **Change Tracking**: Track exactly what changed for efficient updates
- **Dirty State Management**: Manage unsaved changes across the interface
- **List Refresh**: Refresh list data incrementally or fully after create/edit operations
- **State Preservation**: Maintain current search, filters, and page position

## Integration Points

### Required Skill Dependencies
- **blazor-viewmodel-generator**: Provides ViewModel classes with Data Annotations validation for form binding
- **blazor-list-pattern-generator**: Provides list page structure with TODO placeholders
- **service-method-generator**: Uses service methods for create and update operations
- **component-library**: Integrates with modal, dialog, and navigation components

### Service Layer Integration
- **Create Methods**: Calls `service.CreateAsync()` methods
- **Update Methods**: Calls `service.UpdateAsync()` and `service.BulkUpdateAsync()` methods
- **Concurrency Handling**: Integrates with optimistic locking and version checking
- **Validation Integration**: Processes server-side validation results
- **Error Handling**: Converts service exceptions to user-friendly messages

### Form System Integration
- **Form Components**: Integrates with existing form components and ViewModel validation
- **Validation Systems**: Works with FluentValidation, DataAnnotations, etc.
- **Modal Libraries**: Integrates with MudBlazor dialogs, Bootstrap modals, etc.
- **Navigation Systems**: Works with Blazor routing and navigation managers

## Usage Scenarios

> **All scenarios use popup dialogs. Page navigation is not supported by this skill.**

### Scenario 1: Modal Create + Modal Edit (Primary Pattern)
**Input**: Entity with simple to moderate validation rules  
**Output**: `MudDialog` popup for create; `MudDialog` popup pre-populated for edit  
**Features**:
- ✅ Create button opens modal with empty form
- ✅ Edit button opens modal with pre-populated form
- ✅ Form validation with real-time feedback for both
- ✅ Success creates/updates item, closes modal, refreshes list
- ✅ Success notification (toast) displayed
- ✅ List state (filters, page, sort) unchanged after dialog closes

### Scenario 2: Slide-Over Drawer Create + Edit
**Input**: Complex entity with many fields where a full-width layout is preferred  
**Output**: Side drawer panel for create; side drawer panel pre-populated for edit  
**Features**:
- ✅ Create/Edit button opens a `MudDrawer` anchored to the right
- ✅ Full-height scrollable form with sections
- ✅ Edit drawer pre-populated with existing entity data
- ✅ Save/Cancel actions at the bottom of the drawer
- ✅ List remains visible in the background, context is always preserved

### Scenario 3: Inline Table Create + Inline Edit
**Input**: Simple entity suitable for inline operations  
**Output**: Add new row directly in table; click cell to edit in place  
**Features**:
- ✅ "Add New Row" button inserts editable row into table
- ✅ Click-to-edit for existing rows
- ✅ Real-time inline validation and error display
- ✅ Save/Cancel actions with keyboard shortcuts (Enter/Escape)
- ✅ Optimistic updates with error rollback

### Scenario 4: Bulk Edit
**Input**: Multiple selected records requiring field updates  
**Output**: Bulk edit interface with progress tracking  
**Features**:
- ✅ Multi-select with "Edit Selected" button
- ✅ Field-specific bulk update forms
- ✅ Progress indication for large update operations
- ✅ Partial success handling and error reporting

## Implementation Patterns

> All patterns use in-page dialogs or inline editing. There is no `NavigateTo()` call for create/edit operations.

### Modal Create Pattern
```csharp
private async Task ShowCreateModalAsync()
{
    var parameters = new DialogParameters
    {
        ["Entity"] = new RequestEntity(),
        ["OnSave"] = EventCallback.Factory.Create<RequestEntity>(this, OnItemCreated)
    };

    await DialogService.ShowAsync<CreateRequestModal>("Create New Request", parameters);
}

private async Task OnItemCreated(RequestEntity newEntity)
{
    await RefreshDataAsync();
    Snackbar.Add("Request created successfully!", Severity.Success);
}
```

### Modal Edit Pattern
```csharp
private async Task ShowEditModalAsync(RequestEntity entity)
{
    var editEntity = entity.Clone();

    var parameters = new DialogParameters
    {
        ["Entity"] = editEntity,
        ["OriginalEntity"] = entity,
        ["OnSave"] = EventCallback.Factory.Create<RequestEntity>(this, OnEntityUpdated),
        ["OnCancel"] = EventCallback.Factory.Create(this, () => Task.CompletedTask)
    };

    var options = new DialogOptions
    {
        MaxWidth = MaxWidth.Large,
        FullWidth = true,
        DisableBackdropClick = true
    };

    await DialogService.ShowAsync<EditRequestModal>($"Edit {entity.Title}", parameters, options);
}

private async Task OnEntityUpdated(RequestEntity updatedEntity)
{
    var originalIndex = _items.FindIndex(x => x.Id == updatedEntity.Id);
    if (originalIndex >= 0)
    {
        _items[originalIndex] = updatedEntity;
        StateHasChanged();
    }

    ShowSuccessMessage("Request updated successfully");
}
```

### Inline Edit Pattern
```csharp
private async Task StartInlineEditAsync(RequestEntity entity)
{
    if (_editingEntity != null)
    {
        var shouldContinue = await ConfirmUnsavedChangesAsync();
        if (!shouldContinue) return;
        await CancelInlineEditAsync();
    }

    _editingEntity = entity;
    _editingEntityCopy = entity.Clone();
    entity.IsInEditMode = true;
    StateHasChanged();
    await FocusFirstEditableFieldAsync();
}

private async Task SaveInlineEditAsync()
{
    if (_editingEntity == null) return;

    try
    {
        if (!await ValidateEntityAsync(_editingEntity)) return;

        _editingEntity.IsSaving = true;
        StateHasChanged();

        var updatedEntity = await RequestService.UpdateAsync(_editingEntity);
        var originalIndex = _items.IndexOf(_editingEntity);
        _items[originalIndex] = updatedEntity;

        await EndInlineEditAsync();
        ShowSuccessMessage("Changes saved successfully");
    }
    catch (ConcurrencyException ex)
    {
        await HandleConcurrencyConflictAsync(ex);
    }
    catch (ValidationException ex)
    {
        DisplayValidationErrors(ex.Errors);
    }
    finally
    {
        if (_editingEntity != null)
            _editingEntity.IsSaving = false;
    }
}

private async Task CancelInlineEditAsync()
{
    if (_editingEntity == null) return;

    var originalIndex = _items.IndexOf(_editingEntity);
    _items[originalIndex] = _editingEntityCopy;
    await EndInlineEditAsync();
}

private async Task EndInlineEditAsync()
{
    if (_editingEntity != null)
    {
        _editingEntity.IsInEditMode = false;
        _editingEntity = null;
        _editingEntityCopy = null;
    }

    StateHasChanged();
}
```

### Inline Create Pattern
```csharp
private void StartInlineCreation()
{
    _inlineCreateMode = true;
    _newItemTemplate = new RequestEntity
    {
        CreatedDate = DateTime.UtcNow,
        Status = RequestStatus.Draft
    };
    StateHasChanged();
}

private async Task SaveInlineItemAsync()
{
    try
    {
        _isCreatingInline = true;
        var result = await RequestService.CreateAsync(_newItemTemplate);
        _items.Insert(0, result);
        _inlineCreateMode = false;
        Snackbar.Add("Request created successfully!", Severity.Success);
    }
    catch (ValidationException ex)
    {
        DisplayValidationErrors(ex.Errors);
    }
    finally
    {
        _isCreatingInline = false;
        StateHasChanged();
    }
}
```

### Bulk Edit Pattern
```csharp
private async Task BulkEditSelectedAsync()
{
    var selectedItems = _items.Where(x => x.IsSelected).ToList();

    if (!selectedItems.Any())
    {
        ShowWarningMessage("No items selected for editing");
        return;
    }

    var bulkEditModel = new BulkEditModel
    {
        SelectedIds = selectedItems.Select(x => x.Id).ToList(),
        AvailableFields = GetBulkEditableFields()
    };

    var parameters = new DialogParameters
    {
        ["Model"] = bulkEditModel,
        ["OnSave"] = EventCallback.Factory.Create<BulkEditModel>(this, ProcessBulkEditAsync)
    };

    await DialogService.ShowAsync<BulkEditModal>(
        $"Edit {selectedItems.Count} Selected Items", parameters);
}
```

## Error Handling Patterns

### Validation Error Handling
```csharp
private async Task HandleFormSubmissionAsync(RequestEntity entity, bool isCreate)
{
    try
    {
        ClearErrors();
        SetLoading(true);

        if (isCreate)
        {
            var result = await RequestService.CreateAsync(entity);
            await OnSuccessfulCreation(result);
        }
        else
        {
            var result = await RequestService.UpdateAsync(entity);
            await OnSuccessfulUpdate(result);
        }
    }
    catch (ValidationException ex)
    {
        DisplayValidationErrors(ex.Errors);
    }
    catch (ConcurrencyException ex)
    {
        await HandleConcurrencyConflictAsync(ex);
    }
    catch (BusinessRuleException ex)
    {
        DisplayBusinessRuleError(ex.Message);
    }
    catch (Exception ex)
    {
        DisplayGeneralError("Operation failed. Please try again.");
        Logger.LogError(ex, "Error during {Operation} for entity", isCreate ? "create" : "update");
    }
    finally
    {
        SetLoading(false);
    }
}
```

### Concurrency Conflict Resolution
```csharp
private async Task HandleConcurrencyConflictAsync(ConcurrencyException ex)
{
    var resolution = await ShowConflictResolutionDialogAsync(
        current: _editingEntity,
        server: ex.ServerEntity,
        conflicts: ex.ConflictingFields);

    switch (resolution.Resolution)
    {
        case ConflictResolution.UseServerVersion:
            var idx = _items.IndexOf(_editingEntity);
            _items[idx] = ex.ServerEntity;
            await EndInlineEditAsync();
            break;

        case ConflictResolution.OverwriteServer:
            var updated = await RequestService.ForceUpdateAsync(
                _editingEntity, ex.ServerEntity.Version);
            var orig = _items.IndexOf(_editingEntity);
            _items[orig] = updated;
            await EndInlineEditAsync();
            ShowSuccessMessage("Changes saved (overrode server changes)");
            break;

        case ConflictResolution.Merge:
            ApplyMergedValues(_editingEntity, resolution.MergedValues);
            await SaveInlineEditAsync();
            break;

        case ConflictResolution.Cancel:
            await CancelInlineEditAsync();
            break;
    }
}
```

## Testing Strategies

### Playwright UI Tests (Primary — MANDATORY)

All create and edit functionality **MUST** be validated with Playwright browser-based tests.

**Required test coverage — Create:**

| Test Case | Description |
|---|---|
| Create button renders | Verify the "New" / "Create" button is present on the list page |
| Create button navigates | Click the button and verify navigation to create form or modal opens |
| Form fields render | Verify all required form fields are present |
| Validation blocks submit | Submit an empty/invalid form and verify validation messages appear |
| Successful creation | Fill valid data, submit, verify success notification and list refresh |
| Cancel returns to list | Click cancel and verify navigation back to list with no side effects |

**Required test coverage — Edit:**

| Test Case | Description |
|---|---|
| Edit button renders | Verify the Edit button is present on each list row |
| Edit button navigates | Click edit and verify navigation to edit form or dialog opens |
| Form pre-populated | Verify edit form fields contain existing entity data |
| Validation blocks save | Submit invalid data and verify validation messages appear |
| Successful edit | Modify data, save, verify success notification and list updates |
| Cancel discards changes | Click cancel and verify list is unchanged |

## Best Practices

### User Experience
- **Immediate Feedback**: Provide instant feedback for all user actions
- **Shared Form Components**: Reuse the same form layout for create and edit to reduce inconsistency
- **Clear Navigation**: Make create and edit entry points obvious
- **Error Recovery**: Provide clear paths to fix both create and edit errors
- **State Preservation**: Don't lose list search/filter/page context during form operations

### Performance
- **Lazy Loading**: Only load form components when needed
- **Efficient Updates**: Prefer incremental list updates over full refreshes
- **Memory Management**: Clean up form state properly after create/edit
- **Optimistic Updates**: Improve perceived performance for common operations

### Maintainability
- **Shared Logic**: Extract common create/edit patterns into shared methods where appropriate
- **Separation of Concerns**: Keep form logic separate from list logic
- **Error Logging**: Log errors appropriately for both create and edit operations
- **Documentation**: Document create and edit workflows and integration points

## Constraints and Limitations

- **Structure Dependency**: Requires list page structure from `blazor-list-pattern-generator`
- **Service Dependency**: Requires service layer with `CreateAsync()` and `UpdateAsync()` methods
- **Form Dependency**: Requires existing form components for non-inline operations
- **Permission Integration**: Must integrate with existing authorization systems
- **Validation Integration**: Must work with existing validation frameworks
- **Component Library Limits**: Limited to components available in the project's chosen library

## Integration Workflow

### Phase 1: Structure Analysis
1. **Analyze List Structure**: Review list page structure from `blazor-list-pattern-generator`
2. **Identify TODO Placeholders**: Find all create and edit TODO markers
3. **Review Entity Metadata**: Understand entity structure and validation requirements
4. **Assess Dialog Strategy**: Determine `MudDialog` modal vs slide-over drawer vs inline approach — page navigation is not an option

### Phase 2: Implementation
1. **Replace Create TODO Methods**: Implement all create-related placeholder methods
2. **Replace Edit TODO Methods**: Implement all edit-related placeholder methods
3. **Add Navigation Logic**: Implement navigation to forms or modal dialogs for both
4. **Integrate Validation**: Connect with form validation systems
5. **Add Error Handling**: Implement comprehensive error handling and user feedback

### Phase 3: Test Generation
1. **Generate Add Playwright Tests**: Cover the full create workflow
2. **Generate Edit Playwright Tests**: Cover the full edit workflow including pre-population
3. **Test Error Scenarios**: Verify error handling and recovery for both flows

## Key Principles
1. **Unified Dialog Strategy**: Apply the same popup/dialog pattern to both create and edit for consistency — no page navigation
2. **Seamless Integration**: Integrate smoothly with the existing list page structure
3. **User-First Design**: Prioritize user experience in both create and edit workflows
4. **Error Resilience**: Handle all error scenarios — validation, concurrency, network — gracefully
5. **Performance Conscious**: Optimize for responsive create and edit operations
6. **Maintainable Code**: Write clean, testable, and maintainable implementations

## Final Step: Code Formatting

After implementing all create and edit operations and generating Playwright tests, the skill calls `solution-code-formatter` to ensure all generated code follows proper formatting standards:
```bash
bun run ../../utilities/solution-code-formatter/scripts/format-solution.ts [solution-path]
```
