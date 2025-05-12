using WalletFlow.Application.Wallets.Queries.GetWalletBalance;

namespace WalletFlow.Integration.Tests.Wallets.GetWalletBalance;

public class GetWalletBalanceValidatorTests
{
    private readonly GetWalletBalanceQueryValidator _validator = new GetWalletBalanceQueryValidator();

    [Fact]
    public void Validate_Should_HaveError_When_UserIdIsEmpty()
    {
        // Arrange
        var query = new GetWalletBalanceQuery(Guid.Empty);

        // Act
        var result = _validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(query.UserId));
    }

    [Fact]
    public void Validate_Should_Pass_When_UserIdIsValid()
    {
        // Arrange
        var query = new GetWalletBalanceQuery(Guid.NewGuid());

        // Act
        var result = _validator.Validate(query);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}