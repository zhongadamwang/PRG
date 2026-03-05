using Microsoft.AspNetCore.Components;

namespace Sanjel.RequestManagement.Blazor.Components.Layout
{
	/// <summary>
	/// Main layout component for the application.
	/// </summary>
	public partial class MainLayout : LayoutComponentBase
	{
		private bool isSidebarOpen = true;

		/// <summary>
		/// Toggles the sidebar visibility.
		/// </summary>
		/// <returns>A task representing the asynchronous operation.</returns>
		private async Task ToggleSidebarAsync()
		{
			this.isSidebarOpen = !this.isSidebarOpen;
			await Task.CompletedTask;
		}
	}
}
