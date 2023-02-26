using System.ComponentModel;
using System.Security.Claims;
using Application.Exceptions;
using Application.Interfaces.Services;

namespace WebAPI.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId
    {
        get
        {
            var claimValue = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (claimValue == default)
                throw new AuthException($"Invalid JWT");

            return Convert<Guid>(claimValue);
        }
    }

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