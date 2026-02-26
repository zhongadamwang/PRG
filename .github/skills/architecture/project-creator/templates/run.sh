#! /bin/bash

# Clean build artifacts from all projects
find ./src -type d -name "obj" -exec rm -rf {} +
find ./src -type d -name "bin" -exec rm -rf {} +

# Build and run the Blazor server in watch mode
echo "Starting Prg.ProjectName.Blazor in development watch mode..."
dotnet watch --project ./src/app/Prg.ProjectName.Blazor/Prg.ProjectName.Blazor.csproj
