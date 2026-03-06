using Sanjel.RequestManagement.Entities.Data;
using Sanjel.RequestManagement.Entities.Entities;
using Sanjel.RequestManagement.Repositories.Common;

namespace Sanjel.RequestManagement.Repositories;

/// <summary>
/// Repository adapter for Notification entities.
/// Delegates all operations to the Entities.Data layer.
/// </summary>
public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
{
	public NotificationRepository(INotificationDataAccess dataAccess)
		: base(dataAccess)
	{
	}
}
