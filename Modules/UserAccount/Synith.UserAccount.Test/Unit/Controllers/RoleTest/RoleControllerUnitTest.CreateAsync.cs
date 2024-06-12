using Synith.UserAccount.Domain.DataTransferObjects.Role;

namespace Synith.UserAccount.Test.Unit.Controllers.RoleTest;
public partial class RoleControllerUnitTest
{
    [Fact]
    public void CreateAsync_EndpointTemplate()
    {
        var route = TestHelper.GetEndpointTemplate<RoleController>(nameof(RoleController.CreateAsync))!;

        route.Length.Should().Be(1);
        route[0].Template.Should().Be(null);
        route[0].Should().BeOfType<HttpPostAttribute>();
    }

    [Fact]
    public void CreateAsync_RequiresPermission_RoleAdd()
    {
        var permissions = TestHelper.GetRequiredPermissions<RoleController>(nameof(RoleController.CreateAsync))!;
        permissions.Length.Should().Be(1);
        permissions[0].PermissionCode.Should().Be(PermissionCode.Role.Add);
    }

    [Fact]
    public async Task CreateAsync_NoError_ReturnsCreateObjectResult()
    {
        Role role = new();
        Mock<IRoleService> serviceMock = new();
        Mock<ILogger<RoleController>> loggerMock = new();
        serviceMock.Setup(x => x.CreateAsync(It.IsAny<RoleCreate>())).ReturnsAsync(role);

        RoleController controller = new(loggerMock.Object, serviceMock.Object);
        IActionResult result = await controller.CreateAsync(new RoleCreate());

        result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status201Created);
        result.As<ObjectResult>().Value.Should().Be(role);

        serviceMock.Verify(x => x.CreateAsync(It.IsAny<RoleCreate>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_HasError_ReturnsBadRequestResult()
    {
        Exception exception = new($"Error: {nameof(CreateAsync_HasError_ReturnsBadRequestResult)}");
        Mock<IRoleService> serviceMock = new();
        Mock<ILogger<RoleController>> loggerMock = new();
        serviceMock.Setup(x => x.CreateAsync(It.IsAny<RoleCreate>())).ThrowsAsync(exception);

        RoleController controller = new(loggerMock.Object, serviceMock.Object);
        IActionResult result = await controller.CreateAsync(new RoleCreate());

        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be(exception.Message);

        serviceMock.Verify(x => x.CreateAsync(It.IsAny<RoleCreate>()), Times.Once);
    }
}
