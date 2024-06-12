using FluentValidation;
using Synith.Core.Constants;
using Synith.Core.Messages;
using System.Linq.Expressions;
using System.Reflection;

namespace Synith.Core.Base;
partial class EntityValidator<TEntity>
{
    protected const BindingFlags IGNORE_CASE = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
    protected readonly DateTime MINIMUM_DATE = Convert.ToDateTime("1900-01-01");

    protected IRuleBuilder<TEntity, string> ValidateString(
        Expression<Func<TEntity, string>> expression,
        bool isNotEmpty = true,
        int minLength = 0,
        int maxLength = 0,
        bool isEmail = false)
    {
        IRuleBuilder<TEntity, string> rule = RuleFor(expression).Cascade(CascadeMode.Stop);

        if (isNotEmpty)
            rule.NotEmpty()
                .WithMessage(ErrorMessageProvider.Required(ValidationProperty.NAME));

        string propertyName = (expression.Body as MemberExpression)!.Member.Name;
        PropertyInfo propertyInfo = typeof(TEntity).GetProperty(propertyName, IGNORE_CASE)!;
        
        When(entity => !string.IsNullOrEmpty(propertyInfo.GetValue(entity)!.ToString()), () =>
        {
            if (minLength > 0)
                rule.MinimumLength(minLength)
                    .WithMessage(ErrorMessageProvider.MinLength(ValidationProperty.NAME, ValidationProperty.TOTAL_LENGTH, minLength));

            if (maxLength > 0)
                rule.MaximumLength(maxLength)
                    .WithMessage(ErrorMessageProvider.MaxLength(ValidationProperty.NAME, ValidationProperty.TOTAL_LENGTH, maxLength));

            if (isEmail)
                rule.EmailAddress()
                    .WithMessage(ErrorMessageProvider.Invalid(ValidationProperty.NAME, ValidationProperty.VALUE));
        });

        return rule;
    }

    protected IRuleBuilder<TEntity, int?> ValidateInt(
        Expression<Func<TEntity, int?>> expression,
        int? equalTo = null,
        int? greaterThan = null,
        int? greaterOrEqualThan = null,
        int? lessThan = null,
        int? lessOrEqualThan = null)
    {
        IRuleBuilder<TEntity, int?> rule = RuleFor(expression).Cascade(CascadeMode.Stop);

        if (equalTo != null)
            rule.Equal(equalTo.Value)
                .WithMessage(ErrorMessageProvider.NotExpected(ValidationProperty.NAME, ValidationProperty.VALUE, equalTo.Value));

        if (greaterThan != null)
            rule.GreaterThan(greaterThan.Value)
                .WithMessage(ErrorMessageProvider.GreaterThan(ValidationProperty.NAME, ValidationProperty.VALUE, greaterThan.Value));

        if (greaterOrEqualThan != null)
            rule.GreaterThanOrEqualTo(greaterOrEqualThan.Value)
                .WithMessage(ErrorMessageProvider.GreaterThanOrEqual(ValidationProperty.NAME, ValidationProperty.VALUE, greaterOrEqualThan.Value));

        if (lessThan != null)
            rule.LessThan(lessThan.Value)
                .WithMessage(ErrorMessageProvider.LessThan(ValidationProperty.NAME, ValidationProperty.VALUE, lessThan.Value));

        if (lessOrEqualThan != null)
            rule.LessThanOrEqualTo(lessOrEqualThan.Value)
                .WithMessage(ErrorMessageProvider.LessThanOrEqual(ValidationProperty.NAME, ValidationProperty.VALUE, lessOrEqualThan.Value));

        return rule;
    }

    protected IRuleBuilder<TEntity, DateTime?> ValidateDate(
        Expression<Func<TEntity, DateTime?>> expression,
        bool isNullable = false)
    {
        IRuleBuilder<TEntity, DateTime?> rule = RuleFor(expression).Cascade(CascadeMode.Stop);

        if (!isNullable)
        {
            rule.NotNull()
                .WithMessage(ErrorMessageProvider.NotNull(ValidationProperty.NAME));
        }

        string propertyName = (expression.Body as MemberExpression)?.Member.Name ?? "";
        PropertyInfo propertyInfo = typeof(TEntity).GetProperty(propertyName, IGNORE_CASE)!;

        When(entity => propertyInfo.GetValue(entity) != null, () =>
        {
            rule.GreaterThanOrEqualTo(MINIMUM_DATE)
                .WithMessage(ErrorMessageProvider.GreaterThanOrEqual(ValidationProperty.NAME, ValidationProperty.VALUE, MINIMUM_DATE));
        });

        return rule;
    }
}
