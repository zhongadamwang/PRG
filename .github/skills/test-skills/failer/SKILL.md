---
name: taoqf-failer
description: Intentionally fail to test error handling.
---

# taoqf-failer

Failure example: always returns a structured error.

```mermaid
flowchart TD
  Input([input: reason]) --> Fail["return error"]
  Fail --> Output([result.ok = false])
```
