using System;
using System.Collections.Generic;
using System.Linq;
using Sample.API.Auth.Authorization;
using ApiBase.Authorization.ApiKey;

namespace Sample.API.Controller.WeatherForecast.Authorization
{
    internal class WeatherForecastGetAuthorization : ApiKeyAuthorization
    {
        protected override bool Authorize(IEnumerable<string> roles)
        {
            return roles.Contains(nameof(ApiKeyRole.Controller));
        }
    }
}
