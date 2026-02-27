namespace Sanjel.RequestManagement.Repositories.Common
{
	/// <summary>
	/// Generic repository interface for basic CRUD and listing with paging.
	/// </summary>
	/// <typeparam name="TEntity">Underlying MDM entity type used for filtering expressions.</typeparam>
	public interface IRepository<TEntity>
		where TEntity : MetaShare.Common.Core.Entities.Common, new()
	{
		System.Threading.Tasks.Task<Sanjel.RequestManagement.Core.Common.PagerResult<TEntity>> GetPagedListAsync(Sanjel.RequestManagement.Core.Common.Pager pager, System.Linq.Expressions.Expression<Func<TEntity, bool>> expression);

		System.Threading.Tasks.Task<List<TEntity>> GetListAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> expression);

		System.Threading.Tasks.Task<TEntity> GetByIdAsync(int id);

		System.Threading.Tasks.Task<TEntity> GetByIdWithChildrenAsync(int id);

		System.Threading.Tasks.Task<bool> CreateAsync(TEntity entity);

		System.Threading.Tasks.Task<bool> CreateWithChildrenAsync(TEntity entity);

		System.Threading.Tasks.Task<bool> UpdateAsync(TEntity entity);

		System.Threading.Tasks.Task<bool> UpdateWithChildrenAsync(TEntity entity);

		System.Threading.Tasks.Task<bool> DeleteAsync(TEntity entity);

		System.Threading.Tasks.Task<bool> DeleteWithChildrenAsync(TEntity entity);

		System.Threading.Tasks.Task<bool> DeleteAsync(int id);

		System.Threading.Tasks.Task<bool> DeleteWithChildrenAsync(int id);

		System.Threading.Tasks.Task<List<TEntity>> GetListByIdsAsync(string columnName, IEnumerable<int> ids);

		System.Threading.Tasks.Task<List<TEntity>> GetListWithChildrenByIdsAsync(string columnName, IEnumerable<int> ids);
	}
}
