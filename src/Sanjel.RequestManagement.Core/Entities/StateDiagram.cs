using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanjel.RequestManagement.Core.Entities;

public class StateDiagram
{

	/// <summary>
	/// diagram_id property
	/// </summary>
	[Column("diagram_id")]
	[Key]
	[Required]
	[MaxLength(255)]
	public string DiagramId { get; set; }

	/// <summary>
	/// diagram_name property
	/// </summary>
	[Column("diagram_name")]
	[Required]
	[MaxLength(100)]
	public string DiagramName { get; set; }

	/// <summary>
	/// file_path property
	/// </summary>
	[Column("file_path")]
	[Required]
	[MaxLength(255)]
	public string FilePath { get; set; }

	/// <summary>
	/// version property
	/// </summary>
	[Column("version")]
	[Required]
	[MaxLength(255)]
	public string Version { get; set; }

	/// <summary>
	/// import_date property
	/// </summary>
	[Column("import_date")]
	public DateTime ImportDate { get; set; }

	/// <summary>
	/// parsing_confidence property
	/// </summary>
	[Column("parsing_confidence")]
	public decimal ParsingConfidence { get; set; }

	/// <summary>
	/// client_id property
	/// </summary>
	[Column("client_id")]
	[Key]
	[Required]
	[MaxLength(255)]
	public string ClientId { get; set; }

	/// <summary>
	/// diagram_type property
	/// </summary>
	[Column("diagram_type")]
	public DiagramTypeEnum DiagramType { get; set; }

	//
	public virtual DataElement? DataElement { get; set; }

	public virtual Request? Request { get; set; }
}
