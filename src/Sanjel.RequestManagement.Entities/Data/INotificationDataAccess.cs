using System.ComponentModel;
using Entity = Sanjel.RequestManagement.Entities.Entities.Notification;

namespace Sanjel.RequestManagement.Entities.Data;

/// <summary>
/// Data access interface for Notification entity with specific queries.
/// </summary>
public interface INotificationDataAccess : IDataAccess<Entity>
{
	/// <summary>
	/// Gets Notification entities within a date range for sent_date.
	/// </summary>
	[Description("EF Core Query - Date range query, consider indexing")]
	Task<List<Entity>> GetBySentDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets a paged list of Notification entities with custom ordering.
	/// </summary>
	[Description("EF Core Query - Optimized pagination")]
	Task<PagedResult<Entity>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}
