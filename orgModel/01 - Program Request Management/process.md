<!-- Identifier: P-01 -->

# Process: Program Request Management

## Process Overview
The Program Request Management process defines the complete workflow for processing engineering program requests from initial email submission through final program delivery. The process emphasizes automation, clear status tracking, collaborative assignment management, and structured quality assurance.

## Process Goals
- Achieve 80% reduction in manual processing through automation
- Provide real-time visibility into request status for all stakeholders  
- Enable efficient engineer workload management
- Ensure quality control through mandatory peer review
- Generate complete job design ready for operational deployment

## Workflow States

```mermaid
stateDiagram-v2
    [*] --> Open : Email Received
    Open --> Assigned : Manager Assignment
    Assigned --> Open : Engineer Declines
    Assigned --> Acknowledged : Engineer Accepts
    Acknowledged --> InProgress : Begin Data Validation
    InProgress --> AwaitingReview : Submit for Review
    AwaitingReview --> Review : Reviewer Accepts
    Review --> InProgress : Adjustments Needed
    Review --> ProgramReady : Review Approved
    ProgramReady --> [*] : Process Complete
    
    Open --> Open : Escalation Timeout
    Assigned --> Open : Reassignment
```

### State Definitions

| Status | Description | Entry Conditions | Duration | Next Actions |
|--------|-------------|-----------------|----------|--------------|
| **Open** | New request, not yet assigned | Request created, Engineer declined, Request unassigned | Target: <24h | Manager assignment, Auto-triage |
| **Assigned** | Assigned to engineer, awaiting acknowledgment | Manager assigns request | Target: <4h | Engineer accept/decline/reassign |  
| **Acknowledged** | Engineer accepted assignment | Engineer accepts assignment | Brief transition | Begin data validation |
| **In Progress** | Engineer working on request | Engineer begins data validation | Variable (days) | Submit for review |
| **Awaiting Review** | Work submitted for team review | Engineer submits for review | Target: <24h | Team member accepts review |
| **Review** | Review in progress by team member | Team member accepts review | Target: <48h | Approve or request adjustments |
| **Program Ready** | Approved and ready for next phase | Review approved | End state | Process completion |

## Process Activities

### 1. Request Intake and Creation

**Activity**: Automated Email Processing
- **Trigger**: Email received at designated request inbox
- **System Actions**:
  - Parse email content and attachments
  - Extract client and project information
  - Import associated state diagram data
  - Generate unique request identifier
- **Outputs**: 
  - New request in "Open" status
  - Parsed data in draft state requiring validation
- **SLA**: <5 minutes

**Activity**: Initial Request Assessment  
- **Trigger**: Request created in Open status
- **System Actions**:
  - Categorize request type and complexity
  - Identify potential engineer matches based on client/skills
  - Set preliminary priority based on content analysis
- **Outputs**: Request ready for assignment with suggested assignments

### 2. Assignment and Acknowledgment

**Activity**: Manager Assignment
- **Actor**: Manager (Jeremy, Warren)
- **Trigger**: Request in Open status
- **Actions**:  
  - Review request details and suggested assignments
  - Assign to specific engineer based on workload and expertise
  - Override auto-triage suggestions if needed
- **System Actions**:
  - Change status to "Assigned"
  - Send notification to entire team (not just assigned engineer)
  - Start acknowledgment timer
- **SLA**: <24 hours

**Activity**: Engineer Response
- **Actor**: Assigned Engineer 
- **Trigger**: Request assigned notification received
- **Actions Available**:
  - **Accept**: Acknowledge assignment and proceed
  - **Decline**: Reject with reason (returns to Open)
  - **Reassign**: Transfer to another engineer with justification
- **System Actions**:
  - Process response selection via email buttons  
  - Update status based on engineer choice
  - Send follow-up notifications to stakeholders
- **SLA**: <4 hours

**Activity**: Escalation Management
- **Trigger**: No engineer response within SLA
- **System Actions**:
  - Send escalation alert to manager
  - Log unacknowledged status for reporting
  - Optionally auto-reassign based on escalation rules
- **Manual Actions**: Manager intervention and reassignment

### 3. Data Validation and Work Process

**Activity**: Data Validation Setup  
- **Actor**: Engineer
- **Trigger**: Request acknowledgment completed
- **Actions**:
  - Open data validation interface
  - Review three-panel view: data tree, state diagram, data details
  - Identify parsing discrepancies or data gaps
