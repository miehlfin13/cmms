using FluentValidation.TestHelper;
using Synith.Core.Messages;

namespace Synith.Core.Test.Unit.Base.Validator;
partial class EntityValidatorUnitTest
{
    #region Default
    [Fact]
    public void ValidateString_Default_Valid_NoError()
    {
        TestEntityValidator validator = new();
        validator.ValidateString(entity => entity.StringProperty);
        validator.TestValidate(new TestEntity() { StringProperty = "Test" })
            .ShouldNotHaveValidationErrorFor(entity => entity.StringProperty);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void ValidateString_Default_Invalid_HasError(string value)
    {
        TestEntityValidator validator = new();
        validator.ValidateString(entity => entity.StringProperty);
        validator.TestValidate(new TestEntity() { StringProperty = value })
            .ShouldHaveValidationErrorFor(entity => entity.StringProperty)
            .WithErrorMessage(ErrorMessageProvider.Required(nameof(TestEntity.StringProperty).AddSpaces()));
    }
    #endregion

    #region IsNotEmpty
    [Fact]
    public void ValidateString_IsNotEmpty_Valid_NoError()
    {
        TestEntityValidator validator = new();
        validator.ValidateString(entity => entity.StringProperty);
        validator.TestValidate(new TestEntity() { StringProperty = "Test" })
            .ShouldNotHaveValidationErrorFor(entity => entity.StringProperty);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void ValidateString_IsNotEmpty_Invalid_HasError(string value)
    {
        TestEntityValidator validator = new();
        validator.ValidateString(entity => entity.StringProperty);
        validator.TestValidate(new TestEntity() { StringProperty = value })
            .ShouldHaveValidationErrorFor(entity => entity.StringProperty)
            .WithErrorMessage(ErrorMessageProvider.Required(nameof(TestEntity.StringProperty).AddSpaces()));
    }
    #endregion

    #region MinLength
    [Fact]
    public void ValidateString_MinLength_Valid_NoError()
    {
        TestEntityValidator validator = new();
        validator.ValidateString(entity => entity.StringProperty, minLength: 4);
        validator.TestValidate(new TestEntity() { StringProperty = "Test" })
            .ShouldNotHaveValidationErrorFor(entity => entity.StringProperty);
    }

    [Fact]
    public void ValidateString_MinLength_Invalid_HasError()
    {
        string value = "Test";
        int minLength = 5;
        TestEntityValidator validator = new();
        validator.ValidateString(entity => entity.StringProperty, minLength: minLength);
        validator.TestValidate(new TestEntity() { StringProperty = value })
            .ShouldHaveValidationErrorFor(entity => entity.StringProperty)
            .WithErrorMessage(ErrorMessageProvider.MinLength(nameof(TestEntity.StringProperty).AddSpaces(), value.Length, minLength));
    }
    #endregion

    #region MaxLength
    [Fact]
    public void ValidateString_MaxLength_Valid_NoError()
    {
        TestEntityValidator validator = new();
        validator.ValidateString(entity => entity.StringProperty, maxLength: 4);
        validator.TestValidate(new TestEntity() { StringProperty = "Test" })
            .ShouldNotHaveValidationErrorFor(entity => entity.StringProperty);
    }

    [Fact]
    public void ValidateString_MaxLength_Invalid_HasError()
    {
        string value = "Test";
        int maxLength = 3;
        TestEntityValidator validator = new();
        validator.ValidateString(entity => entity.StringProperty, maxLength: maxLength);
        validator.TestValidate(new TestEntity() { StringProperty = value })
            .ShouldHaveValidationErrorFor(entity => entity.StringProperty)
            .WithErrorMessage(ErrorMessageProvider.MaxLength(nameof(TestEntity.StringProperty).AddSpaces(), value.Length, maxLength));
    }
    #endregion

    #region IsEmail
    [Fact]
    public void ValidateString_IsEmail_Valid_NoError()
    {
        TestEntityValidator validator = new();
        validator.ValidateString(entity => entity.StringProperty, isEmail: true);
        validator.TestValidate(new TestEntity() { StringProperty = "test@vnacesoft.com" })
            .ShouldNotHaveValidationErrorFor(entity => entity.StringProperty);
    }

    [Fact]
    public void ValidateString_IsEmail_Invalid_HasError()
    {
        string value = "testvnacesoft.com";
        TestEntityValidator validator = new();
        validator.ValidateString(entity => entity.StringProperty, isEmail: true);
        validator.TestValidate(new TestEntity() { StringProperty = value })
            .ShouldHaveValidationErrorFor(entity => entity.StringProperty)
            .WithErrorMessage(ErrorMessageProvider.Invalid(nameof(TestEntity.StringProperty).AddSpaces(), value));
    }
    #endregion
}
