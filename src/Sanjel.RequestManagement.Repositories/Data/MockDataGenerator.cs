using Sanjel.RequestManagement.Entities.Entities;

namespace Sanjel.RequestManagement.Repositories.Data;

/// <summary>
/// Mock data generator for creating realistic test data.
/// </summary>
public static class MockDataGenerator
{
	private static readonly Random Random = new Random();
	private static readonly string[] FirstNames = { "John", "Jane", "Michael", "Sarah", "David", "Emily", "Robert", "Lisa", "William", "Jennifer" };
	private static readonly string[] LastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez" };
	private static readonly string[] Domains = { "example.com", "test.com", "demo.org", "sample.net" };

	/// <summary>
	/// Generates a collection of mock Request entities.
	/// </summary>
	/// <param name="count">Number of requests to generate.</param>
	/// <returns>List of mock Request entities.</returns>
	public static List<Request> GenerateRequests(int count = 50)
	{
		var requests = new List<Request>();
		var statuses = Enum.GetValues(typeof(StatusEnum)).Cast<StatusEnum>().ToList();
		var priorities = Enum.GetValues(typeof(PriorityEnum)).Cast<PriorityEnum>().ToList();

		for (int i = 1; i <= count; i++)
		{
			var status = statuses[Random.Next(statuses.Count)];
			var priority = priorities[Random.Next(priorities.Count)];
			var firstName = FirstNames[Random.Next(FirstNames.Length)];
			var lastName = LastNames[Random.Next(LastNames.Length)];
			var domain = Domains[Random.Next(Domains.Length)];

			var createdDate = DateTime.Now.AddDays(-Random.Next(1, 90));
			var acknowledgmentDate = status >= StatusEnum.Submitted ? createdDate.AddDays(1) : DateTime.MinValue;
			var completionDate = status >= StatusEnum.Completed ? createdDate.AddDays(7) : DateTime.MinValue;

			var stateDiagram = new StateDiagram
			{
				DiagramId = $"DIAG-{i:D4}",
				DiagramName = $"Diagram {i}",
				FilePath = $"/path/to/diagram/{i}.pdf",
				Version = "1.0",
				ImportDate = DateTime.Now,
				ParsingConfidence = 95.0m,
				ClientId = $"{firstName}.{lastName}".ToLower(),
				DiagramType = DiagramTypeEnum.ProcessFlow,
			};

			var request = new Request
			{
				RequestId = $"REQ-{i:D4}",
				Status = status,
				CreatedDate = createdDate,
				Priority = priority,
				ClientId = $"{firstName}.{lastName}".ToLower(),
				SourceEmail = $"{firstName}.{lastName}@{domain}".ToLower(),
				AssignedEngineerId = $"ENG-{Random.Next(1, 20):D3}",
				AssignedBy = $"MGR-{Random.Next(1, 5):D2}",
				AcknowledgmentDate = acknowledgmentDate == DateTime.MinValue ? default : acknowledgmentDate,
				CompletionDate = completionDate == DateTime.MinValue ? default : completionDate,
				ReviewPackage = i % 3 == 0 ? new ReviewPackage
				{
					PackageId = $"PKG-{i:D4}",
					RequestId = $"REQ-{i:D4}",
					SubmittingEngineerId = $"ENG-{Random.Next(1, 20):D3}",
					AssignedReviewerId = $"REV-{Random.Next(1, 10):D3}",
					SubmissionDate = createdDate.AddDays(1),
					ReviewStatus = ReviewStatusEnum.InProgress,
					WorkSummary = $"Work summary for request {i}",
					ReviewFeedback = "Pending review",
				}
				: null,
				DataElement = i % 2 == 0 ? new List<DataElement>
				{
					new DataElement
					{
						ElementId = $"ELEM-{i:D4}",
						RequestId = $"REQ-{i:D4}",
						ElementType = ElementTypeEnum.Text,
						RawValue = "100",
						ValidatedValue = "100",
						ValidationStatus = ValidationEnum.Valid,
						SourceLocation = "Source A",
						ValidationNotes = "Validated successfully",
					},
				}
				: new List<DataElement>(),
				Notification = i % 5 == 0 ? new Notification
				{
					NotificationId = $"NOTIF-{i:D4}",
					RequestId = $"REQ-{i:D4}",
					RecipientType = RecipientEnum.Engineer,
					NotificationType = NotificationTypeEnum.Email,
					DeliveryMethod = DeliveryEnum.Sent,
					SentDate = createdDate.AddDays(-1),
					Content = $"Notification content for request {i}",
				}
				: null,
				StateDiagram = stateDiagram,
			};

			requests.Add(request);
		}

		return requests;
	}

	/// <summary>
	/// Generates a single mock Request entity with specific properties.
	/// </summary>
	/// <param name="id">Custom request ID.</param>
	/// <param name="status">Custom status.</param>
	/// <param name="priority">Custom priority.</param>
	/// <returns>Mock Request entity.</returns>
	public static Request GenerateRequest(string id = "REQ-0001", StatusEnum status = StatusEnum.InProgress, PriorityEnum priority = PriorityEnum.Normal)
	{
		var now = DateTime.Now;
		return new Request
		{
			RequestId = id,
			Status = status,
			CreatedDate = now.AddDays(-7),
			Priority = priority,
			ClientId = "john.doe",
			SourceEmail = "john.doe@example.com",
			AssignedEngineerId = "ENG-001",
			AssignedBy = "MGR-01",
			AcknowledgmentDate = status >= StatusEnum.Submitted ? now.AddDays(-6) : default,
			CompletionDate = status >= StatusEnum.Completed ? now.AddDays(-1) : default,
		};
	}
}
