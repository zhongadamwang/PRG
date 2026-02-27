---
name: eServiceCloud Blazor Project Creator
description: Creates .NET Blazor Server projects by copying and customizing template files from the templates/ directory with proper naming and namespace replacement.
---

# eServiceCloud Blazor Project Creator

## Overview

🏗️ **Template-Based Project Creator** 🏗️

This skill creates .NET Blazor Server projects by copying all files from the `templates/` directory to a new location, renaming directories/files from `Prg.ProjectName.*` to your specified project name, and replacing all namespace references in the code.

## When To Use

Use this skill when:
- **User wants to create/initialize projects** - Need to quickly generate new project structure from templates
- **Creating new Blazor projects** - Need a standard project architecture and directory structure
- **Starting enterprise applications** - Want proven architecture foundation to begin development
- **Project standardization** - Establishing consistent project patterns and standards across teams
- **Quick project setup** - Avoid manual project creation steps

## Usage

**⚠️ CRITICAL: This skill requires AI to handle user interaction and then execute the TypeScript script with parameters.**

### Execution Process

**Step 1: AI Gathers User Input**
⚠️ **CRITICAL: AI MUST ALWAYS prompt user for these inputs - DO NOT skip this step!**

The AI MUST prompt the user for:
- **Project Name** (e.g., "RequestManagement", "eServiceCloud")
- **Project Prefix** (e.g., "Sanjel", "Prg") 
- **Target Directory** (absolute path to project root directory, defaults to current workspace directory, eg., `/sanjel/project`)

**Step 2: AI Executes Script with Parameters**
```bash
# Navigate to skill directory
cd .github/skills/architecture/project-creator

# Execute script with user-provided parameters
bun run create-project.ts <projectName> <projectPrefix> <targetDirectory>

# Example (creates project in specified root directory):
bun run create-project.ts RequestManagement Sanjel /sanjel/project
# This creates: /sanjel/project/src/Sanjel.RequestManagement.*
```

**🚨 AI Requirements:**
- **MUST ALWAYS ask user for project details BEFORE executing script - NO EXCEPTIONS**
- **NEVER skip user input prompting step**
- **NEVER attempt manual file operations**
- **ALWAYS use the script with parameters**

## What the Script Does

1. **Copy Templates** - Copies entire `templates/` directory structure to target location
2. **Rename Directories** - Changes `Prg.ProjectName.*` folders to `[Prefix].[ProjectName].*`
3. **Rename Files** - Updates `.csproj`, `.slnx` and other project files with new names
4. **Replace Content** - Updates all `Prg.ProjectName` references in code to your project name
5. **Preserve Structure** - Maintains all file relationships and dependencies

## Template Structure

The `templates/` directory contains a complete project with:
- **.NET Solution** - Multi-project solution with Blazor, Core, BusinessProcess, and Repository layers
- **Test Projects** - Corresponding test projects for each layer
- **Configuration Files** - Build scripts, editor config, git settings
- **VS Code Setup** - Debug configuration and recommended extensions
- **Shell Scripts** - Execution and build automation scripts

⚠️ **IMPORTANT**: All files in the `templates/` directory are template files, including `run.sh` and all other scripts. These templates use `Prg.ProjectName` as placeholder names that get replaced with your actual project name during creation.
