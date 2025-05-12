using FluentValidation;

namespace WalletFlow.Application.Users.Commands.Register;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("O primeiro nome é obrigatório.")
            .MaximumLength(50).WithMessage("O primeiro nome não pode exceder 100 caracteres.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("O sobrenome é obrigatório.")
            .MaximumLength(50).WithMessage("O sobrenome não pode exceder 100 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("Informe um e-mail válido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(6).WithMessage("A senha deve ter ao menos 6 caracteres.");
    }
}