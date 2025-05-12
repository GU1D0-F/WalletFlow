using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using WalletFlow.Domain.Exceptions;
using WalletFlow.Infrastructure.Extensions;

namespace WalletFlow.Infrastructure.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var problemDetails = exception switch
        {
            DomainBaseException domainEx => CreateProblemDetails(context, domainEx),
            ValidationException validationEx => CreateProblemDetails(context, validationEx),
            _ => new CustomProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal error",
                Detail = exception.Message,
                Instance = context.Request.Path
            }
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;

        var json = JsonSerializer.Serialize(problemDetails);
        return context.Response.WriteAsync(json);
    }
    
    private static CustomProblemDetails CreateProblemDetails(HttpContext context, ValidationException exception)
    {
        var errors = exception.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        return new CustomProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation Error",
            Detail = "One or more validation errors occurred.",
            Instance = context.Request.Path,
            Errors = errors
        };
    }

    private static CustomProblemDetails CreateProblemDetails(HttpContext context, DomainBaseException exception)
    {
        var statusCode = exception.GetStatusCode();
        return new CustomProblemDetails
        {
            Status = statusCode,
            Title = exception.GetTitle(),
            Detail = exception.GetDescription(),
            Instance = context.Request.Path
        };
    }
}