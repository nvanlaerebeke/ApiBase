using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using ApiBase.Authorization.ApiKey;

namespace ApiBase.Tests.Authorization.ApiKey
{
    public class ApiKeyAuthorizationTests
    {
        private AuthorizationFilterContext AuthorizationFilterContext;

        [SetUp]
        public void SetUp()
        {
            AuthorizationFilterContext = new AuthorizationFilterContext(new ActionContext()
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            }, new List<IFilterMetadata>());
        }

        [Test]
        public void OnAuthorizationFailed()
        {
            //Arrange
            var obj = new Mock<ApiKeyAuthorization>()
            {
                CallBase = true
            };
            _ = obj.Protected().Setup<bool>("Authorize", ItExpr.IsAny<IEnumerable<string>>()).Returns(false);

            //Act
            obj.Object.OnAuthorization(AuthorizationFilterContext);

            //Assert
            Assert.That(AuthorizationFilterContext.Result.GetType().Equals(typeof(UnauthorizedObjectResult)));
            var result = AuthorizationFilterContext.Result as UnauthorizedObjectResult;
            Assert.NotNull(result);
            Assert.AreEqual(401, result.StatusCode);
        }

        [Test]
        public void OnAuthorizationSuccess()
        {
            //Arrange
            var obj = new Mock<ApiKeyAuthorization>()
            {
                CallBase = true
            };
            _ = obj.Protected().Setup<bool>("Authorize", ItExpr.IsAny<IEnumerable<string>>()).Returns(true);

            //Act
            obj.Object.OnAuthorization(AuthorizationFilterContext);

            //Assert
            Assert.IsNull(AuthorizationFilterContext.Result);
        }

        [Test]
        public void GetQuery()
        {
            //Arrange
            var context = new DefaultHttpContext();
            context.Request.QueryString = context.Request.QueryString
                .Add("Satan", "Evil")
                .Add("Sheep", "Meuh")
            ;

            AuthorizationFilterContext = new AuthorizationFilterContext(new ActionContext()
            {
                HttpContext = context,
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            }, new List<IFilterMetadata>());

            //Act
            var obj = new GetQueryTestObject();
            obj.OnAuthorization(AuthorizationFilterContext);
            var r = obj.GetQueryPublic();

            //Assert
            Assert.IsNull(AuthorizationFilterContext.Result);
            Assert.NotNull(r);
            Assert.That(r.Count.Equals(2));

            Assert.That(r.ContainsKey("Satan"));
            Assert.That(r.ContainsKey("Sheep"));
        }

        private class GetQueryTestObject : ApiKeyAuthorization
        {
            public IQueryCollection GetQueryPublic()
            {
                return base.GetQuery();
            }

            protected override bool Authorize(IEnumerable<string> roles)
            {
                return true;
            }
        }
    }
}
