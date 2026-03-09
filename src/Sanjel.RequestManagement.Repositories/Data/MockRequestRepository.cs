using System.Linq.Expressions;
using Sanjel.RequestManagement.Entities.Entities;
using Sanjel.RequestManagement.Repositories.Common;

namespace Sanjel.RequestManagement.Repositories.Data;

/// <summary>
/// Mock implementation of IRequestRepository for development and testing purposes.
/// Provides in-memory data without database dependency.
/// </summary>
public class MockRequestRepository : IRequestRepository
{
	private readonly List<Request> _mockData;

	/// <summary>
	/// Initializes a new instance of the <see cref="MockRequestRepository"/> class.
	/// </summary>
	public MockRequestRepository()
	{
		this._mockData = MockDataGenerator.GenerateRequests(50);
	}

	/// <inheritdoc/>
	public IQueryable<Request> Query()
	{
		return this._mockData.AsQueryable();
	}

	/// <inheritdoc/>
	public Task<Request?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();
		var request = this._mockData.FirstOrDefault(r => r.RequestId == id.ToString());
		return Task.FromResult(request);
	}

	/// <inheritdoc/>
	public Task<Request?> GetSingleAsync(Expression<Func<Request, bool>> predicate, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();
		var request = this._mockData.AsQueryable().FirstOrDefault(predicate);
		return Task.FromResult(request);
	}

	/// <inheritdoc/>
	public Task<Request?> GetFirstAsync(Expression<Func<Request, bool>> predicate, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();
		var request = this._mockData.AsQueryable().FirstOrDefault(predicate);
		return Task.FromResult(request);
	}

	/// <inheritdoc/>
	public Task<List<Request>> GetAllAsync(Expression<Func<Request, bool>>? predicate = null, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();
		var query = this._mockData.AsQueryable();
		if (predicate != null)
		{
			query = query.Where(predicate);
		}
		return Task.FromResult(query.ToList());
	}

	/// <inheritdoc/>
	public Task<PagedResult<Request>> GetPagedAsync(
		int skip,
		int take,
		Expression<Func<Request, bool>>? predicate = null,
		Func<IQueryable<Request>, IOrderedQueryable<Request>>? orderBy = null,
		CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();
		var query = this._mockData.AsQueryable();
		if (predicate != null)
		{
			query = query.Where(predicate);
		}

		var totalCount = query.Count();
		var items = query.Skip(skip).Take(take).ToList();

		var result = new PagedResult<Request>
		{
			Items = items,
			TotalCount = totalCount,
			PageNumber = (skip / take) + 1,
			PageSize = take,
		};

		return Task.FromResult(result);
	}

	/// <inheritdoc/>
	public Task<bool> AnyAsync(Expression<Func<Request, bool>> predicate, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();
		var any = this._mockData.AsQueryable().Any(predicate);
		return Task.FromResult(any);
	}

	/// <inheritdoc/>
	public Task<int> CountAsync(Expression<Func<Request, bool>>? predicate = null, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();
		var count = predicate == null ? this._mockData.Count : this._mockData.AsQueryable().Count(predicate);
		return Task.FromResult(count);
	}

	/// <inheritdoc/>
	public Task<Request> AddAsync(Request entity, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();
		this._mockData.Add(entity);
		return Task.FromResult(entity);
	}

	/// <inheritdoc/>
	public Task AddRangeAsync(IEnumerable<Request> entities, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();
		this._mockData.AddRange(entities);
		return Task.CompletedTask;
	}

	/// <inheritdoc/>
	public Request Update(Request entity)
	{
		var existing = this._mockData.FirstOrDefault(r => r.RequestId == entity.RequestId);
		if (existing != null)
		{
			var index = this._mockData.IndexOf(existing);
			this._mockData[index] = entity;
		}
		return entity;
	}

	/// <inheritdoc/>
	public void UpdateRange(IEnumerable<Request> entities)
	{
		foreach (var entity in entities)
		{
			this.Update(entity);
		}
	}

	/// <inheritdoc/>
	public void Remove(Request entity)
	{
		this._mockData.Remove(entity);
	}

	/// <inheritdoc/>
	public void RemoveRange(IEnumerable<Request> entities)
	{
		foreach (var entity in entities)
		{
			this._mockData.Remove(entity);
		}
	}

	/// <inheritdoc/>
	public Task<bool> RemoveByIdAsync(object id, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();
		var entity = this._mockData.FirstOrDefault(r => r.RequestId == id.ToString());
		if (entity == null)
		{
			return Task.FromResult(false);
		}
		this._mockData.Remove(entity);
		return Task.FromResult(true);
	}

	/// <inheritdoc/>
	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();
		return Task.FromResult(0);
	}

	// IRequestRepository-specific methods

	/// <inheritdoc/>
	public Task<List<Request>> GetByStatusAsync(StatusEnum status, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();
		var requests = this._mockData.Where(r => r.Status == status).ToList();
		return Task.FromResult(requests);
	}

	/// <inheritdoc/>
	public Task<List<Request>> GetByCreatedDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();
		var requests = this._mockData.Where(r => r.CreatedDate >= startDate && r.CreatedDate <= endDate).ToList();
		return Task.FromResult(requests);
	}

	/// <inheritdoc/>
	public Task<List<Request>> GetByAcknowledgmentDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();
		var requests = this._mockData.Where(r => r.AcknowledgmentDate >= startDate && r.AcknowledgmentDate <= endDate).ToList();
		return Task.FromResult(requests);
	}

	/// <inheritdoc/>
	public Task<List<Request>> GetByCompletionDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();
		var requests = this._mockData.Where(r => r.CompletionDate >= startDate && r.CompletionDate <= endDate).ToList();
		return Task.FromResult(requests);
	}

	/// <inheritdoc/>
	public Task<List<Request>> GetByReviewPackageIdAsync(int reviewpackageId, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();
		// Mock implementation - filter by ReviewPackage navigation property
		var requests = this._mockData.Where(r => r.ReviewPackage != null).ToList();
		return Task.FromResult(requests);
	}

	/// <inheritdoc/>
	public Task<List<Request>> GetByDataElementIdAsync(int dataelementId, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();
		// Mock implementation - filter by DataElement navigation property
		var requests = this._mockData.Where(r => r.DataElement.Any(d => true)).ToList();
		return Task.FromResult(requests);
	}

	/// <inheritdoc/>
	public Task<List<Request>> GetByNotificationIdAsync(int notificationId, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();
		// Mock implementation - filter by Notification navigation property
		var requests = this._mockData.Where(r => r.Notification != null).ToList();
		return Task.FromResult(requests);
	}

	/// <inheritdoc/>
	public Task<PagedResult<Request>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
	{
		var skip = (pageNumber - 1) * pageSize;
		return this.GetPagedAsync(skip, pageSize, null, null, cancellationToken);
	}
}
