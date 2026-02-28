using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sanjel.RequestManagement.Core.Entities;

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
    public statusenum Status { get; set; }

    /// <summary>
    /// created_date property
    /// </summary>
    [Column("created_date")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// priority property
    /// </summary>
    [Column("priority")]
    public priorityenum Priority { get; set; }

    /// <summary>
    /// client_id property
    /// </summary>
    [Column("client_id")]
    [Key]
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
    [Key]
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

    // Navigation Properties

    public virtual Engineer? Engineer { get; set; }

    public virtual Manager? Manager { get; set; }

    public virtual ReviewPackage? ReviewPackage { get; set; }

    public virtual ICollection<DataElement> DataElement { get; set; } = new List<DataElement>();

    public virtual Notification? Notification { get; set; }

    public virtual StateDiagram? StateDiagram { get; set; }
}