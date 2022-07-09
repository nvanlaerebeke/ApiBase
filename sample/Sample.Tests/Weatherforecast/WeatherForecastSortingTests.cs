using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Sample.API.Object.API;
using ApiBase.Controller.Response;

namespace Sample.Tests
{
    internal class WeatherForecastSortingTests : WeatherForecastGetTest_V1
    {
        public WeatherForecastSortingTests() : base(new ApiVersion(2, 1))
        {
        }

        [Test]
        public async Task SortByIDAsc()
        {
            //Arrange
            var query = HttpUtility.ParseQueryString("");
            query["OrderBy"] = "ID";
            query["SortDirection"] = "ASC";
            var uri = Routes.WeatherForecast.GetAll() + "?" + query.ToString();

            // Act
            var response = await _client.GetAsync(uri).ConfigureAwait(false);

            // Assert
            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var responseObject = JsonToObject<PageResponse<ForecastResponse>>(responseJson);

            Assert.NotNull(responseObject);
            Assert.AreEqual(responseObject.Data.OrderBy(x => x.ID), responseObject.Data);
        }

        [Test]
        public async Task SortByDateAsc()
        {
            //Arrange
            var query = HttpUtility.ParseQueryString("");
            query["OrderBy"] = "Date";
            query["SortDirection"] = "ASC";
            var uri = Routes.WeatherForecast.GetAll() + "?" + query.ToString();

            // Act
            var response = await _client.GetAsync(uri).ConfigureAwait(false);

            // Assert
            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var responseObject = JsonToObject<PageResponse<ForecastResponse>>(responseJson);

            Assert.NotNull(responseObject);
            Assert.AreEqual(responseObject.Data.OrderBy(x => x.Date), responseObject.Data);
        }

        [Test]
        public async Task SortBySummaryAsc()
        {
            //Arrange
            var query = HttpUtility.ParseQueryString("");
            query["OrderBy"] = "Summary";
            query["SortDirection"] = "ASC";
            var uri = Routes.WeatherForecast.GetAll() + "?" + query.ToString();

            // Act
            var response = await _client.GetAsync(uri).ConfigureAwait(false);

            // Assert
            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var responseObject = JsonToObject<PageResponse<ForecastResponse>>(responseJson);

            Assert.NotNull(responseObject);
            Assert.AreEqual(responseObject.Data.OrderBy(x => x.Summary), responseObject.Data);
        }

        [Test]
        public async Task SortByTempCAsc()
        {
            //Arrange
            var query = HttpUtility.ParseQueryString("");
            query["OrderBy"] = "TemperatureC";
            query["SortDirection"] = "ASC";
            var uri = Routes.WeatherForecast.GetAll() + "?" + query.ToString();

            // Act
            var response = await _client.GetAsync(uri).ConfigureAwait(false);

            // Assert
            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var responseObject = JsonToObject<PageResponse<ForecastResponse>>(responseJson);

            Assert.NotNull(responseObject);
            Assert.AreEqual(responseObject.Data.OrderBy(x => x.TemperatureC), responseObject.Data);
        }
    }
}
