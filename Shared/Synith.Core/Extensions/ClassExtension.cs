using System.Text;
using System.Text.Json;

namespace Synith.Core.Extensions;
public static class ClassExtension
{
    public static StringContent ToStringContent<T>(this T content) where T : class
    {
        return new(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");
    }
}
