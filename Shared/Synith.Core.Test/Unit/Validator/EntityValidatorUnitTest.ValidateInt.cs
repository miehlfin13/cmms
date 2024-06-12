using FluentValidation.TestHelper;

namespace Synith.Core.Test.Unit.Base.Validator;
partial class EntityValidatorUnitTest
{
    #region Default
    [Theory]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    [InlineData(0)]
    public void ValidateInt_Default_Any_NoError(int value)
    {
        TestEntityValidator validator = new();
        validator.ValidateInt(entity => entity.IntProperty);
        validator.TestValidate(new TestEntity() { IntProperty = value })
            .ShouldNotHaveValidationErrorFor(entity => entity.IntProperty);
    }
    #endregion

    #region EqualTo
    [Fact]
    public void ValidateInt_EqualTo_Valid_NoError()
    {
        TestEntityValidator validator = new();
        validator.ValidateInt(entity => entity.IntProperty, equalTo: 0);
        validator.TestValidate(new TestEntity())
            .ShouldNotHaveValidationErrorFor(entity => entity.IntProperty);
    }

    [Fact]
    public void ValidateInt_EqualTo_Invalid_HasError()
    {
        int expected = 1;
        int actual = 0;
        TestEntityValidator validator = new();
        validator.ValidateInt(entity => entity.IntProperty, equalTo: expected);
        validator.TestValidate(new TestEntity() { IntProperty = actual })
            .ShouldHaveValidationErrorFor(entity => entity.IntProperty)
            .WithErrorMessage(ErrorMessageProvider.NotExpected(nameof(TestEntity.IntProperty).AddSpaces(), actual, expected));
    }
    #endregion

    #region GreaterThan
    [Fact]
    public void ValidateInt_GreaterThan_Valid_NoError()
    {
        TestEntityValidator validator = new();
        validator.ValidateInt(entity => entity.IntProperty, greaterThan: 0);
        validator.TestValidate(new TestEntity() { IntProperty = 1 })
            .ShouldNotHaveValidationErrorFor(entity => entity.IntProperty);
    }

    [Fact]
    public void ValidateInt_GreaterThan_Invalid_HasError()
    {
        int expected = 0;
        int actual = 0;
        TestEntityValidator validator = new();
        validator.ValidateInt(entity => entity.IntProperty, greaterThan: expected);
        validator.TestValidate(new TestEntity() { IntProperty = actual })
            .ShouldHaveValidationErrorFor(entity => entity.IntProperty)
            .WithErrorMessage(ErrorMessageProvider.GreaterThan(nameof(TestEntity.IntProperty).AddSpaces(), actual, expected));
    }
    #endregion

    #region GreaterOrEqualThan
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void ValidateInt_GreaterOrEqualThan_Valid_NoError(int actual)
    {
        TestEntityValidator validator = new();
        validator.ValidateInt(entity => entity.IntProperty, greaterOrEqualThan: 0);
        validator.TestValidate(new TestEntity() { IntProperty = actual })
            .ShouldNotHaveValidationErrorFor(entity => entity.IntProperty);
    }

    [Fact]
    public void ValidateInt_GreaterOrEqualThan_Invalid_HasError()
    {
        int expected = 0;
        int actual = -1;
        TestEntityValidator validator = new();
        validator.ValidateInt(entity => entity.IntProperty, greaterOrEqualThan: expected);
        validator.TestValidate(new TestEntity() { IntProperty = actual })
            .ShouldHaveValidationErrorFor(entity => entity.IntProperty)
            .WithErrorMessage(ErrorMessageProvider.GreaterThanOrEqual(nameof(TestEntity.IntProperty).AddSpaces(), actual, expected));
    }
    #endregion

    #region LessThan
    [Fact]
    public void ValidateInt_LessThan_Valid_NoError()
    {
        TestEntityValidator validator = new();
        validator.ValidateInt(entity => entity.IntProperty, lessThan: 1);
        validator.TestValidate(new TestEntity())
            .ShouldNotHaveValidationErrorFor(entity => entity.IntProperty);
    }

    [Fact]
    public void ValidateInt_LessThan_Invalid_HasError()
    {
        int expected = 0;
        int actual = 0;
        TestEntityValidator validator = new();
        validator.ValidateInt(entity => entity.IntProperty, lessThan: expected);
        validator.TestValidate(new TestEntity() { IntProperty = actual })
            .ShouldHaveValidationErrorFor(entity => entity.IntProperty)
            .WithErrorMessage(ErrorMessageProvider.LessThan(nameof(TestEntity.IntProperty).AddSpaces(), actual, expected));
    }

    #endregion

    #region LessOrEqualThan
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void ValidateInt_LessOrEqualThan_Valid_NoError(int actual)
    {
        TestEntityValidator validator = new();
        validator.ValidateInt(entity => entity.IntProperty, lessOrEqualThan: 1);
        validator.TestValidate(new TestEntity() { IntProperty = actual })
            .ShouldNotHaveValidationErrorFor(entity => entity.IntProperty);
    }

    [Fact]
    public void ValidateInt_LessOrEqualThan_Invalid_HasError()
    {
        int expected = 0;
        int actual = 1;
        TestEntityValidator validator = new();
        validator.ValidateInt(entity => entity.IntProperty, lessOrEqualThan: expected);
        validator.TestValidate(new TestEntity() { IntProperty = actual })
            .ShouldHaveValidationErrorFor(entity => entity.IntProperty)
            .WithErrorMessage(ErrorMessageProvider.LessThanOrEqual(nameof(TestEntity.IntProperty).AddSpaces(), actual, expected));
    }
    #endregion
}
