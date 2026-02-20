# 🚀 Getting Started - Audit Management Platform

## Overview

This guide walks you through setting up and running the Audit Management Platform locally within 10 minutes.

## Prerequisites

Before you begin, ensure you have:

- **OS**: Windows, macOS, or Linux
- **.NET SDK**: 10.0 or later ([Download](https://dotnet.microsoft.com/download))
- **PostgreSQL**: 12 or later ([Download](https://www.postgresql.org/download/))
  - *Optional*: Use Docker instead (see below)
- **Git**: Latest version ([Download](https://git-scm.com/))

### Verify Prerequisites

```bash
# Check .NET installation
dotnet --version
# Expected output: 10.0.100 or higher

# Check PostgreSQL installation (if using native)
psql --version
# Expected output: psql (PostgreSQL) 12.0 or higher
```

## Installation Steps

### Step 1: Project Setup (2 minutes)

```bash
# Navigate to project directory
cd auditmanagmnet

# Run automated setup
./setup-dev.sh
```

**What this does:**
- ✅ Verifies .NET installation
- ✅ Restores NuGet packages
- ✅ Builds the entire solution
- ✅ Creates development configuration
- ✅ Checks PostgreSQL availability

### Step 2: Database Setup (3 minutes)

#### Option A: Using Docker (Recommended for first-time users)

```bash
# Start PostgreSQL and PgAdmin containers
docker-compose up -d

# Wait for containers to be ready (usually 10-15 seconds)
sleep 15

# Run database initialization
./setup-database.sh
```

**Accessing services:**
- PostgreSQL: `localhost:5432`
- PgAdmin UI: `http://localhost:5050`
  - Email: `admin@example.com`
  - Password: `admin`

#### Option B: Using Native PostgreSQL Installation

```bash
# Update appsettings.json with your PostgreSQL credentials
# Edit: src/AuditManagement.API/appsettings.json
# Update ConnectionStrings section if needed

# Run database initialization
./setup-database.sh
```

### Step 3: Start the Application (2 minutes)

```bash
# Run the API server
dotnet run --project src/AuditManagement.API

# Expected output:
# Building...
# info: Microsoft.Hosting.Lifetime[14]
#       Now listening on: https://localhost:5001
#       Now listening on: http://localhost:5000
```

### Step 4: Verify Installation (3 minutes)

#### Option A: Using Swagger UI (Easiest)

1. Open browser: https://localhost:5001/swagger
2. You should see all API endpoints
3. Expand "Tenants" section
4. Click "Try it out" on GET /api/tenants
5. Click "Execute"
6. You should see `"totalCount": 0` in response

#### Option B: Using curl

```bash
# List all tenants (should return empty list)
curl https://localhost:5001/api/tenants

# Expected response:
# {"items":[],"totalCount":0,"pageNumber":1,"pageSize":10,"totalPages":0}
```

#### Option C: Using REST Client (VS Code)

Install "REST Client" extension, then create `test.http`:

```http
### List Tenants
GET https://localhost:5001/api/tenants
```

Click "Send Request" above the line.

## Creating Your First Tenant

### Using Swagger UI

1. Open https://localhost:5001/swagger
2. Click on **POST /api/tenants**
3. Click "Try it out"
4. In the request body, enter:

```json
{
  "name": "My First Organization",
  "tenantCode": "MFO",
  "description": "Test organization",
  "isActive": true
}
```

5. Click "Execute"
6. You should get a 201 response with the created tenant

### Using curl

```bash
curl -X POST https://localhost:5001/api/tenants \
  -H "Content-Type: application/json" \
  -d '{
    "name": "My First Organization",
    "tenantCode": "MFO",
    "description": "Test organization"
  }'
```

### Using REST Client (VS Code)

Add to `test.http`:

```http
### Create Tenant
POST https://localhost:5001/api/tenants
Content-Type: application/json

{
  "name": "My First Organization",
  "tenantCode": "MFO",
  "description": "Test organization"
}
```

## Project Navigation

### Key Files & Directories

```
auditmanagmnet/
├── src/
│   ├── AuditManagement.API/
│   │   ├── Controllers/          ← Add new API endpoints here
│   │   ├── Program.cs            ← DI configuration
│   │   └── appsettings.json      ← Configuration
│   │
│   ├── AuditManagement.Domain/
│   │   └── Entities/             ← Define business models here
│   │
│   ├── AuditManagement.Application/
│   │   ├── DTOs/                 ← Data Transfer Objects
│   │   └── Repositories/         ← Repository interfaces
│   │
│   ├── AuditManagement.Infrastructure/
│   │   └── Repositories/         ← Repository implementations
│   │
│   └── AuditManagement.Persistence/
│       ├── AuditManagementDbContext.cs  ← EF Core configuration
│       └── Migrations/                   ← Database migrations
│
├── ARCHITECTURE.md               ← Architecture guide
├── QUICKSTART.md                 ← Command reference
├── BUILD_SUMMARY.md              ← Build overview
└── README.md                      ← Project overview
```

### Understanding the API Response Format

All list endpoints return paginated responses:

```json
{
  "items": [
    {
      "id": "550e8400-e29b-41d4-a716-446655440000",
      "name": "My Organization",
      "tenantCode": "MFO",
      "description": "Test",
      "isActive": true
    }
  ],
  "totalCount": 1,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 1
}
```

**Parameters:**
- `pageNumber`: Current page (default: 1)
- `pageSize`: Items per page (default: 10)
- `totalCount`: Total items in database
- `totalPages`: Total pages available

**Example:** Get page 2 with 20 items per page:
```
GET /api/tenants?pageNumber=2&pageSize=20
```

## Developing New Features

### Add a New API Endpoint

#### 1. Create/Modify Entity (if needed)

```csharp
// src/AuditManagement.Domain/Entities/YourEntity.cs
public class YourEntity : AuditEntity
{
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public Tenant? Tenant { get; set; }
}
```

#### 2. Add to DbContext

```csharp
// src/AuditManagement.Persistence/AuditManagementDbContext.cs
public DbSet<YourEntity> YourEntities => Set<YourEntity>();

// In OnModelCreating method:
modelBuilder.Entity<YourEntity>(entity =>
{
    entity.HasKey(e => e.Id);
    entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
});
```

#### 3. Create Migration

```bash
dotnet ef migrations add AddYourEntity \
  -p src/AuditManagement.Persistence \
  -s src/AuditManagement.API
```

#### 4. Apply Migration

```bash
dotnet ef database update \
  -p src/AuditManagement.Persistence \
  -s src/AuditManagement.API
```

#### 5. Create Controller

```csharp
// src/AuditManagement.API/Controllers/YourEntitiesController.cs
[ApiController]
[Route("api/[controller]")]
public class YourEntitiesController : BaseController
{
    private readonly IRepository<YourEntity> _repository;
    private readonly ILogger<YourEntitiesController> _logger;

    public YourEntitiesController(
        IRepository<YourEntity> repository,
        ILogger<YourEntitiesController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid tenantId)
    {
        var items = await _repository.FindAsync(i => i.TenantId == tenantId);
        return Ok(items);
    }
}
```

## Common Commands

### Build & Run

```bash
# Build solution
dotnet build

# Run API
dotnet run --project src/AuditManagement.API

# Run with specific port
dotnet run --project src/AuditManagement.API --urls="https://localhost:5003"

# Clean build
dotnet clean && dotnet build
```

### Database Management

```bash
# List pending migrations
dotnet ef migrations list -p src/AuditManagement.Persistence

# Create migration
dotnet ef migrations add MigrationName \
  -p src/AuditManagement.Persistence \
  -s src/AuditManagement.API

# Apply migrations
dotnet ef database update \
  -p src/AuditManagement.Persistence \
  -s src/AuditManagement.API

# Revert last migration
dotnet ef migrations remove \
  -p src/AuditManagement.Persistence \
  -s src/AuditManagement.API

# View database context info
dotnet ef dbcontext info -p src/AuditManagement.Persistence
```

### Testing API

```bash
# Test with curl
curl https://localhost:5001/api/tenants

# Test with verbose output
curl -v https://localhost:5001/api/tenants

# Post with data
curl -X POST https://localhost:5001/api/tenants \
  -H "Content-Type: application/json" \
  -d '{"name":"Test","tenantCode":"TEST"}'
```

## Stopping the Application

### Stop API Server

```bash
# Press Ctrl+C in the terminal running the API
```

### Stop Docker Containers

```bash
# Stop PostgreSQL and PgAdmin
docker-compose down

# Remove all data (fresh start)
docker-compose down -v
```

## Troubleshooting

### Build Fails

```bash
# Clean and rebuild
dotnet clean
dotnet nuget locals all --clear
dotnet restore
dotnet build
```

### Port Already in Use

```bash
# On Linux/macOS, find process
lsof -i :5001
kill -9 <PID>

# On Windows
netstat -ano | findstr :5001
taskkill /PID <PID> /F

# Run on different port
dotnet run --project src/AuditManagement.API --urls="http://localhost:5002"
```

### Database Connection Error

```bash
# Verify PostgreSQL is running
psql -h localhost -U postgres -c "SELECT version();"

# Check connection string in appsettings.json
# Format: Host=localhost;Port=5432;Database=AuditManagement;Username=postgres;Password=password

# Test connection with Docker
docker exec auditmanagement-postgres psql -U postgres -d AuditManagement -c "SELECT version();"
```

### Certificate Error (HTTPS)

```bash
# Trust .NET development certificate
dotnet dev-certs https --trust

# On macOS with prompt:
# Press Enter to trust the certificate

# Regenerate certificate if needed
dotnet dev-certs https --clean
dotnet dev-certs https
```

### Swagger Shows Different API

```bash
# Clear browser cache
# Open in incognito/private window
# Try different browser
# Restart API server

# Rebuild and restart
dotnet clean
dotnet build
dotnet run --project src/AuditManagement.API
```

## Next Steps

1. **Explore the Code**
   - Read [ARCHITECTURE.md](ARCHITECTURE.md) 
   - Understand the layered architecture
   - Review entity relationships

2. **Learn the Codebase**
   - Examine Controllers in `src/AuditManagement.API/Controllers/`
   - Review Entities in `src/AuditManagement.Domain/Entities/`
   - Understand Repository pattern in `src/AuditManagement.Infrastructure/`

3. **Add Features**
   - Follow the feature addition guide above
   - Create new API endpoints
   - Add database migrations

4. **Testing**
   - Create unit tests
   - Add integration tests
   - Deploy to staging environment

5. **Deployment**
   - Configure production database
   - Set environment variables
   - Deploy to cloud platform

## Documentation

- **[ARCHITECTURE.md](ARCHITECTURE.md)** - Detailed technical architecture
- **[QUICKSTART.md](QUICKSTART.md)** - Command reference and common tasks
- **[BUILD_SUMMARY.md](BUILD_SUMMARY.md)** - Build overview and project statistics
- **[README.md](README.md)** - Project overview and features
- **[CONTRIBUTING.md](CONTRIBUTING.md)** - Contribution guidelines

## Support

### Running into Issues?

1. Check the Troubleshooting section above
2. Review [QUICKSTART.md](QUICKSTART.md) for command reference
3. Check [ARCHITECTURE.md](ARCHITECTURE.md) for design explanations
4. Review source code comments
5. Open an issue on GitHub

### Getting Help

- 📖 **Documentation**: Check markdown files in project root
- 💬 **Code Comments**: Review XML documentation comments
- 🔗 **External Resources**: 
  - [ASP.NET Core Docs](https://docs.microsoft.com/aspnet/core)
  - [Entity Framework Core](https://docs.microsoft.com/ef/core)
  - [PostgreSQL Docs](https://www.postgresql.org/docs)

## Success Checklist

Before moving to development:

- [ ] All prerequisites installed
- [ ] Project builds successfully
- [ ] Database is initialized
- [ ] API starts without errors
- [ ] Swagger UI is accessible
- [ ] Can create a tenant successfully
- [ ] Can query tenants successfully
- [ ] Documentation reviewed

## Commands Quick Reference

```bash
# Setup
./setup-dev.sh                    # Initial development setup
./setup-database.sh               # Database initialization
docker-compose up -d              # Start PostgreSQL (Docker)

# Build & Run
dotnet build                      # Build solution
dotnet run --project src/AuditManagement.API  # Run API

# Database
dotnet ef migrations add Name -p src/AuditManagement.Persistence
dotnet ef database update

# Help
dotnet --help                     # .NET CLI help
```

---

**Ready to get started? Run `./setup-dev.sh` now! 🚀**

For detailed information, see:
- Quick reference: [QUICKSTART.md](QUICKSTART.md)
- Architecture: [ARCHITECTURE.md](ARCHITECTURE.md)
- Project overview: [README.md](README.md)
