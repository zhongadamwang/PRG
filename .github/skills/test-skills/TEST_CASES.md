# Test cases for isolated skill executor

This document lists reproducible test cases for the test skills in `.github/skills/test-skills/`.
Each case shows: purpose, skill folder, input file, expected result file, run steps, and success criteria.

---

## Common notes
- Runtime artifacts must be written under `{repo-root}/.skill-runs/{rootInvocationId}/`.
- Expected per-invocation files: `invocation.json`, `result.json`, `trace.json` in a numbered step folder (e.g., `00-root`, `01-<skillId>`).
- When running the isolated executor, pick a unique `rootInvocationId` (e.g., `test_run_001`).
- Example invocation (script options depend on local executor):
  - `bun ./.github/skills/isolated-skill-executor/scripts/execute-skill.ts --skillFolder .github/skills/test-skills/<skill> --input examples/input.json --rootInvocationId test_run_001`
  - The script should create `.skill-runs/test_run_001/` and write ordered step folders.

---

### 1) Simple add (success)
- Skill folder: `.github/skills/test-skills/add/`
- Input: `.github/skills/test-skills/add/examples/input.json` (a=2, b=3)
- Expected: `.github/skills/test-skills/add/examples/expected_result.json` (sum=5)
- Run steps:
  1. Run executor with skillFolder `.../add` and input file.
  2. Verify `.skill-runs/{root}/00-root/` (root invocation) and `01-taoqf-add-v0-0-1/` exist.
  3. Verify `result.json.ok === true` and `result.json.value.sum === 5`.
- Success criteria: result matches expected JSON shape and value.

---

### 2) Is-even (branch)
- Skill folder: `.github/skills/test-skills/is-even/`
- Input: `examples/input.json` (n = 4)
- Expected: `examples/expected_result.json` (isEven=true)
- Notes: Test both even and odd (n=3 -> isEven=false).
- Success criteria: `result.json.value.isEven` matches parity.

---

### 3) Counter (loop)
- Skill folder: `.github/skills/test-skills/counter/`
- Input: `examples/input.json` (n = 4)
- Expected: `examples/expected_result.json` (values = [0,1,2,3])
- Notes:
  - Verify that the invocation produces one `result.json` containing the full array.
  - If executor records per-iteration child calls, ensure each iteration is stored as a separate ordered subfolder.
- Success criteria: `result.json.value.values` equals expected array.

---

### 4) Factorial (recursion)
- Skill folder: `.github/skills/test-skills/factorial/`
- Input: `examples/input.json` (n = 5)
- Expected: `examples/expected_result.json` (factorial=120)
- Notes:
  - Because `factorial` allows child skills, expect multiple invocation instances if implemented as child calls.
  - Verify call ordering and that recursionDepth / parentInvocationId (if present) are sensible.
- Success criteria: `result.json.value.factorial === 120` and recursive call folders (if any) are present and ordered.

---

### 5) Caller (composition)
- Skill folder: `.github/skills/test-skills/caller/`
- Input: `examples/input.json` (a=5, b=7)
- Expected: `examples/expected_result.json` (sum=12, isEven=true)
- Notes:
  - This skill should call `taoqf-add` then `taoqf-is-even`.
  - Verify that both child calls appear under the same root run folder and are ordered (e.g., `01-taoqf-add...`, `02-taoqf-is-even...`).
- Success criteria: final caller `result.json.value` contains `{ sum: 12, isEven: true }` and child invocation artifacts exist.

---

### 6) Failer (intentional failure)
- Skill folder: `.github/skills/test-skills/failer/`
- Input: `examples/input.json` (reason)
- Expected: `examples/expected_result.json` (ok=false, error.code=INTENTIONAL_FAILURE)
- Notes:
  - Verify executor treats failures as data (no uncaught exception), writes `result.json` with `ok:false` and an `error` object matching `signature.json`.
- Success criteria: `result.json.ok === false` and `result.json.error.code === "INTENTIONAL_FAILURE"`.

---

## Additional test variants
1. Repeated calls: run the same skill multiple times in one root invocation (if supported) to confirm numeric prefix ordering and unique per-call folders.
2. Looped child calls: create a wrapper that calls a child skill N times and confirm `01-`, `02-`, ... folders exist.
3. Deep recursion: run `factorial` with a smaller limit that still exercises recursion; confirm `maxChildCallDepth` guard works (if implemented) and appropriate error returns on depth exceed.
4. Missing input schema: (optional) try invoking a skill without `input` declared and ensure executor passes an empty input object and that the skill runs or returns an appropriate typed error.

---

## Checklist for each automated run
- [ ] Choose `rootInvocationId` unique string
- [ ] Run executor with skill folder + input file
- [ ] Confirm `.skill-runs/{root}/run.json` exists and lists steps
- [ ] Confirm per-step folders exist and are zero-padded in order
- [ ] Confirm `invocation.json` and `result.json` are present for each step
- [ ] Validate `result.json` against expected JSON in `examples/expected_result.json` (structural + value checks)


---

If any test fails, collect these artifacts and paste them here: `.skill-runs/{root}/.../invocation.json`, `.skill-runs/{root}/.../result.json`, and the executor CLI stdout/stderr.


---

Saved examples in `.github/skills/test-skills/*/examples/` should be used as canonical expected inputs and outputs for quick local verification.
