# Domain Model Parser

## Description
Parse domain model documents and extract structured metadata from Mermaid class diagrams, entity definitions, and relationships. Converts markdown-based domain models into structured JSON format for use by code generation skills.

## When To Use
- When starting a new project and need to extract structured data from domain model documentation
- When domain model documents are complete and ready for code generation
- When need to validate domain model structure and relationships
- Before running any code generation skills that require entity metadata
- When updating domain models and need to regenerate the structured metadata
- As the first step in automated workflow orchestration

## Usage
This skill uses bun to run TypeScript scripts for parsing domain model documents and extracting structured metadata.

## Input
- Domain model markdown file path (typically from orgModel directory)
- Optional: specific entity names to focus parsing on
- Optional: output file path for generated JSON metadata

## Output
- Structured JSON metadata containing:
  - Entity definitions with attributes and types
  - Relationship mappings between entities
  - Enumeration definitions and values
  - Attribute constraints and validation rules
- Parsing validation report
- Entity relationship graph data

## Script Execution
```bash
bun run scripts/parse-domain-model.ts
```

## Dependencies
- Node.js built-in modules only (fs, path, process)
- No external dependencies allowed
- Compatible with bun runtime environment

## Error Handling
- Validates Mermaid syntax before parsing
- Reports missing or malformed entity definitions
- Identifies relationship inconsistencies
- Provides detailed error locations and suggestions