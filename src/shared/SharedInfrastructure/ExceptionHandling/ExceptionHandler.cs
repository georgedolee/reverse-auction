using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;
using SharedKernel.Exceptions;

namespace SharedInfrastructure.ExceptionHandling;

public static class ExceptionHandler
{
    public static async Task HandleValidationExceptionAsync(HttpContext context, ValidationException ex)
    {
        var validationErrors = ex.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        var problemDetails = new ValidationProblemDetails(validationErrors)
        {
            Type = ProblemTypeUrls.BadRequest,
            Title = "One or more validation errors occurred.",
            Status = StatusCodes.Status400BadRequest,
            Detail = "See the errors property for details.",
            Instance = context.Request.Path
        };

        await WriteProblemResponseAsync(
            context: context, 
            problemDetails: problemDetails, 
            statusCode: StatusCodes.Status400BadRequest);
    }

    public static Task HandleNotFoundExceptionAsync(HttpContext context, NotFoundException ex) =>
        WriteSimpleProblemAsync(
            context: context, 
            statusCode: StatusCodes.Status404NotFound, 
            title: "Resource not found.", 
            detail: ex.Message, 
            type: ProblemTypeUrls.NotFound);

    public static Task HandleInvalidOperationExceptionAsync(HttpContext context, InvalidOperationException ex) =>
        WriteSimpleProblemAsync(
            context: context, 
            statusCode: StatusCodes.Status400BadRequest, 
            title: "Invalid operation.", 
            detail: ex.Message, 
            type: ProblemTypeUrls.BadRequest);


    public static Task HandleUnauthorizedAccessExceptionAsync(HttpContext context) =>
        WriteSimpleProblemAsync(
            context: context, 
            statusCode: StatusCodes.Status401Unauthorized, 
            title: "Unauthorized", 
            detail: "Authentication is required or the provided token is invalid.", 
            type: ProblemTypeUrls.Unauthorized);

    public static Task HandleForbiddenExceptionAsync(HttpContext context, ForbiddenException ex) =>
        WriteSimpleProblemAsync(
            context: context, 
            statusCode: StatusCodes.Status403Forbidden, 
            title: "Forbidden", 
            detail: ex.Message, 
            type: ProblemTypeUrls.Forbidden);

    public static Task HandleRateLimitExceptionAsync(HttpContext context) =>
        WriteSimpleProblemAsync(
            context: context, 
            statusCode: StatusCodes.Status429TooManyRequests, 
            title: "Too Many Requests", 
            detail: "You have exceeded the allowed number of requests. Please try again later.", 
            type: ProblemTypeUrls.TooManyRequests);

    public static Task HandleArgumentExceptionAsync(HttpContext context, ArgumentException ex) =>
        WriteSimpleProblemAsync(
            context: context, 
            statusCode: StatusCodes.Status400BadRequest, 
            title: "Invalid argument.", 
            detail: ex.Message, 
            type: ProblemTypeUrls.BadRequest);

    public static Task HandleUnexpectedExceptionAsync(HttpContext context) =>
        WriteSimpleProblemAsync(
            context: context, 
            statusCode: StatusCodes.Status500InternalServerError, 
            title: "An unexpected error occurred.", 
            detail: "Please try again later or contact support.", 
            type: ProblemTypeUrls.InternalServerError);

    private static Task WriteSimpleProblemAsync(HttpContext context, int statusCode, string title, string detail, string type) =>
        WriteProblemResponseAsync(context, new ProblemDetails
        {
            Type = type,
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        }, statusCode);

    private static Task WriteProblemResponseAsync(HttpContext context, ProblemDetails problemDetails, int statusCode)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";
        return context.Response.WriteAsJsonAsync(problemDetails);
    }
}
