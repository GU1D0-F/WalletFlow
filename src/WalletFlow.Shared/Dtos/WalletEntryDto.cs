using WalletFlow.Domain.Entities.Wallets;

namespace WalletFlow.Shared.Dtos;

public record WalletEntryDto(
    Guid Id,
    Guid WalletId,
    string Type,
    decimal Amount,
    string? Description,
    Guid? ReferenceId,
    DateTime CreatedAt
);