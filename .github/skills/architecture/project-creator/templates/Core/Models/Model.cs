namespace Sanjel.eServiceCloud.Core.Models
{
	public class Model<TEntity> : IModel
	where TEntity : MetaShare.Common.Core.Entities.Common, new()
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public int SystemId { get; set; }

		public TEntity BaseToMdm()
		{
			var entity = new TEntity()
			{
				Id = this.Id,
				Name = this.Name,
				Description = this.Description,
			};
			if (entity is MetaShare.Common.Core.Entities.ObjectVersion objectVersion)
			{
				objectVersion.SystemId = this.SystemId;
			}

			return entity;
		}
	}
}
