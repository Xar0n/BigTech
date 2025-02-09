using BigTech.Application.Resources;
using BigTech.Domain.Dto;
using BigTech.Domain.Entity;
using BigTech.Domain.Interfaces.Repositories;
using BigTech.Domain.Interfaces.Services;
using BigTech.Domain.Result;
using BigTech.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BigTech.Application.Services;
public class TokenService : ITokenService
{
    private readonly string _jwtKey;
    private readonly string _issuer;
    private readonly string _audience;

    private readonly IBaseRepository<User> _userRepository;

    public TokenService(IOptions<JwtSettings> options, 
        IBaseRepository<User> userRepository)
    {
        _jwtKey = options.Value.JwtKey;
        _issuer = options.Value.Issuer;
        _audience = options.Value.Audience;

        _userRepository = userRepository;
    }

    public string GenerateAcessToken(IEnumerable<Claim> claims)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var securityToken = new JwtSecurityToken(
            _issuer,
            _audience,
            claims,
            null,
            DateTime.UtcNow.AddMinutes(10),
            credentials
        );
        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return token;
    }

    public string GenerateRefreshToken()
    {
        var randomNumbers = new byte[32];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumbers);
        return Convert.ToBase64String(randomNumbers);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey)),
            ValidateLifetime = true,
            ValidAudience = _audience,
            ValidIssuer = _issuer
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken 
            || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException(ErrorMessage.InvalidToken);
        }
        return claimsPrincipal;
    }

    public async Task<BaseResult<TokenDto>> RefreshToken(TokenDto dto)
    {
        string acessToken = dto.AcessToken;
        string refreshToken = dto.RefreshToken;

        var claimsPrincipal = GetPrincipalFromExpiredToken(acessToken);
        var userName = claimsPrincipal.Identity?.Name;

        var user = await _userRepository.GetAll()
            .Include(ut => ut.UserToken)
            .FirstOrDefaultAsync(u => u.Login == userName);

        if (user == null || 
            user.UserToken.RefreshToken != dto.RefreshToken ||
            user.UserToken.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return new BaseResult<TokenDto>()
            {
                ErrorMessage = ErrorMessage.InvalidClientRequest
            };
        }

        var newAccessToken = GenerateAcessToken(claimsPrincipal.Claims);
        var newRefreshToken = GenerateRefreshToken();

        user.UserToken.RefreshToken = newRefreshToken;
        await _userRepository.UpdateAsync(user);

        return new BaseResult<TokenDto>()
        {
            Data = new TokenDto()
            {
                AcessToken = acessToken,
                RefreshToken = refreshToken,
            }
        };
    }
}
