namespace Sanjel.RequestManagement.Core.Entities;

/// <summary>
/// Auto-generated enumeration for Statusenum values (inferred from entity attributes)
/// </summary>
public enum StatusEnum
{
	/// <summary>
	/// Request is in draft state
	/// </summary>
	Draft = 0,

	/// <summary>
	/// Request has been submitted
	/// </summary>
	Submitted = 1,

	/// <summary>
	/// Request is being processed
	/// </summary>
	InProgress = 2,

	/// <summary>
	/// Request is under review
	/// </summary>
	UnderReview = 3,

	/// <summary>
	/// Request has been approved
	/// </summary>
	Approved = 4,

	/// <summary>
	/// Request has been rejected
	/// </summary>
	Rejected = 5,

	/// <summary>
	/// Request has been completed
	/// </summary>
	Completed = 6,

	/// <summary>
	/// Request has been cancelled
	/// </summary>
	Cancelled = 7
}