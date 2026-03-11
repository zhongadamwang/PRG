using NUnit.Framework;
using Sanjel.RequestManagement.Blazor.Pages.Requests.ViewModels;
using Sanjel.RequestManagement.Entities.Entities;

namespace Sanjel.RequestManagement.Blazor.Tests
{
	/// <summary>
	/// Unit tests for RequestViewModel
	/// Tests validation, default values, filter behavior, and pagination
	/// </summary>
	[TestFixture]
	public class RequestViewModelTests
	{
		#region Default Constructor Tests

		[Test]
		public void DefaultConstructor_ShouldSetSensibleDefaults()
		{
			// Arrange & Act
			var vm = new RequestViewModel();

			// Assert
			Assert.That(vm.RequestId, Is.EqualTo(string.Empty));
			Assert.That(vm.Status, Is.EqualTo(default(StatusEnum)));
			Assert.That(vm.Priority, Is.EqualTo(default(PriorityEnum)));
			Assert.That(vm.ClientId, Is.EqualTo(string.Empty));
			Assert.That(vm.SourceEmail, Is.EqualTo(string.Empty));
			Assert.That(vm.AssignedEngineerId, Is.EqualTo(string.Empty));
			Assert.That(vm.AssignedBy, Is.EqualTo(string.Empty));
			Assert.That(vm.CurrentPage, Is.EqualTo(1));
			Assert.That(vm.PageSize, Is.EqualTo(20));
			Assert.That(vm.TotalItems, Is.EqualTo(0));
		}

		[Test]
		public void DefaultConstructor_FilterProperties_ShouldBeNull()
		{
			// Arrange & Act
			var vm = new RequestViewModel();

			// Assert
			Assert.That(vm.SearchTerm, Is.Null);
			Assert.That(vm.StatusFilter, Is.Null);
			Assert.That(vm.PriorityFilter, Is.Null);
			Assert.That(vm.ClientIdFilter, Is.Null);
			Assert.That(vm.AssignedEngineerIdFilter, Is.Null);
			Assert.That(vm.StartDateFilter, Is.Null);
			Assert.That(vm.EndDateFilter, Is.Null);
		}

		#endregion

		#region Property Assignment Tests

		[Test]
		public void Properties_ShouldAcceptAndStoreValidValues()
		{
			// Arrange
			var vm = new RequestViewModel();
			var testDate = DateTime.Now;

			// Act
			vm.RequestId = "REQ-12345";
			vm.Status = StatusEnum.InProgress;
			vm.Priority = PriorityEnum.High;
			vm.ClientId = "CLIENT-001";
			vm.SourceEmail = "test@example.com";
			vm.AssignedEngineerId = "ENG-001";
			vm.AssignedBy = "MGR-001";
			vm.CreatedDate = testDate;
			vm.AcknowledgmentDate = testDate.AddDays(1);
			vm.CompletionDate = testDate.AddDays(5);

			// Assert
			Assert.That(vm.RequestId, Is.EqualTo("REQ-12345"));
			Assert.That(vm.Status, Is.EqualTo(StatusEnum.InProgress));
			Assert.That(vm.Priority, Is.EqualTo(PriorityEnum.High));
			Assert.That(vm.ClientId, Is.EqualTo("CLIENT-001"));
			Assert.That(vm.SourceEmail, Is.EqualTo("test@example.com"));
			Assert.That(vm.AssignedEngineerId, Is.EqualTo("ENG-001"));
			Assert.That(vm.AssignedBy, Is.EqualTo("MGR-001"));
			Assert.That(vm.CreatedDate, Is.EqualTo(testDate));
			Assert.That(vm.AcknowledgmentDate, Is.EqualTo(testDate.AddDays(1)));
			Assert.That(vm.CompletionDate, Is.EqualTo(testDate.AddDays(5)));
		}

		[Test]
		public void FilterProperties_ShouldAcceptAndStoreValues()
		{
			// Arrange
			var vm = new RequestViewModel();
			var testDate = DateTime.Now;

			// Act
			vm.SearchTerm = "test search";
			vm.StatusFilter = StatusEnum.Submitted;
			vm.PriorityFilter = PriorityEnum.Critical;
			vm.ClientIdFilter = "CLIENT-001";
			vm.AssignedEngineerIdFilter = "ENG-001";
			vm.StartDateFilter = testDate.AddDays(-30);
			vm.EndDateFilter = testDate.AddDays(30);

			// Assert
			Assert.That(vm.SearchTerm, Is.EqualTo("test search"));
			Assert.That(vm.StatusFilter, Is.EqualTo(StatusEnum.Submitted));
			Assert.That(vm.PriorityFilter, Is.EqualTo(PriorityEnum.Critical));
			Assert.That(vm.ClientIdFilter, Is.EqualTo("CLIENT-001"));
			Assert.That(vm.AssignedEngineerIdFilter, Is.EqualTo("ENG-001"));
			Assert.That(vm.StartDateFilter, Is.EqualTo(testDate.AddDays(-30)));
			Assert.That(vm.EndDateFilter, Is.EqualTo(testDate.AddDays(30)));
		}

