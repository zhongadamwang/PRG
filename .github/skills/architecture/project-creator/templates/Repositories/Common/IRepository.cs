namespace Sanjel.eServiceCloud.Repositories.Common
{
	/// <summary>
	/// Generic repository interface for basic CRUD and listing with paging.
	/// </summary>
	/// <typeparam name="TEntity">Underlying MDM entity type used for filtering expressions.</typeparam>
	/// <typeparam name="TModel">Application model type returned by repository operations.</typeparam>
	public interface IRepository<TEntity, TModel>
		where TEntity : MetaShare.Common.Core.Entities.Common, new()
		where TModel : class, Sanjel.eServiceCloud.Core.Models.IModel, new()
	{
		System.Threading.Tasks.Task<Sanjel.eServiceCloud.Core.Common.PagerResult<TModel>> GetPagedListAsync(Sanjel.eServiceCloud.Core.Common.Pager pager, System.Linq.Expressions.Expression<Func<TEntity, bool>> expression);

		System.Threading.Tasks.Task<List<TModel>> GetListAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> expression);

		System.Threading.Tasks.Task<TModel> GetByIdAsync(int id);

		System.Threading.Tasks.Task<TModel> GetByIdWithChildrenAsync(int id);

		System.Threading.Tasks.Task<bool> CreateAsync(TModel model);

		System.Threading.Tasks.Task<bool> CreateWithChildrenAsync(TModel model);

		System.Threading.Tasks.Task<bool> UpdateAsync(TModel model);

		System.Threading.Tasks.Task<bool> UpdateWithChildrenAsync(TModel model);

		System.Threading.Tasks.Task<bool> DeleteAsync(TModel model);

		System.Threading.Tasks.Task<bool> DeleteWithChildrenAsync(TModel model);

		System.Threading.Tasks.Task<bool> DeleteAsync(int id);

		System.Threading.Tasks.Task<bool> DeleteWithChildrenAsync(int id);

		System.Threading.Tasks.Task<List<TModel>> GetListByIdsAsync(string columnName, IEnumerable<int> ids);

		System.Threading.Tasks.Task<List<TModel>> GetListWithChildrenByIdsAsync(string columnName, IEnumerable<int> ids);

		// System.Threading.Tasks.Task<T> GetDependentDataSingleAsync<T>(int id)
		// 	where T : class, Sanjel.eServiceCloud.Core.Models.IModel, new();

		// System.Threading.Tasks.Task<IEnumerable<T>> GetDependentDataListAsync<T>()
		// 	where T : class, Sanjel.eServiceCloud.Core.Models.IModel, new();
	}
}
