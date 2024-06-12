using Synith.Security.Constants;
using Synith.UserAccount.Domain.Junction;

namespace Synith.UserAccount.Test.Integration.Api.RoleTest;
partial class RoleControllerIntegrationTest
{
    [Fact]
    public async Task RetrieveRolePermissionsAsync_NoRecords_NoError()
    {
        _factory.SetAuth();

        string uri = RoleEndpoint.RetrieveRolePermissions
            .Replace("{roleId}", int.MaxValue.ToString());

        HttpResponseMessage response = await _client.GetAsync(uri);
        IEnumerable<RolePermission> actual = await response.DeserializeContentAsync<IEnumerable<RolePermission>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        actual.Should().BeEmpty();
    }

    [Fact]
    public async Task RetrieveRolePermissionsAsync_HasRecords_NoError()
    {
        _factory.SetAuth([
            PermissionCode.Role.Add,
            PermissionCode.Role.Edit,
            PermissionCode.Role.View]);

        Role role = await _factory.CreateRoleAsync();
        IEnumerable<Permission> permissions = await _factory.RetrievePermissionsAsync();
        string[] permissionCodes = [PermissionCode.Company.View, PermissionCode.User.Add];
        IEnumerable<Permission> rolePermissions = permissions.Where(x => permissionCodes.Contains(x.Code));
        await _factory.UpdateRolePermissionsAsync(role.Id, rolePermissions.Select(x => x.Id));

        string uri = RoleEndpoint.RetrieveRolePermissions.Replace("{roleId}", role.Id.ToString());

        HttpResponseMessage response = await _client.GetAsync(uri);
        IEnumerable<Permission> actual = await response.DeserializeContentAsync<IEnumerable<Permission>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        actual.Should().BeEquivalentTo(rolePermissions);
    }
}
