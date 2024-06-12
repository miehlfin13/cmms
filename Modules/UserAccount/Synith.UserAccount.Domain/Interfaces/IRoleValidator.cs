namespace Synith.UserAccount.Domain.Interfaces;
public interface IRoleValidator : IEntityValidator<Role>
{
    IRoleValidator IncludeName();
}
