using ApiBase.Error;

namespace Sample.Error
{
    public class SampleException : ApiException<ApiErrorCode>

    {
        public SampleException(IApiError<ApiErrorCode> error) : base(error)
        {
        }
    }
}
