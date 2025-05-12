using MediatR;
using WalletFlow.Application.Repositories.Core;
using WalletFlow.Application.Wallets.Adapters;
using WalletFlow.Domain.Entities.Wallets;
using WalletFlow.Domain.Exceptions;
using WalletFlow.Shared.Dtos;

namespace WalletFlow.Application.Wallets.Commands.CreateTransfer;

public class CreateTransferCommandHandler(IWalletRepository walletRepositoy, IWalletEntryRepository walletEntryRepository)
    : IRequestHandler<CreateTransferCommand, WalletEntryDto>
{
    public async Task<WalletEntryDto> Handle(
        CreateTransferCommand request,
        CancellationToken cancellationToken)
    {
        var fromWallet = await walletRepositoy.GetAsync(x => x.UserId == request.UserId);
        
        var toWallet = await walletRepositoy
                           .GetAsync(w => w.Id == request.ToWalletId)
                       ?? throw new NotFoundException($"Carteira de destino {request.ToWalletId} não encontrada.");

        if (request.Amount > fromWallet!.Balance)
            throw new BadRequestException("Saldo insuficiente para realizar a transferência.");
        
        fromWallet.SubtractFunds(request.Amount);
        toWallet.AddFunds(request.Amount);
        
        var (sent, received) = WalletEntry.CreateTransfer(
            fromWallet!.Id,
            request.ToWalletId,
            request.Amount,
            request.Description
        );

        await walletRepositoy.UpdateManyAsync([fromWallet, toWallet]);
        await walletEntryRepository.CreateManyAsync([sent, received]);
        
        return WalletEntryMap.Map(sent);
    }
}