using {{entityUsing}};
using {{modelUsing}};
using System.Linq.Expressions;

namespace {{namespace}}.Repositories.{{featureName}}
{
	/// <summary>
	/// Repository interface for {{entityName}} entity operations
	/// Provides CRUD operations and domain-specific queries through BaseRepository inheritance
	/// </summary>
	public interface I{{pascalEntityName}}Repository : Common.IRepository<{{pascalEntityName}}Entity, {{pascalEntityName}}Model>
	{
		// Add custom domain-specific methods here as needed
		// Example: Task<IReadOnlyList<{{pascalEntityName}}Model>> ListByFeatureAsync(int featureId, CancellationToken ct = default);
	}

	/// <summary>
	/// Repository implementation for {{entityName}} entity
	/// Inherits from CommonRepository for standard CRUD operations with mapping and validation
	/// </summary>
	public sealed class {{pascalEntityName}}Repository : Common.CommonRepository<{{pascalEntityName}}Entity, {{pascalEntityName}}Model, {{dataServiceInterface}}>, I{{pascalEntityName}}Repository
	{
		/// <summary>
		/// Initializes a new instance of the {{pascalEntityName}}Repository
		/// </summary>
		/// <param name="dataService">MetaShare data service for {{entityName}} operations</param>
		/// <param name="mappingService">Service for entity-to-model mapping</param>
		/// <param name="validationService">Service for model validation</param>
		public {{pascalEntityName}}Repository(
			{{dataServiceInterface}} dataService,
			Core.Services.IMappingService mappingService,
			Core.Services.IValidationService validationService)
			: base(dataService, mappingService, validationService)
		{
		}

		// Implement custom domain-specific methods here
		// Example:
		// public async Task<IReadOnlyList<{{pascalEntityName}}Model>> ListByFeatureAsync(int featureId, CancellationToken ct = default)
		// {
		//     Expression<Func<{{pascalEntityName}}Entity, bool>> filter = e => e.FeatureId == featureId;
		//     return await this.ListAsync(filter, ct).ConfigureAwait(false);
		// }
	}
}