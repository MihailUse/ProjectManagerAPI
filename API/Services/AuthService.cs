using API.Configs;
using API.Models.Auth;
using DAL.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Services;

public class AuthService
{
    private readonly UserService _userService;
    private readonly AuthConfig _authConfig;
    private readonly string _securityAlgorithm;

    public AuthService(UserService userService, IOptions<AuthConfig> authConfig)
    {
        _userService = userService;
        _authConfig = authConfig.Value;
        _securityAlgorithm = SecurityAlgorithms.HmacSha256;
    }

    public async Task<TokenModel> GetTokens(string login, string password)
    {
        var user = await _userService.GetUserByCredential(login, password);
        return _generateTokens(user);
    }

    public async Task<TokenModel> GetTokensByRefreshToken(string refreshToken)
    {
        var parameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            ValidAlgorithms = new string[] { _securityAlgorithm }
        };

        var principal = new JwtSecurityTokenHandler().ValidateToken(refreshToken, parameters, out var token);
        if (principal.FindFirst(x => x.Type == "id")?.Value is { } id && Guid.TryParse(id, out var guid))
        {
            var user = await _userService.GetUserById(guid);
            return _generateTokens(user);
        }

        throw new Exception("Invalid token");
    }

    private TokenModel _generateTokens(User user)
    {
        var jwtClaims = new Claim[]
        {
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        var refreshClaims = new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        return new TokenModel()
        {
            AccessToken = _generateEncodedToken(jwtClaims, _authConfig.LifeTime),
            RefreshToken = _generateEncodedToken(refreshClaims, _authConfig.RefreshLifeTime),
        };
    }

    private string _generateEncodedToken(IEnumerable<Claim> claims, int lifeTime)
    {
        var dateTime = DateTime.UtcNow;

        var token = new JwtSecurityToken(
            issuer: _authConfig.Issuer,
            audience: _authConfig.Audience,
            claims: claims,
            notBefore: dateTime,
            expires: dateTime.AddMinutes(lifeTime),
            signingCredentials: new SigningCredentials(_authConfig.SymmetricSecurityKey(), _securityAlgorithm)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}