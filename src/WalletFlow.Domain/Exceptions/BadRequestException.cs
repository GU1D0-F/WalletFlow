namespace WalletFlow.Domain.Exceptions;


public class BadRequestException(string message, string[]? errors = null) : DomainBaseException(message)
{
    public string[] Errors { get; } = errors ?? [];
}