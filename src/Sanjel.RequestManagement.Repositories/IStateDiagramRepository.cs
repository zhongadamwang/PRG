using System.ComponentModel;
using System.Linq.Expressions;
using Sanjel.RequestManagement.Repositories.Common;
using Entity = Sanjel.RequestManagement.Entities.Entities.StateDiagram;

namespace Sanjel.RequestManagement.Repositories;

/// <summary>
/// Repository interface for StateDiagram entity operations.
/// </summary>
public interface IStateDiagramRepository : IRepository<Entity>
{
	/// <summary>
	/// Gets StateDiagram entities within a date range for import_date.
	/// </summary>
	/// <param name="startDate">The start date of the range.</param>
	/// <param name="endDate">The end date of the range.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of StateDiagram entities.</returns>
	[Description("EF Core Query - Date range query, consider indexing")]
	Task<List<Entity>> GetByImportDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

	/// <summary>
	/// Searches StateDiagram entities by diagram_name containing the specified text.
	/// </summary>
	/// <param name="searchText">The text to search for.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of StateDiagram entities.</returns>
	[Description("EF Core Query - Text search, consider full-text indexing")]
	Task<List<Entity>> SearchByDiagramNameAsync(string searchText, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets StateDiagram entities related to a specific DataElement.
	/// </summary>
	/// <param name="dataelementId">The ID of the dataelement.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of StateDiagram entities.</returns>
	[Description("EF Core Query - Foreign key relationship")]
	Task<List<Entity>> GetByDataElementIdAsync(int dataelementId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets StateDiagram entities related to a specific Request.
	/// </summary>
	/// <param name="requestId">The ID of the request.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of StateDiagram entities.</returns>
	[Description("EF Core Query - Foreign key relationship")]
	Task<List<Entity>> GetByRequestIdAsync(int requestId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets a paged list of StateDiagram entities with custom ordering.
	/// </summary>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">The page size.</param>
	/// <param name="filter">Optional filter expression.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a paged result of StateDiagram entities.</returns>
	[Description("EF Core Query - Optimized pagination")]
	Task<PagedResult<Entity>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<Entity, bool>>? filter = null, CancellationToken cancellationToken = default);
}
