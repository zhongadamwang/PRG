using Moq;
using Entity = Sesi.SanjelData.Entities.BusinessEntities.Operation.Dispatch.CallSheet;
using IDataService = Sesi.SanjelData.Services.Interfaces.BusinessEntities.Operation.Dispatch.ICallSheetService;

namespace Sanjel.RequestManagement.Repositories.Tests;

[TestFixture]
public class CallSheetRepositoryTests
{
	private Mock<IDataService> _mockDataService;
	private CallSheetRepository _repository;

	[SetUp]
	public void SetUp()
	{
		_mockDataService = new Mock<IDataService>();
		_repository = new CallSheetRepository(_mockDataService.Object);
	}

	[Test]
	public void Constructor_WithDataService_InitializesRepository()
	{
		Assert.That(_repository, Is.Not.Null);
		Assert.That(_repository, Is.InstanceOf<ICallSheetRepository>());
	}

	[Test]
	public async Task GetByIdAsync_WithValidId_CallsDataServiceAsync()
	{
		var id = 1;
		var expected = new Entity { Id = id };
		_mockDataService.Setup(x => x.SelectById(It.Is<Entity>(e => e.Id == id))).Returns(expected);

		var result = await _repository.GetByIdAsync(id);

		Assert.That(result, Is.Not.Null);
		Assert.That(result.Id, Is.EqualTo(id));
		_mockDataService.Verify(x => x.SelectById(It.Is<Entity>(e => e.Id == id)), Times.Once);
	}

	[Test]
	public async Task GetByIdWithChildrenAsync_WithValidId_CallsDataServiceWithChildrenAsync()
	{
		var id = 1;
		var expected = new Entity { Id = id };
		_mockDataService.Setup(x => x.SelectById(It.Is<Entity>(e => e.Id == id), true)).Returns(expected);

		var result = await _repository.GetByIdWithChildrenAsync(id);

		Assert.That(result, Is.Not.Null);
		Assert.That(result.Id, Is.EqualTo(id));
		_mockDataService.Verify(x => x.SelectById(It.Is<Entity>(e => e.Id == id), true), Times.Once);
	}

	[Test]
	public async Task CreateAsync_WithValidEntity_ReturnsTrueAsync()
	{
		var entity = new Entity();
		_mockDataService.Setup(x => x.Insert(It.IsAny<Entity>(), false)).Returns(1);

		var result = await _repository.CreateAsync(entity);

		Assert.That(result, Is.True);
		_mockDataService.Verify(x => x.Insert(entity, false), Times.Once);
	}

	[Test]
	public async Task CreateAsync_WithNullEntity_ReturnsFalseAsync()
	{
		Entity entity = null!;

		var result = await _repository.CreateAsync(entity);

		Assert.That(result, Is.False);
		_mockDataService.Verify(x => x.Insert(It.IsAny<Entity>(), It.IsAny<bool>()), Times.Never);
	}

	[Test]
	public async Task UpdateAsync_WithValidEntity_ReturnsTrueAsync()
	{
		var entity = new Entity { Id = 1 };
		_mockDataService.Setup(x => x.Update(It.IsAny<Entity>(), false)).Returns(1);

		var result = await _repository.UpdateAsync(entity);

		Assert.That(result, Is.True);
		_mockDataService.Verify(x => x.Update(entity, false), Times.Once);
	}

	[Test]
	public async Task DeleteAsync_WithId_ReturnsTrueAsync()
	{
		var id = 1;
		_mockDataService.Setup(x => x.Delete(It.Is<Entity>(e => e.Id == id), false)).Returns(1);

		var result = await _repository.DeleteAsync(id);

		Assert.That(result, Is.True);
		_mockDataService.Verify(x => x.Delete(It.Is<Entity>(e => e.Id == id), false), Times.Once);
	}
}
