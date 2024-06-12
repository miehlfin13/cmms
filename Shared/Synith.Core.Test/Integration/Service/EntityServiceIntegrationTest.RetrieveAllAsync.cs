using static Synith.Core.Test.Integration.Service.EntityServiceFactory;

namespace Synith.Core.Test.Integration.Service;
partial class EntityServiceIntegrationTest
{
    [Fact]
    public async Task RetrieveAllAsync_NoRecords_NoError()
    {
        var list = await _service.RetrieveAllAsync();
        list.Should().BeEmpty();
    }

    [Fact]
    public async Task RetrieveAllAsync_HasRecords_NoError()
    {
        TestEntity first = new();
        TestEntity second = new();
        await _context.TestEntities.AddAsync(first);
        await _context.TestEntities.AddAsync(second);
        await _context.SaveChangesAsync();
        
        var list = await _service.RetrieveAllAsync();

        list.Should().ContainEquivalentOf(first);
        list.Should().ContainEquivalentOf(second);
    }
}
