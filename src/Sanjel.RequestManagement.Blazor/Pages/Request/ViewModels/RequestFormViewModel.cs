using System.ComponentModel.DataAnnotations;
using Sanjel.RequestManagement.Entities.Entities;

namespace Sanjel.RequestManagement.Blazor.Pages.Request.ViewModels;

/// <summary>
/// ViewModel for the Request create/edit form dialog.
/// </summary>
public class RequestFormViewModel
{
	/// <summary>Gets or sets the request ID.</summary>
	[Required(ErrorMessage = "Request ID is required.")]
	[MaxLength(255)]
	public string RequestId { get; set; } = string.Empty;

	/// <summary>Gets or sets the status.</summary>
	public StatusEnum Status { get; set; } = StatusEnum.Draft;

	/// <summary>Gets or sets the priority.</summary>
	public PriorityEnum Priority { get; set; } = PriorityEnum.Normal;

	/// <summary>Gets or sets the client ID.</summary>
	[Required(ErrorMessage = "Client ID is required.")]
	[MaxLength(255)]
	public string ClientId { get; set; } = string.Empty;

	/// <summary>Gets or sets the source email.</summary>
	[Required(ErrorMessage = "Source email is required.")]
	[MaxLength(255)]
	[EmailAddress(ErrorMessage = "Please enter a valid email address.")]
	public string SourceEmail { get; set; } = string.Empty;

	/// <summary>Gets or sets the assigned engineer ID.</summary>
	[Required(ErrorMessage = "Assigned engineer ID is required.")]
	[MaxLength(255)]
	public string AssignedEngineerId { get; set; } = string.Empty;

	/// <summary>Gets or sets the assigned by.</summary>
	[Required(ErrorMessage = "Assigned by is required.")]
	[MaxLength(255)]
	public string AssignedBy { get; set; } = string.Empty;

	/// <summary>Gets or sets the acknowledgment date.</summary>
	public DateTime? AcknowledgmentDate { get; set; }

	/// <summary>Gets or sets the completion date.</summary>
	public DateTime? CompletionDate { get; set; }
}
