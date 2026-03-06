using Sanjel.RequestManagement.Entities.Data;
using Sanjel.RequestManagement.Entities.Entities;
using Sanjel.RequestManagement.Repositories.Common;

namespace Sanjel.RequestManagement.Repositories;

/// <summary>
/// Repository adapter for DataElement entities.
/// Delegates all operations to the Entities.Data layer.
/// </summary>
public class DataElementRepository : BaseRepository<DataElement>, IDataElementRepository
{
	public DataElementRepository(IDataElementDataAccess dataAccess)
		: base(dataAccess)
	{
	}
}
