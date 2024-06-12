using Synith.UserAccount.Application.Mappers;
using Synith.UserAccount.Domain.DataTransferObjects.User;
using Synith.UserAccount.Domain.Enums;
using Synith.UserAccount.Domain.Junction;

namespace Synith.UserAccount.Test.Unit.Mappers;
public class UserMapperUnitTest
{
    [Fact]
    public void UserCreate_ToEntity()
    {
        UserCreate expected = new()
        {
            Username = Guid.NewGuid().ToString(),
            Password = Guid.NewGuid().ToString(),
            Email = Guid.NewGuid().ToString() + "@synith.com",
            LanguageId = 1
        };

        User actual = expected.ToEntity();

        actual.Should().BeEquivalentTo(expected,
            opt => opt.Excluding(x => x.Roles));
    }

    [Fact]
    public void UserUpdate_ToEntity()
    {
        UserUpdate expected = new()
        {
            Id = 1,
            Username = Guid.NewGuid().ToString(),
            Email = Guid.NewGuid().ToString() + "@synith.com",
            LanguageId = 1
        };

        User actual = expected.ToEntity();

        actual.Should().BeEquivalentTo(expected,
            opt => opt.Excluding(x => x.Roles));
    }

    [Fact]
    public void User_ToRetrieveDto()
    {
        User expected = new()
        {
            Id = 1,
            Status = UserStatus.Active,
            Username = Guid.NewGuid().ToString(),
            Email = Guid.NewGuid().ToString() + "@synith.com",
            LanguageId = 1,
            Language = new() { Id = 1, Code = "en-US" },
            Roles = [
                new() { RoleId = 1 },
                new() { RoleId = 2 }
            ]
        };

        UserRetrieve actual = expected.ToRetrieveDto();

        actual.Should().BeEquivalentTo(expected,
            opt => opt.Excluding(x => x.Password)
                      .Excluding(x => x.CreatedDate)
                      .Excluding(x => x.LastLogin)
                      .Excluding(x => x.LanguageId)
                      .Excluding(x => x.InactiveDate)
                      .Excluding(x => x.Roles));

        actual.Roles.Count().Should().Be(expected.Roles.Count());
        int index = 0;
        foreach (var role in actual.Roles)
        {
            role.Id.Should().Be(expected.Roles.ElementAt(index).RoleId);
            index++;
        }
    }
}
