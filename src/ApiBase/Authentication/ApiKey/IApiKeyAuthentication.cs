namespace ApiBase.Authentication.ApiKey
{
    /// <summary>
    /// Interface to make passing the ApiKeyAuthentication class more easily (hides the generics)
    /// </summary>
    public interface IApiKeyAuthentication : IAuthenticate<IApiKeyAuthenticationInfo, ApiKeyAuthenticationParams>
    {
    }
}
