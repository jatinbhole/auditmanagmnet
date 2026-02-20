using Microsoft.AspNetCore.Mvc;
using AuditManagement.Application.DTOs;
using AuditManagement.Application.Repositories;
using AuditManagement.Domain.Entities;

namespace AuditManagement.API.Controllers;

/// <summary>
/// API controller for framework management
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class FrameworksController : BaseController
{
    private readonly IRepository<Framework> _frameworkRepository;
    private readonly ILogger<FrameworksController> _logger;

    public FrameworksController(IRepository<Framework> frameworkRepository, ILogger<FrameworksController> logger)
    {
        _frameworkRepository = frameworkRepository;
        _logger = logger;
    }

    /// <summary>
    /// Get all frameworks for a tenant
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResult<FrameworkDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFrameworks([FromQuery] Guid tenantId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            if (tenantId == Guid.Empty)
            {
                return BadRequest(new ErrorResponse { Message = "TenantId is required", StatusCode = 400 });
            }

            var frameworks = await _frameworkRepository.FindAsync(f => f.TenantId == tenantId);
            var totalCount = frameworks.Count();

            var paginatedFrameworks = frameworks
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(f => new FrameworkDto
                {
                    Id = f.Id,
                    TenantId = f.TenantId,
                    Name = f.Name,
                    Code = f.Code,
                    Description = f.Description,
                    Version = f.Version,
                    IsActive = f.IsActive
                })
                .ToList();

            var result = new PaginatedResult<FrameworkDto>
            {
                Items = paginatedFrameworks,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving frameworks for tenant {TenantId}", tenantId);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ErrorResponse { Message = "Error retrieving frameworks", StatusCode = 500 });
        }
    }

    /// <summary>
    /// Get framework by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(FrameworkDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFrameworkById(Guid id)
    {
        try
        {
            var framework = await _frameworkRepository.GetByIdAsync(id);
            if (framework == null)
            {
                return NotFound(new ErrorResponse { Message = "Framework not found", StatusCode = 404 });
            }

            var dto = new FrameworkDto
            {
                Id = framework.Id,
                TenantId = framework.TenantId,
                Name = framework.Name,
                Code = framework.Code,
                Description = framework.Description,
                Version = framework.Version,
                IsActive = framework.IsActive
            };

            return Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving framework {FrameworkId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ErrorResponse { Message = "Error retrieving framework", StatusCode = 500 });
        }
    }

    /// <summary>
    /// Create a new framework
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(FrameworkDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateFramework([FromBody] FrameworkDto frameworkDto)
    {
        try
        {
            if (frameworkDto.TenantId == Guid.Empty || string.IsNullOrWhiteSpace(frameworkDto.Name))
            {
                return BadRequest(new ErrorResponse { Message = "TenantId and Name are required", StatusCode = 400 });
            }

            var newFramework = new Framework
            {
                Id = Guid.NewGuid(),
                TenantId = frameworkDto.TenantId,
                Name = frameworkDto.Name,
                Code = frameworkDto.Code,
                Description = frameworkDto.Description,
                Version = frameworkDto.Version ?? "1.0",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _frameworkRepository.AddAsync(newFramework);
            await _frameworkRepository.SaveChangesAsync();

            var returnDto = new FrameworkDto
            {
                Id = newFramework.Id,
                TenantId = newFramework.TenantId,
                Name = newFramework.Name,
                Code = newFramework.Code,
                Description = newFramework.Description,
                Version = newFramework.Version,
                IsActive = newFramework.IsActive
            };

            return CreatedAtAction(nameof(GetFrameworkById), new { id = newFramework.Id }, returnDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating framework");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ErrorResponse { Message = "Error creating framework", StatusCode = 500 });
        }
    }

    /// <summary>
    /// Update a framework
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(FrameworkDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateFramework(Guid id, [FromBody] FrameworkDto frameworkDto)
    {
        try
        {
            var framework = await _frameworkRepository.GetByIdAsync(id);
            if (framework == null)
            {
                return NotFound(new ErrorResponse { Message = "Framework not found", StatusCode = 404 });
            }

            framework.Name = frameworkDto.Name;
            framework.Code = frameworkDto.Code;
            framework.Description = frameworkDto.Description;
            framework.IsActive = frameworkDto.IsActive;

            _frameworkRepository.Update(framework);
            await _frameworkRepository.SaveChangesAsync();

            var returnDto = new FrameworkDto
            {
                Id = framework.Id,
                TenantId = framework.TenantId,
                Name = framework.Name,
                Code = framework.Code,
                Description = framework.Description,
                Version = framework.Version,
                IsActive = framework.IsActive
            };

            return Ok(returnDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating framework {FrameworkId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ErrorResponse { Message = "Error updating framework", StatusCode = 500 });
        }
    }

    /// <summary>
    /// Delete a framework
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteFramework(Guid id)
    {
        try
        {
            var framework = await _frameworkRepository.GetByIdAsync(id);
            if (framework == null)
            {
                return NotFound(new ErrorResponse { Message = "Framework not found", StatusCode = 404 });
            }

            _frameworkRepository.Delete(framework);
            await _frameworkRepository.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting framework {FrameworkId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ErrorResponse { Message = "Error deleting framework", StatusCode = 500 });
        }
    }
}
