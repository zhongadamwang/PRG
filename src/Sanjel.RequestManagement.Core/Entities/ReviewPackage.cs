using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sanjel.RequestManagement.Core.Entities;

public class ReviewPackage
{

    /// <summary>
    /// package_id property
    /// </summary>
    [Column("package_id")]
    [Key]
    [Required]
    [MaxLength(255)]
    public string PackageId { get; set; }

    /// <summary>
    /// request_id property
    /// </summary>
    [Column("request_id")]
    [Key]
    [Required]
    [MaxLength(255)]
    public string RequestId { get; set; }

    /// <summary>
    /// submitting_engineer_id property
    /// </summary>
    [Column("submitting_engineer_id")]
    [Key]
    [Required]
    [MaxLength(255)]
    public string SubmittingEngineerId { get; set; }

    /// <summary>
    /// assigned_reviewer_id property
    /// </summary>
    [Column("assigned_reviewer_id")]
    [Key]
    [Required]
    [MaxLength(255)]
    public string AssignedReviewerId { get; set; }

    /// <summary>
    /// submission_date property
    /// </summary>
    [Column("submission_date")]
    public DateTime SubmissionDate { get; set; }

    /// <summary>
    /// review_completion_date property
    /// </summary>
    [Column("review_completion_date")]
    public DateTime ReviewCompletionDate { get; set; }

    /// <summary>
    /// review_status property
    /// </summary>
    [Column("review_status")]
    public ReviewStatusEnum ReviewStatus { get; set; }

    /// <summary>
    /// work_summary property
    /// </summary>
    [Column("work_summary")]
    [Required]
    [MaxLength(255)]
    public string WorkSummary { get; set; }

    /// <summary>
    /// review_feedback property
    /// </summary>
    [Column("review_feedback")]
    [Required]
    [MaxLength(255)]
    public string ReviewFeedback { get; set; }

    // Navigation Properties

    public virtual Request? Request { get; set; }
}