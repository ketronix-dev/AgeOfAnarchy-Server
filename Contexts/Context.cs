using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.ServiceClasses;

namespace Server.Contexts;

public class Context : IdentityDbContext<AuthUser>
{
    public Context() => Database.EnsureCreated();
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=server;Password=postgres");
    }

    public Context(DbContextOptions<Context> options) : base(options) { }
}