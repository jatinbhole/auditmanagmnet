using Microsoft.AspNetCore.Mvc;

namespace AuditManagement.API.Controllers;

/// <summary>
/// Base controller for all API controllers
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    protected Guid CurrentTenantId { get; private set; }

    public BaseController()
    {
        // TODO: Extract tenant ID from claims after authentication is implemented
        CurrentTenantId = Guid.Empty;
    }
}
