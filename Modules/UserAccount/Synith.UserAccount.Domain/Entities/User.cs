using Synith.UserAccount.Domain.Enums;
using Synith.UserAccount.Domain.Junction;

namespace Synith.UserAccount.Domain.Entities;

[Table("Users", Schema = "uac")]
public class User : Entity
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string Email { get; set; } = "";
    public DateTime CreatedDate { get; init; } = DateTime.Now;
    public DateTime? LastLogin { get; set; }
    public int LanguageId { get; set; }
    public Language Language { get; set; } = default!;
    public IEnumerable<UserRole> Roles { get; set; } = default!;

    public new UserStatus Status
    {
        get => (UserStatus)base.Status;
        set => base.Status = (GenericStatus)value;
    }
}
