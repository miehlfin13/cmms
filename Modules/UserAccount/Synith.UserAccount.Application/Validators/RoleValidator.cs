namespace Synith.UserAccount.Application.Validators;

public class RoleValidator : EntityValidator<Role>, IRoleValidator
{
    public override IEntityValidator<Role> IncludeDefaultValidations()
    {
        IncludeName();
        return this;
    }

    public IRoleValidator IncludeName()
    {
        ValidateString(entity => entity.Name, maxLength: 50);
        return this;
    }
}
