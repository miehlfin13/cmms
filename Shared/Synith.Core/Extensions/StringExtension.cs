using System.Text.Json;

namespace Synith.Core.Extensions;
public static class StringExtension
{
    /// <summary>
    /// Add spaces for every uppercase character
    /// </summary>
    public static string AddSpaces(this string value)
    {
        return string.Concat(value.Select(c => char.IsUpper(c) ? " " + c : c.ToString())).Trim();
    }

    /// <summary>
    /// Try to decrypt value.
    /// If success, returns decrypted value.
    /// If failed, returns value.
    /// </summary>
    public static string TryDecrypt(this string value, Func<string, string> decrypt)
    {
        try
        {
            return decrypt(value);
        }
        catch
        {
            return value;
        }
    }

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static T Deserialize<T>(this string json) where T : class
    {
        return JsonSerializer.Deserialize<T>(json, _jsonOptions)!;
    }
}
