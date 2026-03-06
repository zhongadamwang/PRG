using System.ComponentModel;
using Entity = Sanjel.RequestManagement.Entities.Entities.StateDiagram;

namespace Sanjel.RequestManagement.Entities.Data;

/// <summary>
/// Data access interface for StateDiagram entity with specific queries.
/// </summary>
public interface IStateDiagramDataAccess : IDataAccess<Entity>
{
	/// <summary>
	/// Gets StateDiagram entities within a date range for import_date.
	/// </summary>
	[Description("EF Core Query - Date range query, consider indexing")]
	Task<List<Entity>> GetByImportDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

	/// <summary>
	/// Searches StateDiagram entities by diagram_name containing the specified text.
	/// </summary>
	[Description("EF Core Query - Text search, consider full-text indexing")]
	Task<List<Entity>> SearchByDiagramNameAsync(string searchText, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets StateDiagram entities related to a specific DataElement.
	/// </summary>
	[Description("EF Core Query - Foreign key relationship")]
	Task<List<Entity>> GetByDataElementIdAsync(int dataelementId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets StateDiagram entities related to a specific Request.
	/// </summary>
	[Description("EF Core Query - Foreign key relationship")]
	Task<List<Entity>> GetByRequestIdAsync(int requestId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets a paged list of StateDiagram entities with custom ordering.
	/// </summary>
	[Description("EF Core Query - Optimized pagination")]
	Task<PagedResult<Entity>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}
