using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Synith.Core.Interfaces;

namespace Synith.Core.Base;
[Route("[controller]")]
[ApiController]
[Authorize]
public class EntityController<TRetrieveDto> : ControllerBase
    where TRetrieveDto : class
{
    private readonly ILogger<EntityController<TRetrieveDto>> _logger;
    private readonly IEntityService<TRetrieveDto> _service;

    public EntityController(ILogger<EntityController<TRetrieveDto>> logger, IEntityService<TRetrieveDto> service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public virtual async Task<IActionResult> DeactivateAsync(int id)
    {
        try
        {
            await _service.DeactivateAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error has occured while deactivating entity({id}).", id);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public virtual async Task<IActionResult> RetrieveAllAsync()
    {
        try
        {
            return Ok(await _service.RetrieveAllAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error has occured while retrieving entities.");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public virtual async Task<IActionResult> RetrieveByIdAsync(int id)
    {
        try
        {
            return Ok(await _service.RetrieveByIdAsync(id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error has occured while retrieving entity({id}).", id);
            return BadRequest(ex.Message);
        }
    }
}
