namespace Synith.UserAccount.Test.Integration.Api.RoleTest;

[Collection(nameof(UserAccountApiFactory))]
public partial class RoleControllerIntegrationTest
{
    private readonly UserAccountApiFactory _factory;
    private readonly HttpClient _client;

    public static class RoleEndpoint
    {
        public const string Create = "UserAccount/Role";
        public const string RetrieveAll = "UserAccount/Role";
        public const string RetrieveById = "UserAccount/Role/{id}";
        public const string Update = "UserAccount/Role";
        public const string Delete = "UserAccount/Role/{id}";

        public const string RetrieveModules = "UserAccount/Role/Modules";
        public const string RetrievePermissions = "UserAccount/Role/Permissions";
        public const string RetrieveRolePermissions = "UserAccount/Role/Permissions/{roleId}";
        public const string UpdateRolePermissions = "UserAccount/Role/Permissions/{roleId}";

    }

    public RoleControllerIntegrationTest(UserAccountApiFactory factory)
    {
        _factory = factory;
        _client = factory.Client;

        OrganizationPermissions.AddRange(CompanyPermissions);
        OrganizationPermissions.AddRange(AreaPermissions);
        OrganizationPermissions.AddRange(BranchPermissions);

        UserAccessPermissions.AddRange(RolePermissions);
        UserAccessPermissions.AddRange(UserPermissions);

        Permissions.AddRange(OrganizationPermissions);
        Permissions.AddRange(UserAccessPermissions);
        Permissions.AddRange(EmployeePermissions);
    }
}
