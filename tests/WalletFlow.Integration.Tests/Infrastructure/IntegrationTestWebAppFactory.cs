using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using WalletFlow.Infrastructure;

namespace WalletFlow.Integration.Tests.Infrastructure;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Api.Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer;
    private string? _connectionString;

    public IntegrationTestWebAppFactory()
    {
        _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("CoreDbContext")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();
    }

    private async Task EnsureDatabaseIsReady()
    {
        if (_connectionString == null)
        {
            // Add a delay before starting the container to ensure environment is ready
            await Task.Delay(TimeSpan.FromSeconds(3));
            
            await _dbContainer.StartAsync();
            _connectionString = _dbContainer.GetConnectionString() + ";Client Encoding=UTF8";
        }
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Ensure database is ready before configuring services
        EnsureDatabaseIsReady().GetAwaiter().GetResult();

        builder.UseEnvironment("Test");

        if (string.IsNullOrEmpty(_connectionString))
            throw new InvalidOperationException("Database container is not ready. Connection string is null.");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Clear any existing configuration sources
            config.Sources.Clear();
            
            // Add the test configuration
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:Default"] = _connectionString,
                ["Jwt:Key"] = "q3@5F9dL!v8P2xZ7k6Rt4Yh1JmN3UbC4",
                ["Jwt:Issuer"] = "WalletFlow.Api",
                ["Logging:LogLevel:Default"] = "Debug",
                ["Logging:LogLevel:Microsoft"] = "Debug",
                ["Logging:LogLevel:Microsoft.Hosting.Lifetime"] = "Debug"
            });
        });

        builder.ConfigureTestServices(services =>
        {
            var descriptor = services
                .SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<CoreDbContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }
            
            services.AddDbContext<CoreDbContext>(options =>
            {
                options
                    .UseNpgsql(_connectionString)
                    .UseSnakeCaseNamingConvention()
                    .EnableSensitiveDataLogging();
            });
        });
    }

    public async Task InitializeAsync()
    {
        await EnsureDatabaseIsReady();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _dbContainer.DisposeAsync().GetAwaiter().GetResult();
        }

        base.Dispose(disposing);
    }
}