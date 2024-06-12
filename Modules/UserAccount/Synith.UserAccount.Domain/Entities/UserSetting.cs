namespace Synith.UserAccount.Domain.Entities;

[Table("UserSettings", Schema = "uac")]
public class UserSetting
{
    public int UserId { get; set; }
    public string Name { get; set; } = "";
    public string Value { get; set; } = "";
}
