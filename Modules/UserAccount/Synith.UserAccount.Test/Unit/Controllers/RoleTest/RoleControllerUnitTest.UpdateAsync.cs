using Synith.UserAccount.Domain.DataTransferObjects.Role;

namespace Synith.UserAccount.Test.Unit.Controllers.RoleTest;
partial class RoleControllerUnitTest
{
    [Fact]
    public void UpdateAsync_EndpointTemplate()
    {
        var route = TestHelper.GetEndpointTemplate<RoleController>(nameof(RoleController.UpdateAsync))!;
        route.Length.Should().Be(1);
        route[0].Template.Should().Be(null);
        route[0].Should().BeOfType<HttpPutAttribute>();
    }

    [Fact]
    public void UpdateAsync_RequiresPermission_RoleEdit()
    {
        var permissions = TestHelper.GetRequiredPermissions<RoleController>(nameof(RoleController.UpdateAsync))!;
        permissions.Length.Should().Be(1);
        permissions[0].PermissionCode.Should().Be(PermissionCode.Role.Edit);
    }

    [Fact]
    public async Task UpdateAsync_NoError_ReturnsCreateObjectResult()
    {
        Role role = new();
        Mock<IRoleService> serviceMock = new();
        Mock<ILogger<RoleController>> loggerMock = new();
        serviceMock.Setup(x => x.UpdateAsync(It.IsAny<RoleUpdate>())).ReturnsAsync(role);

        RoleController controller = new(loggerMock.Object, serviceMock.Object);
        IActionResult result = await controller.UpdateAsync(new RoleUpdate());

        result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.As<OkObjectResult>().Value.Should().Be(role);

        serviceMock.Verify(x => x.UpdateAsync(It.IsAny<RoleUpdate>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_HasError_ReturnsBadRequestResult()
    {
        Exception exception = new($"Error: {nameof(UpdateAsync_HasError_ReturnsBadRequestResult)}");
        Mock<IRoleService> serviceMock = new();
        Mock<ILogger<RoleController>> loggerMock = new();
        serviceMock.Setup(x => x.UpdateAsync(It.IsAny<RoleUpdate>())).ThrowsAsync(exception);

        RoleController controller = new(loggerMock.Object, serviceMock.Object);
        IActionResult result = await controller.UpdateAsync(new RoleUpdate());

        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be(exception.Message);

        serviceMock.Verify(x => x.UpdateAsync(It.IsAny<RoleUpdate>()), Times.Once);
    }
}
