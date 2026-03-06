using Microsoft.EntityFrameworkCore;
using Sanjel.RequestManagement.Entities.Entities;

namespace Sanjel.RequestManagement.Entities.Data;

/// <summary>
/// Data access implementation for ReviewPackage entities.
/// </summary>
public class ReviewPackageDataAccess : BaseDataAccess<ReviewPackage>, IReviewPackageDataAccess
{
	public ReviewPackageDataAccess(RequestManagementDbContext context)
		: base(context)
	{
	}

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

	public async Task<PagedResult<ReviewPackage>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
	{
		var query = this._dbSet.AsQueryable();

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
