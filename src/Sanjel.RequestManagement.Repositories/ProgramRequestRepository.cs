using Entity = Sesi.SanjelData.Entities.BusinessEntities.Engineering.ProgramDesign.ProgramRequest;
using IDataService = Sesi.SanjelData.Services.Interfaces.BusinessEntities.Engineering.ProgramDesign.IProgramRequestService;

namespace Sanjel.RequestManagement.Repositories
{
	public interface IProgramRequestRepository : Sanjel.RequestManagement.Repositories.Common.IRepository<Entity>
	{
	}

	public sealed class ProgramRequestRepository : Sanjel.RequestManagement.Repositories.Common.CommonRepository<Entity, IDataService>, IProgramRequestRepository
	{
		public ProgramRequestRepository(IDataService dataService)
			: base(dataService)
		{
		}
	}
}