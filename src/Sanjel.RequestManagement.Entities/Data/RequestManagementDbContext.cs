using Microsoft.EntityFrameworkCore;
using Sanjel.RequestManagement.Entities.Entities;

namespace Sanjel.RequestManagement.Entities.Data;

/// <summary>
/// EF Core DbContext for request management application with complete entity framework setup
/// </summary>
public class RequestManagementDbContext : DbContext
{
	/// <summary>
	/// Initializes a new instance of the RequestManagementDbContext class
	/// </summary>
	/// <param name="options">The DbContext options</param>
	public RequestManagementDbContext(DbContextOptions<RequestManagementDbContext> options)
		: base(options)
	{
	}

	// Entity DbSets

	/// <summary>
	/// Gets or sets the Request entities
	/// Program request submitted for engineering work with complete lifecycle tracking
	/// </summary>
	public DbSet<Request> Requests { get; set; }

	/// <summary>
	/// Gets or sets the DataElement entities
	/// </summary>
	public DbSet<DataElement> DataElements { get; set; }

	/// <summary>
	/// Gets or sets the StateDiagram entities
	/// </summary>
	public DbSet<StateDiagram> StateDiagrams { get; set; }

	/// <summary>
	/// Gets or sets the ReviewPackage entities
	/// </summary>
	public DbSet<ReviewPackage> ReviewPackages { get; set; }

	/// <summary>
	/// Gets or sets the Notification entities
	/// System-generated communication to stakeholders about request status and actions
	/// </summary>
	public DbSet<Notification> Notifications { get; set; }

	/// <summary>
	/// Configures the entity model using Fluent API configurations
	/// </summary>
	/// <param name="modelBuilder">The model builder</param>
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// Apply all entity configurations from the Configuration assembly
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(RequestManagementDbContext).Assembly);
	}

	/// <summary>
	/// Configures the database connection and options
	/// </summary>
	/// <param name="optionsBuilder">The options builder</param>
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);

		// Additional configuration can be added here if needed
		// Note: Connection string should be configured in dependency injection
	}
}
