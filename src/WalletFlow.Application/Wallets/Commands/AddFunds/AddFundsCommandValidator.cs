using FluentValidation;

namespace WalletFlow.Application.Wallets.Commands.AddFunds;

public class AddFundsCommandValidator : AbstractValidator<AddFundsCommand>
{
    public AddFundsCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("O ID do usuário é obrigatório.")
            .NotEqual(Guid.Empty).WithMessage("O ID do usuário não pode ser vazio.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("O valor a adicionar deve ser maior que zero.");
    }
}