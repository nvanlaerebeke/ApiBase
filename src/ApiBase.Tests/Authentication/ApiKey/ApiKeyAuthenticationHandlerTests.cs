using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using ApiBase.Authentication.ApiKey;

namespace ApiBase.Tests.Authentication.ApiKey
{
    public class ApiKeyAuthenticationHandlerTests
    {
        private Mock<IOptionsMonitor<ApiKeyAuthenticationOptions>> Monitor;
        private Mock<ILoggerFactory> LoggerFactory;
        private UrlEncoder Encoder;
        private ISystemClock SystemClock;
        private Mock<IApiKeyAuthentication> ApiKeyAuthentication;
        private DefaultHttpContext Context;

        [SetUp]
        public void SetUp()
        {
            Monitor = new Mock<IOptionsMonitor<ApiKeyAuthenticationOptions>>();
            _ = Monitor.Setup(x => x.Get(It.IsAny<string>())).Returns(new ApiKeyAuthenticationOptions());

            var logger = new Mock<ILogger<ApiKeyAuthenticationHandler>>();

            LoggerFactory = new Mock<ILoggerFactory>();
            _ = LoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(logger.Object);

            Encoder = new Mock<UrlEncoder>().Object;
            SystemClock = new Mock<ISystemClock>().Object;
            ApiKeyAuthentication = new Mock<IApiKeyAuthentication>();

            Context = new DefaultHttpContext();
        }

        [Test]
        public async Task AuthenticateNoHeader()
        {
            //Arrange

            //Act
            var obj = new ApiKeyAuthenticationHandler(Monitor.Object, LoggerFactory.Object, Encoder, SystemClock, ApiKeyAuthentication.Object);
            var scheme = new AuthenticationScheme("ApiKey", "ApiKey", typeof(ApiKeyAuthenticationHandler));
            await obj.InitializeAsync(scheme, Context).ConfigureAwait(false);
            var r = await obj.AuthenticateAsync().ConfigureAwait(false);

            //Assert
            Assert.IsFalse(r.Succeeded);
            Assert.AreEqual("Missing X-API-KEY header", r.Failure.Message);
        }

        [Test]
        public async Task AuthenticateUnknownAPIKey()
        {
            //Arrange
            Context.Request.Headers.Add("X-API-KEY", "SomeKey");

            //Act
            var obj = new ApiKeyAuthenticationHandler(Monitor.Object, LoggerFactory.Object, Encoder, SystemClock, ApiKeyAuthentication.Object);
            var scheme = new AuthenticationScheme("ApiKey", "ApiKey", typeof(ApiKeyAuthenticationHandler));
            await obj.InitializeAsync(scheme, Context).ConfigureAwait(false);
            var r = await obj.AuthenticateAsync().ConfigureAwait(false);

            //Assert
            Assert.IsFalse(r.Succeeded);
            Assert.AreEqual("API Key not found", r.Failure.Message);
        }

        [Test]
        public async Task AuthenticateControllerApiKey()
        {
            //Arrange
            var key = new Mock<IApiKeyAuthenticationInfo>();
            _ = key.Setup(x => x.Key).Returns("SomeKey");
            _ = key.Setup(x => x.Type).Returns("Controller");

            Context.Request.Headers.Add("X-API-KEY", "SomeKey");
            _ = ApiKeyAuthentication.Setup(x => x.Authenticate(It.IsAny<ApiKeyAuthenticationParams>())).Returns(key.Object);

            //Act
            var obj = new ApiKeyAuthenticationHandler(Monitor.Object, LoggerFactory.Object, Encoder, SystemClock, ApiKeyAuthentication.Object);
            var scheme = new AuthenticationScheme("ApiKey", "ApiKey", typeof(ApiKeyAuthenticationHandler));
            await obj.InitializeAsync(scheme, Context).ConfigureAwait(false);
            var r = await obj.AuthenticateAsync().ConfigureAwait(false);

            //Assert
            Assert.IsTrue(r.Succeeded);
            Assert.NotNull(r.Ticket);
        }
    }
}
