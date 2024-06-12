using Synith.Security.Interfaces;

namespace Synith.UserAccount.Test.Unit.Controllers.UserTest;
partial class UserControllerUnitTest
{
    [Fact]
    public void RetrieveLanguagesAsync_EndpointTemplate()
    {
        var route = TestHelper.GetEndpointTemplate<UserController>(nameof(UserController.RetrieveLanguagesAsync))!;
        route.Length.Should().Be(1);
        route[0].Template.Should().Be("Languages");
        route[0].Should().BeOfType<HttpGetAttribute>();
    }

    [Fact]
    public void RetrieveLanguagesAsync_NoPermissionRequired()
    {
        var permissions = TestHelper.GetRequiredPermissions<UserController>(nameof(UserController.RetrieveLanguagesAsync))!;
        permissions.Length.Should().Be(0);
    }

    [Fact]
    public async Task RetrieveLanguagesAsync_NoError_ReturnsOkObjectResult()
    {
        Mock<IUserService> serviceMock = new();
        Mock<ILogger<UserController>> loggerMock = new();
        Mock<ITokenService> tokenMock = new();
        Mock<ICryptographyService> cryptographyMock = new();
        serviceMock.Setup(x => x.RetrieveLanguagesAsync());

        UserController controller = new(loggerMock.Object, serviceMock.Object, cryptographyMock.Object, tokenMock.Object);
        IActionResult result = await controller.RetrieveLanguagesAsync();

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeAssignableTo<IEnumerable<Language>>();

        serviceMock.Verify(x => x.RetrieveLanguagesAsync(), Times.Once);
    }

    [Fact]
    public async Task RetrieveLanguagesAsync_HasError_ReturnsBadRequestResult()
    {
        Exception exception = new($"Error: {nameof(RetrieveLanguagesAsync_HasError_ReturnsBadRequestResult)}");
        Mock<IUserService> serviceMock = new();
        Mock<ILogger<UserController>> loggerMock = new();
        Mock<ITokenService> tokenMock = new();
        Mock<ICryptographyService> cryptographyMock = new();
        serviceMock.Setup(x => x.RetrieveLanguagesAsync()).ThrowsAsync(exception);

        UserController controller = new(loggerMock.Object, serviceMock.Object, cryptographyMock.Object, tokenMock.Object);
        IActionResult result = await controller.RetrieveLanguagesAsync();

        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be(exception.Message);

        serviceMock.Verify(x => x.RetrieveLanguagesAsync(), Times.Once);
    }
}
