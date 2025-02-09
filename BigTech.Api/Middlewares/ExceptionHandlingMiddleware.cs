using BigTech.Domain.Enum;
using BigTech.Domain.Result;
using System.Net;
using ILogger = Serilog.ILogger;

namespace BigTech.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger logger)
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
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        _logger.Error(ex.Message);
        var errorMessage = ex.Message;
        var response = ex switch
        {
            UnauthorizedAccessException _ => 
                new BaseResult()
                {
                    ErrorMessage = errorMessage,
                    ErrorCode = (int)HttpStatusCode.Unauthorized,
                },
            _ => new BaseResult()
                {
                    ErrorMessage = errorMessage,
                    ErrorCode = (int)HttpStatusCode.InternalServerError,
            }
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)response.ErrorCode;
        await context.Response.WriteAsJsonAsync(response);
    }
}
