namespace Synith.UserAccount.Domain.Entities;

[Table("Languages", Schema = "uac")]
public class Language
{
    public int Id { get; set; }
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string LocaleName { get; set; } = "";
}
