using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Sample.API.Object.API;
using ApiBase.Controller.Response;

namespace Sample.Tests
{
    internal class WeatherForecastGetTest_V2_1 : WeatherForecastGetTest_V2_0
    {
        public WeatherForecastGetTest_V2_1() : base(new ApiVersion(2, 1))
        {
        }

        [Test]
        public override async Task TestGetAll()
        {
            //Arrange

            // Act
            var response = await _client.GetAsync(Routes.WeatherForecast.GetAll()).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var responseObject = JsonToObject<PageResponse<ForecastResponse>>(json);
            Assert.That(responseObject.Data.ToList().Count.Equals(100));
        }

        [Test]
        public async Task TestGetAllWithPaging()
        {
            //Arrange
            var builder = new UriBuilder("https://example.com" + Routes.WeatherForecast.GetAll());
            var querystring = HttpUtility.ParseQueryString(builder.Query);
            querystring["PageNumber"] = 2.ToString();
            querystring["PageSize"] = 40.ToString();
            builder.Query = querystring.ToString();
            var uri = builder.Uri;

            // Act
            var response = await _client.GetAsync(uri.PathAndQuery).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var responseObject = JsonToObject<PageResponse<ForecastResponse>>(json);
            Assert.That(responseObject.Data.ToList().Count.Equals(20));
        }

        [Test]
        public async Task TestGetAllWithPathBase()
        {
            //Arrange

            // Act
            var response = await _client.GetAsync(Routes.WeatherForecast.GetAllWithPathBase()).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var responseObject = JsonToObject<PageResponse<ForecastResponse>>(json);
            Assert.That(responseObject.Data.ToList().Count.Equals(100));
        }
    }
}
