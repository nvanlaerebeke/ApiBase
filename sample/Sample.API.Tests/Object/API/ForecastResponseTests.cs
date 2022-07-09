using System;
using NUnit.Framework;
using Sample.API.Object.API;
using Sample.Process.Object;

namespace ApiBase.Tests.Authentication.ApiKey
{
    public class ForecastResponseTests
    {
        [Test]
        public void Forecast()
        {
            //Arrange

            //Act
            var obj = new ForecastResponse()
            {
                ID = "1",
                Date = DateTime.Now,
                TemperatureC = 666,
                Summary = nameof(Summary.Chilly)
            };

            //Assert
            Assert.AreEqual(nameof(Summary.Chilly), obj.Summary);
            Assert.AreEqual(666, obj.TemperatureC);
            Assert.AreEqual(32 + (int)(666 / 0.5556), obj.TemperatureF);
            Assert.AreEqual("1", obj.ID);
            Assert.AreEqual("1", obj.GetID());
            Assert.That(DateTime.Now, Is.EqualTo(obj.Date).Within(TimeSpan.FromSeconds(5)));
        }
    }
}
