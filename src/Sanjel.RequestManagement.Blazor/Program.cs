using Microsoft.EntityFrameworkCore;
using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
		.AddInteractiveServerComponents();

// Add Syncfusion Blazor service
builder.Services.AddSyncfusionBlazor();
var syncfusionLicenseKey = builder.Configuration["Blazor:SyncfusionLicenseKey"];
Console.WriteLine($"Syncfusion License Key loaded: {syncfusionLicenseKey}");
if (!string.IsNullOrEmpty(syncfusionLicenseKey))
{
	Console.WriteLine($"Registering Syncfusion license (length: {syncfusionLicenseKey.Length})");
	Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncfusionLicenseKey);
	Console.WriteLine("Syncfusion license registered successfully");
}
else
{
	Console.WriteLine("WARNING: Syncfusion license key is empty or null!");
}

// Register HTTP Context Accessor
builder.Services.AddHttpContextAccessor();

var useMockData = builder.Configuration.GetValue<bool>("UseMockData");
var connectionString = builder.Configuration["SanjelMdm:DbConnectionString"]
	?? builder.Configuration.GetConnectionString("SanjelMdm");

// 添加调试信息
Console.WriteLine($"=== DATABASE DEBUG INFO ===");
Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");
Console.WriteLine($"UseMockData: {useMockData}");
Console.WriteLine($"Connection String: {connectionString}");
Console.WriteLine($"================================");

if (!useMockData && string.IsNullOrWhiteSpace(connectionString))
{
	throw new InvalidOperationException("Database connection string 'SanjelMdm:DbConnectionString' is required when UseMockData is false.");
}

builder.Services.AddDbContext<Sanjel.RequestManagement.Entities.Data.RequestManagementDbContext>(options =>
{
	if (!string.IsNullOrWhiteSpace(connectionString))
	{
		options.UseSqlServer(connectionString);
	}
});

var assemblies = new[]
{
	typeof(Sanjel.RequestManagement.Repositories.Common.IRepository<>).Assembly,  // Repositories
	typeof(Sanjel.RequestManagement.Entities.Data.IDataAccess<>).Assembly,         // Entities/DataAccess
	typeof(Sanjel.RequestManagement.Core.Services.ICurrentUserService).Assembly, // Core
	typeof(Sanjel.RequestManagement.Blazor.App).Assembly,                          // Blazor
};

// Use Scrutor for assembly scanning and auto-registration
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

if (useMockData)
{
	builder.Services.AddScoped<Sanjel.RequestManagement.Repositories.Data.IRequestRepository, Sanjel.RequestManagement.Repositories.Data.MockRequestRepository>();
	Console.WriteLine(">>> Mock Repository enabled for development/testing");
}

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
