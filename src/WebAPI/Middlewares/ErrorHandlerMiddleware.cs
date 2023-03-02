using Application.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception e)
        {
            ActionResult result = e switch
            {
                InvalidOperationException => new BadRequestObjectResult(e.Message),
                AccessDeniedException => new ForbidResult(JwtBearerDefaults.AuthenticationScheme),
                NotFoundException => new NotFoundObjectResult(e.Message),
                ConflictException => new ConflictObjectResult(e.Message),
                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            };

            await result.ExecuteResultAsync(new ActionContext
            {
                HttpContext = httpContext
            });
        }
    }
}

public static class ErrorHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseErrorHandlerMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlerMiddleware>();
    }
}