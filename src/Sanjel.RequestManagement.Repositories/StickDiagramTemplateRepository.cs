using Entity = Sesi.SanjelData.Entities.BusinessEntities.Engineering.ProgramDesign.Template.StickDiagramTemplate;
using IDataService = Sesi.SanjelData.Services.Interfaces.BusinessEntities.Engineering.ProgramDesign.Template.IStickDiagramTemplateService;

namespace Sanjel.RequestManagement.Repositories
{
	public interface IStickDiagramTemplateRepository : Sanjel.RequestManagement.Repositories.Common.IRepository<Entity>
	{
	}

	public sealed class StickDiagramTemplateRepository : Sanjel.RequestManagement.Repositories.Common.CommonVersionRepository<Entity, IDataService>, IStickDiagramTemplateRepository
	{
		public StickDiagramTemplateRepository(IDataService dataService, Sanjel.RequestManagement.Core.Services.ICurrentUserService currentUserService)
			: base(dataService, currentUserService)
		{
		}
	}
}
