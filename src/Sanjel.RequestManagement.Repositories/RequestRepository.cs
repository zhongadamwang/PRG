using Sanjel.RequestManagement.Entities.Data;
using Sanjel.RequestManagement.Entities.Entities;
using Sanjel.RequestManagement.Repositories.Common;

namespace Sanjel.RequestManagement.Repositories;

/// <summary>
/// Repository adapter for Request entities.
/// Delegates all operations to the Entities.Data layer.
/// </summary>
public class RequestRepository : BaseRepository<Request>, IRequestRepository
{
	public RequestRepository(IRequestDataAccess dataAccess)
		: base(dataAccess)
	{
	}
}
