using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Sample.API.Object.API;
using Sample.Process;
using Sample.Tests.Objects;

namespace Sample.Tests
{
    internal class WeatherForecastGetTest_V1 : IntegrationTest
    {
        protected readonly HttpClient _client;

        public WeatherForecastGetTest_V1() : this(new ApiVersion(1, 0))
        {
        }

        public WeatherForecastGetTest_V1(ApiVersion version)
        {
            var factory = new CustomWebApplicationFactory<Program>();
            _client = factory.CreateClient(
                new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions()
                {
                    AllowAutoRedirect = false
                }
            );
            _client.DefaultRequestHeaders.Add("X-API-KEY", "SomeKey");
            _client.DefaultRequestHeaders.Add("X-API-VERSION", version.ToString());
        }

        [SetUp]
        public void SetUp()
        {
            WeatherForecasts.Reset();
        }

        [Test]
        public virtual async Task TestGet()
        {
            //Arrange

            // Act
            var response = await _client.GetAsync(Routes.WeatherForecast.Get("b2216615-de2e-457b-99c1-0376fddecc04")).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var responseObject = JsonToObject<ForecastResponse>(json);
            Assert.NotNull(responseObject);
        }

        [Test]
        public virtual async Task TestGetNotFound()
        {
            //Arrange

            // Act
            var response = await _client.GetAsync(Routes.WeatherForecast.Get("48685548-64ed-403c-ba08-3cde7246e8ee")).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
