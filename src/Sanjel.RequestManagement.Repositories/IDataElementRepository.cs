using System.ComponentModel;
using System.Linq.Expressions;
using Sanjel.RequestManagement.Entities.Entities;
using Sanjel.RequestManagement.Repositories.Common;
using Entity = Sanjel.RequestManagement.Entities.Entities.DataElement;

namespace Sanjel.RequestManagement.Repositories;

/// <summary>
/// Repository interface for DataElement entity operations.
/// </summary>
public interface IDataElementRepository : IRepository<Entity>
{
	/// <summary>
	/// Gets DataElement entities by status.
	/// </summary>
	/// <param name="validation_status">The status to filter by.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of DataElement entities.</returns>
	[Description("EF Core Query - High frequency query, consider indexing")]
	Task<List<Entity>> GetByValidationStatusAsync(ValidationEnum validation_status, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets a paged list of DataElement entities with custom ordering.
	/// </summary>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">The page size.</param>
	/// <param name="filter">Optional filter expression.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a paged result of DataElement entities.</returns>
	[Description("EF Core Query - Optimized pagination")]
	Task<PagedResult<Entity>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<Entity, bool>>? filter = null, CancellationToken cancellationToken = default);
}
