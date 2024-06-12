using FluentValidation;
using System.Linq.Expressions;

namespace Synith.Core.Test.Unit.Base.Validator;
public partial class EntityValidatorUnitTest
{
    private class TestEntity : Entity
    {
        public string StringProperty { get; set; } = "";
        public int IntProperty { get; set; }
        public DateTime? DateTimeProperty { get; set; }
    }

    private class TestEntityValidator : EntityValidator<TestEntity>
    {
        public override IEntityValidator<TestEntity> IncludeDefaultValidations()
        {
            return this;
        }

        #region Exposed protected for unit tests
        public DateTime MinimumDate() => MINIMUM_DATE;

        public new IRuleBuilder<TestEntity, string> ValidateString(
            Expression<Func<TestEntity, string>> expression,
            bool isNotEmpty = true,
            int minLength = 0,
            int maxLength = 0,
            bool isEmail = false)
        => base.ValidateString(expression, isNotEmpty, minLength, maxLength, isEmail);

        public new IRuleBuilder<TestEntity, int?> ValidateInt(
            Expression<Func<TestEntity, int?>> expression,
            int? equalTo = null,
            int? greaterThan = null,
            int? greaterOrEqualThan = null,
            int? lessThan = null,
            int? lessOrEqualThan = null)
        => base.ValidateInt(expression, equalTo, greaterThan, greaterOrEqualThan, lessThan, lessOrEqualThan);

        public new IRuleBuilder<TestEntity, DateTime?> ValidateDate(
            Expression<Func<TestEntity, DateTime?>> expression,
            bool isNullable = false)
        => base.ValidateDate(expression, isNullable);
        #endregion
    }
}
