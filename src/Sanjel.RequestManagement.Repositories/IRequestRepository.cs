using Sanjel.RequestManagement.Repositories.Common;
using Entity = Sanjel.RequestManagement.Entities.Entities.Request;

namespace Sanjel.RequestManagement.Repositories;

/// <summary>
/// Repository interface for Request entity operations.
/// Program request submitted for engineering work with complete lifecycle tracking.
/// </summary>
public interface IRequestRepository : IRepository<Entity>
{
}
