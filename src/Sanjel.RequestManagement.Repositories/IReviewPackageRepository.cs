using System.ComponentModel;
using System.Linq.Expressions;
using Sanjel.RequestManagement.Entities.Entities;
using Sanjel.RequestManagement.Repositories.Common;
using Entity = Sanjel.RequestManagement.Entities.Entities.ReviewPackage;

namespace Sanjel.RequestManagement.Repositories;

/// <summary>
/// Repository interface for ReviewPackage entity operations.
/// </summary>
public interface IReviewPackageRepository : IRepository<Entity>
{
	/// <summary>
	/// Gets ReviewPackage entities by status.
	/// </summary>
	/// <param name="review_status">The status to filter by.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of ReviewPackage entities.</returns>
	[Description("EF Core Query - High frequency query, consider indexing")]
	Task<List<Entity>> GetByReviewStatusAsync(ReviewStatusEnum review_status, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets ReviewPackage entities within a date range for submission_date.
	/// </summary>
	/// <param name="startDate">The start date of the range.</param>
	/// <param name="endDate">The end date of the range.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of ReviewPackage entities.</returns>
	[Description("EF Core Query - Date range query, consider indexing")]
	Task<List<Entity>> GetBySubmissionDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets ReviewPackage entities within a date range for review_completion_date.
	/// </summary>
	/// <param name="startDate">The start date of the range.</param>
	/// <param name="endDate">The end date of the range.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of ReviewPackage entities.</returns>
	[Description("EF Core Query - Date range query, consider indexing")]
	Task<List<Entity>> GetByReviewCompletionDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets a paged list of ReviewPackage entities with custom ordering.
	/// </summary>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">The page size.</param>
	/// <param name="filter">Optional filter expression.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a paged result of ReviewPackage entities.</returns>
	[Description("EF Core Query - Optimized pagination")]
	Task<PagedResult<Entity>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<Entity, bool>>? filter = null, CancellationToken cancellationToken = default);
}
