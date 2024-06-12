namespace Synith.UserAccount.Domain.Junction;

[Table("UserRoles", Schema = "uac")]
public class UserRole
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public Role Role { get; set; } = default!;
}
