using Synith.UserAccount.Domain.DataTransferObjects.Role;

namespace Synith.UserAccount.Application.Services;
public class RoleService : EntityService<Role, Role>, IRoleService
{
    private readonly ILogger<RoleService> _logger;
    private readonly UserAccountDbContext _context;
    private readonly IRoleValidator _validator;

    public RoleService(ILogger<RoleService> logger, UserAccountDbContext context, IRoleValidator validator)
        : base(logger, context)
    {
        _logger = logger;
        _context = context;
        _validator = validator;
    }

    protected override Role ToRetrieveDto(Role role)
    {
        return role;
    }

    public async Task<Role> CreateAsync(RoleCreate roleCreate)
    {
        _logger.LogInformation("Creating role({name}).", roleCreate.Name);

        Role role = roleCreate.ToEntity();

        ValidationResult result = _validator
            .IncludeDefaultValidations()
            .IncludeMustBeZeroId()
            .Validate(role);

        if (result.IsValid)
        {
            await _context.AddAsync(role);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Role({name}) has been successfully created", role.Name);
            return role;
        }
        else
        {
            string errorJson = result.ToJson();
            _logger.LogInformation("One or more role details is invalid.");
            _logger.LogError("{message}", errorJson);
            throw new ValidationException(errorJson);
        }
    }

    public async Task<Role> UpdateAsync(RoleUpdate roleUpdate)
    {
        _logger.LogInformation("Updating role({roleId}).", roleUpdate.Id);

        Role role = roleUpdate.ToEntity();

        ValidationResult result = _validator
            .IncludeDefaultValidations()
            .IncludeId()
            .Validate(role);

        if (result.IsValid)
        {
            if (!await _context.Roles.AnyAsync(x => x.Id == role.Id))
                throw new KeyNotFoundException(ErrorMessageProvider.NotFound($"{nameof(Role)}.{nameof(Role.Id)}", role.Id.ToString()));

            _context.Attach(role);
            _context.Entry(role).Property(nameof(Role.Name)).IsModified = true;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Role({roleId}) has been successfully updated", role.Id);
            return role;
        }
        else
        {
            string errorJson = result.ToJson();
            _logger.LogInformation("One or more role details is invalid.");
            _logger.LogError("{message}", errorJson);
            throw new ValidationException(errorJson);
        }
    }

    public async Task<IEnumerable<Module>> RetrieveModulesAsync()
    {
        _logger.LogInformation("Retreiving modules.");
        IEnumerable<Module> modules = await _context.Modules.ToListAsync();
        _logger.LogInformation("Modules have been successfully retreived.");
        return modules;
    }

    public async Task<IEnumerable<Permission>> RetrievePermissionsAsync()
    {
        _logger.LogInformation("Retreiving permissions.");
        IEnumerable<Permission> permissions = await _context.Permissions.ToListAsync();
        _logger.LogInformation("Permissions have been successfully retreived.");
        return permissions;
    }

    public async Task<IEnumerable<Permission>> RetrieveRolePermissionsAsync(int roleId)
    {
        _logger.LogInformation("Retreiving role permissions({roleId}).", roleId);
        IEnumerable<Permission> permissions = await _context.RolePermissions
            .Where(x => x.RoleId == roleId)
            .Select(x => x.Permission)
            .ToListAsync();
        _logger.LogInformation("Role permissions({roleId}) have been successfully retreived.", roleId);
        return permissions;
    }

    public async Task UpdateRolePermissionsAsync(int roleId, IEnumerable<int> permissionIds)
    {
        _logger.LogInformation("Updating role permissions({roleId}).", roleId);

        string sql = "EXEC [uac].[Role_UpdateRolePermissions] @RoleId, @PermissionIds";

        List<SqlParameter> parameters =
            [
                new("@RoleId", roleId),
                new("@PermissionIds", string.Join(",", permissionIds))
            ];

        await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        _logger.LogInformation("Role permissions({roleId}) have been successfully updated.", roleId);
    }
}
