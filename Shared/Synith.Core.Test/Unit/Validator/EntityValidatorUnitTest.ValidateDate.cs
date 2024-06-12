using FluentValidation.TestHelper;
using System.Globalization;

namespace Synith.Core.Test.Unit.Base.Validator;
partial class EntityValidatorUnitTest
{
    #region Default
    [Fact]
    public void ValidateDate_Default_Valid_NoError()
    {
        TestEntityValidator validator = new();
        validator.ValidateDate(entity => entity.DateTimeProperty);
        validator.TestValidate(new TestEntity() { DateTimeProperty = DateTime.UtcNow })
            .ShouldNotHaveValidationErrorFor(entity => entity.DateTimeProperty);
    }

    [Fact]
    public void ValidateDate_Default_Invalid_HasError()
    {
        DateTime? value = null;
        TestEntityValidator validator = new();
        validator.ValidateDate(entity => entity.DateTimeProperty);
        validator.TestValidate(new TestEntity() { DateTimeProperty = value })
            .ShouldHaveValidationErrorFor(entity => entity.DateTimeProperty)
            .WithErrorMessage(ErrorMessageProvider.NotNull(nameof(TestEntity.DateTimeProperty).AddSpaces()));
    }
    #endregion

    #region IsNullable
    [Theory]
    [InlineData(null)]
    [InlineData("2023-10-26")]
    public void ValidateDate_IsNullable_Valid_NoError(string? value)
    {
        DateTime? actual = value == null ? null : DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);

        TestEntityValidator validator = new();
        validator.ValidateDate(entity => entity.DateTimeProperty, isNullable: true);
        validator.TestValidate(new TestEntity() { DateTimeProperty = actual })
            .ShouldNotHaveValidationErrorFor(entity => entity.DateTimeProperty);
    }

    [Fact]
    public void ValidateDate_IsNullable_Invalid_HasError()
    {
        TestEntityValidator validator = new();
        DateTime expected = validator.MinimumDate();
        string actual = "01/01/0001 12:00:00 am";
        validator.ValidateDate(entity => entity.DateTimeProperty, isNullable: true);
        validator.TestValidate(new TestEntity() { DateTimeProperty = DateTime.Parse(actual) })
            .ShouldHaveValidationErrorFor(entity => entity.DateTimeProperty)
            .WithErrorMessage(ErrorMessageProvider.GreaterThanOrEqual(nameof(TestEntity.DateTimeProperty).AddSpaces(), actual, expected));
    }
    #endregion
}
