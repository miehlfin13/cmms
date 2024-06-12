namespace Synith.UserAccount.Test.Integration.Api.UserTest;

[Collection(nameof(UserAccountApiFactory))]
public partial class UserControllerIntegrationTest
{
    private readonly UserAccountApiFactory _factory;
    private readonly HttpClient _client;

    public static class UserEndpoint
    {
        public const string Create = "UserAccount/User";
        public const string RetrieveAll = "UserAccount/User";
        public const string RetrieveById = "UserAccount/User/{id}";
        public const string Update = "UserAccount/User";
        public const string Delete = "UserAccount/User/{id}";

        public const string RetrieveLanguages = "UserAccount/User/Languages";
        public const string RetrieveUserRoles = "UserAccount/User/Roles/{userId}";
        public const string UpdateUserRolePermissions = "UserAccount/User/Roles/{userId}";
    }

    public UserControllerIntegrationTest(UserAccountApiFactory factory)
    {
        _factory = factory;
        _client = factory.Client;
    }
}
