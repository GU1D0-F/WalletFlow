using FluentValidation.Results;
using WalletFlow.Application.Wallets.Queries.GetUserTransfers;

namespace WalletFlow.Integration.Tests.Wallets.GetUserTransfers;

public class GetUserTransfersValidatorTests
{
    private readonly GetUserTransfersQueryValidator _validator = new GetUserTransfersQueryValidator();

    [Fact]
    public void Constructor_Should_Throw_When_FromWithoutTo()
    {
        var from = DateTime.UtcNow;
        Assert.Throws<ArgumentException>(() => new GetUserTransfersQuery(Guid.NewGuid(), from, null));
    }

    [Fact]
    public void Constructor_Should_Throw_When_ToWithoutFrom()
    {
        var to = DateTime.UtcNow;
        Assert.Throws<ArgumentException>(() => new GetUserTransfersQuery(Guid.NewGuid(), null, to));
    }

    [Fact]
    public void Constructor_Should_Throw_When_FromGreaterThanTo()
    {
        var from = DateTime.UtcNow;
        var to = from.AddDays(-1);
        Assert.Throws<ArgumentException>(() => new GetUserTransfersQuery(Guid.NewGuid(), from, to));
    }

    [Fact]
    public void Constructor_Should_NotThrow_When_ValidParameters()
    {
        var from = DateTime.UtcNow.AddDays(-1);
        var to = DateTime.UtcNow;
        var ex = Record.Exception(() => new GetUserTransfersQuery(Guid.NewGuid(), from, to));
        Assert.Null(ex);
    }

    [Fact]
    public void Validate_Should_Pass_When_OnlyWalletIdIsProvided()
    {
        // Arrange
        var query = new GetUserTransfersQuery(Guid.NewGuid(), null, null);

        // Act
        var result = _validator.Validate(query);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_Should_Pass_When_FromAndToAreValid()
    {
        // Arrange
        var from = DateTime.UtcNow.AddDays(-1);
        var to = DateTime.UtcNow;
        var query = new GetUserTransfersQuery(Guid.NewGuid(), from, to);

        // Act
        var result = _validator.Validate(query);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}