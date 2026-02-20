# Audit Management Platform - Project Setup & Architecture

## Overview
This is a comprehensive AI-Agentic GRC (Governance, Risk, and Compliance) Platform built with ASP.NET Core and PostgreSQL, designed to streamline compliance management across multiple frameworks.

## Project Structure

```
AuditManagement/
├── src/
│   ├── AuditManagement.API/              # REST API Layer
│   │   ├── Controllers/                  # API endpoints
│   │   ├── Program.cs                    # Startup configuration
│   │   ├── appsettings.json              # Configuration
│   │   └── AuditManagement.API.csproj
│   │
│   ├── AuditManagement.Application/      # Business Logic & Use Cases
│   │   ├── DTOs/                         # Data Transfer Objects
│   │   ├── Repositories/                 # Repository interfaces
│   │   ├── Services/                     # Application services
│   │   └── AuditManagement.Application.csproj
│   │
│   ├── AuditManagement.Domain/           # Domain Entities & Models
│   │   ├── Entities/                     # Core business entities
│   │   │   ├── AuditEntity.cs            # Base entity with audit tracking
│   │   │   ├── Tenant.cs                 # Multi-tenancy support
│   │   │   ├── User.cs                   # User & Role management
│   │   │   ├── Control.cs                # Control & Framework mapping
│   │   │   ├── Evidence.cs               # Evidence & Policy tracking
│   │   │   ├── Risk.cs                   # Risk management
│   │   │   ├── Vendor.cs                 # Vendor assessment
│   │   │   ├── RemediationTask.cs        # Task management
│   │   │   └── Integration.cs            # External integrations
│   │   └── AuditManagement.Domain.csproj
│   │
│   ├── AuditManagement.Infrastructure/   # Cross-cutting Concerns
│   │   ├── Repositories/                 # Generic repository implementation
│   │   ├── Services/                     # Infrastructure services
│   │   └── AuditManagement.Infrastructure.csproj
│   │
│   └── AuditManagement.Persistence/      # Data Access Layer
│       ├── AuditManagementDbContext.cs   # EF Core DbContext
│       ├── Migrations/                   # Database migrations
│       └── AuditManagement.Persistence.csproj
│
├── AuditManagement.slnx                  # Solution file
└── global.json                           # .NET version specification
```

## Architecture Principles

### Clean Architecture
The project follows clean architecture principles with clear separation of concerns:

1. **Domain Layer** (AuditManagement.Domain)
   - Contains all business logic and domain entities
   - No external dependencies
   - Framework-agnostic

2. **Application Layer** (AuditManagement.Application)
   - Implements use cases and business rules
   - Contains DTOs and service interfaces
   - Depends only on Domain layer

3. **Infrastructure Layer** (AuditManagement.Infrastructure)
   - Implements repository pattern
   - Cross-cutting concerns (logging, caching)
   - Depends on Application and Domain

4. **Persistence Layer** (AuditManagement.Persistence)
   - Entity Framework Core configuration
   - Database migrations and DbContext
   - Depends on Domain layer

5. **API Layer** (AuditManagement.API)
   - REST API endpoints
   - Request/Response handling
   - Dependency injection setup
   - Depends on all layers

### Key Design Patterns

#### Repository Pattern
```csharp
// Generic repository interface in Application layer
public interface IRepository<T> where T : AuditEntity
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<T> AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<int> SaveChangesAsync();
}

// Concrete implementation in Infrastructure layer
public class Repository<T> : IRepository<T> where T : AuditEntity
{
    protected readonly AuditManagementDbContext Context;
    protected readonly DbSet<T> DbSet;
    // ... implementation details
}
```

#### Dependency Injection
All dependencies are registered in Program.cs following ASP.NET Core conventions:
```csharp
builder.Services.AddDbContext<AuditManagementDbContext>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
```

#### Soft Delete Pattern
All entities implement soft delete through the `AuditEntity` base class:
```csharp
public abstract class AuditEntity
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    // Also tracks CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
}
```

## Core Entities

### Tenant & Multi-Tenancy
- **Tenant**: Represents organization/company
- **User**: User account per tenant
- **Role**: Tenant-specific roles with UserRole mapping

### Compliance Framework
- **Framework**: Compliance framework (SOC 2, ISO 27001, GDPR, etc.)
- **Control**: Unified control library mapped to multiple frameworks
- **FrameworkControl**: Junction table for many-to-many relationship

### Evidence & Policies
- **Policy**: Policies, procedures, and documentation
- **Evidence**: Evidence supporting control compliance
- **EvidenceAuditLog**: Audit trail for evidence changes

### Risk Management
- **Risk**: Risk register with likelihood, impact, and score
- **RiskControl**: Link risks to controls
- **RemediationTask**: Tasks for risk remediation

