# Solution Code Formatter

## Description
Format all code in the entire solution using dotnet format commands. Provides comprehensive code formatting for C# projects using both basic formatting and style formatting to ensure consistent code style across the entire solution.

## When To Use
- After code generation skills complete their work
- When consistent code formatting is needed across entire solution
- Before committing generated code to version control
- During continuous integration processes
- When migrating between different coding standards
- After bulk code modifications or imports

## Usage
This skill uses bun to run TypeScript scripts for executing dotnet format commands on the solution.

## Input
- Solution file path (.slnx) - Auto-detected from project root if not provided
- Optional: Specific project paths for selective formatting
- Optional: Formatting options and severity levels

## Output
- Formatted code across entire solution
- Formatting status report with success/failure counts
- List of files that were modified during formatting
- Error reports for files that couldn't be formatted
- Summary statistics of formatting changes

## Script Execution
```bash
bun run scripts/format-solution.ts [solution-path] [options]
```

## Features
- **Comprehensive Formatting**: Executes both `dotnet format` and `dotnet format style`
- **Auto-Detection**: Automatically finds .slnx solution files in project
- **Error Handling**: Gracefully handles formatting errors with detailed reporting
- **Selective Formatting**: Supports formatting specific projects when needed
- **Status Reporting**: Provides detailed feedback on formatting operations
- **Performance Optimized**: Formats entire solution in single operations
- **CI/CD Ready**: Suitable for automated build processes

## Commands Executed
1. `dotnet format <solution-path>` - Basic code formatting
2. `dotnet format style <solution-path>` - Comprehensive style formatting

## Error Handling
- Continues on individual file formatting failures
- Reports formatting errors without failing entire operation  
- Provides detailed error messages for troubleshooting
- Maintains formatting operation logs