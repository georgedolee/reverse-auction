using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SharedInfrastructure.ExceptionHandling;
using SharedKernel.Exceptions;
using ValidationException = FluentValidation.ValidationException;

namespace SharedInfrastructure.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ForbiddenException ex)
        {
            _logger.LogWarning(ex, "Access forbidden: {Message}", ex.Message);
            await ExceptionHandler.HandleForbiddenExceptionAsync(context, ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Unauthrorized access: {Message}", ex.Message);
            await ExceptionHandler.HandleUnauthorizedAccessExceptionAsync(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation failed: {Errors}", ex.Errors);
            await ExceptionHandler.HandleValidationExceptionAsync(context, ex);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Resource not found: {Message}", ex.Message);
            await ExceptionHandler.HandleNotFoundExceptionAsync(context, ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Invalid operation: {Message}", ex.Message);
            await ExceptionHandler.HandleInvalidOperationExceptionAsync(context, ex);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Argument exception: {Message}", ex.Message);
            await ExceptionHandler.HandleArgumentExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");
            await ExceptionHandler.HandleUnexpectedExceptionAsync(context);
        }
    }
}
