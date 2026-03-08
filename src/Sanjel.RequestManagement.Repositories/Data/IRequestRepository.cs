using Sanjel.RequestManagement.Entities.Entities;

namespace Sanjel.RequestManagement.Repositories.Data;

/// <summary>
/// Repository interface for Request entity providing domain-specific data access operations.
/// Wraps IRequestDataAccess to provide repository pattern abstraction.
/// </summary>
public interface IRequestRepository : Sanjel.RequestManagement.Repositories.Common.IRepository<Request>
{
	/// <summary>
	/// Gets Request entities by status.
	/// </summary>
	Task<List<Request>> GetByStatusAsync(StatusEnum status, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets Request entities within a date range for created_date.
	/// </summary>
	Task<List<Request>> GetByCreatedDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets Request entities within a date range for acknowledgment_date.
	/// </summary>
	Task<List<Request>> GetByAcknowledgmentDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets Request entities within a date range for completion_date.
	/// </summary>
	Task<List<Request>> GetByCompletionDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets Request entities related to a specific ReviewPackage.
	/// </summary>
	Task<List<Request>> GetByReviewPackageIdAsync(int reviewpackageId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets Request entities related to a specific DataElement.
	/// </summary>
	Task<List<Request>> GetByDataElementIdAsync(int dataelementId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets Request entities related to a specific Notification.
	/// </summary>
	Task<List<Request>> GetByNotificationIdAsync(int notificationId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets a paged list of Request entities with custom ordering.
	/// </summary>
	Task<Sanjel.RequestManagement.Repositories.Common.PagedResult<Request>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}
