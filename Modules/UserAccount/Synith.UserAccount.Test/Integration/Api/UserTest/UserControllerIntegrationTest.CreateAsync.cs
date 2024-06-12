using Synith.UserAccount.Domain.DataTransferObjects.User;

namespace Synith.UserAccount.Test.Integration.Api.UserTest;
partial class UserControllerIntegrationTest
{
    [Fact]
    public async Task CreateAsync_Valid_NoError()
    {
        _factory.SetAuth([
            PermissionCode.Role.Add,
            PermissionCode.User.Add]);

        Role role = await _factory.CreateRoleAsync();

        UserCreate user = new()
        {
            Username = Guid.NewGuid().ToString(),
            Password = Guid.NewGuid().ToString(),
            Email = Guid.NewGuid().ToString() + "@synith.com",
            LanguageId = 1,
            Roles = [role.Id]
        };
        string uri = UserEndpoint.Create;

        HttpResponseMessage response = await _client.PostAsync(uri, user.ToStringContent());
        UserRetrieve actual = await response.DeserializeContentAsync<UserRetrieve>();

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        actual.Should().BeEquivalentTo(user,
            opt => opt.Excluding(x => x.Password)
                      .Excluding(x => x.LanguageId)
                      .Excluding(x => x.Roles));
        actual.Language.Id.Should().Be(user.LanguageId);
        actual.Roles.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_Invalid_HasErrors()
    {
        _factory.SetAuth([PermissionCode.User.Add]);

        UserCreate user = new();
        string uri = UserEndpoint.Create;

        HttpResponseMessage response = await _client.PostAsync(uri, user.ToStringContent());
        var errors = await response.DeserializeContentAsync<IEnumerable<ResponseMessage>>();

        List<string> properties =
            [
                nameof(UserCreate.Username),
                nameof(UserCreate.Password),
                nameof(UserCreate.Email),
                nameof(UserCreate.LanguageId).AddSpaces()
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
    public async Task CreateAsync_DuplicateUsername_HasError()
    {
        _factory.SetAuth([PermissionCode.User.Add]);

        UserCreate user = new()
        {
            Username = Guid.NewGuid().ToString(),
            Password = Guid.NewGuid().ToString(),
            Email = Guid.NewGuid().ToString() + "@synith.com",
            LanguageId = 1
        };
        string uri = UserEndpoint.Create;

        await _client.PostAsync(uri, user.ToStringContent());

        user.Email = Guid.NewGuid().ToString() + "@synith.com";
        HttpResponseMessage response = await _client.PostAsync(uri, user.ToStringContent());
        string errorMessage = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errorMessage.Should().Be(ErrorMessageProvider.UniqueKeyViolation(user.Username));
    }

    [Fact]
    public async Task CreateAsync_DuplicateEmail_HasError()
    {
        _factory.SetAuth([PermissionCode.User.Add]);

        UserCreate user = new()
        {
            Username = Guid.NewGuid().ToString(),
            Password = Guid.NewGuid().ToString(),
            Email = Guid.NewGuid().ToString() + "@synith.com",
            LanguageId = 1
        };
        string uri = UserEndpoint.Create;

        await _client.PostAsync(uri, user.ToStringContent());

        user.Username = Guid.NewGuid().ToString();
        HttpResponseMessage response = await _client.PostAsync(uri, user.ToStringContent());
        string errorMessage = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errorMessage.Should().Be(ErrorMessageProvider.UniqueKeyViolation(user.Email));
    }
}
