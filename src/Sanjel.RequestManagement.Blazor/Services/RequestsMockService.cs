using Sanjel.RequestManagement.Entities.Entities;

namespace Sanjel.RequestManagement.Blazor.Services;

/// <summary>
/// Mock service for Request data - used for demo purposes
/// </summary>
public interface IRequestsMockService
{
	Task<List<Request>> GetAllRequestsAsync();
	Task<Request?> GetRequestByIdAsync(string requestId);
	Task<Request> CreateRequestAsync(Request request);
	Task<Request> UpdateRequestAsync(Request request);
	Task<bool> DeleteRequestAsync(string requestId);
}

/// <summary>
/// Mock service implementation for Request data
/// </summary>
public class RequestsMockService : IRequestsMockService
{
	private static List<Request> _requests = new();

	static RequestsMockService()
	{
		// Initialize with mock data
		_requests = new List<Request>
		{
			new Request
			{
				RequestId = "REQ-001",
				Status = StatusEnum.InProgress,
				CreatedDate = DateTime.Now.AddDays(-10),
				Priority = PriorityEnum.High,
				ClientId = "CLIENT-001",
				SourceEmail = "client1@example.com",
				AssignedEngineerId = "ENG-001",
				AssignedBy = "MANAGER-001",
				AcknowledgmentDate = DateTime.Now.AddDays(-8),
				CompletionDate = default,
			},
			new Request
			{
				RequestId = "REQ-002",
				Status = StatusEnum.UnderReview,
				CreatedDate = DateTime.Now.AddDays(-5),
				Priority = PriorityEnum.Normal,
				ClientId = "CLIENT-002",
				SourceEmail = "client2@example.com",
				AssignedEngineerId = "ENG-002",
				AssignedBy = "MANAGER-001",
				AcknowledgmentDate = DateTime.Now.AddDays(-4),
				CompletionDate = default,
			},
			new Request
			{
				RequestId = "REQ-003",
				Status = StatusEnum.Completed,
				CreatedDate = DateTime.Now.AddDays(-15),
				Priority = PriorityEnum.Critical,
				ClientId = "CLIENT-003",
				SourceEmail = "client3@example.com",
				AssignedEngineerId = "ENG-003",
				AssignedBy = "MANAGER-002",
				AcknowledgmentDate = DateTime.Now.AddDays(-14),
				CompletionDate = DateTime.Now.AddDays(-1),
			},
			new Request
			{
				RequestId = "REQ-004",
				Status = StatusEnum.Draft,
				CreatedDate = DateTime.Now.AddDays(-2),
				Priority = PriorityEnum.Low,
				ClientId = "CLIENT-004",
				SourceEmail = "client4@example.com",
				AssignedEngineerId = "ENG-001",
				AssignedBy = "MANAGER-001",
				AcknowledgmentDate = default,
				CompletionDate = default,
			},
			new Request
			{
				RequestId = "REQ-005",
				Status = StatusEnum.Approved,
				CreatedDate = DateTime.Now.AddDays(-7),
				Priority = PriorityEnum.High,
				ClientId = "CLIENT-005",
				SourceEmail = "client5@example.com",
				AssignedEngineerId = "ENG-002",
				AssignedBy = "MANAGER-002",
				AcknowledgmentDate = DateTime.Now.AddDays(-6),
				CompletionDate = default,
			},
		};
	}

	public Task<List<Request>> GetAllRequestsAsync()
	{
		return Task.FromResult(_requests.ToList());
	}

	public Task<Request?> GetRequestByIdAsync(string requestId)
	{
		var request = _requests.FirstOrDefault(r => r.RequestId == requestId);
		return Task.FromResult(request);
	}

	public Task<Request> CreateRequestAsync(Request request)
	{
		// Generate new ID if not provided
		if (string.IsNullOrEmpty(request.RequestId))
		{
			var maxId = _requests
				.Where(r => r.RequestId.StartsWith("REQ-"))
				.Select(static r => int.Parse(r.RequestId[4..]))
				.DefaultIfEmpty(0)
				.Max();
			request.RequestId = $"REQ-{maxId + 1:D3}";
		}

		request.CreatedDate = DateTime.Now;
		_requests.Add(request);
		return Task.FromResult(request);
	}

	public Task<Request> UpdateRequestAsync(Request request)
	{
		var existingIndex = _requests.FindIndex(r => r.RequestId == request.RequestId);
		if (existingIndex >= 0)
		{
			_requests[existingIndex] = request;
		}
		return Task.FromResult(request);
	}

	public Task<bool> DeleteRequestAsync(string requestId)
	{
		var request = _requests.FirstOrDefault(r => r.RequestId == requestId);
		if (request != null)
		{
			_requests.Remove(request);
			return Task.FromResult(true);
		}
		return Task.FromResult(false);
	}
}
