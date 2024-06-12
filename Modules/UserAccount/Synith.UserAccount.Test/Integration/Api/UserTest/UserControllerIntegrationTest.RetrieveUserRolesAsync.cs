using Synith.UserAccount.Domain.DataTransferObjects.User;

namespace Synith.UserAccount.Test.Integration.Api.UserTest;
partial class UserControllerIntegrationTest
{
    [Fact]
    public async Task RetrieveUserRolesAsync_NoRecords_NoError()
    {
        _factory.SetAuth([PermissionCode.User.View]);

        string uri = UserEndpoint.RetrieveUserRoles.Replace("{userId}", int.MaxValue.ToString());

        HttpResponseMessage response = await _client.GetAsync(uri);
        IEnumerable<UserRoleRetrieve> actual = await response.DeserializeContentAsync<IEnumerable<UserRoleRetrieve>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        actual.Should().BeEmpty();
    }

    [Fact]
    public async Task RetrieveUserRolesAsync_HasRecords_NoError()
    {
        _factory.SetAuth([
            PermissionCode.Role.Add,
            PermissionCode.User.Add,
            PermissionCode.User.View]);

        List<Role> roles =
            [
                await _factory.CreateRoleAsync(),
                await _factory.CreateRoleAsync()
            ];
        UserRetrieve user = await _factory.CreateUserAsync(roles.Select(x => x.Id));

        string uri = UserEndpoint.RetrieveUserRoles.Replace("{userId}", user.Id.ToString());

        HttpResponseMessage response = await _client.GetAsync(uri);
        IEnumerable<UserRoleRetrieve> actual = await response.DeserializeContentAsync<IEnumerable<UserRoleRetrieve>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        roles.Should().BeEquivalentTo(actual);
    }
}
