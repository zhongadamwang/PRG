using System.Linq.Expressions;

namespace Sanjel.RequestManagement.Entities.Data;

/// <summary>
/// Base interface for data access operations with CRUD and common queries.
/// </summary>
/// <typeparam name="TEntity">Entity type.</typeparam>
public interface IDataAccess<TEntity>
	where TEntity : class
{
	/// <summary>
	/// Gets a paged list of entities.
	/// </summary>
	Task<PagedResult<TEntity>> GetPagedListAsync(
		int pageNumber,
		int pageSize,
		Expression<Func<TEntity, bool>>? filter = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets a list of entities matching the predicate.
	/// </summary>
	Task<List<TEntity>> GetListAsync(
		Expression<Func<TEntity, bool>>? predicate = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets an entity by its primary key.
	/// </summary>
	Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets an entity by its primary key with related entities.
	/// </summary>
	Task<TEntity?> GetByIdWithChildrenAsync(object id, CancellationToken cancellationToken = default);

	/// <summary>
	/// Creates a new entity.
	/// </summary>
	Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);

	/// <summary>
	/// Creates a new entity with related entities.
	/// </summary>
	Task<TEntity> CreateWithChildrenAsync(TEntity entity, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing entity.
	/// </summary>
	Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing entity with related entities.
	/// </summary>
	Task<TEntity> UpdateWithChildrenAsync(TEntity entity, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes an entity.
	/// </summary>
	Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes an entity with related entities.
	/// </summary>
	Task DeleteWithChildrenAsync(TEntity entity, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets a list of entities by their IDs.
	/// </summary>
	Task<List<TEntity>> GetListByIdsAsync(IEnumerable<object> ids, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets a list of entities with related entities by their IDs.
	/// </summary>
	Task<List<TEntity>> GetListWithChildrenByIdsAsync(IEnumerable<object> ids, CancellationToken cancellationToken = default);
}
