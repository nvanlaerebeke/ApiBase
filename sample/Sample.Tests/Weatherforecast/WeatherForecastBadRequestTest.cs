using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Sample.Process.Object;

namespace Sample.Tests
{
    internal class WeatherForecastBadRequestTest : WeatherForecastGetTest_V1
    {
        public WeatherForecastBadRequestTest() : base(new ApiVersion(2, 1))
        {
        }

        [Test]
        public async Task Test()
        {
            //Arrange
            var obj = new Forecast()
            {
                ID = Guid.NewGuid().ToString(),
                Date = DateTime.Now,
                Summary = nameof(Summary.Chilly),
                TemperatureC = "invalid"
            };
            var json = JsonSerializer.Serialize(obj);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(Routes.WeatherForecast.Create(), data).ConfigureAwait(false);

            // Assert
            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var responseObject = JsonToObject<BadRequest>(responseJson);

            Assert.NotNull(responseObject);
            Assert.AreEqual("UnknownError", responseObject.Code);
            Assert.AreEqual(nameof(HttpStatusCode.BadRequest), responseObject.HttpStatusCode);
            Assert.That(responseObject.Message.StartsWith("The JSON value could not be converted to System.Int32. Path: $.TemperatureC"));
        }

        /// <summary>
        /// Use a custom class to break the int type for the temperatureC property
        /// </summary>
        private class Forecast
        {
            public string ID { get; set; }
            public DateTime Date { get; set; }
            public string Summary { get; set; }
            public string TemperatureC { get; set; }
        }

        /// <summary>
        /// Use a custom class, http status code is returned as a string instead of the HttpStatusCode type
        /// </summary>
        private class BadRequest
        {
            public string Code { get; set; }

            public string Message { get; set; }

            public string HttpStatusCode { get; set; }
        }
    }
}
