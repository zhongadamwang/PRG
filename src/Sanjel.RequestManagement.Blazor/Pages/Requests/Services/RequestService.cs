using Sanjel.RequestManagement.Blazor.Pages.Requests.ViewModels;
using Sanjel.RequestManagement.Entities.Entities;
using Sanjel.RequestManagement.Repositories.Data;

namespace Sanjel.RequestManagement.Blazor.Pages.Requests.Services;

/// <summary>
/// Application service for the Requests page.
/// Acts as the single coordinator between the presentation and data access layers.
/// </summary>
public class RequestService
{
	private readonly IRequestRepository _requestRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="RequestService"/> class.
	/// </summary>
	/// <param name="requestRepository">The repository for Request data access operations.</param>
	public RequestService(IRequestRepository requestRepository)
	{
		this._requestRepository = requestRepository ?? throw new ArgumentNullException(nameof(requestRepository));
	}

	/// <summary>
	/// Creates a new Request entity from the provided ViewModel.
	/// </summary>
	public async Task<Request> CreateAsync(RequestViewModel viewModel, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(viewModel);

		var entity = new Request
		{
			RequestId = viewModel.RequestId,
			ClientId = viewModel.ClientId,
			SourceEmail = viewModel.SourceEmail,
			AssignedEngineerId = viewModel.AssignedEngineerId,
			AssignedBy = viewModel.AssignedBy,
			Status = viewModel.Status,
			Priority = viewModel.Priority,
			CreatedDate = viewModel.CreatedDate,
			AcknowledgmentDate = viewModel.AcknowledgmentDate,
			CompletionDate = viewModel.CompletionDate,
		};

		var created = await this._requestRepository.AddAsync(entity, cancellationToken);
		await this._requestRepository.SaveChangesAsync(cancellationToken);
		return created;
	}

	/// <summary>
	/// Updates an existing Request entity from the provided ViewModel.
	/// </summary>
	public async Task<Request> UpdateAsync(Request existing, RequestViewModel viewModel, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(existing);
		ArgumentNullException.ThrowIfNull(viewModel);

		existing.ClientId = viewModel.ClientId;
		existing.SourceEmail = viewModel.SourceEmail;
		existing.AssignedEngineerId = viewModel.AssignedEngineerId;
		existing.AssignedBy = viewModel.AssignedBy;
		existing.Status = viewModel.Status;
		existing.Priority = viewModel.Priority;
		existing.CreatedDate = viewModel.CreatedDate;
		existing.AcknowledgmentDate = viewModel.AcknowledgmentDate;
		existing.CompletionDate = viewModel.CompletionDate;

		var updated = this._requestRepository.Update(existing);
		await this._requestRepository.SaveChangesAsync(cancellationToken);
		return updated;
	}

	/// <summary>
	/// Deletes a Request entity by its primary key.
	/// </summary>
	/// <param name="requestId">The primary key of the Request to delete.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>True if the entity was found and deleted, false otherwise.</returns>
	public async Task<bool> DeleteAsync(int requestId, CancellationToken cancellationToken = default)
	{
		var deleted = await this._requestRepository.RemoveByIdAsync(requestId, cancellationToken);
		if (deleted)
		{
			await this._requestRepository.SaveChangesAsync(cancellationToken);
		}
		return deleted;
	}

	/// <summary>
	/// Deletes a Request entity.
	/// </summary>
	/// <param name="request">The Request entity to delete.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	public async Task DeleteAsync(Request request, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);

		this._requestRepository.Remove(request);
		await this._requestRepository.SaveChangesAsync(cancellationToken);
	}

	/// <summary>
	/// Deletes multiple Request entities.
	/// </summary>
	/// <param name="requests">The collection of Request entities to delete.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	public async Task DeleteRangeAsync(IEnumerable<Request> requests, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(requests);

		var requestList = requests.ToList();
		if (requestList.Count == 0)
		{
			return;
		}

		this._requestRepository.RemoveRange(requestList);
		await this._requestRepository.SaveChangesAsync(cancellationToken);
	}

	/// <summary>
	/// Validates if a Request can be safely deleted by checking for dependencies and constraints.
	/// </summary>
	/// <param name="requestId">The primary key of the Request to validate.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>A tuple indicating if deletion is allowed and any constraint message.</returns>
	public async Task<(bool CanDelete, string? ConstraintMessage)> ValidateDeleteAsync(string requestId, CancellationToken cancellationToken = default)
	{
		var request = await this._requestRepository.GetByIdAsync(requestId, cancellationToken);
		if (request == null)
		{
			return (false, "Request not found.");
		}

		// Check for business constraints
		if (request.Status == StatusEnum.InProgress)
		{
			return (false, "Cannot delete requests that are currently in progress. Please change status first.");
		}

		if (request.Status == StatusEnum.Completed && request.CompletionDate > DateTime.UtcNow.AddDays(-30))
		{
			return (false, "Cannot delete recently completed requests (within 30 days). This is required for audit compliance.");
		}

		// Additional constraint checks can be added here
		// For example: check for related entities, user permissions, etc.

		return (true, null);
	}
}
