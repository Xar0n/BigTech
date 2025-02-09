using BigTech.Domain.Dto;
using BigTech.Domain.Result;
using System.Security.Claims;

namespace BigTech.Domain.Interfaces.Services;
public interface ITokenService
{
    string GenerateAcessToken(IEnumerable<Claim> claims);

    string GenerateRefreshToken();

    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

    Task<BaseResult<TokenDto>> RefreshToken(TokenDto dto);
}
