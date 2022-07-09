using System;
using NUnit.Framework;
using Sample.Process.Object;

namespace Sample.Process.Tests.Object
{
    public class ForecastTests
    {
        [Test]
        public void Forecast()
        {
            //Arrange

            //Act
            var obj = new Forecast()
            {
                ID = "1",
                Date = DateTime.Now,
                TemperatureC = 666,
                Summary = "Summary"
            };

            //Assert
            Assert.AreEqual("Summary", obj.Summary);
            Assert.AreEqual(666, obj.TemperatureC);
            Assert.AreEqual("1", obj.ID);
            Assert.That(DateTime.Now, Is.EqualTo(obj.Date).Within(TimeSpan.FromSeconds(5)));
        }
    }
}
