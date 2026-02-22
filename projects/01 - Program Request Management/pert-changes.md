# PERT Adjustments Log - Program Request Management (PRG-01)

## Overview
This document tracks all changes to the project PERT schedule including task duration adjustments, dependency modifications, and critical path impacts.

## Progress Update Procedure

### Step 1: Update Task Progress
**Current Date**: [Status Date]

| Task | Planned Duration | Elapsed Time | % Complete | Actual Progress | Remaining Duration | Revised Duration |
|------|------------------|--------------|------------|-----------------|-------------------|------------------|
| T001 | 1.83 days       | 0 days       | 0%         | 0 days          | 1.83 days         | 1.83 days        |
| T002 | 2.20 days       | 0 days       | 0%         | 0 days          | 2.20 days         | 2.20 days        |
| T003 | 1.78 days       | 0 days       | 0%         | 0 days          | 1.78 days         | 1.78 days        |

### Step 2: Forward Pass Recalculation
**Rules for Progress Updates**:
- **Completed Tasks**: ES = Actual Start Date, EF = Actual Finish Date
- **In-Progress Tasks**: ES = Actual Start Date, EF = Status Date + Remaining Duration
- **Future Tasks**: Recalculate ES based on new predecessor EF values

**Updated Forward Pass**:
| Task | Status | Actual/Revised ES | Actual/Revised EF | Baseline EF | Variance |
|------|--------|-------------------|-------------------|-------------|----------|
| T001 | Not Started | 0 | 1.83 | 1.83 | 0 days |
| T002 | Not Started | 1.83 | 4.03 | 4.03 | 0 days |
| T003 | Not Started | 1.83 | 3.61 | 3.61 | 0 days |

### Step 3: Backward Pass Recalculation
**Starting from project end date**: May 16, 2026

| Task | Revised LF | Revised LS | New Total Slack | New Critical Status |
|------|------------|------------|-----------------|---------------------|
| T001 | 1.83       | 0          | 0               | **Critical**        |
| T002 | 4.03       | 1.83       | 0               | **Critical**        |
| T003 | 12.49      | 10.71      | 8.88            | Non-Critical        |

### Step 4: Schedule Impact Analysis
- **Project End Date Change**: [Original] → [New] ([+/- X days])
- **New Critical Path**: [Path with zero slack]
- **Slack Changes**: [Tasks that gained/lost slack]
- **Resource Reallocation Needed**: [Yes/No - details]

---

## Change Request Template

### Change Request: [CR-ID] - [Description]
**Date**: [Date]
**Requested by**: [Stakeholder]
**Type**: [Add Task / Modify Duration / Change Dependencies / Remove Task]

#### Pre-Change Schedule Analysis
**Current Critical Path Duration**: 58.32 days
**Current Project End**: May 16, 2026

#### Post-Change Forward Pass
Starting from project start, recalculate ES/EF for all affected tasks:

| Task | Previous Duration | New Duration | Previous ES | New ES | Previous EF | New EF | Change |
|------|------------------|--------------|-------------|--------|-------------|--------|----------|
| [Task] | X days | Y days | A | B | C | D | ±Z days |

#### Post-Change Backward Pass
Working backward from project target end date:

| Task | Previous LS | New LS | Previous LF | New LF | Slack Change | Critical Impact |
|------|-------------|--------|-------------|--------|--------------|----------------|
| [Task] | A | B | C | D | ±X days | [Yes/No] |

#### Impact Summary
- **Critical Path Duration**: [Previous] → [New] ([Change])
- **Project End Date Impact**: [±X days]
- **New Critical Tasks**: [List]
- **Tasks No Longer Critical**: [List]
- **Maximum Acceptable Delay Before Next Milestone**: [X days]

#### Mitigation Options
1. **Fast-Track Parallel Tasks**: [Specific opportunities]
2. **Crash Critical Path**: [Add resources to specific tasks]  
3. **Scope Reduction**: [Non-critical features to defer]
4. **Resource Reallocation**: [Move resources from non-critical to critical tasks]

### Updated Network Diagram
[Insert updated Mermaid diagram here]

### Updated Critical Path Calculation
**New Critical Path**: [Task sequence]
**Duration**: [X days]
**Probability of On-Time Completion**: [X%]

---

## Change History

### CR-001: [Change Description]
**Date**: [Date]
**Type**: [Change Type]
**Impact**: [Brief description]
**Approval**: [Approved/Denied]
**Implementation Date**: [Date]

---

**Last Updated**: February 21, 2026
**Next Review Date**: February 24, 2026 (Project Start)
**Document Owner**: Project Manager