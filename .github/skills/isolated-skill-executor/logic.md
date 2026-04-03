# Isolated Skill Executor Logic

This skill only documents the high-level relationship between the executor and the target skill it launches.

**Main input**: `targetSkillId | targetSkillPath`, `targetInput`  
**Main output**: executor `result.json`, target run artifacts under `.skill-runs/`

```mermaid
flowchart TB
	Caller[Caller] --> Executor[sanjel-isolated-skill-executor-v1-0-0]
	Executor --> Resolve[Resolve target skill]
	Resolve --> Session[Create isolated session]
	Session --> Target[Run target skill in new Bun process]
	Target --> Persist[Persist invocation/result/trace]
	Persist --> Close[Close isolated session]
	Close --> Result[Return executor result]
```
