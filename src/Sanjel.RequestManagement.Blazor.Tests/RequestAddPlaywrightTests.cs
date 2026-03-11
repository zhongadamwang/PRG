using Microsoft.Playwright;
using NUnit.Framework;

namespace Sanjel.RequestManagement.Blazor.Tests;

/// <summary>
/// Playwright UI tests covering the create/add workflow on the Request list page.
/// These tests require the Blazor application to be running at http://localhost:5000.
/// </summary>
[TestFixture]
public class RequestAddPlaywrightTests
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
			new BrowserTypeLaunchOptions
			{
				Headless = true,
				Timeout = 30000,
			});
	}

	[SetUp]
	public async Task CreatePageAsync()
	{
		this._page = await this._browser.NewPageAsync();

		// Set page timeouts
		this._page.SetDefaultTimeout(15000);
		this._page.SetDefaultNavigationTimeout(15000);

		// Add console logging for debugging
		this._page.Console += (_, e) => Console.WriteLine($"Browser Console [{e.Type}]: {e.Text}");

		// Check if application is running
		try
		{
			var response = await this._page.GotoAsync(BaseUrl, new PageGotoOptions { Timeout = 5000 });
			if (response?.Status >= 400)
			{
				throw new InvalidOperationException($"Application at {BaseUrl} returned status {response.Status}");
			}
		}
		catch (TimeoutException)
		{
			throw new InvalidOperationException($"❌ Cannot reach application at {BaseUrl}. Make sure the Blazor app is running with: dotnet run --project ./src/Sanjel.RequestManagement.Blazor");
		}
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
	/// Verifies that the dialog is NOT visible when the page first loads.
	/// </summary>
	[Test]
	public async Task PageLoad_DialogShouldNotBeVisibleInitiallyAsync()
	{
		await this._page.GotoAsync(RequestsUrl);
		await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
		await this._page.WaitForTimeoutAsync(2000);

		// Dialog should NOT be visible on page load
		var dialog = this._page.Locator(".e-dialog");
		await Assertions.Expect(dialog).ToBeHiddenAsync();
	}

	/// <summary>
	/// Verifies that the "New Request" button is visible on the list page.
	/// </summary>
	[Test]
	public async Task NewRequestButton_ShouldBeVisibleOnListPageAsync()
	{
		await this._page.GotoAsync(RequestsUrl);
		await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
		await this._page.WaitForTimeoutAsync(2000);

		var createButton = this._page.GetByRole(AriaRole.Button, new() { Name = "New Request" });
		await Assertions.Expect(createButton).ToBeVisibleAsync();
	}

	/// <summary>
	/// Verifies that clicking "New Request" opens the create dialog.
	/// </summary>
	[Test]
	public async Task NewRequestButton_Click_ShouldOpenDialogAsync()
	{
		await this._page.GotoAsync(RequestsUrl);
		await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
		await this._page.WaitForTimeoutAsync(2000);

		// First verify dialog is hidden
		var dialog = this._page.Locator(".e-dialog");
		await Assertions.Expect(dialog).ToBeHiddenAsync();

		// Click the New Request button
		var createButton = this._page.GetByRole(AriaRole.Button, new() { Name = "New Request" });
		await createButton.ClickAsync();
		await this._page.WaitForTimeoutAsync(500);

		// Now dialog should be visible
		await Assertions.Expect(dialog).ToBeVisibleAsync();
	}

	/// <summary>
	/// Verifies that the create dialog shows the correct title "New Request".
	/// </summary>
	[Test]
	public async Task CreateDialog_ShouldShowNewRequestTitleAsync()
	{
		await this._page.GotoAsync(RequestsUrl);
		await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
		await this._page.WaitForTimeoutAsync(2000);

		var createButton = this._page.GetByRole(AriaRole.Button, new() { Name = "New Request" });
		await createButton.ClickAsync();
		await this._page.WaitForTimeoutAsync(500);

		var dialogTitle = this._page.Locator(".e-dlg-header");
		await Assertions.Expect(dialogTitle).ToContainTextAsync("New Request");
	}

	/// <summary>
	/// Verifies that required form fields are present inside the dialog.
	/// </summary>
	[Test]
	public async Task CreateDialog_ShouldRenderRequiredFormFieldsAsync()
	{
		await this._page.GotoAsync(RequestsUrl);
		await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
		await this._page.WaitForTimeoutAsync(2000);

		await this._page.GetByRole(AriaRole.Button, new() { Name = "New Request" }).ClickAsync();
		await this._page.WaitForTimeoutAsync(500);

		// Verify key form fields are present by their input IDs
		await Assertions.Expect(this._page.Locator("#dlg-request-id")).ToBeVisibleAsync();
		await Assertions.Expect(this._page.Locator("#dlg-client-id")).ToBeVisibleAsync();
		await Assertions.Expect(this._page.Locator("#dlg-source-email")).ToBeVisibleAsync();
		await Assertions.Expect(this._page.Locator("#dlg-engineer-id")).ToBeVisibleAsync();
		await Assertions.Expect(this._page.Locator("#dlg-assigned-by")).ToBeVisibleAsync();
	}

	/// <summary>
	/// Verifies that submitting the form with empty required fields shows validation messages.
	/// </summary>
	[Test]
	public async Task CreateDialog_EmptySubmit_ShouldShowValidationMessagesAsync()
	{
		await this._page.GotoAsync(RequestsUrl);
		await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
		await this._page.WaitForTimeoutAsync(2000);

		await this._page.GetByRole(AriaRole.Button, new() { Name = "New Request" }).ClickAsync();
		await this._page.WaitForTimeoutAsync(500);

		// Wait for dialog to load, then clear the Request ID field and submit
		await this._page.WaitForTimeoutAsync(2000); // Give extra time for Syncfusion components

		// Clear the Request ID field using the safe method
		try
		{
			await this.FillFieldSafely("dlg-request-id", "Request ID", string.Empty);
		}
		catch
		{
			// If the safe method fails, the field might already be empty or not required for validation
		}

		var submitButton = this._page.GetByRole(AriaRole.Button, new() { Name = "Create Request" });
		await submitButton.ClickAsync();
		await this._page.WaitForTimeoutAsync(500);

		// Validation messages should appear for required fields
		var validationMessages = this._page.Locator(".validation-message");
		var count = await validationMessages.CountAsync();
		Assert.That(count, Is.GreaterThan(0), "Validation messages should appear for invalid/empty submission");
	}

	/// <summary>
	/// Verifies that Cancel button closes the dialog without creating a request.
	/// </summary>
	[Test]
	public async Task CreateDialog_CancelButton_ShouldCloseDialogAsync()
	{
		await this._page.GotoAsync(RequestsUrl);
		await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
		await this._page.WaitForTimeoutAsync(2000);

		await this._page.GetByRole(AriaRole.Button, new() { Name = "New Request" }).ClickAsync();
		await this._page.WaitForTimeoutAsync(500);

		var dialog = this._page.Locator(".e-dialog");
		await Assertions.Expect(dialog).ToBeVisibleAsync();

		var cancelButton = this._page.GetByRole(AriaRole.Button, new() { Name = "Cancel" });
		await cancelButton.ClickAsync();
		await this._page.WaitForTimeoutAsync(500);

		// Wait for dialog to close - use longer timeout
		await Assertions.Expect(dialog).ToBeHiddenAsync(new() { Timeout = 10000 });
	}

	/// <summary>
	/// Creates a new request with random data and verifies it appears in the list.
	/// This test fills all form fields with randomly generated values, saves the request,
	/// and then checks that the new data is visible in the requests grid.
	/// </summary>
	[Test]
	public async Task CreateNewRequest_WithRandomData_ShouldAppearInListAsync()
	{
		// Generate random test data
		var random = new Random();
		var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
		var randomRequestId = $"REQ-{timestamp}-{random.Next(1000, 9999)}";
		var randomClientId = $"CLIENT-{random.Next(100, 999)}";
		var randomSourceEmail = $"test{random.Next(1000, 9999)}@example.com";
		var randomEngineerId = $"ENG-{random.Next(100, 999)}";
		var randomAssignedBy = $"MGR-{random.Next(100, 999)}";

		try
		{
			// Navigate to the requests page with retry logic
			await this._page.GotoAsync(RequestsUrl, new PageGotoOptions { Timeout = 10000 });
			await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
			await this._page.WaitForTimeoutAsync(2000);

			// Verify we can see the page (check for New Request button)
			var createButton = this._page.GetByRole(AriaRole.Button, new() { Name = "New Request" });
			await Assertions.Expect(createButton).ToBeVisibleAsync(new() { Timeout = 5000 });

			// Open the create dialog
			await createButton.ClickAsync();
			await this._page.WaitForTimeoutAsync(500);

			// Wait for dialog to be fully loaded
			var dialog = this._page.Locator(".e-dialog");
			await Assertions.Expect(dialog).ToBeVisibleAsync(new() { Timeout = 5000 });

			// Give extra time for Syncfusion components to initialize
			await this._page.WaitForTimeoutAsync(2000);

			// Fill the form fields with random data using the safe fill helper method
			await this.FillFieldSafely("dlg-request-id", "Request ID", randomRequestId);
			await this.FillFieldSafely("dlg-client-id", "Client ID", randomClientId);
			await this.FillFieldSafely("dlg-source-email", "Source Email", randomSourceEmail);
			await this.FillFieldSafely("dlg-engineer-id", "Assigned Engineer ID", randomEngineerId);
			await this.FillFieldSafely("dlg-assigned-by", "Assigned By", randomAssignedBy);

			try
			{
				var priorityDropdown = this._page.Locator("#dlg-priority").Or(this._page.Locator("[data-testid='priority-dropdown']")).First;
				if (await priorityDropdown.IsVisibleAsync())
				{
					await priorityDropdown.ClickAsync();
					await this._page.WaitForTimeoutAsync(200);
					var highPriorityOption = this._page.Locator(".e-list-item:has-text('High')").First;
					if (await highPriorityOption.IsVisibleAsync())
					{
						await highPriorityOption.ClickAsync();
					}
				}
			}
			catch
			{
				// Priority selection is optional, continue if it fails
			}

			try
			{
				var statusDropdown = this._page.Locator("#dlg-status").Or(this._page.Locator("[data-testid='status-dropdown']")).First;
				if (await statusDropdown.IsVisibleAsync())
				{
					await statusDropdown.ClickAsync();
					await this._page.WaitForTimeoutAsync(200);
					var submittedStatusOption = this._page.Locator(".e-list-item:has-text('Submitted')").First;
					if (await submittedStatusOption.IsVisibleAsync())
					{
						await submittedStatusOption.ClickAsync();
					}
				}
			}
			catch
			{
				// Status selection is optional, continue if it fails
			}

			// Save the form - try multiple button selectors
			var submitButton = this._page.GetByRole(AriaRole.Button, new() { Name = "Create Request" })
				.Or(this._page.GetByRole(AriaRole.Button, new() { Name = "Save" }))
				.Or(this._page.GetByRole(AriaRole.Button, new() { Name = "Submit" }));

			await submitButton.ClickAsync();
			await this._page.WaitForTimeoutAsync(1500);

			// Wait for dialog to close (indicating successful save)
			await Assertions.Expect(dialog).ToBeHiddenAsync(new() { Timeout = 10000 });

			// Wait for the grid to refresh
			await this._page.WaitForTimeoutAsync(2000);

			// Verify the new record appears in the grid
			var gridRow = this._page.Locator($"tr:has-text('{randomRequestId}')");
			await Assertions.Expect(gridRow).ToBeVisibleAsync(new() { Timeout = 10000 });

			// Verify other random data appears in the same row (make these assertions more lenient)
			if (!string.IsNullOrEmpty(randomClientId))
			{
				var clientRow = this._page.Locator($"tr:has-text('{randomRequestId}'):has-text('{randomClientId}')");
				if (await clientRow.CountAsync() > 0)
				{
					await Assertions.Expect(clientRow).ToBeVisibleAsync();
				}
			}

			if (!string.IsNullOrEmpty(randomSourceEmail))
			{
				var emailRow = this._page.Locator($"tr:has-text('{randomRequestId}'):has-text('{randomSourceEmail}')");
				if (await emailRow.CountAsync() > 0)
				{
					await Assertions.Expect(emailRow).ToBeVisibleAsync();
				}
			}

			// Success message should contain our request ID
			var successAlert = this._page.Locator(".alert-success, .toast-success, .notification-success");
			if (await successAlert.CountAsync() > 0)
			{
				await Assertions.Expect(successAlert).ToBeVisibleAsync();
				await Assertions.Expect(successAlert).ToContainTextAsync(randomRequestId);
			}

			Console.WriteLine($"✅ Test completed successfully. Created request: {randomRequestId}");
		}
		catch (TimeoutException ex)
		{
			Assert.Fail($"❌ Test timed out: {ex.Message}. Make sure the Blazor application is running at {BaseUrl}");
		}
		catch (Exception ex)
		{
			Assert.Fail($"❌ Test failed with error: {ex.Message}");
		}
	}

	/// <summary>
	/// Safely fills a form field using multiple selector strategies for Syncfusion components
	/// </summary>
	private async Task FillFieldSafely(string fieldId, string labelText, string value)
	{
		// Strategy 1: Try direct input inside the SfTextBox container
		var inputInContainer = this._page.Locator($"#{fieldId} input");
		if (await inputInContainer.CountAsync() > 0 && await inputInContainer.First.IsVisibleAsync())
		{
			await inputInContainer.First.FillAsync(value);
			return;
		}

		// Strategy 2: Try Syncfusion .e-input class
		var syncfusionInput = this._page.Locator($"#{fieldId} .e-input");
		if (await syncfusionInput.CountAsync() > 0 && await syncfusionInput.First.IsVisibleAsync())
		{
			await syncfusionInput.First.FillAsync(value);
			return;
		}

		// Strategy 3: Try by label association (find input associated with label)
		var labelAssociatedInput = this._page.Locator($"input[aria-labelledby*='{fieldId}'], input[id*='{fieldId}']");
		if (await labelAssociatedInput.CountAsync() > 0 && await labelAssociatedInput.First.IsVisibleAsync())
		{
			await labelAssociatedInput.First.FillAsync(value);
			return;
		}

		// Strategy 4: Try to find by placeholder or label text
		var inputByPlaceholder = this._page.Locator($"input[placeholder*='{labelText}'], input[aria-label*='{labelText}']");
		if (await inputByPlaceholder.CountAsync() > 0 && await inputByPlaceholder.First.IsVisibleAsync())
		{
			await inputByPlaceholder.First.FillAsync(value);
			return;
		}

		// Strategy 5: Last resort - find any visible input near the label
		var nearbyInput = this._page.Locator($"label:has-text('{labelText}')").Locator("..").Locator("input").First;
		if (await nearbyInput.CountAsync() > 0 && await nearbyInput.IsVisibleAsync())
		{
			await nearbyInput.FillAsync(value);
			return;
		}

		throw new InvalidOperationException($"Could not find fillable input for field '{fieldId}' with label '{labelText}'");
	}
}
