using Microsoft.AspNetCore.Components;
using Sanjel.RequestManagement.Blazor.Pages.Request.Services;
using Sanjel.RequestManagement.Blazor.Pages.Request.ViewModels;
using Sanjel.RequestManagement.Entities.Entities;
using Sanjel.RequestManagement.Repositories.Common;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Notifications;
using Syncfusion.Blazor.Popups;
using RequestEntity = Sanjel.RequestManagement.Entities.Entities.Request;

namespace Sanjel.RequestManagement.Blazor.Pages.Request;

/// <summary>
/// Data structure for Status dropdown items
/// </summary>
public class StatusEnumItem
{
	public StatusEnum Value { get; set; }
	public string Text { get; set; } = string.Empty;
}

/// <summary>
/// Data structure for Priority dropdown items
/// </summary>
public class PriorityEnumItem
{
	public PriorityEnum Value { get; set; }
	public string Text { get; set; } = string.Empty;
}

public partial class Index
{
	[Inject]
	private RequestService RequestService { get; set; } = null!;

	private RequestListViewModel ViewModel { get; } = new RequestListViewModel();
	private PagedResult<RequestEntity>? pagedResult;
	private HashSet<RequestEntity> selectedRequests = new HashSet<RequestEntity>();
	private bool isLoading = true;
	private List<string> pageSizes = RequestListViewModel.AvailablePageSizes.Select(p => p.ToString()).ToList();
	private GridFilterSettings filterSettings = new GridFilterSettings { Type = Syncfusion.Blazor.Grids.FilterType.Menu };
	private List<GridSortColumn> initialSortColumns = new List<GridSortColumn>
	{
		new GridSortColumn { Field = "CreatedDate", Direction = SortDirection.Descending },
	};

	private SfDialog deleteConfirmDialog = null!;
	private SfDialog batchDeleteConfirmDialog = null!;
	private SfToast toastRef = null!;
	private string deleteTargetRequestId = string.Empty;
	private TaskCompletionSource<bool>? _deleteConfirmTcs;
	private TaskCompletionSource<bool>? _batchDeleteConfirmTcs;

	private List<StatusEnumItem> statusOptionItems = Enum.GetValues<StatusEnum>()
		.Select(status => new StatusEnumItem
		{
			Value = status,
			Text = GetStatusDisplayName(status),
		}).ToList();

	private List<PriorityEnumItem> priorityOptionItems = Enum.GetValues<PriorityEnum>()
		.Select(priority => new PriorityEnumItem
		{
			Value = priority,
			Text = GetPriorityDisplayName(priority),
		}).ToList();

	protected override async Task OnInitializedAsync()
	{
		this.ViewModel.SortColumn = "CreatedDate";
		this.ViewModel.SortDirection = "DESC";
		await this.LoadDataAsync();
	}

	/// <summary>
	/// Get user-friendly display name for status enum
	/// </summary>
	private static string GetStatusDisplayName(StatusEnum status)
	{
		return status switch
		{
			StatusEnum.Draft => "Draft",
			StatusEnum.Submitted => "Submitted",
			StatusEnum.InProgress => "In Progress",
			StatusEnum.UnderReview => "Under Review",
			StatusEnum.Approved => "Approved",
			StatusEnum.Rejected => "Rejected",
			StatusEnum.Completed => "Completed",
			StatusEnum.Cancelled => "Cancelled",
			_ => status.ToString()
		};
	}

	/// <summary>
	/// Get user-friendly display name for priority enum
	/// </summary>
	private static string GetPriorityDisplayName(PriorityEnum priority)
	{
		return priority switch
		{
			PriorityEnum.Low => "Low",
			PriorityEnum.Normal => "Normal",
			PriorityEnum.High => "High",
			PriorityEnum.Critical => "Critical",
			_ => priority.ToString()
		};
	}

	private async Task LoadDataAsync()
	{
		this.isLoading = true;
		try
		{
			this.pagedResult = await this.RequestService.GetPagedListAsync(this.ViewModel);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error loading requests: {ex.Message}");
		}
		finally
		{
			this.isLoading = false;
			await this.InvokeAsync(this.StateHasChanged);
		}
	}

	private async Task ApplyFiltersAsync()
	{
		this.ViewModel.CurrentPage = 1;
		await this.LoadDataAsync();
	}

	private async Task ClearFiltersAsync()
	{
		this.ViewModel.ResetFilters();
		await this.LoadDataAsync();
	}

	private void OnRowSelected(RowSelectEventArgs<RequestEntity> args)
	{
		if (args.Data != null)
		{
			this.selectedRequests.Add(args.Data);
		}
		this.StateHasChanged();
	}

	private void OnRowDeselected(RowDeselectEventArgs<RequestEntity> args)
	{
		if (args.Data != null)
		{
			this.selectedRequests.Remove(args.Data);
		}
		this.StateHasChanged();
	}

	private async Task OnActionBeginAsync(ActionEventArgs<RequestEntity> args)
	{
		// Paging is handled by the grid's built-in pager
		// We'll need to sync with our ViewModel through PageChanged event
	}

	private void CreateNewRequest()
	{
		// TODO: Navigate to create page
		Console.WriteLine("Create new request");
	}

	private void ViewDetail(RequestEntity? request)
	{
		if (request == null)
		{
			return;
		}

		var url = this.GetDetailUrl(request.RequestId);
		// TODO: Navigate to detail page
		Console.WriteLine($"Navigate to: {url}");
	}

