using Microsoft.EntityFrameworkCore;
using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
		.AddInteractiveServerComponents();

// Add Syncfusion Blazor service
builder.Services.AddSyncfusionBlazor();

// Register Syncfusion license (if applicable)
// Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("LICENSE_KEY");

// Register HTTP Context Accessor
builder.Services.AddHttpContextAccessor();

var connectionString = builder.Configuration.GetConnectionString("SanjelMdm:DbConnectionString")
	?? builder.Configuration["SanjelMdm:DbConnectionString"];

// Register RequestManagementDbContext
builder.Services.AddDbContext<Sanjel.RequestManagement.Entities.Data.RequestManagementDbContext>(options =>
{
	options.UseSqlServer(connectionString);
});

// Auto-register project dependencies using convention-based scanning
var assemblies = new[]
{
	typeof(Sanjel.RequestManagement.Repositories.Common.IRepository<>).Assembly,  // Repositories
	typeof(Sanjel.RequestManagement.Entities.Data.IDataAccess<>).Assembly,        // Entities/Data
	typeof(Sanjel.RequestManagement.Core.Services.ICurrentUserService).Assembly, // Core
	typeof(Sanjel.RequestManagement.Blazor.App).Assembly,                          // Blazor
};

builder.Services.Scan(scan => scan
	.FromAssemblies(assemblies)

	// Register DataAccess layer first (base dependencies)
	.AddClasses(classes => classes
		.Where(type =>
			type.Name.EndsWith("DataAccess") &&
			type.IsClass &&
			!type.IsAbstract))
	.AsMatchingInterface()
	.WithScopedLifetime()

	// Register DataAccess by all implemented interfaces (including base interfaces)
	.AddClasses(classes => classes
		.Where(type =>
			type.Name.EndsWith("DataAccess") &&
			type.IsClass &&
			!type.IsAbstract))
	.AsImplementedInterfaces()
	.WithScopedLifetime()

	// Register Repository layer (depends on DataAccess)
	.AddClasses(classes => classes
		.Where(type =>
			type.Name.EndsWith("Repository") &&
			type.IsClass &&
			!type.IsAbstract))
	.AsMatchingInterface()
	.WithScopedLifetime()

	// Register Repository by all implemented interfaces (including IRepository<T>)
	.AddClasses(classes => classes
		.Where(type =>
			type.Name.EndsWith("Repository") &&
			type.IsClass &&
			!type.IsAbstract))
	.AsImplementedInterfaces()
	.WithScopedLifetime()

	// Register Service layer with matching interfaces
	.AddClasses(classes => classes
		.Where(type =>
			type.Name.EndsWith("Service") &&
			!type.Name.EndsWith("DataService") &&
			type.IsClass &&
			!type.IsAbstract &&
			type.GetInterfaces().Any(i => i.Name == $"I{type.Name}")))
	.AsMatchingInterface()
	.WithScopedLifetime()

	// Register Service classes without matching interfaces (as self)
	.AddClasses(classes => classes
		.Where(type =>
			type.Name.EndsWith("Service") &&
			!type.Name.EndsWith("DataService") &&
			type.IsClass &&
			!type.IsAbstract &&
			!type.GetInterfaces().Any(i => i.Name == $"I{type.Name}")))
	.AsSelf()
	.WithScopedLifetime()
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
	app.UseHttpsRedirection();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<Sanjel.RequestManagement.Blazor.App>()
		.AddInteractiveServerRenderMode();

app.Run();
