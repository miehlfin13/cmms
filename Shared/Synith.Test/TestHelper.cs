using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Synith.Security.Attributes;
using System.Reflection;

namespace Synith.Test;
public static class TestHelper
{
    public static IRouteTemplateProvider[]? GetEndpointTemplate<T>(string methodName) where T : ControllerBase
    {
        MethodInfo methodInfo = typeof(T).GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public)!;
        return methodInfo.GetCustomAttributes(typeof(IRouteTemplateProvider), true) as IRouteTemplateProvider[];
    }

    public static RequiresPermissionAttribute[]? GetRequiredPermissions<T>(string methodName) where T : ControllerBase
    {
        MethodInfo methodInfo = typeof(T).GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public)!;
        return methodInfo.GetCustomAttributes(typeof(RequiresPermissionAttribute), true) as RequiresPermissionAttribute[];
    }
}
