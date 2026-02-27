#! /bin/bash

# Clean build artifacts from all projects
find ./src -type d -name "obj" -exec rm -rf {} +
find ./src -type d -name "bin" -exec rm -rf {} +

# Build and run the Blazor server in watch mode
echo "Starting Sanjel.RequestManagement.Blazor in development watch mode..."
dotnet watch --project ./src/Sanjel.RequestManagement.Blazor/Sanjel.RequestManagement.Blazor.csproj
