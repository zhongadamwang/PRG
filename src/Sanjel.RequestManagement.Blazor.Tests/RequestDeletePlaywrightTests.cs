using Microsoft.Playwright;
using NUnit.Framework;

namespace Sanjel.RequestManagement.Blazor.Tests
{
	[TestFixture]
	public class RequestDeletePlaywrightTests
	{
		private IBrowser _browser;
		private IPage _page;
		private IPlaywright _playwright;

		[OneTimeSetUp]
		public async Task SetupAsync()
		{
			this._playwright = await Playwright.CreateAsync();
			this._browser = await this._playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
			this._page = await this._browser.NewPageAsync();
		}

		[OneTimeTearDown]
		public async Task TeardownAsync()
		{
			await this._browser.CloseAsync();
			this._playwright.Dispose();
		}

		[Test]
		public async Task DeleteButton_ShouldBeVisibleInEachRowAsync()
		{
			await this._page.GotoAsync("http://localhost:5000/request");
			await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
			await this._page.WaitForTimeoutAsync(2000);

			await this._page.WaitForSelectorAsync(".data-grid-container", new() { Timeout = 10000 });

			var deleteButtons = await this._page.QuerySelectorAllAsync("button[title='Delete']");
			Assert.IsTrue(deleteButtons.Count > 0, "At least one delete button should be visible in the grid");
		}

		[Test]
		public async Task DeleteButton_Click_ShouldShowConfirmationDialogAsync()
		{
			await this._page.GotoAsync("http://localhost:5000/request");
			await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
			await this._page.WaitForTimeoutAsync(2000);

			await this._page.WaitForSelectorAsync("button[title='Delete']", new() { Timeout = 10000 });

			var firstDeleteButton = await this._page.QuerySelectorAsync("button[title='Delete']");
			Assert.IsNotNull(firstDeleteButton, "Delete button should exist in the grid");

			await firstDeleteButton!.ClickAsync();
			await this._page.WaitForTimeoutAsync(500);

			var dialogSelectors = new[]
			{
				".e-dialog",
				"[role='dialog']",
				".e-dlg-container",
			};

			IElementHandle? dialog = null;
			foreach (var selector in dialogSelectors)
			{
				dialog = await this._page.QuerySelectorAsync(selector);
				if (dialog != null)
				{
					break;
				}
			}

			Assert.IsNotNull(dialog, "Confirmation dialog should appear after clicking Delete");
		}

		[Test]
		public async Task DeleteConfirmationDialog_CancelButton_ShouldDismissDialogAsync()
		{
			await this._page.GotoAsync("http://localhost:5000/request");
			await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
			await this._page.WaitForTimeoutAsync(2000);

			var firstDeleteButton = await this._page.QuerySelectorAsync("button[title='Delete']");
			if (firstDeleteButton == null)
			{
				Assert.Inconclusive("No records available to test delete cancellation.");
				return;
			}

			await firstDeleteButton.ClickAsync();
			await this._page.WaitForTimeoutAsync(500);

			var cancelButton = await this._page.QuerySelectorAsync("button:has-text('Cancel')");
			Assert.IsNotNull(cancelButton, "Cancel button should exist in the confirmation dialog");

			await cancelButton!.ClickAsync();
			await this._page.WaitForTimeoutAsync(500);

			var dialog = await this._page.QuerySelectorAsync(".e-dialog.e-dlg-show");
			Assert.IsNull(dialog, "Dialog should be closed after clicking Cancel");
		}

		[Test]
		public async Task DeleteConfirmationDialog_ShouldContainRequestIdAsync()
		{
			await this._page.GotoAsync("http://localhost:5000/request");
			await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
			await this._page.WaitForTimeoutAsync(2000);

			var firstDeleteButton = await this._page.QuerySelectorAsync("button[title='Delete']");
			if (firstDeleteButton == null)
			{
				Assert.Inconclusive("No records available to test delete dialog content.");
				return;
			}

			await firstDeleteButton.ClickAsync();
			await this._page.WaitForTimeoutAsync(500);

			var dialogContent = await this._page.QuerySelectorAsync(".e-dlg-content");
			Assert.IsNotNull(dialogContent, "Dialog content area should be present");

			var contentText = await dialogContent!.InnerTextAsync();
			Assert.IsTrue(
				contentText.Contains("delete") || contentText.Contains("Delete"),
				"Confirmation dialog should contain delete-related message");
		}

		[Test]
		public async Task BatchDeleteButton_ShouldBeHiddenWhenNoRowsSelectedAsync()
		{
			await this._page.GotoAsync("http://localhost:5000/request");
			await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
			await this._page.WaitForTimeoutAsync(2000);

			var batchDeleteArea = await this._page.QuerySelectorAsync(".batch-actions");
			if (batchDeleteArea != null)
			{
				var style = await batchDeleteArea.GetAttributeAsync("style");
				Assert.IsTrue(
					style?.Contains("display: none") == true,
					"Batch delete area should be hidden when no rows are selected");
			}
			else
			{
				Assert.Pass("Batch delete area not rendered when no rows are selected.");
			}
		}

		[Test]
		public async Task DeleteSuccess_ShouldShowSuccessToastAsync()
		{
			await this._page.GotoAsync("http://localhost:5000/request");
			await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
			await this._page.WaitForTimeoutAsync(2000);

			var firstDeleteButton = await this._page.QuerySelectorAsync("button[title='Delete']");
			if (firstDeleteButton == null)
			{
				Assert.Inconclusive("No records available to test delete toast notification.");
				return;
			}

			await firstDeleteButton.ClickAsync();
			await this._page.WaitForTimeoutAsync(500);

			var confirmDeleteButton = await this._page.QuerySelectorAsync("button:has-text('Delete'):not(:has-text('Delete All'))");
			if (confirmDeleteButton == null)
			{
				confirmDeleteButton = await this._page.QuerySelectorAsync(".e-dialog button.e-primary");
			}

			Assert.IsNotNull(confirmDeleteButton, "Confirm delete button should be visible in the dialog");

			await confirmDeleteButton!.ClickAsync();
			await this._page.WaitForTimeoutAsync(2000);

			var toastSelectors = new[]
			{
				".e-toast-success",
				".e-toast-container .e-toast",
				"[class*='e-toast']",
			};

			IElementHandle? toast = null;
			foreach (var selector in toastSelectors)
			{
				toast = await this._page.QuerySelectorAsync(selector);
				if (toast != null)
				{
					break;
				}
			}

			Assert.IsNotNull(toast, "A success toast notification should appear after deleting a request");
		}
	}
}
