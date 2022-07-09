namespace ApiBase.Authentication.ApiKey
{
    public class ApiKeyAuthenticationParams : IAuthenticationParams
    {
        private readonly string ApiKey;

        public ApiKeyAuthenticationParams(string apiKey) => ApiKey = apiKey;

        public string GetApiKey() => ApiKey;
    }
}
