namespace AuditManagement.Domain.Entities;

/// <summary>
/// Represents a remediation task
/// </summary>
public class RemediationTask : AuditEntity
{
    public Guid TenantId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid? ControlId { get; set; }
    public Guid? RiskId { get; set; }
    public string AssignedTo { get; set; } = string.Empty;
    public TaskStatus Status { get; set; } = TaskStatus.Open;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public DateTime DueDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? ExternalTaskId { get; set; } // For Jira integration
    
    // Navigation properties
    public Tenant? Tenant { get; set; }
    public Control? Control { get; set; }
    public Risk? Risk { get; set; }
    public ICollection<TaskNotification> Notifications { get; set; } = [];
}

/// <summary>
/// Represents notifications sent for tasks
/// </summary>
public class TaskNotification : AuditEntity
{
    public Guid TaskId { get; set; }
    public string RecipientEmail { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
    
    // Navigation properties
    public RemediationTask? Task { get; set; }
}

/// <summary>
/// Task status enumeration
/// </summary>
public enum TaskStatus
{
    Open = 0,
    InProgress = 1,
    InReview = 2,
    Completed = 3,
    Cancelled = 4
}

/// <summary>
/// Task priority enumeration
/// </summary>
public enum TaskPriority
{
    Low = 0,
    Medium = 1,
    High = 2,
    Critical = 3
}
