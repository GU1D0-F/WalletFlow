using Bogus;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WalletFlow.Domain.Entities.Users;
using WalletFlow.Domain.Entities.Wallets;
using WalletFlow.Infrastructure;
using WalletFlow.Integration.Tests.TestData;

namespace WalletFlow.Integration.Tests.Infrastructure;

public class BaseIntegrationTest: IClassFixture<IntegrationTestWebAppFactory>, IAsyncLifetime
{
    protected readonly Faker Faker;
    private readonly IServiceScope _scope;
    protected readonly IMediator Mediator;
    protected readonly CoreDbContext DbContext;
    private readonly TestDataSeeder _dataSeeder;
    protected readonly IHttpContextAccessor HttpContextAccessor;
    
    private User? principalUser { get; set; } = null;

    protected User PrincipalUser
    {
        get
        {
            principalUser ??= DbContext.Users
                .FirstOrDefault(x => x.Email!.Equals(UsersDetails.Email));

            return principalUser!;
        }
        private set => principalUser = value;
    }

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        Faker = new Faker("pt_BR");
        Mediator = _scope.ServiceProvider.GetRequiredService<IMediator>();
        DbContext = _scope.ServiceProvider.GetRequiredService<CoreDbContext>();
        HttpContextAccessor  = _scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
        
        var userManager = _scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        _dataSeeder = new TestDataSeeder(DbContext, userManager);

        if (DbContext.Database.GetPendingMigrations().Any())
        {
            DbContext.Database.Migrate();
        }
    }

    public async Task InitializeAsync()
    {
        await _dataSeeder.SeedAsync();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}