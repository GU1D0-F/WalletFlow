using Microsoft.EntityFrameworkCore;
using WalletFlow.Application.Wallets.Commands.AddFunds;
using WalletFlow.Domain.Exceptions;
using WalletFlow.Integration.Tests.Infrastructure;

namespace WalletFlow.Integration.Tests.Wallets.AddFunds;

public class AddFundsTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Handle_Should_AddFunds_WhenCommandIsValid()
    {
        // Arrange
        var user = PrincipalUser;
        var wallet = await DbContext.Wallet
            .FirstAsync(w => w.UserId == user.Id);
        var initialBalance = wallet.Balance;
        var amountToAdd = Faker.Random.Decimal(1, 1000);

        // Act
        await Mediator.Send(new AddFundsCommand(user.Id, amountToAdd));

        // Assert
        var updated = await DbContext.Wallet
            .FirstAsync(w => w.Id == wallet.Id);
        Assert.Equal(initialBalance + amountToAdd, updated.Balance);
    }

    [Fact]
    public async Task Handle_Should_ThrowNotFoundException_WhenWalletNotFound()
    {
        // Arrange
        var invalidUserId = Guid.NewGuid();
        var amount = Faker.Random.Decimal(1, 1000);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => Mediator.Send(new AddFundsCommand(invalidUserId, amount))
        );
    }
}