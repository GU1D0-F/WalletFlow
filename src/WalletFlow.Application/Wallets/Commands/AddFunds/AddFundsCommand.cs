using MediatR;

namespace WalletFlow.Application.Wallets.Commands.AddFunds;

public record AddFundsCommand(Guid UserId, decimal Amount) : IRequest;