using Synith.UserAccount.Domain.DataTransferObjects.Role;

namespace Synith.UserAccount.Domain.Interfaces;
public interface IRoleService : IEntityService<Role>
{
    Task<Role> CreateAsync(RoleCreate roleCreate);
    Task<Role> UpdateAsync(RoleUpdate roleUpdate);
    Task<IEnumerable<Module>> RetrieveModulesAsync();
    Task<IEnumerable<Permission>> RetrievePermissionsAsync();
    Task<IEnumerable<Permission>> RetrieveRolePermissionsAsync(int roleId);
    Task UpdateRolePermissionsAsync(int roleId, IEnumerable<int> permissionIds);
}
