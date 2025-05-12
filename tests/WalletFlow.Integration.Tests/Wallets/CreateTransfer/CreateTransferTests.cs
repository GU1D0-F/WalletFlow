using Microsoft.EntityFrameworkCore;
using WalletFlow.Application.Wallets.Commands.CreateTransfer;
using WalletFlow.Domain.Entities.Wallets;
using WalletFlow.Integration.Tests.Infrastructure;

namespace WalletFlow.Integration.Tests.Wallets.CreateTransfer;

public class CreateTransferTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Handle_Should_CreateTwoEntries_WhenCommandIsValid()
    {
        // Arrange
        var user = PrincipalUser;
        var toWallet = await DbContext.Wallet.FirstAsync(x => x.Id != user.Wallet.Id);

        var amount = Faker.Random.Decimal(5, 100);
        var description = Faker.Lorem.Sentence();

        // Act
        var result = await Mediator.Send(
            new CreateTransferCommand(
                user.Id,
                toWallet.Id,
                amount,
                description
            )
        );

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Wallet.Id, result.WalletId);
        Assert.Equal(amount, result.Amount);
        Assert.Equal(description, result.Description);
        Assert.Equal(WalletEntryType.TransferSent.ToString(), result.Type);

        // Verify persistence of both entries
        var entries = await DbContext.WalletEntry
            .Where(e => e.ReferenceId == result.ReferenceId)
            .ToListAsync();
        Assert.Equal(2, entries.Count);
        Assert.Contains(entries, e => e.WalletId == user.Wallet.Id && e.Type == WalletEntryType.TransferSent);
        Assert.Contains(entries, e => e.WalletId == toWallet.Id && e.Type == WalletEntryType.TransferReceived);
    }
}