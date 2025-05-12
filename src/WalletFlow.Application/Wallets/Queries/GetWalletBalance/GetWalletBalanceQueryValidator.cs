using FluentValidation;

namespace WalletFlow.Application.Wallets.Queries.GetWalletBalance;

public class GetWalletBalanceQueryValidator : AbstractValidator<GetWalletBalanceQuery>
{
    public GetWalletBalanceQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("O ID do usuário é obrigatório.")
            .NotEqual(Guid.Empty).WithMessage("O ID do usuário não pode ser vazio.");
    }
}