using NUnit.Framework;
using Sample.API.Auth.Authentication;

namespace ApiBase.Tests.Authentication.ApiKey
{
    public class ApiKeyProviderTests
    {
        [Test]
        public void GetByKey()
        {
            //Arrange

            //Act
            var obj = new ApiKeyProvider();
            var result = obj.GetByKey("SomeKey");

            //Assert
            Assert.AreEqual("Controller", result.Type);
            Assert.AreEqual("SomeKey", result.Key);
        }

        [Test]
        public void GetByKeyFromCache()
        {
            //Arrange

            //Act
            var obj = new ApiKeyProvider();
            var result1 = obj.GetByKey("SomeKey");
            var result2 = obj.GetByKey("SomeKey");

            var result3 = obj.GetByKey("SomeKey2");

            //Assert
            Assert.That(result1.Equals(result2));
            Assert.That(!result1.Equals(result3));
        }
    }
}
