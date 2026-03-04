using Microsoft.AspNetCore.Components;
using Sanjel.RequestManagement.Blazor.Services;
using Sanjel.RequestManagement.Core.Entities;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Popups;

namespace Sanjel.RequestManagement.Blazor.Components.Pages.Requests;

/// <summary>
/// Code-behind for RequestsList page
/// </summary>
public partial class RequestsList : ComponentBase
{
	// Grid settings
	private readonly string[] pageSizes = { "10", "20", "50", "100" };

	// Grid reference
	private SfGrid<Request>? requestsGrid;

	// Modal references
	private RequestEditModal? editModal;

	private SfDialog? deleteDialog;

	// Data properties
	private List<Request> requests = new();
	private Request? currentRequest;

	// State properties
	private bool isLoading = false;

	private bool isEditModalVisible = false;
	private bool isDeleteDialogVisible = false;

	[Inject]
	private IRequestsMockService RequestsService { get; set; } = default!;

	/// <summary>
	/// Initialize component and load data
	/// </summary>
	protected override async Task OnInitializedAsync()
	{
		await this.LoadRequestsAsync();
	}

	/// <summary>
	/// Get CSS class for status badge
	/// </summary>
	private static string GetStatusCssClass(StatusEnum? status)
	{
		return status switch
		{
			StatusEnum.Draft => "status-draft",
			StatusEnum.Submitted => "status-submitted",
			StatusEnum.InProgress => "status-inprogress",
			StatusEnum.UnderReview => "status-underreview",
			StatusEnum.Approved => "status-approved",
			StatusEnum.Rejected => "status-rejected",
			StatusEnum.Completed => "status-completed",
			StatusEnum.Cancelled => "status-cancelled",
			_ => "status-draft"
		};
	}

	/// <summary>
	/// Get CSS class for priority badge
	/// </summary>
	private static string GetPriorityCssClass(PriorityEnum? priority)
	{
		return priority switch
		{
			PriorityEnum.Low => "priority-low",
			PriorityEnum.Normal => "priority-normal",
			PriorityEnum.High => "priority-high",
			PriorityEnum.Critical => "priority-critical",
			_ => "priority-normal"
		};
	}

	/// <summary>
	/// Load all requests from service
	/// </summary>
	private async Task LoadRequestsAsync()
	{
		try
		{
			this.isLoading = true;
			this.StateHasChanged();

			this.requests = await this.RequestsService.GetAllRequestsAsync();
		}
		catch (Exception ex)
		{
			// In a real app, you would log this error and show user-friendly message
			Console.WriteLine($"Error loading requests: {ex.Message}");
			this.requests = new List<Request>();
		}
		finally
		{
			this.isLoading = false;
			this.StateHasChanged();
		}
	}

	/// <summary>
	/// Handle add new request action
	/// </summary>
	private void HandleAddRequest()
	{
		this.currentRequest = new Request
		{
			Status = StatusEnum.Draft,
			Priority = PriorityEnum.Normal,
			CreatedDate = DateTime.Now,
		};

		this.isEditModalVisible = true;
		this.StateHasChanged();
	}

	/// <summary>
	/// Handle edit existing request action
	/// </summary>
	private void HandleEditRequest(Request? request)
	{
		if (request == null)
		{
			return;
		}

		// Create a copy to avoid modifying the original while editing
		this.currentRequest = new Request
		{
			RequestId = request.RequestId,
			Status = request.Status,
			CreatedDate = request.CreatedDate,
			Priority = request.Priority,
			ClientId = request.ClientId,
			SourceEmail = request.SourceEmail,
			AssignedEngineerId = request.AssignedEngineerId,
			AssignedBy = request.AssignedBy,
			AcknowledgmentDate = request.AcknowledgmentDate,
			CompletionDate = request.CompletionDate,
		};

		this.isEditModalVisible = true;
		this.StateHasChanged();
	}

	/// <summary>
	/// Handle save request (create or update)
	/// </summary>
	private async Task HandleSaveRequestAsync(Request request)
	{
		try
		{
			this.isLoading = true;
			this.isEditModalVisible = false;
			this.StateHasChanged();

			if (string.IsNullOrEmpty(request.RequestId) || !this.requests.Any(r => r.RequestId == request.RequestId))
			{
				// Create new request
				await this.RequestsService.CreateRequestAsync(request);
			}
			else
			{
				// Update existing request
				await this.RequestsService.UpdateRequestAsync(request);
			}

			// Reload the grid data
			await this.LoadRequestsAsync();

			// Refresh grid display
			if (this.requestsGrid != null)
			{
				await this.requestsGrid.Refresh();
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error saving request: {ex.Message}");
			// In a real app, show error message to user
		}
		finally
		{
			this.isLoading = false;
			this.StateHasChanged();
		}
	}

	/// <summary>
	/// Handle cancel edit action
	/// </summary>
	private void HandleCancelEdit()
	{
		this.isEditModalVisible = false;
		this.currentRequest = null;
		this.StateHasChanged();
	}

	/// <summary>
	/// Handle delete request action - show confirmation dialog
	/// </summary>
	private void HandleDeleteRequest(Request? request)
	{
		if (request == null)
		{
			return;
		}

		this.currentRequest = request;
		this.isDeleteDialogVisible = true;
		this.StateHasChanged();
	}

	/// <summary>
	/// Handle confirmed delete action
	/// </summary>
	private async Task HandleConfirmDeleteAsync()
	{
		if (this.currentRequest?.RequestId == null)
		{
			return;
		}

		try
		{
			this.isLoading = true;
			this.isDeleteDialogVisible = false;
			this.StateHasChanged();

			var success = await this.RequestsService.DeleteRequestAsync(this.currentRequest.RequestId);

			if (success)
			{
				// Reload the grid data
				await this.LoadRequestsAsync();

				// Refresh grid display
				if (this.requestsGrid != null)
				{
					await this.requestsGrid.Refresh();
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error deleting request: {ex.Message}");
			// In a real app, show error message to user
		}
		finally
		{
			this.currentRequest = null;
			this.isLoading = false;
			this.StateHasChanged();
		}
	}

	/// <summary>
	/// Handle cancel delete action
	/// </summary>
	private void HandleCancelDelete()
	{
		this.isDeleteDialogVisible = false;
		this.currentRequest = null;
		this.StateHasChanged();
	}
}
