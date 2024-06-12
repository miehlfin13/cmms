using FluentValidation.TestHelper;
using Synith.UserAccount.Application.Validators;

namespace Synith.UserAccount.Test.Unit.Validators.UserTest;
public partial class UserValidatorUnitTest
{
    [Theory]
    [InlineData("ABCDEFGH", "test@synith.com")]
    public void IncludeDefaultValidations_Valid_NoError(string username, string email)
    {
        var validator = new UserValidator().IncludeDefaultValidations();
        var result = validator.Validate(new User()
        {
            Username = username,
            Email = email,
            LanguageId = 1
        });
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData("", "test")]
    public void IncludeDefaultValidations_Invalid_HasError(string username, string email)
    {
        var validator = new UserValidator();
        validator.IncludeDefaultValidations();
        var result = validator.TestValidate(
            new User()
            {
                Username = username,
                Email = email
            });
        result.ShouldHaveValidationErrorFor(user => user.Username);
        result.ShouldHaveValidationErrorFor(user => user.Email);
    }
}
