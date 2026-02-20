namespace AuditManagement.Domain.Entities;

/// <summary>
/// Represents a compliance framework (SOC 2, ISO 27001, GDPR, etc.)
/// </summary>
public class Framework : AuditEntity
{
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Version { get; set; } = "1.0";
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public Tenant? Tenant { get; set; }
    public ICollection<FrameworkControl> FrameworkControls { get; set; } = [];
}

/// <summary>
/// Represents a control in the unified control library
/// </summary>
public class Control : AuditEntity
{
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Owner { get; set; } = string.Empty;
    public ControlStatus Status { get; set; } = ControlStatus.NotStarted;
    public int CompliancePercentage { get; set; } = 0;
    
    // Navigation properties
    public Tenant? Tenant { get; set; }
    public ICollection<FrameworkControl> FrameworkControls { get; set; } = [];
    public ICollection<Evidence> Evidence { get; set; } = [];
    public ICollection<RiskControl> RiskControls { get; set; } = [];
}

/// <summary>
/// Junction table mapping controls to frameworks
/// </summary>
public class FrameworkControl
{
    public Guid FrameworkId { get; set; }
    public Guid ControlId { get; set; }
    public string Requirement { get; set; } = string.Empty;
    public int Sequence { get; set; }
    
    // Navigation properties
    public Framework? Framework { get; set; }
    public Control? Control { get; set; }
}

/// <summary>
/// Control status enumeration
/// </summary>
public enum ControlStatus
{
    NotStarted = 0,
    InProgress = 1,
    Completed = 2,
    Failed = 3,
    Archived = 4
}
