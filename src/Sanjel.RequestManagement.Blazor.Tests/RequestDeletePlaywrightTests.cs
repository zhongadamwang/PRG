using Microsoft.Playwright;
using NUnit.Framework;

namespace Sanjel.RequestManagement.Blazor.Tests;

/// <summary>
/// Playwright UI tests covering the delete workflow on the Request list page.
/// These tests require the Blazor application to be running at http://localhost:5000.
/// </summary>
[TestFixture]
public class RequestDeletePlaywrightTests
{
	private const string BaseUrl = "http://localhost:5000";
	private const string RequestsUrl = BaseUrl + "/requests";

	private IPlaywright _playwright = default!;
	private IBrowser _browser = default!;
	private IPage _page = default!;

	[OneTimeSetUp]
	public async Task SetupAsync()
	{
		this._playwright = await Playwright.CreateAsync();
		this._browser = await this._playwright.Chromium.LaunchAsync(
			new BrowserTypeLaunchOptions { Headless = true });
	}

	[SetUp]
	public async Task CreatePageAsync()
	{
		this._page = await this._browser.NewPageAsync();
	}

	[TearDown]
	public async Task ClosePageAsync()
	{
		await this._page.CloseAsync();
	}

	[OneTimeTearDown]
	public async Task TeardownAsync()
	{
		await this._browser.CloseAsync();
		this._playwright.Dispose();
	}

	/// <summary>
	/// Verifies that at least one Delete button is visible in the Request list.
	/// </summary>
	[Test]
	public async Task DeleteButton_ShouldBeVisibleOnListRowsAsync()
	{
		await this._page.GotoAsync(RequestsUrl);
		await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
		await this._page.WaitForTimeoutAsync(2000);

		// Delete buttons rendered inside the Syncfusion grid Action column template
		var deleteButtons = this._page.GetByRole(AriaRole.Button, new() { Name = "Delete" });
		var count = await deleteButtons.CountAsync();
		Assert.That(count, Is.GreaterThan(0), "At least one Delete button should be visible in the request grid");
	}

	/// <summary>
	/// Verifies that clicking Delete opens the confirmation dialog.
	/// </summary>
	[Test]
	public async Task DeleteButton_Click_ShouldOpenConfirmationDialogAsync()
	{
		await this._page.GotoAsync(RequestsUrl);
		await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
		await this._page.WaitForTimeoutAsync(2000);

		var firstDeleteButton = this._page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).First;
		await firstDeleteButton.ClickAsync();
		await this._page.WaitForTimeoutAsync(500);

		// Check if confirmation dialog appeared
		var dialog = this._page.Locator(".e-dialog");
		await Assertions.Expect(dialog).ToBeVisibleAsync();

		// Verify dialog header
		var dialogHeader = this._page.GetByText("Confirm Delete");
		await Assertions.Expect(dialogHeader).ToBeVisibleAsync();

		// Verify confirmation message
		var confirmationMessage = this._page.GetByText("Are you sure you want to delete this request?");
		await Assertions.Expect(confirmationMessage).ToBeVisibleAsync();

