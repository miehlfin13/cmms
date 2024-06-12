using FluentValidation.TestHelper;

namespace Synith.Core.Test.Unit.Base.Validator;
partial class EntityValidatorUnitTest
{
    [Fact]
    public void IncludeId_Valid_NoError()
    {
        TestEntityValidator validator = new();
        validator.IncludeId();
        validator.TestValidate(new TestEntity() { Id = 1 })
            .ShouldNotHaveValidationErrorFor(entity => entity.Id);
    }

    [Fact]
    public void IncludeId_Invalid_HasError()
    {
        TestEntityValidator validator = new();
        validator.IncludeId();
        validator.TestValidate(new TestEntity())
            .ShouldHaveValidationErrorFor(entity => entity.Id);
    }
}
