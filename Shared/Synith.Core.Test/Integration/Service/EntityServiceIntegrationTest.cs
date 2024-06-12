using static Synith.Core.Test.Integration.Service.EntityServiceFactory;

namespace Synith.Core.Test.Integration.Service;
public partial class EntityServiceIntegrationTest : IClassFixture<EntityServiceFactory>
{
    private readonly TestEntityDbContext _context;
    private readonly TestEntityService _service;

    public EntityServiceIntegrationTest(EntityServiceFactory containerFactory)
    {
        _context = containerFactory.DbContext;
        _service = containerFactory.Service;
    }
}
