# Business Requirements Document (BRD)
## Project: AI-Agentic GRC Platform (Scrut-like)
## Tech Stack: ASP.NET Core (API) + PostgreSQL

---

## 1. Overview

### 1.1 Purpose
This BRD defines the business and functional requirements for building a cloud-based, AI-agentic Governance, Risk and Compliance (GRC) platform similar to Scrut, using ASP.NET Core for backend services and PostgreSQL as the primary data store.[page:0]

### 1.2 Objectives
- Provide a unified view of security, risk and compliance posture across infrastructure, applications, people and vendors.[page:0]
- Automate evidence collection, control monitoring and recurring workflows to minimize manual effort.[page:0]
- Enable continuous audit readiness across multiple frameworks (e.g., SOC 2, ISO 27001, GDPR, HIPAA, PCI DSS).
- Embed AI agents to handle questionnaires, generate documentation and orchestrate remediation workflows.

### 1.3 Scope
In scope for MVP:
- Multi-framework compliance management.
- Unified control and policy library.
- Risk and vendor management.
- Integrations for at least one cloud provider and one task management system.
- AI-driven assistance for documentation and questionnaire responses.

Out of scope for MVP:
- Deep domain-specific content for niche industries.
- Built-in penetration testing engine.

---

## 2. Stakeholders and Personas

### 2.1 Stakeholders
- Product Owner
- CISO / Head of Security
- Security / GRC Manager
- Engineering / DevOps Lead
- External Auditors

### 2.2 User Personas
- **CISO / Head of Security**: Needs executive-level dashboards and insights.
- **GRC Manager**: Manages controls, risks, evidence, and audits.
- **DevOps / Engineering**: Receives remediation tasks and acts on technical findings.
- **Auditor**: Reviews evidence, controls, and audit trails.

---

## 3. High-Level Functional Requirements

### 3.1 Dashboard & Reporting
- Provide real-time dashboards showing:
  - Compliance status per framework and overall posture.
  - Control health and failing tests.[page:0]
  - Open remediation tasks and overdue items.
- Support filters by framework, business unit, and severity.
- Export reports (PDF/CSV) for management and auditors.

### 3.2 Framework & Control Management
- Maintain a repository of compliance frameworks (SOC 2, ISO 27001, etc.).
- Define a unified control library, with a single control mapped to multiple frameworks.[page:0]
- Allow creation of custom frameworks and controls.
- Versioning of controls and frameworks.

### 3.3 Policy & Evidence Management
- Store policies, procedures and related documents in PostgreSQL-backed storage (metadata) with external file storage for binaries.
- Map policies to controls and frameworks.
- Automatically collect and associate evidence from integrations.[page:0]
- Maintain full audit trail of evidence updates and approvals.

### 3.4 Risk Management
- Maintain a risk register with fields: title, description, owner, likelihood, impact, score, mitigation plans.
- Link risks to controls, assets and remediation tasks.
- Provide risk heatmap and risk trend views.

### 3.5 Vendor Management
- Maintain vendor master data with risk tier, services, and contact info.
- Support vendor assessments via questionnaires.
- Link vendor risks to overall risk posture.[page:0]

### 3.6 Workflows & Tasks
- Create and assign tasks for remediation, evidence requests and reviews.
- Integrate with external task tools (e.g., Jira) to sync task status.
- Support SLAs, due dates and notifications (email/Slack/etc.).

### 3.7 Integrations & Continuous Monitoring
- Provide connectors for:
  - Cloud providers (e.g., AWS).
  - Identity providers (e.g., Okta, Azure AD).
  - Ticketing systems (e.g., Jira).
- Run scheduled control checks and ingest configuration or log data.
- Trigger alerts when controls fail, and automatically create remediation tasks.[page:0]

### 3.8 AI / Agentic Features
- AI assistant for:
  - Drafting responses to security questionnaires using stored policies and controls.
  - Generating policy drafts and audit narratives.
  - Summarizing evidence and control status for auditors.
- Agentic workflows:
  - Orchestrate multi-step operations (e.g., “Prepare SOC 2 readiness report”).
  - Invoke backend APIs to create/update tasks and risks.
  - Offer natural language interface to query posture and delegate tasks.

---

## 4. Non-Functional Requirements

### 4.1 Security
- Enforce authentication and authorization using ASP.NET Core Identity or external IdP.
- Encrypt data in transit (TLS) and at rest (PostgreSQL encryption where applicable).
- Maintain audit logs of all user and system actions.

### 4.2 Performance & Scalability
- Support thousands of controls, risks and tasks per tenant without performance degradation.
- The API must respond within agreed SLAs for key user journeys (e.g., dashboard load < 3 seconds under nominal load).

### 4.3 Availability & Reliability
- Design for high availability with redundancy on application and database layers.
- Implement health checks and graceful handling of integration failures.

### 4.4 Maintainability & Extensibility
- Use modular architecture (e.g., clean architecture / hexagonal) to ease addition of new frameworks and integrations.
- Use migration tools (e.g., EF Core Migrations) for PostgreSQL schema evolution.

---

## 5. System Architecture (High-Level)

### 5.1 Application Layers
- **API Layer (ASP.NET Core)**:
  - RESTful endpoints for UI and external integrations.
  - Authentication/authorization middleware.
- **Business Layer**:
  - Services for controls, frameworks, risks, vendors, tasks, AI orchestration.
- **Data Access Layer**:
  - Repository pattern/EF Core accessing PostgreSQL.
  - Separate schemas per tenant if needed.

### 5.2 Data Storage (PostgreSQL)
- Core tables (indicative):
  - `Tenants`
  - `Users`, `Roles`, `UserRoles`
  - `Frameworks`, `Controls`, `FrameworkControls`
  - `Policies`, `Evidence`, `EvidenceAuditTrail`
  - `Risks`, `RiskControls`
  - `Vendors`, `VendorQuestionnaires`, `VendorRisks`
  - `Tasks`, `Integrations`, `IntegrationEvents`

### 5.3 AI / Agent Integration
- AI orchestration service responsible for:
  - Calling LLM APIs with context from PostgreSQL.
  - Executing agent “tools” via internal service APIs.
  - Storing conversation history and generated artifacts (reports, drafts).

---

## 6. User Stories (Sample)

- As a CISO, I want to view a consolidated dashboard of compliance and risk so that I can understand our security posture at a glance.
- As a GRC Manager, I want to map a single control to multiple frameworks so that I avoid duplicated work.[page:0]
- As a DevOps Engineer, I want remediation tasks to be auto-created in Jira when a control fails so that I can fix issues quickly.
- As an Auditor, I want to see evidence and change history for each control so that I can verify compliance efficiently.

---

## 7. Assumptions & Constraints

- Backend implemented using ASP.NET Core (latest LTS) with RESTful APIs.
- PostgreSQL is the primary relational database.
- AI layer relies on external LLM providers (e.g., OpenAI/Azure OpenAI) with appropriate security controls.
- Frontend (web) will consume the .NET Core APIs but is out of scope for this BRD except for core flows.

---

## 8. Acceptance Criteria (MVP)

- Users can:
  - Onboard a tenant and configure at least one framework.
  - Define and map controls and upload policies.
  - Create and track risks, vendors and tasks.
  - Integrate at least one cloud provider and see automated control results.
  - Use AI to generate at least one type of document (policy draft / questionnaire response).
- System:
  - Stores all data in PostgreSQL.
  - Provides audit logs for critical actions.
  - Meets defined performance and security requirements.

---
