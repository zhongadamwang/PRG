using System.ComponentModel;
using Sanjel.RequestManagement.Entities.Entities;
using Entity = Sanjel.RequestManagement.Entities.Entities.ReviewPackage;

namespace Sanjel.RequestManagement.Entities.Data;

/// <summary>
/// Data access interface for ReviewPackage entity with specific queries.
/// </summary>
public interface IReviewPackageDataAccess : IDataAccess<Entity>
{
	/// <summary>
	/// Gets ReviewPackage entities by review status.
	/// </summary>
	[Description("EF Core Query - High frequency query, consider indexing")]
	Task<List<Entity>> GetByReviewStatusAsync(ReviewStatusEnum review_status, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets ReviewPackage entities within a date range for submission_date.
	/// </summary>
	[Description("EF Core Query - Date range query, consider indexing")]
	Task<List<Entity>> GetBySubmissionDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets ReviewPackage entities within a date range for review_completion_date.
	/// </summary>
	[Description("EF Core Query - Date range query, consider indexing")]
	Task<List<Entity>> GetByReviewCompletionDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets a paged list of ReviewPackage entities with custom ordering.
	/// </summary>
	[Description("EF Core Query - Optimized pagination")]
	Task<PagedResult<Entity>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}
