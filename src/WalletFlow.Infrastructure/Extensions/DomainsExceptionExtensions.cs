using Microsoft.AspNetCore.Http;
using WalletFlow.Domain.Exceptions;

namespace WalletFlow.Infrastructure.Extensions;

public static class DomainsExceptionExtensions
{
    public static int GetStatusCode(this DomainBaseException exception) => exception switch
    {
        BadRequestException => StatusCodes.Status400BadRequest,
        NotFoundException => StatusCodes.Status404NotFound,
        UnauthorizedException => StatusCodes.Status401Unauthorized,
        _ => StatusCodes.Status422UnprocessableEntity
    };
}