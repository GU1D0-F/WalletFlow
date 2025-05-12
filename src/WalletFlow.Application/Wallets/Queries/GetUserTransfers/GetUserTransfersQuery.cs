using MediatR;
using WalletFlow.Shared.Dtos;

namespace WalletFlow.Application.Wallets.Queries.GetUserTransfers;

public record GetUserTransfersQuery : IRequest<List<WalletEntryDto>>
{
    public Guid UserId { get; }
    public DateTime? From { get; }
    public DateTime? To { get; }

    public GetUserTransfersQuery(
        Guid userId,
        DateTime? from = null,
        DateTime? to = null)
    {
        if (from.HasValue ^ to.HasValue)
            throw new ArgumentException("Se informar 'From', deve informar também 'To' (e vice-versa).");

        if (from.HasValue && to.HasValue && from > to)
            throw new ArgumentException("'From' não pode ser maior que 'To'.");

        UserId = userId;
        From = from;
        To = to;
    }
}