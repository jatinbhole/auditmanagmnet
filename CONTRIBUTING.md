# Contributing to Audit Management Platform

## Getting Started

1. Fork the repository
2. Clone your fork: `git clone https://github.com/yourusername/auditmanagmnet.git`
3. Create feature branch: `git checkout -b feature/your-feature`
4. Make changes and commit: `git commit -am 'Add feature'`
5. Push to branch: `git push origin feature/your-feature`
6. Create Pull Request

## Code Style

### C# Guidelines
- Follow Microsoft C# naming conventions
- Use meaningful variable names
- Add XML documentation comments for public APIs
- Use async/await for I/O operations
- Keep methods focused and single-responsibility

### Format
```csharp
// Use camelCase for private fields and local variables
private string _connectionString;
private readonly ILogger<MyClass> _logger;

// Use PascalCase for properties and public members
public Guid Id { get; set; }
public string Name { get; set; }

// Add documentation comments
/// <summary>
/// Gets or sets the tenant name.
/// </summary>
public string Name { get; set; }

// Use meaningful names
var frameworks = await _frameworkRepository.FindAsync(f => f.TenantId == tenantId);
```

## Commit Messages

- Use clear, descriptive messages
- Start with verb: Add, Fix, Update, Remove, Refactor
- Reference issue numbers when applicable

Examples:
```
Add tenant management endpoints
Fix database migration error
Update documentation for API endpoints
Remove deprecated method
```

## Pull Request Process

1. Update [CHANGELOG.md](CHANGELOG.md) with your changes
2. Update documentation if adding/modifying features
3. Ensure code builds: `dotnet build`
4. Run tests: `dotnet test` (when available)
5. Request review from maintainers

## Development Workflow

### Feature Development
```bash
# Create feature branch
git checkout -b feature/add-control-tests

# Make changes
# ... code ...

# Build and verify
dotnet build
dotnet test

# Commit with clear message
git commit -m "Add unit tests for Control entity"

# Push to your fork
git push origin feature/add-control-tests

# Create pull request on GitHub
```

### Bug Fixes
```bash
# Create fix branch
git checkout -b fix/connection-string-issue

# Make fix
# ... code ...

# Test the fix
dotnet build

# Commit
git commit -m "Fix PostgreSQL connection string parsing"

# Push and create PR
```

## Adding New Features

### 1. Create Domain Entity
```csharp
// src/AuditManagement.Domain/Entities/YourEntity.cs
public class YourEntity : AuditEntity
{
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public Tenant? Tenant { get; set; }
}
```

### 2. Update DbContext
```csharp
// src/AuditManagement.Persistence/AuditManagementDbContext.cs
public DbSet<YourEntity> YourEntities => Set<YourEntity>();

// Configure in OnModelCreating
modelBuilder.Entity<YourEntity>(entity =>
{
    entity.HasKey(e => e.Id);
    entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
});
```

### 3. Create Migration
```bash
dotnet ef migrations add AddYourEntity \
  -p src/AuditManagement.Persistence \
  -s src/AuditManagement.API
```

### 4. Create API Endpoint
```csharp
// src/AuditManagement.API/Controllers/YourEntitiesController.cs
[ApiController]
[Route("api/[controller]")]
public class YourEntitiesController : BaseController
{
    private readonly IRepository<YourEntity> _repository;
    
    public YourEntitiesController(IRepository<YourEntity> repository)
    {
        _repository = repository;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid tenantId)
    {
        var items = await _repository.FindAsync(i => i.TenantId == tenantId);
        return Ok(items);
    }
}
```

## Testing Guidelines

### Unit Tests
- Test individual methods in isolation
- Use Arrange-Act-Assert pattern
- Mock external dependencies

### Integration Tests
- Test database interactions
- Use TestContainers for PostgreSQL
- Clean up test data after each test

### API Tests
- Test endpoint behavior
- Verify response codes and structure
- Test error scenarios

## Documentation

### Add XML Comments
```csharp
/// <summary>
/// Creates a new framework in the system.
/// </summary>
/// <param name="framework">The framework to create</param>
/// <returns>The created framework with ID</returns>
/// <exception cref="ArgumentNullException">Thrown when framework is null</exception>
public async Task<Framework> CreateFrameworkAsync(Framework framework)
{
    // implementation
}
```

### Update Architecture Documentation
When making architectural changes, update [ARCHITECTURE.md](ARCHITECTURE.md)

### Update API Documentation
Document new endpoints with examples in comments

## Performance Considerations

- Use async/await for I/O operations
- Add indexes for frequently queries columns
- Implement pagination for large datasets
- Use select projections instead of full entities
- Monitor query performance

## Security Checklist

- [ ] Validate all user inputs
- [ ] Use parameterized queries (EF Core does this)
- [ ] Implement proper authentication/authorization
- [ ] Don't log sensitive information
- [ ] Use HTTPS in production
- [ ] Encrypt sensitive data at rest

## Reporting Issues

When reporting issues, include:
- Clear description of the problem
- Steps to reproduce
- Expected vs actual behavior
- Error messages or logs
- Environment details (OS, .NET version, etc.)

## Questions?

Feel free to:
- Open an issue for questions
- Reach out to maintainers
- Check existing documentation
- Review similar implementations

## License

By contributing, you agree that your contributions will be licensed under the MIT License.

---

Thank you for contributing to Audit Management Platform! 🙌
