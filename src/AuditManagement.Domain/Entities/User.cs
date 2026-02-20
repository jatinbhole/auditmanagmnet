namespace AuditManagement.Domain.Entities;

/// <summary>
/// Represents a user in the system
/// </summary>
public class User : AuditEntity
{
    public Guid TenantId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public Tenant? Tenant { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = [];
}

/// <summary>
/// Represents roles in the system
/// </summary>
public class Role : AuditEntity
{
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Priority { get; set; }
    
    // Navigation properties
    public Tenant? Tenant { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = [];
}

/// <summary>
/// Junction table for User-Role mapping
/// </summary>
public class UserRole
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    
    // Navigation properties
    public User? User { get; set; }
    public Role? Role { get; set; }
}
