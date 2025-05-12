using System.Linq.Expressions;
using WalletFlow.Domain.Entities;

namespace WalletFlow.Domain.Exceptions;

public class DomainBaseException(string message) : Exception(message)
{
    public string GetDescription() => Message;

    public string GetTitle() =>
        InnerException?.Message ?? "Uma violação de regra de negócio ocorreu.";
}

public class DomainBaseException<T> : DomainBaseException where T : BaseEntity
{
    public DomainBaseException(string businessCode)
        : base($"Domain Exception ({typeof(T).Name}): {businessCode}")
    {
    }

    public static void ThrowIfIsNullOrEmpty(Expression<Func<T, string>> propertyExpression, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            var propertyName = GetPropertyName(propertyExpression);
            throw new DomainBaseException<T>($"Invalid value for property '{propertyName}'.");
        }
    }

    public static void ThrowIfIsNull(Expression<Func<T, object>> propertyExpression, object? value)
    {
        if (value is null)
        {
            var propertyName = GetPropertyName(propertyExpression);
            throw new DomainBaseException<T>($"Property '{propertyName}' cannot be null.");
        }
    }

    private static string GetPropertyName(LambdaExpression expression)
    {
        return expression.Body switch
        {
            MemberExpression m => m.Member.Name,
            UnaryExpression u when u.Operand is MemberExpression m => m.Member.Name,
            _ => throw new InvalidOperationException("Invalid expression.")
        };
    }
}