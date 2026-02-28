# Skills Development Guidelines

## Overview

This document provides specific development guidelines and constraints for creating Skills scripts in the sanjel-drb-blazor project. All Skills must follow these standards to ensure consistency, maintainability, and proper execution within the VS Code Copilot environment.

## TypeScript Script Requirements

### Runtime Environment
- **Runtime**: All scripts must use `bun` runtime (not Node.js directly)
- **Language**: TypeScript only, no JavaScript
- **Execution**: Scripts are executed via `bun run <script-name>.ts`

### Import and Module Constraints

#### Node.js Modules Only
```typescript
// ✅ ALLOWED - Node.js built-in modules only
// @ts-ignore
import { readFileSync, writeFileSync, existsSync } from 'node:fs';
// @ts-ignore
import { join, dirname, basename } from 'node:path';
// @ts-ignore
import { execSync } from 'node:child_process';

// ❌ FORBIDDEN - Third-party modules
// import lodash from 'lodash';
// import axios from 'axios';
// import express from 'express';
```

#### Required Import Format
- **MUST** use `node:` prefix for all Node.js modules
- **MUST** add `// @ts-ignore` before each import statement
- **MUST** only use Node.js built-in modules (fs, path, process, child_process, etc.)

#### Global Variables and Special Properties
```typescript
// @ts-ignore
const process = globalThis.process;

// @ts-ignore
if (import.meta.main) {
  main();
}
```
Add `// @ts-ignore` before accessing:
- Node.js globals if TypeScript cannot find the type definition
- **import.meta.main** property which also causes type errors in TypeScript

### Programming Paradigm

#### Functional Programming Required
```typescript
// ✅ PREFERRED - Functional approach
function parseEntity(data: string): Entity {
  return {
    id: generateId(data),
    name: extractName(data),
    attributes: parseAttributes(data)
  };
}

function processEntities(entities: Entity[]): ProcessedEntity[] {
  return entities.map(entity => transformEntity(entity));
}

// ❌ AVOID - Class-based approach
class EntityParser {
  private data: string;
  
  constructor(data: string) {
    this.data = data;
  }
  
  parse(): Entity {
    // ...
  }
}
```

#### Function Characteristics
- **Pure Functions**: Prefer pure functions with no side effects
- **Immutability**: Avoid mutating input parameters
- **Single Responsibility**: Each function should have one clear purpose
- **Composability**: Design functions to be easily combined and reused

### Code Structure Standards

#### File Organization
```typescript
// 1. Type definitions and interfaces first
interface EntityData {
  id: string;
  name: string;
}

// 2. Utility functions
function normalizeString(input: string): string {
  return input.trim().toLowerCase();
}

// 3. Core processing functions
function parseInput(content: string): ParsedData {
  // ...
}

// 4. Main entry point
function main(): void {
  // ...
}

// 5. Execution guard
// @ts-ignore
if (import.meta.main) {
  main();
}
```

#### Error Handling
```typescript
function main(): void {
  try {
    const args = process.argv.slice(2);
    
    if (args.length === 0) {
      console.error('❌ Usage: bun run script.ts <required-arg> [optional-arg]');
      process.exit(1);
    }

    // Process logic here
    
  } catch (error) {
    console.error('❌ Error occurred:', error);
    process.exit(1);
  }
}
```

#### Console Output Standards
- Use emoji prefixes for better readability:
  - `🔍` - Processing/analyzing
  - `✅` - Success
  - `❌` - Error
  - `📖` - Reading input
  - `💾` - Writing output
  - `📊` - Statistics/summary
  - `🎉` - Completion

## Skill Architecture Principles

### Single Responsibility Principle
Each skill must focus on **one specific task only**:

```typescript
// ✅ GOOD - Single purpose
// domain-model-parser: Parse domain models into JSON metadata
// entity-class-generator: Generate C# entity classes from JSON metadata
// database-migration-generator: Create EF Core migrations from entity metadata

// ❌ BAD - Multiple purposes
// super-generator: Parse domain models AND generate classes AND create migrations
```

### Input/Output Conventions

#### File Path Handling
```typescript
function main(): void {
  const args = process.argv.slice(2);
  const inputPath = args[0];
  const outputPath = args[1] || generateDefaultOutputPath(inputPath);
}

function generateDefaultOutputPath(inputPath: string): string {
  return inputPath.replace('.md', '-metadata.json');
}
```

#### JSON Output Format
```typescript
interface StandardMetadata {
  version: string;            // Semantic version
  generatedAt: string;        // ISO timestamp
  sourceFile: string;         // Input file path
  // ... specific data
  statistics: {
    // Summary counts
  };
}
```

