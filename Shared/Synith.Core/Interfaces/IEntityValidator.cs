using FluentValidation.Results;
using Synith.Core.Base;

namespace Synith.Core.Interfaces;
public interface IEntityValidator<TEntity> where TEntity : Entity
{
    IEntityValidator<TEntity> IncludeDefaultValidations();
    IEntityValidator<TEntity> IncludeId();
    IEntityValidator<TEntity> IncludeMustBeZeroId();
    ValidationResult Validate(TEntity entity);
}
