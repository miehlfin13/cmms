using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Synith.Security.Constants;
using Synith.Security.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Synith.Security.Services;
public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly HttpContext _httpContext;

    public TokenService(IConfiguration configuration, IHttpContextAccessor accessor)
    {
        _configuration = configuration;
        _httpContext = accessor.HttpContext!;
    }

    public string GenerateToken(IEnumerable<Claim> claims, bool isRefresh = false)
    {
        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

        DateTime expiration = isRefresh ?
            DateTime.Now.AddDays(int.Parse(_configuration["Jwt:RefreshExpirationInDays"]!))
            : DateTime.Now.AddMinutes(int.Parse(_configuration["Jwt:AccessExpirationInMinutes"]!));

        JwtSecurityToken token = new(
            claims: claims,
            expires: expiration,
            signingCredentials: credentials,
            issuer: _configuration["Jwt:Issuer"]);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool IsToBeExpired()
    {
        long exp = long.Parse(_httpContext.User.FindFirstValue(ClaimName.Expiration) ?? "0");
        DateTime expiration = DateTimeOffset.FromUnixTimeSeconds(exp).LocalDateTime;
        return expiration < DateTime.Now.AddDays(int.Parse(_configuration["Jwt:RefreshRenewalOffsetDays"]!));
    }

    public int UserId => int.Parse(_httpContext.User.FindFirstValue(ClaimName.UserId) ?? "0");
    public string Username => _httpContext.User.FindFirstValue(ClaimName.Username) ?? "";
    public IEnumerable<string> RoleIds => string.IsNullOrEmpty(_httpContext.User.FindFirstValue(ClaimName.RoleIds)) ? [] :
        _httpContext.User.FindFirstValue(ClaimName.RoleIds)!.Split(",");

}
