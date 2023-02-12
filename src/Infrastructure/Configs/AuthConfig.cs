using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure.Configs;

public class AuthConfig
{
    public const string Position = "AuthConfig";
    public string Key { get; set; } = String.Empty;
    public string Issuer { get; set; } = String.Empty;
    public string Audience { get; set; } = String.Empty;
    public int LifeTime { get; set; }
    public int RefreshLifeTime { get; set; }

    public SymmetricSecurityKey SymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(Key));
}