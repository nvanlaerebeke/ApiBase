using NUnit.Framework;
using ApiBase.Authentication.ApiKey;

namespace ApiBase.Tests.Authentication.ApiKey
{
    public class ApiKeyAuthenticationParamsTests
    {
        [Test]
        public void GetApiKey()
        {
            //Arrange
            var obj = new ApiKeyAuthenticationParams("SomeKey");

            //Act
            var r = obj.GetApiKey();

            //Assert
            Assert.AreEqual("SomeKey", r);
        }
    }
}
