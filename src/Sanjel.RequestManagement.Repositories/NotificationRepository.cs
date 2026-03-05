using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Sanjel.RequestManagement.Entities.Data;
using Sanjel.RequestManagement.Entities.Entities;
using Sanjel.RequestManagement.Repositories.Common;

namespace Sanjel.RequestManagement.Repositories;

/// <summary>
/// Repository implementation for Notification entities using EF Core.
/// System-generated communication to stakeholders about request status and actions.
/// </summary>
public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
{
	public NotificationRepository(RequestManagementDbContext context)
		: base(context)
	{
	}

	// Implement entity-specific methods
	public async Task<List<Notification>> GetBySentDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
	{
		return await this._dbSet
			.Where(e => e.SentDate >= startDate && e.SentDate <= endDate)
			.ToListAsync(cancellationToken);
	}

	public async Task<PagedResult<Notification>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<Notification, bool>>? filter = null, CancellationToken cancellationToken = default)
	{
		var query = this._dbSet.AsQueryable();

		if (filter != null)
		{
			query = query.Where(filter);
		}

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
