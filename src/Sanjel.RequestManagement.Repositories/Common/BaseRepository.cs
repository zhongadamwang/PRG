using System.Linq.Expressions;
using Sanjel.RequestManagement.Entities.Data;

namespace Sanjel.RequestManagement.Repositories.Common;

/// <summary>
/// Base repository adapter implementation that delegates to Entities.Data layer.
/// </summary>
/// <typeparam name="TEntity">Entity type.</typeparam>
public class BaseRepository<TEntity> : IRepository<TEntity>
	where TEntity : class
{
#pragma warning disable SA1401
	protected readonly IDataAccess<TEntity> _dataAccess;
#pragma warning restore SA1401

	public BaseRepository(IDataAccess<TEntity> dataAccess)
	{
		this._dataAccess = dataAccess;
	}

	public virtual async Task<Sanjel.RequestManagement.Entities.Data.PagedResult<TEntity>> GetPagedListAsync(
		int pageNumber,
		int pageSize,
		Expression<Func<TEntity, bool>>? filter = null,
		CancellationToken cancellationToken = default)
	{
		return await this._dataAccess.GetPagedListAsync(pageNumber, pageSize, filter, cancellationToken);
	}

	public virtual async Task<List<TEntity>> GetListAsync(
		Expression<Func<TEntity, bool>>? predicate = null,
		CancellationToken cancellationToken = default)
	{
		return await this._dataAccess.GetListAsync(predicate, cancellationToken);
	}

	public virtual async Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
	{
		return await this._dataAccess.GetByIdAsync(id, cancellationToken);
	}

	public virtual async Task<TEntity?> GetByIdWithChildrenAsync(object id, CancellationToken cancellationToken = default)
	{
		return await this._dataAccess.GetByIdWithChildrenAsync(id, cancellationToken);
	}

	public virtual async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		return await this._dataAccess.CreateAsync(entity, cancellationToken);
	}

	public virtual async Task<TEntity> CreateWithChildrenAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		return await this._dataAccess.CreateWithChildrenAsync(entity, cancellationToken);
	}

	public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		return await this._dataAccess.UpdateAsync(entity, cancellationToken);
	}

	public virtual async Task<TEntity> UpdateWithChildrenAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		return await this._dataAccess.UpdateWithChildrenAsync(entity, cancellationToken);
	}

	public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		await this._dataAccess.DeleteAsync(entity, cancellationToken);
	}

	public virtual async Task DeleteWithChildrenAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		await this._dataAccess.DeleteWithChildrenAsync(entity, cancellationToken);
	}

	public virtual async Task<List<TEntity>> GetListByIdsAsync(IEnumerable<object> ids, CancellationToken cancellationToken = default)
	{
		return await this._dataAccess.GetListByIdsAsync(ids, cancellationToken);
	}

	public virtual async Task<List<TEntity>> GetListWithChildrenByIdsAsync(IEnumerable<object> ids, CancellationToken cancellationToken = default)
	{
		return await this._dataAccess.GetListWithChildrenByIdsAsync(ids, cancellationToken);
	}
}
