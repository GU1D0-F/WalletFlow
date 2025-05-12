using FluentValidation.Results;
using WalletFlow.Application.Wallets.Commands.AddFunds;

namespace WalletFlow.Integration.Tests.Wallets.AddFunds;

public class AddFundsValidatorTests
{
    private readonly AddFundsCommandValidator _validator = new AddFundsCommandValidator();

    [Fact]
    public void Validate_Should_HaveError_When_UserIdIsEmpty()
    {
        // Arrange
        var command = new AddFundsCommand(Guid.Empty, 100);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(command.UserId));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-50)]
    public void Validate_Should_HaveError_When_AmountNotGreaterThanZero(decimal invalidAmount)
    {
        // Arrange
        var command = new AddFundsCommand(Guid.NewGuid(), invalidAmount);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(command.Amount));
    }

    [Fact]
    public void Validate_Should_Pass_When_CommandIsValid()
    {
        // Arrange
        var command = new AddFundsCommand(Guid.NewGuid(), 150);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}