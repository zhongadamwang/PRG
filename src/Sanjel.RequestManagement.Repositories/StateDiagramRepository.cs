using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Sanjel.RequestManagement.Core.Data;
using Sanjel.RequestManagement.Core.Entities;
using Sanjel.RequestManagement.Repositories.Common;

namespace Sanjel.RequestManagement.Repositories;

/// <summary>
/// Repository implementation for StateDiagram entities using EF Core.
/// </summary>
public class StateDiagramRepository : BaseRepository<StateDiagram>, IStateDiagramRepository
{
	public StateDiagramRepository(RequestManagementDbContext context)
		: base(context)
	{
	}

	// Implement entity-specific methods
	public async Task<List<StateDiagram>> GetByImportDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
	{
		return await this._dbSet
			.Where(e => e.ImportDate >= startDate && e.ImportDate <= endDate)
			.ToListAsync(cancellationToken);
	}

	public async Task<List<StateDiagram>> SearchByDiagramNameAsync(string searchText, CancellationToken cancellationToken = default)
	{
		return await this._dbSet
			.Where(e => e.DiagramName.Contains(searchText))
			.ToListAsync(cancellationToken);
	}

	public async Task<List<StateDiagram>> GetByDataElementIdAsync(int dataelementId, CancellationToken cancellationToken = default)
	{
		// No direct relationship between StateDiagram and DataElement
		// Return empty list as this relationship does not exist in the domain model
		return new List<StateDiagram>();
	}

	public async Task<List<StateDiagram>> GetByRequestIdAsync(int requestId, CancellationToken cancellationToken = default)
	{
		// No direct relationship between StateDiagram and Request
		// Return empty list as this relationship does not exist in the domain model
		return new List<StateDiagram>();
	}

	public async Task<PagedResult<StateDiagram>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<StateDiagram, bool>>? filter = null, CancellationToken cancellationToken = default)
	{
		var query = this._dbSet.AsQueryable();

		if (filter != null)
		{
			query = query.Where(filter);
		}

		var totalCount = await query.CountAsync(cancellationToken);
		var skip = (pageNumber - 1) * pageSize;

		var items = await query
			.OrderBy(e => e.ImportDate) // Default ordering
			.Skip(skip)
			.Take(pageSize)
			.ToListAsync(cancellationToken);

		return new PagedResult<StateDiagram>
		{
			Items = items,
			TotalCount = totalCount,
			PageNumber = pageNumber,
			PageSize = pageSize,
		};
	}
}
