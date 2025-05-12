using Microsoft.EntityFrameworkCore;
using WalletFlow.Application.Users.Commands.Register;
using WalletFlow.Domain.Exceptions;
using WalletFlow.Integration.Tests.Infrastructure;
using WalletFlow.Integration.Tests.TestData;

namespace WalletFlow.Integration.Tests.Auth;

public class RegisterUserTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Handle_ShouldCreateUserAndReturnDto_WhenCommandIsValid()
    {
        // Arrange
        var firstName = Faker.Name.FirstName();
        var lastName  = Faker.Name.LastName();
        var email     = Faker.Internet.Email().ToLowerInvariant();
        var password  = "SenhaForte123!";

        var command = new RegisterUserCommand(firstName, lastName, email, password);

        // Act
        var result = await Mediator.Send(command);

        // Assert – retorno do handler
        Assert.NotNull(result);
        Assert.Equal(firstName, result.FirstName);
        Assert.Equal(lastName,  result.LastName);
        Assert.Equal(email,     result.Email);

        // Assert – persistência no banco
        var userInDb = await DbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        Assert.NotNull(userInDb);
        Assert.Equal(firstName, userInDb!.FirstName);
        Assert.Equal(lastName,  userInDb.LastName);
    }

    [Fact]
    public async Task Handle_ShouldThrowBadRequestException_WhenEmailAlreadyExists()
    {
        // Arrange: e-mail já inserido pelo TestDataSeeder
        var existingEmail = UsersDetails.Email;
        var command = new RegisterUserCommand(
            "Qualquer",
            "Usuario",
            existingEmail,
            "SenhaForte123!"
        );

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BadRequestException>(
            () => Mediator.Send(command)
        );

        Assert.Equal("E-mail já está em uso.", ex.Message);
    }
}