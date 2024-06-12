using Synith.UserAccount.Domain.DataTransferObjects.User;

namespace Synith.UserAccount.Domain.Interfaces;
public interface IUserService : IEntityService<UserRetrieve>
{
    Task<UserRetrieve> CreateAsync(UserCreate userCreate);
    Task<UserRetrieve> UpdateAsync(UserUpdate userUpdate);
    Task<IEnumerable<Language>> RetrieveLanguagesAsync();
    Task<IEnumerable<UserRoleRetrieve>> RetrieveUserRolesAsync(int userId);
    Task UpdateUserRolesAsync(int userId, IEnumerable<int> roleIds);
    Task<IEnumerable<UserSetting>> RetrieveSettingAsync(int userId);
    Task UpdatePasswordAsync(UserPasswordChange passwordChange);
    Task UpdateSettingAsync(UserSetting setting);
    Task<UserRetrieve> ValidateLoginAsync(UserLogin login);
}
