namespace Synith.UserAccount.Domain.Junction;

[Table("RolePermissions", Schema = "uac")]
public class RolePermission
{
    public int RoleId { get; set; }
    public int PermissionId { get; set; }
    public Permission Permission { get; set; } = default!;
}
