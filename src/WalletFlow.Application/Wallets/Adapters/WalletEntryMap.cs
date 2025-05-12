using WalletFlow.Domain.Entities.Wallets;
using WalletFlow.Shared.Converters;
using WalletFlow.Shared.Dtos;

namespace WalletFlow.Application.Wallets.Adapters;

public static class WalletEntryMap
{
    public static WalletEntryDto Map(WalletEntry entry) =>
        new WalletEntryDto(
            entry.Id,
            entry.WalletId,
            entry.Type.ToDescription(),
            entry.Amount,
            entry.Description,
            entry.ReferenceId,
            entry.CreatedAt
        );
}