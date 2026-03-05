using Microsoft.AspNetCore.Components;

namespace Sanjel.RequestManagement.Blazor.Components.Layout;

/// <summary>
/// Navigation menu component for the application.
/// </summary>
public partial class NavMenu : ComponentBase
{
	private List<NavigationItem> navigationItems = new()
	{
		new NavigationItem { Id = "1", Text = "Home", NavigateUrl = "/", IconCss = "e-icons e-home" },
		new NavigationItem { Id = "2", Text = "Counter", NavigateUrl = "/counter", IconCss = "e-icons e-plus" },
	};
}

/// <summary>
/// Represents a navigation item.
/// </summary>
public class NavigationItem
{
	/// <summary>
	/// Gets or sets the ID of the navigation item.
	/// </summary>
	public string Id { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the text of the navigation item.
	/// </summary>
	public string Text { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the navigation URL of the item.
	/// </summary>
	public string NavigateUrl { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the icon CSS class of the navigation item.
	/// </summary>
	public string IconCss { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the child items of the navigation item.
	/// </summary>
	public List<NavigationItem>? Items { get; set; }
}
