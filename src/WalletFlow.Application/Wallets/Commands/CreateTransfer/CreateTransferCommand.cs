using MediatR;
using WalletFlow.Shared.Dtos;

namespace WalletFlow.Application.Wallets.Commands.CreateTransfer;

public record CreateTransferCommand(
    Guid UserId,
    Guid ToWalletId,
    decimal Amount,
    string? Description = null
) : IRequest<WalletEntryDto>;