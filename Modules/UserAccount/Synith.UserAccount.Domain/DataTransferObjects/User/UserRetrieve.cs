using Synith.UserAccount.Domain.Enums;

namespace Synith.UserAccount.Domain.DataTransferObjects.User;
public class UserRetrieve
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public UserStatus Status { get; set; } = UserStatus.Pending;
    public Language Language { get; set; } = default!;
    public IEnumerable<UserRoleRetrieve> Roles { get; set; } = default!;
}
