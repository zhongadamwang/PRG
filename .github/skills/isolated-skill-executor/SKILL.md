---
name: isolated-skill-executor
description: Execute a target skill in a fresh isolated session, persist only invocation and result artifacts, and close the session afterward.
license: MIT
---

# Isolated Skill Executor

Runs a script-executable skill in a fresh Bun process with an isolated session directory, a dedicated invocation record, and a standardized result artifact under `.skill-runs/`.

## Core Function

**Input**: Executor invocation whose `input` identifies a target skill by `targetSkillId` or `targetSkillPath`, plus `targetInput`  
**Output**: Standard executor result envelope plus persisted target `invocation.json`, `result.json`, and `trace.json` under `.skill-runs/{rootInvocationId}/`

## Supported Target Skills

This executor currently supports target skills whose `skill.json` declares:

```json
{
  "entry": {
    "kind": "bun-script",
    "script": "scripts/run.ts",
    "runtime": "bun"
  }
}
```

If a target skill is documentation-only or uses an unsupported entry kind, the executor returns a typed failure instead of trying to run it.

## Execution Contract

1. Read the executor invocation file.
2. Resolve the target skill by ID or explicit path.
3. Validate target input against the target signature when available.
4. Create a new temporary session directory.
5. Copy the target skill definition into that session.
6. Launch the target skill in a new Bun process.
7. Record target invocation, result, and trace artifacts under `.skill-runs/`.
8. Remove the temporary session directory.

## CLI Usage

```bash
bun run .github/skills/isolated-skill-executor/scripts/execute-skill.ts /path/to/executor-invocation.json
```

## Target Script Runtime Interface

The launched target skill receives:

- CLI argument 1: absolute path to the target `invocation.json`
- `SKILL_INVOCATION_PATH`: same invocation path
- `SKILL_RESULT_PATH`: required output path for the target `result.json`
- `SKILL_TRACE_PATH`: optional output path for target trace data
- `SKILL_SESSION_DIR`: isolated temporary session directory
- `SKILL_STEP_DIR`: persistent step artifact directory
- `SKILL_ISOLATION_MODE`: `isolated-session`

The target script should write a standard `result.json` to `SKILL_RESULT_PATH`.

## Failure Model

The executor never reports failures through untyped exceptions once invocation loading succeeds. Common typed failures include:

- `INVALID_EXECUTOR_INPUT`
- `TARGET_SKILL_NOT_FOUND`
- `SELF_EXECUTION_FORBIDDEN`
- `UNSUPPORTED_ENTRY_KIND`
- `TARGET_INPUT_INVALID`
- `TARGET_PROCESS_FAILED`
- `TARGET_RESULT_MISSING`
- `TARGET_RESULT_INVALID`

## Notes

- The executor does not execute itself as a target skill.
- The isolation model is process/session isolation, not an OS-level sandbox.
- The persistent artifacts that remain after execution are the run tree outputs under `.skill-runs/`.
