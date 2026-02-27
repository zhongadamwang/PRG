using Sanjel.RequestManagement.Core.Common;

namespace Sanjel.RequestManagement.Core.Tests;

public class PagerResultTests
{
	private class TestEntity : MetaShare.Common.Core.Entities.Common
	{
	}

	[Test]
	public void DefaultConstructor_InitializesCollections()
	{
		var pr = new PagerResult<TestEntity>();

		Assert.That(pr.Result, Is.Not.Null);
		Assert.That(pr.Pager, Is.Not.Null);
		Assert.That(pr.Result, Is.InstanceOf<IEnumerable<TestEntity>>());
	}

	[Test]
	public void Result_CanBeSetAndEnumerated()
	{
		var pr = new PagerResult<TestEntity>();
		var list = new List<TestEntity>
				{
						new TestEntity { Name = "A" },
						new TestEntity { Name = "B" }
				};

		pr.Result = list;

		Assert.That(pr.Result.Count(), Is.EqualTo(2));
		Assert.That(pr.Result.First().Name, Is.EqualTo("A"));
	}
}
