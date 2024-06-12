using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace Synith.Core.Test.Integration.Service;
public class EntityServiceFactory : IAsyncLifetime
{
    public TestEntityDbContext DbContext = default!;
    public TestEntityService Service = default!;
    private readonly MsSqlContainer _dbContainer;

    public class TestEntity : Entity { }

    public class TestEntityDbContext : DbContext
    {
        public TestEntityDbContext(DbContextOptions<TestEntityDbContext> options) : base(options) { }
        public DbSet<TestEntity> TestEntities { get; set; }
    }

    public class TestEntityService : EntityService<TestEntity, TestEntity>
    {
        public TestEntityService(ILogger<TestEntityService> logger, DbContext context) : base(logger, context) { }
        protected override TestEntity ToRetrieveDto(TestEntity entity) => entity;
    }

    public EntityServiceFactory()
    {
        _dbContainer = new MsSqlBuilder().WithName($"entity-test-sqlserver").Build();
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        string dacpacPath = @"C:\Users\GBF\source\repos\Synith\CMMS\DatabaseTestData\Synith.Test.Database.Core\bin\Debug\Synith.Test.Database.Core.dacpac";
        new Dacpac(_dbContainer.GetConnectionString(), dacpacPath).PublishDatabase();

        Mock<ILogger<TestEntityService>> loggerMock = new();

        DbContext = new(BuildDbContextOptions<TestEntityDbContext>());
        Service = new(loggerMock.Object, DbContext);
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }

    private DbContextOptions<TDbContext> BuildDbContextOptions<TDbContext>() where TDbContext : DbContext
    {
        DbContextOptionsBuilder<TDbContext> optionsBuilder = new();
        optionsBuilder.UseSqlServer(_dbContainer.GetConnectionString());
        return optionsBuilder.Options;
    }
}
