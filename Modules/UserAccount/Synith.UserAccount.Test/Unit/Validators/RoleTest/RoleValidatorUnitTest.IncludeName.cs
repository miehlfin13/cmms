using Synith.UserAccount.Application.Validators;

namespace Synith.UserAccount.Test.Unit.Validators.RoleTest;
partial class RoleValidatorUnitTest
{
    [Theory]
    [InlineData("A")]
    [InlineData("ABCDEFGHIJABCDEFGHIJABCDEFGHIJABCDEFGHIJABCDEFGHIJ")] // length 50
    public void IncludeName_Valid_NoError(string value)
    {
        var validator = new RoleValidator().IncludeName();
        var result = validator.Validate(new Role() { Name = value });
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("ABCDEFGHIJABCDEFGHIJABCDEFGHIJABCDEFGHIJABCDEFGHIJA")] // length 51
    public void IncludeName_Invalid_HasError(string value)
    {
        var validator = new RoleValidator().IncludeName();
        var result = validator.Validate(new Role() { Name = value });
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }
}
