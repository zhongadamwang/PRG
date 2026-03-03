using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Sanjel.RequestManagement.Core.Data;
using Sanjel.RequestManagement.Core.Entities;
using Sanjel.RequestManagement.Repositories.Common;

namespace Sanjel.RequestManagement.Repositories;

/// <summary>
/// Repository implementation for DataElement entities using EF Core.
/// </summary>
public class DataElementRepository : BaseRepository<DataElement>, IDataElementRepository
{
	public DataElementRepository(RequestManagementDbContext context)
		: base(context)
	{
	}

	// Implement entity-specific methods
	public async Task<List<DataElement>> GetByValidationStatusAsync(ValidationEnum validation_status, CancellationToken cancellationToken = default)
	{
		return await this._dbSet
			.Where(e => e.ValidationStatus == validation_status)
			.ToListAsync(cancellationToken);
	}

	public async Task<PagedResult<DataElement>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<DataElement, bool>>? filter = null, CancellationToken cancellationToken = default)
	{
		var query = this._dbSet.AsQueryable();

		if (filter != null)
		{
			query = query.Where(filter);
		}

		var totalCount = await query.CountAsync(cancellationToken);
		var skip = (pageNumber - 1) * pageSize;

		var items = await query
			.OrderBy(e => e.ElementId) // Default ordering
			.Skip(skip)
			.Take(pageSize)
			.ToListAsync(cancellationToken);

		return new PagedResult<DataElement>
		{
			Items = items,
			TotalCount = totalCount,
			PageNumber = pageNumber,
			PageSize = pageSize,
		};
	}
}
