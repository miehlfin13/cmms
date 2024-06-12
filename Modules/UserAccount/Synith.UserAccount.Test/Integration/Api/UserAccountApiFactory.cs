using Synith.UserAccount.Domain.DataTransferObjects.Role;
using Synith.UserAccount.Domain.DataTransferObjects.User;
using Synith.UserAccount.Infrastructure.Persistence;
using static Synith.UserAccount.Test.Integration.Api.RoleTest.RoleControllerIntegrationTest;
using static Synith.UserAccount.Test.Integration.Api.UserTest.UserControllerIntegrationTest;

namespace Synith.UserAccount.Test.Integration.Api;
public class UserAccountApiFactory : EntityApiFactory<SynithCMMSEntryPoint, UserAccountDbContext>
{
    public async Task<Role> CreateRoleAsync()
    {
        RoleCreate role = new()
        {
            Name = $"Name-{Guid.NewGuid()}"
        };

        string uri = RoleEndpoint.Create;
        HttpResponseMessage response = await Client.PostAsync(uri, role.ToStringContent());
        return await response.DeserializeContentAsync<Role>();
    }

    public async Task<IEnumerable<Permission>> RetrievePermissionsAsync()
    {
        string uri = RoleEndpoint.RetrievePermissions;
        HttpResponseMessage response = await Client.GetAsync(uri);
        return await response.DeserializeContentAsync<IEnumerable<Permission>>();
    }

    public async Task<IEnumerable<Permission>> RetrieveRolePermissionsAsync(int roleId)
    {
        string uri = RoleEndpoint.RetrieveRolePermissions.Replace("{roleId}", roleId.ToString());
        HttpResponseMessage response = await Client.GetAsync(uri);
        return await response.DeserializeContentAsync<IEnumerable<Permission>>();
    }

    public async Task<HttpResponseMessage> UpdateRolePermissionsAsync(int roleId, IEnumerable<int> permissions)
    {
        string uri = RoleEndpoint.UpdateRolePermissions.Replace("{roleId}", roleId.ToString());
        return await Client.PatchAsync(uri, permissions.ToStringContent());
    }

    public async Task<IEnumerable<UserRoleRetrieve>> RetrieveUserRolesAsync(int userId)
    {
        string uri = UserEndpoint.RetrieveUserRoles.Replace("{userId}", userId.ToString());
        HttpResponseMessage response = await Client.GetAsync(uri);
        return await response.DeserializeContentAsync<IEnumerable<UserRoleRetrieve>>();
    }

    public async Task<HttpResponseMessage> UpdateUserRolesAsync(int userId, IEnumerable<int> roleIds)
    {
        string uri = UserEndpoint.UpdateUserRolePermissions.Replace("{userId}", userId.ToString());
        return await Client.PatchAsync(uri, roleIds.ToStringContent());
    }

    public async Task<UserRetrieve> CreateUserAsync(int roleId = 0)
    {
        return await CreateUserAsync(roleId < 1 ? [] : [roleId]);
    }

    public async Task<UserRetrieve> CreateUserAsync(IEnumerable<int> roleIds)
    {
        UserCreate user = new()
        {
            Username = Guid.NewGuid().ToString(),
            Password = Guid.NewGuid().ToString(),
            Email = Guid.NewGuid().ToString() + "@synith.com",
            LanguageId = 1,
            Roles = roleIds
        };

        string uri = UserEndpoint.Create;
        HttpResponseMessage response = await Client.PostAsync(uri, user.ToStringContent());
        return await response.DeserializeContentAsync<UserRetrieve>();
    }
}
