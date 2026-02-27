using System.Linq.Expressions;
using Moq;
using Entity = Sesi.SanjelData.Entities.BusinessEntities.Engineering.ProgramDesign.ProgramRequest;
using IDataService = Sesi.SanjelData.Services.Interfaces.BusinessEntities.Engineering.ProgramDesign.IProgramRequestService;

namespace Sanjel.RequestManagement.Repositories.Tests;

[TestFixture]
public class ProgramRequestRepositoryTests
{
	private Mock<IDataService> _mockDataService;
	private ProgramRequestRepository _repository;

	[SetUp]
	public void SetUp()
	{
		_mockDataService = new Mock<IDataService>();
		_repository = new ProgramRequestRepository(_mockDataService.Object);
	}

	[Test]
	public void Constructor_WithDataService_InitializesRepository()
	{
		// Arrange & Act - performed in SetUp

		// Assert
		Assert.That(_repository, Is.Not.Null);
		Assert.That(_repository, Is.InstanceOf<IProgramRequestRepository>());
	}

	[Test]
	public async Task GetByIdAsync_WithValidId_ReturnsEntityAsync()
	{
		// Arrange
		var expectedId = 1;
		var expectedEntity = new Entity { Id = expectedId, Name = "Test Request" };
		_mockDataService.Setup(x => x.SelectById(It.Is<Entity>(e => e.Id == expectedId)))
				.Returns(expectedEntity);

		// Act
		var result = await _repository.GetByIdAsync(expectedId);

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result.Id, Is.EqualTo(expectedId));
		Assert.That(result.Name, Is.EqualTo("Test Request"));
		_mockDataService.Verify(x => x.SelectById(It.Is<Entity>(e => e.Id == expectedId)), Times.Once);
	}

	[Test]
	public async Task GetByIdWithChildrenAsync_WithValidId_ReturnsEntityWithChildrenAsync()
	{
		// Arrange
		var expectedId = 1;
		var expectedEntity = new Entity { Id = expectedId, Name = "Test Request" };
		_mockDataService.Setup(x => x.SelectById(It.Is<Entity>(e => e.Id == expectedId), true))
				.Returns(expectedEntity);

		// Act
		var result = await _repository.GetByIdWithChildrenAsync(expectedId);

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result.Id, Is.EqualTo(expectedId));
		_mockDataService.Verify(x => x.SelectById(It.Is<Entity>(e => e.Id == expectedId), true), Times.Once);
	}

	[Test]
	public async Task GetPagedListAsync_WithPagerAndExpression_ReturnsPagedResultAsync()
	{
		// Arrange
		var pager = new Sanjel.RequestManagement.Core.Common.Pager { PageIndex = 1, PageSize = 10 };
		Expression<Func<Entity, bool>> expression = x => x.Id > 0;
		var entities = new List<Entity>
				{
						new Entity { Id = 1, Name = "Request 1" },
						new Entity { Id = 2, Name = "Request 2" }
				};

		_mockDataService.Setup(x => x.SelectBy(It.IsAny<MetaShare.Common.Core.Entities.Pager>(), It.IsAny<Entity>(), It.IsAny<Expression<Func<Entity, bool>>>()))
				.Returns(entities);

		// Act
		var result = await _repository.GetPagedListAsync(pager, expression);

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result.Result, Is.Not.Null);
		Assert.That(result.Result.Count, Is.EqualTo(2));
		Assert.That(result.Pager, Is.EqualTo(pager));
		_mockDataService.Verify(x => x.SelectBy(It.IsAny<MetaShare.Common.Core.Entities.Pager>(), It.IsAny<Entity>(), expression), Times.Once);
	}

	[Test]
	public async Task GetListAsync_WithExpression_ReturnsListAsync()
	{
		// Arrange
		Expression<Func<Entity, bool>> expression = x => x.Id > 0;
		var entities = new List<Entity>
				{
						new Entity { Id = 1, Name = "Request 1" },
						new Entity { Id = 2, Name = "Request 2" }
				};

		_mockDataService.Setup(x => x.SelectBy(It.IsAny<Entity>(), expression))
				.Returns(entities);

		// Act
		var result = await _repository.GetListAsync(expression);

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result.Count, Is.EqualTo(2));
		_mockDataService.Verify(x => x.SelectBy(It.IsAny<Entity>(), expression), Times.Once);
	}

	[Test]
	public async Task GetListAsync_WithNullResult_ReturnsEmptyListAsync()
	{
		// Arrange
		Expression<Func<Entity, bool>> expression = x => x.Id > 0;
		_mockDataService.Setup(x => x.SelectBy(It.IsAny<Entity>(), expression))
				.Returns((List<Entity>)null!);

		// Act
		var result = await _repository.GetListAsync(expression);

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result.Count, Is.EqualTo(0));
	}

	[Test]
	public async Task CreateAsync_WithValidEntity_ReturnsTrueAsync()
	{
		// Arrange
		var entity = new Entity { Name = "New Request" };
		_mockDataService.Setup(x => x.Insert(It.IsAny<Entity>(), false))
				.Returns(1);

		// Act
		var result = await _repository.CreateAsync(entity);

		// Assert
		Assert.That(result, Is.True);
		_mockDataService.Verify(x => x.Insert(entity, false), Times.Once);
	}

	[Test]
	public async Task CreateAsync_WithNullEntity_ReturnsFalseAsync()
	{
		// Arrange
		Entity entity = null!;

		// Act
		var result = await _repository.CreateAsync(entity);

		// Assert
		Assert.That(result, Is.False);
		_mockDataService.Verify(x => x.Insert(It.IsAny<Entity>(), It.IsAny<bool>()), Times.Never);
	}

	[Test]
	public async Task CreateWithChildrenAsync_WithValidEntity_ReturnsTrueAsync()
	{
		// Arrange
		var entity = new Entity { Name = "New Request" };
		_mockDataService.Setup(x => x.Insert(It.IsAny<Entity>(), true))
				.Returns(1);

		// Act
		var result = await _repository.CreateWithChildrenAsync(entity);

		// Assert
		Assert.That(result, Is.True);
		_mockDataService.Verify(x => x.Insert(entity, true), Times.Once);
	}

	[Test]
	public async Task UpdateAsync_WithValidEntity_ReturnsTrueAsync()
	{
		// Arrange
		var entity = new Entity { Id = 1, Name = "Updated Request" };
		_mockDataService.Setup(x => x.Update(It.IsAny<Entity>(), false))
				.Returns(1);

		// Act
		var result = await _repository.UpdateAsync(entity);

		// Assert
		Assert.That(result, Is.True);
		_mockDataService.Verify(x => x.Update(entity, false), Times.Once);
	}

	[Test]
	public async Task UpdateAsync_WithNullEntity_ReturnsFalseAsync()
	{
		// Arrange
		Entity entity = null!;

		// Act
		var result = await _repository.UpdateAsync(entity);

		// Assert
		Assert.That(result, Is.False);
		_mockDataService.Verify(x => x.Update(It.IsAny<Entity>(), It.IsAny<bool>()), Times.Never);
	}

	[Test]
	public async Task UpdateWithChildrenAsync_WithValidEntity_ReturnsTrueAsync()
	{
		// Arrange
		var entity = new Entity { Id = 1, Name = "Updated Request" };
		_mockDataService.Setup(x => x.Update(It.IsAny<Entity>(), true))
				.Returns(1);

		// Act
		var result = await _repository.UpdateWithChildrenAsync(entity);

		// Assert
		Assert.That(result, Is.True);
		_mockDataService.Verify(x => x.Update(entity, true), Times.Once);
	}

	[Test]
	public async Task DeleteAsync_WithValidEntity_ReturnsTrueAsync()
	{
		// Arrange
		var entity = new Entity { Id = 1, Name = "Request to Delete" };
		_mockDataService.Setup(x => x.Delete(It.IsAny<Entity>(), false))
				.Returns(1);

		// Act
		var result = await _repository.DeleteAsync(entity);

		// Assert
		Assert.That(result, Is.True);
		_mockDataService.Verify(x => x.Delete(entity, false), Times.Once);
	}

	[Test]
	public async Task DeleteAsync_WithNullEntity_ReturnsFalseAsync()
	{
		// Arrange
		Entity entity = null!;

		// Act
		var result = await _repository.DeleteAsync(entity);

		// Assert
		Assert.That(result, Is.False);
		_mockDataService.Verify(x => x.Delete(It.IsAny<Entity>(), It.IsAny<bool>()), Times.Never);
	}

	[Test]
	public async Task DeleteAsync_WithId_ReturnsTrueAsync()
	{
		// Arrange
		var id = 1;
		_mockDataService.Setup(x => x.Delete(It.Is<Entity>(e => e.Id == id), false))
				.Returns(1);

		// Act
		var result = await _repository.DeleteAsync(id);

		// Assert
		Assert.That(result, Is.True);
		_mockDataService.Verify(x => x.Delete(It.Is<Entity>(e => e.Id == id), false), Times.Once);
	}

	[Test]
	public async Task DeleteWithChildrenAsync_WithValidEntity_ReturnsTrueAsync()
	{
		// Arrange
		var entity = new Entity { Id = 1, Name = "Request to Delete" };
		_mockDataService.Setup(x => x.Delete(It.IsAny<Entity>(), true))
				.Returns(1);

		// Act
		var result = await _repository.DeleteWithChildrenAsync(entity);

		// Assert
		Assert.That(result, Is.True);
		_mockDataService.Verify(x => x.Delete(entity, true), Times.Once);
	}

	[Test]
	public async Task DeleteWithChildrenAsync_WithId_ReturnsTrueAsync()
	{
		// Arrange
		var id = 1;
		_mockDataService.Setup(x => x.Delete(It.Is<Entity>(e => e.Id == id), true))
				.Returns(1);

		// Act
		var result = await _repository.DeleteWithChildrenAsync(id);

		// Assert
		Assert.That(result, Is.True);
		_mockDataService.Verify(x => x.Delete(It.Is<Entity>(e => e.Id == id), true), Times.Once);
	}

	[Test]
	public async Task GetListByIdsAsync_WithColumnNameAndIds_ReturnsListAsync()
	{
		// Arrange
		var columnName = "Id";
		var ids = new[] { 1, 2, 3 };
		var entities = new List<Entity>
				{
						new Entity { Id = 1, Name = "Request 1" },
						new Entity { Id = 2, Name = "Request 2" },
						new Entity { Id = 3, Name = "Request 3" }
				};

		_mockDataService.Setup(x => x.SelectByColumnIds(columnName, ids, false))
				.Returns(entities);

		// Act
		var result = await _repository.GetListByIdsAsync(columnName, ids);

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result.Count, Is.EqualTo(3));
		_mockDataService.Verify(x => x.SelectByColumnIds(columnName, ids, false), Times.Once);
	}

	[Test]
	public async Task GetListWithChildrenByIdsAsync_WithColumnNameAndIds_ReturnsListAsync()
	{
		// Arrange
		var columnName = "Id";
		var ids = new[] { 1, 2, 3 };
		var entities = new List<Entity>
				{
						new Entity { Id = 1, Name = "Request 1" },
						new Entity { Id = 2, Name = "Request 2" },
						new Entity { Id = 3, Name = "Request 3" }
				};

		_mockDataService.Setup(x => x.SelectByColumnIds(columnName, ids, true))
				.Returns(entities);

		// Act
		var result = await _repository.GetListWithChildrenByIdsAsync(columnName, ids);

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result.Count, Is.EqualTo(3));
		_mockDataService.Verify(x => x.SelectByColumnIds(columnName, ids, true), Times.Once);
	}

	[Test]
	public async Task GetListByIdsAsync_WithNullResult_ReturnsEmptyListAsync()
	{
		// Arrange
		var columnName = "Id";
		var ids = new[] { 1, 2, 3 };
		_mockDataService.Setup(x => x.SelectByColumnIds(columnName, ids, false))
				.Returns((List<Entity>)null!);

		// Act
		var result = await _repository.GetListByIdsAsync(columnName, ids);

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result.Count, Is.EqualTo(0));
	}
}