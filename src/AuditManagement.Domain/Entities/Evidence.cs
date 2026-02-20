namespace AuditManagement.Domain.Entities;

/// <summary>
/// Represents a policy or procedure document
/// </summary>
public class Policy : AuditEntity
{
    public Guid TenantId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Version { get; set; } = "1.0";
    public Guid? ControlId { get; set; }
    public DateTime LastReviewedAt { get; set; }
    public string Owner { get; set; } = string.Empty;
    public bool IsApproved { get; set; }
    
    // Navigation properties
    public Tenant? Tenant { get; set; }
    public Control? Control { get; set; }
    public ICollection<Evidence> Evidence { get; set; } = [];
}

/// <summary>
/// Represents evidence supporting control compliance
/// </summary>
public class Evidence : AuditEntity
{
    public Guid TenantId { get; set; }
    public Guid ControlId { get; set; }
    public Guid? PolicyId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public int FileSizeBytes { get; set; }
    public DateTime EvidenceDate { get; set; }
    public EvidenceStatus Status { get; set; } = EvidenceStatus.Pending;
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    
    // Navigation properties
    public Tenant? Tenant { get; set; }
    public Control? Control { get; set; }
    public Policy? Policy { get; set; }
    public ICollection<EvidenceAuditLog> AuditLogs { get; set; } = [];
}

/// <summary>
/// Audit trail for evidence changes
/// </summary>
public class EvidenceAuditLog : AuditEntity
{
    public Guid EvidenceId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public string ChangedBy { get; set; } = string.Empty;
    
    // Navigation properties
    public Evidence? Evidence { get; set; }
}

/// <summary>
/// Evidence status enumeration
/// </summary>
public enum EvidenceStatus
{
    Pending = 0,
    Approved = 1,
    Rejected = 2,
    UnderReview = 3
}
