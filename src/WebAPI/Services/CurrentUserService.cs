using System.ComponentModel;
using System.Security.Claims;
using Application.Exceptions;
using Application.Interfaces.Services;

namespace WebAPI.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly Guid _sessionId;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor, IIdentityService identityService)
    {
        _httpContextAccessor = httpContextAccessor;

        var sessionIdString = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.PrimarySid);
        if (sessionIdString == default)
            return;

        var userIdString = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdString == default)
            throw new AuthException($"Invalid JWT");

        _sessionId = Convert<Guid>(sessionIdString);
        UserId = Convert<Guid>(userIdString);
    }

    public Guid UserId { get; }

    private static T? Convert<T>(string input)
    {
        try
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));

            if (converter.ConvertFromString(input) is T result)
                return result;

            return default;
        }
        catch (NotSupportedException)
        {
            return default;
        }
    }
}