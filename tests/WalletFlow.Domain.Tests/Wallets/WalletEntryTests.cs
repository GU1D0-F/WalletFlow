using WalletFlow.Domain.Entities.Wallets;

namespace WalletFlow.Domain.Tests.Wallets;

public class WalletEntryTests
{
    private readonly Guid _walletId1 = Guid.NewGuid();
    private readonly Guid _walletId2 = Guid.NewGuid();

    [Fact]
    public void CreateDeposit_WithPositiveAmount_ShouldInitializeWalletEntry()
    {
        // Arrange
        var amount = 150.75m;
        var description = "Initial deposit";
        var before = DateTime.UtcNow;

        // Act
        var entry = WalletEntry.CreateDeposit(_walletId1, amount, description);
        var after = DateTime.UtcNow;

        // Assert
        Assert.Equal(_walletId1, entry.WalletId);
        Assert.Equal(WalletEntryType.Deposit, entry.Type);
        Assert.Equal(amount, entry.Amount);
        Assert.Equal(description, entry.Description);

        Assert.NotEqual(Guid.Empty, entry.Id);
        Assert.InRange(entry.CreatedAt, before, after);
        Assert.Null(entry.ReferenceId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void CreateDeposit_WithNonPositiveAmount_ShouldThrowArgumentOutOfRangeException(decimal amount)
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            WalletEntry.CreateDeposit(_walletId1, amount));
        Assert.Equal("amount", ex.ParamName);
        Assert.Contains("Amount must be greater than zero", ex.Message);
    }

    [Fact]
    public void CreateTransfer_WithValidParams_ShouldCreateSentAndReceivedEntries()
    {
        // Arrange
        var amount = 60.00m;
        var description = "Service payment";
        var before = DateTime.UtcNow;

        // Act
        var (sent, received) = WalletEntry.CreateTransfer(_walletId1, _walletId2, amount, description);
        var after = DateTime.UtcNow;

        // Assert sent entry
        Assert.Equal(_walletId1, sent.WalletId);
        Assert.Equal(WalletEntryType.TransferSent, sent.Type);
        Assert.Equal(amount, sent.Amount);
        Assert.Equal(description, sent.Description);
        Assert.NotEqual(Guid.Empty, sent.Id);
        Assert.InRange(sent.CreatedAt, before, after);

        // Assert received entry
        Assert.Equal(_walletId2, received.WalletId);
        Assert.Equal(WalletEntryType.TransferReceived, received.Type);
        Assert.Equal(amount, received.Amount);
        Assert.Equal(description, received.Description);
        Assert.NotEqual(Guid.Empty, received.Id);
        Assert.InRange(received.CreatedAt, before, after);

        // Both entries share the same reference ID
        Assert.NotNull(sent.ReferenceId);
        Assert.Equal(sent.ReferenceId, received.ReferenceId);
        Assert.NotEqual(Guid.Empty, sent.ReferenceId.Value);
    }

    [Fact]
    public void CreateTransfer_WithSameOriginAndDestination_ShouldThrowArgumentException()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() =>
            WalletEntry.CreateTransfer(_walletId1, _walletId1, 25m));
        Assert.Equal("toWalletId", ex.ParamName);
        Assert.Contains("Origin and destination wallets must be different", ex.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void CreateTransfer_WithNonPositiveAmount_ShouldThrowArgumentOutOfRangeException(decimal amount)
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            WalletEntry.CreateTransfer(_walletId1, _walletId2, amount));
        Assert.Equal("amount", ex.ParamName);
        Assert.Contains("Amount must be greater than zero", ex.Message);
    }
}