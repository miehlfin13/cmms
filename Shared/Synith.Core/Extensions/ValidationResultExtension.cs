using FluentValidation.Results;

namespace Synith.Core.Extensions;
public static class ValidationResultExtension
{
    public static string ToJson(this ValidationResult result)
    {
        return $"[{string.Join(",", result.Errors.Select(x => x.ErrorMessage))}]";
    }
}
