using Sanjel.RequestManagement.Entities.Data;
using Sanjel.RequestManagement.Entities.Entities;
using Sanjel.RequestManagement.Repositories.Common;

namespace Sanjel.RequestManagement.Repositories;

/// <summary>
/// Repository adapter for ReviewPackage entities.
/// Delegates all operations to the Entities.Data layer.
/// </summary>
public class ReviewPackageRepository : BaseRepository<ReviewPackage>, IReviewPackageRepository
{
	public ReviewPackageRepository(IReviewPackageDataAccess dataAccess)
		: base(dataAccess)
	{
	}
}
