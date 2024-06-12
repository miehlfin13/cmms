using Synith.Security.Interfaces;
using Synith.UserAccount.Domain.DataTransferObjects.User;

namespace Synith.UserAccount.Test.Unit.Controllers.UserTest;
partial class UserControllerUnitTest
{
    [Fact]
    public void UpdateAsync_EndpointTemplate()
    {
        var route = TestHelper.GetEndpointTemplate<UserController>(nameof(UserController.UpdateAsync))!;
        route.Length.Should().Be(1);
        route[0].Template.Should().Be(null);
        route[0].Should().BeOfType<HttpPutAttribute>();
    }

    [Fact]
    public void UpdateAsync_RequiresPermission_UserEdit()
    {
        var permissions = TestHelper.GetRequiredPermissions<UserController>(nameof(UserController.UpdateAsync))!;
        permissions.Length.Should().Be(1);
        permissions[0].PermissionCode.Should().Be(PermissionCode.User.Edit);
    }

    [Fact]
    public async Task UpdateAsync_NoError_ReturnsCreateObjectResult()
    {
        UserRetrieve user = new();
        Mock<IUserService> serviceMock = new();
        Mock<ILogger<UserController>> loggerMock = new();
        Mock<ITokenService> tokenMock = new();
        Mock<ICryptographyService> cryptographyMock = new();
        serviceMock.Setup(x => x.UpdateAsync(It.IsAny<UserUpdate>())).ReturnsAsync(user);

        UserController controller = new(loggerMock.Object, serviceMock.Object, cryptographyMock.Object, tokenMock.Object);
        IActionResult result = await controller.UpdateAsync(new UserUpdate());

        result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.As<OkObjectResult>().Value.Should().Be(user);

        serviceMock.Verify(x => x.UpdateAsync(It.IsAny<UserUpdate>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_HasError_ReturnsBadRequestResult()
    {
        Exception exception = new($"Error: {nameof(UpdateAsync_HasError_ReturnsBadRequestResult)}");
        Mock<IUserService> serviceMock = new();
        Mock<ILogger<UserController>> loggerMock = new();
        Mock<ITokenService> tokenMock = new();
        Mock<ICryptographyService> cryptographyMock = new();
        serviceMock.Setup(x => x.UpdateAsync(It.IsAny<UserUpdate>())).ThrowsAsync(exception);

        UserController controller = new(loggerMock.Object, serviceMock.Object, cryptographyMock.Object, tokenMock.Object);
        IActionResult result = await controller.UpdateAsync(new UserUpdate());

        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be(exception.Message);

        serviceMock.Verify(x => x.UpdateAsync(It.IsAny<UserUpdate>()), Times.Once);
    }
}
