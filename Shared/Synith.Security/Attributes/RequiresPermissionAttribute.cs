using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Synith.Caching;
using Synith.Security.Interfaces;
using System.Text.Json;

namespace Synith.Security.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class RequiresPermissionAttribute : Attribute, IAsyncAuthorizationFilter
{
    public string PermissionCode { get; }

    public RequiresPermissionAttribute(string permissionCode)
    {
        PermissionCode = permissionCode;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var client = context.HttpContext.RequestServices.GetRequiredService<IHttpClientFactory>().CreateClient();
        var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var cache = context.HttpContext.RequestServices.GetRequiredService<ICache>();
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<RequiresPermissionAttribute>>();
        var token = context.HttpContext.RequestServices.GetRequiredService<ITokenService>();

        logger.LogDebug("Validating permission({permissionCode}).", PermissionCode);

        string bearer = context.HttpContext.Request.Headers.Authorization
            .ToString().Replace("Bearer ", "");
        client.DefaultRequestHeaders.Authorization = new("Bearer", bearer);

        IEnumerable<string> roleIds = token.RoleIds;
        if (!roleIds.Any())
        {
            logger.LogWarning("No assigned role for user({username}).", token.Username);
            context.Result = new ForbidResult();
            return;
        }

        foreach (string roleId in roleIds)
        {
            IEnumerable<string> permissions = await RetrieveRolePermissionsAsync(int.Parse(roleId));

            if (permissions.Contains(PermissionCode))
            {
                logger.LogDebug("User({username}) has permission({permissionCode}).", token.Username, PermissionCode);
                return;
            }
        }

        logger.LogDebug("User({username}) has no permission({permissionCode}).", token.Username, PermissionCode);
        context.Result = new ForbidResult();

        #region local functions
        async Task<IEnumerable<string>> RetrieveRolePermissionsAsync(int roleId)
        {
            IEnumerable<IDictionary<string, object>>? permissions = null;
            string key = CacheKey.RolePermissions.Replace("{roleId}", roleId.ToString());

            logger.LogDebug("Retrieving role permissions from cache.");
            permissions = cache.Get<IEnumerable<IDictionary<string, object>>>(key);

            if (permissions == null)
            {
                logger.LogDebug("Retrieving role permissions from api endpoint.");
                string baseUri = $"{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}";
                string uri = $"{baseUri}/UserAccount/Role/Permissions/{roleId}";
                HttpResponseMessage response = await client.GetAsync(uri);
                
                string json = await response.Content.ReadAsStringAsync();
                permissions = JsonSerializer.Deserialize<IEnumerable<IDictionary<string, object>>>(json, _jsonOptions)!;

                logger.LogDebug("Saving role permissions to cache.");
                cache.Set(key, permissions);
            }

            return permissions.Select(x => x["code"].ToString())!;
        }
        #endregion
    }

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
}
