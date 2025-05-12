using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WalletFlow.Application.Users.Adapters;
using WalletFlow.Domain.Entities.Users;
using WalletFlow.Domain.Exceptions;
using WalletFlow.Shared.Dtos;

namespace WalletFlow.Application.Users.Queries.GetProfile;

public class GetProfileHandler(
    UserManager<User> userManager, 
    IHttpContextAccessor accessor) : IRequestHandler<GetProfileQuery, UserDto>
{
    public async Task<UserDto> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var userId = accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
            throw new UnauthorizedException("Usuário não autenticado.");

        var user = await userManager.Users
            .Include(u => u.Wallet)
            .FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId), cancellationToken);

        if (user == null)
            throw new NotFoundException("Usuário não encontrado.");

        return UserMap.Map(user);
    }
}