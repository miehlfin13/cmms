namespace Synith.UserAccount.Domain.DataTransferObjects.User;
public class UserCreate
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string Email { get; set; } = "";
    public int LanguageId { get; set; }
    public IEnumerable<int>? Roles { get; set; }
}
