using MediatR;
using WalletFlow.Application.Repositories.Core;
using WalletFlow.Application.Wallets.Adapters;
using WalletFlow.Domain.Entities.Wallets;
using WalletFlow.Shared.Dtos;

namespace WalletFlow.Application.Wallets.Queries.GetUserTransfers;

public class GetUserTransfersQueryHandler(IWalletRepository walletRepository, IWalletEntryRepository walletEntryRepository)  : IRequestHandler<GetUserTransfersQuery, List<WalletEntryDto>>
{
    public async Task<List<WalletEntryDto>> Handle(
        GetUserTransfersQuery request,
        CancellationToken cancellationToken)
    {
        var wallet = await walletRepository.GetAsync(x => x.UserId == request.UserId);
        
        var entries = await walletEntryRepository.FindAsync(e =>
            e.WalletId == wallet!.Id
            && (e.Type == WalletEntryType.TransferSent
                || e.Type == WalletEntryType.TransferReceived)
            && (!request.From.HasValue
                || (e.CreatedAt >= request.From.Value 
                    && e.CreatedAt <= request.To!.Value)));

        return entries
            .OrderByDescending(e => e.CreatedAt)
            .Select(WalletEntryMap.Map)
            .ToList();
    }
}