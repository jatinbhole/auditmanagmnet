# Audit Management Platform - Quick Start Guide

## 5-Minute Quick Start

### Step 1: Environment Check
```bash
# Verify .NET is installed
dotnet --version              # Should be 10.0+

# Verify PostgreSQL is available
psql --version               # Should be 12+
```

### Step 2: Project Setup
```bash
# Clone and navigate
cd auditmanagmnet

# Run development setup
./setup-dev.sh

# Or manually:
dotnet restore
dotnet build
```

### Step 3: Database Setup
```bash
# Configure connection in appsettings.json if needed
# Then run:
./setup-database.sh

# Or manually:
dotnet ef database update -p src/AuditManagement.Persistence
```

### Step 4: Run Application
```bash
dotnet run --project src/AuditManagement.API

# API will start at:
# - https://localhost:5001
# - http://localhost:5000
# - Swagger UI: https://localhost:5001/swagger
```

## API Testing

### Using curl
```bash
# List tenants
curl https://localhost:5001/api/tenants

# Create tenant
curl -X POST https://localhost:5001/api/tenants \
  -H "Content-Type: application/json" \
  -d '{"name":"Test Corp","tenantCode":"TEST","description":"Test tenant"}'

# Get specific tenant
curl https://localhost:5001/api/tenants/{id}
```

### Using Swagger UI
1. Open https://localhost:5001/swagger
2. Expand endpoints
3. Click "Try it out"
4. Fill in parameters
5. Click "Execute"

### Using VS Code REST Client
Create `test.http`:
```http
### Get all tenants
GET https://localhost:5001/api/tenants

### Create tenant
POST https://localhost:5001/api/tenants
Content-Type: application/json

{
  "name": "Acme Corp",
  "tenantCode": "ACME",
  "description": "Test organization"
}

### Get frameworks
GET https://localhost:5001/api/frameworks?tenantId={tenantId}
```

## Common Development Tasks

### Database Migrations

#### Create a Migration
```bash
# Add migration for model changes
dotnet ef migrations add AddNewTable \
  -p src/AuditManagement.Persistence \
  -s src/AuditManagement.API

# Migration file will be created in:
# src/AuditManagement.Persistence/Migrations/
```

#### Apply Migrations
```bash
# Apply to development database
dotnet ef database update \
  -p src/AuditManagement.Persistence \
  -s src/AuditManagement.API
```

#### Revert Last Migration
```bash
dotnet ef migrations remove \
  -p src/AuditManagement.Persistence \
  -s src/AuditManagement.API
```

### Building & Testing

```bash
# Clean build
dotnet clean && dotnet build

# Build specific project
dotnet build src/AuditManagement.API

# Run with verbose output
dotnet run --project src/AuditManagement.API --verbosity diagnostic
```

## Project Structure Overview

```
src/
├── AuditManagement.API           # REST API Layer
│   ├── Controllers/              # API controllers
│   ├── appsettings.json          # Configuration
│   └── Program.cs                # Startup
│
├── AuditManagement.Domain        # Domain Entities
│   └── Entities/                 # Business entities
│
├── AuditManagement.Application   # Business Logic
│   ├── DTOs/                     # Data Transfer Objects
│   └── Repositories/             # Repository interfaces
│
├── AuditManagement.Infrastructure # Cross-Cutting Concerns
│   ├── Repositories/             # Repository implementations
│   └── Services/                 # Infrastructure services
│
└── AuditManagement.Persistence   # Data Access
    ├── AuditManagementDbContext.cs
    └── Migrations/               # EF Core migrations
```

## Configuration Files

### appsettings.json
Main configuration file with:
- Database connection string
- JWT settings
- Logging configuration
- Application settings

### appsettings.Development.json
Development-specific overrides (gitignored)

### global.json
Specifies .NET SDK version (10.0.0)

## Environment Variables

Set these for production deployments:
```bash
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_HTTPS_PORT=443
ConnectionStrings__DefaultConnection="postgresql://..."
JwtSettings__SecretKey="your-secret-key"
```

## Troubleshooting

### Build Fails
```bash
# Clean and restore
dotnet clean
dotnet nuget locals all --clear
dotnet restore
dotnet build
```

### Database Connection Error
```bash
# Verify PostgreSQL is running
psql -h localhost -U postgres -c "SELECT version();"

# Check connection string in appsettings.json
# Format: Host=localhost;Port=5432;Database=AuditManagement;Username=postgres;Password=password
```

### Port Already in Use
```bash
# Find and kill process on port 5001
lsof -i :5001
kill -9 <PID>

# Or run on different port
dotnet run --project src/AuditManagement.API --urls="http://localhost:5002"
```

### EF Core Errors
```bash
# Update EF Core tools
dotnet tool update dotnet-ef

# Verify DbContext
dotnet ef dbcontext info -p src/AuditManagement.Persistence
```

## Architecture Overview

### Clean Architecture Layers
1. **Domain**: Core business entities and logic
2. **Application**: Use cases and service interfaces  
3. **Infrastructure**: Implementation of cross-cutting concerns
4. **Persistence**: Data access with Entity Framework Core
5. **API**: REST endpoints and dependency injection

### Database Design
- **Multi-tenancy**: Each tenant is isolated at data level
- **Soft Delete**: Logical deletion with IsDeleted flag
- **Audit Trail**: CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
- **Foreign Keys**: Referential integrity with cascading deletes

### API Response Format
```json
{
  "items": [],
  "totalCount": 0,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 0
}
```

## Next Steps

1. **Add Authentication**
   - Implement JWT token generation
   - Add authentication middleware
   - Configure authorization policies

2. **Create Service Layer**
   - Implement application services
   - Add business logic abstractions
   - Create MediatR command handlers

3. **Add Logging & Monitoring**
   - Configure Serilog sinks
   - Add performance monitoring
   - Set up error tracking

4. **Implement Tests**
   - Unit tests for domain entities
   - Integration tests for repositories
   - API endpoint tests

5. **Add Documentation**
   - OpenAPI/Swagger schemas
   - API documentation
   - Architecture decision records

## Getting Help

- Check [ARCHITECTURE.md](ARCHITECTURE.md) for detailed architecture
- Review [requarments.md](requarments.md) for business requirements
- See ASP.NET Core [documentation](https://docs.microsoft.com/en-us/dotnet/core)
- Entity Framework Core [guide](https://docs.microsoft.com/en-us/ef/core)

## Additional Resources

- **REST Client for VS Code**: REST Client extension
- **Database Tools**: pgAdmin or DBeaver for PostgreSQL
- **Postman**: API testing collection support
- **Git Hooks**: Set up pre-commit hooks for code quality

