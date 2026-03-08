using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sanjel.RequestManagement.Entities.Entities;

namespace Sanjel.RequestManagement.Entities.Configuration;

/// <summary>
/// Configuration for Notification entity
/// System-generated communication to stakeholders about request status and actions
/// </summary>
public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
	public void Configure(EntityTypeBuilder<Notification> builder)
	{
		// Table configuration
		builder.ToTable("notifications");

		// Composite Primary Key configuration
		builder.HasKey(e => new { e.NotificationId, e.RequestId });

		// Property configurations
		builder.Property(e => e.NotificationId)
			.HasColumnName("notification_id")
			.HasMaxLength(255)
			.IsRequired();

		builder.Property(e => e.RequestId)
			.HasColumnName("request_id")
			.HasMaxLength(255)
			.IsRequired();

		builder.Property(e => e.RecipientType)
			.HasColumnName("recipient_type");

		builder.Property(e => e.NotificationType)
			.HasColumnName("notification_type");

		builder.Property(e => e.DeliveryMethod)
			.HasColumnName("delivery_method");

		builder.Property(e => e.SentDate)
			.HasColumnName("sent_date")
			.HasColumnType("datetime2");

		builder.Property(e => e.Content)
			.HasColumnName("content")
			.HasMaxLength(255)
			.IsRequired();

		// Relationship configurations
		// One-to-one relationship with Request
		builder.HasOne(e => e.Request)
			.WithOne(r => r.Notification)
			.HasForeignKey<Notification>(e => e.RequestId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
