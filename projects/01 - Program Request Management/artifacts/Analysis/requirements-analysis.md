# Requirements Analysis Report

**Project**: PRG-01  
**Sources**: 
- workflow-meeting-transcript.md
- Sanjel Engineering Request Process Diagram (REV 2).pdf
- Program request Management tool Phase 1.docx
**Generated**: 2026-02-20T15:30:00Z  
**Updated**: 2026-02-21T00:00:00Z (Strategy Update)  
**Total Requirements**: 47

## Strategy Update

**Implementation Approach**: Human interface first, then implement automation. All automation features are targeted for future phases while manual alternatives drive MVP development.

## Executive Summary

Analysis of requirements documents reveals a comprehensive program request management workflow spanning from initial request submission through engineering completion. **Updated Strategy**: The system will prioritize manual interfaces for all functions to enable quick MVP development and requirement validation, with automation features implemented in subsequent phases. This approach reduces initial complexity while validating workflows before automation investment.

## Core Requirements

| ID | Section | Text | Tags | Confidence | Source | **Phase** |
|----|---------|------|------|------------|---------|--------|
| R-001 | Request Creation | ~~System SHALL automatically generate requests from incoming emails~~ **MVP: Manual request creation interface SHALL be provided** | functional | high | transcript-workflow | **Phase 1 (Manual)** |
| R-002 | Request Creation | ~~System SHALL import state diagram data automatically~~ **MVP: Manual state diagram data entry interface SHALL be provided** | functional | high | transcript-workflow | **Phase 1 (Manual)** |
| R-003 | Status Management | System SHALL support request status progression: Open → Assigned → Acknowledged → In Progress → Awaiting Review → Review → Program Ready | functional | high | transcript-workflow | **Phase 1** |
| R-004 | Status Management | New requests SHALL default to "Open" status when not yet assigned | functional | high | transcript-workflow |
| R-005 | Assignment Process | System SHALL allow managers to assign requests to specific engineers | functional | high | transcript-workflow |
| R-006 | Assignment Process | System SHALL change status to "Assigned" when manager assigns request to engineer | functional | high | transcript-workflow |
| R-007 | Notification System | System SHALL notify all team members when request is assigned (not just assigned engineer) | functional | high | transcript-workflow |
| R-008 | Assignment Response | Assigned engineer SHALL be able to Accept, Decline, or Reassign requests via email buttons | functional | high | transcript-workflow |
| R-009 | Assignment Response | System SHALL change status to "Acknowledged" when engineer accepts assignment | functional | high | transcript-workflow |
| R-010 | Assignment Response | System SHALL return request to "Open" status when engineer declines assignment | functional | high | transcript-workflow |
| R-011 | Assignment Response | System SHALL allow engineer to reassign request to another engineer | functional | high | transcript-workflow |
| R-012 | Assignment Response | Only the assigned engineer SHALL see Accept/Decline/Reassign buttons in notifications | functional | high | transcript-workflow |
| R-013 | Data Validation | System SHALL require engineers to validate imported state diagram data before proceeding | functional | high | transcript-workflow |
| R-014 | Data Validation | System SHALL provide data tree view, state diagram view, and data details view for validation | functional | high | transcript-workflow |
| R-015 | Data Validation | Engineers SHALL be able to correct automatically parsed data during validation | functional | high | transcript-workflow |
| R-016 | Progress Tracking | System SHALL change status to "In Progress" when engineer begins data validation process | functional | high | transcript-workflow |
| R-017 | Review Process | Engineers SHALL be able to send completed work for group review | functional | high | transcript-workflow |
| R-018 | Review Process | System SHALL change status to "Awaiting Review" when sent for review | functional | high | transcript-workflow |
| R-019 | Review Process | Review requests SHALL be sent to entire engineering team (not specific reviewer) | functional | high | transcript-workflow |
| R-020 | Review Process | Team member SHALL be able to accept review responsibility | functional | high | transcript-workflow |
| R-021 | Review Process | System SHALL change status to "Review" when team member accepts review | functional | high | transcript-workflow |
| R-022 | Review Approval | Reviewers SHALL be able to approve or request adjustments | functional | high | transcript-workflow |
| R-023 | Review Approval | System SHALL change status to "Program Ready" when review is approved | functional | high | transcript-workflow |
| R-024 | Review Adjustment | System SHALL return request to engineer when adjustments requested | functional | high | transcript-workflow |
| R-025 | Review Adjustment | All previous approvals SHALL be invalidated when adjustments requested | functional | high | transcript-workflow |
| R-026 | Review Adjustment | Adjusted requests SHALL restart full approval process | functional | high | transcript-workflow |
| R-027 | Escalation | System SHALL automatically escalate unacknowledged assignments based on priority rules | functional | medium | transcript-workflow |
| R-028 | Escalation | High priority requests SHALL have 4-5 hour acknowledgment timeout | nonfunctional | medium | transcript-workflow |
| R-029 | Security | System SHALL integrate with organizational AD groups for role management | nonfunctional | high | transcript-workflow |
| R-030 | Security | System SHALL support role-based access control for assignment privileges | functional | high | transcript-workflow |
| R-031 | Integration | System SHALL integrate with existing team management tools for user lists | nonfunctional | medium | transcript-workflow |
| R-032 | Integration | System SHALL potentially integrate with Bamboo HR for team member data | nonfunctional | low | transcript-workflow |
| R-033 | Automation | ~~System SHALL support automated request triaging based on client lists~~ **Future Phase: Automation Enhancement** | functional | medium | transcript-workflow | **Future Phase** |
| R-034 | Automation | ~~Managers SHALL retain override capability for automated triage decisions~~ **Future Phase: Manual triage in Phase 1** | functional | medium | transcript-workflow | **Future Phase** |
| R-035 | Work Separation | System SHALL separate job design work from pricing work | functional | high | transcript-workflow |
| R-036 | Work Separation | Engineering phase SHALL focus only on job design completion | functional | high | transcript-workflow |
| R-037 | Program Generation | System SHALL save complete program when engineering work is finished | functional | high | transcript-workflow |
| R-038 | Program Generation | System SHALL generate customer-facing service reports and service tickets | functional | medium | transcript-workflow |
| R-039 | Program Output | System SHALL provide PDF generation and printing capabilities | functional | medium | transcript-workflow |
| R-040 | Program Output | System SHALL potentially auto-save programs to SharePoint structure | functional | low | transcript-workflow |
| R-041 | Document Design | System SHALL provide improved layout design for customer documents | nonfunctional | medium | transcript-workflow |
| R-042 | Document Design | Generated documents SHALL be reusable across multiple workflow stages | functional | medium | transcript-workflow |
| R-043 | Scope Limitation | Phase 1 SHALL exclude sales workflow beyond pricing input | constraint | high | transcript-workflow |
| R-044 | Scope Limitation | Phase 1 SHALL exclude operations workflow | constraint | high | transcript-workflow |
| R-045 | Scope Limitation | Phase 1 SHALL focus primarily on engineering request workflow | constraint | high | transcript-workflow |
| R-046 | Platform | System SHALL be implemented as ASP.NET Blazor web application | constraint | high | project-init |
| R-047 | Stakeholders | System SHALL serve Product Owner, Engineering, Sales, and IT stakeholder groups | constraint | high | project-init |

