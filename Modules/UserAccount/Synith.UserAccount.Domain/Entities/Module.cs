namespace Synith.UserAccount.Domain.Entities;

[Table("Modules", Schema = "uac")]
public class Module
{
    public int Id { get; set; }
    public string Code { get; set; } = "";
}
