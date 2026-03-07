---
name: solution-code-formatter
description: Format all code in the entire solution using dotnet format commands. Provides comprehensive code formatting across all projects in a solution, ensuring consistent code style and formatting standards with error handling and selective formatting capabilities.
---

# Solution Code Formatter ✅ **🔧 Script-Driven**

**Responsibility**: Format all code in the entire solution using dotnet format commands
**Input**: Solution file path (.slnx)
**Output**: Formatted code across entire solution

**Script**: `format-solution.ts` (run with bun)
- Execute `dotnet format` on the entire solution
- Execute `dotnet format style` for comprehensive style formatting
- Provide formatting status and results
- Handle formatting errors gracefully
- Support selective formatting by project if needed

## Description
Format all code in the entire solution using dotnet format commands. This skill provides comprehensive code formatting across all projects in a solution, ensuring consistent code style and formatting standards.

## When To Use
- Formatting entire solution codebase
- Ensuring consistent code style across all projects
- Preparing code for code review or deployment
- Standardizing code format after code generation
- Running as part of CI/CD pipeline for code quality

## Key Features
- **Comprehensive Formatting**: Formats all code in the solution
- **Multiple Format Types**: Supports both standard and style formatting
- **Error Handling**: Graceful handling of formatting errors
- **Selective Formatting**: Option to format specific projects
- **Status Reporting**: Detailed formatting results and status

## Usage
This skill formats all code in a solution using dotnet format tools.

## Input
- **Solution File**: Path to .slnx solution file
- **Format Type**: Optional specification of format type (standard, style, all)

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