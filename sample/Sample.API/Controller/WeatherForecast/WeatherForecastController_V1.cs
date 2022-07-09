using System.ComponentModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sample.API.Controller.WeatherForecast.Authorization;
using Sample.API.Object.API;
using Sample.Error;
using Sample.Process;
using ApiBase.Controller;

namespace Sample.API.Controller.WeatherForecast
{
    /// <summary>
    /// Weather forecast V1 API controller
    /// </summary>
    [ApiVersion("0.1", Deprecated = true)]
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = "ApiKey")]
    [ApiController]
    [Route("WeatherForecast")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiError))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiError))]
    [DisplayName("WeatherForecast")]
    public class WeatherForecastController_V1 : APIController
    {
        /// <summary>
        /// Mapper that holds the translation between the API objects and the application objects
        /// </summary>
        protected readonly IMapper Mapper;
        /// <summary>
        /// Cached forecasts
        /// </summary>
        protected readonly WeatherForecasts Forecasts;

        /// <summary>
        /// Creates a Weatherforecast controller
        /// </summary>
        /// <param name="mapper"></param>
        public WeatherForecastController_V1(IMapper mapper)
        {
            Mapper = mapper;
            Forecasts = new WeatherForecasts();
        }

        /// <summary>
        /// Gets the forecast by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [WeatherForecastGetAuthorization]
        [HttpGet("{id}")]
        public virtual IActionResult Get(string id)
        {
            var foreCast = Forecasts.Get(id);
            if (foreCast == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<ForecastResponse>(foreCast));
        }
    }
}
