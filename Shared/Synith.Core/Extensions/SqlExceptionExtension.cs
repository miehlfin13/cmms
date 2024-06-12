using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Synith.Core.Extensions;
public static class SqlExceptionExtension
{
    public static string GetUniqueKeyViolationValue(this SqlException ex)
    {
        string pattern = @"The duplicate key value is \((.*?)\)";
        Match match = Regex.Match(ex.Message, pattern, RegexOptions.None, TimeSpan.FromMilliseconds(100));
        return match.Groups[1].Value;
    }
}
