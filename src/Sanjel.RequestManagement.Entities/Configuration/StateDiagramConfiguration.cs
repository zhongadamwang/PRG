using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sanjel.RequestManagement.Entities.Entities;

namespace Sanjel.RequestManagement.Entities.Configuration;

public class StateDiagramConfiguration : IEntityTypeConfiguration<StateDiagram>
{
	public void Configure(EntityTypeBuilder<StateDiagram> builder)
	{
		// Table configuration
		builder.ToTable("statediagrams");

		// Property configurations
		builder.Property(e => e.DiagramId)
	.HasColumnName("diagram_id")
	.HasMaxLength(255)
	.IsRequired();

		builder.Property(e => e.DiagramName)
	.HasColumnName("diagram_name")
	.HasMaxLength(255)
	.IsRequired();

		builder.Property(e => e.FilePath)
	.HasColumnName("file_path")
	.HasMaxLength(255)
	.IsRequired();

		builder.Property(e => e.Version)
	.HasColumnName("version")
	.HasMaxLength(255)
	.IsRequired();

		builder.Property(e => e.ImportDate)
	.HasColumnName("import_date")
	.HasColumnType("datetime2");

		builder.Property(e => e.ParsingConfidence)
	.HasColumnName("parsing_confidence")
	.HasColumnType("decimal(18,2)");

		builder.Property(e => e.ClientId)
	.HasColumnName("client_id")
	.HasMaxLength(255)
	.IsRequired();

		builder.Property(e => e.DiagramType)
	.HasColumnName("diagram_type");

		// Relationship configurations
		// One-to-one relationship with DataElement
		builder.HasOne(d => d.DataElement)
			.WithOne()
			.HasForeignKey<DataElement>("StateDiagramId");
		// One-to-one relationship with Request
		builder.HasOne(d => d.Request)
			.WithOne()
			.HasForeignKey<Request>("StateDiagramId");
	}
}
