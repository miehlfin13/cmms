using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Synith.Caching;
using Synith.Security.Constants;
using Synith.Security.Interfaces;
using System.Security.Claims;
using Testcontainers.MsSql;
using Xunit;

namespace Synith.Test;
public abstract class EntityApiFactory<TEntryPoint, TDbContext> : WebApplicationFactory<TEntryPoint>, IAsyncLifetime
    where TEntryPoint : class
    where TDbContext : DbContext
{
    private readonly MsSqlContainer _dbContainer;
    private readonly IConfiguration _testConfig;
    private ITokenService _token = default!;
    private ICache _cache = default!;

    public HttpClient Client { get; init; }

    public EntityApiFactory()
    {
        string name = typeof(TDbContext).Name.Replace("DbContext", "").ToLower();
        string id = Guid.NewGuid().ToString();
        _testConfig = BuildConfiguration();
        _dbContainer = new MsSqlBuilder().WithName($"{name}-test-sqlserver-{id}").Build();
        Client = CreateClient();
    }

    public void SetAuth(IEnumerable<string>? permissionCodes = null)
    {
        int roleId = new Random().Next();
        SetHeaderBearer();
        AddRolePermissionCache();

        #region local functions
        void SetHeaderBearer()
        {
            List<Claim> claims = [new(ClaimName.RoleIds, roleId.ToString())];
            string bearer = _token.GenerateToken(claims, true);
            Client.DefaultRequestHeaders.Authorization = new("Bearer", bearer);
        }

        void AddRolePermissionCache()
        {
            string key = CacheKey.RolePermissions.Replace("{roleId}", roleId.ToString());
            List<Dictionary<string, object>> permissions = [];

            if (permissionCodes != null)
            {
                foreach (string permissionCode in permissionCodes)
                {
                    permissions.Add(new()
                    {
                        { "code", permissionCode }
                    });
                }
            }
            _cache.Set(key, permissions);
        }
        #endregion
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<TDbContext>));
            services.AddDbContext<TDbContext>(options =>
            {
                options.UseSqlServer(_dbContainer.GetConnectionString());
            });
        });

        builder.ConfigureAppConfiguration(configuration =>
        {
            configuration.AddInMemoryCollection(
                new Dictionary<string, string?>()
                {
                    ["Jwt:AccessExpirationInMinutes"] = "30",
                    ["Jwt:RefreshExpirationInDays"] = "30",
                    ["Jwt:RefreshRenewalOffsetDays"] = "5"
                });
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        new Dacpac(_dbContainer.GetConnectionString(), _testConfig["DacpacPath"]!).PublishDatabase();

        using var scope = Services.CreateScope();

        _token = scope.ServiceProvider.GetRequiredService<ITokenService>();
        _cache = scope.ServiceProvider.GetRequiredService<ICache>();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }

    private static IConfiguration BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("testsettings.json", false, true)
            .AddEnvironmentVariables();
        return builder.Build();
    }
}
