using System;

namespace ApiBase.Error
{
#pragma warning disable RCS1194 // Implement exception constructors.

    public class ApiException<T> : Exception, IApiException
#pragma warning restore RCS1194 // Implement exception constructors.
    {
        private readonly IApiError<T> Error;

        public ApiException(IApiError<T> error)
        {
            Error = error;
        }

        public IApiError<T> GetError()
        {
            return Error;
        }
    }
}
