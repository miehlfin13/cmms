using Synith.Security.Constants;

namespace Synith.UserAccount.Test.Integration.Api.RoleTest;
partial class RoleControllerIntegrationTest
{
    [Fact]
    public async Task RetrievePermissionsAsync_HasDefaultValues()
    {
        _factory.SetAuth([PermissionCode.Role.View]);

        string uri = RoleEndpoint.RetrievePermissions;

        HttpResponseMessage response = await _client.GetAsync(uri);
        IEnumerable<Permission> actual = await response.DeserializeContentAsync<IEnumerable<Permission>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        actual.Should().BeEquivalentTo(Permissions,
            opt => opt.Excluding(x => x.Id).Excluding(x => x.ParentId));

        ShouldHaveChild(actual, PermissionCode.Company.View, CompanyPermissions);
        ShouldHaveChild(actual, PermissionCode.Area.View, AreaPermissions);
        ShouldHaveChild(actual, PermissionCode.Branch.View, BranchPermissions);

        ShouldHaveChild(actual, PermissionCode.Role.View, RolePermissions);
        ShouldHaveChild(actual, PermissionCode.User.View, UserPermissions);

        ShouldHaveChild(actual, PermissionCode.Employee.View, EmployeePermissions);
    }

    private static void ShouldHaveChild(IEnumerable<Permission> actual, string code, IEnumerable<Permission> permissions)
    {
        int parentId = actual.Where(x => x.Code == code).Single().Id;
        actual.Where(x => x.ParentId == parentId).Count().Should().Be(permissions.Count() - 1);
    }

    private List<Permission> Permissions = new();

    private List<Permission> OrganizationPermissions = new();

    private List<Permission> CompanyPermissions =
        [
            new Permission { Code = PermissionCode.Company.View, ModuleId = 1, SortIndex = 1 },
            new Permission { Code = PermissionCode.Company.Add, ModuleId = 1, SortIndex = 2 },
            new Permission { Code = PermissionCode.Company.Edit, ModuleId = 1, SortIndex = 3 },
            new Permission { Code = PermissionCode.Company.Deactivate, ModuleId = 1, SortIndex = 4 }
        ];

    private List<Permission> AreaPermissions =
        [
            new Permission { Code = PermissionCode.Area.View, ModuleId = 1, SortIndex = 11 },
            new Permission { Code = PermissionCode.Area.Add, ModuleId = 1, SortIndex = 12 },
            new Permission { Code = PermissionCode.Area.Edit, ModuleId = 1, SortIndex = 13 },
            new Permission { Code = PermissionCode.Area.Deactivate, ModuleId = 1, SortIndex = 14 }
        ];

    private List<Permission> BranchPermissions = [
        new Permission { Code = PermissionCode.Branch.View, ModuleId = 1, SortIndex = 21 },
        new Permission { Code = PermissionCode.Branch.Add, ModuleId = 1, ParentId = 9, SortIndex = 22 },
        new Permission { Code = PermissionCode.Branch.Edit, ModuleId = 1, ParentId = 9, SortIndex = 23 },
        new Permission { Code = PermissionCode.Branch.Deactivate, ModuleId = 1, ParentId = 9, SortIndex = 24 }
    ];

    private List<Permission> UserAccessPermissions = [];

    private List<Permission> RolePermissions =
        [
            new Permission { Code = PermissionCode.Role.View, ModuleId = 2, SortIndex = 1 },
            new Permission { Code = PermissionCode.Role.Add, ModuleId = 2, SortIndex = 2 },
            new Permission { Code = PermissionCode.Role.Edit, ModuleId = 2, SortIndex = 3 },
            new Permission { Code = PermissionCode.Role.Deactivate, ModuleId = 2, SortIndex = 4 }
        ];

    private List<Permission> UserPermissions =
        [
            new Permission { Code = PermissionCode.User.View, ModuleId = 2, SortIndex = 11 },
            new Permission { Code = PermissionCode.User.Add, ModuleId = 2, SortIndex = 12 },
            new Permission { Code = PermissionCode.User.Edit, ModuleId = 2, SortIndex = 13 },
            new Permission { Code = PermissionCode.User.Deactivate, ModuleId = 2, SortIndex = 14 }
        ];

    private List<Permission> EmployeePermissions =
        [
            new Permission { Code = PermissionCode.Employee.View, ModuleId = 3, SortIndex = 1 },
            new Permission { Code = PermissionCode.Employee.Add, ModuleId = 3, SortIndex = 2 },
            new Permission { Code = PermissionCode.Employee.Edit, ModuleId = 3, SortIndex = 3 },
            new Permission { Code = PermissionCode.Employee.Deactivate, ModuleId = 3, SortIndex = 4 }
        ];
}
