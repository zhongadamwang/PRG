---
name: repository-creator
description: Creates repository pattern implementation with interface and concrete class following eServiceCloud patterns. Includes CRUD operations through BaseRepository, custom query extensions, and dependency injection setup.
---

# Repository Creator

## Overview

Creates data access layer repositories following eServiceCloud patterns with clean architecture. Generates repository interfaces and implementations that inherit from BaseRepository, provide CRUD operations, support custom query extensions, and integrate with dependency injection.

## When to Use

Use this skill when:
- Creating new data access repositories for entities
- Need standardized CRUD operations with mapping and validation  
- Want to extend repositories with custom query methods
- Require proper separation between data and business layers
- Need repositories that follow eServiceCloud patterns
- Want repositories with proper dependency injection setup

## Features

- **BaseRepository Integration**: Inherits from eServiceCloud BaseRepository with standard CRUD operations
- **Interface Implementation**: Creates IRepository<TEntity, TModel> compliant interfaces
- **Custom Query Extensions**: Support for domain-specific query methods
- **Mapping & Validation**: Automatic entity-to-model mapping with validation
- **Dependency Injection**: Proper Scoped registration for service layer usage
- **Async/Await Support**: Full async support with CancellationToken
- **Error Handling**: Consistent error handling through base repository

## Input Requirements

### Required Parameters

- **Entity Name**: The domain entity name (e.g., "ProgramRequest", "JobType")
- **Feature Area**: The feature directory name for organization
- **Data Service**: The MetaShare data service interface to use

### Optional Parameters

- **Model Name**: The corresponding model/DTO class name (default: EntityNameModel)
- **Namespace**: Custom namespace (default: derived from project)
- **Custom Methods**: List of custom query methods to generate

### Interactive Prompts

The skill will ask for:

1. **Entity Details**: Entity name and MetaShare data service
2. **Feature Organization**: Which feature area this repository belongs to
3. **Custom Methods**: Any domain-specific query methods needed

## Generated Files

### Repository Implementation
```
src/app/[ProjectName].Repositories/[Feature]/[EntityName]Repository.cs
```
Contains both interface and implementation in a single file for easier management.

### Creation Script
```
scripts/create-repository.ts
```
TypeScript script to generate repository files using bun runtime.

## Usage Examples

### Basic Repository Generation
```bash
# Generate basic CRUD repository  
gh copilot suggest "Create ProgramRequest repository using repository-creator skill"
```

### Repository with Custom Methods
```bash
# Generate repository with custom query methods
gh copilot suggest "Create JobType repository with GetByJobFamilyId method using repository-creator"
```

## Template Structure

The repository creator generates:

1. **Combined Template**: Interface and implementation in single .cs file
2. **TypeScript Script**: Bun-powered generation script
3. **Documentation**: README with usage instructions
4. **Examples**: Sample custom query implementations

## Architecture Compliance

Generated repositories follow eServiceCloud patterns:

- **Single Responsibility**: Pure data access, no business logic
- **Dependency Injection**: Scoped lifetime, service layer access only
- **Clean Architecture**: Proper layer separation and dependency flow
- **Async Operations**: Full async/await with cancellation support
- **Error Boundaries**: Repository-level error handling without UI coupling
- **Testability**: Interface-based design for easy mocking and testing

## Best Practices

1. **Naming Conventions**: Follow I[EntityName]Repository and [EntityName]Repository naming
2. **Feature Organization**: Group repositories by business feature areas
3. **Custom Methods**: Keep domain queries in repositories, business rules in services
4. **Service Layer Usage**: Only inject repositories into application services
5. **Validation**: Let BaseRepository handle mapping and validation concerns

## Related Skills

- **project-creator**: Required foundation for repository structure
- **service-creator**: Application services that consume repositories
- **viewmodel-creator**: Models that repositories map to
- **unit-test-creator**: Test coverage for repository methods

---

This skill creates production-ready repositories following eServiceCloud clean architecture patterns with full CRUD capabilities, custom query support, and proper dependency management.