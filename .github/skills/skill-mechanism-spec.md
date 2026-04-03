# Skill Mechanism Specification

This document defines a standard mechanism for describing, invoking, composing, and recording skills in a file-based system.

The design goals are:

- use JSON files to declare formal input and output types
- forbid exception-based skill contracts and use result-style outputs instead
- allow one skill to call another skill safely
- use standard JSON files to persist actual invocation input and actual result output
- keep the specification simple enough for tooling and humans to read

---

## 1. Core Principles

### 1.1 No Exceptions in the Skill Contract

A skill must never expose failure through an untyped exception contract.

All skill outcomes must be represented as data:

- success is a typed result value
- failure is a typed error value

At the contract level, every skill behaves like a Rust-style `Result<TSuccess, TError>`.

### 1.2 Types Are Declared, Data Is Recorded

The mechanism separates:

- **signature files**: formal declarations of allowed input and output shapes
- **runtime files**: actual invocation data and actual result data

Type files and runtime data files must never be mixed into the same document.

### 1.3 File Absence Has Meaning

If an input or output schema file does not exist, that absence is meaningful:

- missing input schema means the skill accepts no typed input
- missing success output schema means the skill returns no success payload
- the standard error result still exists, because all skills must be able to return failure as data

### 1.4 Prefer One Canonical Shape

To reduce ambiguity, the recommended contract form is a single `signature.json` file.

Split files are allowed for compatibility and tooling convenience:

- `input.schema.json`
- `output.schema.json`

If both the single-file and split-file forms are present, `signature.json` is the source of truth.

### 1.5 Logic Must Be Documented Visually

Each skill must include a standard Markdown file with a Mermaid diagram that describes the skill at a high level.

This file serves a role similar to high-value function documentation:

- it shows which other skills may be called
- it shows the high-level invocation relationship between skills
- it does not need to describe detailed internal control flow
- it gives reviewers a stable, human-readable view of behavior

---

## 2. Standard Files

Each skill should live in its own folder.

### 2.1 Recommended Skill Folder Layout

```text
{skill-folder}/
├── SKILL.md
├── skill.json
├── logic.md                     # required high-level skill relationship diagram
├── signature.json              # recommended canonical contract
├── input.schema.json           # optional split form
└── output.schema.json          # optional split form
```

### 2.2 File Responsibilities

#### `skill.json`

Describes skill identity and operational metadata.

#### `logic.md`

Required Markdown documentation for the skill's high-level logic and skill relationships.

It must contain at least:

- a short prose summary of the skill purpose
- one Mermaid diagram showing the high-level relationship between this skill and any child skills it may call
- the main input and output names

It does not need to document low-level internal branching, loops, or implementation detail unless those details are necessary to understand inter-skill orchestration.

#### `signature.json`

Defines the formal input and output contract in one file.

#### `input.schema.json`

Optional split alternative for formal input type definition.

#### `output.schema.json`

Optional split alternative for formal success output type definition.

---

## 3. Identity and Metadata

Every skill should declare a stable identity in `skill.json`.

### 3.1 Required Metadata Fields

```json
{
  "schemaVersion": "skill-manifest/v1",
  "skillId": "sanjel-requirements-ingest-v1-0-0",
  "author": "sanjel",
  "skillName": "requirements-ingest",
  "name": "Requirements Ingest",
  "version": "1.0.0",
  "description": "Normalize requirements into structured artifacts",
  "entry": {
    "kind": "prompt-skill"
  }
}
```

### 3.2 Recommended Additional Metadata

```json
{
  "timeoutMs": 300000,
  "idempotent": true,
  "sideEffects": [
    "filesystem.write"
  ],
  "permissions": {
    "filesystem": "workspace",
    "network": false,
    "environment": []
  },
  "allowsChildSkills": true,
  "maxChildCallDepth": 8
}
```

### 3.3 Why This Matters

The manifest fills gaps that type definitions do not cover:

- unique identity
- versioning
- capability declaration
- security boundaries
- execution limits
- composition rules

### 3.4 Standard `skillId` Format

To avoid naming collisions, `skillId` must use this format:

```text
[author]-[skill-name]-[version]
```

Examples:

- `sanjel-requirements-ingest-v1-0-0`
- `github-project-planning-tracking-v2-1-0`

### 3.5 `skillId` Rules

- `author` identifies the publisher or owning namespace
- `skill-name` is the stable logical skill name
- `version` must be derived from the declared `version` field
- the version token in `skillId` should normalize dots to hyphens and be prefixed with `v`

