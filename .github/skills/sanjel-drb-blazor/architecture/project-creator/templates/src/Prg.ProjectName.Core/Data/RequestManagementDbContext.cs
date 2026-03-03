using Microsoft.EntityFrameworkCore;

namespace Prg.ProjectName.Core.Data;

/// <summary>
/// EF Core DbContext for Request Management application
/// </summary>
public class ProjectNameDbContext : DbContext
{
	public ProjectNameDbContext(DbContextOptions<ProjectNameDbContext> options)
		: base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// Apply entity configurations from generated files
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProjectNameDbContext).Assembly);
	}
}
