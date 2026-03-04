using Microsoft.AspNetCore.Components;
using Sanjel.RequestManagement.Core.Entities;

namespace Sanjel.RequestManagement.Blazor.Components.Pages.Requests;

/// <summary>
/// Code-behind for RequestEditModal component
/// </summary>
public partial class RequestEditModal : ComponentBase
{
	// Dialog reference
	private Syncfusion.Blazor.Popups.SfDialog? dlg;

	// Parameters
	[Parameter] public bool IsVisible { get; set; }
	[Parameter] public Request? RequestModel { get; set; }
	[Parameter] public EventCallback<Request> OnSave { get; set; }
	[Parameter] public EventCallback OnCancel { get; set; }

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

	// Dropdown options
	private List<DropdownOption<StatusEnum>> StatusOptions { get; set; } = new();
	private List<DropdownOption<PriorityEnum>> PriorityOptions { get; set; } = new();

	// Validation
	private List<string> ValidationMessages { get; set; } = new();

	// UI properties
	private bool IsNewRequest => string.IsNullOrEmpty(this.RequestModel?.RequestId);
	private string DialogTitle => this.IsNewRequest ? "Add New Request" : $"Edit Request - {this.RequestModel?.RequestId}";
	private string SaveButtonText => this.IsNewRequest ? "Create" : "Update";

	/// <summary>
	/// Initialize component and setup dropdown options
	/// </summary>
	protected override void OnInitialized()
	{
		this.SetupDropdownOptions();
	}

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

	/// <summary>
	/// Handle dialog open event
	/// </summary>
	private void HandleDialogOpen()
	{
		this.ValidateForm();
		this.StateHasChanged();
	}

	/// <summary>
	/// Handle save button click
	/// </summary>
	private async Task HandleSaveAsync()
	{
		if (!this.ValidateForm())
		{
			this.StateHasChanged();
			return;
		}

		if (this.RequestModel != null && this.OnSave.HasDelegate)
		{
			await this.OnSave.InvokeAsync(this.RequestModel);
		}
	}

	/// <summary>
	/// Handle cancel button click
	/// </summary>
	private async Task HandleCancelAsync()
	{
		this.ValidationMessages.Clear();

		if (this.OnCancel.HasDelegate)
		{
			await this.OnCancel.InvokeAsync();
		}
	}

	/// <summary>
	/// Validate form data
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

		// Validate business rules
		if (this.RequestModel.Status == StatusEnum.Completed && this.RequestModel.CompletionDate == default)
		{
			this.ValidationMessages.Add("Completion Date is required when status is Completed");
		}

		if (this.RequestModel.AcknowledgmentDate != default && this.RequestModel.AcknowledgmentDate < this.RequestModel.CreatedDate)
		{
			this.ValidationMessages.Add("Acknowledgment Date cannot be before Created Date");
		}

		if (this.RequestModel.CompletionDate != default && this.RequestModel.CompletionDate < this.RequestModel.CreatedDate)
		{
			this.ValidationMessages.Add("Completion Date cannot be before Created Date");
		}

		return this.ValidationMessages.Count == 0;
	}

	/// <summary>
	/// Helper class for dropdown options
	/// </summary>
	private class DropdownOption<T>
	{
		public T Value { get; set; } = default!;
		public string Text { get; set; } = string.Empty;
	}
}