- **System Actions**:
  - Change status to "In Progress" upon interface access
  - Lock data for validation to prevent concurrent editing
  - Provide correction tools and validation support

**Activity**: Data Validation and Correction
- **Actor**: Engineer
- **Process**:
  1. Review automatically parsed data against state diagram
  2. Identify and flag inconsistencies or errors
  3. Make corrections using provided tools
  4. Validate corrected data against business rules
  5. Approve final validated dataset for job design
- **System Actions**:
  - Track validation progress and completion
  - Generate audit trail of corrections made
  - Unlock data for job design work once validated

**Activity**: Job Design Development
- **Actor**: Engineer  
- **Process**:
  1. Use validated data for technical design work
  2. Develop comprehensive program specifications
  3. Generate required documentation and outputs
  4. Prepare work package for peer review
- **Duration**: Variable based on complexity (typically 1-5 days)

### 4. Review and Quality Assurance

**Activity**: Review Submission
- **Actor**: Engineer
- **Trigger**: Job design work completed
- **Actions**:
  - Compile completed work package
  - Submit for team review with work summary
  - Include any specific review focus areas
- **System Actions**:
  - Change status to "Awaiting Review" 
  - Notify entire engineering team of review availability
  - Create review task in queue

**Activity**: Review Assignment
- **Actor**: Engineering Team Member
- **Trigger**: Review notification received
- **Process**:
  - Review team member volunteers for review task
  - System assigns review to first volunteer
  - Status changes to "Review" with assigned reviewer
- **SLA**: <24 hours for review acceptance

**Activity**: Peer Review Process  
- **Actor**: Assigned Reviewer
- **Process**:
  1. Review work package for technical accuracy
  2. Validate against requirements and standards
  3. Check data validation quality and completeness
  4. Provide feedback and recommendations
- **Outcomes**:
  - **Approved**: Work meets standards, proceed to Program Ready
  - **Adjustments Needed**: Return to In Progress with specific feedback
- **SLA**: <48 hours for review completion

### 5. Process Completion

**Activity**: Final Approval  
- **Trigger**: Peer review approved
- **System Actions**:
  - Change status to "Program Ready"  
  - Generate final program package
  - Notify stakeholders of completion
  - Archive request with full audit trail
- **Outputs**:
  - Complete job design ready for operational deployment
  - Audit trail and quality documentation
  - Performance metrics for process improvement

## Escalation Procedures

### Assignment Escalation
- **Trigger**: No engineer acknowledgment within 4 hours
- **Actions**: Manager notification, workload review, potential reassignment
- **Resolution**: Manual intervention by management

### Review Escalation  
- **Trigger**: No review acceptance within 24 hours
- **Actions**: Management notification, resource allocation review
- **Resolution**: Management assignment of reviewer

### Quality Escalation
- **Trigger**: Multiple adjustment cycles or significant quality concerns
- **Actions**: Senior engineer involvement, process review
- **Resolution**: Additional mentoring or process improvements

## Integration Points

### Email Integration
- **Incoming**: Designated request inbox monitoring
- **Outgoing**: Status notifications, assignment alerts, review requests
- **Requirements**: Secure email processing with attachment handling

### State Diagram Integration  
- **Function**: Automated import and parsing of engineering diagrams
- **Validation**: Data integrity checks and human validation interface
- **Storage**: Secure document management with version control

### User Management Integration
- **Authentication**: AD integration for role-based access
- **Teams**: Engineering team membership and manager hierarchies  
- **Permissions**: Role-based workflow access and action restrictions

## Performance Metrics

### Throughput Metrics
- Requests processed per engineer per period
- Average time in each workflow state
- End-to-end process completion time

### Quality Metrics  
- Data validation accuracy rates
- Review pass rates (first-time approval)
- Rework frequency and reasons

### Efficiency Metrics
- Automated vs manual triage success rate
- Acknowledgment response time compliance
- Review assignment and completion timeliness

## Process Variations

### Express Processing
- **Criteria**: Urgent requests or simple program types  
- **Modifications**: Accelerated SLAs, priority reviewer assignment
- **Approval**: Manager override required

### Complex Program Handling
- **Criteria**: Multi-phase programs or novel technical challenges
- **Modifications**: Senior engineer review, extended validation time
- **Approval**: Technical lead involvement required