	private void EditRequest(RequestEntity? request)
	{
		if (request == null)
		{
			return;
		}

		var url = $"/request/edit/{request.RequestId}";
		// TODO: Navigate to edit page
		Console.WriteLine($"Navigate to: {url}");
	}

	private async Task DeleteRequestAsync(RequestEntity? request)
	{
		if (request == null)
		{
			return;
		}

		bool confirmed = await this.ConfirmDeleteAsync(request);
		if (!confirmed)
		{
			return;
		}

		this.isLoading = true;
		try
		{
			bool success = await this.RequestService.DeleteAsync(request.RequestId);
			if (success)
			{
				this.selectedRequests.Remove(request);
				await this.LoadDataAsync();
				await this.ShowSuccessToastAsync($"Request '{request.RequestId}' deleted successfully.");
			}
			else
			{
				await this.ShowErrorToastAsync("Failed to delete request. Please try again.");
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error deleting request: {ex.Message}");
			await this.ShowErrorToastAsync("An error occurred while deleting the request.");
		}
		finally
		{
			this.isLoading = false;
		}
	}

	private async Task BatchDeleteAsync()
	{
		if (this.selectedRequests.Count == 0)
		{
			return;
		}

		bool confirmed = await this.ConfirmBatchDeleteAsync();
		if (!confirmed)
		{
			return;
		}

		this.isLoading = true;
		try
		{
			var requestIds = this.selectedRequests.Select(r => r.RequestId).ToList();
			int deletedCount = await this.RequestService.BatchDeleteAsync(requestIds);

			if (deletedCount > 0)
			{
				this.selectedRequests.Clear();
				await this.LoadDataAsync();
				await this.ShowSuccessToastAsync($"Successfully deleted {deletedCount} request(s).");
			}
			else
			{
				await this.ShowErrorToastAsync("No requests were deleted. Please try again.");
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error batch deleting requests: {ex.Message}");
			await this.ShowErrorToastAsync("An error occurred while deleting the selected requests.");
		}
		finally
		{
			this.isLoading = false;
		}
	}

	private async Task<bool> ConfirmDeleteAsync(RequestEntity request)
	{
		this.deleteTargetRequestId = request.RequestId;
		this._deleteConfirmTcs = new TaskCompletionSource<bool>();
		await this.deleteConfirmDialog.ShowAsync();
		return await this._deleteConfirmTcs.Task;
	}

	private async Task<bool> ConfirmBatchDeleteAsync()
	{
		this._batchDeleteConfirmTcs = new TaskCompletionSource<bool>();
		await this.batchDeleteConfirmDialog.ShowAsync();
		return await this._batchDeleteConfirmTcs.Task;
	}

	private async Task OnDeleteConfirmedAsync()
	{
		this._deleteConfirmTcs?.TrySetResult(true);
		await this.deleteConfirmDialog.HideAsync();
	}

	private async Task OnDeleteCancelledAsync()
	{
		this._deleteConfirmTcs?.TrySetResult(false);
		await this.deleteConfirmDialog.HideAsync();
	}

	private void OnDeleteDialogClosed()
	{
		this._deleteConfirmTcs?.TrySetResult(false);
	}

	private async Task OnBatchDeleteConfirmedAsync()
	{
		this._batchDeleteConfirmTcs?.TrySetResult(true);
		await this.batchDeleteConfirmDialog.HideAsync();
	}

	private async Task OnBatchDeleteCancelledAsync()
	{
		this._batchDeleteConfirmTcs?.TrySetResult(false);
		await this.batchDeleteConfirmDialog.HideAsync();
	}

	private void OnBatchDeleteDialogClosed()
	{
		this._batchDeleteConfirmTcs?.TrySetResult(false);
	}

	private async Task ShowSuccessToastAsync(string message)
	{
		await this.toastRef.ShowAsync(new ToastModel
		{
			Title = "Success",
			Content = message,
			CssClass = "e-toast-success",
			ShowCloseButton = true,
			Timeout = 4000,
		});
	}

	private async Task ShowErrorToastAsync(string message)
	{
		await this.toastRef.ShowAsync(new ToastModel
		{
			Title = "Error",
			Content = message,
			CssClass = "e-toast-danger",
			ShowCloseButton = true,
			Timeout = 5000,
		});
	}

	private string GetDetailUrl(string? requestId)
	{
		return $"/request/detail/{requestId}";
	}

	private string GetEmptyStateMessage()
	{
		if (this.ViewModel.HasActiveFilters())
		{
			return "No requests match your current filters. Try adjusting your search criteria.";
		}

		return "No requests have been created yet. Click the button below to create your first request.";
	}

	private string GetDisplayRange()
	{
		if (this.pagedResult == null || this.pagedResult.TotalCount == 0)
		{
			return "0 - 0";
		}

		return $"{this.pagedResult.StartIndex} - {this.pagedResult.EndIndex}";
	}

	private string GetStatusBadge(StatusEnum? status)
	{
		if (!status.HasValue)
		{
			return "-";
		}

		var cssClass = status.Value switch
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

		return $@"<span class=""status-badge {cssClass}"">{status.Value.ToString()}</span>";
	}

	private string GetPriorityBadge(PriorityEnum? priority)
	{
		if (!priority.HasValue)
		{
			return "-";
		}

		var cssClass = priority.Value switch
		{
			PriorityEnum.Low => "priority-low",
			PriorityEnum.Normal => "priority-normal",
			PriorityEnum.High => "priority-high",
			PriorityEnum.Critical => "priority-critical",
			_ => "priority-normal"
		};

		return $@"<span class=""priority-badge {cssClass}"">{priority.Value.ToString()}</span>";
	}
}
