using static Synith.Core.Test.Integration.Service.EntityServiceFactory;

namespace Synith.Core.Test.Integration.Service;
partial class EntityServiceIntegrationTest
{
    [Fact]
    public async Task RetrieveByIdAsync_Valid_NoError()
    {
        TestEntity entity = new();
        await _context.TestEntities.AddAsync(entity);
        await _context.SaveChangesAsync();

        TestEntity actual = await _service.RetrieveByIdAsync(entity.Id);

        actual.Should().BeEquivalentTo(entity);
    }

    [Fact]
    public async Task RetrieveByIdAsync_Invalid_HasError()
    {
        int id = int.MaxValue;
        Func<Task> action = async () => await _service.RetrieveByIdAsync(id);

        await action.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage(ErrorMessageProvider.NotFound($"{nameof(Entity)}.{nameof(Entity.Id)}", id.ToString()));
    }
}
