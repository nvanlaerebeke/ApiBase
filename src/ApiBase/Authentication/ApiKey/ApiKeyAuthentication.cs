namespace ApiBase.Authentication.ApiKey
{
    /// <summary>
    /// This class does authentication for API keys that are provided by an IApiKeyProvider
    /// </summary>
    public class ApiKeyAuthentication : IApiKeyAuthentication
    {
        /// <summary>
        /// provides the api keys
        /// </summary>
        private readonly IApiKeyProvider ApiKeyProvider;

        public ApiKeyAuthentication(IApiKeyProvider apiKeyProvider) => ApiKeyProvider = apiKeyProvider;

        /// <summary>
        /// Authenticates the given info
        /// </summary>
        /// <param name="authenticationParams"></param>
        /// <returns>API key information, null when not found</returns>
        public IApiKeyAuthenticationInfo Authenticate(ApiKeyAuthenticationParams apiKeyAuthenticationParams) => ApiKeyProvider.GetByKey(apiKeyAuthenticationParams.GetApiKey());

        /// <summary>
        /// Verifies an apiKey
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public bool Verify(string apiKey) => Authenticate(new ApiKeyAuthenticationParams(apiKey)) != null;
    }
}
