using FluentValidation;

namespace WalletFlow.Application.Wallets.Queries.GetUserTransfers;

public class GetUserTransfersQueryValidator : AbstractValidator<GetUserTransfersQuery>
{
    public GetUserTransfersQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("'UserId' é obrigatório.");
        
        RuleFor(x => x.To)
            .NotNull()
            .When(x => x.From.HasValue)
            .WithMessage("Se informar 'From', deve informar também 'To'.");
        
        RuleFor(x => x.From)
            .NotNull()
            .When(x => x.To.HasValue)
            .WithMessage("Se informar 'To', deve informar também 'From'.");
        
        When(x => x.From.HasValue && x.To.HasValue, () =>
        {
            RuleFor(x => x)
                .Must(x => x.From <= x.To)
                .WithMessage("'From' não pode ser maior que 'To'.");
        });
    }
}