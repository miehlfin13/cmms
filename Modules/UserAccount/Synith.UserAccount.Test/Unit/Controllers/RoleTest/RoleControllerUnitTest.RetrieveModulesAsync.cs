namespace Synith.UserAccount.Test.Unit.Controllers.RoleTest;
partial class RoleControllerUnitTest
{
    [Fact]
    public void RetrieveModulesAsync_EndpointTemplate()
    {
        var route = TestHelper.GetEndpointTemplate<RoleController>(nameof(RoleController.RetrieveModulesAsync))!;
        route.Length.Should().Be(1);
        route[0].Template.Should().Be("Modules");
        route[0].Should().BeOfType<HttpGetAttribute>();
    }

    [Fact]
    public void RetrieveModulesAsync_RequiresPermission_RoleView()
    {
        var permissions = TestHelper.GetRequiredPermissions<RoleController>(nameof(RoleController.RetrieveModulesAsync))!;
        permissions.Length.Should().Be(1);
        permissions[0].PermissionCode.Should().Be(PermissionCode.Role.View);
    }

    [Fact]
    public async Task RetrieveModulesAsync_NoError_ReturnsOkObjectResult()
    {
        Mock<IRoleService> serviceMock = new();
        Mock<ILogger<RoleController>> loggerMock = new();
        serviceMock.Setup(x => x.RetrieveModulesAsync());

        RoleController controller = new(loggerMock.Object, serviceMock.Object);
        IActionResult result = await controller.RetrieveModulesAsync();

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeAssignableTo<IEnumerable<Module>>();

        serviceMock.Verify(x => x.RetrieveModulesAsync(), Times.Once);
    }

    [Fact]
    public async Task RetrieveModulesAsync_HasError_ReturnsBadRequestResult()
    {
        Exception exception = new($"Error: {nameof(RetrieveModulesAsync_HasError_ReturnsBadRequestResult)}");
        Mock<IRoleService> serviceMock = new();
        Mock<ILogger<RoleController>> loggerMock = new();
        serviceMock.Setup(x => x.RetrieveModulesAsync()).ThrowsAsync(exception);

        RoleController controller = new(loggerMock.Object, serviceMock.Object);
        IActionResult result = await controller.RetrieveModulesAsync();

        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be(exception.Message);

        serviceMock.Verify(x => x.RetrieveModulesAsync(), Times.Once);
    }
}
