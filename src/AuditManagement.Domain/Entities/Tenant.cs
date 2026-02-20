namespace AuditManagement.Domain.Entities;

/// <summary>
/// Represents a tenant (company/organization)
/// </summary>
public class Tenant : AuditEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string TenantCode { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public ICollection<User> Users { get; set; } = [];
    public ICollection<Framework> Frameworks { get; set; } = [];
    public ICollection<Control> Controls { get; set; } = [];
    public ICollection<Policy> Policies { get; set; } = [];
    public ICollection<Risk> Risks { get; set; } = [];
    public ICollection<Vendor> Vendors { get; set; } = [];
    public ICollection<RemediationTask> Tasks { get; set; } = [];
}
