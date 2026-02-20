# Audit Management Platform

A comprehensive AI-Agentic GRC (Governance, Risk, and Compliance) Platform built with ASP.NET Core and PostgreSQL.

## Features

✅ **Multi-Tenancy Support** - Isolated tenant environments with role-based access control

✅ **Compliance Framework Management** - Support for SOC 2, ISO 27001, GDPR, HIPAA, PCI DSS and custom frameworks

✅ **Unified Control Library** - Single control mapped to multiple frameworks, avoiding duplication

✅ **Evidence & Policy Management** - Comprehensive tracking of compliance evidence with audit trails

✅ **Risk Management** - Risk register with scoring, controls linking, and remediation workflows

✅ **Vendor Management** - Vendor assessment questionnaires and risk tracking

✅ **Task Management** - Remediation tasks with priority, status tracking, and notifications

✅ **External Integrations** - Support for AWS, Okta, Jira, and other cloud services

✅ **RESTful API** - Clean, well-documented API with Swagger/OpenAPI

✅ **Comprehensive Logging** - Structured logging with Serilog for debugging and auditing

## Quick Start

### Prerequisites
- .NET 10 SDK
- PostgreSQL 12+
- Docker (optional, for containerized database)

### Installation

1. **Clone the repository**
```bash
git clone https://github.com/yourusername/auditmanagmnet.git
cd auditmanagmnet
```

2. **Run development setup**
```bash
./setup-dev.sh
```

3. **Configure database** (optional with Docker)
```bash
# Using Docker
docker-compose up -d

# Or manually install PostgreSQL and run:
./setup-database.sh
```

4. **Start the application**
```bash
dotnet run --project src/AuditManagement.API
```

5. **Access the API**
- API: https://localhost:5001
- Swagger UI: https://localhost:5001/swagger
- PgAdmin (if using Docker): http://localhost:5050

See [QUICKSTART.md](QUICKSTART.md) for detailed instructions.

## Documentation

- [ARCHITECTURE.md](ARCHITECTURE.md) - System architecture and design patterns
- [QUICKSTART.md](QUICKSTART.md) - Quick start guide and common tasks
- [requarments.md](requarments.md) - Business requirements document

## Project Structure

```
src/
├── AuditManagement.API              # REST API Layer
├── AuditManagement.Application      # Business Logic
├── AuditManagement.Domain           # Domain Entities
├── AuditManagement.Infrastructure   # Cross-Cutting Concerns
└── AuditManagement.Persistence      # Data Access Layer
```

## API Endpoints

### Tenant Management
- `GET /api/tenants` - List all tenants
- `POST /api/tenants` - Create tenant
- `GET /api/tenants/{id}` - Get tenant details
- `PUT /api/tenants/{id}` - Update tenant
- `DELETE /api/tenants/{id}` - Delete tenant

### Framework Management
- `GET /api/frameworks` - List frameworks
- `POST /api/frameworks` - Create framework
- `GET /api/frameworks/{id}` - Get framework details
- `PUT /api/frameworks/{id}` - Update framework
- `DELETE /api/frameworks/{id}` - Delete framework

*(Additional endpoints for Controls, Risks, Vendors, Tasks coming soon)*

## Database Schema

The platform uses PostgreSQL with the following core entities:

- **Tenants** - Multi-tenancy isolation
- **Users & Roles** - User management and RBAC
- **Frameworks & Controls** - Compliance framework definitions
- **Policies & Evidence** - Documentation and compliance proof
- **Risks** - Risk register and assessment
- **Vendors** - Third-party management
- **Tasks** - Remediation and remediation tracking
- **Integrations** - External system connectors

## Development

### Build
```bash
dotnet build
```

### Run Tests
```bash
dotnet test
```

### Database Migrations
```bash
# Create migration
dotnet ef migrations add MigrationName -p src/AuditManagement.Persistence

# Apply migration
dotnet ef database update -p src/AuditManagement.Persistence
```

### Code Organization
- **Clean Architecture**: Layered architecture with clear separation of concerns
- **Repository Pattern**: Abstract data access layer
- **Dependency Injection**: ASP.NET Core built-in DI container
- **Entity Framework Core**: ORM for database access
- **Soft Delete**: Logical deletion with audit trail

## Environment Configuration

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=AuditManagement;Username=postgres;Password=password"
  },
  "JwtSettings": {
    "SecretKey": "your-secret-key-here",
    "Issuer": "AuditManagement",
    "Audience": "AuditManagementUsers",
    "ExpirationMinutes": 60
  }
}
```

## Roadmap

### Phase 1 (Current MVP)
- ✅ Multi-tenant architecture
- ✅ Framework and control management
- ✅ Basic API endpoints
- ✅ Database schema

### Phase 2
- 🚧 AI/LLM integration for questionnaire assistance
- 🚧 Real-time dashboards
- 🚧 Advanced reporting
- 🚧 Vendor assessment workflows

### Phase 3
- 📋 Microservices architecture
- 📋 Event-driven workflows
- 📋 Advanced analytics
- 📋 Multi-region deployment

## Technology Stack

- **Backend**: ASP.NET Core 10 (.NET 10)
- **Database**: PostgreSQL 16
- **ORM**: Entity Framework Core
- **Logging**: Serilog
- **API Documentation**: Swagger/OpenAPI
- **Dependency Injection**: Built-in ASP.NET Core DI
- **Authentication**: JWT (forthcoming)

## Security

- Multi-tenancy isolation at database level
- Soft delete for data retention compliance
- Audit trail for all changes
- Role-based access control (RBAC)
- SSL/TLS for data in transit
- Database-level encryption support

## Performance

- Indexed database queries
- Pagination for large datasets
- Connection pooling with Npgsql
- Soft delete with query filters
- Async/await throughout

## Monitoring & Logging

- Structured logging with Serilog
- Console output in development
- File output with rolling logs
- EF Core query logging
- Error tracking and diagnostics

## Contributing

1. Create feature branch: `git checkout -b feature/feature-name`
2. Commit changes: `git commit -am 'Add feature'`
3. Push to branch: `git push origin feature/feature-name`
4. Submit pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For issues, questions, or suggestions:
1. Check existing [issues](https://github.com/yourusername/auditmanagmnet/issues)
2. Create new issue with detailed description
3. Include error messages and steps to reproduce

## Acknowledgments

- ASP.NET Core team for excellent framework
- Entity Framework team for powerful ORM
- PostgreSQL for reliable database
- Serilog for structured logging

## Additional Resources

- [.NET Documentation](https://docs.microsoft.com/dotnet)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [PostgreSQL Documentation](https://www.postgresql.org/docs)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

---

**Current Version**: 1.0.0 (MVP)  
**Last Updated**: February 2026  
**Status**: 🟢 Active Development