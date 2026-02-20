namespace AuditManagement.Domain.Entities;

/// <summary>
/// Represents a risk entry in the risk register
/// </summary>
public class Risk : AuditEntity
{
    public Guid TenantId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Owner { get; set; } = string.Empty;
    public int Likelihood { get; set; } // 1-5
    public int Impact { get; set; } // 1-5
    public int RiskScore { get; set; }
    public RiskStatus Status { get; set; } = RiskStatus.Open;
    public string MitigationPlan { get; set; } = string.Empty;
    
    // Navigation properties
    public Tenant? Tenant { get; set; }
    public ICollection<RiskControl> RiskControls { get; set; } = [];
    public ICollection<RemediationTask> RemediationTasks { get; set; } = [];
}

/// <summary>
/// Junction table linking risks to controls
/// </summary>
public class RiskControl
{
    public Guid RiskId { get; set; }
    public Guid ControlId { get; set; }
    
    // Navigation properties
    public Risk? Risk { get; set; }
    public Control? Control { get; set; }
}

/// <summary>
/// Risk status enumeration
/// </summary>
public enum RiskStatus
{
    Open = 0,
    InProgress = 1,
    Mitigated = 2,
    Closed = 3
}
