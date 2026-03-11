using Microsoft.AspNetCore.Components;
using Sanjel.RequestManagement.Blazor.Pages.Requests.Services;
using Sanjel.RequestManagement.Blazor.Pages.Requests.ViewModels;
using Sanjel.RequestManagement.Entities.Entities;
using Sanjel.RequestManagement.Repositories.Data;

namespace Sanjel.RequestManagement.Blazor.Pages.Requests;

public partial class Index : ComponentBase
{
	[Inject]
	protected IRequestRepository RequestRepository { get; set; } = default!;

	[Inject]
	protected RequestService RequestService { get; set; } = default!;

	protected IReadOnlyList<Request> Requests { get; private set; } = Array.Empty<Request>();

	protected bool IsLoading { get; private set; } = true;

	protected string? ErrorMessage { get; private set; }

	protected string? SuccessMessage { get; private set; }

	// Dialog state
	protected string DialogTitle => this.IsEditMode ? "Edit Request" : "New Request";

	protected bool IsDialogVisible { get; private set; } = false;

	protected bool IsEditMode { get; private set; }

	protected RequestViewModel DialogViewModel { get; private set; } = new RequestViewModel();

	private Request? _editingRequest;

	protected IReadOnlyList<SummaryMetric> SummaryMetrics =>
	[
		new("Total Requests", this.Requests.Count.ToString(), "Current repository backlog"),
		new("Active Work", this.Requests.Count(request => request.Status is StatusEnum.Submitted or StatusEnum.InProgress or StatusEnum.UnderReview).ToString(), "Submitted, in progress, or under review"),
		new("High Priority", this.Requests.Count(request => request.Priority is PriorityEnum.High or PriorityEnum.Critical).ToString(), "Requires expedited attention"),
		new("Completed", this.Requests.Count(request => request.Status == StatusEnum.Completed).ToString(), "Closed successfully"),
	];

	protected override async Task OnInitializedAsync()
	{
		this.IsDialogVisible = false;
		await this.LoadRequestsAsync();
	}

	protected Task RefreshAsync()
	{
		return this.LoadRequestsAsync();
	}

	protected void DismissMessages()
	{
		this.ErrorMessage = null;
		this.SuccessMessage = null;
	}

	protected void ShowCreateDialog()
	{
		this.IsEditMode = false;
		this._editingRequest = null;
		this.DialogViewModel = new RequestViewModel
		{
			CreatedDate = DateTime.UtcNow,
			AcknowledgmentDate = DateTime.UtcNow,
			CompletionDate = DateTime.UtcNow.AddDays(30),
			Status = StatusEnum.Draft,
			Priority = PriorityEnum.Normal,
		};
		this.ErrorMessage = null;
		this.SuccessMessage = null;
		this.IsDialogVisible = true;
	}

	protected void ShowEditDialog(Request? request)
	{
		if (request is null)
		{
			return;
		}

		this._editingRequest = request;
		this.IsEditMode = true;
		this.DialogViewModel = new RequestViewModel
		{
			RequestId = request.RequestId,
			ClientId = request.ClientId,
			SourceEmail = request.SourceEmail,
			AssignedEngineerId = request.AssignedEngineerId,
			AssignedBy = request.AssignedBy,
			Status = request.Status,
			Priority = request.Priority,
			CreatedDate = request.CreatedDate,
			AcknowledgmentDate = request.AcknowledgmentDate,
			CompletionDate = request.CompletionDate,
		};
		this.ErrorMessage = null;
		this.SuccessMessage = null;
		this.IsDialogVisible = true;
	}

	protected async Task OnDialogSaveAsync(RequestViewModel viewModel)
	{
		try
		{
			if (this.IsEditMode && this._editingRequest is not null)
			{
				var updated = await this.RequestService.UpdateAsync(this._editingRequest, viewModel);
				var list = this.Requests.ToList();
				var idx = list.FindIndex(r => r.RequestId == updated.RequestId);
				if (idx >= 0)
				{
					list[idx] = updated;
					this.Requests = list;
				}
				else
				{
					await this.LoadRequestsAsync();
				}

				this.SuccessMessage = $"Request '{updated.RequestId}' updated successfully.";
			}
			else
			{
				var created = await this.RequestService.CreateAsync(viewModel);
				await this.LoadRequestsAsync();
				this.SuccessMessage = $"Request '{created.RequestId}' created successfully.";
			}

			this.IsDialogVisible = false;
		}
		catch (Exception ex)
		{
			this.ErrorMessage = $"Operation failed: {ex.Message}";
		}
	}

	protected void OnDialogCancel()
	{
		this.IsDialogVisible = false;
	}

	private async Task LoadRequestsAsync()
	{
		this.IsLoading = true;
		this.ErrorMessage = null;

		try
		{
			var requests = await this.RequestRepository.GetAllAsync();
			this.Requests = requests
				.OrderByDescending(request => request.CreatedDate)
				.ThenBy(request => request.RequestId)
				.ToList();
		}
		catch (Exception ex)
		{
			this.Requests = Array.Empty<Request>();
			this.ErrorMessage = $"Unable to load request data. {ex.Message}";
		}
		finally
		{
			this.IsLoading = false;
		}
	}

	protected sealed class SummaryMetric
	{
		public SummaryMetric(string label, string value, string detail)
		{
			this.Label = label;
			this.Value = value;
			this.Detail = detail;
		}

		public string Label { get; }

		public string Value { get; }

		public string Detail { get; }
	}
}
