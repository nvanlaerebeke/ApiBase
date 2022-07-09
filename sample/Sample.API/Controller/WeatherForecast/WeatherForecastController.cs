using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sample.API.Controller.WeatherForecast.Authorization;
using Sample.API.Object.API;
using Sample.Error;
using Sample.Process.Object;
using ApiBase.Controller.Response;
using ApiBase.Filter.Pagination;
using ApiBase.Filter.Sorting;

namespace Sample.API.Controller.WeatherForecast
{
    /// <summary>
    /// Weather forecast V1 API controller
    /// </summary>
    [ApiVersion("2.1")]
    [ApiVersion("2.0")]
    [Authorize(AuthenticationSchemes = "ApiKey")]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : WeatherForecastController_V1
    {
        /// <summary>
        /// Creates the Weatherforecast API controller
        /// </summary>
        /// <param name="mapper"></param>
        public WeatherForecastController(IMapper mapper) : base(mapper)
        {
        }

        /// <summary>
        /// Returns the forecast by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [WeatherForecastGetAuthorization]
        [ProducesErrorResponseType(typeof(ApiError))]
        [HttpGet("{id}")]
        public override IActionResult Get(string id)
        {
            var foreCast = Forecasts.Get(id);
            if (foreCast == null)
            {
                return NotFound();
            }
            return Ok(new Response<ForecastResponse>(Mapper.Map<ForecastResponse>(foreCast)));
        }

        /// <summary>
        /// Get all forecasts
        /// </summary>
        /// <param name="paginationQuery"></param>
        /// <param name="sortQuery"></param>
        /// <returns></returns>
        [WeatherForecastGetAllAuthorization]
        [HttpGet, ApiVersion("2.1")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQueryString paginationQuery, [FromQuery] SortQueryString sortQuery)
        {
            var forecasts = await Forecasts.GetAsync(
                Mapper.Map<PaginationFilter>(paginationQuery),
                Mapper.Map<SortFilter>(sortQuery)
            ).ConfigureAwait(false);

            return Ok(new PageResponse<ForecastResponse>(
                Mapper.Map<List<ForecastResponse>>(forecasts.Data),
                forecasts.TotalRecords,
                forecasts.PaginationFilter)
            );
        }

        /// <summary>
        /// Gets all forecasts
        /// </summary>
        /// <returns></returns>
        [ApiVersion("2.0")]
        [WeatherForecastGetAllAuthorization]
        [HttpGet]
        public async Task<IActionResult> GetAll_v2()
        {
            var forecasts = await Forecasts.GetAsync().ConfigureAwait(false);
            var result = Mapper.Map<List<ForecastResponse>>(forecasts.Data);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new forecast
        /// </summary>
        /// <param name="forecast"></param>
        /// <returns></returns>
        [WeatherForecastCreateAuthorization]
        [HttpPost]
        public IActionResult Create(ForecastResponse forecast)
        {
            var newForcast = Mapper.Map<Forecast>(forecast);
            newForcast = Forecasts.Add(newForcast);
            return Redirect(new RedirectResponse<ForecastResponse>(this, Mapper.Map<ForecastResponse>(newForcast)).Uri.ToString());
        }

        /// <summary>
        /// Updates a forecast
        /// </summary>
        /// <param name="forecast"></param>
        /// <returns></returns>
        [WeatherForecastUpdateAuthorization]
        [HttpPut]
        public IActionResult Update(ForecastResponse forecast)
        {
            var updatedForcast = Mapper.Map<Forecast>(forecast);
            updatedForcast = Forecasts.Update(updatedForcast);
            if (updatedForcast == null)
            {
                return NotFound();
            }
            return Redirect(new RedirectResponse<ForecastResponse>(this, Mapper.Map<ForecastResponse>(updatedForcast)).Uri.ToString());
        }

        /// <summary>
        /// Deletes a forecast by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [WeatherForecastDeleteAuthorization]
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (id.Equals("972944b0-38c3-4b43-a56d-b773ebf11a11", System.StringComparison.OrdinalIgnoreCase))
            {
                throw new SampleException(new ApiError(ApiErrorCode.Critical));
            }
            var removed = Forecasts.Delete(id);
            if (removed == null)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
