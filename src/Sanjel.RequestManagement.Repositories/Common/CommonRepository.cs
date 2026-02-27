namespace Sanjel.RequestManagement.Repositories.Common
{
	public abstract class CommonRepository<TEntity, TIDataService> : IRepository<TEntity>
		where TEntity : MetaShare.Common.Core.Entities.Common, new()
		where TIDataService : MetaShare.Common.Core.Services.IPagingService<TEntity>, MetaShare.Common.Core.Services.IService<TEntity>, MetaShare.Common.Core.CommonService.IService
	{
#pragma warning disable SA1401 // FieldsMustBePrivate
		protected readonly TIDataService _dataService;

		// protected readonly Sanjel.RequestManagement.Core.Services.IDependentDataService _dependentDataService;
#pragma warning restore SA1401 // FieldsMustBePrivate

		public CommonRepository(TIDataService dataService)
		{
			this._dataService = dataService;
		}

		public virtual async System.Threading.Tasks.Task<Core.Common.PagerResult<TEntity>> GetPagedListAsync(Core.Common.Pager pager, System.Linq.Expressions.Expression<Func<TEntity, bool>> expression)
		{
			var entityPager = new MetaShare.Common.Core.Entities.Pager();
			pager.PopulateTo(entityPager);
			var entities = (await System.Threading.Tasks.Task.Run(() => this._dataService.SelectBy(entityPager, new TEntity(), expression))).AsEnumerable();

			pager.PopulateFrom(entityPager);

			var pagerResult = new Core.Common.PagerResult<TEntity>()
			{
				Pager = pager,
				Result = entities.ToList(),
			};

			return pagerResult;
		}

		public virtual async System.Threading.Tasks.Task<TEntity> GetByIdAsync(int id)
		{
			var entity = await System.Threading.Tasks.Task.Run(() => this._dataService.SelectById(new TEntity() { Id = id }));

			return entity;
		}

		public virtual async System.Threading.Tasks.Task<TEntity> GetByIdWithChildrenAsync(int id)
		{
			var entity = await System.Threading.Tasks.Task.Run(() => this._dataService.SelectById(new TEntity() { Id = id }, true));

			return entity;
		}

		public virtual async System.Threading.Tasks.Task<bool> CreateAsync(TEntity entity)
		{
			if (entity == null)
			{
				return false;
			}

			var en = await this.GetEntityAsync(entity);
			var created = await System.Threading.Tasks.Task.Run(() => this._dataService.Insert(en, false));
			entity.Id = en.Id; // Update the Id back to the entity after creation
			return created == 1;
		}

		public async System.Threading.Tasks.Task<bool> CreateWithChildrenAsync(TEntity entity)
		{
			if (entity == null)
			{
				return false;
			}

			var en = await this.GetEntityAsync(entity);
			var created = await System.Threading.Tasks.Task.Run(() => this._dataService.Insert(en, true));
			entity.Id = en.Id; // Update the Id back to the entity after creation
			return created == 1;
		}

		public virtual async System.Threading.Tasks.Task<bool> UpdateAsync(TEntity entity)
		{
			if (entity == null)
			{
				return false;
			}

			var en = await this.GetEntityAsync(entity);
			var updated = await System.Threading.Tasks.Task.Run(() => this._dataService.Update(en, false));
			return updated == 1;
		}

		public async System.Threading.Tasks.Task<bool> UpdateWithChildrenAsync(TEntity entity)
		{
			if (entity == null)
			{
				return false;
			}

			var en = await this.GetEntityAsync(entity);
			var updated = await System.Threading.Tasks.Task.Run(() => this._dataService.Update(en, true));
			return updated == 1;
		}

		public virtual async System.Threading.Tasks.Task<bool> DeleteAsync(TEntity entity)
		{
			if (entity == null)
			{
				return false;
			}

			var en = await this.GetEntityAsync(entity);
			var deleted = await System.Threading.Tasks.Task.Run(() => this._dataService.Delete(en, false));
			return deleted == 1;
		}

		public virtual async System.Threading.Tasks.Task<bool> DeleteWithChildrenAsync(TEntity entity)
		{
			if (entity == null)
			{
				return false;
			}

			var en = await this.GetEntityAsync(entity);
			var deleted = await System.Threading.Tasks.Task.Run(() => this._dataService.Delete(en, true));
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

		public virtual async System.Threading.Tasks.Task<List<TEntity>> GetListAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> expression)
		{
			var entities = await System.Threading.Tasks.Task.Run(() => this._dataService.SelectBy(new TEntity(), expression));

			return entities ?? new List<TEntity>();
		}

		public virtual async System.Threading.Tasks.Task<List<TEntity>> GetListByIdsAsync(string columnName, IEnumerable<int> ids)
		{
			var entities = await System.Threading.Tasks.Task.Run(() => this._dataService.SelectByColumnIds(columnName, ids.ToArray(), false));

			return entities ?? new List<TEntity>();
		}

		public virtual async System.Threading.Tasks.Task<List<TEntity>> GetListWithChildrenByIdsAsync(string columnName, IEnumerable<int> ids)
		{
			var entities = await System.Threading.Tasks.Task.Run(() => this._dataService.SelectByColumnIds(columnName, ids.ToArray(), true));

			return entities ?? new List<TEntity>();
		}

		protected virtual async System.Threading.Tasks.Task<TEntity> GetEntityAsync(TEntity entity)
		{
			return entity;
		}
	}
}
