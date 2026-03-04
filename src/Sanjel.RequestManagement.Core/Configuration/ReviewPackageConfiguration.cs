using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sanjel.RequestManagement.Core.Entities;

namespace Sanjel.RequestManagement.Core.Configuration;

public class ReviewPackageConfiguration : IEntityTypeConfiguration<ReviewPackage>
{
	public void Configure(EntityTypeBuilder<ReviewPackage> builder)
	{
		// Table configuration
		builder.ToTable("reviewpackages");

		// Property configurations
		builder.Property(e => e.PackageId)
	.HasColumnName("package_id")
	.HasMaxLength(255)
	.IsRequired();

		builder.Property(e => e.RequestId)
	.HasColumnName("request_id")
	.HasMaxLength(255)
	.IsRequired();

		builder.Property(e => e.SubmittingEngineerId)
	.HasColumnName("submitting_engineer_id")
	.HasMaxLength(255)
	.IsRequired();

		builder.Property(e => e.AssignedReviewerId)
	.HasColumnName("assigned_reviewer_id")
	.HasMaxLength(255)
	.IsRequired();

		builder.Property(e => e.SubmissionDate)
	.HasColumnName("submission_date")
	.HasColumnType("datetime2");

		builder.Property(e => e.ReviewCompletionDate)
	.HasColumnName("review_completion_date")
	.HasColumnType("datetime2");

		builder.Property(e => e.ReviewStatus)
	.HasColumnName("review_status");

		builder.Property(e => e.WorkSummary)
	.HasColumnName("work_summary")
	.HasMaxLength(255)
	.IsRequired();

		builder.Property(e => e.ReviewFeedback)
	.HasColumnName("review_feedback")
	.HasMaxLength(255)
	.IsRequired();

		// Relationship configurations
		// Foreign key reference to Request
		builder.Property(e => e.RequestId)
			.HasColumnName("request_id");
	}
}
