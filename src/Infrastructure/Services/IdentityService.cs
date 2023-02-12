using Application.Common.DTO.Auth;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Configs;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;

namespace Infrastructure.Services;

internal class IdentityService : IIdentityService
{
    private readonly IDatabaseContext _context;
    private readonly AuthConfig _authConfig;
    private readonly IHashGeneratorService _hashGenerator;

    public IdentityService(IDatabaseContext context, IOptions<AuthConfig> authConfig, IHashGeneratorService hashGenerator)
    {
        _context = context;
        _authConfig = authConfig.Value;
        _hashGenerator = hashGenerator;
    }

    public async Task<AccessTokensDto> CreateUserAsync(string login, string password)
    {
        // create user
        var userSession = new UserSession();
        var user = new User()
        {
            Login = login,
            PasswordHash = _hashGenerator.GetHash(password),
            UserSessions = new List<UserSession>()
            {
                userSession
            }
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return GenerateTokens(userSession);
    }

    public async Task<AccessTokensDto> AuthorizeAsync(string login, string password)
    {
        var user = _context.Users.FirstOrDefault(x => x.Login == login && !x.DeletedAt.HasValue);
        if (user == default)
            throw new NotFoundException("User not found");

        if (!_hashGenerator.Verify(user.PasswordHash, password))
            throw new AuthenticationException();

        var userSession = new UserSession(user.Id);
        await _context.UserSessions.AddAsync(userSession);
        await _context.SaveChangesAsync();

        return GenerateTokens(userSession);
    }

    public async Task<AccessTokensDto> ReAuthorizeAsync(Guid refreshTokenId)
    {
        var userSession = _context.UserSessions
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefault(x => x.RefreshTokenId == refreshTokenId && x.IsActive);

        if (userSession == default)
            throw new NotFoundException("Session not found");

        // update RefreshTokenId
        userSession.RefreshTokenId = Guid.NewGuid();
        _context.UserSessions.Update(userSession);
        await _context.SaveChangesAsync();

        return GenerateTokens(userSession);
    }

    private AccessTokensDto GenerateTokens(UserSession userSession)
    {
        // token claims
        var tokenClaims = new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, userSession.Id.ToString()),
        };

        // refresh tokens
        var refreshTokenClaims = new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, userSession.RefreshTokenId.ToString()),
        };

        return new AccessTokensDto()
        {
            AccessToken = GenerateEncodedToken(tokenClaims, _authConfig.LifeTime),
            RefreshToken = GenerateEncodedToken(refreshTokenClaims, _authConfig.RefreshLifeTime),
        };
    }

    private string GenerateEncodedToken(Claim[] claims, int lifeTime)
    {
        var dateTime = DateTime.UtcNow;
        var token = new JwtSecurityToken(
            issuer: _authConfig.Issuer,
            audience: _authConfig.Audience,
            claims: claims,
            notBefore: dateTime,
            expires: dateTime.AddMinutes(lifeTime),
            signingCredentials: new SigningCredentials(_authConfig.SymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
