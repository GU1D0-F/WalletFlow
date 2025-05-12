namespace WalletFlow.Domain.Exceptions;

public class UnauthorizedException(string message = "Credenciais inválidas.") : DomainBaseException(message);