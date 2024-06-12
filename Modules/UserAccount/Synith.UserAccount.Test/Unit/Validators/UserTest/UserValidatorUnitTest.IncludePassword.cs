using Synith.UserAccount.Application.Validators;

namespace Synith.UserAccount.Test.Unit.Validators.UserTest;
partial class UserValidatorUnitTest
{
    [Theory]
    [InlineData("ABCDEFGH")] // length 8
    [InlineData("ABCDEFGHIJABCDEFGHIJABCDEFGHIJABCDEFGHIJABCDEFGHIJ")] // length 50
    public void IncludePassword_Valid_NoError(string value)
    {
        var validator = new UserValidator().IncludePassword();
        var result = validator.Validate(new User() { Password = value });
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData("ABCDEFG")] // length 7
    [InlineData("ABCDEFGHIJABCDEFGHIJABCDEFGHIJABCDEFGHIJABCDEFGHIJA")] // length 51
    public void IncludePassword_Invalid_HasError(string value)
    {
        var validator = new UserValidator().IncludePassword();
        var result = validator.Validate(new User() { Password = value });
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }
}
