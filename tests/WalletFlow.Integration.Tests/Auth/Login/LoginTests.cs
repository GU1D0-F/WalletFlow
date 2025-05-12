using FluentAssertions;
using WalletFlow.Application.Users.Commands.Login;
using WalletFlow.Domain.Exceptions;
using WalletFlow.Integration.Tests.Infrastructure;
using WalletFlow.Integration.Tests.TestData;
using WalletFlow.Shared.Models.Users.Responses;

namespace WalletFlow.Integration.Tests.Auth.Login;

public class LoginTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    private const string CorrectPassword = "SenhaForte123!";

    [Fact]
    public async Task Handle_DeveRetornarToken_QuandoCredenciaisForemValidas()
    {
        // Arrange
        var email = UsersDetails.Email;
        var command = new LoginCommand(email, CorrectPassword);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<LoginResultDto>();
        result.Token.Should().NotBeNullOrWhiteSpace();
        result.ExpiresAt.Should().BeAfter(DateTime.UtcNow);
    }

    [Fact]
    public async Task Handle_DeveLancarUnauthorizedException_QuandoEmailNaoExistir()
    {
        // Arrange
        var command = new LoginCommand("nao.existe@example.com", CorrectPassword);

        // Act
        Func<Task> act = () => Mediator.Send(command);

        // Assert
        await act.Should()
            .ThrowAsync<UnauthorizedException>();
    }

    [Fact]
    public async Task Handle_DeveLancarUnauthorizedException_QuandoSenhaEstiverIncorreta()
    {
        // Arrange
        var email = UsersDetails.Email;
        var command = new LoginCommand(email, "SenhaErrada!");

        // Act
        Func<Task> act = () => Mediator.Send(command);

        // Assert
        await act.Should()
            .ThrowAsync<UnauthorizedException>();
    }
}