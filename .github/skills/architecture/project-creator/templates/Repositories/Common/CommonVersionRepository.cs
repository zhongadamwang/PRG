namespace Sanjel.eServiceCloud.Repositories.Common
{
	public abstract class CommonVersionRepository<TEntity, TModel, TIDataService> : CommonRepository<TEntity, TModel, TIDataService>
		where TEntity : MetaShare.Common.Core.Entities.ObjectVersion, new()

		// where TModel : class, Sanjel.eServiceCloud.Core.Models.IModel2<TEntity>, new()
		where TModel : class, Sanjel.eServiceCloud.Core.Models.IModel, new()
		where TIDataService : MetaShare.Common.Core.Services.IPagingService<TEntity>, MetaShare.Common.Core.Services.IService<TEntity>, MetaShare.Common.Core.CommonService.IService
	{
#pragma warning disable SA1401 // FieldsMustBePrivate
		protected readonly Sanjel.eServiceCloud.Core.Services.ICurrentUserService _currentUserService;
#pragma warning restore SA1401 // FieldsMustBePrivate

		public CommonVersionRepository(TIDataService dataService, Sanjel.eServiceCloud.Core.Services.IMappingService mappingService, Sanjel.eServiceCloud.Core.Services.IValidationService validationService, Sanjel.eServiceCloud.Core.Services.ICurrentUserService currentUserService)
			: base(dataService, mappingService, validationService)
		{
			this._currentUserService = currentUserService;
		}

		protected override async Task<TEntity?> GetMdmEntityAsync(TModel model)
		{
			var entity = await base.GetMdmEntityAsync(model);
			if (entity == null)
			{
				return null;
			}

			var userName = this._currentUserService.GetCurrentUsername();
			entity.ModifiedUserName = userName;
			return entity;
		}
	}
}
