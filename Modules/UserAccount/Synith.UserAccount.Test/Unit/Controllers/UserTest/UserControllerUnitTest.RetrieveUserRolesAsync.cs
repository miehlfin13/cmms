using Synith.Security.Interfaces;
using Synith.UserAccount.Domain.DataTransferObjects.User;

namespace Synith.UserAccount.Test.Unit.Controllers.UserTest;
partial class UserControllerUnitTest
{
    [Fact]
    public void RetrieveUserRolesAsync_EndpointTemplate()
    {
        var route = TestHelper.GetEndpointTemplate<UserController>(nameof(UserController.RetrieveUserRolesAsync))!;
        route.Length.Should().Be(1);
        route[0].Template.Should().Be("Roles/{userId}");
        route[0].Should().BeOfType<HttpGetAttribute>();
    }

    [Fact]
    public void RetrieveUserRolesAsync_RequiresPermission_UserView()
    {
        var permissions = TestHelper.GetRequiredPermissions<UserController>(nameof(UserController.RetrieveUserRolesAsync))!;
        permissions.Length.Should().Be(1);
        permissions[0].PermissionCode.Should().Be(PermissionCode.User.View);
    }

    [Fact]
    public async Task RetrieveUserRolesAsync_NoError_ReturnsOkObjectResult()
    {
        Mock<IUserService> serviceMock = new();
        Mock<ILogger<UserController>> loggerMock = new();
        Mock<ITokenService> tokenMock = new();
        Mock<ICryptographyService> cryptographyMock = new();
        serviceMock.Setup(x => x.RetrieveUserRolesAsync(It.IsAny<int>()));

        UserController controller = new(loggerMock.Object, serviceMock.Object, cryptographyMock.Object, tokenMock.Object);
        IActionResult result = await controller.RetrieveUserRolesAsync(It.IsAny<int>());

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeAssignableTo<IEnumerable<UserRoleRetrieve>>();

        serviceMock.Verify(x => x.RetrieveUserRolesAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task RetrieveUserRolesAsync_HasError_ReturnsBadRequestResult()
    {
        Exception exception = new($"Error: {nameof(RetrieveUserRolesAsync_HasError_ReturnsBadRequestResult)}");
        Mock<IUserService> serviceMock = new();
        Mock<ILogger<UserController>> loggerMock = new();
        Mock<ITokenService> tokenMock = new();
        Mock<ICryptographyService> cryptographyMock = new();
        serviceMock.Setup(x => x.RetrieveUserRolesAsync(It.IsAny<int>())).ThrowsAsync(exception);

        UserController controller = new(loggerMock.Object, serviceMock.Object, cryptographyMock.Object, tokenMock.Object);
        IActionResult result = await controller.RetrieveUserRolesAsync(It.IsAny<int>());

        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be(exception.Message);

        serviceMock.Verify(x => x.RetrieveUserRolesAsync(It.IsAny<int>()), Times.Once);
    }
}
