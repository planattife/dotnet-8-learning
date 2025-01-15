using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace APIProductCatalog.Services;

public interface ITokenService
{
    JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _config);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincialFromExpiredToken(string token, IConfiguration _config);
}