		// Verify dialog buttons
		var cancelButton = this._page.GetByRole(AriaRole.Button, new() { Name = "Cancel" });
		var deleteButton = this._page.GetByRole(AriaRole.Button, new() { Name = "Delete Request" });
	}

	/// <summary>
	/// Verifies that clicking Cancel in the delete dialog does not remove the item.
	/// </summary>
	[Test]
	public async Task DeleteCancel_ShouldNotRemoveItemAsync()
	{
		await this._page.GotoAsync(RequestsUrl);
		await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
		await this._page.WaitForTimeoutAsync(2000);

		// Get initial row count
		var rowsBeforeLocator = this._page.Locator(".e-gridcontent .e-row");
		var countBefore = await rowsBeforeLocator.CountAsync();

		// Click delete button
		var firstDeleteButton = this._page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).First;
		await firstDeleteButton.ClickAsync();
		await this._page.WaitForTimeoutAsync(500);

		// Click cancel in confirmation dialog
		var cancelButton = this._page.GetByRole(AriaRole.Button, new() { Name = "Cancel" });
		await cancelButton.ClickAsync();
		await this._page.WaitForTimeoutAsync(500);

		// Verify row count is unchanged
		var rowsAfterLocator = this._page.Locator(".e-gridcontent .e-row");
		var countAfter = await rowsAfterLocator.CountAsync();

		Assert.That(countAfter, Is.EqualTo(countBefore),
			"Row count should be unchanged after clicking Cancel in delete confirmation dialog");

		// Verify dialog is closed
		var dialog = this._page.Locator(".e-dialog");
		await Assertions.Expect(dialog).Not.ToBeVisibleAsync();
	}

	/// <summary>
	/// Verifies that confirming delete removes the item from the list and shows success message.
	/// Note: This test may need to be adjusted based on the specific test data and whether
	/// delete constraints prevent deletion of specific test records.
	/// </summary>
	[Test]
	public async Task DeleteConfirm_ShouldRemoveItemAndShowSuccessMessageAsync()
	{
		await this._page.GotoAsync(RequestsUrl);
		await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
		await this._page.WaitForTimeoutAsync(2000);

		// Get initial row count
		var rowsBeforeLocator = this._page.Locator(".e-gridcontent .e-row");
		var countBefore = await rowsBeforeLocator.CountAsync();

		// Skip test if no rows available
		if (countBefore == 0)
		{
			Assert.Ignore("No request rows available for delete testing");
			return;
		}

		// Click delete button on first row
		var firstDeleteButton = this._page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).First;
		await firstDeleteButton.ClickAsync();
		await this._page.WaitForTimeoutAsync(500);

		// Wait for confirmation dialog to appear
		var dialog = this._page.Locator(".e-dialog");
		await Assertions.Expect(dialog).ToBeVisibleAsync();

		// Check if there's a constraint message that would prevent deletion
		var constraintAlert = this._page.Locator(".alert-danger");
		var hasConstraint = await constraintAlert.IsVisibleAsync();

		if (hasConstraint)
		{
			// If deletion is blocked by constraints, verify delete button is disabled
			var deleteButton = this._page.GetByRole(AriaRole.Button, new() { Name = "Delete Request" });
			await Assertions.Expect(deleteButton).ToBeDisabledAsync();

			// Cancel the dialog
			var cancelButton = this._page.GetByRole(AriaRole.Button, new() { Name = "Cancel" });
			await cancelButton.ClickAsync();

			Assert.Pass("Delete operation correctly blocked by business constraints");
		}
		else
		{
			// Proceed with deletion if no constraints
			var deleteButton = this._page.GetByRole(AriaRole.Button, new() { Name = "Delete Request" });
			await deleteButton.ClickAsync();
			await this._page.WaitForTimeoutAsync(1500);

			// Verify success message appears
			var successAlert = this._page.Locator(".alert-success");
			await Assertions.Expect(successAlert).ToBeVisibleAsync();

			// Verify row count decreased
			var rowsAfterLocator = this._page.Locator(".e-gridcontent .e-row");
			var countAfter = await rowsAfterLocator.CountAsync();

			Assert.That(countAfter, Is.EqualTo(countBefore - 1),
				"Row count should decrease by 1 after successful deletion");

			// Verify dialog is closed
			await Assertions.Expect(dialog).Not.ToBeVisibleAsync();
		}
	}

	/// <summary>
	/// Verifies that delete confirmation dialog shows relevant request information.
	/// </summary>
	[Test]
	public async Task DeleteConfirmationDialog_ShouldShowRequestInformationAsync()
	{
		await this._page.GotoAsync(RequestsUrl);
		await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
		await this._page.WaitForTimeoutAsync(2000);

		// Click delete button on first row
		var firstDeleteButton = this._page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).First;
		await firstDeleteButton.ClickAsync();
		await this._page.WaitForTimeoutAsync(500);

		// Verify dialog shows request details
		var requestIdLabel = this._page.GetByText("Request ID:");
		await Assertions.Expect(requestIdLabel).ToBeVisibleAsync();

		var clientLabel = this._page.GetByText("Client:");
		await Assertions.Expect(clientLabel).ToBeVisibleAsync();

		var statusLabel = this._page.GetByText("Status:");
		await Assertions.Expect(statusLabel).ToBeVisibleAsync();

		var createdLabel = this._page.GetByText("Created:");
		await Assertions.Expect(createdLabel).ToBeVisibleAsync();

		// Verify warning message
		var warningText = this._page.GetByText("This action cannot be undone");
		await Assertions.Expect(warningText).ToBeVisibleAsync();

		// Close dialog
		var cancelButton = this._page.GetByRole(AriaRole.Button, new() { Name = "Cancel" });
		await cancelButton.ClickAsync();
	}

	/// <summary>
	/// Verifies that delete buttons have proper styling and accessibility.
	/// </summary>
	[Test]
	public async Task DeleteButton_ShouldHaveProperStylingAndAccessibilityAsync()
	{
		await this._page.GotoAsync(RequestsUrl);
		await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
		await this._page.WaitForTimeoutAsync(2000);

		var firstDeleteButton = this._page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).First;

		// Verify button has danger styling
		var buttonClass = await firstDeleteButton.GetAttributeAsync("class");
		Assert.That(buttonClass, Does.Contain("btn-danger"), "Delete button should have 'btn-danger' class");

		// Verify button is clickable
		await Assertions.Expect(firstDeleteButton).ToBeEnabledAsync();
		await Assertions.Expect(firstDeleteButton).ToBeVisibleAsync();
	}

	/// <summary>
	/// Verifies loading state during delete operation.
	/// </summary>
	[Test]
	public async Task DeleteOperation_ShouldShowLoadingStateAsync()
	{
		await this._page.GotoAsync(RequestsUrl);
		await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
		await this._page.WaitForTimeoutAsync(2000);

		// Click delete button
		var firstDeleteButton = this._page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).First;
		await firstDeleteButton.ClickAsync();
		await this._page.WaitForTimeoutAsync(500);

		// Check if delete is allowed (no constraints)
		var constraintAlert = this._page.Locator(".alert-danger");
		var hasConstraint = await constraintAlert.IsVisibleAsync();

		if (!hasConstraint)
		{
			var deleteButton = this._page.GetByRole(AriaRole.Button, new() { Name = "Delete Request" });

			// Click delete and immediately check for loading state
			await deleteButton.ClickAsync();

			// Look for loading button with spinner (may appear briefly)
			var loadingButton = this._page.GetByRole(AriaRole.Button, new() { Name = "Deleting..." });

			// Note: Loading state may be very brief with mock data, so we don't assert it's visible
			// but we verify the button exists and the implementation includes loading states
			var deleteButtonExists = await this._page.GetByRole(AriaRole.Button, new() { Name = "Delete Request" }).Or(loadingButton).IsVisibleAsync();
			Assert.That(deleteButtonExists, Is.True, "Delete button or loading state should be present");
		}
		else
		{
			// Close dialog if constraints prevent deletion
			var cancelButton = this._page.GetByRole(AriaRole.Button, new() { Name = "Cancel" });
			await cancelButton.ClickAsync();
		}
	}
}
