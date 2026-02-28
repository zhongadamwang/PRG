namespace Prg.ProjectName.Core.Common
{
	public class Pager
	{
		public int PageIndex { get; set; }

		public int PageSize { get; set; }

		public int PageTotal { get; set; }

		public int TotalCounts { get; set; }

		public string OrderBy { get; set; } = string.Empty;

		/// <summary>
		/// Current filter conditions applied to the data set.
		/// Each item represents a single filter on a field with an operator and value.
		/// </summary>
		public IList<FilterCondition> Filter { get; set; } = new List<FilterCondition>();

		public Pager()
		{
			this.PageIndex = 1;
			this.PageSize = 8;
		}

		public void Init()
		{
			this.PageIndex = 1;
			this.PageTotal = 0;
			this.TotalCounts = 0;
			this.OrderBy = string.Empty;
		}

		public Pager(int pageIndex, int pageSize, int pageTotal, int totalCounts)
		{
			this.PageIndex = pageIndex;
			this.PageTotal = pageTotal;
			this.PageSize = pageSize;
			TotalCounts = totalCounts;
			this.OrderBy = string.Empty;
		}

		public void PopulateFrom(MetaShare.Common.Core.Entities.Pager entity)
		{
			this.PageIndex = entity.PageIndex;
			this.PageSize = entity.PageSize;
			this.PageTotal = entity.PageTotal;
			this.TotalCounts = entity.TotalCounts;
			this.OrderBy = entity.OrderBy;
		}

		public void PopulateTo(MetaShare.Common.Core.Entities.Pager entity)
		{
			if (entity != null)
			{
				entity.PageIndex = this.PageIndex;
				entity.PageSize = this.PageSize;
				entity.PageTotal = this.PageTotal;
				entity.TotalCounts = this.TotalCounts;
				entity.OrderBy = this.OrderBy;
			}
		}
	}

	/// <summary>
	/// Represents a single filter condition for querying.
	/// </summary>
	public class FilterCondition
	{
		/// <summary>
		/// The target field name to filter on (e.g., "Status").
		/// </summary>
		public string Field { get; set; } = string.Empty;

		/// <summary>
		/// The comparison operator (e.g., "equal", "contains", "greaterThan").
		/// </summary>
		public string Operator { get; set; } = string.Empty;

		/// <summary>
		/// The value to compare against. May be string, number, date, etc.
		/// </summary>
		public object? Value { get; set; }
	}
}
