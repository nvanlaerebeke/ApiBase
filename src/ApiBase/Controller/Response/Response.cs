using System.Text.Json.Serialization;

namespace ApiBase.Controller.Response
{
    public class Response<T>
    {
        public Response()
        {
        }

        public Response(T response)
        {
            Data = response;
        }

        [JsonPropertyName("data")]
        public T Data { get; set; }
    }
}
