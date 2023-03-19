using Application.DTO.Auth;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Services;
using Domain.Entities;
using Infrastructure.Configs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using Infrastructure.Persistence;

namespace Infrastructure.Services;

public enum TokenType
{
    Access,
    Refresh
}

public class IdentityService : IIdentityService
{
    // TODO: remove database and use user service or repository 
    private readonly DatabaseContext _database;
    private readonly AuthConfig _authConfig;
    private readonly IHashGenerator _hashGenerator;

    public IdentityService(
        DatabaseContext database,
        IOptions<AuthConfig> authConfig,
        IHashGenerator hashGenerator
    )
    {
        _database = database;
        _authConfig = authConfig.Value;
        _hashGenerator = hashGenerator;
    }

    public async Task<AccessTokensDto> AuthorizeAsync(string login, string password)
    {
        var user = _database.Users.FirstOrDefault(x => x.Login == login);
        if (user == default)
            throw new NotFoundException("User not found");

        if (!_hashGenerator.Verify(user.PasswordHash, password))
            throw new AuthenticationException();

        var userSession = new UserSession(user.Id);
        await _database.UserSessions.AddAsync(userSession);
        await _database.SaveChangesAsync();

        return GenerateTokens(userSession);
    }

    public async Task<AccessTokensDto> ReAuthorizeAsync(string refreshToken)
    {
        // validate refresh token
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _authConfig.Issuer,
            ValidAudience = _authConfig.Audience,
            IssuerSigningKey = _authConfig.SymmetricSecurityKey(),
        };

        // TODO: test this validation
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(refreshToken, tokenValidationParameters, out var securityToken);
        if (securityToken == default || principal == default)
            throw new AuthException("Invalid token");

        // get refreshTokenId
        var refreshTokenId = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.PrimarySid)?.Value;
        if (refreshTokenId == default)
            throw new AuthException("Invalid token");

        // get session
        var session = _database.UserSessions.FirstOrDefault(x
            => x.RefreshTokenId == Guid.Parse(refreshTokenId) && x.IsActive);
        if (session == default)
            throw new AuthException("Session not found");

        // update RefreshTokenId
        session.RefreshTokenId = Guid.NewGuid();
        _database.UserSessions.Update(session);
        await _database.SaveChangesAsync();

        return GenerateTokens(session);
    }

    public async Task<bool> IsValidSession(Guid sessionId)
    {
        return await _database.UserSessions.AnyAsync(x => x.Id == sessionId && x.IsActive);
    }

    public AccessTokensDto GenerateTokens(UserSession userSession)
    {
        // token claims
        var tokenClaims = new[]
        {
            new Claim(ClaimTypes.PrimarySid, userSession.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, userSession.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Typ, TokenType.Access.ToString()),
        };

        // refresh tokens
        var refreshTokenClaims = new[]
        {
            new Claim(ClaimTypes.PrimarySid, userSession.RefreshTokenId.ToString()),
            new Claim(JwtRegisteredClaimNames.Typ, TokenType.Refresh.ToString()),
        };

        return new AccessTokensDto()
        {
            AccessToken = GenerateEncodedToken(tokenClaims, _authConfig.LifeTime),
            RefreshToken = GenerateEncodedToken(refreshTokenClaims, _authConfig.RefreshLifeTime),
        };
    }

    private string GenerateEncodedToken(IEnumerable<Claim> claims, int lifeTime)
    {
        var dateTime = DateTime.UtcNow;
        var token = new JwtSecurityToken(
            issuer: _authConfig.Issuer,
            audience: _authConfig.Audience,
            claims: claims,
            notBefore: dateTime,
            expires: dateTime.AddMinutes(lifeTime),
            signingCredentials: new SigningCredentials(_authConfig.SymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}