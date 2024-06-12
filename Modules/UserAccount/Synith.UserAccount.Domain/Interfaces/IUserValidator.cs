namespace Synith.UserAccount.Domain.Interfaces;
public interface IUserValidator : IEntityValidator<User>
{
    IUserValidator IncludeUsername();
    IUserValidator IncludePassword();
    IUserValidator IncludeEmail();
    IUserValidator IncludeLanguageId();
}
