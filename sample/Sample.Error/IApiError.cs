using ApiBase.Error;

namespace Sample.Error
{
    public interface IApiError : IGeneralApiError
    {
        ApiErrorCode Code { get; set; }
    }
}