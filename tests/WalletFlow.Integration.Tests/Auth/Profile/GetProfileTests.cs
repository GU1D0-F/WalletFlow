using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using WalletFlow.Application.Users.Queries.GetProfile;
using WalletFlow.Domain.Exceptions;
using WalletFlow.Integration.Tests.Infrastructure;
using WalletFlow.Shared.Dtos;

namespace WalletFlow.Integration.Tests.Auth.Profile;

public class GetProfileTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Handle_ShouldReturnUserDto_WhenUserIsAuthenticated()
    {
        // Arrange: monta um HttpContext com o Claim de NameIdentifier
        var userId = PrincipalUser.Id.ToString();
        HttpContextAccessor.HttpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(
                new ClaimsIdentity([
                    new Claim(ClaimTypes.NameIdentifier, userId)
                ], "TestAuth"))
        };

        // Act
        var result = await Mediator.Send(new GetProfileQuery());

        // Assert
        Assert.NotNull(result);
        Assert.IsType<UserDto>(result);
        Assert.Equal(PrincipalUser.FirstName, result.FirstName);
        Assert.Equal(PrincipalUser.LastName, result.LastName);
        Assert.Equal(PrincipalUser.Email, result.Email);
    }

    [Fact]
    public async Task Handle_ShouldThrowUnauthorizedException_WhenNoUserClaim()
    {
        // Arrange: HttpContext sem Claims
        HttpContextAccessor.HttpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity()) // sem ClaimTypes.NameIdentifier
        };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(
            () => Mediator.Send(new GetProfileQuery())
        );
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange: Claim com id que não está no banco
        var fakeId = Guid.NewGuid().ToString();
        HttpContextAccessor.HttpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(
                new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, fakeId)
                }, "TestAuth"))
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => Mediator.Send(new GetProfileQuery())
        );
    }
}