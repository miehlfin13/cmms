namespace Synith.UserAccount.Application.Validators;
public class UserValidator : EntityValidator<User>, IUserValidator
{
    public override IEntityValidator<User> IncludeDefaultValidations()
    {
        IncludeUsername();
        IncludeEmail();
        IncludeLanguageId();
        return this;
    }

    public IUserValidator IncludeUsername()
    {
        ValidateString(entity => entity.Username, maxLength: 50);
        return this;
    }

    public IUserValidator IncludePassword()
    {
        ValidateString(entity => entity.Password, minLength: 8, maxLength: 50);
        return this;
    }

    public IUserValidator IncludeEmail()
    {
        ValidateString(entity => entity.Email, isEmail: true);
        return this;
    }

    public IUserValidator IncludeLanguageId()
    {
        ValidateInt(entity => entity.LanguageId, greaterThan: 0);
        return this;
    }
}
