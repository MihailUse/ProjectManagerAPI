using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace API.Configs
{
    public class AuthConfig
    {
        public const string Position = "Auth";
        public string Key { get; set; } = String.Empty;
        public string Issuer { get; set; } = String.Empty;
        public string Audience { get; set; } = String.Empty;
        public int LifeTime { get; set; }
        public int RefreshLifeTime { get; set; }

        public SymmetricSecurityKey SymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(Key));
    }
}