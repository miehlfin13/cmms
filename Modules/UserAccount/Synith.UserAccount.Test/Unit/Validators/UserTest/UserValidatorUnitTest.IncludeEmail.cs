using Synith.UserAccount.Application.Validators;

namespace Synith.UserAccount.Test.Unit.Validators.UserTest;
partial class UserValidatorUnitTest
{
    [Theory]
    [InlineData("test@synith.com")]
    [InlineData("test.123@synith.com")]
    public void IncludeEmail_Valid_NoError(string value)
    {
        var validator = new UserValidator().IncludeEmail();
        var result = validator.Validate(new User() { Email = value });
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData("test")]
    [InlineData("test.com")]
    public void IncludeEmail_Invalid_HasError(string value)
    {
        var validator = new UserValidator().IncludeEmail();
        var result = validator.Validate(new User() { Email = value });
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }
}
