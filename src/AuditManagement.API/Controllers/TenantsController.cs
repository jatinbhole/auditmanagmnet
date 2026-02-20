using Microsoft.AspNetCore.Mvc;
using AuditManagement.Application.DTOs;
using AuditManagement.Application.Repositories;
using AuditManagement.Domain.Entities;

namespace AuditManagement.API.Controllers;

/// <summary>
/// API controller for tenant management
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TenantsController : BaseController
{
    private readonly IRepository<Tenant> _tenantRepository;
    private readonly ILogger<TenantsController> _logger;

    public TenantsController(IRepository<Tenant> tenantRepository, ILogger<TenantsController> logger)
    {
        _tenantRepository = tenantRepository;
        _logger = logger;
    }

    /// <summary>
    /// Get all tenants
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResult<TenantDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTenants([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var tenants = await _tenantRepository.GetAllAsync();
            var totalCount = tenants.Count();
            
            var paginatedTenants = tenants
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TenantDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    TenantCode = t.TenantCode,
                    IsActive = t.IsActive
                })
                .ToList();

            var result = new PaginatedResult<TenantDto>
            {
                Items = paginatedTenants,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tenants");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ErrorResponse { Message = "Error retrieving tenants", StatusCode = 500 });
        }
    }

    /// <summary>
    /// Get tenant by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TenantDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTenantById(Guid id)
    {
        try
        {
            var tenant = await _tenantRepository.GetByIdAsync(id);
            if (tenant == null)
            {
                return NotFound(new ErrorResponse { Message = "Tenant not found", StatusCode = 404 });
            }

            var dto = new TenantDto
            {
                Id = tenant.Id,
                Name = tenant.Name,
                Description = tenant.Description,
                TenantCode = tenant.TenantCode,
                IsActive = tenant.IsActive
            };

            return Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tenant {TenantId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ErrorResponse { Message = "Error retrieving tenant", StatusCode = 500 });
        }
    }

    /// <summary>
    /// Create a new tenant
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(TenantDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTenant([FromBody] TenantDto tenantDto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(tenantDto.Name) || string.IsNullOrWhiteSpace(tenantDto.TenantCode))
            {
                return BadRequest(new ErrorResponse { Message = "Name and TenantCode are required", StatusCode = 400 });
            }

            var newTenant = new Tenant
            {
                Id = Guid.NewGuid(),
                Name = tenantDto.Name,
                Description = tenantDto.Description,
                TenantCode = tenantDto.TenantCode,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _tenantRepository.AddAsync(newTenant);
            await _tenantRepository.SaveChangesAsync();

            var returnDto = new TenantDto
            {
                Id = newTenant.Id,
                Name = newTenant.Name,
                Description = newTenant.Description,
                TenantCode = newTenant.TenantCode,
                IsActive = newTenant.IsActive
            };

            return CreatedAtAction(nameof(GetTenantById), new { id = newTenant.Id }, returnDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tenant");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ErrorResponse { Message = "Error creating tenant", StatusCode = 500 });
        }
    }

    /// <summary>
    /// Update a tenant
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TenantDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTenant(Guid id, [FromBody] TenantDto tenantDto)
    {
        try
        {
            var tenant = await _tenantRepository.GetByIdAsync(id);
            if (tenant == null)
            {
                return NotFound(new ErrorResponse { Message = "Tenant not found", StatusCode = 404 });
            }

            tenant.Name = tenantDto.Name;
            tenant.Description = tenantDto.Description;
            tenant.IsActive = tenantDto.IsActive;

            _tenantRepository.Update(tenant);
            await _tenantRepository.SaveChangesAsync();

            var returnDto = new TenantDto
            {
                Id = tenant.Id,
                Name = tenant.Name,
                Description = tenant.Description,
                TenantCode = tenant.TenantCode,
                IsActive = tenant.IsActive
            };

            return Ok(returnDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tenant {TenantId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ErrorResponse { Message = "Error updating tenant", StatusCode = 500 });
        }
    }

    /// <summary>
    /// Delete a tenant
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTenant(Guid id)
    {
        try
        {
            var tenant = await _tenantRepository.GetByIdAsync(id);
            if (tenant == null)
            {
                return NotFound(new ErrorResponse { Message = "Tenant not found", StatusCode = 404 });
            }

            _tenantRepository.Delete(tenant);
            await _tenantRepository.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tenant {TenantId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ErrorResponse { Message = "Error deleting tenant", StatusCode = 500 });
        }
    }
}
