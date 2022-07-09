namespace ApiBase.Authentication.ApiKey
{
    public interface IApiKeyAuthenticationInfo : IAuthenticationInfo
    {
        string Key { get; }
        string Type { get; }
    }
}
