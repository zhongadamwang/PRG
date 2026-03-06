using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Sanjel.RequestManagement.Entities.Data;

/// <summary>
/// Base implementation for data access operations.
/// </summary>
/// <typeparam name="TEntity">Entity type.</typeparam>
#pragma warning disable SA1401
public abstract class BaseDataAccess<TEntity> : IDataAccess<TEntity>
	where TEntity : class
{
	protected readonly RequestManagementDbContext _context;
	protected readonly DbSet<TEntity> _dbSet;
#pragma warning restore SA1401

	protected BaseDataAccess(RequestManagementDbContext context)
	{
		this._context = context;
		this._dbSet = context.Set<TEntity>();
	}

	public virtual async Task<PagedResult<TEntity>> GetPagedListAsync(
		int pageNumber,
		int pageSize,
		Expression<Func<TEntity, bool>>? filter = null,
		CancellationToken cancellationToken = default)
	{
		var query = this._dbSet.AsQueryable();

		if (filter != null)
		{
			query = query.Where(filter);
		}

		var totalCount = await query.CountAsync(cancellationToken);
		var skip = (pageNumber - 1) * pageSize;

		var items = await query
			.Skip(skip)
			.Take(pageSize)
			.ToListAsync(cancellationToken);

		return new PagedResult<TEntity>
		{
			Items = items,
			TotalCount = totalCount,
			PageNumber = pageNumber,
			PageSize = pageSize,
		};
	}

	public virtual async Task<List<TEntity>> GetListAsync(
		Expression<Func<TEntity, bool>>? predicate = null,
		CancellationToken cancellationToken = default)
	{
		var query = this._dbSet.AsQueryable();

		if (predicate != null)
		{
			query = query.Where(predicate);
		}

		return await query.ToListAsync(cancellationToken);
	}

	public virtual async Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
	{
		return await this._dbSet.FindAsync(new[] { id }, cancellationToken);
	}

	public virtual async Task<TEntity?> GetByIdWithChildrenAsync(object id, CancellationToken cancellationToken = default)
	{
		var entity = await this.GetByIdAsync(id, cancellationToken);

		if (entity != null)
		{
			// Load navigation properties based on EF Core conventions
			foreach (var navigation in this._context.Entry(entity).Navigations)
			{
				await navigation.LoadAsync(cancellationToken);
			}
		}

		return entity;
	}

	public virtual async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		var entry = await this._dbSet.AddAsync(entity, cancellationToken);
		await this._context.SaveChangesAsync(cancellationToken);
		return entry.Entity;
	}

	public virtual async Task<TEntity> CreateWithChildrenAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		var entry = await this._dbSet.AddAsync(entity, cancellationToken);
		await this._context.SaveChangesAsync(cancellationToken);

		// Reload to get all navigation properties
		var keyValues = this._context.Entry(entity).Properties
			.Where(p => p.Metadata.IsPrimaryKey())
			.Select(p => p.CurrentValue)
			.ToArray();

		return await this.GetByIdWithChildrenAsync(keyValues[0]!, cancellationToken) ?? entity;
	}

	public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		this._dbSet.Update(entity);
		await this._context.SaveChangesAsync(cancellationToken);
		return entity;
	}

	public virtual async Task<TEntity> UpdateWithChildrenAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		this._dbSet.Update(entity);
		await this._context.SaveChangesAsync(cancellationToken);

		// Reload to get all navigation properties
		var keyValues = this._context.Entry(entity).Properties
			.Where(p => p.Metadata.IsPrimaryKey())
			.Select(p => p.CurrentValue)
			.ToArray();

		return await this.GetByIdWithChildrenAsync(keyValues[0]!, cancellationToken) ?? entity;
	}

	public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		this._dbSet.Remove(entity);
		await this._context.SaveChangesAsync(cancellationToken);
	}

	public virtual async Task DeleteWithChildrenAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		// Load all navigation properties to cascade delete
		var entry = this._context.Entry(entity);
		foreach (var navigation in entry.Navigations)
		{
			if (navigation.CurrentValue != null)
			{
				await navigation.LoadAsync(cancellationToken);
			}
		}

		this._dbSet.Remove(entity);
		await this._context.SaveChangesAsync(cancellationToken);
	}

	public virtual async Task<List<TEntity>> GetListByIdsAsync(IEnumerable<object> ids, CancellationToken cancellationToken = default)
	{
		var idList = ids.ToList();
		if (!idList.Any())
		{
			return new List<TEntity>();
		}

		var entity = await this._dbSet.FindAsync(new[] { idList[0] }, cancellationToken);
		if (entity == null)
		{
			return new List<TEntity>();
		}

		var primaryKey = this._context.Model.FindEntityType(typeof(TEntity))?.FindPrimaryKey();
		if (primaryKey == null)
		{
			return new List<TEntity>();
		}

		var propertyName = primaryKey.Properties[0].Name;
		var parameter = Expression.Parameter(typeof(TEntity), "e");
		var property = Expression.Property(parameter, propertyName);
		var constant = Expression.Constant(idList);
		var containsMethod = typeof(Enumerable).GetMethod("Contains", new[] { typeof(IEnumerable<object>) })!;
		var contains = Expression.Call(constant, containsMethod, Expression.Convert(property, typeof(object)));
		var predicate = Expression.Lambda<Func<TEntity, bool>>(contains, parameter);

		return await this._dbSet.Where(predicate).ToListAsync(cancellationToken);
	}

	public virtual async Task<List<TEntity>> GetListWithChildrenByIdsAsync(IEnumerable<object> ids, CancellationToken cancellationToken = default)
	{
		var entities = await this.GetListByIdsAsync(ids, cancellationToken);

		foreach (var entity in entities)
		{
			foreach (var navigation in this._context.Entry(entity).Navigations)
			{
				await navigation.LoadAsync(cancellationToken);
			}
		}

		return entities;
	}
}
