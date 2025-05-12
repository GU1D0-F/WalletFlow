using WalletFlow.Domain.Entities.Users;

namespace WalletFlow.Application.Services.Identity.JwtToken;

public interface IJwtTokenService
{
    /// <summary>
    /// Gera um JWT a partir de um usuário autenticado.
    /// </summary>
    /// <param name="user">Usuário Identity</param>
    /// <returns>Token JWT</returns>
    (string Token, DateTime ExpiresAt) GenerateToken(User user);
}