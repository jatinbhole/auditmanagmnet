namespace AuditManagement.Domain.Entities;

/// <summary>
/// Represents an external integration (AWS, Okta, Jira, etc.)
/// </summary>
public class Integration : AuditEntity
{
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string IntegrationType { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public string? ApiKey { get; set; }
    public string? SecretKey { get; set; }
    public string? Configuration { get; set; } // JSON configuration
    public DateTime? LastSyncAt { get; set; }
    
    // Navigation properties
    public Tenant? Tenant { get; set; }
    public ICollection<IntegrationEvent> Events { get; set; } = [];
}

/// <summary>
/// Represents events triggered by integrations
/// </summary>
public class IntegrationEvent : AuditEntity
{
    public Guid IntegrationId { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string EventData { get; set; } = string.Empty;
    public bool Processed { get; set; }
    public DateTime? ProcessedAt { get; set; }
    
    // Navigation properties
    public Integration? Integration { get; set; }
}
