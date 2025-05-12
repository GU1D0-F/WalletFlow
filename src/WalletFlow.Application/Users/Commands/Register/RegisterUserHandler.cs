using MediatR;
using Microsoft.AspNetCore.Identity;
using WalletFlow.Application.Repositories.Core;
using WalletFlow.Application.Users.Adapters;
using WalletFlow.Domain.Entities.Users;
using WalletFlow.Domain.Entities.Wallets;
using WalletFlow.Domain.Exceptions;
using WalletFlow.Shared.Dtos;

namespace WalletFlow.Application.Users.Commands.Register;

public class RegisterUserHandler(UserManager<User> userManager, IWalletRepository walletRepository)
    : IRequestHandler<RegisterUserCommand, UserDto>
{
    public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        if (await userManager.FindByEmailAsync(email) is not null)
            throw new BadRequestException("E-mail já está em uso.");

        var user = User.Create(request.FirstName.Trim(), request.LastName.Trim(), request.Email);

        var result = await userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            await CreateWallet(user);
            return UserMap.Map(user);
        }

        var errorMessages = result.Errors.Select(e => e.Description).ToArray();
        throw new BadRequestException("Falha ao registrar usuário.", errorMessages);
    }

    private async Task CreateWallet(User user)
    {
        var wallet = Wallet.Create(user.Id);
        user.SetWallet(wallet);

        await walletRepository.CreateAsync(wallet);
    }
}