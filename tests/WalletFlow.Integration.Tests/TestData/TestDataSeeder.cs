using Bogus;
using Microsoft.AspNetCore.Identity;
using WalletFlow.Domain.Entities.Users;
using WalletFlow.Domain.Entities.Wallets;
using WalletFlow.Infrastructure;

namespace WalletFlow.Integration.Tests.TestData;

public class TestDataSeeder
{
    private const string Password = "SenhaForte123!";
    private readonly UserManager<User> _userManager;
    private readonly CoreDbContext _context;
    private readonly Faker _faker;

    public TestDataSeeder(CoreDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
        _faker = new Faker("pt_BR");
    }

    public async Task SeedAsync()
    {
        await _context.Database.EnsureCreatedAsync();

        await CreaterUser();
        await CreatePrincipalUserAsync();
        await _context.SaveChangesAsync();
    }


    private async Task CreatePrincipalUserAsync()
    {
        if (await _userManager.FindByEmailAsync(UsersDetails.Email) != null)
            return;

        var firstName = _faker.Name.FirstName();
        var lastName = _faker.Name.LastName();
        var email = UsersDetails.Email;

        var user = User.Create(firstName, lastName, email);

        var result = await _userManager.CreateAsync(user, Password);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new Exception($"Falha ao semear usuário de teste: {errors}");
        }
        
        var wallet = Wallet.Create(user.Id);
        wallet.AddFunds(10000);
        user.SetWallet(wallet);

        await _context.Wallet.AddAsync(wallet);
    }


    private async Task CreaterUser()
    {
        var firstName = _faker.Name.FirstName();
        var lastName = _faker.Name.LastName();
        var email = _faker.Person.Email;
        
        var user = User.Create(firstName, lastName, email);
        
        var result = await _userManager.CreateAsync(user, Password);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new Exception($"Falha ao semear usuário de teste: {errors}");
        }
        
        var wallet = Wallet.Create(user.Id);
        user.SetWallet(wallet);

        await _context.Wallet.AddAsync(wallet);
    }
    
}