### Vendor Management
- **Vendor**: Third-party/vendor information
- **VendorQuestionnaire**: Questionnaire templates for vendor assessment
- **VendorQuestion**: Individual questions in questionnaire
- **VendorRisk**: Risks associated with vendors

### External Integrations
- **Integration**: Configuration for external systems (AWS, Okta, Jira)
- **IntegrationEvent**: Events triggered by integrations

## Database Configuration

### PostgreSQL Setup
The application uses PostgreSQL with Entity Framework Core:

```csharp
// Connection string in appsettings.json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=AuditManagement;Username=postgres;Password=password"
}

// DbContext configuration
builder.Services.AddDbContext<AuditManagementDbContext>(options =>
    options.UseNpgsql(connection)
);
```

### Migrations
Entity Framework migrations are run automatically on application startup:

```csharp
// In Program.cs
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AuditManagementDbContext>();
    dbContext.Database.Migrate();
}
```

## API Endpoints (MVP)

### Tenant Management
- `GET /api/tenants` - List all tenants
- `GET /api/tenants/{id}` - Get tenant by ID
- `POST /api/tenants` - Create new tenant
- `PUT /api/tenants/{id}` - Update tenant
- `DELETE /api/tenants/{id}` - Delete tenant

### Framework Management
- `GET /api/frameworks?tenantId={id}` - List frameworks
- `GET /api/frameworks/{id}` - Get framework by ID
- `POST /api/frameworks` - Create framework
- `PUT /api/frameworks/{id}` - Update framework
- `DELETE /api/frameworks/{id}` - Delete framework

## Getting Started

### Prerequisites
- .NET 10 SDK
- PostgreSQL 12+
- Visual Studio Code or Visual Studio

### Installation
1. Clone the repository
2. Restore packages: `dotnet restore`
3. Build solution: `dotnet build`
4. Configure PostgreSQL connection in `appsettings.json`
5. Create database: `dotnet ef database update`
6. Run API: `dotnet run --project src/AuditManagement.API`

### Configuration Files
- **appsettings.json**: Database connection, logging, JWT settings
- **global.json**: .NET SDK version (10.0.0)
- **.env**: (Not tracked) Store sensitive data like DB passwords

## Logging & Monitoring

### Serilog Integration
Structured logging with Serilog:
- Console output for development
- File output with daily rolling logs
- Log level configuration per namespace

```csharp
builder.Host.UseSerilog();
```

## Security Considerations

### Authentication & Authorization
- ASP.NET Core Identity for user management
- JWT tokens for API authentication (to be implemented)
- Role-based access control (RBAC)

### Data Protection
- Soft delete for data retention
- Encryption for sensitive fields
- Audit trail for compliance tracking

## Future Enhancements

### Phase 2
- AI/LLM integration for questionnaire assistance
- Real-time dashboards with SignalR
- Advanced reporting and analytics
- Custom framework creation
- Control testing framework

### Phase 3
- Microservices architecture
- Event-driven workflows
- Advanced vendor management
- Multi-region deployment
- Advanced caching strategies

## Development Workflow

### Adding a New Entity
1. Create entity class in `Domain/Entities`
2. Add DbSet to `AuditManagementDbContext`
3. Configure in `OnModelCreating`
4. Create migration: `dotnet ef migrations add <MigrationName> -p src/AuditManagement.Persistence`
5. Create repository interface in `Application/Repositories`
6. Register in dependency injection
7. Create API controller

### Database Migration
```bash
# Add migration
dotnet ef migrations add MigrationName -p src/AuditManagement.Persistence

# Apply migration
dotnet ef database update -p src/AuditManagement.Persistence

# Remove last migration
dotnet ef migrations remove -p src/AuditManagement.Persistence
```

## Performance Optimization

### Strategies Implemented
- Soft delete with query filters for efficient lookups
- Indexed tenant ID and unique codes
- Foreign key relationships for referential integrity
- Connection pooling with Npgsql

### Future Optimizations
- Redis caching layer
- Database query optimization
- Pagination for large datasets
- Async/await throughout
- Background job processing

## Testing Strategy

### Planned Test Coverage
- Unit tests for domain entities (xUnit)
- Integration tests for repositories (TestContainers)
- API endpoint tests (xUnit + Moq)
- Database migration tests

## Deployment

### Production Considerations
- Environment-specific configurations
- Database backup strategy
- SSL/TLS encryption
- CORS configuration
- Rate limiting
- API versioning

## Troubleshooting

### Database Connection Issues
```bash
# Check PostgreSQL is running
sudo systemctl status postgresql

# Test connection
psql -h localhost -U postgres -d AuditManagement
```

### Build Issues
```bash
# Clear cache
dotnet clean
dotnet nuget locals all --clear

# Restore packages
dotnet restore
```

## Resources
- [ASP.NET Core Documentation](https://docs.microsoft.com/dotnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [PostgreSQL Documentation](https://www.postgresql.org/docs)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

