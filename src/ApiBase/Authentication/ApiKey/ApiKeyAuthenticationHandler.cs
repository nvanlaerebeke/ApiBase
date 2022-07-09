using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ApiBase.Authentication.ApiKey
{
    /// <summary>
    /// <para>Class that handles the authentication process using an AuthenticationHandler</para>
    /// <para>@see: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-5.0</para>
    /// </summary>
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        /// <summary>
        /// Class that will validate the authentication
        /// </summary>
        private readonly IApiKeyAuthentication Authenticator;

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<ApiKeyAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IApiKeyAuthentication authenticator
        ) : base(options, logger, encoder, clock)
        {
            Authenticator = authenticator;
        }

        /// <summary>
        /// Starts the authentication process asynchronously
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return await Task.Run(() =>
            {
                ///Headers should be treated case insensitive (see HTTP RFC)
                var ApiKey = Request.Headers.Where(h => h.Key.ToLower().Equals("x-api-key", System.StringComparison.Ordinal)).Select(k => k.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(ApiKey))
                {
                    return Validate(ApiKey);
                }
                return AuthenticateResult.Fail("Missing X-API-KEY header");
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Preforms the validation for the apiKey
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        private AuthenticateResult Validate(string apiKey)
        {
            var key = Authenticator.Authenticate(new ApiKeyAuthenticationParams(apiKey));
            if (key == null)
            {
                return AuthenticateResult.Fail("API Key not found");
            }
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, key.Key)
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new GenericPrincipal(identity, new string[] { key.Type });
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}
