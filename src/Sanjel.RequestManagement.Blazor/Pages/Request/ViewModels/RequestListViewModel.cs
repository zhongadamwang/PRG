using System.ComponentModel.DataAnnotations;
using Sanjel.RequestManagement.Entities.Entities;

namespace Sanjel.RequestManagement.Blazor.Pages.Request.ViewModels;

/// <summary>
/// ViewModel for Request list page with search, filter, and pagination support.
/// </summary>
public class RequestListViewModel
{
	/// <summary>
	/// Gets or sets the request ID filter.
	/// </summary>
	[MaxLength(255)]
	public string? RequestIdFilter { get; set; }

	/// <summary>
	/// Gets or sets the status filter.
	/// </summary>
	public StatusEnum? StatusFilter { get; set; }

	/// <summary>
	/// Gets or sets the priority filter.
	/// </summary>
	public PriorityEnum? PriorityFilter { get; set; }

	/// <summary>
	/// Gets or sets the client ID filter.
	/// </summary>
	[MaxLength(255)]
	public string? ClientIdFilter { get; set; }

	/// <summary>
	/// Gets or sets the source email filter.
	/// </summary>
	[MaxLength(255)]
	public string? SourceEmailFilter { get; set; }

	/// <summary>
	/// Gets or sets the assigned engineer ID filter.
	/// </summary>
	[MaxLength(255)]
	public string? AssignedEngineerIdFilter { get; set; }

	/// <summary>
	/// Gets or sets the assigned by filter.
	/// </summary>
	[MaxLength(255)]
	public string? AssignedByFilter { get; set; }

	/// <summary>
	/// Gets or sets the created date start filter.
	/// </summary>
	public DateTime? CreatedDateStartFilter { get; set; }

	/// <summary>
	/// Gets or sets the created date end filter.
	/// </summary>
	public DateTime? CreatedDateEndFilter { get; set; }

	/// <summary>
	/// Gets or sets the acknowledgment date start filter.
	/// </summary>
	public DateTime? AcknowledgmentDateStartFilter { get; set; }

	/// <summary>
	/// Gets or sets the acknowledgment date end filter.
	/// </summary>
	public DateTime? AcknowledgmentDateEndFilter { get; set; }

	/// <summary>
	/// Gets or sets the completion date start filter.
	/// </summary>
	public DateTime? CompletionDateStartFilter { get; set; }

	/// <summary>
	/// Gets or sets the completion date end filter.
	/// </summary>
	public DateTime? CompletionDateEndFilter { get; set; }

	/// <summary>
	/// Gets or sets the global search term.
	/// </summary>
	[MaxLength(500)]
	public string? SearchTerm { get; set; }

	/// <summary>
	/// Gets or sets the current page number (1-based).
	/// </summary>
	public int CurrentPage { get; set; } = 1;

	/// <summary>
	/// Gets or sets the page size.
	/// </summary>
	public int PageSize { get; set; } = 25;

	/// <summary>
	/// Gets or sets the sort column.
	/// </summary>
	public string? SortColumn { get; set; }

	/// <summary>
	/// Gets or sets the sort direction (ASC or DESC).
	/// </summary>
	public string SortDirection { get; set; } = "DESC";

	/// <summary>
	/// Gets the available page sizes.
	/// </summary>
	public static readonly int[] AvailablePageSizes = [10, 25, 50, 100];

	/// <summary>
	/// Gets the available status options.
	/// </summary>
	public static readonly StatusEnum[] AvailableStatuses = Enum.GetValues<StatusEnum>();

	/// <summary>
	/// Gets the available priority options.
	/// </summary>
	public static readonly PriorityEnum[] AvailablePriorities = Enum.GetValues<PriorityEnum>();

	/// <summary>
	/// Resets all filters to default values.
	/// </summary>
	public void ResetFilters()
	{
		this.RequestIdFilter = null;
		this.StatusFilter = null;
		this.PriorityFilter = null;
		this.ClientIdFilter = null;
		this.SourceEmailFilter = null;
		this.AssignedEngineerIdFilter = null;
		this.AssignedByFilter = null;
		this.CreatedDateStartFilter = null;
		this.CreatedDateEndFilter = null;
		this.AcknowledgmentDateStartFilter = null;
		this.AcknowledgmentDateEndFilter = null;
		this.CompletionDateStartFilter = null;
		this.CompletionDateEndFilter = null;
		this.SearchTerm = null;
		this.CurrentPage = 1;
		this.SortColumn = "CreatedDate";
		this.SortDirection = "DESC";
	}

	/// <summary>
	/// Checks if any filters are active.
	/// </summary>
	public bool HasActiveFilters()
	{
		return !string.IsNullOrWhiteSpace(this.RequestIdFilter) ||
			this.StatusFilter.HasValue ||
			this.PriorityFilter.HasValue ||
			!string.IsNullOrWhiteSpace(this.ClientIdFilter) ||
			!string.IsNullOrWhiteSpace(this.SourceEmailFilter) ||
			!string.IsNullOrWhiteSpace(this.AssignedEngineerIdFilter) ||
			!string.IsNullOrWhiteSpace(this.AssignedByFilter) ||
			this.CreatedDateStartFilter.HasValue ||
			this.CreatedDateEndFilter.HasValue ||
			this.AcknowledgmentDateStartFilter.HasValue ||
			this.AcknowledgmentDateEndFilter.HasValue ||
			this.CompletionDateStartFilter.HasValue ||
			this.CompletionDateEndFilter.HasValue ||
			!string.IsNullOrWhiteSpace(this.SearchTerm);
	}
}
