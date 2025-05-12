using WalletFlow.Application.Wallets.Commands.CreateTransfer;

namespace WalletFlow.Integration.Tests.Wallets.CreateTransfer;

public class CreateTransferValidatorTests
{
    private readonly CreateTransferCommandValidator _validator = new CreateTransferCommandValidator();

        [Fact]
        public void Validate_Should_HaveError_When_FromUserIdIsEmpty()
        {
            var cmd = new CreateTransferCommand(Guid.Empty, Guid.NewGuid(), 10, null);

            var result = _validator.Validate(cmd);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.UserId));
        }

        [Fact]
        public void Validate_Should_HaveError_When_ToWalletIdIsEmpty()
        {
            var cmd = new CreateTransferCommand(Guid.NewGuid(), Guid.Empty, 10, null);

            var result = _validator.Validate(cmd);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.ToWalletId));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void Validate_Should_HaveError_When_AmountNotGreaterThanZero(decimal invalidAmount)
        {
            var cmd = new CreateTransferCommand(Guid.NewGuid(), Guid.NewGuid(), invalidAmount, null);

            var result = _validator.Validate(cmd);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.Amount));
        }

        [Fact]
        public void Validate_Should_HaveError_When_DescriptionExceedsMaxLength()
        {
            var longDesc = new string('a', 501);
            var cmd = new CreateTransferCommand(Guid.NewGuid(), Guid.NewGuid(), 10, longDesc);

            var result = _validator.Validate(cmd);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(cmd.Description));
        }

        [Fact]
        public void Validate_Should_Pass_When_CommandIsValid()
        {
            var cmd = new CreateTransferCommand(Guid.NewGuid(), Guid.NewGuid(), 10, "Transfer OK");

            var result = _validator.Validate(cmd);

            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }
}