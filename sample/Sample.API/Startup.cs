using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sample.API.Auth.Authentication;
using Sample.Error;
using ApiBase;
using ApiBase.Authentication.ApiKey;
using ApiBase.Error;

namespace Sample.API
{
    /// <summary>
    /// ApiBase startup class
    /// </summary>
    public class Startup : ApiBase.ApiBase
    {
        /// <summary>
        /// Creates an instance of the  startup class
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Configure the Services used in the .NET API
        /// </summary>
        /// <param name="services"></param>
        protected override void ConfigureApi(IServiceCollection services)
        {
            Configuration["PathBase"] = "/sample";
            _ = services.AddAuthentication("ApiKey").AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>("ApiKey", null);
            _ = services.AddSingleton<IApiKeyAuthentication>(_ => new ApiKeyAuthentication(new ApiKeyProvider()));
            _ = services.AddAutoMapper(typeof(Startup));
        }

        /// <summary>
        /// Configure the swagger (openapi) documentation
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        protected override bool ConfigureOpenAPI(IServiceCollection services)
        {
            OpenAPIConfiguration.Setup(services);
            return true;
        }

        /// <summary>
        /// Returns the default API version to use
        /// </summary>
        /// <returns></returns>
        protected override ApiVersion DefaultVersion()
        {
            return new ApiVersion(2, 1);
        }

        /// <summary>
        /// Returns the filename of the XML descriptions
        /// </summary>
        /// <returns></returns>
        protected override string GetAPIXmlDescriptionsFileName()
        {
            return "API.xml";
        }

        /// <summary>
        /// Returns the class used to handle the bad request errors
        /// </summary>
        /// <returns></returns>
        protected override IBadRequestErrorCodeProvider GetBadRequestErrorProvider()
        {
            return new BadRequestErrorCodeProvider();
        }
    }
}
