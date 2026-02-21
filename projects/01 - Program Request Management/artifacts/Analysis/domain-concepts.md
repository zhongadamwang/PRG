# Domain Concepts Analysis: Program Request Management (PRG-01)

**Project**: PRG-01  
**Generated**: 2026-02-20T16:00:00Z  
**Updated**: 2026-02-21T00:00:00Z (Strategy Update)  
**Source**: requirements.json, goals.json  
**Total Entities**: 12 | **Total Concepts**: 18  
**Confidence**: 87%

## Strategy Update

**Implementation Approach**: Human interface first, then implement automation. Manual interface concepts prioritized for MVP development.

## Domain Areas

### Core Workflow
**Key Entities**: Request, Assignment  
**Core Concepts**: Manual Assignment Workflow, Status Management, Manual Escalation  
**Primary Processes**: Manual Request Creation, Assignment, Status Progression

### User Management  
**Key Entities**: Engineer, Manager  
**Core Concepts**: Role-based Access, Team Management, Manual Coordination  
**Primary Processes**: User Authentication, Role Assignment, Manual Team Coordination

### Data Management
**Key Entities**: StateData  
**Core Concepts**: Manual Data Entry, Data Validation, Form-based Input  
**Primary Processes**: Manual Data Entry, Validation, Correction

### Quality Control
**Key Entities**: Review  
**Core Concepts**: Peer Review, Quality Assurance, Manual Review Assignment  
**Primary Processes**: Review Submission, Manual Review Assignment, Approval

### Communication
**Key Entities**: Notification  
**Core Concepts**: Web Interface Notifications, Manual Action Processing  
**Primary Processes**: Web Notification Delivery, Manual Action Processing

### Future Domain Areas (Automation Phase)
**Core Concepts**: Automated Triage, Email Integration, Escalation Rules  
**Primary Processes**: Auto-assignment, Timeout Handling, Rule Processing

## Core Entities

### Request *(ENT-001)*
**Domain Area**: Core Workflow  
**Description**: Program request submitted for engineering work

**Attributes**:
- `request_id` (identifier): Unique request identifier [public]
- `status` (enumeration): Current request status [open, assigned, acknowledged, in_progress, awaiting_review, review, program_ready] [public]
- `created_date` (datetime): Request creation timestamp [public]
- `priority` (enumeration): Request priority level [high, medium, low, urgent] [public]
- `client_id` (identifier): Associated client identifier [public]
- `source_email` (string): Originating email content [private]

**Operations**:
- `assign(engineer_id, manager_id)` → boolean: Assign request to engineer [public]
- `updateStatus(new_status, user_id)` → void: Change request status [public]
- `escalate(escalation_reason)` → void: Escalate unacknowledged request [public]

*Source: R-001, R-003, R-005, R-027 | Confidence: 95%*

### Engineer *(ENT-002)*
**Domain Area**: User Management  
**Description**: Engineering staff member who processes requests

**Attributes**:
- `engineer_id` (identifier): Unique engineer identifier [public]
- `name` (string): Engineer full name [public]
- `email` (string): Engineer email address [public]
- `specialties` (list): Engineering specialization areas [public]
- `workload_capacity` (integer): Maximum concurrent requests [private]

**Operations**:
- `acceptAssignment(request_id)` → boolean: Accept assigned request [public]
- `declineAssignment(request_id, reason)` → boolean: Decline assigned request [public]
- `reassignRequest(request_id, target_engineer_id)` → boolean: Reassign request to another engineer [public]
- `validateData(request_id, validation_results)` → boolean: Validate imported state diagram data [public]

*Source: R-008, R-009, R-010, R-013 | Confidence: 90%*

### Manager *(ENT-003)*
**Domain Area**: User Management  
**Description**: Management user who assigns and oversees requests

**Attributes**:
- `manager_id` (identifier): Unique manager identifier [public]
- `name` (string): Manager full name [public]
- `department` (string): Organizational department [public]
- `authorization_level` (enumeration): Management authorization level [supervisor, senior_manager, director] [private]

**Operations**:
- `assignRequest(request_id, engineer_id)` → boolean: Assign request to engineer [public]
- `escalateRequest(request_id, escalation_type)` → void: Escalate problematic request [public]
- `overrideTriage(request_id, manual_assignment)` → boolean: Override automated triage assignment [public]

*Source: R-005, R-027, R-034 | Confidence: 85%*

### StateData *(ENT-004)*
**Domain Area**: Data Management  
**Description**: Imported diagram data requiring validation

**Attributes**:
- `data_id` (identifier): Unique data identifier [public]
- `request_id` (identifier): Associated request identifier [public]
- `raw_content` (text): Original imported data content [private]
- `parsed_data` (json): Processed and structured data [public]
- `validation_status` (enumeration): Data validation state [pending, validated, requires_correction] [public]
- `corrections` (json): Applied data corrections [public]

