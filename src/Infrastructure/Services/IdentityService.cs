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

namespace Infrastructure.Services;

public enum TokenType
{
    Access,
    Refresh
}

public class IdentityService : IIdentityService
{
    private readonly IDatabaseContext _context;
    private readonly AuthConfig _authConfig;
    private readonly ImageGeneratorConfig _imageConfig;
    private readonly IHashGenerator _hashGenerator;
    private readonly IImageGenerator _imageGenerator;

    public IdentityService(
        IDatabaseContext context,
        IOptions<AuthConfig> authConfig,
        IOptions<ImageGeneratorConfig> imageConfig,
        IHashGenerator hashGenerator,
        IImageGenerator imageGenerator
    )
    {
        _context = context;
        _authConfig = authConfig.Value;
        _imageConfig = imageConfig.Value;
        _hashGenerator = hashGenerator;
        _imageGenerator = imageGenerator;
    }

    public async Task<AccessTokensDto> CreateUserAsync(string login, string password)
    {
        // create user
        var userSession = new UserSession();
        var avatar = _imageGenerator.GenerateImage(
            _imageConfig.PixelsInWidth,
            _imageConfig.PixelsInHeight,
            _imageConfig.CountColor,
            _imageConfig.WhiteFrequency
        );
        var user = new User()
        {
            Login = login,
            Avatar = avatar,
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
        var user = _context.Users.FirstOrDefault(x => x.Login == login);
        if (user == default)
            throw new NotFoundException("User not found");

        if (!_hashGenerator.Verify(user.PasswordHash, password))
            throw new AuthenticationException();

        var userSession = new UserSession(user.Id);
        await _context.UserSessions.AddAsync(userSession);
        await _context.SaveChangesAsync();

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
        var refreshTokenId = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (refreshTokenId == default)
            throw new AuthException("Invalid token");

        // get session
        var session = _context.UserSessions.FirstOrDefault(x
            => x.RefreshTokenId == Guid.Parse(refreshTokenId) && x.IsActive);
        if (session == default)
            throw new AuthException("Session not found");

        // update RefreshTokenId
        session.RefreshTokenId = Guid.NewGuid();
        _context.UserSessions.Update(session);
        await _context.SaveChangesAsync();

        return GenerateTokens(session);
    }

    public async Task<bool> IsValidSession(Guid sessionId)
    {
        return await _context.UserSessions.AnyAsync(x => x.Id == sessionId && x.IsActive);
    }

    private AccessTokensDto GenerateTokens(UserSession userSession)
    {
        // token claims
        var tokenClaims = new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, userSession.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Typ, TokenType.Access.ToString()),
        };

        // refresh tokens
        var refreshTokenClaims = new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, userSession.RefreshTokenId.ToString()),
            new Claim(JwtRegisteredClaimNames.Typ, TokenType.Refresh.ToString()),
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
            signingCredentials: new SigningCredentials(_authConfig.SymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}