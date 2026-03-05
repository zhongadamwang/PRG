using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Sanjel.RequestManagement.Repositories.Common;

/// <summary>
/// Base repository implementation using EF Core.
/// </summary>
/// <typeparam name="TEntity">Entity type.</typeparam>
public class BaseRepository<TEntity> : IRepository<TEntity>
	where TEntity : class
{
#pragma warning disable SA1401
	protected readonly Sanjel.RequestManagement.Entities.Data.RequestManagementDbContext _context;
	protected readonly DbSet<TEntity> _dbSet;
#pragma warning restore SA1401

	public BaseRepository(Sanjel.RequestManagement.Entities.Data.RequestManagementDbContext context)
	{
		this._context = context;
		this._dbSet = context.Set<TEntity>();
	}

	public virtual IQueryable<TEntity> Query()
	{
		return this._dbSet.AsQueryable();
	}

	public virtual async Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
	{
		return await this._dbSet.FindAsync(new object[] { id }, cancellationToken);
	}

	public virtual async Task<TEntity?> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
	{
		return await this._dbSet.SingleOrDefaultAsync(predicate, cancellationToken);
	}

	public virtual async Task<TEntity?> GetFirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
	{
		return await this._dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
	}

	public virtual async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
	{
		IQueryable<TEntity> query = this._dbSet;

		if (predicate != null)
		{
			query = query.Where(predicate);
		}

		return await query.ToListAsync(cancellationToken);
	}

	public virtual async Task<PagedResult<TEntity>> GetPagedAsync(
		int skip,
		int take,
		Expression<Func<TEntity, bool>>? predicate = null,
		Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
		CancellationToken cancellationToken = default)
	{
		IQueryable<TEntity> query = this._dbSet;

		if (predicate != null)
		{
			query = query.Where(predicate);
		}

		var totalCount = await query.CountAsync(cancellationToken);

		if (orderBy != null)
		{
			query = orderBy(query);
		}

		var items = await query
			.Skip(skip)
			.Take(take)
			.ToListAsync(cancellationToken);

		return new PagedResult<TEntity>
		{
			Items = items,
			TotalCount = totalCount,
			PageNumber = (skip / take) + 1,
			PageSize = take,
		};
	}

	public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
	{
		return await this._dbSet.AnyAsync(predicate, cancellationToken);
	}

	public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
	{
		IQueryable<TEntity> query = this._dbSet;

		if (predicate != null)
		{
			query = query.Where(predicate);
		}

		return await query.CountAsync(cancellationToken);
	}

	public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		var entry = await this._dbSet.AddAsync(entity, cancellationToken);
		return entry.Entity;
	}

	public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
	{
		await this._dbSet.AddRangeAsync(entities, cancellationToken);
	}

	public virtual TEntity Update(TEntity entity)
	{
		var entry = this._dbSet.Update(entity);
		return entry.Entity;
	}

	public virtual void UpdateRange(IEnumerable<TEntity> entities)
	{
		this._dbSet.UpdateRange(entities);
	}

	public virtual void Remove(TEntity entity)
	{
		this._dbSet.Remove(entity);
	}

	public virtual void RemoveRange(IEnumerable<TEntity> entities)
	{
		this._dbSet.RemoveRange(entities);
	}

	public virtual async Task<bool> RemoveByIdAsync(object id, CancellationToken cancellationToken = default)
	{
		var entity = await this.GetByIdAsync(id, cancellationToken);
		if (entity != null)
		{
			this.Remove(entity);
			return true;
		}

		return false;
	}

	public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return await this._context.SaveChangesAsync(cancellationToken);
	}
}
