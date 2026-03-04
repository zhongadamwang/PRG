using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanjel.RequestManagement.Core.Entities;

public class DataElement
{
	/// <summary>
	/// element_id property
	/// </summary>
	[Column("element_id")]
	[Key]
	[Required]
	[MaxLength(255)]
	public string ElementId { get; set; }

	/// <summary>
	/// request_id property
	/// </summary>
	[Column("request_id")]
	[Key]
	[Required]
	[MaxLength(255)]
	public string RequestId { get; set; }

	/// <summary>
	/// element_type property
	/// </summary>
	[Column("element_type")]
	public ElementTypeEnum ElementType { get; set; }

	/// <summary>
	/// raw_value property
	/// </summary>
	[Column("raw_value")]
	[Required]
	[MaxLength(255)]
	public string RawValue { get; set; }

	/// <summary>
	/// validated_value property
	/// </summary>
	[Column("validated_value")]
	[Required]
	[MaxLength(255)]
	public string ValidatedValue { get; set; }

	/// <summary>
	/// validation_status property
	/// </summary>
	[Column("validation_status")]
	public ValidationEnum ValidationStatus { get; set; }

	/// <summary>
	/// source_location property
	/// </summary>
	[Column("source_location")]
	[Required]
	[MaxLength(255)]
	public string SourceLocation { get; set; }

	/// <summary>
	/// validation_notes property
	/// </summary>
	[Column("validation_notes")]
	[Required]
	[MaxLength(255)]
	public string ValidationNotes { get; set; }

	// Navigation Properties

	public virtual Request? Request { get; set; }

	public virtual StateDiagram? StateDiagram { get; set; }
}
