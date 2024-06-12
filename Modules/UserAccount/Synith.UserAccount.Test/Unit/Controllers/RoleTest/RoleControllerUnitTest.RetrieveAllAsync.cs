namespace Synith.UserAccount.Test.Unit.Controllers.RoleTest;
partial class RoleControllerUnitTest
{
    [Fact]
    public void RetrieveAllAsync_RequiresPermission_RoleView()
    {
        var permissions = TestHelper.GetRequiredPermissions<RoleController>(nameof(RoleController.RetrieveAllAsync))!;
        permissions.Length.Should().Be(1);
        permissions[0].PermissionCode.Should().Be(PermissionCode.Role.View);
    }
}
