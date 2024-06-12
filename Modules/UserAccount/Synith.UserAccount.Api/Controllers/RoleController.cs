using Synith.UserAccount.Domain.DataTransferObjects.Role;

namespace Synith.UserAccount.Api.Controllers;
[Route($"{nameof(UserAccount)}/[controller]")]
public class RoleController : EntityController<Role>
{
    private readonly ILogger<RoleController> _logger;
    private readonly IRoleService _service;

    public RoleController(ILogger<RoleController> logger, IRoleService service) : base(logger, service)
    {
        _logger = logger;
        _service = service;
    }

    [RequiresPermission(PermissionCode.Role.View)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public override Task<IActionResult> RetrieveAllAsync()
    {
        return base.RetrieveAllAsync();
    }

    [RequiresPermission(PermissionCode.Role.View)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public override Task<IActionResult> RetrieveByIdAsync(int id)
    {
        return base.RetrieveByIdAsync(id);
    }

    [RequiresPermission(PermissionCode.Role.Deactivate)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public override Task<IActionResult> DeactivateAsync(int id)
    {
        return base.DeactivateAsync(id);
    }

    [HttpPost]
    [RequiresPermission(PermissionCode.Role.Add)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Role))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateAsync(RoleCreate role)
    {
        try
        {
            return StatusCode(StatusCodes.Status201Created, await _service.CreateAsync(role));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error has occured while creating a role({name}).", role.Name);
            return BadRequest(ex.GetSqlErrorMessage());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error has occured while creating a role({name}).", role.Name);
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    [RequiresPermission(PermissionCode.Role.Edit)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Role))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateAsync(RoleUpdate role)
    {
        try
        {
            return Ok(await _service.UpdateAsync(role));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error has occured while updating a role({roleId}).", role.Id);
            return BadRequest(ex.GetSqlErrorMessage());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error has occured while updating a role({roleId}).", role.Id);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("Modules")]
    [RequiresPermission(PermissionCode.Role.View)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Module>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RetrieveModulesAsync()
    {
        try
        {
            return Ok(await _service.RetrieveModulesAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error has occured while retreiving modules.");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("Permissions")]
    [RequiresPermission(PermissionCode.Role.View)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Permission>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RetrievePermissionsAsync()
    {
        try
        {
            return Ok(await _service.RetrievePermissionsAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error has occured while retreiving permissions.");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("Permissions/{roleId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Permission>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RetrieveRolePermissionsAsync(int roleId)
    {
        try
        {
            return Ok(await _service.RetrieveRolePermissionsAsync(roleId));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error has occured while retreiving role permissions({roleId}).", roleId);
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("Permissions/{roleId}")]
    [RequiresPermission(PermissionCode.Role.Edit)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateRolePermissionsAsync(int roleId, IEnumerable<int> permissions)
    {
        try
        {
            await _service.UpdateRolePermissionsAsync(roleId, permissions);
            return Ok();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error has occured while updating role permissions({roleId}).", roleId);
            return BadRequest(ex.GetSqlErrorMessage());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error has occured while updating role permissions({roleId}).", roleId);
            return BadRequest(ex.Message);
        }
    }
}