		#endregion

		#region HasActiveFilters Tests

		[Test]
		public void HasActiveFilters_WithNoFilters_ShouldReturnFalse()
		{
			// Arrange
			var vm = new RequestViewModel();

			// Act
			var result = vm.HasActiveFilters();

			// Assert
			Assert.That(result, Is.False);
		}

		[Test]
		public void HasActiveFilters_WithSearchTerm_ShouldReturnTrue()
		{
			// Arrange
			var vm = new RequestViewModel { SearchTerm = "test" };

			// Act
			var result = vm.HasActiveFilters();

			// Assert
			Assert.That(result, Is.True);
		}

		[Test]
		public void HasActiveFilters_WithStatusFilter_ShouldReturnTrue()
		{
			// Arrange
			var vm = new RequestViewModel { StatusFilter = StatusEnum.InProgress };

			// Act
			var result = vm.HasActiveFilters();

			// Assert
			Assert.That(result, Is.True);
		}

		[Test]
		public void HasActiveFilters_WithPriorityFilter_ShouldReturnTrue()
		{
			// Arrange
			var vm = new RequestViewModel { PriorityFilter = PriorityEnum.High };

			// Act
			var result = vm.HasActiveFilters();

			// Assert
			Assert.That(result, Is.True);
		}

		[Test]
		public void HasActiveFilters_WithClientIdFilter_ShouldReturnTrue()
		{
			// Arrange
			var vm = new RequestViewModel { ClientIdFilter = "CLIENT-001" };

			// Act
			var result = vm.HasActiveFilters();

			// Assert
			Assert.That(result, Is.True);
		}

		[Test]
		public void HasActiveFilters_WithEngineerIdFilter_ShouldReturnTrue()
		{
			// Arrange
			var vm = new RequestViewModel { AssignedEngineerIdFilter = "ENG-001" };

			// Act
			var result = vm.HasActiveFilters();

			// Assert
			Assert.That(result, Is.True);
		}

		[Test]
		public void HasActiveFilters_WithStartDateFilter_ShouldReturnTrue()
		{
			// Arrange
			var vm = new RequestViewModel { StartDateFilter = DateTime.Now };

			// Act
			var result = vm.HasActiveFilters();

			// Assert
			Assert.That(result, Is.True);
		}

		[Test]
		public void HasActiveFilters_WithEndDateFilter_ShouldReturnTrue()
		{
			// Arrange
			var vm = new RequestViewModel { EndDateFilter = DateTime.Now };

			// Act
			var result = vm.HasActiveFilters();

			// Assert
			Assert.That(result, Is.True);
		}

		[Test]
		public void HasActiveFilters_WithEmptyStringSearchTerm_ShouldReturnFalse()
		{
			// Arrange
			var vm = new RequestViewModel { SearchTerm = string.Empty };

			// Act
			var result = vm.HasActiveFilters();

			// Assert
			Assert.That(result, Is.False);
		}

		[Test]
		public void HasActiveFilters_WithWhitespaceSearchTerm_ShouldReturnFalse()
		{
			// Arrange
			var vm = new RequestViewModel { SearchTerm = "   " };

			// Act
			var result = vm.HasActiveFilters();

			// Assert
			Assert.That(result, Is.False);
		}

		[Test]
		public void HasActiveFilters_WithMultipleFilters_ShouldReturnTrue()
		{
			// Arrange
			var vm = new RequestViewModel
			{
				SearchTerm = "test",
				StatusFilter = StatusEnum.InProgress,
				PriorityFilter = PriorityEnum.High,
			};

			// Act
			var result = vm.HasActiveFilters();

			// Assert
			Assert.That(result, Is.True);
		}

		#endregion

		#region ResetFilters Tests

