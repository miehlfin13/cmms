using Microsoft.EntityFrameworkCore;
using Synith.Core.Extensions;
using Synith.UserAccount.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new NullReferenceException("DefaultConnection");

string serviceName = "UserAccount";
builder.Services.AddDbContext<UserAccountDbContext>(options =>
{
    options.UseSqlServer(connectionString, options => options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
});

builder.ConfigureSynith(serviceName);

namespace Synith
{
    public class SynithCMMSEntryPoint { }
}
