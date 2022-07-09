using System.Collections.Concurrent;
using ApiBase.Authentication.ApiKey;

namespace Sample.API.Auth.Authentication
{
    /// <summary>
    /// Class that provides the API keys or the ApiKeyAuthentication method
    /// </summary>
    internal class ApiKeyProvider : IApiKeyProvider
    {
        private readonly ConcurrentDictionary<string, IApiKeyAuthenticationInfo> Cache = new();

        public IApiKeyAuthenticationInfo GetByKey(string apiKey)
        {
            if (Cache.ContainsKey(apiKey) && Cache.TryGetValue(apiKey, out var key))
            {
                return key;
            }

            /*
             * ToDo: Add a database backend to fetch the api keys
             * using(var work = new BackgroundTaskUnitOfWork()) {
             *     var obj = work.ApiKey.GetByKey(authenticationParams.ApiKey); // Entity implements IApiKeyAuthenticationInfo
             *     if(obj != null) {
             *         Cache.TryAdd(obj.Key, obj);
             *         return obj;
             *     }
             * }
             * return null;
             */
            var o = new ApiKey(apiKey, "Controller");
            _ = Cache.TryAdd(o.Key, o);
            return o;
        }
    }
}
