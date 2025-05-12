using Microsoft.EntityFrameworkCore;
using WalletFlow.Application.Wallets.Commands.CreateTransfer;
using WalletFlow.Application.Wallets.Queries.GetUserTransfers;
using WalletFlow.Domain.Entities.Wallets;
using WalletFlow.Integration.Tests.Infrastructure;

namespace WalletFlow.Integration.Tests.Wallets.GetUserTransfers;

public class GetUserTransfersTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Handle_Should_ReturnEntries_ForFromWalletWithoutFilter()
    {
        // Arrange
        var user = PrincipalUser;
        var toWallet = await DbContext.Wallet.AsNoTracking().FirstAsync(x => x.Id != user.Wallet.Id);

        // Create a transfer
        var amount = Faker.Random.Decimal(5, 100);
        var description = "Test transfer";
        await Mediator.Send(new CreateTransferCommand(user.Id, toWallet.Id, amount, description));

        // Act
        var result = await Mediator.Send(new GetUserTransfersQuery(user.Id));

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        var entryDto = result[0];
        Assert.Equal(user.Wallet.Id, entryDto.WalletId);
        Assert.Equal(amount, entryDto.Amount);
        Assert.Equal(description, entryDto.Description);
        Assert.Equal(WalletEntryType.TransferSent.ToString(), entryDto.Type);
    }

    [Fact]
    public async Task Handle_Should_FilterByDateRange()
    {
        // Arrange
        DbContext.RemoveRange(DbContext.WalletEntry);
        var user = PrincipalUser;
        var toUserWallet = await DbContext.Users.Include(u => u.Wallet).FirstAsync(x => x.Id != user.Id);

        var amount = Faker.Random.Decimal(5, 100);
        var description = "Date filter transfer";

        // Create a transfer at current time
        var now = DateTime.UtcNow;
        await Mediator.Send(new CreateTransferCommand(user.Id, toUserWallet.Wallet.Id, amount, description));

        // Act: within range
        var from = now.AddMinutes(-1);
        var to = now.AddMinutes(1);
        var queryWithin = new GetUserTransfersQuery(user.Id, from, to);
        var resultWithin = await Mediator.Send(queryWithin);

        // Act: outside range
        var queryOutside = new GetUserTransfersQuery(user.Id, now.AddDays(-2), now.AddDays(-1));
        var resultOutside = await Mediator.Send(queryOutside);

        // Assert
        Assert.Single(resultWithin);
        Assert.Empty(resultOutside);
    }
}