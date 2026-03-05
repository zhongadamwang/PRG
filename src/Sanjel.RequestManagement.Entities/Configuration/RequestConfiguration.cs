using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sanjel.RequestManagement.Entities.Entities;

namespace Sanjel.RequestManagement.Entities.Configuration;

/// <summary>
/// Configuration for Request entity
/// Program request submitted for engineering work with complete lifecycle tracking
/// </summary>
public class RequestConfiguration : IEntityTypeConfiguration<Request>
{
	public void Configure(EntityTypeBuilder<Request> builder)
	{
		// Table configuration
		builder.ToTable("requests");

		// Property configurations
		builder.Property(e => e.RequestId)
	.HasColumnName("request_id")
	.HasMaxLength(255)
	.IsRequired();

		builder.Property(e => e.Status)
	.HasColumnName("status");

		builder.Property(e => e.CreatedDate)
	.HasColumnName("created_date")
	.HasColumnType("datetime2");

		builder.Property(e => e.Priority)
	.HasColumnName("priority");

		builder.Property(e => e.ClientId)
	.HasColumnName("client_id")
	.HasMaxLength(255)
	.IsRequired();

		builder.Property(e => e.SourceEmail)
	.HasColumnName("source_email")
	.HasMaxLength(255)
	.IsRequired();

		builder.Property(e => e.AssignedEngineerId)
	.HasColumnName("assigned_engineer_id")
	.HasMaxLength(255)
	.IsRequired();

		builder.Property(e => e.AssignedBy)
	.HasColumnName("assigned_by")
	.HasMaxLength(255)
	.IsRequired();

		builder.Property(e => e.AcknowledgmentDate)
	.HasColumnName("acknowledgment_date")
	.HasColumnType("datetime2");

		builder.Property(e => e.CompletionDate)
	.HasColumnName("completion_date")
	.HasColumnType("datetime2");

		// Index configurations
		// Index on SourceEmail for performance
		builder.HasIndex(e => e.SourceEmail)
			.HasDatabaseName("IX_Request_SourceEmail");

		// Relationship configurations
		// One-to-one relationship with ReviewPackage
		builder.HasOne(d => d.ReviewPackage)
			.WithOne()
			.HasForeignKey<ReviewPackage>("RequestId");
		// One-to-many relationship with DataElement
		builder.HasMany(d => d.DataElement)
			.WithOne()
			.HasForeignKey("RequestId")
			.OnDelete(DeleteBehavior.Cascade);
		// One-to-one relationship with Notification
		builder.HasOne(d => d.Notification)
			.WithOne()
			.HasForeignKey<Notification>("RequestId");
	}
}
