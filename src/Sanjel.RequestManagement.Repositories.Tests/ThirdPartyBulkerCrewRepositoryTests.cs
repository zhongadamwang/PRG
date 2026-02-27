using Moq;
using Entity = Sesi.SanjelData.Entities.BusinessEntities.Operation.Crew.ThirdPartyBulkerCrew;
using IDataService = Sesi.SanjelData.Services.Interfaces.BusinessEntities.Operation.Crew.IThirdPartyBulkerCrewService;

namespace Sanjel.RequestManagement.Repositories.Tests;

[TestFixture]
public class ThirdPartyBulkerCrewRepositoryTests
{
    private Mock<IDataService> _mockDataService;
    private Mock<Sanjel.RequestManagement.Core.Services.ICurrentUserService> _mockCurrentUserService;
    private ThirdPartyBulkerCrewRepository _repository;

    [SetUp]
    public void SetUp()
    {
        _mockDataService = new Mock<IDataService>();
        _mockCurrentUserService = new Mock<Sanjel.RequestManagement.Core.Services.ICurrentUserService>();
        _repository = new ThirdPartyBulkerCrewRepository(_mockDataService.Object, _mockCurrentUserService.Object);
    }

    [Test]
    public void Constructor_WithDependencies_InitializesRepository()
    {
        Assert.That(_repository, Is.Not.Null);
        Assert.That(_repository, Is.InstanceOf<IThirdPartyBulkerCrewRepository>());
    }

    [Test]
    public async Task CreateAsync_SetsModifiedUserName_AndCallsInsertAsync()
    {
        var user = "tester";
        _mockCurrentUserService.Setup(x => x.GetCurrentUsername()).Returns(user);

        _mockDataService.Setup(x => x.Insert(It.Is<Entity>(e => e.ModifiedUserName == user), false)).Returns(1);

        var entity = new Entity();

        var result = await _repository.CreateAsync(entity);

        Assert.That(result, Is.True);
        _mockDataService.Verify(x => x.Insert(It.Is<Entity>(e => e.ModifiedUserName == user), false), Times.Once);
    }

    [Test]
    public async Task UpdateAsync_SetsModifiedUserName_AndCallsUpdateAsync()
    {
        var user = "updater";
        _mockCurrentUserService.Setup(x => x.GetCurrentUsername()).Returns(user);

        _mockDataService.Setup(x => x.Update(It.Is<Entity>(e => e.ModifiedUserName == user), false)).Returns(1);

        var entity = new Entity { Id = 5 };

        var result = await _repository.UpdateAsync(entity);

        Assert.That(result, Is.True);
        _mockDataService.Verify(x => x.Update(It.Is<Entity>(e => e.ModifiedUserName == user), false), Times.Once);
    }

    [Test]
    public async Task DeleteAsync_WithEntity_SetsModifiedUserName_AndCallsDeleteAsync()
    {
        var user = "deleter";
        _mockCurrentUserService.Setup(x => x.GetCurrentUsername()).Returns(user);

        _mockDataService.Setup(x => x.Delete(It.Is<Entity>(e => e.ModifiedUserName == user), false)).Returns(1);

        var entity = new Entity { Id = 2 };

        var result = await _repository.DeleteAsync(entity);

        Assert.That(result, Is.True);
        _mockDataService.Verify(x => x.Delete(It.Is<Entity>(e => e.ModifiedUserName == user), false), Times.Once);
    }
}
