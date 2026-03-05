using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Sanjel.RequestManagement.Core.Data;
using Sanjel.RequestManagement.Core.Entities;
using Sanjel.RequestManagement.Repositories.Common;

namespace Sanjel.RequestManagement.Repositories;

/// <summary>
/// Repository implementation for ReviewPackage entities using EF Core.
/// </summary>
public class ReviewPackageRepository : BaseRepository<ReviewPackage>, IReviewPackageRepository
{
	public ReviewPackageRepository(RequestManagementDbContext context)
		: base(context)
	{
	}

	// Implement entity-specific methods
	public async Task<List<ReviewPackage>> GetByReviewStatusAsync(ReviewStatusEnum review_status, CancellationToken cancellationToken = default)
	{
		return await this._dbSet
			.Where(e => e.ReviewStatus == review_status)
			.ToListAsync(cancellationToken);
	}

	public async Task<List<ReviewPackage>> GetBySubmissionDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
	{
		return await this._dbSet
			.Where(e => e.SubmissionDate >= startDate && e.SubmissionDate <= endDate)
			.ToListAsync(cancellationToken);
	}

	public async Task<List<ReviewPackage>> GetByReviewCompletionDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
	{
		return await this._dbSet
			.Where(e => e.ReviewCompletionDate >= startDate && e.ReviewCompletionDate <= endDate)
			.ToListAsync(cancellationToken);
	}

	public async Task<PagedResult<ReviewPackage>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<ReviewPackage, bool>>? filter = null, CancellationToken cancellationToken = default)
	{
		var query = this._dbSet.AsQueryable();

		if (filter != null)
		{
			query = query.Where(filter);
		}

		var totalCount = await query.CountAsync(cancellationToken);
		var skip = (pageNumber - 1) * pageSize;

		var items = await query
			.OrderBy(e => e.SubmissionDate) // Default ordering
			.Skip(skip)
			.Take(pageSize)
			.ToListAsync(cancellationToken);

		return new PagedResult<ReviewPackage>
		{
			Items = items,
			TotalCount = totalCount,
			PageNumber = pageNumber,
			PageSize = pageSize,
		};
	}
}
