using System.ComponentModel;
using System.Linq.Expressions;
using Sanjel.RequestManagement.Core.Entities;
using Sanjel.RequestManagement.Repositories.Common;
using Entity = Sanjel.RequestManagement.Core.Entities.Request;

namespace Sanjel.RequestManagement.Repositories;

/// <summary>
/// Repository interface for Request entity operations.
/// Program request submitted for engineering work with complete lifecycle tracking.
/// </summary>
public interface IRequestRepository : IRepository<Entity>
{
	/// <summary>
	/// Gets Request entities by status.
	/// </summary>
	/// <param name="status">The status to filter by.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of Request entities.</returns>
	[Description("EF Core Query - High frequency query, consider indexing")]
	Task<List<Entity>> GetByStatusAsync(StatusEnum status, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets Request entities within a date range for created_date.
	/// </summary>
	/// <param name="startDate">The start date of the range.</param>
	/// <param name="endDate">The end date of the range.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of Request entities.</returns>
	[Description("EF Core Query - Date range query, consider indexing")]
	Task<List<Entity>> GetByCreatedDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets Request entities within a date range for acknowledgment_date.
	/// </summary>
	/// <param name="startDate">The start date of the range.</param>
	/// <param name="endDate">The end date of the range.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of Request entities.</returns>
	[Description("EF Core Query - Date range query, consider indexing")]
	Task<List<Entity>> GetByAcknowledgmentDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets Request entities within a date range for completion_date.
	/// </summary>
	/// <param name="startDate">The start date of the range.</param>
	/// <param name="endDate">The end date of the range.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of Request entities.</returns>
	[Description("EF Core Query - Date range query, consider indexing")]
	Task<List<Entity>> GetByCompletionDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets Request entities related to a specific ReviewPackage.
	/// </summary>
	/// <param name="reviewpackageId">The ID of the reviewpackage.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of Request entities.</returns>
	[Description("EF Core Query - Foreign key relationship")]
	Task<List<Entity>> GetByReviewPackageIdAsync(int reviewpackageId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets Request entities related to a specific DataElement.
	/// </summary>
	/// <param name="dataelementId">The ID of the dataelement.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of Request entities.</returns>
	[Description("EF Core Query - Foreign key relationship")]
	Task<List<Entity>> GetByDataElementIdAsync(int dataelementId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets Request entities related to a specific Notification.
	/// </summary>
	/// <param name="notificationId">The ID of the notification.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of Request entities.</returns>
	[Description("EF Core Query - Foreign key relationship")]
	Task<List<Entity>> GetByNotificationIdAsync(int notificationId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets a paged list of Request entities with custom ordering.
	/// </summary>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">The page size.</param>
	/// <param name="filter">Optional filter expression.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a paged result of Request entities.</returns>
	[Description("EF Core Query - Optimized pagination")]
	Task<PagedResult<Entity>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<Entity, bool>>? filter = null, CancellationToken cancellationToken = default);
}
