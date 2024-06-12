using Synith.UserAccount.Application.Validators;

namespace Synith.UserAccount.Test.Unit.Validators.UserTest;
partial class UserValidatorUnitTest
{
    [Fact]
    public void IncludeLanguageId_Valid_NoError()
    {
        var validator = new UserValidator().IncludeLanguageId();
        var result = validator.Validate(new User() { LanguageId = 1 });
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void IncludeLanguageId_Invalid_HasError()
    {
        var validator = new UserValidator().IncludeLanguageId();
        var result = validator.Validate(new User());
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }
}
