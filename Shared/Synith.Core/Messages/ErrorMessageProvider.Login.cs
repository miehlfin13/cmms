using System.Text.Json;

namespace Synith.Core.Messages;
public static partial class ErrorMessageProvider
{
    public static class Login
    {
        public static string Invalid()
            => JsonSerializer.Serialize(new ResponseMessage { Code = $"{nameof(Login)}.{nameof(Invalid)}" });
        public static string Inactive()
            => JsonSerializer.Serialize(new ResponseMessage { Code = $"{nameof(Login)}.{nameof(Inactive)}" });
        public static string Pending()
            => JsonSerializer.Serialize(new ResponseMessage { Code = $"{nameof(Login)}.{nameof(Pending)}" });
        public static string Locked()
            => JsonSerializer.Serialize(new ResponseMessage { Code = $"{nameof(Login)}.{nameof(Locked)}" });
    }
}
