var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
		.AddInteractiveServerComponents();

// Register HTTP Context Accessor
builder.Services.AddHttpContextAccessor();

var assemblies = new[]
{
	typeof({Prg}.{ProjectName}.Repositories.Common.IRepository<>).Assembly,  // Repositories
	typeof({Prg}.{ProjectName}.Core.Services.ICurrentUserService).Assembly, // Core
	typeof({Prg}.{ProjectName}.Blazor.App).Assembly,                          // Blazor
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
app.MapRazorComponents<{Prg}.{ProjectName}.Blazor.App>()
		.AddInteractiveServerRenderMode();

app.Run();
