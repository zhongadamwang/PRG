<!-- Identifier: V-01 -->

# Vocabulary: Program Request Management

## Domain Terminology Standards

This vocabulary establishes canonical terminology for the Program Request Management process to ensure consistent communication across stakeholders, documentation, and system interfaces.

## Core Process Terms

### Request Management
| Term | Definition | Aliases | Context |
|------|------------|---------|---------|
| **Program Request** | A formal request for engineering work to develop technical programs based on client specifications | Request, Ticket, Work Order | Primary workflow entity |
| **Request Status** | Current state of a request in the workflow process | Status, State, Phase | Workflow tracking |
| **Request ID** | Unique identifier assigned to each program request | Ticket Number, Reference ID | System identification |
| **Client** | External organization or internal department requesting engineering work | Customer, Requestor | Business relationship |

### Workflow States  
| Term | Definition | Entry Condition | Exit Condition |
|------|------------|----------------|----------------|
| **Open** | Request created but not yet assigned to an engineer | System creation, Engineer decline, Reassignment | Manager assignment |
| **Assigned** | Request allocated to specific engineer, awaiting acknowledgment | Manager assignment action | Engineer accept/decline/reassign |
| **Acknowledged** | Engineer has accepted responsibility for the request | Engineer acceptance | Begin data validation |
| **In Progress** | Engineer actively working on request development | Data validation start | Submit for review |
| **Awaiting Review** | Completed work submitted, waiting for peer reviewer | Work submission | Reviewer assignment |
| **Review** | Peer review in progress by assigned team member | Reviewer acceptance | Approve or request adjustments |
| **Program Ready** | Approved work package ready for operational deployment | Review approval | Process completion |

### Assignment and Responsibility
| Term | Definition | Scope | Authority Level |
|------|------------|-------|----------------|
| **Assignment** | Allocation of a request to a specific engineer for completion | Single engineer per request | Manager decision |
| **Acknowledgment** | Engineer's formal acceptance of assignment responsibility | Assigned engineer only | Engineer choice |
| **Reassignment** | Transfer of request from one engineer to another | Current assignee or manager | Engineer or manager action |
| **Escalation** | Process intervention when SLAs are not met | System or manual trigger | Management intervention |

## Actor and Role Definitions

### Human Actors
| Role | Primary Functions | Decision Authority | System Access Level |
|------|------------------|-------------------|-------------------|
| **Manager** | Request assignment, escalation handling, team oversight | Full assignment control | Administrative access |
| **Engineer** | Request processing, data validation, technical development | Own work and assignments | User access with validation tools |
| **Reviewer** | Quality assessment, peer review, approval decisions | Review approval/rejection | Review-specific access |
| **System Administrator** | System configuration, user management, integration support | Full system control | Administrative configuration |

### System Actors
| System | Function | Interface Type | Integration Level |
|--------|----------|----------------|------------------|
| **Email Integration** | Automated request intake and stakeholder notifications | SMTP/IMAP | Deep integration |
| **State Diagram Import** | Technical document parsing and data extraction | File processing | Service integration |
| **Authentication Directory** | User identity and role management | LDAP/SAML | Security integration |
| **Notification System** | Multi-channel communication and alerts | REST/Webhook | Service integration |

## Data and Information Terms

### Data Management
| Term | Definition | Data Type | Validation Requirements |
|------|------------|-----------|----------------------|
| **State Diagram** | Engineering document containing technical specifications | PDF/CAD file | Automated parsing with human validation |
| **Data Element** | Individual data point extracted from state diagrams | Structured data | Mandatory engineer validation before use |
| **Validation** | Process of verifying and correcting extracted data | Interactive process | 100% completion required |
| **Work Package** | Complete set of deliverables for peer review | Document collection | Quality standards compliance |

### Quality and Process Control  
| Term | Definition | Success Criteria | Measurement |
|------|------------|-----------------|-------------|
| **Peer Review** | Collaborative quality assessment by engineering team | Technical accuracy and completeness | Review approval |
| **Quality Gate** | Checkpoint requiring validation before process continuation | Defined quality criteria met | Pass/fail determination |
| **SLA (Service Level Agreement)** | Time-based performance commitment | Response within defined timeframe | Automated monitoring |
| **Audit Trail** | Complete record of actions and decisions | All process steps documented | System logging |

