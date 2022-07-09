namespace ApiBase.Authentication.ApiKey
{
    public interface IApiKeyProvider
    {
        public IApiKeyAuthenticationInfo GetByKey(string apiKey);
    }
}
