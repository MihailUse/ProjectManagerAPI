using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace API.Configs
{
    public class AuthConfig
    {
        public const string Position = "Auth";
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public DateTimeOffset LifeTime { get; set; }

        public SymmetricSecurityKey SymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(Key));
    }
}