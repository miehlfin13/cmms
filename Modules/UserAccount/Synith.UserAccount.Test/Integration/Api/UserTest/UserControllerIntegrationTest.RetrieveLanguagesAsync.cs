namespace Synith.UserAccount.Test.Integration.Api.UserTest;
partial class UserControllerIntegrationTest
{
    [Fact]
    public async Task RetrieveLanguagesAsync_HasDefaultValues()
    {
        _factory.SetAuth([PermissionCode.User.View]);

        Language expected = new() { Code = "US" };
        string uri = UserEndpoint.RetrieveLanguages;

        HttpResponseMessage response = await _client.GetAsync(uri);
        IEnumerable<Language> actual = await response.DeserializeContentAsync<IEnumerable<Language>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        actual.Should().ContainEquivalentOf(expected,
            options => options.Excluding(x => x.Id)
                              .Excluding(x => x.Name)
                              .Excluding(x => x.LocaleName));
    }
}
