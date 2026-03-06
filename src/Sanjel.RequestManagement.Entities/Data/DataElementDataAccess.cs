using Microsoft.EntityFrameworkCore;
using Sanjel.RequestManagement.Entities.Entities;

namespace Sanjel.RequestManagement.Entities.Data;

/// <summary>
/// Data access implementation for DataElement entities.
/// </summary>
public class DataElementDataAccess : BaseDataAccess<DataElement>, IDataElementDataAccess
{
	public DataElementDataAccess(RequestManagementDbContext context)
		: base(context)
	{
	}

	public async Task<List<DataElement>> GetByValidationStatusAsync(ValidationEnum validation_status, CancellationToken cancellationToken = default)
	{
		return await this._dbSet
			.Where(e => e.ValidationStatus == validation_status)
			.ToListAsync(cancellationToken);
	}

	public async Task<PagedResult<DataElement>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
	{
		var query = this._dbSet.AsQueryable();

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
