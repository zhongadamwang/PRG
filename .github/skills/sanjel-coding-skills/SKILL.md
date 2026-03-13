---
name: sanjel-coding-skills
description: Top-level orchestrator that generates a complete Blazor project from a domain model, by sequentially invoking architecture, entity, and page-generation skills.
---

## Overview

Top-level orchestrator skill that drives full Blazor project code generation from a domain model. It sequentially invokes system-architecture-generation and entity-class-generation, then delegates page generation to the `page-generation` subprocess.

## Actor

Senior .NET Architect - responsible for overseeing the entire code generation process, ensuring that each skill is executed in the correct sequence, and that the final output meets architectural and coding standards.

## Type

Orchestrator - coordinates multiple skills and subprocesses to achieve the overall goal of generating a complete Blazor project.

## Flow

```mermaid
graph TB
	Start([Start]) --> S01[system-architecture-generation]
	S01 --> S02[entity-class-generation]
	S02 --> S03[page-coding-skills]
	click S03 "./page-generation/SKILL.md"

	S03 --> End([Complete Code Files])

	classDef arch fill:#f8cecc,stroke:#b85450;
	classDef core fill:#dae8fc,stroke:#6c8ebf;
	classDef page fill:#fff2cc,stroke:#d6b656;

	class S01 arch;
	class S02,S03 core;
```

> For inputs and outputs, see [structure.json](structure.json).
