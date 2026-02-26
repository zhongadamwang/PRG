# Sanjel PRG (Program Request Management)

A comprehensive Program Request Management system built with .NET 10 and Blazor Server-Side, designed to streamline program request workflows with integrated collaboration, domain modeling, and process management capabilities.

## Quick Start

### Prerequisites
- .NET 10 SDK
- Visual Studio Code or Visual Studio 2022+
- Git

### Running the Application

1. **Clone and navigate to the project:**
   ```bash
   git clone <repository-url>
   cd sanjel/PRG
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore src/Sanjel.PRG.slnx
   ```

3. **Build the solution:**
   ```bash
   dotnet build src/Sanjel.PRG.slnx
   ```

4. **Run the Blazor application:**
   ```bash
   dotnet run --project src/app/Sanjel.PRG.Blazor --urls "http://localhost:5002;https://localhost:5003"
   ```

5. **Open your browser:**
   - HTTP: http://localhost:5002
   - HTTPS: https://localhost:5003

### Development Commands

- **Build:** `dotnet build src/Sanjel.PRG.slnx`
- **Test:** `dotnet test src/Sanjel.PRG.slnx`
- **Watch (auto-reload):** `dotnet watch run --project src/app/Sanjel.PRG.Blazor`
- **Clean:** `dotnet clean src/Sanjel.PRG.slnx`

### VS Code Integration

The project is fully configured for VS Code development with:
- Debug configurations for Blazor Server
- Build tasks and test runners
- Recommended extensions
- IntelliSense and formatting settings

Press `F5` to start debugging or use `Ctrl+Shift+P` → "Tasks: Run Task" to access build commands.

## Architecture

### Solution Structure
- **Sanjel.PRG.Core**: Domain models, entities, and business logic
- **Sanjel.PRG.BusinessProcess**: Business process implementations and workflows
- **Sanjel.PRG.Repositories**: Data access layer and repository patterns
- **Sanjel.PRG.Blazor**: Web UI built with Blazor Server-Side
- **Test Projects**: Corresponding test projects for each main project

### Technology Stack
- **.NET 10**: Latest framework features and performance
- **Blazor Server**: Interactive web UI with SignalR
- **Entity Framework Core**: Data access and ORM
- **NUnit**: Unit testing framework
- **Dependency Injection**: Built-in .NET DI container

## Project Structure

```
/sanjel/PRG/
├── README.md
├── .gitignore
├── orgModel/                          # Organizational models and processes
│   └── 01 - Program Request Management/
├── projects/                          # Project management and planning
│   └── 01 - Program Request Management/
├── src/                              # Source code
│   ├── Sanjel.PRG.slnx              # Solution file
│   └── app/                         # Application projects
│       ├── Sanjel.PRG.Blazor/       # Web UI (Blazor Server)
│       ├── Sanjel.PRG.Core/         # Domain models and business logic
│       ├── Sanjel.PRG.BusinessProcess/ # Business process implementations
│       ├── Sanjel.PRG.Repositories/ # Data access layer
│       └── *.Tests/                 # Test projects
└── .vscode/                         # VS Code configuration
    ├── launch.json                  # Debug configurations
    ├── tasks.json                   # Build tasks
    ├── settings.json                # Workspace settings
    └── extensions.json              # Recommended extensions
```

## Development Workflow

### Initial Setup
1. **Install Recommended Extensions**: VS Code will prompt to install recommended extensions
2. **Trust Developer Certificate**: Run `dotnet dev-certs https --trust` for HTTPS development
3. **Configure Database**: Update connection strings in `appsettings.Development.json`

### Daily Development
1. **Start Development Server**: Press `F5` or use `dotnet watch run`
2. **Run Tests**: Use `Ctrl+Shift+P` → "Test: Run All Tests"
3. **Debug**: Set breakpoints and use VS Code debugger
4. **Hot Reload**: Changes are automatically applied with `dotnet watch`

## Testing

All projects have corresponding test projects using NUnit:

```bash
# Run all tests
dotnet test src/Sanjel.PRG.slnx

# Run specific test project
dotnet test src/app/Sanjel.PRG.Core.Tests

# Run tests with coverage
dotnet test src/Sanjel.PRG.slnx --collect:"XPlat Code Coverage"
```

## Configuration

### Environment Settings
- **Development**: `appsettings.Development.json`
- **Production**: `appsettings.json`
- **Local**: `appsettings.Local.json` (ignored by Git)

### Key Configuration Areas
- Database connection strings
- Authentication providers
- Email service settings
- External API endpoints
- Logging configuration

## Deployment

### Local Development
```bash
dotnet run --project src/app/Sanjel.PRG.Blazor
```

### Production Build
```bash
dotnet publish src/app/Sanjel.PRG.Blazor -c Release -o ./publish
```

### Docker (Future)
Docker configuration can be added for containerized deployment.

## Contributing

1. Create a feature branch from `main`
2. Follow established code style and patterns
3. Add tests for new functionality
4. Ensure all tests pass
5. Update documentation as needed
6. Submit pull request

## Support

For questions and support:
- Check existing documentation in `/orgModel/`
- Review project tasks in `/projects/01 - Program Request Management/tasks/`
- Consult domain models and process documentation

## License

[Add appropriate license information]
