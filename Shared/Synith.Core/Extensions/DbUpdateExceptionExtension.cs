using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Synith.Core.Constants;
using Synith.Core.Messages;

namespace Synith.Core.Extensions;
public static class DbUpdateExceptionExtension
{
    public static string GetSqlErrorMessage(this DbUpdateException ex, Func<string, string>? decrypt = null)
    {
        var sqlEx = (ex.InnerException as SqlException)!;

        if (sqlEx.Number == SqlErrorNumber.UniqueKeyViolation)
        {
            string message = sqlEx.GetUniqueKeyViolationValue();
            if (decrypt != null)
            {
                message = message.TryDecrypt(decrypt);
            }
            return ErrorMessageProvider.UniqueKeyViolation(message);
        }

        return ex.Message;
    }
}
