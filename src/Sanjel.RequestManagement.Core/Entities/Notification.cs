using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanjel.RequestManagement.Core.Entities;

/// <summary>
/// System-generated communication to stakeholders about request status and actions
/// </summary>
public class Notification
{

	/// <summary>
	/// notification_id property
	/// </summary>
	[Column("notification_id")]
	[Key]
	[Required]
	[MaxLength(255)]
	public string NotificationId { get; set; }

	/// <summary>
	/// request_id property
	/// </summary>
	[Column("request_id")]
	[Key]
	[Required]
	[MaxLength(255)]
	public string RequestId { get; set; }

	/// <summary>
	/// recipient_type property
	/// </summary>
	[Column("recipient_type")]
	public RecipientEnum RecipientType { get; set; }

	/// <summary>
	/// notification_type property
	/// </summary>
	[Column("notification_type")]
	public NotificationTypeEnum NotificationType { get; set; }

	/// <summary>
	/// delivery_method property
	/// </summary>
	[Column("delivery_method")]
	public DeliveryEnum DeliveryMethod { get; set; }

	/// <summary>
	/// sent_date property
	/// </summary>
	[Column("sent_date")]
	public DateTime SentDate { get; set; }

	/// <summary>
	/// content property
	/// </summary>
	[Column("content")]
	[Required]
	[MaxLength(255)]
	public string Content { get; set; }

	/// <summary>
	/// action_buttons property
	/// </summary>
	[Column("action_buttons")]
	[Required]
	[MaxLength(255)]
	public ICollection<string> ActionButtons { get; set; } = new List<string>();

	// Navigation Properties

	public virtual Request? Request { get; set; }
}