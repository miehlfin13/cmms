using Microsoft.EntityFrameworkCore;
using Synith.UserAccount.Domain.Entities;
using Synith.UserAccount.Domain.Junction;

namespace Synith.UserAccount.Infrastructure.Persistence;
public class UserAccountDbContext : DbContext
{
	public UserAccountDbContext(DbContextOptions<UserAccountDbContext> options) : base(options)
	{

	}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RolePermission>()
            .HasKey(x => new { x.RoleId, x.PermissionId });

        modelBuilder.Entity<UserRole>()
            .HasKey(x => new { x.UserId, x.RoleId });

        modelBuilder.Entity<UserSetting>()
            .HasKey(x => new { x.UserId, x.Name });
    }

    public DbSet<Module> Modules { get; set; }
    public DbSet<Permission> Permissions { get; set; }
	public DbSet<Role> Roles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }

    public DbSet<Language> Languages { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<UserSetting> UserSettings { get; set; }
}
