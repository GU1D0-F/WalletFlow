using FluentValidation.TestHelper;
using WalletFlow.Application.Users.Commands.Login;

namespace WalletFlow.Integration.Tests.Auth.Login;

public class LoginCommandValidatorTests
{
    private readonly LoginCommandValidator _validator = new();

    [Theory]
    [InlineData("", "Senha123!")]     // e-mail vazio
    [InlineData("not-an-email", "Senha123!")] // e-mail inválido
    public void Email_Invalido_DeveFalhar(string email, string password)
    {
        var cmd = new LoginCommand(email, password);
        var result = _validator.TestValidate(cmd);

        result.ShouldHaveValidationErrorFor(c => c.Email)
            .WithErrorMessage(
                string.IsNullOrWhiteSpace(email)
                    ? "O e-mail é obrigatório."
                    : "Informe um e-mail válido."
            );
    }

    [Theory]
    [InlineData("user@example.com", "")]     // senha vazia
    [InlineData("user@example.com", "12345")] // senha muito curta
    public void Password_Invalida_DeveFalhar(string email, string password)
    {
        var cmd = new LoginCommand(email, password);
        var result = _validator.TestValidate(cmd);

        result.ShouldHaveValidationErrorFor(c => c.Password)
            .WithErrorMessage(
                string.IsNullOrWhiteSpace(password)
                    ? "A senha é obrigatória."
                    : "A senha deve ter ao menos 6 caracteres."
            );
    }

    [Fact]
    public void Comando_Valido_NaoDeveFalhar()
    {
        var cmd = new LoginCommand("joao.silva@example.com", "Senha123!");
        var result = _validator.TestValidate(cmd);
        result.ShouldNotHaveAnyValidationErrors();
    }
}