using Entity = Sesi.SanjelData.Entities.BusinessEntities.Operation.Dispatch.CallSheet;
using IDataService = Sesi.SanjelData.Services.Interfaces.BusinessEntities.Operation.Dispatch.ICallSheetService;

namespace Sanjel.RequestManagement.Repositories
{
	public interface ICallSheetRepository : Sanjel.RequestManagement.Repositories.Common.IRepository<Entity>
	{
	}

	public sealed class CallSheetRepository : Sanjel.RequestManagement.Repositories.Common.CommonRepository<Entity, IDataService>, ICallSheetRepository
	{
		public CallSheetRepository(IDataService dataService)
			: base(dataService)
		{
		}
	}
}