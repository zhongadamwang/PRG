using Sanjel.RequestManagement.Entities.Data;
using Sanjel.RequestManagement.Entities.Entities;
using Sanjel.RequestManagement.Repositories.Common;

namespace Sanjel.RequestManagement.Repositories;

/// <summary>
/// Repository adapter for StateDiagram entities.
/// Delegates all operations to the Entities.Data layer.
/// </summary>
public class StateDiagramRepository : BaseRepository<StateDiagram>, IStateDiagramRepository
{
	public StateDiagramRepository(IStateDiagramDataAccess dataAccess)
		: base(dataAccess)
	{
	}
}
