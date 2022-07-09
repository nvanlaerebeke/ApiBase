using System.Net;
using System.Text.Json.Serialization;
using ApiBase.Error;

namespace Sample.Error
{
    public class ApiError : IApiError<ApiErrorCode>, IApiError
    {
        [JsonConstructor]
        public ApiError() : this(ApiErrorCode.Critical) { }

        public ApiError(ApiErrorCode code) : this(code, HttpStatusCode.InternalServerError)
        {
        }

        public ApiError(ApiErrorCode code, HttpStatusCode httpStatusCode)
        {
            Code = code;
            Message = ApiErrorMessage.GetMessageForCode(code);
            HttpStatusCode = httpStatusCode;
        }

        public ApiErrorCode Code { get; set; }
        public string Message { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
