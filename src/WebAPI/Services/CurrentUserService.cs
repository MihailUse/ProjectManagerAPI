using System.ComponentModel;
using System.Security.Claims;
using Application.Interfaces.Services;

namespace WebAPI.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetUserId(bool isRequired = true)
    {
        string? claimValue = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (claimValue == default)
            return isRequired ? throw new Exception($"Invalid JWT") : default;

        return Convert<Guid>(claimValue);
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