		[Test]
		public void ResetFilters_ShouldClearAllFilters()
		{
			// Arrange
			var vm = new RequestViewModel
			{
				SearchTerm = "test",
				StatusFilter = StatusEnum.InProgress,
				PriorityFilter = PriorityEnum.High,
				ClientIdFilter = "CLIENT-001",
				AssignedEngineerIdFilter = "ENG-001",
				StartDateFilter = DateTime.Now.AddDays(-30),
				EndDateFilter = DateTime.Now.AddDays(30),
				CurrentPage = 5,
			};

			// Act
			vm.ResetFilters();

			// Assert
			Assert.That(vm.SearchTerm, Is.Null);
			Assert.That(vm.StatusFilter, Is.Null);
			Assert.That(vm.PriorityFilter, Is.Null);
			Assert.That(vm.ClientIdFilter, Is.Null);
			Assert.That(vm.AssignedEngineerIdFilter, Is.Null);
			Assert.That(vm.StartDateFilter, Is.Null);
			Assert.That(vm.EndDateFilter, Is.Null);
			Assert.That(vm.CurrentPage, Is.EqualTo(1));
		}

		[Test]
		public void ResetFilters_WhenAlreadyReset_ShouldNotChangeValues()
		{
			// Arrange
			var vm = new RequestViewModel();

			// Act
			vm.ResetFilters();

			// Assert - Should remain at default values
			Assert.That(vm.SearchTerm, Is.Null);
			Assert.That(vm.StatusFilter, Is.Null);
			Assert.That(vm.PriorityFilter, Is.Null);
			Assert.That(vm.ClientIdFilter, Is.Null);
			Assert.That(vm.AssignedEngineerIdFilter, Is.Null);
			Assert.That(vm.StartDateFilter, Is.Null);
			Assert.That(vm.EndDateFilter, Is.Null);
			Assert.That(vm.CurrentPage, Is.EqualTo(1));
		}

		[Test]
		public void ResetFilters_AfterReset_HasActiveFiltersShouldReturnFalse()
		{
			// Arrange
			var vm = new RequestViewModel
			{
				SearchTerm = "test",
				StatusFilter = StatusEnum.InProgress,
			};

			// Act
			vm.ResetFilters();
			var result = vm.HasActiveFilters();

			// Assert
			Assert.That(result, Is.False);
		}

		#endregion

		#region Pagination Tests

		[Test]
		public void TotalPages_WithZeroItems_ShouldReturnZero()
		{
			// Arrange
			var vm = new RequestViewModel { TotalItems = 0, PageSize = 20 };

			// Act
			var totalPages = vm.TotalPages;

			// Assert
			Assert.That(totalPages, Is.EqualTo(0));
		}

		[Test]
		public void TotalPages_WithItemsLessThanPageSize_ShouldReturnOne()
		{
			// Arrange
			var vm = new RequestViewModel { TotalItems = 15, PageSize = 20 };

			// Act
			var totalPages = vm.TotalPages;

			// Assert
			Assert.That(totalPages, Is.EqualTo(1));
		}

		[Test]
		public void TotalPages_WithItemsEqualToPageSize_ShouldReturnOne()
		{
			// Arrange
			var vm = new RequestViewModel { TotalItems = 20, PageSize = 20 };

			// Act
			var totalPages = vm.TotalPages;

			// Assert
			Assert.That(totalPages, Is.EqualTo(1));
		}

		[Test]
		public void TotalPages_WithItemsMoreThanPageSize_ShouldCalculateCorrectly()
		{
			// Arrange
			var vm = new RequestViewModel { TotalItems = 45, PageSize = 20 };

			// Act
			var totalPages = vm.TotalPages;

			// Assert
			Assert.That(totalPages, Is.EqualTo(3));
		}

		[Test]
		public void TotalPages_WithPartialPage_ShouldRoundUp()
		{
			// Arrange
			var vm = new RequestViewModel { TotalItems = 41, PageSize = 20 };

			// Act
			var totalPages = vm.TotalPages;

			// Assert
			Assert.That(totalPages, Is.EqualTo(3));
		}

		[Test]
		public void CurrentPage_DefaultValue_ShouldBeOne()
		{
			// Arrange & Act
			var vm = new RequestViewModel();

			// Assert
			Assert.That(vm.CurrentPage, Is.EqualTo(1));
		}

		[Test]
		public void PageSize_DefaultValue_ShouldBeTwenty()
		{
			// Arrange & Act
			var vm = new RequestViewModel();

			// Assert
			Assert.That(vm.PageSize, Is.EqualTo(20));
		}

		[Test]
		public void PaginationProperties_ShouldAcceptValidValues()
		{
			// Arrange
			var vm = new RequestViewModel();

			// Act
			vm.CurrentPage = 5;
			vm.PageSize = 50;
			vm.TotalItems = 250;

			// Assert
			Assert.That(vm.CurrentPage, Is.EqualTo(5));
			Assert.That(vm.PageSize, Is.EqualTo(50));
			Assert.That(vm.TotalItems, Is.EqualTo(250));
			Assert.That(vm.TotalPages, Is.EqualTo(5));
		}

		#endregion
	}
}
