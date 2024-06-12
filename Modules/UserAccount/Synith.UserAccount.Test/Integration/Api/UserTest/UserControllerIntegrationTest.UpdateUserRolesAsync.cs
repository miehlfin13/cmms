using Synith.UserAccount.Domain.DataTransferObjects.User;

namespace Synith.UserAccount.Test.Integration.Api.UserTest;
partial class UserControllerIntegrationTest
{
    [Fact]
    public async Task UpdateUserRolesAsync_HasNewRoles_AddRemove()
    {
        _factory.SetAuth([
            PermissionCode.Role.Add,
            PermissionCode.User.Add,
            PermissionCode.User.Edit,
            PermissionCode.User.View]);

        List<Role> roles =
            [
                await _factory.CreateRoleAsync(),
                await _factory.CreateRoleAsync(),
                await _factory.CreateRoleAsync()
            ];

        // initial roles
        int[] userRoles = [roles[0].Id, roles[1].Id];
        UserRetrieve user = await _factory.CreateUserAsync(userRoles);

        // new user roles
        userRoles = [roles[1].Id, roles[2].Id];
        HttpResponseMessage response =  await _factory.UpdateUserRolesAsync(user.Id, userRoles);
        IEnumerable<UserRoleRetrieve> actual = await _factory.RetrieveUserRolesAsync(user.Id);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        actual.Select(x => x.Id).Should().BeEquivalentTo(userRoles);
    }

    [Fact]
    public async Task UpdateUserRolesAsync_HasNoNewRoles_Remove()
    {
        _factory.SetAuth([
            PermissionCode.Role.Add,
            PermissionCode.User.Add,
            PermissionCode.User.Edit,
            PermissionCode.User.View]);

        List<Role> roles =
            [
                await _factory.CreateRoleAsync(),
                await _factory.CreateRoleAsync(),
                await _factory.CreateRoleAsync()
            ];

        // initial roles
        int[] userRoles = [roles[0].Id, roles[1].Id];
        UserRetrieve user = await _factory.CreateUserAsync(userRoles);

        // new user roles
        userRoles = [];
        HttpResponseMessage response = await _factory.UpdateUserRolesAsync(user.Id, userRoles);
        IEnumerable<UserRoleRetrieve> actual = await _factory.RetrieveUserRolesAsync(user.Id);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        actual.Should().BeEmpty();
    }
}
