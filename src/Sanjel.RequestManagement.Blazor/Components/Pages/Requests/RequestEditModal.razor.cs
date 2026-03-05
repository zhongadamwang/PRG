using Microsoft.AspNetCore.Components;
using Sanjel.RequestManagement.Entities.Entities;

namespace Sanjel.RequestManagement.Blazor.Components.Pages.Requests;

/// <summary>
/// Code-behind for RequestEditModal component with Syncfusion Dialog implementation
/// </summary>
public partial class RequestEditModal : ComponentBase
{
	#region Parameters

	[Parameter] public bool IsVisible { get; set; }
	[Parameter] public Request? RequestModel { get; set; }
	[Parameter] public EventCallback<Request> OnSave { get; set; }
	[Parameter] public EventCallback OnCancel { get; set; }
	[Parameter] public bool CloseOnBackdropClick { get; set; } = false;

	#endregion

	#region Private Properties

	// Internal state properties
	private bool IsProcessing { get; set; } = false;
	private List<string> ValidationMessages { get; set; } = new();

	// Dropdown options
	private List<DropdownOption<StatusEnum>> StatusOptions { get; set; } = new();
	private List<DropdownOption<PriorityEnum>> PriorityOptions { get; set; } = new();

	// UI properties
	private bool IsNewRequest => string.IsNullOrEmpty(this.RequestModel?.RequestId);
	private string DialogTitle => this.IsNewRequest ? "Add New Request" : $"Edit Request - {this.RequestModel?.RequestId}";
	private string SaveButtonText => this.IsNewRequest ? "Create" : "Update";

	// Internal properties for nullable date handling
	private DateTime? AcknowledgmentDate
	{
		get => this.RequestModel?.AcknowledgmentDate == default ? null : this.RequestModel?.AcknowledgmentDate;
		set => this.RequestModel!.AcknowledgmentDate = value ?? default;
	}

	private DateTime? CompletionDate
	{
		get => this.RequestModel?.CompletionDate == default ? null : this.RequestModel?.CompletionDate;
		set => this.RequestModel!.CompletionDate = value ?? default;
	}

	#endregion

	#region Lifecycle Methods

	/// <summary>
	/// Initialize component and setup dropdown options
	/// </summary>
	protected override void OnInitialized()
	{
		this.SetupDropdownOptions();
	}

	/// <summary>
	/// Handle parameter changes - especially IsVisible changes
	/// </summary>
	protected override async Task OnParametersSetAsync()
	{
		// Clear validation when modal opens with new data
		if (this.IsVisible && this.ValidationMessages.Any())
		{
			this.ValidationMessages.Clear();
		}

		// Add keyboard event listener when modal is visible
		if (this.IsVisible)
		{
			await Task.Delay(50); // Small delay to ensure DOM is updated
		}

		await base.OnParametersSetAsync();
	}

	#endregion

	#region Event Handlers

	/// <summary>
	/// Validate email format
	/// </summary>
	private static bool IsValidEmail(string email)
	{
		try
		{
			var addr = new System.Net.Mail.MailAddress(email);
			return addr.Address == email;
		}
		catch
		{
			return false;
		}
	}

	/// <summary>
	/// Get display text for status enum
	/// </summary>
	private static string GetStatusDisplayText(StatusEnum status)
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
	/// Get display text for priority enum
	/// </summary>
	private static string GetPriorityDisplayText(PriorityEnum priority)
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

	/// <summary>
	/// Handle dialog opened event
	/// </summary>
	private void OnDialogOpen()
	{
		// Clear validation when modal opens with new data
		if (this.ValidationMessages.Any())
		{
			this.ValidationMessages.Clear();
			this.StateHasChanged();
		}
	}

	/// <summary>
	/// Handle dialog closed event
	/// </summary>
	private void OnDialogClose()
	{
		// Ensure dialog is properly closed and state is cleaned up
		this.ValidationMessages.Clear();
		this.IsProcessing = false;
	}

	/// <summary>
	/// Handle save button click with improved error handling
	/// </summary>
	private async Task HandleSaveAsync()
	{
		if (this.IsProcessing)
		{
			return;
		}

		try
		{
			if (!this.ValidateForm())
			{
				this.StateHasChanged();
				return;
			}

			this.IsProcessing = true;
			this.StateHasChanged();

			if (this.RequestModel != null && this.OnSave.HasDelegate)
			{
				await this.OnSave.InvokeAsync(this.RequestModel);
			}
		}
		catch (Exception ex)
		{
			// Add error to validation messages for display
			this.ValidationMessages.Clear();
			this.ValidationMessages.Add($"An error occurred while saving: {ex.Message}");
			this.StateHasChanged();
		}
		finally
		{
			this.IsProcessing = false;
			this.StateHasChanged();
		}
	}

