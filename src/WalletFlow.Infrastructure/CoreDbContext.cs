using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WalletFlow.Domain.Entities.Users;
using WalletFlow.Domain.Entities.Wallets;

namespace WalletFlow.Infrastructure;

//dotnet ef migrations add <MigrationName> --project .\src\WalletFlow.Infrastructure\ --startup-project .\src\WalletFlow.Api\ --context CoreDbContext
public class CoreDbContext(DbContextOptions<CoreDbContext> options) : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Wallet> Wallet => Set<Wallet>();
    public DbSet<WalletEntry> WalletEntry => Set<WalletEntry>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoreDbContext).Assembly);
    }
}