using WalletFlow.Domain.Entities.Users;

namespace WalletFlow.Domain.Entities.Wallets;

public class Wallet : BaseEntity
{
    public Guid UserId { get; }
    public decimal Balance { get; private set; }
    public string Currency { get; }
    
    public User? User { get; private set; }
    
    private Wallet(Guid id, Guid userId, decimal balance)
    {
        Id = id;
        UserId = userId;
        Balance = balance;
        Currency = "BRL";
    }

    public static Wallet Create(Guid userId)
    {
        var now = DateTime.UtcNow;
        return new Wallet(
            Guid.NewGuid(),
            userId,
            0m
        );
    }

    public void AddFunds(decimal amount)
    {
        ValidateAmount(amount);
        Balance += amount;
        SetUpdatedAt();
    }

    public void SubtractFunds(decimal amount)
    {
        ValidateAmount(amount);
        if (amount > Balance)
            throw new InvalidOperationException("Insufficient funds.");

        Balance -= amount;
        SetUpdatedAt();
    }

    private static void ValidateAmount(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");
    }
}