**Operations**:
- `parseData(parsing_rules)` → json: Parse imported raw data [private]
- `validateData(validation_criteria)` → boolean: Validate parsed data accuracy [public]
- `applyCorrections(correction_data, engineer_id)` → boolean: Apply engineer corrections to data [public]

*Source: R-002, R-013, R-015 | Confidence: 88%*

### Review *(ENT-005)*
**Domain Area**: Quality Control  
**Description**: Peer review process for completed work

**Attributes**:
- `review_id` (identifier): Unique review identifier [public]
- `request_id` (identifier): Request being reviewed [public]
- `reviewer_id` (identifier): Assigned reviewer identifier [public]
- `review_status` (enumeration): Review progress status [pending, in_progress, completed] [public]
- `review_result` (enumeration): Review outcome [approved, adjustments_required] [public]
- `comments` (text): Reviewer feedback and comments [public]
- `created_date` (datetime): Review creation timestamp [public]

**Operations**:
- `acceptReview(reviewer_id)` → boolean: Accept review responsibility [public]
- `completeReview(result, comments)` → void: Complete review with outcome [public]
- `requestAdjustments(adjustment_details)` → void: Request work adjustments [public]

*Source: R-017, R-018, R-020, R-022 | Confidence: 90%*

### Notification *(ENT-006)*
**Domain Area**: Communication  
**Description**: System notification to users

**Attributes**:
- `notification_id` (identifier): Unique notification identifier [public]
- `recipient_id` (identifier): Notification recipient [public]
- `type` (enumeration): Notification type [assignment, escalation, review_request, status_change] [public]
- `content` (text): Notification message content [public]
- `action_buttons` (list): Available action buttons [accept, decline, reassign] [public]
- `sent_date` (datetime): Notification send timestamp [public]

**Operations**:
- `send(delivery_method)` → boolean: Send notification to recipient [public]
- `processAction(action_type, user_id)` → boolean: Process user action from notification [public]

*Source: R-007, R-008, R-012 | Confidence: 85%*

## Business Concepts

### Assignment Workflow *(CON-001)*
**Domain Area**: Core Workflow  
**Definition**: Process of assigning requests to engineers and managing responses  
**Synonyms**: task assignment, work allocation  
*Source: R-005, R-008, R-009 | Confidence: 92%*

### Data Validation *(CON-002)*
**Domain Area**: Data Management  
**Definition**: Process of verifying and correcting imported state diagram data  
**Synonyms**: data verification, data correction  
*Source: R-013, R-014, R-015 | Confidence: 90%*

### Peer Review *(CON-003)*
**Domain Area**: Quality Control  
**Definition**: Team-based quality control review of completed engineering work  
**Synonyms**: quality review, work approval  
*Source: R-017, R-019, R-022 | Confidence: 88%*

### Escalation *(CON-004)*
**Domain Area**: Process Management  
**Definition**: Automated process for handling unacknowledged or overdue requests  
**Synonyms**: timeout handling, automatic escalation  
*Source: R-027, R-028 | Confidence: 85%*

### Automated Triage *(CON-005)*
**Domain Area**: Automation  
**Definition**: System-based assignment of requests based on predefined rules  
**Synonyms**: auto-assignment, intelligent routing  
*Source: R-033, R-034 | Confidence: 80%*

## Key Terminology

| Term | Definition | Context | Domain Area |
|------|------------|---------|-------------|
| **State Diagram** | Technical diagram containing job specifications imported during request processing | Engineering data import and validation | Technical |
| **Job Design** | Engineering deliverable representing completed technical work separate from pricing | Engineering workflow output | Core Business |
| **Program Ready** | Status indicating completed engineering work ready for next phase | Workflow status management | Core Workflow |
| **Acknowledgment** | Engineer confirmation of assignment acceptance | Assignment response workflow | Core Workflow |

## Entity Relationships

```
Request ←→ Engineer (many-to-one) [Assignment]
Request ←→ StateData (one-to-one) [Contains]
Request ←→ Review (one-to-many) [Undergoes]
Engineer ←→ Review (many-to-many) [Participates]
Manager ←→ Request (one-to-many) [Assigns]
Review → Request (dependency) [Depends on completion]
```

## Workflow State Model

```
[Open] → [Assigned] → [Acknowledged] → [In Progress] → [Awaiting Review] → [Review] → [Program Ready]
    ↑         ↓
    └─[Declined]─┘
```

**State Transitions**:
- **Open → Assigned**: Manager assigns request
- **Assigned → Acknowledged**: Engineer accepts assignment  
- **Assigned → Open**: Engineer declines assignment
- **Acknowledged → In Progress**: Engineer begins data validation
- **In Progress → Awaiting Review**: Engineer submits for review
- **Awaiting Review → Review**: Team member accepts review
- **Review → Program Ready**: Review approved
- **Review → In Progress**: Adjustments requested (restart cycle)

---
**Analysis Methodology**: Domain-driven design patterns with entity extraction, concept identification, and relationship mapping  
**Validation**: Cross-referenced with 47 extracted requirements  
**Traceability**: All entities and concepts traced to source requirements