using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanjel.RequestManagement.Entities.Entities;

/// <summary>
/// Program request submitted for engineering work with complete lifecycle tracking
/// </summary>
public class Request
{
	/// <summary>
	/// request_id property
	/// </summary>
	[Column("request_id")]
	[Key]
	[Required]
	[MaxLength(255)]
	public string RequestId { get; set; }

	/// <summary>
	/// status property
	/// </summary>
	[Column("status")]
	public StatusEnum Status { get; set; }

	/// <summary>
	/// created_date property
	/// </summary>
	[Column("created_date")]
	public DateTime CreatedDate { get; set; }

	/// <summary>
	/// priority property
	/// </summary>
	[Column("priority")]
	public PriorityEnum Priority { get; set; }

	/// <summary>
	/// client_id property
	/// </summary>
	[Column("client_id")]
	[Required]
	[MaxLength(255)]
	public string ClientId { get; set; }

	/// <summary>
	/// source_email property
	/// </summary>
	[Column("source_email")]
	[Required]
	[MaxLength(255)]
	public string SourceEmail { get; set; }

	/// <summary>
	/// assigned_engineer_id property
	/// </summary>
	[Column("assigned_engineer_id")]
	[Required]
	[MaxLength(255)]
	public string AssignedEngineerId { get; set; }

	/// <summary>
	/// assigned_by property
	/// </summary>
	[Column("assigned_by")]
	[Required]
	[MaxLength(255)]
	public string AssignedBy { get; set; }

	/// <summary>
	/// acknowledgment_date property
	/// </summary>
	[Column("acknowledgment_date")]
	public DateTime AcknowledgmentDate { get; set; }

	/// <summary>
	/// completion_date property
	/// </summary>
	[Column("completion_date")]
	public DateTime CompletionDate { get; set; }
}
