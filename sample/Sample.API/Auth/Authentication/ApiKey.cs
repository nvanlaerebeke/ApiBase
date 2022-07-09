using ApiBase.Authentication.ApiKey;

namespace Sample.API.Auth.Authentication
{
    /// <summary>
    /// ToDo: will be replaced by the Model that implements IAuthenticationInfo
    /// </summary>
    internal class ApiKey : IApiKeyAuthenticationInfo
    {
        public ApiKey(string key, string type)
        {
            Type = type;
            Key = key;
        }

        public string Type { get; }

        public string Key { get; }
    }
}
