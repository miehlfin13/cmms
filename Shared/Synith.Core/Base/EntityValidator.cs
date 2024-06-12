using FluentValidation;
using Synith.Core.Interfaces;

namespace Synith.Core.Base;
public abstract partial class EntityValidator<TEntity> : AbstractValidator<TEntity>, IEntityValidator<TEntity>
    where TEntity : Entity
{
    public abstract IEntityValidator<TEntity> IncludeDefaultValidations();

    public IEntityValidator<TEntity> IncludeId()
    {
        ValidateInt(entity => entity.Id, greaterThan: 0);
        return this;
    }

    public IEntityValidator<TEntity> IncludeMustBeZeroId()
    {
        ValidateInt(entity => entity.Id, equalTo: 0);
        return this;
    }
}
