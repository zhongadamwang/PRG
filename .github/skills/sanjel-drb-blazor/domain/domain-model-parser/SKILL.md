---
name: domain-model-parser
description: Parse domain model documents and extract structured metadata for code generation. Analyzes Mermaid class diagrams and domain model documentation to produce structured JSON metadata with entities, relationships, enums, and attributes for downstream processing.
---

# Domain Model Parser ✅ **Finished** **🔧 Script-Driven**

**Responsibility**: Parse domain model documents and extract structured metadata
**Input**: Domain model markdown files
**Output**: JSON format parsing results

**Script**: `parse-domain-model.ts` (run with bun)
- Parse Mermaid class diagrams
- Extract entity definitions and attributes
- Analyze entity relationships
- Generate structured metadata

```json
{
  "entities": [...],
  "relationships": [...],
  "enums": [...],
  "attributes": [...]
}
```

## Description
Parse domain model documents and extract structured metadata for code generation. This skill analyzes Mermaid class diagrams and domain model documentation to produce structured JSON metadata that serves as input for other code generation skills.

## When To Use
- Converting domain model documentation to structured data
- Extracting entities, relationships, and attributes from domain models
- Preparing domain model data for code generation workflows
- Analyzing Mermaid class diagrams for entity extraction

## Key Features
- **Mermaid Diagram Parsing**: Extract entities and relationships from Mermaid class diagrams
- **Attribute Analysis**: Detailed attribute extraction with type information
- **Relationship Mapping**: Complete relationship analysis including cardinalities
- **Structured Output**: Clean JSON format for downstream processing
- **Enum Extraction**: Identification and parsing of enumeration types

## Usage
This skill parses domain model markdown files and produces structured metadata for code generation.

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