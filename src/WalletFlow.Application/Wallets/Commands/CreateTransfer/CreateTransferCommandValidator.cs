using FluentValidation;
using WalletFlow.Application.Repositories.Core;

namespace WalletFlow.Application.Wallets.Commands.CreateTransfer;

public class CreateTransferCommandValidator : AbstractValidator<CreateTransferCommand>
{
    public CreateTransferCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("'UserId' é obrigatório.");
        
        RuleFor(x => x.ToWalletId)
            .NotEmpty()
            .WithMessage("'ToWalletId' é obrigatório.");
        
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("'Amount' deve ser maior que zero.");
        
        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("'Description' não pode exceder 500 caracteres.")
            .When(x => x.Description != null);
    }
}