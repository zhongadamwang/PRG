using Entity = Sesi.SanjelData.Entities.BusinessEntities.Operation.Crew.ThirdPartyBulkerCrew;
using IDataService = Sesi.SanjelData.Services.Interfaces.BusinessEntities.Operation.Crew.IThirdPartyBulkerCrewService;

namespace Sanjel.RequestManagement.Repositories
{
	public interface IThirdPartyBulkerCrewRepository : Sanjel.RequestManagement.Repositories.Common.IRepository<Entity>
	{
	}

	public sealed class ThirdPartyBulkerCrewRepository : Sanjel.RequestManagement.Repositories.Common.CommonVersionRepository<Entity, IDataService>, IThirdPartyBulkerCrewRepository
	{
		public ThirdPartyBulkerCrewRepository(IDataService dataService, Sanjel.RequestManagement.Core.Services.ICurrentUserService currentUserService)
			: base(dataService, currentUserService)
		{
		}
	}
}