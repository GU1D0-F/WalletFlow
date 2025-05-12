
using FluentValidation;
using MediatR;

namespace WalletFlow.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationFailures =
            await Task.WhenAll(validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

        if (validationFailures.Any(f => !f.IsValid))
            throw new ValidationException(validationFailures.SelectMany(f => f.Errors));

        var response = await next(cancellationToken);

        return response;
    }
}