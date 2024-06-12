namespace Synith.UserAccount.Domain.Entities;

[Table("Permissions", Schema = "uac")]
public class Permission
{
    public int Id { get; set; }
    public string Code { get; set; } = "";
    public int ModuleId { get; set; }
    public int? ParentId { get; set; }
    public int SortIndex { get; set; }
}
