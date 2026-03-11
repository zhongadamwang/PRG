using System.ComponentModel.DataAnnotations;
using Sanjel.RequestManagement.Entities.Entities;

namespace Sanjel.RequestManagement.Blazor.Pages.Requests.ViewModels
{
	/// <summary>
	/// ViewModel for Request entity with comprehensive data validation.
	/// Serves as the data binding layer between Blazor UI components and the Request entity.
	/// </summary>
	public class RequestViewModel
	{
		/// <summary>
		/// Unique identifier for the request
		/// </summary>
		[Required(ErrorMessage = "Request ID is required")]
		[StringLength(50, MinimumLength = 1, ErrorMessage = "Request ID must be between 1 and 50 characters")]
		public string RequestId { get; set; } = string.Empty;

		/// <summary>
		/// Current status of the request
		/// </summary>
		[Required(ErrorMessage = "Status is required")]
		public StatusEnum Status { get; set; }

		/// <summary>
		/// Date and time when the request was created
		/// </summary>
		[Required(ErrorMessage = "Created date is required")]
		[DataType(DataType.DateTime)]
		public DateTime CreatedDate { get; set; }

		/// <summary>
		/// Priority level of the request
		/// </summary>
		[Required(ErrorMessage = "Priority is required")]
		public PriorityEnum Priority { get; set; }

		/// <summary>
		/// Unique identifier for the client who submitted the request
		/// </summary>
		[Required(ErrorMessage = "Client ID is required")]
		[StringLength(50, MinimumLength = 1, ErrorMessage = "Client ID must be between 1 and 50 characters")]
		public string ClientId { get; set; } = string.Empty;

		/// <summary>
		/// Email address from which the request originated
		/// </summary>
		[Required(ErrorMessage = "Source email is required")]
		[EmailAddress(ErrorMessage = "Please enter a valid email address")]
		[StringLength(255, ErrorMessage = "Email address cannot exceed 255 characters")]
		public string SourceEmail { get; set; } = string.Empty;

		/// <summary>
		/// Unique identifier for the assigned engineer
		/// </summary>
		[Required(ErrorMessage = "Assigned engineer ID is required")]
		[StringLength(50, MinimumLength = 1, ErrorMessage = "Engineer ID must be between 1 and 50 characters")]
		public string AssignedEngineerId { get; set; } = string.Empty;

		/// <summary>
		/// Unique identifier for the manager who assigned the request
		/// </summary>
		[Required(ErrorMessage = "Assigned by field is required")]
		[StringLength(50, MinimumLength = 1, ErrorMessage = "Manager ID must be between 1 and 50 characters")]
		public string AssignedBy { get; set; } = string.Empty;

		/// <summary>
		/// Date and time when the request was acknowledged
		/// </summary>
		[Required(ErrorMessage = "Acknowledgment date is required")]
		[DataType(DataType.DateTime)]
		public DateTime AcknowledgmentDate { get; set; }

		/// <summary>
		/// Date and time when the request was completed
		/// </summary>
		[Required(ErrorMessage = "Completion date is required")]
		[DataType(DataType.DateTime)]
		public DateTime CompletionDate { get; set; }

		#region Filter Properties

		/// <summary>
		/// Search term for filtering requests
		/// </summary>
		[StringLength(255, ErrorMessage = "Search term cannot exceed 255 characters")]
		public string? SearchTerm { get; set; }

		/// <summary>
		/// Filter by status
		/// </summary>
		public StatusEnum? StatusFilter { get; set; }

		/// <summary>
		/// Filter by priority
		/// </summary>
		public PriorityEnum? PriorityFilter { get; set; }

		/// <summary>
		/// Filter by client ID
		/// </summary>
		[StringLength(50, ErrorMessage = "Client ID filter cannot exceed 50 characters")]
		public string? ClientIdFilter { get; set; }

		/// <summary>
		/// Filter by assigned engineer ID
		/// </summary>
		[StringLength(50, ErrorMessage = "Engineer ID filter cannot exceed 50 characters")]
		public string? AssignedEngineerIdFilter { get; set; }

		/// <summary>
		/// Start date for date range filtering
		/// </summary>
		public DateTime? StartDateFilter { get; set; }

		/// <summary>
		/// End date for date range filtering
		/// </summary>
		public DateTime? EndDateFilter { get; set; }

		#endregion

		#region Pagination Properties

		/// <summary>
		/// Current page number for pagination
		/// </summary>
		[Range(1, int.MaxValue, ErrorMessage = "Current page must be greater than 0")]
		public int CurrentPage { get; set; } = 1;

		/// <summary>
		/// Number of items per page
		/// </summary>
		[Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
		public int PageSize { get; set; } = 20;

		/// <summary>
		/// Total number of items (calculated)
		/// </summary>
		public int TotalItems { get; set; }

		/// <summary>
		/// Total number of pages (calculated)
		/// </summary>
		public int TotalPages => (int)Math.Ceiling((double)this.TotalItems / this.PageSize);

		#endregion

		#region Helper Methods

		/// <summary>
		/// Determines if any filters are currently active
		/// </summary>
		/// <returns>True if any filter has a value, false otherwise</returns>
		public bool HasActiveFilters()
		{
			return !string.IsNullOrWhiteSpace(this.SearchTerm)
				|| this.StatusFilter.HasValue
				|| this.PriorityFilter.HasValue
				|| !string.IsNullOrWhiteSpace(this.ClientIdFilter)
				|| !string.IsNullOrWhiteSpace(this.AssignedEngineerIdFilter)
				|| this.StartDateFilter.HasValue
				|| this.EndDateFilter.HasValue;
		}

		/// <summary>
		/// Resets all filter properties to their default values
		/// </summary>
		public void ResetFilters()
		{
			this.SearchTerm = null;
			this.StatusFilter = null;
			this.PriorityFilter = null;
			this.ClientIdFilter = null;
			this.AssignedEngineerIdFilter = null;
			this.StartDateFilter = null;
			this.EndDateFilter = null;
			this.CurrentPage = 1;
		}

		#endregion
	}
}