For example:

- `version = "1.0.0"` -> `skillId` suffix `v1-0-0`

The `skillId` and `version` fields must agree with each other.

---

## 4. Type System for Formal Contracts

The formal type files should use a constrained JSON-Schema-compatible style rather than inventing a completely new type language.

This gives:

- existing validator support
- good editor tooling
- enough structure to model TypeScript-like object shapes

### 4.1 Supported Type Features

The contract type system should support at least:

- primitive types: `string`, `number`, `integer`, `boolean`, `null`
- `object`
- `array`
- `enum`
- `oneOf` for tagged unions or constrained alternatives
- `const`
- `required`
- `$ref`
- `$defs`
- `additionalProperties`
- `description`

### 4.2 Contract Scope

The formal contract covers:

- **input**: the typed request payload
- **success output**: the typed value returned when `ok = true`
- **error details**: optional skill-specific structured details when `ok = false`

The outer result envelope itself is standardized by this specification and does not need to be redefined by every skill.

---

## 5. Canonical Contract Format

`signature.json` is the preferred file because it keeps the whole contract together.

### 5.1 Canonical `signature.json`

```json
{
  "schemaVersion": "skill-signature/v1",
  "skillId": "sanjel-requirements-ingest-v1-0-0",
  "skillVersion": "1.0.0",
  "input": {
    "type": "object",
    "required": ["projectId", "sourcePaths"],
    "properties": {
      "projectId": { "type": "string" },
      "sourcePaths": {
        "type": "array",
        "items": { "type": "string" },
        "minItems": 1
      }
    },
    "additionalProperties": false
  },
  "success": {
    "type": "object",
    "required": ["requirementsPath", "requirementCount"],
    "properties": {
      "requirementsPath": { "type": "string" },
      "requirementCount": { "type": "integer", "minimum": 0 }
    },
    "additionalProperties": false
  },
  "errorDetails": {
    "type": "object",
    "properties": {
      "failedFiles": {
        "type": "array",
        "items": { "type": "string" }
      }
    },
    "additionalProperties": false
  }
}
```

### 5.2 Meaning of Missing Sections

- missing `input` means no typed input payload is expected
- missing `success` means success returns no payload beyond the standard envelope
- missing `errorDetails` means only the standard error fields are allowed

### 5.3 Split-File Equivalent

If a project prefers split files, the equivalent layout is:

```text
input.schema.json
output.schema.json
```

With this interpretation:

- missing `input.schema.json` means no typed input
- missing `output.schema.json` means no success payload

Skill-specific error detail shape may be placed in `skill.json` or another explicitly referenced file, but the recommended form remains `signature.json`.

---

## 6. Standard Runtime Data Files

Formal types define what is allowed.

Runtime data files record what actually happened.

Unlike skill definition files, runtime files must be stored in one fixed, centralized location.

### 6.0 Fixed Runtime Storage Root

All invocation outputs must be written under a single fixed root folder:

```text
{repo-root}/.skill-runs/
```

Each top-level invocation gets one main folder:

```text
{repo-root}/.skill-runs/{rootInvocationId}/
```

If that invocation triggers child-skill calls, their outputs must be stored under the same main folder and separated by call order.

### 6.0.1 Recommended Runtime Layout

```text
{repo-root}/.skill-runs/
└── {rootInvocationId}/
    ├── run.json
    ├── 00-root/
    │   ├── invocation.json
    │   ├── result.json
    │   └── trace.json
    ├── 01-sanjel-requirements-ingest-v1-0-0/
    │   ├── invocation.json
    │   ├── result.json
    │   └── trace.json
    ├── 02-sanjel-goals-extract-v1-0-0/
    │   ├── invocation.json
    │   ├── result.json
    │   └── trace.json
    └── 03-sanjel-process-w5h-v1-0-0/
        ├── invocation.json
        ├── result.json
        └── trace.json
```

Rules:

- one root invocation -> one main folder
- each direct or indirect call gets its own ordered subfolder
- call order must be preserved by a zero-padded numeric prefix
- all artifacts for one execution tree stay together

### 6.0.3 Repeated, Loop, and Recursive Calls

Runtime storage must be organized by invocation instance, not by `skillId`.

This means:

- if the same skill is called multiple times, each call gets its own subfolder
- if a skill is called inside a loop, each loop iteration gets its own subfolder
- if a skill calls itself recursively, each recursive step gets its own subfolder

