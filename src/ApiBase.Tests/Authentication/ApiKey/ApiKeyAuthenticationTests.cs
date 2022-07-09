using Moq;
using NUnit.Framework;
using ApiBase.Authentication.ApiKey;

namespace ApiBase.Tests.Authentication.ApiKey
{
    public class ApiKeyAuthenticationTests
    {
        private readonly string ApiKey = "MyApiKey";
        private readonly string Type = "Controller";

        [Test]
        public void Authenticate()
        {
            //Arrange
            var k = new Mock<IApiKeyAuthenticationInfo>();
            var p = new Mock<IApiKeyProvider>();
            _ = p.Setup(x => x.GetByKey(ApiKey)).Returns(k.Object);

            _ = k.Setup(x => x.Key).Returns(ApiKey);
            _ = k.Setup(x => x.Type).Returns(Type);

            //Act
            var obj = new ApiKeyAuthentication(p.Object);
            var r = obj.Authenticate(new ApiKeyAuthenticationParams(ApiKey));

            //Assert
            p.Verify(x => x.GetByKey(ApiKey), Times.Once);
            Assert.AreEqual(ApiKey, r.Key);
            Assert.AreEqual(Type, r.Type);
        }

        [Test]
        public void VerifyFail()
        {
            //Arrange
            var p = new Mock<IApiKeyProvider>();

            //Act
            var obj = new ApiKeyAuthentication(p.Object);
            var r = obj.Verify(ApiKey);

            //Assert
            p.Verify(x => x.GetByKey(ApiKey), Times.Once);
            Assert.IsFalse(r);
        }

        [Test]
        public void VerifySuccess()
        {
            //Arrange
            var k = new Mock<IApiKeyAuthenticationInfo>();
            var p = new Mock<IApiKeyProvider>();
            _ = p.Setup(x => x.GetByKey(ApiKey)).Returns(k.Object);

            _ = k.Setup(x => x.Key).Returns(ApiKey);
            _ = k.Setup(x => x.Type).Returns(Type);

            //Act
            var obj = new ApiKeyAuthentication(p.Object);
            var r = obj.Verify(ApiKey);

            //Assert
            p.Verify(x => x.GetByKey(ApiKey), Times.Once);
            Assert.IsTrue(r);
        }
    }
}
