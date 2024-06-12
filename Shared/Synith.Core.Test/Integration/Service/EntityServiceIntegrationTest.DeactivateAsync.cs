using Microsoft.EntityFrameworkCore;
using static Synith.Core.Test.Integration.Service.EntityServiceFactory;

namespace Synith.Core.Test.Integration.Service;
partial class EntityServiceIntegrationTest
{
    [Fact]
    public async Task DeactivateAsync_Valid_NoError()
    {
        TestEntity entity = new();
        await _context.TestEntities.AddAsync(entity);
        await _context.SaveChangesAsync();
        _context.Entry(entity).State = EntityState.Detached;

        await _service.DeactivateAsync(entity.Id);

        entity = await _service.RetrieveByIdAsync(entity.Id);

        entity.Status.Should().Be(Enums.GenericStatus.Inactive);
        entity.InactiveDate.Should().NotBeNull();
    }

    [Fact]
    public async Task DeactivateAsync_Invalid_HasError()
    {
        int id = int.MaxValue;
        Func<Task> action = async () => await _service.DeactivateAsync(id);

        await action.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage(ErrorMessageProvider.NotFound($"{nameof(Entity)}.{nameof(Entity.Id)}", id.ToString()));
    }
}