The storage key must never rely on `skillId` alone, because the same skill may appear many times within one execution tree.

The minimum identifiers that distinguish one invocation from another are:

- `rootInvocationId`
- `invocationId`
- `stepOrder`
- `skillId`

When recursion or nested composition is involved, these should also be recorded:

- `parentInvocationId`
- `recursionDepth`

### 6.0.4 Example: Loop Calls

```text
{repo-root}/.skill-runs/run_001/
├── 00-root/
├── 01-sanjel-validate-item-v1-0-0/
├── 02-sanjel-validate-item-v1-0-0/
├── 03-sanjel-validate-item-v1-0-0/
└── 04-sanjel-aggregate-results-v1-0-0/
```

In this example, the same validation skill is called three times in one loop. Each call has its own `invocation.json` and `result.json`.

### 6.0.5 Example: Recursive Calls

```text
{repo-root}/.skill-runs/run_002/
├── 00-root/
├── 01-sanjel-tree-walk-v1-0-0/
├── 02-sanjel-tree-walk-v1-0-0/
├── 03-sanjel-tree-walk-v1-0-0/
└── 04-sanjel-tree-walk-v1-0-0/
```

In this example, the same tree-walk skill calls itself recursively. The calls are separated by `stepOrder`, and the parent-child relationship is recorded in metadata rather than inferred from folder names alone.

### 6.0.6 `run.json`

`run.json` is the summary index for the entire execution tree.

Example:

```json
{
  "schemaVersion": "skill-run/v1",
  "rootInvocationId": "run_01JQ7N4ROOT",
  "rootSkillId": "sanjel-project-bootstrap-v1-0-0",
  "startedAt": "2026-04-03T02:40:00Z",
  "status": "failed",
  "steps": [
    {
      "order": 0,
      "folder": "00-root",
      "skillId": "sanjel-project-bootstrap-v1-0-0",
      "invocationId": "inv_root_01"
    },
    {
      "order": 1,
      "folder": "01-sanjel-requirements-ingest-v1-0-0",
      "skillId": "sanjel-requirements-ingest-v1-0-0",
      "invocationId": "inv_child_01"
    }
  ]
}
```

### 6.1 `invocation.json`

```json
{
  "schemaVersion": "skill-invocation/v1",
  "invocationId": "inv_01JQ7N4K8D5R3P6M2B9T",
  "skillId": "sanjel-requirements-ingest-v1-0-0",
  "skillVersion": "1.0.0",
  "timestamp": "2026-04-03T02:40:00Z",
  "caller": {
    "kind": "skill",
    "skillId": "sanjel-project-bootstrap-v1-0-0",
    "invocationId": "inv_01JQ7N1Y5M7F9Q2K1A8C"
  },
  "run": {
    "rootInvocationId": "run_01JQ7N4ROOT",
    "stepOrder": 1,
    "stepFolder": "01-sanjel-requirements-ingest-v1-0-0",
    "parentInvocationId": "inv_01JQ7N1Y5M7F9Q2K1A8C",
    "recursionDepth": 0
  },
  "input": {
    "projectId": "PRG-001",
    "sourcePaths": [
      "projects/01 - Program Request Management/artifacts/Requirements/workflow-meeting-transcript.md"
    ]
  },
  "context": {
    "workspace": "/repo",
    "traceId": "trace_01JQ7N4A8Y"
  },
  "limits": {
    "timeoutMs": 300000,
    "maxChildCallDepth": 8
  }
}
```

### 6.2 `result.json`

```json
{
  "schemaVersion": "skill-result/v1",
  "invocationId": "inv_01JQ7N4K8D5R3P6M2B9T",
  "skillId": "sanjel-requirements-ingest-v1-0-0",
  "finishedAt": "2026-04-03T02:40:08Z",
  "durationMs": 8241,
  "ok": true,
  "value": {
    "requirementsPath": "projects/PRG-001/artifacts/Analysis/requirements.json",
    "requirementCount": 25
  },
  "error": null
}
```

### 6.3 Failure Result Example

```json
{
  "schemaVersion": "skill-result/v1",
  "invocationId": "inv_01JQ7N4K8D5R3P6M2B9T",
  "skillId": "sanjel-requirements-ingest-v1-0-0",
  "finishedAt": "2026-04-03T02:40:08Z",
  "durationMs": 8241,
  "ok": false,
  "value": null,
  "error": {
    "code": "SOURCE_PARSE_FAILED",
    "message": "One or more source files could not be parsed",
    "retryable": false,
    "details": {
      "failedFiles": [
        "specs/legacy.doc"
      ]
    }
  }
}
```

