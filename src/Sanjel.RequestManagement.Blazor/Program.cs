var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
		.AddInteractiveServerComponents();

// Register HTTP Context Accessor
builder.Services.AddHttpContextAccessor();

// Use Scrutor for assembly scanning and auto-registration
builder.Services.Scan(scan => scan
	// Scan multiple assemblies
	.FromAssemblyOf<Sanjel.RequestManagement.Repositories.IProgramRequestRepository>()
	.FromAssemblyOf<Sanjel.RequestManagement.Core.Services.ICurrentUserService>()
	.FromAssemblyOf<Sanjel.RequestManagement.Blazor.Components.App>()

	// Register by naming convention: IService -> Service, IRepository -> Repository
	.AddClasses(classes => classes
		.Where(type => type.Name.EndsWith("Service") &&
						!type.Name.EndsWith("DataService") &&
						 type.GetInterfaces().Any(i => i.Name == $"I{type.Name}"))) // Only services WITH interfaces
	.AsMatchingInterface()
	.WithScopedLifetime()

	.AddClasses(classes => classes
		.Where(type => type.Name.EndsWith("Repository")))
	.AsMatchingInterface()
	.WithScopedLifetime()

	// Register all classes that implement IRepository<T>
	.AddClasses(classes => classes
		.AssignableTo(typeof(Sanjel.RequestManagement.Repositories.Common.IRepository<>)))
	.AsImplementedInterfaces()
	.WithScopedLifetime()

	// Register concrete Service classes WITHOUT interfaces
	.AddClasses(classes => classes
		.Where(type => type.Name.EndsWith("Service") &&
						!type.Name.EndsWith("DataService") &&
						 !type.GetInterfaces().Any(i => i.Name == $"I{type.Name}"))) // Only services WITHOUT interfaces
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
app.MapRazorComponents<Sanjel.RequestManagement.Blazor.Components.App>()
		.AddInteractiveServerRenderMode();

app.Run();
