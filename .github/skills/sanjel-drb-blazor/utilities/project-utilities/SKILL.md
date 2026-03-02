# Project Utilities

## Description
Provides common utility functions used across multiple code generation skills. Centralizes project structure detection, code formatting, and common string manipulation functions to avoid code duplication.

## When To Use
- When other skills need to detect project root directory and structure
- When code formatting is required after code generation
- When common string transformations are needed (PascalCase, camelCase, etc.)
- When standardized file system operations are required

## Usage
This utility skill is imported and used by other skills rather than being run directly. Other skills import the utility functions they need.

## Available Functions

### Project Structure Detection
- `detectProjectRoot()` - Find project root by locating .slnx files
- `detectProjectInfo()` - Get project name, root path, and solution file info
- `constructMigrationPath()` - Build standard migration directory path
- `constructEntityPath()` - Build standard entity classes directory path
- `constructRepositoryPath()` - Build standard repository classes directory path

### Code Formatting
- `formatGeneratedCode(outputDir)` - Format C# code using dotnet format
- `formatSpecificFiles(filePaths)` - Format specific files only

### String Utilities
- `toPascalCase(str)` - Convert to PascalCase (EntityName)
- `toCamelCase(str)` - Convert to camelCase (entityName)
- `toKebabCase(str)` - Convert to kebab-case (entity-name)
- `pluralize(str)` - Convert singular to plural form
- `singularize(str)` - Convert plural to singular form

### File System Utilities
- `ensureDirectoryExists(path)` - Create directory if it doesn't exist
- `findFiles(pattern, dir)` - Find files matching a pattern
- `readJsonFile(path)` - Read and parse JSON file safely
- `writeJsonFile(path, data)` - Write JSON file with formatting

## Input
- Various parameters depending on the utility function being called
- Most functions work with current working directory context

## Output
- Utility functions return specific data types (strings, objects, booleans)
- No direct file generation (used by other skills)

## Script Execution
```bash
# Not run directly - imported by other skills
# Example import in other skills:
# import { detectProjectRoot, formatGeneratedCode, toPascalCase } from '../../../utilities/project-utilities/scripts/utilities.ts';
```