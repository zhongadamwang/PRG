using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sanjel.RequestManagement.Entities.Entities;

namespace Sanjel.RequestManagement.Entities.Configuration;

public class DataElementConfiguration : IEntityTypeConfiguration<DataElement>
{
	public void Configure(EntityTypeBuilder<DataElement> builder)
	{
		// Table configuration
		builder.ToTable("dataelements");

		// Composite Primary Key configuration
		builder.HasKey(e => new { e.ElementId, e.RequestId });

		// Property configurations
		builder.Property(e => e.ElementId)
			.HasColumnName("element_id")
			.HasMaxLength(255)
			.IsRequired();

		builder.Property(e => e.RequestId)
			.HasColumnName("request_id")
			.HasMaxLength(255)
			.IsRequired();

		builder.Property(e => e.ElementType)
			.HasColumnName("element_type");

		builder.Property(e => e.RawValue)
			.HasColumnName("raw_value")
			.HasMaxLength(255)
			.IsRequired();

		builder.Property(e => e.ValidatedValue)
			.HasColumnName("validated_value")
			.HasMaxLength(255)
			.IsRequired();

		builder.Property(e => e.ValidationStatus)
			.HasColumnName("validation_status");

		builder.Property(e => e.SourceLocation)
			.HasColumnName("source_location")
			.HasMaxLength(255)
			.IsRequired();

		builder.Property(e => e.ValidationNotes)
			.HasColumnName("validation_notes")
			.HasMaxLength(255)
			.IsRequired();

		// Relationship configurations
		// Foreign key reference to Request
		builder.HasOne(e => e.Request)
			.WithMany(r => r.DataElement)
			.HasForeignKey(e => e.RequestId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(e => e.StateDiagram)
			.WithMany()
			.HasForeignKey(e => e.ElementId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}
