namespace AuditManagement.Domain.Entities;

/// <summary>
/// Represents a vendor/third-party
/// </summary>
public class Vendor : AuditEntity
{
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Services { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public RiskTier RiskTier { get; set; } = RiskTier.Medium;
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public Tenant? Tenant { get; set; }
    public ICollection<VendorQuestionnaire> Questionnaires { get; set; } = [];
    public ICollection<VendorRisk> VendorRisks { get; set; } = [];
}

/// <summary>
/// Represents a questionnaire for vendor assessment
/// </summary>
public class VendorQuestionnaire : AuditEntity
{
    public Guid VendorId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime IssuedAt { get; set; }
    public DateTime DueAt { get; set; }
    public QuestionnaireStatus Status { get; set; } = QuestionnaireStatus.Pending;
    
    // Navigation properties
    public Vendor? Vendor { get; set; }
    public ICollection<VendorQuestion> Questions { get; set; } = [];
}

/// <summary>
/// Represents a question in a vendor questionnaire
/// </summary>
public class VendorQuestion : AuditEntity
{
    public Guid QuestionnaireId { get; set; }
    public string Question { get; set; } = string.Empty;
    public string? Answer { get; set; }
    public QuestionType Type { get; set; }
    public int Sequence { get; set; }
    public bool IsRequired { get; set; }
    
    // Navigation properties
    public VendorQuestionnaire? Questionnaire { get; set; }
}

/// <summary>
/// Represents a risk associated with a vendor
/// </summary>
public class VendorRisk : AuditEntity
{
    public Guid VendorId { get; set; }
    public string RiskDescription { get; set; } = string.Empty;
    public int Likelihood { get; set; }
    public int Impact { get; set; }
    public int RiskScore { get; set; }
    
    // Navigation properties
    public Vendor? Vendor { get; set; }
}

/// <summary>
/// Risk tier enumeration
/// </summary>
public enum RiskTier
{
    Low = 0,
    Medium = 1,
    High = 2,
    Critical = 3
}

/// <summary>
/// Questionnaire status enumeration
/// </summary>
public enum QuestionnaireStatus
{
    Draft = 0,
    Pending = 1,
    InProgress = 2,
    Completed = 3,
    Approved = 4
}

/// <summary>
/// Question type enumeration
/// </summary>
public enum QuestionType
{
    Text = 0,
    MultipleChoice = 1,
    YesNo = 2,
    Document = 3
}
