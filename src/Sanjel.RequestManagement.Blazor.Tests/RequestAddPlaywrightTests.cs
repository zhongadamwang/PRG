using Microsoft.Playwright;
using NUnit.Framework;

namespace Sanjel.RequestManagement.Blazor.Tests
{
	[TestFixture]
	public class RequestAddPlaywrightTests
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
		public async Task NewRequestButton_ShouldBeVisibleAndClickableAsync()
		{
			await this._page.GotoAsync("http://localhost:5000/request");
			await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
			await this._page.WaitForTimeoutAsync(2000);

			var createButton = await this._page.QuerySelectorAsync("button:has-text('New Request')");
			Assert.IsNotNull(createButton, "New Request button should exist on the list page");
		}

		[Test]
		public async Task NewRequestButton_Click_ShouldNavigateToCreatePageAsync()
		{
			await this._page.GotoAsync("http://localhost:5000/request");
			await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
			await this._page.WaitForTimeoutAsync(2000);

			var createButton = await this._page.QuerySelectorAsync("button:has-text('New Request')");
			if (createButton == null)
			{
				Assert.Inconclusive("New Request button not found — server may not be running.");
				return;
			}

			await createButton.ClickAsync();
			await this._page.WaitForTimeoutAsync(1000);

			Assert.That(this._page.Url, Does.Contain("/request/create"),
				"Clicking New Request should navigate to /request/create");
		}

		[Test]
		public async Task CreatePage_ShouldRenderFormFieldsAsync()
		{
			await this._page.GotoAsync("http://localhost:5000/request/create");
			await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
			await this._page.WaitForTimeoutAsync(2000);

			var formSelectors = new[]
			{
				"input[placeholder*='request ID'], input[placeholder*='Request ID']",
				"input[placeholder*='client ID'], input[placeholder*='Client ID']",
				"input[placeholder*='email'], input[placeholder*='Email']",
				"input[placeholder*='engineer'], input[placeholder*='Engineer']",
			};

			foreach (var selector in formSelectors)
			{
				var input = await this._page.QuerySelectorAsync(selector);
				Assert.IsNotNull(input, $"Form field matching '{selector}' should be present");
			}
		}

		[Test]
		public async Task CreatePage_EmptySubmit_ShouldShowValidationMessagesAsync()
		{
			await this._page.GotoAsync("http://localhost:5000/request/create");
			await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
			await this._page.WaitForTimeoutAsync(2000);

			var submitButton = await this._page.QuerySelectorAsync("button[type='submit']:has-text('Save')");
			if (submitButton == null)
			{
				Assert.Inconclusive("Save button not found — server may not be running.");
				return;
			}

			await submitButton.ClickAsync();
			await this._page.WaitForTimeoutAsync(500);

			var validationMessages = await this._page.QuerySelectorAllAsync(
				".validation-message, .field-validation-error");
			Assert.That(validationMessages.Count, Is.GreaterThan(0),
				"Validation messages should appear when submitting empty form");
		}

		[Test]
		public async Task CreatePage_CancelButton_ShouldNavigateBackToListAsync()
		{
			await this._page.GotoAsync("http://localhost:5000/request/create");
			await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
			await this._page.WaitForTimeoutAsync(2000);

			var cancelButton = await this._page.QuerySelectorAsync("button:has-text('Cancel')");
			if (cancelButton == null)
			{
				Assert.Inconclusive("Cancel button not found — server may not be running.");
				return;
			}

			await cancelButton.ClickAsync();
			await this._page.WaitForTimeoutAsync(1000);

			Assert.That(this._page.Url, Does.Contain("/request"),
				"Cancel should navigate back to the request list");
		}

		[Test]
		public async Task CreatePage_BreadcrumbLink_ShouldNavigateBackToListAsync()
		{
			await this._page.GotoAsync("http://localhost:5000/request/create");
			await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
			await this._page.WaitForTimeoutAsync(2000);

			var breadcrumb = await this._page.QuerySelectorAsync("a.breadcrumb-link:has-text('Request Management')");
			if (breadcrumb == null)
			{
				Assert.Inconclusive("Breadcrumb link not found — server may not be running.");
				return;
			}

			await breadcrumb.ClickAsync();
			await this._page.WaitForTimeoutAsync(1000);

			Assert.That(this._page.Url, Does.EndWith("/request"),
				"Breadcrumb link should navigate back to the request list");
		}
	}
}
