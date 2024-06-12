namespace Synith.UserAccount.Test.Unit.Controllers.RoleTest;
partial class RoleControllerUnitTest
{
    [Fact]
    public void RetrievePermissionsAsync_EndpointTemplate()
    {
        var route = TestHelper.GetEndpointTemplate<RoleController>(nameof(RoleController.RetrievePermissionsAsync))!;
        route.Length.Should().Be(1);
        route[0].Template.Should().Be("Permissions");
        route[0].Should().BeOfType<HttpGetAttribute>();
    }

    [Fact]
    public void RetrievePermissionsAsync_RequiresPermission_RoleView()
    {
        var permissions = TestHelper.GetRequiredPermissions<RoleController>(nameof(RoleController.RetrievePermissionsAsync))!;
        permissions.Length.Should().Be(1);
        permissions[0].PermissionCode.Should().Be(PermissionCode.Role.View);
    }

    [Fact]
    public async Task RetrievePermissionsAsync_NoError_ReturnsOkObjectResult()
    {
        Mock<IRoleService> serviceMock = new();
        Mock<ILogger<RoleController>> loggerMock = new();
        serviceMock.Setup(x => x.RetrievePermissionsAsync());

        RoleController controller = new(loggerMock.Object, serviceMock.Object);
        IActionResult result = await controller.RetrievePermissionsAsync();

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeAssignableTo<IEnumerable<Permission>>();

        serviceMock.Verify(x => x.RetrievePermissionsAsync(), Times.Once);
    }

    [Fact]
    public async Task RetrievePermissionsAsync_HasError_ReturnsBadRequestResult()
    {
        Exception exception = new($"Error: {nameof(RetrievePermissionsAsync_HasError_ReturnsBadRequestResult)}");
        Mock<IRoleService> serviceMock = new();
        Mock<ILogger<RoleController>> loggerMock = new();
        serviceMock.Setup(x => x.RetrievePermissionsAsync()).ThrowsAsync(exception);

        RoleController controller = new(loggerMock.Object, serviceMock.Object);
        IActionResult result = await controller.RetrievePermissionsAsync();

        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be(exception.Message);

        serviceMock.Verify(x => x.RetrievePermissionsAsync(), Times.Once);
    }
}
