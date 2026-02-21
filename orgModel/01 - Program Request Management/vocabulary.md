<!-- Identifier: V-01 -->

# Vocabulary: Program Request Management

## Domain Terminology Standards

This vocabulary establishes canonical terminology for the Program Request Management process to ensure consistent communication across stakeholders, documentation, and system interfaces. **Phase 1 Focus**: Terminology prioritizes manual interface concepts with automation terms designated for future phases.

## Core Process Terms

### Request Management
| Term | Definition | Aliases | Context | **Phase** |
|------|------------|---------|---------|----------|
| **Program Request** | A formal request for engineering work to develop technical programs based on client specifications | Request, Ticket, Work Order | Primary workflow entity | Phase 1 |
| **Manual Request Creation** | Web form-based interface for creating new program requests | Request Form, Manual Entry | Phase 1 interface method | Phase 1 |
| **Request Status** | Current state of a request in the workflow process | Status, State, Phase | Workflow tracking | Phase 1 |
| **Request ID** | Unique identifier assigned to each program request | Ticket Number, Reference ID | System identification | Phase 1 |
| **Client** | External organization or internal department requesting engineering work | Customer, Requestor | Business relationship | Phase 1 |
| **State Diagram Data** | Technical engineering data entered manually through structured forms | State Data, Technical Data | Manual data entry context | Phase 1 |
| **Automated Request Processing** | Future capability for email-based request creation | Email Integration, Auto-Processing | Future automation feature | Future |

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
| Term | Definition | Scope | Authority Level | **Phase** |
|------|------------|-------|----------------|----------|
| **Assignment** | Allocation of a request to a specific engineer for completion via web interface | Single engineer per request | Manager decision | Phase 1 |
| **Web-based Acknowledgment** | Engineer's formal acceptance of assignment responsibility through dashboard | Assigned engineer only | Engineer choice via web interface | Phase 1 |
| **Manual Assignment** | Manager assigns requests through web interface with engineer workload visibility | Manager-initiated process | Manager authority | Phase 1 |
| **Reassignment** | Transfer of request from one engineer to another via web interface | Current assignee or manager | Engineer or manager action | Phase 1 |
| **Manual Escalation** | Process intervention when SLAs are not met, handled through management dashboard | System reports, manual action | Management intervention | Phase 1 |
| **Email-based Acknowledgment** | Future capability for engineers to respond via email buttons | Future automation feature | Future system capability | Future |

## Actor and Role Definitions

### Human Actors
| Role | Primary Functions | Decision Authority | System Access Level |
|------|------------------|-------------------|-------------------|
| **Manager** | Request assignment, escalation handling, team oversight | Full assignment control | Administrative access |
| **Engineer** | Request processing, data validation, technical development | Own work and assignments | User access with validation tools |
| **Reviewer** | Quality assessment, peer review, approval decisions | Review approval/rejection | Review-specific access |
| **System Administrator** | System configuration, user management, integration support | Full system control | Administrative configuration |

### System Actors

#### Phase 1 Systems (Manual Interface)
| System | Function | Interface Type | Integration Level | **Phase** |
|--------|----------|----------------|------------------|----------|
| **Web Application Interface** | Manual request creation and workflow management | Web forms/dashboard | Core system | Phase 1 |
| **Authentication Directory** | User identity and role management | LDAP/SAML | Security integration | Phase 1 |
| **Data Entry Interface** | Structured manual data entry for state diagrams | Web forms | Form validation | Phase 1 |
| **Dashboard System** | Real-time status tracking and notifications | Web UI | User interface | Phase 1 |

#### Future Systems (Automation Enhancement)
| System | Function | Interface Type | Integration Level | **Phase** |
|--------|----------|----------------|------------------|----------|
| **Email Integration** | Automated request intake and stakeholder notifications | SMTP/IMAP | Deep integration | Future |
| **State Diagram Import** | Technical document parsing and data extraction | File processing | Service integration | Future |
| **Notification System** | Multi-channel communication and alerts | REST/Webhook | Service integration | Future |

## Data and Information Terms

### Data Management
| Term | Definition | Data Type | Validation Requirements | **Phase** |
|------|------------|-----------|----------------------|----------|
| **State Diagram** | Engineering document uploaded for manual data entry | PDF/CAD file | Manual entry with form validation | Phase 1 |
| **State Diagram Data** | Technical specifications entered manually from uploaded diagrams | Structured form data | Manual validation during entry | Phase 1 |
| **Data Element** | Individual data point entered through structured forms | Form input data | Real-time form validation | Phase 1 |
| **Manual Data Entry** | Process of inputting state diagram information through web forms | Interactive forms | 100% completion check | Phase 1 |
| **Work Package** | Complete set of deliverables for peer review | Document collection | Quality standards compliance | Phase 1 |
| **Automated Data Extraction** | Future capability to parse state diagrams automatically | Parsed data | Future automated validation | Future |

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

#### Phase 1 (Manual Interface Metrics)
| Term | Definition | Measurement Unit | Target Value | **Phase** |
|------|------------|-----------------|--------------|----------|
| **Manual Processing Efficiency** | Speed of manual request creation and data entry | Requests per hour | 2-3 requests per hour | Phase 1 |
| **Form Completion Time** | Time to complete manual request forms | Minutes per request | <15 minutes average | Phase 1 |
| **Response Time** | Time from trigger to required action | Hours or days | <4h acknowledgment, <24h review | Phase 1 |
| **First-Pass Quality** | Percentage of work approved without rework | Percentage | >85% approval rate | Phase 1 |
| **SLA Compliance** | Percentage of activities completed within target time | Percentage | >95% compliance | Phase 1 |

#### Future Phase (Automation Metrics)  
| Term | Definition | Measurement Unit | Target Value | **Phase** |
|------|------------|-----------------|--------------|----------|
| **Throughput** | Number of requests processed per time period | Requests per day/week | 80% improvement over manual baseline | Future |
| **Automated Processing Time** | Time for automated parsing and validation | Minutes per request | <5 minutes processing | Future |

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