---

## 7. Standard Result Envelope

Every runtime result must use the same outer shape.

### 7.1 Envelope Definition

Conceptually:

```ts
type SkillResult<TSuccess, TErrorDetails = unknown> =
  | {
      ok: true;
      value: TSuccess | null;
      error: null;
    }
  | {
      ok: false;
      value: null;
      error: {
        code: string;
        message: string;
        retryable: boolean;
        details?: TErrorDetails;
      };
    };
```

### 7.2 Required Error Fields

Every failure result must include:

- `code`: stable machine-readable error identifier
- `message`: human-readable explanation
- `retryable`: whether calling again might succeed without changing the contract

### 7.3 Recommended Error Fields

- `details`: typed skill-specific detail payload
- `causedBy`: parent or child invocation references
- `hints`: optional suggested next actions

### 7.4 Error Code Rules

Error codes should:

- be stable across versions whenever practical
- be uppercase snake case
- describe a failure category, not a sentence

Examples:

- `INVALID_INPUT`
- `TIMEOUT`
- `DEPENDENCY_FAILED`
- `PERMISSION_DENIED`
- `RESOURCE_LIMIT_EXCEEDED`

---

## 8. Validation Rules

Validation is mandatory at runtime, not only at authoring time.

### 8.1 Input Validation

Before executing the skill body:

- load the declared input schema if present
- validate `invocation.json.input`
- if invalid, return `ok: false` with `code: "INVALID_INPUT"`

### 8.2 Success Validation

Before writing a success result:

- validate the produced success payload against the declared success schema if present
- if invalid, return `ok: false` with `code: "INVALID_OUTPUT"`

### 8.3 Error Validation

If `error.details` is present and `errorDetails` is declared:

- validate `error.details` against the declared error detail schema

This prevents "failure as data" from becoming untyped failure in practice.

---

## 9. Skill-to-Skill Invocation

A skill may call other skills, but composition must be explicit and bounded.

### 9.1 Child Skill Calls

When skill `A` calls skill `B`:

- `A` becomes the caller recorded in `B`'s `invocation.json`
- `A` must handle `B`'s result as data
- `A` must not rethrow `B`'s failure as an untyped exception
- `B` must write its runtime files into the same root run folder as `A`
- `B` must receive the next available ordered step folder for that execution tree

If `A` calls the same child skill multiple times, each child call must still get a distinct invocation record and a distinct ordered step folder.

### 9.2 Propagation Rule

A parent skill may:

- map a child failure into its own error code
- embed child failure information into `error.details`
- return the child failure unchanged if that is the declared contract

But the parent must always return a valid result envelope.

### 9.2.1 Loop and Recursion Semantics

For looped calls:

- each iteration is a separate invocation
- the iteration index may be recorded in `invocation.json.context` or `invocation.json.run`
- results must remain isolated per invocation

For recursive calls:

- each recursive step is a separate invocation
- `parentInvocationId` should point to the immediate caller
- `rootInvocationId` must remain stable across the entire recursion tree
- `recursionDepth` should increase monotonically from the root call

The implementation must not overwrite prior invocation data when the same skill appears repeatedly in one execution tree.

### 9.3 Safety Constraints

The mechanism should enforce:

- maximum child-call depth
- cycle detection
- timeout propagation
- explicit permission checks

### 9.4 Recommended Parent Failure Pattern

```json
{
  "ok": false,
  "value": null,
  "error": {
    "code": "DEPENDENCY_FAILED",
    "message": "A child skill failed",
    "retryable": false,
    "details": {
      "childSkillId": "sanjel-requirements-ingest-v1-0-0",
      "childInvocationId": "inv_01JQ7N4K8D5R3P6M2B9T",
      "childErrorCode": "SOURCE_PARSE_FAILED"
    }
  }
}
```

---

## 10. Traceability and Observability

To make composition debuggable, each run should carry trace metadata.

### 10.1 Required Runtime Identifiers

- `invocationId`
- `skillId`
- `skillVersion`

### 10.2 Recommended Trace Fields

- `traceId`
- `parentInvocationId`
- `rootInvocationId`
- `childInvocations`
- timing data

### 10.3 `trace.json` Example

