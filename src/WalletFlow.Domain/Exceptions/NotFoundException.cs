namespace WalletFlow.Domain.Exceptions;

public class NotFoundException(string message) : DomainBaseException(message);