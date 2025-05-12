using Microsoft.AspNetCore.Identity;
using WalletFlow.Domain.Entities.Wallets;

namespace WalletFlow.Domain.Entities.Users;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? LastLogin { get; private set; }
    
    public Wallet Wallet { get; private set; } = null!;
    
    private User() { }
    

    public static User Create(string firstName, string lastName, string email)
    {
        return new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            UserName = email
        };
    }

    public void UpdateLastLogin() => LastLogin = DateTime.UtcNow;
    
    public void SetWallet(Wallet wallet)
    {
        Wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));
    }
}