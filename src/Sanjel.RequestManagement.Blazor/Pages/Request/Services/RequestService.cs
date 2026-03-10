using System.Linq.Expressions;
using Sanjel.RequestManagement.Blazor.Pages.Request.ViewModels;
using Sanjel.RequestManagement.Entities.Entities;
using Sanjel.RequestManagement.Repositories.Common;
using Sanjel.RequestManagement.Repositories.Data;
using RequestEntity = Sanjel.RequestManagement.Entities.Entities.Request;

namespace Sanjel.RequestManagement.Blazor.Pages.Request.Services;

/// <summary>
/// Application service for Request page.
/// Acts as the single coordinator between presentation and data access layers.
/// </summary>
public class RequestService
{
	private readonly IRequestRepository _repository;
	private readonly ILogger<RequestService> _logger;

	/// <summary>
	/// Initializes a new instance of the <see cref="RequestService"/> class.
	/// </summary>
	/// <param name="repository">The repository for data access operations.</param>
	/// <param name="logger">The logger instance.</param>
	public RequestService(IRequestRepository repository, ILogger<RequestService> logger)
	{
		this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
		this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	/// <summary>
	/// Gets a paginated and filtered list of requests based on the view model criteria.
	/// </summary>
	/// <param name="viewModel">The view model containing filter and pagination criteria.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>Paged result of requests.</returns>
	public async Task<PagedResult<RequestEntity>> GetPagedListAsync(RequestListViewModel viewModel, CancellationToken cancellationToken = default)
	{
		this._logger.LogInformation(
			"Getting paged list of requests - Page: {CurrentPage}, PageSize: {PageSize}, SearchTerm: '{SearchTerm}'",
			viewModel.CurrentPage,
			viewModel.PageSize,
			viewModel.SearchTerm);

		var predicate = this.BuildFilterPredicate(viewModel);

		var skip = (viewModel.CurrentPage - 1) * viewModel.PageSize;
		var take = viewModel.PageSize;

		this._logger.LogDebug("Query parameters - Skip: {Skip}, Take: {Take}", skip, take);

		Func<IQueryable<RequestEntity>, IOrderedQueryable<RequestEntity>>? orderBy = null;
		if (!string.IsNullOrWhiteSpace(viewModel.SortColumn))
		{
			orderBy = this.BuildSortOrder(viewModel.SortColumn, viewModel.SortDirection);
			this._logger.LogDebug(
				"Sorting by column: {SortColumn}, Direction: {SortDirection}",
				viewModel.SortColumn, viewModel.SortDirection);
		}

		try
		{
			var result = await this._repository.GetPagedAsync(skip, take, predicate, orderBy, cancellationToken);
			this._logger.LogInformation(
				"Successfully retrieved {ItemCount} requests out of {TotalCount} total. Page {CurrentPage} of {TotalPages}",
				result.Items.Count, result.TotalCount, viewModel.CurrentPage,
				(int)Math.Ceiling((double)result.TotalCount / viewModel.PageSize));
			return result;
		}
		catch (Exception ex)
		{
			this._logger.LogError(ex, "Error retrieving paged list of requests");
			throw;
		}
	}

	/// <summary>
	/// Gets a request by ID.
	/// </summary>
	/// <param name="requestId">The request ID.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>The request or null if not found.</returns>
	public async Task<RequestEntity?> GetByIdAsync(string requestId, CancellationToken cancellationToken = default)
	{
		return await this._repository.GetByIdAsync(requestId, cancellationToken);
	}

	/// <summary>
	/// Creates a new request.
	/// </summary>
	/// <param name="request">The request to create.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>The created request.</returns>
	public async Task<RequestEntity> CreateAsync(RequestEntity request, CancellationToken cancellationToken = default)
	{
		return await this._repository.AddAsync(request, cancellationToken);
	}

	/// <summary>
	/// Updates an existing request.
	/// </summary>
	/// <param name="request">The request to update.</param>
	/// <returns>The updated request.</returns>
	public RequestEntity Update(RequestEntity request)
	{
		return this._repository.Update(request);
	}

	/// <summary>
	/// Updates an existing request asynchronously.
	/// </summary>
	/// <param name="request">The request to update.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>The updated request.</returns>
	public Task<RequestEntity> UpdateAsync(RequestEntity request, CancellationToken cancellationToken = default)
	{
		this._logger.LogInformation("Updating request: {RequestId}", request.RequestId);
		try
		{
			var updated = this._repository.Update(request);
			this._logger.LogInformation("Successfully updated request: {RequestId}", request.RequestId);
			return Task.FromResult(updated);
		}
		catch (Exception ex)
		{
			this._logger.LogError(ex, "Error updating request: {RequestId}", request.RequestId);
			throw;
		}
	}

	/// <summary>
	/// Deletes a request.
	/// </summary>
	/// <param name="requestId">The request ID.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>True if the request was deleted.</returns>
	public async Task<bool> DeleteAsync(string requestId, CancellationToken cancellationToken = default)
	{
		return await this._repository.RemoveByIdAsync(requestId, cancellationToken);
	}

	/// <summary>
	/// Batch deletes multiple requests.
	/// </summary>
	/// <param name="requestIds">The list of request IDs to delete.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>Number of requests deleted.</returns>
	public async Task<int> BatchDeleteAsync(IEnumerable<string> requestIds, CancellationToken cancellationToken = default)
	{
		var requests = await this._repository.GetAllAsync(r => requestIds.Contains(r.RequestId), cancellationToken);
		this._repository.RemoveRange(requests);
		return await this._repository.SaveChangesAsync(cancellationToken);
	}

	/// <summary>
	/// Updates the status of a request.
	/// </summary>
	/// <param name="requestId">The request ID.</param>
	/// <param name="newStatus">The new status.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>True if updated successfully.</returns>
	public async Task<bool> UpdateStatusAsync(string requestId, StatusEnum newStatus, CancellationToken cancellationToken = default)
	{
		var request = await this._repository.GetByIdAsync(requestId, cancellationToken);
		if (request == null)
		{
			return false;
		}

		request.Status = newStatus;
		this._repository.Update(request);
		await this._repository.SaveChangesAsync(cancellationToken);
		return true;
	}

	/// <summary>
	/// Updates the priority of a request.
	/// </summary>
	/// <param name="requestId">The request ID.</param>
	/// <param name="newPriority">The new priority.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>True if updated successfully.</returns>
	public async Task<bool> UpdatePriorityAsync(string requestId, PriorityEnum newPriority, CancellationToken cancellationToken = default)
	{
		var request = await this._repository.GetByIdAsync(requestId, cancellationToken);
		if (request == null)
		{
			return false;
		}

		request.Priority = newPriority;
		this._repository.Update(request);
		await this._repository.SaveChangesAsync(cancellationToken);
		return true;
	}

	/// <summary>
	/// Builds a filter predicate based on the view model criteria.
	/// </summary>
	/// <param name="viewModel">The view model containing filter criteria.</param>
	/// <returns>The filter expression.</returns>
	private Expression<Func<RequestEntity, bool>>? BuildFilterPredicate(RequestListViewModel viewModel)
	{
		Expression<Func<RequestEntity, bool>>? predicate = null;

		if (!string.IsNullOrWhiteSpace(viewModel.SearchTerm))
		{
			var searchTerm = viewModel.SearchTerm.ToLower();
			predicate = r =>
				(r.RequestId != null && r.RequestId.ToLower().Contains(searchTerm)) ||
				(r.ClientId != null && r.ClientId.ToLower().Contains(searchTerm)) ||
				(r.SourceEmail != null && r.SourceEmail.ToLower().Contains(searchTerm)) ||
				(r.AssignedEngineerId != null && r.AssignedEngineerId.ToLower().Contains(searchTerm)) ||
				(r.AssignedBy != null && r.AssignedBy.ToLower().Contains(searchTerm));
		}

		if (!string.IsNullOrWhiteSpace(viewModel.RequestIdFilter))
		{
			var requestIdFilter = viewModel.RequestIdFilter;
			predicate = this.And(predicate, r => r.RequestId != null && r.RequestId.Contains(requestIdFilter));
		}

		if (viewModel.StatusFilter.HasValue)
		{
			var statusFilter = viewModel.StatusFilter.Value;
			predicate = this.And(predicate, r => r.Status == statusFilter);
		}

		if (viewModel.PriorityFilter.HasValue)
		{
			var priorityFilter = viewModel.PriorityFilter.Value;
			predicate = this.And(predicate, r => r.Priority == priorityFilter);
		}

		if (!string.IsNullOrWhiteSpace(viewModel.ClientIdFilter))
		{
			var clientIdFilter = viewModel.ClientIdFilter;
			predicate = this.And(predicate, r => r.ClientId != null && r.ClientId.Contains(clientIdFilter));
		}

		if (!string.IsNullOrWhiteSpace(viewModel.SourceEmailFilter))
		{
			var sourceEmailFilter = viewModel.SourceEmailFilter;
			predicate = this.And(predicate, r => r.SourceEmail != null && r.SourceEmail.Contains(sourceEmailFilter));
		}

		if (!string.IsNullOrWhiteSpace(viewModel.AssignedEngineerIdFilter))
		{
			var assignedEngineerIdFilter = viewModel.AssignedEngineerIdFilter;
			predicate = this.And(predicate, r => r.AssignedEngineerId != null && r.AssignedEngineerId.Contains(assignedEngineerIdFilter));
		}

		if (!string.IsNullOrWhiteSpace(viewModel.AssignedByFilter))
		{
			var assignedByFilter = viewModel.AssignedByFilter;
			predicate = this.And(predicate, r => r.AssignedBy != null && r.AssignedBy.Contains(assignedByFilter));
		}

		if (viewModel.CreatedDateStartFilter.HasValue)
		{
			var startDate = viewModel.CreatedDateStartFilter.Value;
			predicate = this.And(predicate, r => r.CreatedDate >= startDate);
		}

		if (viewModel.CreatedDateEndFilter.HasValue)
		{
			var endDate = viewModel.CreatedDateEndFilter.Value;
			predicate = this.And(predicate, r => r.CreatedDate <= endDate);
		}

		if (viewModel.AcknowledgmentDateStartFilter.HasValue)
		{
			var startDate = viewModel.AcknowledgmentDateStartFilter.Value;
			predicate = this.And(predicate, r => r.AcknowledgmentDate >= startDate);
		}

		if (viewModel.AcknowledgmentDateEndFilter.HasValue)
		{
			var endDate = viewModel.AcknowledgmentDateEndFilter.Value;
			predicate = this.And(predicate, r => r.AcknowledgmentDate <= endDate);
		}

		if (viewModel.CompletionDateStartFilter.HasValue)
		{
			var startDate = viewModel.CompletionDateStartFilter.Value;
			predicate = this.And(predicate, r => r.CompletionDate >= startDate);
		}

		if (viewModel.CompletionDateEndFilter.HasValue)
		{
			var endDate = viewModel.CompletionDateEndFilter.Value;
			predicate = this.And(predicate, r => r.CompletionDate <= endDate);
		}

		return predicate;
	}

	/// <summary>
	/// Combines two predicates with AND logic.
	/// </summary>
	/// <typeparam name="T">The entity type.</typeparam>
	/// <param name="first">The first predicate.</param>
	/// <param name="second">The second predicate.</param>
	/// <returns>The combined predicate.</returns>
	private Expression<Func<T, bool>>? And<T>(Expression<Func<T, bool>>? first, Expression<Func<T, bool>> second)
	{
		if (first == null)
		{
			return second;
		}

		var parameter = Expression.Parameter(typeof(T));

		var leftVisitor = new ReplaceExpressionVisitor(first.Parameters[0], parameter);
		var left = leftVisitor.Visit(first.Body);

		var rightVisitor = new ReplaceExpressionVisitor(second.Parameters[0], parameter);
		var right = rightVisitor.Visit(second.Body);

		return Expression.Lambda<Func<T, bool>>(
			Expression.AndAlso(left, right),
			parameter);
	}

	/// <summary>
	/// Builds a sort order based on the column and direction.
	/// </summary>
	/// <param name="sortColumn">The column to sort by.</param>
	/// <param name="sortDirection">The sort direction.</param>
	/// <returns>The sort order function.</returns>
	private Func<IQueryable<RequestEntity>, IOrderedQueryable<RequestEntity>>? BuildSortOrder(string sortColumn, string sortDirection)
	{
		bool isAscending = sortDirection.Equals("ASC", StringComparison.OrdinalIgnoreCase);

		return sortColumn.ToLower() switch
		{
			"requestid" => q => isAscending ? q.OrderBy(r => r.RequestId) : q.OrderByDescending(r => r.RequestId),
			"status" => q => isAscending ? q.OrderBy(r => r.Status) : q.OrderByDescending(r => r.Status),
			"createddate" => q => isAscending ? q.OrderBy(r => r.CreatedDate) : q.OrderByDescending(r => r.CreatedDate),
			"priority" => q => isAscending ? q.OrderBy(r => r.Priority) : q.OrderByDescending(r => r.Priority),
			"clientid" => q => isAscending ? q.OrderBy(r => r.ClientId) : q.OrderByDescending(r => r.ClientId),
			"sourceemail" => q => isAscending ? q.OrderBy(r => r.SourceEmail) : q.OrderByDescending(r => r.SourceEmail),
			"assignedengineerid" => q => isAscending ? q.OrderBy(r => r.AssignedEngineerId) : q.OrderByDescending(r => r.AssignedEngineerId),
			"assignedby" => q => isAscending ? q.OrderBy(r => r.AssignedBy) : q.OrderByDescending(r => r.AssignedBy),
			"acknowledgmentdate" => q => isAscending ? q.OrderBy(r => r.AcknowledgmentDate) : q.OrderByDescending(r => r.AcknowledgmentDate),
			"completiondate" => q => isAscending ? q.OrderBy(r => r.CompletionDate) : q.OrderByDescending(r => r.CompletionDate),
			_ => q => q.OrderByDescending(r => r.CreatedDate)
		};
	}

	/// <summary>
	/// Expression visitor for replacing parameters in expressions.
	/// </summary>
	private class ReplaceExpressionVisitor : ExpressionVisitor
	{
		private readonly Expression _oldValue;
		private readonly Expression _newValue;

		public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
		{
			this._oldValue = oldValue;
			this._newValue = newValue;
		}

		protected override Expression VisitParameter(ParameterExpression node)
		{
			return node == this._oldValue ? this._newValue : base.VisitParameter(node);
		}
	}
}
