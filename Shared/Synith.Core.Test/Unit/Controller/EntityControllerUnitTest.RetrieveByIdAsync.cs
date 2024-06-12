namespace Synith.Core.Test.Unit.Base.Controller;
partial class EntityControllerUnitTest
{
    [Fact]
    public void RetrieveByIdAsync_EndpointTemplate()
    {
        var route = TestHelper.GetEndpointTemplate<EntityController<Entity>>(nameof(EntityController<Entity>.RetrieveByIdAsync))!;
        route.Length.Should().Be(1);
        route[0].Template.Should().Be("{id}");
        route[0].Should().BeOfType<HttpGetAttribute>();
    }

    [Fact]
    public void RetrieveByIdAsync_NoRequiredPermission()
    {
        var permissions = TestHelper.GetRequiredPermissions<EntityController<Entity>>(nameof(EntityController<Entity>.RetrieveByIdAsync))!;
        permissions.Length.Should().Be(0);
    }

    [Fact]
    public async Task RetrieveByIdAsync_NoError_ReturnsOkObjectResult()
    {
        Entity entity = It.IsAny<Entity>();
        Mock<IEntityService<Entity>> serviceMock = new();
        Mock<ILogger<EntityController<Entity>>> loggerMock = new();
        serviceMock.Setup(x => x.RetrieveByIdAsync(It.IsAny<int>())).ReturnsAsync(entity);

        EntityController<Entity> controller = new(loggerMock.Object, serviceMock.Object);
        IActionResult result = await controller.RetrieveByIdAsync(It.IsAny<int>());

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().Be(entity);

        serviceMock.Verify(x => x.RetrieveByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task RetrieveByIdAsync_HasError_ReturnsBadRequestResult()
    {
        Exception exception = new($"Error: {nameof(RetrieveByIdAsync_HasError_ReturnsBadRequestResult)}");
        Mock<IEntityService<Entity>> serviceMock = new();
        Mock<ILogger<EntityController<Entity>>> loggerMock = new();
        serviceMock.Setup(x => x.RetrieveByIdAsync(It.IsAny<int>())).ThrowsAsync(exception);

        EntityController<Entity> controller = new(loggerMock.Object, serviceMock.Object);
        IActionResult result = await controller.RetrieveByIdAsync(It.IsAny<int>());

        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be(exception.Message);

        serviceMock.Verify(x => x.RetrieveByIdAsync(It.IsAny<int>()), Times.Once);
    }
}
