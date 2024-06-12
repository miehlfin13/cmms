using FluentValidation.TestHelper;

namespace Synith.Core.Test.Unit.Base.Validator;
partial class EntityValidatorUnitTest
{
    [Fact]
    public void IncludeMustBeZeroId_Valid_NoError()
    {
        TestEntityValidator validator = new();
        validator.IncludeMustBeZeroId();
        validator.TestValidate(new TestEntity())
            .ShouldNotHaveValidationErrorFor(entity => entity.Id);
    }

    [Fact]
    public void IncludeMustBeZeroId_Invalid_HasError()
    {
        TestEntityValidator validator = new();
        validator.IncludeMustBeZeroId();
        validator.TestValidate(new TestEntity() { Id = 1 })
            .ShouldHaveValidationErrorFor(entity => entity.Id);
    }
}
