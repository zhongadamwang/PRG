namespace Sanjel.eServiceCloud.Repositories.Common
{
	public abstract class CommonRepository<TEntity, TModel, TIDataService> : IRepository<TEntity, TModel>
		where TEntity : MetaShare.Common.Core.Entities.Common, new()

		// where TModel : class, Sanjel.eServiceCloud.Core.Models.IModel2<TEntity>, new()
		where TModel : class, Sanjel.eServiceCloud.Core.Models.IModel, new()
		where TIDataService : MetaShare.Common.Core.Services.IPagingService<TEntity>, MetaShare.Common.Core.Services.IService<TEntity>, MetaShare.Common.Core.CommonService.IService
	{
#pragma warning disable SA1401 // FieldsMustBePrivate
		protected readonly TIDataService _dataService;
		protected readonly Sanjel.eServiceCloud.Core.Services.IMappingService _mappingService;
		protected readonly Sanjel.eServiceCloud.Core.Services.IValidationService _validationService;

		// protected readonly Sanjel.eServiceCloud.Core.Services.IDependentDataService _dependentDataService;
#pragma warning restore SA1401 // FieldsMustBePrivate

		public CommonRepository(TIDataService dataService, Sanjel.eServiceCloud.Core.Services.IMappingService mappingService, Sanjel.eServiceCloud.Core.Services.IValidationService validationService)
		{
			this._dataService = dataService;
			this._mappingService = mappingService;
			this._validationService = validationService;
		}

		public virtual async System.Threading.Tasks.Task<Core.Common.PagerResult<TModel>> GetPagedListAsync(Core.Common.Pager pager, System.Linq.Expressions.Expression<Func<TEntity, bool>> expression)
		{
			var entityPager = new MetaShare.Common.Core.Entities.Pager();
			pager.PopulateTo(entityPager);
			var entities = (await System.Threading.Tasks.Task.Run(() => this._dataService.SelectBy(entityPager, new TEntity(), expression))).AsEnumerable();

			var result = await this._mappingService.MapManyAsync<TEntity, TModel>(entities);
			await result.ValidateResultsAsync(this._validationService);
			pager.PopulateFrom(entityPager);

			var pagerResult = new Core.Common.PagerResult<TModel>()
			{
				Pager = pager,
				Result = result.ToList(),
			};

			return pagerResult;
		}

		public virtual async System.Threading.Tasks.Task<TModel> GetByIdAsync(int id)
		{
			var entity = this._dataService.SelectById(new TEntity() { Id = id });

			var result = await this._mappingService.MapOneAsync<TEntity, TModel>(entity);
			return result;
		}

		public virtual async System.Threading.Tasks.Task<TModel> GetByIdWithChildrenAsync(int id)
		{
			var entity = this._dataService.SelectById(new TEntity() { Id = id }, true);

			var result = await this._mappingService.MapOneAsync<TEntity, TModel>(entity);
			return result;
		}

		public virtual async System.Threading.Tasks.Task<bool> CreateAsync(TModel model)
		{
			var entity = await this.GetMdmEntityAsync(model);
			if (entity == null)
			{
				return false;
			}

			var created = await System.Threading.Tasks.Task.Run(() => this._dataService.Insert(entity, false));
			model.Id = entity.Id;
			return created == 1;
		}

		public async System.Threading.Tasks.Task<bool> CreateWithChildrenAsync(TModel model)
		{
			var entity = await this.GetMdmEntityAsync(model);
			if (entity == null)
			{
				return false;
			}

			var created = await System.Threading.Tasks.Task.Run(() => this._dataService.Insert(entity, true));
			model.Id = entity.Id;
			return created == 1;
		}

		public virtual async System.Threading.Tasks.Task<bool> UpdateAsync(TModel model)
		{
			var entity = await this.GetMdmEntityAsync(model);
			if (entity == null)
			{
				return false;
			}

			var updated = await System.Threading.Tasks.Task.Run(() => this._dataService.Update(entity, false));
			return updated == 1;
		}

		public async System.Threading.Tasks.Task<bool> UpdateWithChildrenAsync(TModel model)
		{
			var entity = await this.GetMdmEntityAsync(model);
			if (entity == null)
			{
				return false;
			}

			var updated = await System.Threading.Tasks.Task.Run(() => this._dataService.Update(entity, true));
			return updated == 1;
		}

		public virtual async System.Threading.Tasks.Task<bool> DeleteAsync(TModel model)
		{
			var entity = await this.GetMdmEntityAsync(model);
			if (entity == null)
			{
				return false;
			}

			var deleted = await System.Threading.Tasks.Task.Run(() => this._dataService.Delete(entity, false));
			return deleted == 1;
		}

		public virtual async System.Threading.Tasks.Task<bool> DeleteWithChildrenAsync(TModel model)
		{
			var entity = await this.GetMdmEntityAsync(model);
			if (entity == null)
			{
				return false;
			}

			var deleted = await System.Threading.Tasks.Task.Run(() => this._dataService.Delete(entity, true));
			return deleted == 1;
		}

		public virtual async System.Threading.Tasks.Task<bool> DeleteAsync(int id)
		{
			var entity = new TEntity { Id = id };

			var deleted = await System.Threading.Tasks.Task.Run(() => this._dataService.Delete(entity, false));
			return deleted == 1;
		}

		public virtual async System.Threading.Tasks.Task<bool> DeleteWithChildrenAsync(int id)
		{
			var entity = new TEntity { Id = id };

			var deleted = await System.Threading.Tasks.Task.Run(() => this._dataService.Delete(entity, true));
			return deleted == 1;
		}

		public virtual async System.Threading.Tasks.Task<List<TModel>> GetListAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> expression)
		{
			var entities = (await System.Threading.Tasks.Task.Run(() => this._dataService.SelectBy(new TEntity(), expression))).AsEnumerable();

			var result = await this._mappingService.MapManyAsync<TEntity, TModel>(entities);
			var ret = await result.ValidateResultsAsync(this._validationService);
			return ret == null ? new List<TModel>() : ret.ToList();
		}

		public virtual async System.Threading.Tasks.Task<List<TModel>> GetListByIdsAsync(string columnName, IEnumerable<int> ids)
		{
			var entities = (await System.Threading.Tasks.Task.Run(() => this._dataService.SelectByColumnIds(columnName, ids.ToArray(), false))).AsEnumerable();

			var result = await this._mappingService.MapManyAsync<TEntity, TModel>(entities);
			var ret = await result.ValidateResultsAsync(this._validationService);
			return ret == null ? new List<TModel>() : ret.ToList();
		}

		public virtual async System.Threading.Tasks.Task<List<TModel>> GetListWithChildrenByIdsAsync(string columnName, IEnumerable<int> ids)
		{
			var entities = (await System.Threading.Tasks.Task.Run(() => this._dataService.SelectByColumnIds(columnName, ids.ToArray(), true))).AsEnumerable();

			var result = await this._mappingService.MapManyAsync<TEntity, TModel>(entities);
			var ret = await result.ValidateResultsAsync(this._validationService);
			return ret == null ? new List<TModel>() : ret.ToList();
		}

		protected virtual async System.Threading.Tasks.Task<TEntity?> GetMdmEntityAsync(TModel model)
		{
			Func<TModel, TEntity> toMdm = Sanjel.eServiceCloud.Repositories.Common.MdmMap.Map<TModel, TEntity>;
			var result = await this._validationService.ValidateAsync(model, new[] { Sanjel.eServiceCloud.Core.Validators.RuleSets.Update }, true);
			if (result == null)
			{
				return null;
			}

			var entity = toMdm.Invoke(result);
			return entity;
		}
	}
}
