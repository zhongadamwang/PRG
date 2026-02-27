namespace Sanjel.RequestManagement.Core.Common
{
	public class PagerResult<TEntity>
		where TEntity : MetaShare.Common.Core.Entities.Common, new()
	{
		public PagerResult()
		{
			this.Result = new List<TEntity>();
			this.Pager = new Pager();
		}

		public IEnumerable<TEntity> Result { get; set; }

		public Pager Pager { get; set; }
	}
}
