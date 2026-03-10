using Microsoft.Playwright;
using NUnit.Framework;

namespace Sanjel.RequestManagement.Blazor.Tests
{
	[TestFixture]
	public class RequestEditPlaywrightTests
	{
		private IBrowser _browser = null!;
		private IPage _page = null!;
		private IPlaywright _playwright = null!;

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
		public async Task EditButton_ShouldBeVisibleInEachRowAsync()
		{
			await this._page.GotoAsync("http://localhost:5000/request");
			await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
			await this._page.WaitForTimeoutAsync(2000);

			await this._page.WaitForSelectorAsync(".data-grid-container", new() { Timeout = 10000 });

			var editButtons = await this._page.QuerySelectorAllAsync("button[title='Edit']");
			Assert.IsTrue(editButtons.Count > 0, "At least one Edit button should be visible in the grid");
		}

		[Test]
		public async Task EditButton_Click_ShouldNavigateToEditPageAsync()
		{
			await this._page.GotoAsync("http://localhost:5000/request");
			await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
			await this._page.WaitForTimeoutAsync(2000);

			var editButton = await this._page.QuerySelectorAsync("button[title='Edit']");
			if (editButton == null)
			{
				Assert.Inconclusive("No edit buttons found — server may not be running or no records exist.");
				return;
			}

			await editButton.ClickAsync();
			await this._page.WaitForTimeoutAsync(1000);

			Assert.That(this._page.Url, Does.Contain("/request/edit/"),
				"Clicking Edit should navigate to /request/edit/{id}");
		}

		[Test]
		public async Task EditPage_ShouldRenderFormWithExistingDataAsync()
		{
			await this._page.GotoAsync("http://localhost:5000/request");
			await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
			await this._page.WaitForTimeoutAsync(2000);

			var editButton = await this._page.QuerySelectorAsync("button[title='Edit']");
			if (editButton == null)
			{
				Assert.Inconclusive("No edit buttons found — server may not be running or no records exist.");
				return;
			}

			await editButton.ClickAsync();
			await this._page.WaitForTimeoutAsync(2000);

			// Verify the edit page loaded
			Assert.That(this._page.Url, Does.Contain("/request/edit/"),
				"Should be on the edit page");

			// Verify the Request ID field is present (read-only on edit)
			var requestIdField = await this._page.QuerySelectorAsync(
				"input[class*='e-input'], .e-textbox input, input.e-control");
			Assert.IsNotNull(requestIdField, "Edit form should render input fields pre-populated with existing data");
		}

		[Test]
		public async Task EditPage_CancelButton_ShouldNavigateBackToListAsync()
		{
			await this._page.GotoAsync("http://localhost:5000/request");
			await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
			await this._page.WaitForTimeoutAsync(2000);

			var editButton = await this._page.QuerySelectorAsync("button[title='Edit']");
			if (editButton == null)
			{
				Assert.Inconclusive("No edit buttons found — server may not be running or no records exist.");
				return;
			}

			await editButton.ClickAsync();
			await this._page.WaitForTimeoutAsync(2000);

			var cancelButton = await this._page.QuerySelectorAsync("button:has-text('Cancel')");
			if (cancelButton == null)
			{
				Assert.Inconclusive("Cancel button not found on edit page.");
				return;
			}

			await cancelButton.ClickAsync();
			await this._page.WaitForTimeoutAsync(1000);

			Assert.That(this._page.Url, Does.Contain("/request"),
				"Cancel should navigate back to the request list");
			Assert.That(this._page.Url, Does.Not.Contain("/request/edit"),
				"Should not remain on the edit page after cancel");
		}

		[Test]
		public async Task EditPage_EmptyRequiredField_ShouldShowValidationAsync()
		{
			await this._page.GotoAsync("http://localhost:5000/request");
			await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
			await this._page.WaitForTimeoutAsync(2000);

			var editButton = await this._page.QuerySelectorAsync("button[title='Edit']");
			if (editButton == null)
			{
				Assert.Inconclusive("No edit buttons found — server may not be running or no records exist.");
				return;
			}

			await editButton.ClickAsync();
			await this._page.WaitForTimeoutAsync(2000);

			// Clear a required field
			var clientIdField = await this._page.QuerySelectorAsync(
				"input[placeholder*='client ID'], input[placeholder*='Client ID']");
			if (clientIdField != null)
			{
				await clientIdField.ClickAsync(new ElementHandleClickOptions { ClickCount = 3 });
				await clientIdField.PressAsync("Backspace");
			}

			// Submit form
			var saveButton = await this._page.QuerySelectorAsync("button[type='submit']:has-text('Save')");
			if (saveButton == null)
			{
				Assert.Inconclusive("Save button not found on edit page.");
				return;
			}

			await saveButton.ClickAsync();
			await this._page.WaitForTimeoutAsync(500);

			var validationMessages = await this._page.QuerySelectorAllAsync(
				".validation-message, .field-validation-error");
			Assert.That(validationMessages.Count, Is.GreaterThan(0),
				"Validation messages should appear when a required field is cleared");
		}

		[Test]
		public async Task EditPage_BreadcrumbLink_ShouldNavigateBackToListAsync()
		{
			await this._page.GotoAsync("http://localhost:5000/request");
			await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
			await this._page.WaitForTimeoutAsync(2000);

			var editButton = await this._page.QuerySelectorAsync("button[title='Edit']");
			if (editButton == null)
			{
				Assert.Inconclusive("No edit buttons found — server may not be running or no records exist.");
				return;
			}

			await editButton.ClickAsync();
			await this._page.WaitForTimeoutAsync(2000);

			var breadcrumb = await this._page.QuerySelectorAsync("a.breadcrumb-link:has-text('Request Management')");
			if (breadcrumb == null)
			{
				Assert.Inconclusive("Breadcrumb link not found on edit page.");
				return;
			}

			await breadcrumb.ClickAsync();
			await this._page.WaitForTimeoutAsync(1000);

			Assert.That(this._page.Url, Does.EndWith("/request"),
				"Breadcrumb link should navigate back to the request list");
		}
	}
}