### Documentation Requirements

#### Skill Documentation (SKILL.md)
Each skill must have a comprehensive SKILL.md with:

```markdown
# Skill Name

## Description
Brief description of what the skill does.

## When To Use
- Specific scenario 1
- Specific scenario 2
- Triggering condition 3

## Usage
This skill uses bun to run TypeScript scripts for [specific purpose].

## Input
- Required parameter 1: Description
- Optional parameter 2: Description

## Output
- Expected output 1: Description
- Generated file 2: Description

## Script Execution
```bash
bun run scripts/script-name.ts
```

## Dependencies
- Node.js built-in modules only
- No external dependencies
- Compatible with bun runtime
```

#### Code Comments
```typescript
/**
 * Parses domain model content and extracts structured metadata
 * @param content - Raw markdown content containing Mermaid diagrams
 * @param context - Parsing context to accumulate results
 */
function extractMermaidDiagrams(content: string, context: ParseContext): void {
  // Implementation details
}
```

## Quality Standards

### Performance Guidelines
- **Streaming**: Process large files in chunks when possible
- **Memory**: Avoid loading entire large files into memory unnecessarily
- **Async**: Use synchronous operations for simplicity unless performance requires async

### Validation and Error Messages
```typescript
function validateInput(filePath: string): void {
  if (!existsSync(filePath)) {
    throw new Error(`Input file not found: ${filePath}`);
  }
  
  if (!filePath.endsWith('.md')) {
    throw new Error(`Expected .md file, got: ${filePath}`);
  }
}
```

### Testing Approach
- **Functional Testing**: Each skill should be easily testable with sample inputs
- **Error Cases**: Handle and test common error scenarios
- **Output Validation**: Verify generated output format and content

## Integration Guidelines

### Skill Composition
Skills should be designed to work together in workflows:

```typescript
// Skill A outputs JSON metadata
// Skill B takes JSON metadata as input
// Skill C combines outputs from A and B
```

### File Naming Conventions
```
/.github/skills/sanjel-drb-blazor/
├── category-name/
│   └── skill-name/
│       ├── SKILL.md
│       └── scripts/
│           └── kebab-case-name.ts
```

### Logging and Debugging
```typescript
function debugMode(): boolean {
  return process.env.DEBUG === 'true';
}

function log(message: string): void {
  if (debugMode()) {
    console.log(`🔧 DEBUG: ${message}`);
  }
}
```

## Compliance Checklist

Before submitting any skill, ensure:

- [ ] Uses bun runtime with TypeScript
- [ ] Only imports Node.js built-in modules with `node:` prefix
- [ ] All imports have `// @ts-ignore` annotations
- [ ] All `import.meta.main` usage has `// @ts-ignore` annotations
- [ ] Follows functional programming paradigm (no classes)
- [ ] Single responsibility principle applied
- [ ] Comprehensive SKILL.md documentation
- [ ] Proper error handling with meaningful messages
- [ ] Consistent emoji-based console output
- [ ] Input validation and graceful error handling
- [ ] JSON output follows standard metadata format
- [ ] Successfully tested with real domain model data

## Example Template

```typescript
// @ts-ignore
import { readFileSync, writeFileSync, existsSync } from 'node:fs';
// @ts-ignore
const process = globalThis.process;

interface InputData {
  // Define input structure
}

interface OutputData {
  version: string;
  generatedAt: string;
  sourceFile: string;
  // ... specific data
  statistics: Record<string, number>;
}

function processInput(filePath: string): OutputData {
  // Core processing logic
}

function main(): void {
  try {
    const args = process.argv.slice(2);
    
    if (args.length === 0) {
      console.error('❌ Usage: bun run script.ts <input-file> [output-file]');
      process.exit(1);
    }

    const inputPath = args[0];
    const outputPath = args[1] || inputPath.replace('.md', '-output.json');
    
    console.log(`📖 Processing: ${inputPath}`);
    
    const result = processInput(inputPath);
    
    console.log(`💾 Writing output to: ${outputPath}`);
    writeFileSync(outputPath, JSON.stringify(result, null, 2), 'utf-8');
    
    console.log('🎉 Processing completed successfully!');
    
  } catch (error) {
    console.error('❌ Error:', error);
    process.exit(1);
  }
}

// @ts-ignore
if (import.meta.main) {
  main();
}
```

---

**Note**: These guidelines ensure all skills maintain consistency, reliability, and compatibility within the VS Code Copilot environment. Deviation from these standards may result in skills that fail to execute properly or integrate with the broader skill ecosystem.