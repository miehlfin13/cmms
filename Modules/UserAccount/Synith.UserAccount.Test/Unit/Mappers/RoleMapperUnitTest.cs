using Synith.UserAccount.Application.Mappers;
using Synith.UserAccount.Domain.DataTransferObjects.Role;

namespace Synith.UserAccount.Test.Unit.Mappers;
public class RoleMapperUnitTest
{
    [Fact]
    public void RoleCreate_ToEntity()
    {
        RoleCreate expected = new()
        {
            Name = Guid.NewGuid().ToString()
        };

        Role actual = expected.ToEntity();

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void RoleUpdate_ToEntity()
    {
        RoleUpdate expected = new()
        {
            Id = 1,
            Name = Guid.NewGuid().ToString()
        };

        Role actual = expected.ToEntity();

        actual.Should().BeEquivalentTo(expected);
    }
}
