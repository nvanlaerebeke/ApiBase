using NUnit.Framework;

namespace ApiBase.Tests.Authentication.ApiKey
{
    public class ApiKeyTests
    {
        [Test]
        public void ApiKey()
        {
            //Arrange

            //Act
            var obj = new Sample.API.Auth.Authentication.ApiKey("SomeKey", "Controller");

            //Assert
            Assert.AreEqual("Controller", obj.Type);
            Assert.AreEqual("SomeKey", obj.Key);
        }
    }
}
