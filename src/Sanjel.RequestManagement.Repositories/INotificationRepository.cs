using System.ComponentModel;
using System.Linq.Expressions;
using Sanjel.RequestManagement.Repositories.Common;
using Entity = Sanjel.RequestManagement.Entities.Entities.Notification;

namespace Sanjel.RequestManagement.Repositories;

/// <summary>
/// Repository interface for Notification entity operations.
/// System-generated communication to stakeholders about request status and actions.
/// </summary>
public interface INotificationRepository : IRepository<Entity>
{
	/// <summary>
	/// Gets Notification entities within a date range for sent_date.
	/// </summary>
	/// <param name="startDate">The start date of the range.</param>
	/// <param name="endDate">The end date of the range.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of Notification entities.</returns>
	[Description("EF Core Query - Date range query, consider indexing")]
	Task<List<Entity>> GetBySentDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets a paged list of Notification entities with custom ordering.
	/// </summary>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">The page size.</param>
	/// <param name="filter">Optional filter expression.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a paged result of Notification entities.</returns>
	[Description("EF Core Query - Optimized pagination")]
	Task<PagedResult<Entity>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<Entity, bool>>? filter = null, CancellationToken cancellationToken = default);
}
