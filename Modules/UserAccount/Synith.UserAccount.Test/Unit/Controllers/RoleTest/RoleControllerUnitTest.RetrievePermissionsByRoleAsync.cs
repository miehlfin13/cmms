namespace Synith.UserAccount.Test.Unit.Controllers.RoleTest;
partial class RoleControllerUnitTest
{
    [Fact]
    public void RetrievePermissionsByRoleAsync_EndpointTemplate()
    {
        var route = TestHelper.GetEndpointTemplate<RoleController>(nameof(RoleController.RetrieveRolePermissionsAsync))!;
        route.Length.Should().Be(1);
        route[0].Template.Should().Be("Permissions/{roleId}");
        route[0].Should().BeOfType<HttpGetAttribute>();
    }

    [Fact]
    public void RetrieveRolePermissionsAsync_NoRequiredPermission()
    {
        var permissions = TestHelper.GetRequiredPermissions<RoleController>(nameof(RoleController.RetrieveRolePermissionsAsync))!;
        permissions.Length.Should().Be(0);
    }

    [Fact]
    public async Task RetrievePermissionsByRoleAsync_NoError_ReturnsOkObjectResult()
    {
        Mock<IRoleService> serviceMock = new();
        Mock<ILogger<RoleController>> loggerMock = new();
        serviceMock.Setup(x => x.RetrieveRolePermissionsAsync(It.IsAny<int>()));

        RoleController controller = new(loggerMock.Object, serviceMock.Object);
        IActionResult result = await controller.RetrieveRolePermissionsAsync(It.IsAny<int>());

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeAssignableTo<IEnumerable<Permission>>();

        serviceMock.Verify(x => x.RetrieveRolePermissionsAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task RetrievePermissionsByRoleAsync_HasError_ReturnsBadRequestResult()
    {
        Exception exception = new($"Error: {nameof(RetrievePermissionsByRoleAsync_HasError_ReturnsBadRequestResult)}");
        Mock<IRoleService> serviceMock = new();
        Mock<ILogger<RoleController>> loggerMock = new();
        serviceMock.Setup(x => x.RetrieveRolePermissionsAsync(It.IsAny<int>())).ThrowsAsync(exception);

        RoleController controller = new(loggerMock.Object, serviceMock.Object);
        IActionResult result = await controller.RetrieveRolePermissionsAsync(It.IsAny<int>());

        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be(exception.Message);

        serviceMock.Verify(x => x.RetrieveRolePermissionsAsync(It.IsAny<int>()), Times.Once);
    }
}
