namespace Synith.UserAccount.Domain.Entities;

[Table("Roles", Schema = "uac")]
public class Role : Entity
{
    public string Name { get; set; } = "";
}
