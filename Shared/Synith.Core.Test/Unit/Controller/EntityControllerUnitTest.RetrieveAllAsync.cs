namespace Synith.Core.Test.Unit.Base.Controller;
partial class EntityControllerUnitTest
{
    [Fact]
    public void RetrieveAllAsync_EndpointTemplate()
    {
        var route = TestHelper.GetEndpointTemplate<EntityController<Entity>>(nameof(EntityController<Entity>.RetrieveAllAsync))!;
        route.Length.Should().Be(1);
        route[0].Template.Should().Be(null);
        route[0].Should().BeOfType<HttpGetAttribute>();
    }

    [Fact]
    public void RetrieveAllAsync_NoRequiredPermission()
    {
        var permissions = TestHelper.GetRequiredPermissions<EntityController<Entity>>(nameof(EntityController<Entity>.RetrieveAllAsync))!;
        permissions.Length.Should().Be(0);
    }

    [Fact]
    public async Task RetrieveAllAsync_NoError_ReturnsOkObjectResult()
    {
        Mock<IEntityService<Entity>> serviceMock = new();
        Mock<ILogger<EntityController<Entity>>> loggerMock = new();
        serviceMock.Setup(x => x.DeactivateAsync(It.IsAny<int>()));

        EntityController<Entity> controller = new(loggerMock.Object, serviceMock.Object);
        IActionResult result = await controller.RetrieveAllAsync();

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeAssignableTo<IEnumerable<Entity>>();

        serviceMock.Verify(x => x.RetrieveAllAsync(), Times.Once);
    }

    [Fact]
    public async Task RetrieveAllAsync_HasError_ReturnsBadRequestResult()
    {
        Exception exception = new($"Error: {nameof(RetrieveAllAsync_HasError_ReturnsBadRequestResult)}");
        Mock<IEntityService<Entity>> serviceMock = new();
        Mock<ILogger<EntityController<Entity>>> loggerMock = new();
        serviceMock.Setup(x => x.RetrieveAllAsync()).ThrowsAsync(exception);

        EntityController<Entity> controller = new(loggerMock.Object, serviceMock.Object);
        IActionResult result = await controller.RetrieveAllAsync();

        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be(exception.Message);

        serviceMock.Verify(x => x.RetrieveAllAsync(), Times.Once);
    }
}
