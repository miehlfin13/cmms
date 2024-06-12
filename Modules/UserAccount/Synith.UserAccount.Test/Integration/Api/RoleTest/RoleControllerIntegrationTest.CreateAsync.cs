using Synith.UserAccount.Domain.DataTransferObjects.Role;

namespace Synith.UserAccount.Test.Integration.Api.RoleTest;
partial class RoleControllerIntegrationTest
{
    [Fact]
    public async Task CreateAsync_Valid_NoError()
    {
        _factory.SetAuth([PermissionCode.Role.Add]);

        RoleCreate role = new()
        {
            Name = $"Name-{Guid.NewGuid()}"
        };
        string uri = RoleEndpoint.Create;

        HttpResponseMessage response = await _client.PostAsync(uri, role.ToStringContent());
        Role actual = await response.DeserializeContentAsync<Role>();

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        actual.Should().BeEquivalentTo(role);
        actual.Id.Should().NotBe(0);
        actual.Status.Should().Be(GenericStatus.Active);
        actual.InactiveDate.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_Invalid_HasErrors()
    {
        _factory.SetAuth([PermissionCode.Role.Add]);

        RoleCreate role = new();
        string uri = RoleEndpoint.Create;

        HttpResponseMessage response = await _client.PostAsync(uri, role.ToStringContent());
        var errors = await response.DeserializeContentAsync<IEnumerable<ResponseMessage>>();

        List<string> properties =
            [
                nameof(role.Name)
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
    public async Task CreateAsync_DuplicateName_HasError()
    {
        _factory.SetAuth([PermissionCode.Role.Add]);

        RoleCreate role = new()
        {
            Name = $"Name-{Guid.NewGuid()}"
        };
        string uri = RoleEndpoint.Create;

        await _client.PostAsync(uri, role.ToStringContent());

        HttpResponseMessage response = await _client.PostAsync(uri, role.ToStringContent());
        string errorMessage = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errorMessage.Should().Be(ErrorMessageProvider.UniqueKeyViolation(role.Name));
    }
}
