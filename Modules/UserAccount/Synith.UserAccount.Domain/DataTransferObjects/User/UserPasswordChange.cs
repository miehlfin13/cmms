namespace Synith.UserAccount.Domain.DataTransferObjects.User;
public class UserPasswordChange
{
    public int UserId { get; set; }
    public string CurrentPassword { get; set; } = "";
    public string NewPassword { get; set; } = "";
}
