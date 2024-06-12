namespace Synith.UserAccount.Test.Unit.Controllers.RoleTest;
partial class RoleControllerUnitTest
{
    [Fact]
    public void DeactivateAsync_RequiresPermission_RoleDeactivate()
    {
        var permissions = TestHelper.GetRequiredPermissions<RoleController>(nameof(RoleController.DeactivateAsync))!;
        permissions.Length.Should().Be(1);
        permissions[0].PermissionCode.Should().Be(PermissionCode.Role.Deactivate);
    }
}
