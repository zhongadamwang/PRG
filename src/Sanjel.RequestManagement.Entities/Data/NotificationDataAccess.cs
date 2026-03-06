using Microsoft.EntityFrameworkCore;
using Sanjel.RequestManagement.Entities.Entities;

namespace Sanjel.RequestManagement.Entities.Data;

/// <summary>
/// Data access implementation for Notification entities.
/// System-generated communication to stakeholders about request status and actions.
/// </summary>
public class NotificationDataAccess : BaseDataAccess<Notification>, INotificationDataAccess
{
	public NotificationDataAccess(RequestManagementDbContext context)
		: base(context)
	{
	}

	public async Task<List<Notification>> GetBySentDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
	{
		return await this._dbSet
			.Where(e => e.SentDate >= startDate && e.SentDate <= endDate)
			.ToListAsync(cancellationToken);
	}

	public async Task<PagedResult<Notification>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
	{
		var query = this._dbSet.AsQueryable();

		var totalCount = await query.CountAsync(cancellationToken);
		var skip = (pageNumber - 1) * pageSize;

		var items = await query
			.OrderBy(e => e.SentDate) // Default ordering
			.Skip(skip)
			.Take(pageSize)
			.ToListAsync(cancellationToken);

		return new PagedResult<Notification>
		{
			Items = items,
			TotalCount = totalCount,
			PageNumber = pageNumber,
			PageSize = pageSize,
		};
	}
}