```json
{
  "schemaVersion": "skill-trace/v1",
  "traceId": "trace_01JQ7N4A8Y",
  "rootInvocationId": "inv_root_01",
  "currentInvocationId": "inv_parent_01",
  "childInvocations": [
    {
      "skillId": "sanjel-requirements-ingest-v1-0-0",
      "invocationId": "inv_child_01",
      "stepOrder": 1,
      "stepFolder": "01-sanjel-requirements-ingest-v1-0-0",
      "status": "failed"
    }
  ]
}
```

---

## 11. Versioning and Compatibility

Versioning must exist at both the skill level and the specification level.

### 11.1 Required Versions

- manifest schema version
- signature schema version
- runtime file schema version
- skill version

### 11.2 Compatibility Rules

Minor, backward-compatible changes may include:

- adding optional fields
- widening an enum if consumers are documented to ignore unknown values
- adding new error codes

Breaking changes include:

- removing required fields
- renaming fields
- changing field meaning
- changing result envelope semantics

### 11.3 Recommended Rule

Treat signature changes like API changes:

- backward-compatible change -> minor version bump
- breaking contract change -> major version bump

---

## 12. Side Effects and Permissions

Type declarations alone do not describe operational risk.

Each skill should explicitly declare:

- whether it is idempotent
- whether it writes files
- whether it uses network access
- whether it reads environment variables
- whether it is allowed to call child skills

This belongs in `skill.json`, not in `signature.json`.

The required logic documentation belongs in `logic.md`, not in `signature.json`.

---

## 13. Recommended Defaults

If you want a practical minimal standard, use these defaults:

- canonical contract file: `signature.json`
- manifest file: `skill.json`
- required logic file: `logic.md`
- runtime root folder: `{repo-root}/.skill-runs/{rootInvocationId}/`
- runtime input file: `{repo-root}/.skill-runs/{rootInvocationId}/{ordered-step}/invocation.json`
- runtime output file: `{repo-root}/.skill-runs/{rootInvocationId}/{ordered-step}/result.json`
- optional composition trace: `{repo-root}/.skill-runs/{rootInvocationId}/{ordered-step}/trace.json`
- standard result envelope with `ok`, `value`, and `error`
- JSON-Schema-compatible typing

---

## 14. Minimal Examples

### 14.1 No-Input, No-Payload Skill

If a skill takes no typed input and returns no success payload:

- `signature.json` may be omitted entirely, or
- `signature.json` may exist without `input` and `success`

Success result:

```json
{
  "schemaVersion": "skill-result/v1",
  "invocationId": "inv_ping_01",
  "skillId": "sanjel-cache-warmup-v1-0-0",
  "ok": true,
  "value": null,
  "error": null
}
```

### 14.2 Split Contract Example

`input.schema.json`

```json
{
  "type": "object",
  "required": ["requestId"],
  "properties": {
    "requestId": { "type": "string" }
  },
  "additionalProperties": false
}
```

`output.schema.json`

```json
{
  "type": "object",
  "required": ["status"],
  "properties": {
    "status": {
      "type": "string",
      "enum": ["accepted", "rejected"]
    }
  },
  "additionalProperties": false
}
```

---

## 15. Summary of Required Rules

An implementation conforming to this specification must satisfy all of the following:

1. A skill has stable metadata in `skill.json`.
2. A skill has required logic documentation in `logic.md`, including a high-level Mermaid diagram of inter-skill calls.
3. A skill declares formal types with `signature.json`, `input.schema.json`, `output.schema.json`, or explicit file absence.
4. A skill `skillId` follows `[author]-[skill-name]-[version]`.
5. A skill never exposes failure as an untyped exception contract.
6. Every execution tree writes standardized runtime data files into one fixed root run folder.
7. Child-skill results are stored in ordered subfolders that preserve call order.
8. Every runtime result uses the standard `ok/value/error` envelope.
9. Runtime input and output are validated against formal contract files when present.
10. Skill-to-skill calls are explicit, traceable, and bounded.
11. Versioning and compatibility are treated as first-class concerns.

---

## 16. Recommended Next Refinements

If this mechanism moves from concept to implementation, the next documents worth defining are:

1. a formal JSON Schema for `skill.json`
2. a formal JSON Schema for `signature.json`
3. a formal JSON Schema for `invocation.json`
4. a formal JSON Schema for `result.json`
5. a child-skill permission and sandbox policy
6. a compatibility policy for schema evolution
