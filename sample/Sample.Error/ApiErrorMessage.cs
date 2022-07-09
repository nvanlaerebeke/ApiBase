using System.Collections.Generic;

namespace Sample.Error
{
    public static class ApiErrorMessage
    {
        private static readonly Dictionary<ApiErrorCode, string> Messages = new()
        {
            { ApiErrorCode.Critical, "this is bad" },
            { ApiErrorCode.InvalidSummary, "Invalid Summary" },
            { ApiErrorCode.InvalidValue, "Invalid value" },
            { ApiErrorCode.UnknownError, "Unknown Error" },
        };

        public static string GetMessageForCode(ApiErrorCode code)
        {
            return Messages[code];
        }
    }
}