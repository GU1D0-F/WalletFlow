using MediatR;
using Microsoft.AspNetCore.Identity;
using WalletFlow.Application.Services.Identity.JwtToken;
using WalletFlow.Domain.Entities.Users;
using WalletFlow.Domain.Exceptions;
using WalletFlow.Shared.Models.Users.Responses;

namespace WalletFlow.Application.Users.Commands.Login;

public class LoginHandler(SignInManager<User> signInManager,
                          UserManager<User> userManager, 
                          IJwtTokenService jwtService)  : IRequestHandler<LoginCommand, LoginResultDto>
{
    public async Task<LoginResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
            throw new UnauthorizedException();

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
        
        if (!result.Succeeded)
            throw new UnauthorizedException();

        var (token, expiresAt) = jwtService.GenerateToken(user);
        return new LoginResultDto(token, expiresAt);
    }
}