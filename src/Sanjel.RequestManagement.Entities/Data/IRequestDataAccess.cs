using System.ComponentModel;
using Sanjel.RequestManagement.Entities.Entities;
using Entity = Sanjel.RequestManagement.Entities.Entities.Request;

namespace Sanjel.RequestManagement.Entities.Data;

/// <summary>
/// Data access interface for Request entity with specific queries.
/// </summary>
public interface IRequestDataAccess : IDataAccess<Entity>
{
	/// <summary>
	/// Gets Request entities by status.
	/// </summary>
	[Description("EF Core Query - High frequency query, consider indexing")]
	Task<List<Entity>> GetByStatusAsync(StatusEnum status, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets Request entities within a date range for created_date.
	/// </summary>
	[Description("EF Core Query - Date range query, consider indexing")]
	Task<List<Entity>> GetByCreatedDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets Request entities within a date range for acknowledgment_date.
	/// </summary>
	[Description("EF Core Query - Date range query, consider indexing")]
	Task<List<Entity>> GetByAcknowledgmentDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets Request entities within a date range for completion_date.
	/// </summary>
	[Description("EF Core Query - Date range query, consider indexing")]
	Task<List<Entity>> GetByCompletionDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets Request entities related to a specific ReviewPackage.
	/// </summary>
	[Description("EF Core Query - Foreign key relationship")]
	Task<List<Entity>> GetByReviewPackageIdAsync(int reviewpackageId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets Request entities related to a specific DataElement.
	/// </summary>
	[Description("EF Core Query - Foreign key relationship")]
	Task<List<Entity>> GetByDataElementIdAsync(int dataelementId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets Request entities related to a specific Notification.
	/// </summary>
	[Description("EF Core Query - Foreign key relationship")]
	Task<List<Entity>> GetByNotificationIdAsync(int notificationId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets a paged list of Request entities with custom ordering.
	/// </summary>
	[Description("EF Core Query - Optimized pagination")]
	Task<PagedResult<Entity>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}
