using Synith.UserAccount.Domain.DataTransferObjects.User;

namespace Synith.UserAccount.Application.Mappers;
public static class UserMapper
{
    public static User ToEntity(this UserCreate user)
    {
        return new User
        {
            Username = user.Username,
            Password = user.Password,
            Email = user.Email,
            LanguageId = user.LanguageId
        };
    }

    public static User ToEntity(this UserUpdate user)
    {
        return new User
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            LanguageId = user.LanguageId
        };
    }

    public static UserRetrieve ToRetrieveDto(this User user)
    {
        return new UserRetrieve
        {
            Id = user.Id,
            Status = user.Status,
            Username = user.Username,
            Email = user.Email,
            Language = user.Language,
            Roles = user.Roles == null ? default! :
                user.Roles.Select(r => new UserRoleRetrieve
                {
                    Id = r.RoleId,
                })
        };
    }
}
