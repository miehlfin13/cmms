using Elasticsearch.Net;
using System.Text.Json;

namespace Synith.Core.Messages;
public static partial class ErrorMessageProvider
{
    private const string General = "";

    public static string NotNull(string item)
        => JsonSerializer.Serialize(new ResponseMessage
        {
            Code = $"{nameof(General)}.{nameof(NotNull)}",
            ParametersJson = JsonSerializer.Serialize(new { item })
        });

    public static string NotFound(string item, string value)
        => JsonSerializer.Serialize(new ResponseMessage
        {
            Code = $"{nameof(General)}.{nameof(NotFound)}",
            ParametersJson = JsonSerializer.Serialize(new { item, value })
        });

    public static string Required(string item)
        => JsonSerializer.Serialize(new ResponseMessage
        {
            Code = $"{nameof(General)}.{nameof(Required)}",
            ParametersJson = JsonSerializer.Serialize(new { item })
        });

    public static string Invalid(string item, string value)
        => JsonSerializer.Serialize(new ResponseMessage
        {
            Code = $"{nameof(General)}.{nameof(Invalid)}",
            ParametersJson = JsonSerializer.Serialize(new { item, value })
        });

    public static string InvalidPassword()
        => JsonSerializer.Serialize(new ResponseMessage
        {
            Code = $"{nameof(General)}.{nameof(InvalidPassword)}"
        });

    public static string NotAuthorized()
        => JsonSerializer.Serialize(new ResponseMessage
        {
            Code = $"{nameof(General)}.{nameof(NotAuthorized)}"
        });

    public static string NotExpected(string item, string actualValue, int expectedValue)
        => JsonSerializer.Serialize(new ResponseMessage
        {
            Code = $"{nameof(General)}.{nameof(NotExpected)}",
            ParametersJson = JsonSerializer.Serialize(new { item, actualValue, expectedValue })
        });
    public static string NotExpected(string item, int actualValue, int expectedValue)
        => NotExpected(item, actualValue.ToString(), expectedValue);

    public static string MaxLength(string item, string actualLength, int expectedLength)
        => JsonSerializer.Serialize(new ResponseMessage
        {
            Code = $"{nameof(General)}.{nameof(MaxLength)}",
            ParametersJson = JsonSerializer.Serialize(new { item, actualLength, expectedLength })
        });
    public static string MaxLength(string item, int actualLength, int expectedLength)
        => MaxLength(item, actualLength.ToString(), expectedLength);

    public static string MinLength(string item, string actualLength, int expectedLength)
        => JsonSerializer.Serialize(new ResponseMessage
        {
            Code = $"{nameof(General)}.{nameof(MinLength)}",
            ParametersJson = JsonSerializer.Serialize(new { item, actualLength, expectedLength })
        });
    public static string MinLength(string item, int actualLength, int expectedLength)
        => MinLength(item, actualLength.ToString(), expectedLength);

    public static string GreaterThan(string item, string actualValue, int expectedValue)
        => JsonSerializer.Serialize(new ResponseMessage
        {
            Code = $"{nameof(General)}.{nameof(GreaterThan)}",
            ParametersJson = JsonSerializer.Serialize(new { item, actualValue, expectedValue })
        });
    public static string GreaterThan(string item, int actualValue, int expectedValue)
        => GreaterThan(item, actualValue.ToString(), expectedValue);

    public static string GreaterThanOrEqual(string item, string actualValue, int expectedValue)
        => JsonSerializer.Serialize(new ResponseMessage
        {
            Code = $"{nameof(General)}.{nameof(GreaterThanOrEqual)}",
            ParametersJson = JsonSerializer.Serialize(new { item, actualValue, expectedValue })
        });
    public static string GreaterThanOrEqual(string item, int actualValue, int expectedValue)
        => GreaterThanOrEqual(item, actualValue.ToString(), expectedValue);
    public static string GreaterThanOrEqual(string item, string actualValue, DateTime expectedValue)
        => JsonSerializer.Serialize(new ResponseMessage
        {
            Code = $"{nameof(General)}.{nameof(GreaterThanOrEqual)}",
            ParametersJson = JsonSerializer.Serialize(new { item, actualValue, expectedValue })
        });

    public static string LessThan(string item, string actualValue, int expectedValue)
        => JsonSerializer.Serialize(new ResponseMessage
        {
            Code = $"{nameof(General)}.{nameof(LessThan)}",
            ParametersJson = JsonSerializer.Serialize(new { item, actualValue, expectedValue })
        });
    public static string LessThan(string item, int actualValue, int expectedValue)
        => LessThan(item, actualValue.ToString(), expectedValue);

    public static string LessThanOrEqual(string item, string actualValue, int expectedValue)
        => JsonSerializer.Serialize(new ResponseMessage
        {
            Code = $"{nameof(General)}.{nameof(LessThanOrEqual)}",
            ParametersJson = JsonSerializer.Serialize(new { item, actualValue, expectedValue })
        });
    public static string LessThanOrEqual(string item, int actualValue, int expectedValue)
        => LessThanOrEqual(item, actualValue.ToString(), expectedValue);

    public static string UniqueKeyViolation(string value)
        => JsonSerializer.Serialize(new ResponseMessage
        {
            Code = $"{nameof(General)}.{nameof(UniqueKeyViolation)}",
            ParametersJson = JsonSerializer.Serialize(new { value })
        });
}
