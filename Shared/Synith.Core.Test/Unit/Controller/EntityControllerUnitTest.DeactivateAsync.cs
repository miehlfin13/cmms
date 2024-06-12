namespace Synith.Core.Test.Unit.Base.Controller;
public partial class EntityControllerUnitTest
{
    [Fact]
    public void DeactivateAsync_EndpointTemplate()
    {
        var route = TestHelper.GetEndpointTemplate<EntityController<Entity>>(nameof(EntityController<Entity>.DeactivateAsync))!;
        route.Length.Should().Be(1);
        route[0].Template.Should().Be("{id}");
        route[0].Should().BeOfType<HttpDeleteAttribute>();
    }

    [Fact]
    public void RetrieveRolePermissionsAsync_NoRequiredPermission()
    {
        var permissions = TestHelper.GetRequiredPermissions<EntityController<Entity>>(nameof(EntityController<Entity>.DeactivateAsync))!;
        permissions.Length.Should().Be(0);
    }

    [Fact]
    public async Task DeactivateAsync_NoError_ReturnsNoContentResult()
    {
        Mock<IEntityService<Entity>> serviceMock = new();
        Mock<ILogger<EntityController<Entity>>> loggerMock = new();
        serviceMock.Setup(x => x.DeactivateAsync(It.IsAny<int>()));

        EntityController<Entity> controller = new(loggerMock.Object, serviceMock.Object);
        IActionResult result = await controller.DeactivateAsync(It.IsAny<int>());

        result.Should().BeOfType<NoContentResult>();

        serviceMock.Verify(x => x.DeactivateAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task DeactivateAsync_HasError_ReturnsBadRequestResult()
    {
        Exception exception = new($"Error: {nameof(DeactivateAsync_HasError_ReturnsBadRequestResult)}");
        Mock<IEntityService<Entity>> serviceMock = new();
        Mock<ILogger<EntityController<Entity>>> loggerMock = new();
        serviceMock.Setup(x => x.DeactivateAsync(It.IsAny<int>())).ThrowsAsync(exception);

        EntityController<Entity> controller = new(loggerMock.Object, serviceMock.Object);
        IActionResult result = await controller.DeactivateAsync(It.IsAny<int>());

        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be(exception.Message);

        serviceMock.Verify(x => x.DeactivateAsync(It.IsAny<int>()), Times.Once);
    }
}
