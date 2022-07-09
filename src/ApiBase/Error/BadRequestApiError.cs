using System.Net;

namespace ApiBase.Error
{
    public class BadRequestApiError : IApiError<string>
    {
        public BadRequestApiError() { }
        public BadRequestApiError(string code, string message, HttpStatusCode httpStatusCode)
        {
            Code = code;
            Message = message;
            HttpStatusCode = httpStatusCode;
        }

        public string Code { get; set; }

        public string Message { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
