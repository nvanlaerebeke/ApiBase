using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Sample.API.Object.API;
using Sample.Process.Object;
using ApiBase.Controller.Response;

namespace Sample.Tests
{
    internal class WeatherForecastGetTest_V2_0 : WeatherForecastGetTest_V1
    {
        public WeatherForecastGetTest_V2_0() : base(new ApiVersion(2, 0))
        {
        }

        public WeatherForecastGetTest_V2_0(ApiVersion version) : base(version)
        {
        }

        [Test]
        public override async Task TestGet()
        {
            //Arrange

            // Act
            var response = await _client.GetAsync(Routes.WeatherForecast.Get("b2216615-de2e-457b-99c1-0376fddecc04")).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var responseObject = JsonToObject<Response<ForecastResponse>>(json);
            Assert.NotNull(responseObject);
        }

        [Test]
        public virtual async Task TestGetAll()
        {
            //Arrange

            // Act
            var response = await _client.GetAsync(Routes.WeatherForecast.GetAll()).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var responseObject = JsonToObject<List<ForecastResponse>>(json);
            Assert.That(responseObject.Count.Equals(100));
        }

        [Test]
        public async Task TestCreate()
        {
            //Arrange
            var obj = new ForecastResponse()
            {
                ID = Guid.NewGuid().ToString(),
                Date = DateTime.Now,
                Summary = nameof(Summary.Chilly),
                TemperatureC = 666
            };
            var json = JsonSerializer.Serialize(obj);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(Routes.WeatherForecast.Create(), data).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.Redirect, response.StatusCode);
            Assert.That(response.Headers.Contains("Location"));
            Assert.IsTrue(response.Headers.TryGetValues("Location", out var location));
            Assert.AreNotEqual(new Uri(location.First()).LocalPath, "/WeatherForecast/" + obj.ID); //test if the ID provided was not used for the create
            Assert.That(new Uri(location.First()).LocalPath.Length.Equals(("/WeatherForecast/" + obj.ID).Length));
        }

        [Test]
        public async Task TestUpdate()
        {
            //Arrange
            var obj = new ForecastResponse()
            {
                ID = "b2216615-de2e-457b-99c1-0376fddecc04",
                Date = DateTime.Now,
                Summary = nameof(Summary.Chilly),
                TemperatureC = 666
            };
            var json = JsonSerializer.Serialize(obj);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync(Routes.WeatherForecast.Update(), data).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.Redirect, response.StatusCode);
            Assert.That(response.Headers.Contains("Location"));
            Assert.IsTrue(response.Headers.TryGetValues("Location", out var location));
            Assert.AreEqual(new Uri(location.First()).LocalPath, "/WeatherForecast/" + obj.ID); //test if the ID provided was not used for the create

            var newRequest = await _client.GetAsync(Routes.WeatherForecast.Get("b2216615-de2e-457b-99c1-0376fddecc04")).ConfigureAwait(false);
            var newJson = await newRequest.Content.ReadAsStringAsync().ConfigureAwait(false);
            var newObject = JsonToObject<Response<ForecastResponse>>(newJson).Data;

            Assert.AreEqual(obj.ID, newObject.ID);
            Assert.AreEqual(obj.Summary, newObject.Summary);
            Assert.AreEqual(obj.TemperatureC, newObject.TemperatureC);
        }

        [Test]
        public async Task TestUpdateNotFound()
        {
            //Arrange
            var obj = new ForecastResponse()
            {
                ID = "37eaa491-6b79-4fca-b5aa-50473f06553f",
                Date = DateTime.Now,
                Summary = nameof(Summary.Chilly),
                TemperatureC = 666
            };
            var json = JsonSerializer.Serialize(obj);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync(Routes.WeatherForecast.Update(), data).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task TestDelete()
        {
            //Arrange

            // Act
            var deleteRequest = await _client.DeleteAsync(Routes.WeatherForecast.Delete("b2216615-de2e-457b-99c1-0376fddecc04")).ConfigureAwait(false);

            //Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, deleteRequest.StatusCode);

            var getRequest = await _client.GetAsync(Routes.WeatherForecast.Get("b2216615-de2e-457b-99c1-0376fddecc04")).ConfigureAwait(false);
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, getRequest.StatusCode);
        }

        [Test]
        public async Task TestDeleteNotFound()
        {
            //Arrange
            // Act
            var deleteRequest = await _client.DeleteAsync(Routes.WeatherForecast.Delete("b8bf707c-1c26-42bd-9ac3-a4f71e376474")).ConfigureAwait(false);
            //Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, deleteRequest.StatusCode);
        }

        [Test]
        public async Task TestFailedDelete()
        {
            //Arrange
            // Act
            var deleteRequest = await _client.DeleteAsync(Routes.WeatherForecast.Delete("972944b0-38c3-4b43-a56d-b773ebf11a11")).ConfigureAwait(false);
            //Assert
            Assert.AreEqual(System.Net.HttpStatusCode.InternalServerError, deleteRequest.StatusCode);
        }
    }
}