	/// <summary>
	/// Handle cancel button click
	/// </summary>
	private async Task HandleCancelAsync()
	{
		if (this.IsProcessing)
		{
			return;
		}

		this.ValidationMessages.Clear();

		if (this.OnCancel.HasDelegate)
		{
			await this.OnCancel.InvokeAsync();
		}
	}

	#endregion

	#region Validation Methods

	/// <summary>
	/// Validate form data with comprehensive checks
	/// </summary>
	private bool ValidateForm()
	{
		this.ValidationMessages.Clear();

		if (this.RequestModel == null)
		{
			this.ValidationMessages.Add("Request model is required");
			return false;
		}

		// Validate required fields
		this.ValidateRequiredFields();

		// Validate business rules
		this.ValidateBusinessRules();

		return this.ValidationMessages.Count == 0;
	}

	/// <summary>
	/// Validate required fields
	/// </summary>
	private void ValidateRequiredFields()
	{
		if (this.RequestModel == null)
		{
			return;
		}

		if (string.IsNullOrWhiteSpace(this.RequestModel.ClientId))
		{
			this.ValidationMessages.Add("Client ID is required");
		}

		if (string.IsNullOrWhiteSpace(this.RequestModel.SourceEmail))
		{
			this.ValidationMessages.Add("Source Email is required");
		}
		else if (!IsValidEmail(this.RequestModel.SourceEmail))
		{
			this.ValidationMessages.Add("Source Email must be a valid email address");
		}

		if (string.IsNullOrWhiteSpace(this.RequestModel.AssignedEngineerId))
		{
			this.ValidationMessages.Add("Assigned Engineer ID is required");
		}

		if (string.IsNullOrWhiteSpace(this.RequestModel.AssignedBy))
		{
			this.ValidationMessages.Add("Assigned By is required");
		}
	}

	/// <summary>
	/// Validate business rules
	/// </summary>
	private void ValidateBusinessRules()
	{
		if (this.RequestModel == null)
		{
			return;
		}

		// Validate completion date when status is completed
		if (this.RequestModel.Status == StatusEnum.Completed && this.RequestModel.CompletionDate == default)
		{
			this.ValidationMessages.Add("Completion Date is required when status is Completed");
		}

		// Validate acknowledgment date
		if (this.RequestModel.AcknowledgmentDate != default && this.RequestModel.AcknowledgmentDate < this.RequestModel.CreatedDate)
		{
			this.ValidationMessages.Add("Acknowledgment Date cannot be before Created Date");
		}

		// Validate completion date
		if (this.RequestModel.CompletionDate != default && this.RequestModel.CompletionDate < this.RequestModel.CreatedDate)
		{
			this.ValidationMessages.Add("Completion Date cannot be before Created Date");
		}

		// Validate completion date vs acknowledgment date
		if (this.RequestModel.CompletionDate != default && this.RequestModel.AcknowledgmentDate != default &&
			this.RequestModel.CompletionDate < this.RequestModel.AcknowledgmentDate)
		{
			this.ValidationMessages.Add("Completion Date cannot be before Acknowledgment Date");
		}

		// Validate status transitions
		this.ValidateStatusTransitions();
	}

	/// <summary>
	/// Validate status transition rules
	/// </summary>
	private void ValidateStatusTransitions()
	{
		if (this.RequestModel == null)
		{
			return;
		}

		// If status is InProgress or later, acknowledgment date should be set
		if ((this.RequestModel.Status == StatusEnum.InProgress ||
			 this.RequestModel.Status == StatusEnum.UnderReview ||
			 this.RequestModel.Status == StatusEnum.Approved ||
			 this.RequestModel.Status == StatusEnum.Completed) &&
			this.RequestModel.AcknowledgmentDate == default)
		{
			this.ValidationMessages.Add("Acknowledgment Date is required for In Progress or later statuses");
		}
	}

	#endregion

	#region Helper Methods

	/// <summary>
	/// Setup dropdown list options for enums
	/// </summary>
	private void SetupDropdownOptions()
	{
		// Setup Status options
		this.StatusOptions = Enum.GetValues<StatusEnum>()
			.Select(status => new DropdownOption<StatusEnum>
			{
				Value = status,
				Text = GetStatusDisplayText(status),
			})
			.ToList();

		// Setup Priority options
		this.PriorityOptions = Enum.GetValues<PriorityEnum>()
			.Select(priority => new DropdownOption<PriorityEnum>
			{
				Value = priority,
				Text = GetPriorityDisplayText(priority),
			})
			.ToList();
	}

	#endregion

	#region Helper Classes

	/// <summary>
	/// Helper class for dropdown options
	/// </summary>
	private class DropdownOption<T>
	{
		public T Value { get; set; } = default!;
		public string Text { get; set; } = string.Empty;
	}

	#endregion
}
