using FluentValidation.TestHelper;
using WalletFlow.Application.Users.Commands.Register;

namespace WalletFlow.Integration.Tests.Auth;

public class RegisterUserCommandValidatorTests
{
    private readonly RegisterUserCommandValidator _validator = new();

        [Theory]
        [InlineData("", "Sobrenome", "user@example.com", "Senha123!")]   // Primeiro nome vazio
        [InlineData(null!, "Sobrenome", "user@example.com", "Senha123!")] // Primeiro nome nulo
        public void PrimeiroNome_Invalido_DeveFalhar(string? firstName, string lastName, string email, string password)
        {
            var cmd = new RegisterUserCommand(firstName!, lastName, email, password);
            var result = _validator.TestValidate(cmd);
            result.ShouldHaveValidationErrorFor(c => c.FirstName)
                  .WithErrorMessage("O primeiro nome é obrigatório.");
        }

        [Theory]
        [InlineData("UmNomeMuitoLongoQueExcedeOCinquentaCaracteresPorqueÉMuitíssimoExtenso", "Sobrenome", "user@example.com", "Senha123!")]
        public void PrimeiroNome_MuitoLongo_DeveFalhar(string firstName, string lastName, string email, string password)
        {
            var cmd = new RegisterUserCommand(firstName, lastName, email, password);
            var result = _validator.TestValidate(cmd);
            result.ShouldHaveValidationErrorFor(c => c.FirstName)
                  .WithErrorMessage("O primeiro nome não pode exceder 100 caracteres.");
        }

        [Theory]
        [InlineData("Nome", "", "user@example.com", "Senha123!")]   // Sobrenome vazio
        [InlineData("Nome", null, "user@example.com", "Senha123!")] // Sobrenome nulo
        public void Sobrenome_Invalido_DeveFalhar(string firstName, string? lastName, string email, string password)
        {
            var cmd = new RegisterUserCommand(firstName, lastName!, email, password);
            var result = _validator.TestValidate(cmd);
            result.ShouldHaveValidationErrorFor(c => c.LastName)
                  .WithErrorMessage("O sobrenome é obrigatório.");
        }

        [Theory]
        [InlineData("Nome", "Sobrenome", "", "Senha123!")]         // E-mail vazio
        [InlineData("Nome", "Sobrenome", "not-an-email", "Senha123!")] // E-mail inválido
        public void Email_Invalido_DeveFalhar(string firstName, string lastName, string email, string password)
        {
            var cmd = new RegisterUserCommand(firstName, lastName, email, password);
            var result = _validator.TestValidate(cmd);
            result.ShouldHaveValidationErrorFor(c => c.Email)
                  .WithErrorMessage(email == "" 
                      ? "O e-mail é obrigatório." 
                      : "Informe um e-mail válido.");
        }

        [Theory]
        [InlineData("Nome", "Sobrenome", "user@example.com", "")]    // Senha vazia
        [InlineData("Nome", "Sobrenome", "user@example.com", "12345")] // Senha muito curta
        public void Senha_Invalida_DeveFalhar(string firstName, string lastName, string email, string password)
        {
            var cmd = new RegisterUserCommand(firstName, lastName, email, password);
            var result = _validator.TestValidate(cmd);
            result.ShouldHaveValidationErrorFor(c => c.Password)
                  .WithErrorMessage(password == "" 
                      ? "A senha é obrigatória." 
                      : "A senha deve ter ao menos 6 caracteres.");
        }

        [Fact]
        public void Comando_Valido_NaoDeveFalhar()
        {
            var cmd = new RegisterUserCommand("João", "Silva", "joao.silva@example.com", "Senha123!");
            var result = _validator.TestValidate(cmd);
            result.ShouldNotHaveAnyValidationErrors();
        }
}