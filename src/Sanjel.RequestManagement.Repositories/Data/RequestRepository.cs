using System.Linq.Expressions;
using Sanjel.RequestManagement.Entities.Data;
using Sanjel.RequestManagement.Entities.Entities;

namespace Sanjel.RequestManagement.Repositories.Data;

/// <summary>
/// Repository implementation for Request entity providing domain-specific data access operations.
/// Wraps IRequestDataAccess and implements generic IRepository pattern.
/// </summary>
public class RequestRepository : Sanjel.RequestManagement.Repositories.Common.IRepository<Request>, IRequestRepository
{
	private readonly IRequestDataAccess _dataAccess;

	/// <summary>
	/// Initializes a new instance of the <see cref="RequestRepository"/> class.
	/// </summary>
	/// <param name="dataAccess">The data access layer for Request entities.</param>
	public RequestRepository(IRequestDataAccess dataAccess)
	{
		this._dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
	}

	/// <inheritdoc/>
	public IQueryable<Request> Query()
	{
		// Note: IQueryable is not directly available through IDataAccess interface
		// This is a limitation of the current architecture
		throw new NotImplementedException("Query is not supported through the IDataAccess interface.");
	}

	/// <inheritdoc/>
	public Task<Request?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
	{
		return this._dataAccess.GetByIdAsync(id, cancellationToken);
	}

	/// <inheritdoc/>
	public Task<Request?> GetSingleAsync(Expression<Func<Request, bool>> predicate, CancellationToken cancellationToken = default)
	{
		return this._dataAccess.GetListAsync(predicate, cancellationToken).ContinueWith(t => t.Result.FirstOrDefault());
	}

	/// <inheritdoc/>
	public Task<Request?> GetFirstAsync(Expression<Func<Request, bool>> predicate, CancellationToken cancellationToken = default)
	{
		return this._dataAccess.GetListAsync(predicate, cancellationToken).ContinueWith(t => t.Result.FirstOrDefault());
	}

	/// <inheritdoc/>
	public Task<List<Request>> GetAllAsync(Expression<Func<Request, bool>>? predicate = null, CancellationToken cancellationToken = default)
	{
		return this._dataAccess.GetListAsync(predicate, cancellationToken);
	}

	/// <inheritdoc/>
	public Task<Sanjel.RequestManagement.Repositories.Common.PagedResult<Request>> GetPagedAsync(
		int skip,
		int take,
		Expression<Func<Request, bool>>? predicate = null,
		Func<IQueryable<Request>, IOrderedQueryable<Request>>? orderBy = null,
		CancellationToken cancellationToken = default)
	{
		// Note: IDataAccess.GetPagedListAsync uses pageNumber and pageSize, not skip and take
		var pageNumber = (skip / take) + 1;
		return ConvertPagedResultAsync(this._dataAccess.GetPagedListAsync(pageNumber, take, predicate, cancellationToken));
	}

	/// <inheritdoc/>
	public Task<bool> AnyAsync(Expression<Func<Request, bool>> predicate, CancellationToken cancellationToken = default)
	{
		return this._dataAccess.GetListAsync(predicate, cancellationToken).ContinueWith(t => t.Result.Any());
	}

	/// <inheritdoc/>
	public Task<int> CountAsync(Expression<Func<Request, bool>>? predicate = null, CancellationToken cancellationToken = default)
	{
		return this._dataAccess.GetListAsync(predicate, cancellationToken).ContinueWith(t => t.Result.Count);
	}

	/// <inheritdoc/>
	public Task<Request> AddAsync(Request entity, CancellationToken cancellationToken = default)
	{
		return this._dataAccess.CreateAsync(entity, cancellationToken);
	}

	/// <inheritdoc/>
	public Task AddRangeAsync(IEnumerable<Request> entities, CancellationToken cancellationToken = default)
	{
		foreach (var entity in entities)
		{
			this._dataAccess.CreateAsync(entity, cancellationToken);
		}
		return Task.CompletedTask;
	}

