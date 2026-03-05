using Microsoft.AspNetCore.Components;

namespace Sanjel.RequestManagement.Blazor.Components;

/// <summary>
/// Simple Syncfusion grid component for testing.
/// </summary>
public partial class SimpleGrid : ComponentBase
{
	private List<SimpleItem> GridData { get; } = new()
		{
				new() { Id = 1, Name = "Item 1", Status = "Active" },
				new() { Id = 2, Name = "Item 2", Status = "Inactive" },
				new() { Id = 3, Name = "Item 3", Status = "Active" },
		};

	/// <summary>
	/// Simple item model for grid data.
	/// </summary>
	public class SimpleItem
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Status { get; set; } = string.Empty;
	}
}
