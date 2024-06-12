namespace Synith.UserAccount.Test.Unit.Controllers.UserTest;
partial class UserControllerUnitTest
{
    [Fact]
    public void RetrieveByIdAsync_RequiresPermission_UserView()
    {
        var permissions = TestHelper.GetRequiredPermissions<UserController>(nameof(UserController.RetrieveByIdAsync))!;
        permissions.Length.Should().Be(1);
        permissions[0].PermissionCode.Should().Be(PermissionCode.User.View);
    }
}
