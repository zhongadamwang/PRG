using System.Linq.Expressions;

namespace {Prg}.{ProjectName}.Repositories.Common;

/// <summary>
/// Generic repository interface for EF Core-based CRUD operations and querying.
/// </summary>
/// <typeparam name="TEntity">Entity type that inherits from a base entity class.</typeparam>
public interface IRepository<TEntity>
		where TEntity : class
{
	/// <summary>
	/// Gets a queryable collection of entities for advanced querying.
	/// </summary>
	/// <returns>IQueryable of entities for LINQ operations.</returns>
	IQueryable<TEntity> Query();

	/// <summary>
	/// Gets an entity by its primary key.
	/// </summary>
	/// <param name="id">Primary key value.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>Entity or null if not found.</returns>
	Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets a single entity matching the predicate.
	/// </summary>
	/// <param name="predicate">Filter expression.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>Entity or null if not found.</returns>
	Task<TEntity?> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets the first entity matching the predicate.
	/// </summary>
	/// <param name="predicate">Filter expression.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>Entity or null if not found.</returns>
	Task<TEntity?> GetFirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets all entities matching the predicate.
	/// </summary>
	/// <param name="predicate">Optional filter expression.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>List of entities.</returns>
	Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets a paged list of entities.
	/// </summary>
	/// <param name="skip">Number of entities to skip.</param>
	/// <param name="take">Number of entities to take.</param>
	/// <param name="predicate">Optional filter expression.</param>
	/// <param name="orderBy">Optional ordering function.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>Paged result with entities and total count.</returns>
	Task<PagedResult<TEntity>> GetPagedAsync(
		int skip,
		int take,
		Expression<Func<TEntity, bool>>? predicate = null,
		Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Checks if any entities match the predicate.
	/// </summary>
	/// <param name="predicate">Filter expression.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>True if any entities match.</returns>
	Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

	/// <summary>
	/// Count entities matching the predicate.
	/// </summary>
	/// <param name="predicate">Optional filter expression.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>Count of entities.</returns>
	Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);

	/// <summary>
	/// Adds a new entity.
	/// </summary>
	/// <param name="entity">Entity to add.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>Added entity.</returns>
	Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

	/// <summary>
	/// Adds multiple entities.
	/// </summary>
	/// <param name="entities">Entities to add.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing entity.
	/// </summary>
	/// <param name="entity">Entity to update.</param>
	/// <returns>Updated entity.</returns>
	TEntity Update(TEntity entity);

	/// <summary>
	/// Updates multiple entities.
	/// </summary>
	/// <param name="entities">Entities to update.</param>
	void UpdateRange(IEnumerable<TEntity> entities);

	/// <summary>
	/// Removes an entity.
	/// </summary>
	/// <param name="entity">Entity to remove.</param>
	void Remove(TEntity entity);

	/// <summary>
	/// Removes multiple entities.
	/// </summary>
	/// <param name="entities">Entities to remove.</param>
	void RemoveRange(IEnumerable<TEntity> entities);

	/// <summary>
	/// Removes an entity by its primary key.
	/// </summary>
	/// <param name="id">Primary key value.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>True if entity was found and removed.</returns>
	Task<bool> RemoveByIdAsync(object id, CancellationToken cancellationToken = default);

	/// <summary>
	/// Saves all changes to the database.
	/// </summary>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>Number of entities saved.</returns>
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
