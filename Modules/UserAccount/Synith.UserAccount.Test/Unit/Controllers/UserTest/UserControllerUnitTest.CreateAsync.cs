using Synith.Security.Interfaces;
using Synith.UserAccount.Domain.DataTransferObjects.User;

namespace Synith.UserAccount.Test.Unit.Controllers.UserTest;
public partial class UserControllerUnitTest
{
    [Fact]
    public void CreateAsync_EndpointTemplate()
    {
        var route = TestHelper.GetEndpointTemplate<UserController>(nameof(UserController.CreateAsync))!;
        route.Length.Should().Be(1);
        route[0].Template.Should().Be(null);
        route[0].Should().BeOfType<HttpPostAttribute>();
    }

    [Fact]
    public void CreateAsync_RequiresPermission_UserAdd()
    {
        var permissions = TestHelper.GetRequiredPermissions<UserController>(nameof(UserController.CreateAsync))!;
        permissions.Length.Should().Be(1);
        permissions[0].PermissionCode.Should().Be(PermissionCode.User.Add);
    }

    [Fact]
    public async Task CreateAsync_NoError_ReturnsCreateObjectResult()
    {
        UserRetrieve user = new();
        Mock<IUserService> serviceMock = new();
        Mock<ILogger<UserController>> loggerMock = new();
        Mock<ITokenService> tokenMock = new();
        Mock<ICryptographyService> cryptographyMock = new();
        serviceMock.Setup(x => x.CreateAsync(It.IsAny<UserCreate>())).ReturnsAsync(user);

        UserController controller = new(loggerMock.Object, serviceMock.Object, cryptographyMock.Object, tokenMock.Object);
        IActionResult result = await controller.CreateAsync(new UserCreate());

        result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status201Created);
        result.As<ObjectResult>().Value.Should().Be(user);

        serviceMock.Verify(x => x.CreateAsync(It.IsAny<UserCreate>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_HasError_ReturnsBadRequestResult()
    {
        Exception exception = new($"Error: {nameof(CreateAsync_HasError_ReturnsBadRequestResult)}");
        Mock<IUserService> serviceMock = new();
        Mock<ILogger<UserController>> loggerMock = new();
        Mock<ITokenService> tokenMock = new();
        Mock<ICryptographyService> cryptographyMock = new();
        serviceMock.Setup(x => x.CreateAsync(It.IsAny<UserCreate>())).ThrowsAsync(exception);

        UserController controller = new(loggerMock.Object, serviceMock.Object, cryptographyMock.Object, tokenMock.Object);
        IActionResult result = await controller.CreateAsync(new UserCreate());

        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be(exception.Message);

        serviceMock.Verify(x => x.CreateAsync(It.IsAny<UserCreate>()), Times.Once);
    }
}
