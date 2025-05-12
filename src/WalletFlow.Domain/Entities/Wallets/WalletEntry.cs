namespace WalletFlow.Domain.Entities.Wallets;

public enum WalletEntryType
{
    Deposit,
    TransferSent,
    TransferReceived
}

public class WalletEntry : BaseEntity
{
    public Guid WalletId { get; private set; }
    public WalletEntryType Type { get; private set; }
    public decimal Amount { get; private set; }
    public string? Description { get; private set; }
    public Guid? ReferenceId { get; private set; }
    
    protected WalletEntry() { }

    private WalletEntry(
        Guid walletId,
        WalletEntryType type,
        decimal amount,
        string? description,
        Guid? referenceId)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");

        Id = Guid.NewGuid();
        WalletId = walletId;
        Type = type;
        Amount = amount;
        Description = description;
        ReferenceId = referenceId;
        CreatedAt = DateTime.UtcNow;
    }

    public static WalletEntry CreateDeposit(Guid walletId, decimal amount, string? description = null)
        => new(walletId, WalletEntryType.Deposit, amount, description, null);

    public static (WalletEntry Sent, WalletEntry Received) CreateTransfer(
        Guid fromWalletId,
        Guid toWalletId,
        decimal amount,
        string? description = null)
    {
        if (fromWalletId == toWalletId)
            throw new ArgumentException("Origin and destination wallets must be different.", nameof(toWalletId));

        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");

        var referenceId = Guid.NewGuid();

        var sent = new WalletEntry(
            walletId: fromWalletId,
            type: WalletEntryType.TransferSent,
            amount: amount,
            description: description,
            referenceId: referenceId);

        var received = new WalletEntry(
            walletId: toWalletId,
            type: WalletEntryType.TransferReceived,
            amount: amount,
            description: description,
            referenceId: referenceId);

        return (sent, received);
    }
}