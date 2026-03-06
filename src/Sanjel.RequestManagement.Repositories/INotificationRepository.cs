using Sanjel.RequestManagement.Repositories.Common;
using Entity = Sanjel.RequestManagement.Entities.Entities.Notification;

namespace Sanjel.RequestManagement.Repositories;

/// <summary>
/// Repository interface for Notification entity operations.
/// System-generated communication to stakeholders about request status and actions.
/// </summary>
public interface INotificationRepository : IRepository<Entity>
{
}
