namespace Sanjel.RequestManagement.Repositories.Common
{
	public abstract class CommonVersionRepository<TEntity, TIDataService> : CommonRepository<TEntity, TIDataService>
		where TEntity : MetaShare.Common.Core.Entities.ObjectVersion, new()

		where TIDataService : MetaShare.Common.Core.Services.IPagingService<TEntity>, MetaShare.Common.Core.Services.IService<TEntity>, MetaShare.Common.Core.CommonService.IService
	{
#pragma warning disable SA1401 // FieldsMustBePrivate
		protected readonly Sanjel.RequestManagement.Core.Services.ICurrentUserService _currentUserService;
#pragma warning restore SA1401 // FieldsMustBePrivate

		public CommonVersionRepository(TIDataService dataService, Sanjel.RequestManagement.Core.Services.ICurrentUserService currentUserService)
			: base(dataService)
		{
			this._currentUserService = currentUserService;
		}

		protected override async Task<TEntity> GetEntityAsync(TEntity entity)
		{
			var userName = this._currentUserService.GetCurrentUsername();
			entity.ModifiedUserName = userName;
			return entity;
		}
	}
}
