using Entity = {{EntityClassFullPath}};
using IDataService = {{EntityDataServiceInterfaceFullPath}};

namespace Sanjel.{{ProjectName}}.Repositories
{
	public interface I{{EntityName}}Repository : Sanjel.{{ProjectName}}.Repositories.Common.IRepository<Entity>
	{
	}

	public sealed class {{EntityName}}Repository : Sanjel.{{ProjectName}}.Repositories.Common.CommonRepository<Entity, IDataService>, I{{EntityName}}Repository
	{
		public {{EntityName}}Repository(IDataService dataService)
			: base(dataService)
		{
		}
	}
}