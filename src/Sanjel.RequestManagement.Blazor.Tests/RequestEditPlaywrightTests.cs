using Microsoft.Playwright;
using NUnit.Framework;

namespace Sanjel.RequestManagement.Blazor.Tests;

/// <summary>
/// Playwright UI tests covering the edit/modify workflow on the Request list page.
/// These tests require the Blazor application to be running at http://localhost:5000.
/// </summary>
[TestFixture]
public class RequestEditPlaywrightTests
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
	/// Verifies that at least one Edit button is visible in the Request list.
	/// </summary>
	[Test]
	public async Task EditButton_ShouldBeVisibleOnListRowsAsync()
	{
		await this._page.GotoAsync(RequestsUrl);
		await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
		await this._page.WaitForTimeoutAsync(2000);

		// Edit buttons rendered inside the Syncfusion grid Action column template
		var editButtons = this._page.GetByRole(AriaRole.Button, new() { Name = "Edit" });
		var count = await editButtons.CountAsync();
		Assert.That(count, Is.GreaterThan(0), "At least one Edit button should be visible in the request grid");
	}

	/// <summary>
	/// Verifies that clicking Edit opens the edit dialog.
	/// </summary>
	[Test]
	public async Task EditButton_Click_ShouldOpenEditDialogAsync()
	{
		await this._page.GotoAsync(RequestsUrl);
		await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
		await this._page.WaitForTimeoutAsync(2000);

		var firstEditButton = this._page.GetByRole(AriaRole.Button, new() { Name = "Edit" }).First;
		await firstEditButton.ClickAsync();
		await this._page.WaitForTimeoutAsync(500);

		var dialog = this._page.Locator(".e-dialog");
		await Assertions.Expect(dialog).ToBeVisibleAsync();
	}

	/// <summary>
	/// Verifies that the edit dialog shows the correct "Edit Request" title.
	/// </summary>
	[Test]
	public async Task EditDialog_ShouldShowEditRequestTitleAsync()
	{
		await this._page.GotoAsync(RequestsUrl);
		await this._page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
		await this._page.WaitForTimeoutAsync(2000);

		await this._page.GetByRole(AriaRole.Button, new() { Name = "Edit" }).First.ClickAsync();
		await this._page.WaitForTimeoutAsync(500);

		var dialogTitle = this._page.Locator(".e-dlg-header");
		await Assertions.Expect(dialogTitle).ToContainTextAsync("Edit Request");
	}
}
