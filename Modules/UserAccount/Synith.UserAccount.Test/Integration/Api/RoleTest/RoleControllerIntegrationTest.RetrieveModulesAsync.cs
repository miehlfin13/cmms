namespace Synith.UserAccount.Test.Integration.Api.RoleTest;
partial class RoleControllerIntegrationTest
{
    [Fact]
    public async Task RetrieveModules_HasDefaultValues()
    {
        _factory.SetAuth([PermissionCode.Role.View]);

        List<Module> modules =
            [
                new Module { Id = 1, Code = "ORGANIZATION" },
                new Module { Id = 2, Code = "USERACCESS" },
                new Module { Id = 3, Code = "EMPLOYEE" }
            ];
        string uri = RoleEndpoint.RetrieveModules;

        HttpResponseMessage response = await _client.GetAsync(uri);
        IEnumerable<Module> actual = await response.DeserializeContentAsync<IEnumerable<Module>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        actual.Should().BeEquivalentTo(modules);
    }
}
