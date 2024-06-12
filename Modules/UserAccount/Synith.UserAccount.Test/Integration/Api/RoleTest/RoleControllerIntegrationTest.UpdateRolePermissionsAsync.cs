using Synith.Security.Constants;

namespace Synith.UserAccount.Test.Integration.Api.RoleTest;
partial class RoleControllerIntegrationTest
{
    [Fact]
    public async Task UpdateRolePermissionsAsync_HasNewPermissions_AddRemove()
    {
        _factory.SetAuth([
            PermissionCode.Role.Add,
            PermissionCode.Role.Edit,
            PermissionCode.Role.View]);

        Role role = await _factory.CreateRoleAsync();
        IEnumerable<Permission> permissions = await _factory.RetrievePermissionsAsync();
        
        // initial permissions
        string[] permissionCodes = [PermissionCode.Company.View, PermissionCode.User.Add];
        IEnumerable<Permission> rolePermissions = permissions.Where(x => permissionCodes.Contains(x.Code));
        await _factory.UpdateRolePermissionsAsync(role.Id, rolePermissions.Select(x => x.Id));

        // new permissions
        permissionCodes = [PermissionCode.Company.View, PermissionCode.Area.View, PermissionCode.Branch.View];
        rolePermissions = permissions.Where(x => permissionCodes.Contains(x.Code));
        HttpResponseMessage response = await _factory.UpdateRolePermissionsAsync(role.Id, rolePermissions.Select(x => x.Id));
        IEnumerable<Permission> actual = await _factory.RetrieveRolePermissionsAsync(role.Id);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        actual.Should().BeEquivalentTo(rolePermissions);
    }

    [Fact]
    public async Task UpdateRolePermissionsAsync_HasNoNewPermissions_Remove()
    {
        _factory.SetAuth([
            PermissionCode.Role.Add,
            PermissionCode.Role.Edit,
            PermissionCode.Role.View]);

        Role role = await _factory.CreateRoleAsync();
        IEnumerable<Permission> permissions = await _factory.RetrievePermissionsAsync();

        // initial permissions
        string[] permissionCodes = [PermissionCode.Company.View, PermissionCode.User.Add];
        IEnumerable<Permission> rolePermissions = permissions.Where(x => permissionCodes.Contains(x.Code));
        await _factory.UpdateRolePermissionsAsync(role.Id, rolePermissions.Select(x => x.Id));

        // new permissions
        HttpResponseMessage response = await _factory.UpdateRolePermissionsAsync(role.Id, []);
        IEnumerable<Permission> actual = await _factory.RetrieveRolePermissionsAsync(role.Id);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        actual.Should().BeEmpty();
    }
}
