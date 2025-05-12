using WalletFlow.Domain.Entities.Wallets;

namespace WalletFlow.Domain.Tests.Wallets;

public class WalletTests
{
     private readonly Guid _userId = Guid.NewGuid();

        [Fact]
        public void Create_ShouldInitializeWithZeroBalance_AndCurrencyBRL_AndNonEmptyId()
        {
            // Act
            var wallet = Wallet.Create(_userId);

            // Assert
            Assert.Equal(_userId, wallet.UserId);
            Assert.Equal(0m, wallet.Balance);
            Assert.Equal("BRL", wallet.Currency);
            Assert.NotEqual(Guid.Empty, wallet.Id);
        }

        [Theory]
        [InlineData(100.50)]
        [InlineData(0.01)]
        public void AddFunds_WithPositiveAmount_ShouldIncreaseBalance(decimal amount)
        {
            // Arrange
            var wallet = Wallet.Create(_userId);

            // Act
            wallet.AddFunds(amount);

            // Assert
            Assert.Equal(amount, wallet.Balance);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void AddFunds_WithZeroOrNegativeAmount_ShouldThrowArgumentOutOfRangeException(decimal amount)
        {
            // Arrange
            var wallet = Wallet.Create(_userId);

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => wallet.AddFunds(amount));
            Assert.Contains("Amount must be greater than zero", ex.Message);
        }

        [Fact]
        public void SubtractFunds_WithSufficientBalance_ShouldDecreaseBalance()
        {
            // Arrange
            var wallet = Wallet.Create(_userId);
            wallet.AddFunds(200m);

            // Act
            wallet.SubtractFunds(75.25m);

            // Assert
            Assert.Equal(124.75m, wallet.Balance);
        }

        [Fact]
        public void SubtractFunds_WhenAmountExceedsBalance_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var wallet = Wallet.Create(_userId);
            // balance = 0

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => wallet.SubtractFunds(1m));
            Assert.Equal("Insufficient funds.", ex.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void SubtractFunds_WithZeroOrNegativeAmount_ShouldThrowArgumentOutOfRangeException(decimal amount)
        {
            // Arrange
            var wallet = Wallet.Create(_userId);

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => wallet.SubtractFunds(amount));
            Assert.Contains("Amount must be greater than zero", ex.Message);
        }
}