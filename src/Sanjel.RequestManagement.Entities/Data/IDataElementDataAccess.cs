using System.ComponentModel;
using Sanjel.RequestManagement.Entities.Entities;
using Entity = Sanjel.RequestManagement.Entities.Entities.DataElement;

namespace Sanjel.RequestManagement.Entities.Data;

/// <summary>
/// Data access interface for DataElement entity with specific queries.
/// </summary>
public interface IDataElementDataAccess : IDataAccess<Entity>
{
	/// <summary>
	/// Gets DataElement entities by validation status.
	/// </summary>
	[Description("EF Core Query - High frequency query, consider indexing")]
	Task<List<Entity>> GetByValidationStatusAsync(ValidationEnum validation_status, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets a paged list of DataElement entities with custom ordering.
	/// </summary>
	[Description("EF Core Query - Optimized pagination")]
	Task<PagedResult<Entity>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}
