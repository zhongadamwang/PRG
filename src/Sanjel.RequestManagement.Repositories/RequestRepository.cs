using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Sanjel.RequestManagement.Core.Data;
using Sanjel.RequestManagement.Core.Entities;
using Sanjel.RequestManagement.Repositories.Common;

namespace Sanjel.RequestManagement.Repositories;

/// <summary>
/// Repository implementation for Request entities using EF Core.
/// Program request submitted for engineering work with complete lifecycle tracking.
/// </summary>
public class RequestRepository : BaseRepository<Request>, IRequestRepository
{
	public RequestRepository(RequestManagementDbContext context)
		: base(context)
	{
	}

	// Implement entity-specific methods
	public async Task<List<Request>> GetByStatusAsync(StatusEnum status, CancellationToken cancellationToken = default)
	{
		return await this._dbSet
			.Where(e => e.Status == status)
			.ToListAsync(cancellationToken);
	}

	public async Task<List<Request>> GetByCreatedDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
	{
		return await this._dbSet
			.Where(e => e.CreatedDate >= startDate && e.CreatedDate <= endDate)
			.ToListAsync(cancellationToken);
	}

	public async Task<List<Request>> GetByAcknowledgmentDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
	{
		return await this._dbSet
			.Where(e => e.AcknowledgmentDate >= startDate && e.AcknowledgmentDate <= endDate)
			.ToListAsync(cancellationToken);
	}

	public async Task<List<Request>> GetByCompletionDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
	{
		return await this._dbSet
			.Where(e => e.CompletionDate >= startDate && e.CompletionDate <= endDate)
			.ToListAsync(cancellationToken);
	}

	public async Task<List<Request>> GetByReviewPackageIdAsync(int reviewpackageId, CancellationToken cancellationToken = default)
	{
		// Relationship exists but requires EF Core navigation properties
		// TODO: Configure navigation properties between Request and ReviewPackage
		return new List<Request>();
	}

	public async Task<List<Request>> GetByDataElementIdAsync(int dataelementId, CancellationToken cancellationToken = default)
	{
		// Relationship exists but requires EF Core navigation properties
		// TODO: Configure navigation properties between Request and DataElement
		return new List<Request>();
	}

	public async Task<List<Request>> GetByNotificationIdAsync(int notificationId, CancellationToken cancellationToken = default)
	{
		// Relationship exists but requires EF Core navigation properties
		// TODO: Configure navigation properties between Request and Notification
		return new List<Request>();
	}

	public async Task<PagedResult<Request>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<Request, bool>>? filter = null, CancellationToken cancellationToken = default)
	{
		var query = this._dbSet.AsQueryable();

		if (filter != null)
		{
			query = query.Where(filter);
		}

		var totalCount = await query.CountAsync(cancellationToken);
		var skip = (pageNumber - 1) * pageSize;

		var items = await query
			.OrderBy(e => e.CreatedDate) // Default ordering
			.Skip(skip)
			.Take(pageSize)
			.ToListAsync(cancellationToken);

		return new PagedResult<Request>
		{
			Items = items,
			TotalCount = totalCount,
			PageNumber = pageNumber,
			PageSize = pageSize,
		};
	}
}
