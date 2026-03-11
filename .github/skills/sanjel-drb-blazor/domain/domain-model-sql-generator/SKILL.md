---
name: domain-model-sql-generator
description: Generate SQL Server DDL scripts from domain model entity metadata. Reads the JSON output of domain-model-parser and produces an ordered CREATE TABLE script with primary keys, foreign keys, indexes, enum lookup tables, and standard audit columns for SQL Server.
---

# Domain Model SQL Generator 📋 **Planned** **🔧 Script-Driven**

**Responsibility**: Generate SQL Server DDL scripts from domain model entity metadata
**Input**: Entity metadata JSON (output of `domain-model-parser`)
**Output**: SQL Server `CREATE TABLE` scripts with constraints, indexes, and relationships

**Script**: `generate-sql-schema.ts` (run with bun)

## Description

Reads the structured JSON metadata produced by `domain-model-parser` and generates a complete, ready-to-execute SQL Server DDL script. The output respects table dependency order (parent tables are created before child tables) and follows consistent conventions for column types, constraints, and audit columns.

## When To Use

- Initial database schema creation for a new project
- Rebuilding the schema after domain model changes
- Reviewing the database structure implied by the domain model
- Providing DBA teams with a clean, reviewable schema baseline

## Key Features

- **Type Mapping**: Converts domain/C# types to SQL Server data types (e.g. `string → NVARCHAR`, `DateTime → DATETIME2`)
- **Primary Keys**: Detects `*_id` attributes and generates `PRIMARY KEY` constraints
- **Foreign Keys**: Infers FK relationships from attribute naming conventions and the relationships graph
- **Enum Lookup Tables**: Generates a `*_Lookup` table for each enum with `Code` and `Description` columns
- **Audit Columns**: Appends `CreatedAt`, `CreatedBy`, `UpdatedAt`, `UpdatedBy`, and `IsDeleted` to every entity table
- **Soft Delete**: Includes `IsDeleted BIT NOT NULL DEFAULT 0` and a filtered index on `IsDeleted = 0`
- **Dependency Ordering**: Topological sort ensures parent tables appear before child tables
- **Array Attributes**: Array-typed attributes (e.g. `specializations: string[]`) are emitted as separate junction/value tables

## Input

```
bun run scripts/generate-sql-schema.ts <metadata-json-path>
```

| Argument | Description |
|---|---|
| `metadata-json-path` | Path to the JSON file produced by `domain-model-parser` |
| `output-sql-path` | (Optional) Path to write the `.sql` file; defaults to `<metadata>.sql` |

## Output

A single `.sql` file containing:
1. `USE` / `GO` header comments
2. Enum lookup tables (`*Enum_Lookup`)
3. Entity tables in dependency order
4. Array-value tables for array-typed attributes
5. `CREATE INDEX` statements for FK columns

### Example Entity Table

```sql
CREATE TABLE [dbo].[Requests] (
    [RequestId]           NVARCHAR(64)   NOT NULL,
    [Status]              NVARCHAR(64)   NOT NULL,
    [CreatedDate]         DATETIME2      NOT NULL,
    [Priority]            NVARCHAR(64)   NOT NULL,
    [ClientId]            NVARCHAR(64)   NOT NULL,
    [SourceEmail]         NVARCHAR(256)  NOT NULL,
    [AssignedEngineerId]  NVARCHAR(64)       NULL,
    [AssignedBy]          NVARCHAR(64)       NULL,
    [AcknowledgmentDate]  DATETIME2          NULL,
    [CompletionDate]      DATETIME2          NULL,
    -- Audit columns
    [CreatedAt]           DATETIME2      NOT NULL  DEFAULT GETUTCDATE(),
    [CreatedBy]           NVARCHAR(256)  NOT NULL  DEFAULT N'',
    [UpdatedAt]           DATETIME2          NULL,
    [UpdatedBy]           NVARCHAR(256)      NULL,
    [IsDeleted]           BIT            NOT NULL  DEFAULT 0,
    CONSTRAINT [PK_Requests] PRIMARY KEY ([RequestId])
);
```

### Example Enum Lookup Table

```sql
CREATE TABLE [dbo].[StatusEnum_Lookup] (
    [Code]        NVARCHAR(64)   NOT NULL,
    [Description] NVARCHAR(256)      NULL,
    CONSTRAINT [PK_StatusEnum_Lookup] PRIMARY KEY ([Code])
);
```

## Type Mapping Reference

| Domain / C# Type | SQL Server Type |
|---|---|
| `string` (id/key) | `NVARCHAR(64)` |
| `string` (email/name) | `NVARCHAR(256)` |
| `string` (content/notes/summary) | `NVARCHAR(MAX)` |
| `string` (default) | `NVARCHAR(256)` |
| `int` | `INT` |
| `float` | `FLOAT` |
| `decimal` | `DECIMAL(18, 2)` |
| `bool` | `BIT` |
| `DateTime` | `DATETIME2` |
| `*enum` types | `NVARCHAR(64)` |
| `guid` / `Guid` | `UNIQUEIDENTIFIER` |

## Script Execution

```bash
# Generate SQL from domain model metadata
bun run scripts/generate-sql-schema.ts \
    "/sanjel/PRG/orgModel/01 - Program Request Management/domain-model-metadata.json"
```

## Dependencies

- Node.js built-in modules only (`fs`, `path`)
- No external dependencies allowed
- Compatible with bun runtime environment

## Error Handling

- Validates that the input JSON exists and is parseable
- Reports missing primary key candidates with a warning (falls back to `Id` column)
- Warns on circular FK dependencies before sorting
- Exits with code 1 on critical errors; exits with code 0 on success
