using MediatR;

namespace WalletFlow.Application.Wallets.Queries.GetWalletBalance;

public record GetWalletBalanceQuery(Guid UserId) : IRequest<decimal>;