using System.Security.Claims;

namespace Synith.Security.Interfaces;
public interface ITokenService
{
    int UserId { get; }
    string Username { get; }
    IEnumerable<string> RoleIds { get; }

    string GenerateToken(IEnumerable<Claim> claims, bool isRefresh = false);
    bool IsToBeExpired();
}