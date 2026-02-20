namespace AuditManagement.Application.DTOs;

/// <summary>
/// DTO for Tenant creation and updates
/// </summary>
public class TenantDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string TenantCode { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

/// <summary>
/// DTO for User operations
/// </summary>
public class UserDto
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

/// <summary>
/// DTO for Framework operations
/// </summary>
public class FrameworkDto
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Version { get; set; } = "1.0";
    public bool IsActive { get; set; }
}

/// <summary>
/// DTO for Control operations
/// </summary>
public class ControlDto
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Owner { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int CompliancePercentage { get; set; }
}

/// <summary>
/// DTO for Risk operations
/// </summary>
public class RiskDto
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Owner { get; set; } = string.Empty;
    public int Likelihood { get; set; }
    public int Impact { get; set; }
    public int RiskScore { get; set; }
    public string Status { get; set; } = string.Empty;
}

/// <summary>
/// DTO for Vendor operations
/// </summary>
public class VendorDto
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Services { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string RiskTier { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

/// <summary>
/// DTO for Remediation Task operations
/// </summary>
public class RemediationTaskDto
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid? ControlId { get; set; }
    public Guid? RiskId { get; set; }
    public string AssignedTo { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
}

/// <summary>
/// DTO for Error responses
/// </summary>
public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
    public string? DetailedMessage { get; set; }
    public int StatusCode { get; set; }
}

/// <summary>
/// DTO for paginated responses
/// </summary>
public class PaginatedResult<T>
{
    public List<T> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
}