## Workflow States Identified

1. **Open** - New request, not yet assigned
2. **Assigned** - Assigned to engineer, awaiting acknowledgment  
3. **Acknowledged** - Engineer accepted assignment
4. **In Progress** - Engineer working (triggered by data validation start)
5. **Awaiting Review** - Work submitted for team review
6. **Review** - Review in progress by team member
7. **Program Ready** - Approved and ready for next phase

## Key User Roles

- **Manager** (Jeremy/Warren): Assigns requests, manages escalations
- **Engineer** (Henry/Jason): Accepts assignments, validates data, completes job design
- **Reviewer** (Engineering Team): Reviews completed work, approves/requests adjustments
- **System Admin**: Manages user roles, integration settings

## Critical Business Rules

1. Engineers can accept, decline, or reassign assigned requests
2. Data validation is mandatory before work can progress
3. Review is team-based (not individual assignment)
4. Adjustment requests invalidate all prior approvals
5. Phase 1 scope limited to engineering workflow only

## Glossary Terms

- **State Diagram**: Technical diagram imported and validated during request processing
- **Job Design**: Engineering deliverable separate from pricing
- **Program**: Complete deliverable including job design and pricing
- **Service Report**: Customer-facing document generated from program
- **Service Ticket**: Operational document generated from program
- **Triage**: Automated or manual assignment of requests to appropriate engineers

## Out of Scope (Phase 1)

- Sales workflow beyond basic pricing interface
- Operations workflow management
- Advanced reporting and analytics
- Mobile application interface
- External client portal access