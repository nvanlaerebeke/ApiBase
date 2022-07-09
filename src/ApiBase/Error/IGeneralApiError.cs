using System.Net;

namespace ApiBase.Error
{
    public interface IGeneralApiError
    {
        string Message { get; }

        HttpStatusCode HttpStatusCode { get; }
    }
}
