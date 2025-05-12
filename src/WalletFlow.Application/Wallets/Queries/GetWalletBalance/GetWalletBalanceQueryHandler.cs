using MediatR;
using WalletFlow.Application.Repositories.Core;
using WalletFlow.Domain.Exceptions;

namespace WalletFlow.Application.Wallets.Queries.GetWalletBalance;

public class GetWalletBalanceQueryHandler(IWalletRepository repository): IRequestHandler<GetWalletBalanceQuery, decimal>
{
    public async Task<decimal> Handle(GetWalletBalanceQuery request, CancellationToken cancellationToken)
    {
        var wallet = await repository.GetAsync(w => w.UserId == request.UserId) ?? throw new NotFoundException($"Carteira não encontrada para o usuário {request.UserId}");

        return wallet.Balance;
    }
}