## Technical and System Terms

### System Components
| Component | Purpose | Technology | Integration Points |
|-----------|---------|------------|------------------|
| **Data Validation Interface** | Three-panel engineer validation tool | Web application | State diagram system, data repository |
| **Request Repository** | Central data store for all request information | Database system | All workflow components |
| **Workflow Engine** | Process automation and state management | Business process system | All actors and systems |
| **Notification Gateway** | Multi-channel communication hub | Email and web services | External email systems |

### Integration Terminology 
| Term | Definition | Protocol | Security Requirements |
|------|------------|----------|---------------------|
| **API Integration** | Programmatic interface for system communication | REST/SOAP | Authenticated and encrypted |
| **Email Integration** | Bidirectional email communication with embedded actions | SMTP/IMAP | Secure email processing |
| **File Processing** | Automated document parsing and data extraction | HTTP upload | Virus scanning and validation |
| **Single Sign-On (SSO)** | Unified authentication across integrated systems | SAML/OAuth | Identity federation |

## Business and Performance Terms

### Metrics and Performance
| Term | Definition | Measurement Unit | Target Value |
|------|------------|-----------------|--------------|
| **Throughput** | Number of requests processed per time period | Requests per day/week | 80% improvement over manual |
| **Response Time** | Time from trigger to required action | Hours or days | <4h acknowledgment, <24h review |
| **First-Pass Quality** | Percentage of work approved without rework | Percentage | >85% approval rate |
| **SLA Compliance** | Percentage of activities completed within target time | Percentage | >95% compliance |

### Business Value
| Term | Definition | Impact Area | Measurement Method |
|------|------------|-------------|------------------|
| **Process Automation** | Replacement of manual tasks with system processes | Efficiency improvement | Time savings calculation |
| **Quality Assurance** | Systematic approach to preventing defects | Quality improvement | Defect rate reduction |
| **Stakeholder Visibility** | Real-time access to process status and progress | Communication enhancement | Response time improvement |
| **Resource Optimization** | Efficient allocation of engineering capacity | Resource utilization | Capacity utilization metrics |

## Communication Standards

### Notification Templates
| Notification Type | Purpose | Recipients | Action Required |
|------------------|---------|------------|----------------|
| **Assignment Notification** | Inform of new request assignment | Team and assigned engineer | Accept/decline/reassign |
| **Status Update** | Report workflow progress | Relevant stakeholders | Information only |
| **Escalation Alert** | Report SLA violations or delays | Management and responsible parties | Investigation and action |
| **Review Request** | Request peer review participation | Engineering team | Volunteer for review |

### Status Communication
| Status | Standard Message | Stakeholder Impact | Next Action Communication |
|--------|-----------------|-------------------|-------------------------|
| **Open** | "Request awaiting assignment" | Manager attention required | "Please assign to engineer" |
| **Assigned** | "Request assigned, awaiting acknowledgment" | Engineer attention required | "Please accept or decline assignment" |
| **In Progress** | "Engineer working on request" | Work in progress | "Data validation and job design underway" |
| **Awaiting Review** | "Work completed, seeking reviewer" | Team attention required | "Volunteer for peer review" |
| **Program Ready** | "Request completed and approved" | All stakeholders | "Program ready for deployment" |

## Compliance and Standards

### Quality Standards
| Standard | Application | Requirement Level | Validation Method |
|----------|-------------|------------------|------------------|
| **Data Accuracy** | All extracted and validated data | 100% validation required | Engineer verification |
| **Review Quality** | All peer review activities | Constructive feedback mandatory | Review template compliance |
| **Documentation Completeness** | All work packages | Complete deliverable set | Checklist validation |
| **Audit Compliance** | All process activities | Full traceability required | System audit trail |

### Terminology Governance
- **Change Control**: All terminology updates require stakeholder review and approval
- **Version Management**: Vocabulary versioned with process documentation updates  
- **Training Integration**: New terms incorporated into user training materials
- **System Alignment**: Technical systems updated to reflect canonical terminology

This vocabulary serves as the authoritative reference for all Program Request Management communications, ensuring clarity and consistency across the organization.