using WalletFlow.Application.Wallets.Commands.AddFunds;
using WalletFlow.Application.Wallets.Queries.GetWalletBalance;
using WalletFlow.Domain.Exceptions;
using WalletFlow.Integration.Tests.Infrastructure;

namespace WalletFlow.Integration.Tests.Wallets.GetWalletBalance;

public class GetWalletBalanceTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Handle_Should_ReturnCorrectBalance_AfterAddingFunds()
    {
        // Arrange
        var user = PrincipalUser;

        // Initial balance should be zero (seeded wallet)
        var initialBalance = await Mediator.Send(new GetWalletBalanceQuery(user.Id));
        Assert.Equal(10000, initialBalance);

        // Act: add funds
        var amount = Faker.Random.Decimal(1, 500);
        await Mediator.Send(new AddFundsCommand(user.Id, amount));

        // Assert: updated balance
        var updatedBalance = await Mediator.Send(new GetWalletBalanceQuery(user.Id));
        Assert.Equal(initialBalance + amount, updatedBalance);
    }

    [Fact]
    public async Task Handle_Should_ThrowNotFoundException_WhenWalletNotFound()
    {
        // Arrange
        var invalidUserId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => Mediator.Send(new GetWalletBalanceQuery(invalidUserId))
        );
    }
}