	/// <inheritdoc/>
	public Request Update(Request entity)
	{
		// IDataAccess.UpdateAsync is async, but IRepository.Update is synchronous
		// This is a design limitation we work around by fire-and-forget
		Task.Run(() => this._dataAccess.UpdateAsync(entity, CancellationToken.None));
		return entity;
	}

	/// <inheritdoc/>
	public void UpdateRange(IEnumerable<Request> entities)
	{
		foreach (var entity in entities)
		{
			Task.Run(() => this._dataAccess.UpdateAsync(entity, CancellationToken.None));
		}
	}

	/// <inheritdoc/>
	public void Remove(Request entity)
	{
		Task.Run(() => this._dataAccess.DeleteAsync(entity, CancellationToken.None));
	}

	/// <inheritdoc/>
	public void RemoveRange(IEnumerable<Request> entities)
	{
		foreach (var entity in entities)
		{
			Task.Run(() => this._dataAccess.DeleteAsync(entity, CancellationToken.None));
		}
	}

	/// <inheritdoc/>
	public async Task<bool> RemoveByIdAsync(object id, CancellationToken cancellationToken = default)
	{
		var entity = await this.GetByIdAsync(id, cancellationToken);
		if (entity == null)
		{
			return false;
		}
		await this._dataAccess.DeleteAsync(entity, cancellationToken);
		return true;
	}

	/// <inheritdoc/>
	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		// IDataAccess methods handle their own SaveChanges
		// This is a no-op in this adapter pattern
		return Task.FromResult(0);
	}

	// IRequestDataAccess-specific methods

	/// <inheritdoc/>
	public Task<List<Request>> GetByStatusAsync(StatusEnum status, CancellationToken cancellationToken = default)
	{
		return this._dataAccess.GetByStatusAsync(status, cancellationToken);
	}

	/// <inheritdoc/>
	public Task<List<Request>> GetByCreatedDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
	{
		return this._dataAccess.GetByCreatedDateRangeAsync(startDate, endDate, cancellationToken);
	}

	/// <inheritdoc/>
	public Task<List<Request>> GetByAcknowledgmentDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
	{
		return this._dataAccess.GetByAcknowledgmentDateRangeAsync(startDate, endDate, cancellationToken);
	}

	/// <inheritdoc/>
	public Task<List<Request>> GetByCompletionDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
	{
		return this._dataAccess.GetByCompletionDateRangeAsync(startDate, endDate, cancellationToken);
	}

	/// <inheritdoc/>
	public Task<List<Request>> GetByReviewPackageIdAsync(int reviewpackageId, CancellationToken cancellationToken = default)
	{
		return this._dataAccess.GetByReviewPackageIdAsync(reviewpackageId, cancellationToken);
	}

	/// <inheritdoc/>
	public Task<List<Request>> GetByDataElementIdAsync(int dataelementId, CancellationToken cancellationToken = default)
	{
		return this._dataAccess.GetByDataElementIdAsync(dataelementId, cancellationToken);
	}

	/// <inheritdoc/>
	public Task<List<Request>> GetByNotificationIdAsync(int notificationId, CancellationToken cancellationToken = default)
	{
		return this._dataAccess.GetByNotificationIdAsync(notificationId, cancellationToken);
	}

	/// <inheritdoc/>
	public Task<Sanjel.RequestManagement.Repositories.Common.PagedResult<Request>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
	{
		return ConvertPagedResultAsync(this._dataAccess.GetPagedAsync(pageNumber, pageSize, cancellationToken));
	}

	private static Task<Sanjel.RequestManagement.Repositories.Common.PagedResult<Request>> ConvertPagedResultAsync(Task<PagedResult<Request>> dataAccessResult)
	{
		return dataAccessResult.ContinueWith(
			t =>
		{
			var result = t.Result;
			return new Sanjel.RequestManagement.Repositories.Common.PagedResult<Request>
			{
				Items = result.Items,
				TotalCount = result.TotalCount,
				PageNumber = result.PageNumber,
				PageSize = result.PageSize,
			};
		}, TaskScheduler.Default);
	}
}
