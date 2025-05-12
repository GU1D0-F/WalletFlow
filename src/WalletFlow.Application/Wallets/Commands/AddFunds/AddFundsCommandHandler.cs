using MediatR;
using WalletFlow.Application.Repositories.Core;
using WalletFlow.Domain.Exceptions;

namespace WalletFlow.Application.Wallets.Commands.AddFunds;

public class AddFundsCommandHandler(IWalletRepository repository) : IRequestHandler<AddFundsCommand>
{
    public async Task Handle(AddFundsCommand request, CancellationToken cancellationToken)
    {
        var wallet = await repository.GetAsync(w => w.UserId == request.UserId) ?? throw new NotFoundException($"Carteira não encontrada para o usuário {request.UserId}");
        
        wallet.AddFunds(request.Amount);
        
        await repository.UpdateAsync(wallet);
    }
}