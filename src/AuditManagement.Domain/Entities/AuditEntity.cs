namespace AuditManagement.Domain.Entities;

/// <summary>
/// Base entity class with audit tracking
/// </summary>
public abstract class AuditEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}
