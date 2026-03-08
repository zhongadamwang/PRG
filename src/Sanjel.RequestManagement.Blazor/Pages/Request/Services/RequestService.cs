namespace Sanjel.RequestManagement.Blazor.Pages.Request.Services;

using Sanjel.RequestManagement.Repositories.Data;

/// <summary>
/// Application service for Request page.
/// Acts as the single coordinator between presentation and data access layers.
/// </summary>
public class RequestService
{
	private readonly IRequestRepository _repository;

	/// <summary>
	/// Initializes a new instance of the <see cref="RequestService"/> class.
	/// </summary>
	/// <param name="repository">The repository for data access operations.</param>
	public RequestService(IRequestRepository repository)
	{
		this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
	}
}
