using Synith.UserAccount.Domain.DataTransferObjects.User;

namespace Synith.UserAccount.Test.Integration.Api.UserTest;
partial class UserControllerIntegrationTest
{
    [Fact]
    public async Task UpdateAsync_Valid_NoError()
    {
        _factory.SetAuth([
            PermissionCode.Role.Add,
            PermissionCode.User.Add,
            PermissionCode.User.Edit]);

        Role role = await _factory.CreateRoleAsync();
        UserRetrieve user = await _factory.CreateUserAsync(role.Id);

        UserUpdate expected = new()
        {
            Id = user.Id,
            Username = Guid.NewGuid().ToString(),
            Email = Guid.NewGuid().ToString() + "@synith.com",
            LanguageId = 1
        };
        string uri = UserEndpoint.Update;

        HttpResponseMessage response = await _client.PutAsync(uri, expected.ToStringContent());
        UserRetrieve actual = await response.DeserializeContentAsync<UserRetrieve>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        actual.Should().BeEquivalentTo(expected,
            opt => opt.Excluding(x => x.LanguageId)
                      .Excluding(x => x.Roles));
        actual.Language.Id.Should().Be(user.Language.Id);
        actual.Roles.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_Invalid_HasErrors()
    {
        _factory.SetAuth([PermissionCode.User.Add, PermissionCode.User.Edit]);

        UserRetrieve user = await _factory.CreateUserAsync();

        UserUpdate expected = new();
        string uri = UserEndpoint.Create;

        HttpResponseMessage response = await _client.PutAsync(uri, expected.ToStringContent());
        var errors = await response.DeserializeContentAsync<IEnumerable<ResponseMessage>>();

        List<string> properties =
            [
                nameof(UserUpdate.Username),
                nameof(UserUpdate.Email),
                nameof(UserUpdate.LanguageId).AddSpaces(),
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
        _factory.SetAuth([PermissionCode.User.Edit]);

        UserUpdate user = new()
        {
            Id = int.MaxValue,
            Username = Guid.NewGuid().ToString(),
            Email = Guid.NewGuid().ToString() + "@synith.com",
            LanguageId = 1
        };
        string uri = UserEndpoint.Update;

        HttpResponseMessage response = await _client.PutAsync(uri, user.ToStringContent());
        string error = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Should().Be(ErrorMessageProvider.NotFound($"{nameof(User)}.{nameof(User.Id)}", user.Id.ToString()));
    }

    [Fact]
    public async Task UpdateAsync_DuplicateUsername_HasError()
    {
        _factory.SetAuth([PermissionCode.User.Add, PermissionCode.User.Edit]);

        UserRetrieve first = await _factory.CreateUserAsync();
        UserRetrieve second = await _factory.CreateUserAsync();

        UserUpdate user = new()
        {
            Id = first.Id,
            Username = second.Username,
            Email = first.Email,
            LanguageId = first.Language.Id
        };
        string uri = UserEndpoint.Update;

        HttpResponseMessage response = await _client.PutAsync(uri, user.ToStringContent());
        string errorMessage = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errorMessage.Should().Be(ErrorMessageProvider.UniqueKeyViolation(user.Username));
    }

    [Fact]
    public async Task UpdateAsync_DuplicateEmail_HasError()
    {
        _factory.SetAuth([PermissionCode.User.Add, PermissionCode.User.Edit]);

        UserRetrieve first = await _factory.CreateUserAsync();
        UserRetrieve second = await _factory.CreateUserAsync();

        UserUpdate user = new()
        {
            Id = first.Id,
            Username = first.Username,
            Email = second.Email,
            LanguageId = first.Language.Id
        };
        string uri = UserEndpoint.Update;

        HttpResponseMessage response = await _client.PutAsync(uri, user.ToStringContent());
        string errorMessage = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errorMessage.Should().Be(ErrorMessageProvider.UniqueKeyViolation(user.Email));
    }
}
