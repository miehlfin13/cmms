namespace Synith.UserAccount.Domain.DataTransferObjects.User;
public class UserUpdate
{
    public int Id { get; init; }
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public int LanguageId { get; set; }
    public IEnumerable<int>? Roles { get; set; }
}
