using FluentValidation.TestHelper;
using Synith.UserAccount.Application.Validators;

namespace Synith.UserAccount.Test.Unit.Validators.RoleTest;
public partial class RoleValidatorUnitTest
{
    [Theory]
    [InlineData("A")]
    public void IncludeDefaultValidations_Valid_NoError(string name)
    {
        var validator = new RoleValidator().IncludeDefaultValidations();
        var result = validator.Validate(new Role() { Name = name });
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    public void IncludeDefaultValidations_Invalid_HasError(string name)
    {
        var validator = new RoleValidator();
        validator.IncludeDefaultValidations();
        var result = validator.TestValidate(new Role() { Name = name });
        result.ShouldHaveValidationErrorFor(role => role.Name);
    }
}
