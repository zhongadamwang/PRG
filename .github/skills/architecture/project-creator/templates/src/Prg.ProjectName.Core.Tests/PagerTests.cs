using Prg.ProjectName.Core.Common;

namespace Prg.ProjectName.Core.Tests;

public class PagerTests
{
	[Test]
	public void DefaultConstructor_SetsDefaults()
	{
		var pager = new Pager();

		Assert.That(pager.PageIndex, Is.EqualTo(1));
		Assert.That(pager.PageSize, Is.EqualTo(8));
		Assert.That(pager.OrderBy, Is.EqualTo(string.Empty));
		Assert.That(pager.Filter, Is.Not.Null);
	}

	[Test]
	public void Init_ResetsToDefaults()
	{
		var pager = new Pager(3, 20, 5, 100);
		pager.OrderBy = "name";
		pager.Init();

		Assert.That(pager.PageIndex, Is.EqualTo(1));
		Assert.That(pager.PageTotal, Is.EqualTo(0));
		Assert.That(pager.TotalCounts, Is.EqualTo(0));
		Assert.That(pager.OrderBy, Is.EqualTo(string.Empty));
	}

	[Test]
	public void ParameterConstructor_SetsValues()
	{
		var pager = new Pager(2, 15, 4, 60);

		Assert.That(pager.PageIndex, Is.EqualTo(2));
		Assert.That(pager.PageSize, Is.EqualTo(15));
		Assert.That(pager.PageTotal, Is.EqualTo(4));
		Assert.That(pager.TotalCounts, Is.EqualTo(60));
	}

	[Test]
	public void PopulateFromAndTo_TransfersValues()
	{
		var entity = new MetaShare.Common.Core.Entities.Pager
		{
			PageIndex = 5,
			PageSize = 50,
			PageTotal = 10,
			TotalCounts = 500,
			OrderBy = "email"
		};

		var pager = new Pager();
		pager.PopulateFrom(entity);

		Assert.That(pager.PageIndex, Is.EqualTo(5));
		Assert.That(pager.PageSize, Is.EqualTo(50));
		Assert.That(pager.PageTotal, Is.EqualTo(10));
		Assert.That(pager.TotalCounts, Is.EqualTo(500));
		Assert.That(pager.OrderBy, Is.EqualTo("email"));

		var toEntity = new MetaShare.Common.Core.Entities.Pager();
		pager.PopulateTo(toEntity);

		Assert.That(toEntity.PageIndex, Is.EqualTo(5));
		Assert.That(toEntity.PageSize, Is.EqualTo(50));
		Assert.That(toEntity.PageTotal, Is.EqualTo(10));
		Assert.That(toEntity.TotalCounts, Is.EqualTo(500));
		Assert.That(toEntity.OrderBy, Is.EqualTo("email"));

		// Confirm PopulateTo handles null without throwing
		Assert.DoesNotThrow(() => pager.PopulateTo(null!));
	}

	[Test]
	public void FilterCondition_DefaultsAndAssignment()
	{
		var f = new FilterCondition();
		Assert.That(f.Field, Is.EqualTo(string.Empty));
		Assert.That(f.Operator, Is.EqualTo(string.Empty));
		Assert.That(f.Value, Is.Null);

		f.Field = "Status";
		f.Operator = "equal";
		f.Value = 1;

		Assert.That(f.Field, Is.EqualTo("Status"));
		Assert.That(f.Operator, Is.EqualTo("equal"));
		Assert.That(f.Value, Is.EqualTo(1));
	}
}
