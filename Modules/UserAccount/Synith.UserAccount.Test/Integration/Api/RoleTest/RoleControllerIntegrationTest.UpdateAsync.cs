using Synith.UserAccount.Domain.DataTransferObjects.Role;

namespace Synith.UserAccount.Test.Integration.Api.RoleTest;
partial class RoleControllerIntegrationTest
{
    [Fact]
    public async Task UpdateAsync_Valid_NoError()
    {
        _factory.SetAuth([PermissionCode.Role.Add, PermissionCode.Role.Edit]);

        Role first = await _factory.CreateRoleAsync();
        RoleUpdate role = new()
        {
            Id = first.Id,
            Name = $"Name-{Guid.NewGuid()}"
        };
        string uri = RoleEndpoint.Update;

        HttpResponseMessage response = await _client.PutAsync(uri, role.ToStringContent());
        Role actual = await response.DeserializeContentAsync<Role>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        actual.Should().BeEquivalentTo(role);
    }

    [Fact]
    public async Task UpdateAsync_Invalid_HasErrors()
    {
        _factory.SetAuth([PermissionCode.Role.Add, PermissionCode.Role.Edit]);

        RoleUpdate role = new();
        string uri = RoleEndpoint.Update;

        HttpResponseMessage response = await _client.PutAsync(uri, role.ToStringContent());
        var errors = await response.DeserializeContentAsync<IEnumerable<ResponseMessage>>();

        List<string> properties =
            [
                nameof(Role.Name),
                nameof(Entity.Id)
            ];

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().HaveCount(properties.Count);

        foreach (var error in errors)
        {
            var parameters = error.ParametersJson.Deserialize<IDictionary<string, object>>();
            string item = parameters["item"].ToString()!;
            item.Should().BeOneOf(properties);
            properties.Remove(item);
        }

        properties.Should().HaveCount(0);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingId_HasError()
    {
        _factory.SetAuth([PermissionCode.Role.Add, PermissionCode.Role.Edit]);

        RoleUpdate role = new()
        {
            Id = int.MaxValue,
            Name = $"Name-{Guid.NewGuid()}"
        };
        string uri = RoleEndpoint.Update;

        HttpResponseMessage response = await _client.PutAsync(uri, role.ToStringContent());
        string error = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Should().Be(ErrorMessageProvider.NotFound($"{nameof(Role)}.{nameof(Role.Id)}", role.Id.ToString()));
    }

    [Fact]
    public async Task UpdateAsync_DuplicateName_HasError()
    {
        _factory.SetAuth([PermissionCode.Role.Add, PermissionCode.Role.Edit]);

        Role firstRole = await _factory.CreateRoleAsync();
        Role secondRole = await _factory.CreateRoleAsync();
        RoleUpdate role = new()
        {
            Id = firstRole.Id,
            Name = secondRole.Name
        };
        string uri = RoleEndpoint.Update;

        HttpResponseMessage response = await _client.PutAsync(uri, role.ToStringContent());
        string errorMessage = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errorMessage.Should().Be(ErrorMessageProvider.UniqueKeyViolation(role.Name));
    }
}
