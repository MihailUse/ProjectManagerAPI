using Application.Exceptions;
using Application.Interfaces.Services;
using Infrastructure.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebAPI.Middlewares;

public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    public AuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, IIdentityService identityService)
    {
        if (httpContext.User.Claims.Any())
        {
            var tokenType = httpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Typ)?.Value;
            var sessionId = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.PrimarySid)?.Value;
            if (tokenType == default || sessionId == default)
                throw new AuthException("Invalid token");

            if (Enum.Parse<TokenType>(tokenType) == TokenType.Refresh)
                throw new AuthException("Invalid token type");

            if (!await identityService.IsValidSession(Guid.Parse(sessionId)))
                throw new AuthException("Invalid token");
        }

        await _next(httpContext);
    }
}

public static class AuthorizationMiddlewareExtensions
{
    public static IApplicationBuilder UseAuthorizationMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthorizationMiddleware>();
    }
}