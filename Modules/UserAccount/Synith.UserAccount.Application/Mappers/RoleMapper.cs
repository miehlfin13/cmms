using Synith.UserAccount.Domain.DataTransferObjects.Role;

namespace Synith.UserAccount.Application.Mappers;
public static class RoleMapper
{
    public static Role ToEntity(this RoleCreate role)
    {
        return new Role
        {
            Name = role.Name
        };
    }

    public static Role ToEntity(this RoleUpdate role)
    {
        return new Role
        {
            Id = role.Id,
            Name = role.Name
        };
    }
}
