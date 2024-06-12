using Microsoft.AspNetCore.Authorization;
using Synith.UserAccount.Domain.DataTransferObjects.User;
using System.Security.Claims;

namespace Synith.UserAccount.Api.Controllers;
[Route($"{nameof(UserAccount)}/[controller]")]
public class UserController : EntityController<UserRetrieve>
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _service;
    private readonly ICryptographyService _cryptography;
    private readonly ITokenService _token;

    public UserController(ILogger<UserController> logger, IUserService service,
        ICryptographyService cryptography, ITokenService token) : base(logger, service)
    {
        _logger = logger;
        _service = service;
        _cryptography = cryptography;
        _token = token;
    }

    [RequiresPermission(PermissionCode.User.View)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public override Task<IActionResult> RetrieveAllAsync()
    {
        return base.RetrieveAllAsync();
    }

    [RequiresPermission(PermissionCode.User.View)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public override Task<IActionResult> RetrieveByIdAsync(int id)
    {
        return base.RetrieveByIdAsync(id);
    }

    [RequiresPermission(PermissionCode.User.Deactivate)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public override Task<IActionResult> DeactivateAsync(int id)
    {
        return base.DeactivateAsync(id);
    }

    [HttpPost]
    [RequiresPermission(PermissionCode.User.Add)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateAsync(UserCreate user)
    {
        try
        {
            return StatusCode(StatusCodes.Status201Created, await _service.CreateAsync(user));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error has occured while creating a new user({username}).", user.Username);
            return BadRequest(ex.GetSqlErrorMessage(_cryptography.Decrypt));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error has occured while creating a new user({username}).", user.Username);
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    [RequiresPermission(PermissionCode.User.Edit)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateAsync(UserUpdate user)
    {
        try
        {
            return Ok(await _service.UpdateAsync(user));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error has occured while updating a user({userId}).", user.Id);
            return BadRequest(ex.GetSqlErrorMessage(_cryptography.Decrypt));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error has occured while updating a user({userId}).", user.Id);
            return BadRequest(ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpGet("Languages")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Language>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RetrieveLanguagesAsync()
    {
        try
        {
            return Ok(await _service.RetrieveLanguagesAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error has occured while retreiving languages.");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("Roles/{userId}")]
    [RequiresPermission(PermissionCode.User.View)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserRoleRetrieve>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RetrieveUserRolesAsync(int userId)
    {
        try
        {
            return Ok(await _service.RetrieveUserRolesAsync(userId));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error has occured while retreiving user roles({userId}).", userId);
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("Roles/{userId}")]
    [RequiresPermission(PermissionCode.User.Edit)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateUserRolesAsync(int userId, IEnumerable<int> roleIds)
    {
        try
        {
            await _service.UpdateUserRolesAsync(userId, roleIds);
            return Ok();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error has occured while updating user roles({userId}).", userId);
            return BadRequest(ex.GetSqlErrorMessage());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error has occured while updating user roles({userId}).", userId);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("Settings/{userId}")]
    [RequiresPermission(PermissionCode.User.View)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserSetting>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RetrieveSettingsAsync(int userId)
    {
        try
        {
            return Ok(await _service.RetrieveSettingAsync(userId));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error has occured while retreiving user settings.");
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("Settings")]
    [RequiresPermission(PermissionCode.User.Edit)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateSettingAsync(UserSetting setting)
    {
        try
        {
            await _service.UpdateSettingAsync(setting);
            return Ok();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error has occured while updating user setting({userId}.{name}).", setting.UserId, setting.Name);
            return BadRequest(ex.GetSqlErrorMessage());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error has occured while updating user setting({userId}.{name}).", setting.UserId, setting.Name);
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("PasswordChange")]
    [RequiresPermission(PermissionCode.User.Edit)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdatePasswordAsync(UserPasswordChange passwordChange)
    {
        try
        {
            await _service.UpdatePasswordAsync(passwordChange);
            return Ok();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error has occured while updating password({userId}).", passwordChange.UserId);
            return BadRequest(ex.GetSqlErrorMessage());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error has occured while updating password({userId}).", passwordChange.UserId);
            return BadRequest(ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("ValidateLogin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ValidateLoginAsync(UserLogin login)
    {
        try
        {
            UserRetrieve user = await _service.ValidateLoginAsync(login);

            IEnumerable<Claim> accessClaims = GenerateAccessClaims(user);
            string access_token = _token.GenerateToken(accessClaims);

            IEnumerable<Claim> refreshClaims = GenerateRefreshClaims(user);
            string refresh_token = _token.GenerateToken(refreshClaims, true);

            return Ok(new { access_token, refresh_token });
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "An error has occured while validating user({username}).", login.Username);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("RefreshToken")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshToken()
    {
        try
        {
            // TODO: Validate if refresh token
            UserRetrieve user = await _service.RetrieveByIdAsync(_token.UserId);
            IEnumerable<Claim> accessClaims = GenerateAccessClaims(user);
            string access_token = _token.GenerateToken(accessClaims);

            if (_token.IsToBeExpired())
            {
                IEnumerable<Claim> refreshClaims = GenerateRefreshClaims(user);
                string refresh_token = _token.GenerateToken(refreshClaims, true);
                return Ok(new { access_token, refresh_token });
            }

            return Ok(new { access_token });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error has occured while requesting for new access token.");
            return BadRequest(ex.Message);
        }
    }

    private static List<Claim> GenerateAccessClaims(UserRetrieve user)
    {
        return
        [
            new(ClaimName.UserId, user.Id.ToString()),
            new(ClaimName.Username, user.Username),
            new(ClaimName.LanguageCode, user.Language.Code),
            new(ClaimName.RoleIds, string.Join(",", user.Roles.Select(x => x.Id)))
        ];
    }

    private static List<Claim> GenerateRefreshClaims(UserRetrieve user)
    {
        return
        [
            new(ClaimName.UserId, user.Id.ToString()),
            new(ClaimName.Username, user.Username)
        ];
    }
}
