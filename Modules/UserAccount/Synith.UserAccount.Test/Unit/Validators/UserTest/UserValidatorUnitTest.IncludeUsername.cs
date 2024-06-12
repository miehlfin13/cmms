using Synith.UserAccount.Application.Validators;

namespace Synith.UserAccount.Test.Unit.Validators.UserTest;
partial class UserValidatorUnitTest
{
    [Theory]
    [InlineData("A")]
    [InlineData("ABCDEFGHIJABCDEFGHIJABCDEFGHIJABCDEFGHIJABCDEFGHIJ")] // length 50
    public void IncludeUsername_Valid_NoError(string value)
    {
        var validator = new UserValidator().IncludeUsername();
        var result = validator.Validate(new User() { Username = value });
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("ABCDEFGHIJABCDEFGHIJABCDEFGHIJABCDEFGHIJABCDEFGHIJA")] // length 51
    public void IncludeUsername_Invalid_HasError(string value)
    {
        var validator = new UserValidator().IncludeUsername();
        var result = validator.Validate(new User() { Username = value });
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }
}
