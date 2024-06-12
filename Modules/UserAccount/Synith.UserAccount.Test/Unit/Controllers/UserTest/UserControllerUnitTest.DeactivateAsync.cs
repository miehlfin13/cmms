namespace Synith.UserAccount.Test.Unit.Controllers.UserTest;
partial class UserControllerUnitTest
{
    [Fact]
    public void DeactivateAsync_RequiresPermission_UserDeactivate()
    {
        var permissions = TestHelper.GetRequiredPermissions<UserController>(nameof(UserController.DeactivateAsync))!;
        permissions.Length.Should().Be(1);
        permissions[0].PermissionCode.Should().Be(PermissionCode.User.Deactivate);
    }
}
