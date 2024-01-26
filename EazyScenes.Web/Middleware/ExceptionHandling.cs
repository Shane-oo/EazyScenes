using EazyScenes.Core;
using Microsoft.AspNetCore.Mvc;

namespace EazyScenes.Web.Middleware;

public class ExceptionHandling
{
    #region Fields

    private readonly ILogger<ExceptionHandling> _logger;

    private readonly RequestDelegate _next;

    #endregion

    #region Construction

    public ExceptionHandling(RequestDelegate next, ILogger<ExceptionHandling> logger)
    {
        _next = next;
        _logger = logger;
    }

    #endregion

    #region Private Methods

    private static ExceptionDetails GetExceptionDetails(Exception exception)
    {
        return exception switch
               {
                   ValidationException validationException => new ExceptionDetails(
                                                                                   StatusCodes.Status400BadRequest, "ValidationFailure",
                                                                                   "Validation error",
                                                                                   "One or more validation errors has occured",
                                                                                   validationException.ValidationErrors.ToArray()
                                                                                  ),
                   _ => new ExceptionDetails(StatusCodes.Status500InternalServerError, "ServerError", "Server Error", "An unexpected error has occured", Error.Unknown)
               };
    }

    #endregion

    #region Public Methods

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch(Exception exception)
        {
            var exceptionDetails = GetExceptionDetails(exception);
            if (exceptionDetails.Status == StatusCodes.Status500InternalServerError)
            {
                _logger.LogError(exception, "Internal Server Error Occured: Message:{Message}.\n Inner Exception: {InnerException}",
                                 exception.Message, exception.InnerException?.Message);
            }

            var problemDetails = new ProblemDetails
                                 {
                                     Status = exceptionDetails.Status,
                                     Type = exceptionDetails.Type,
                                     Title = exceptionDetails.Title,
                                     Detail = exceptionDetails.Detail,
                                     Extensions =
                                     {
                                         ["errors"] = exceptionDetails.Errors
                                     }
                                 };

            httpContext.Response.StatusCode = exceptionDetails.Status;

            await httpContext.Response.WriteAsJsonAsync(problemDetails);
        }
    }

    #endregion
}
