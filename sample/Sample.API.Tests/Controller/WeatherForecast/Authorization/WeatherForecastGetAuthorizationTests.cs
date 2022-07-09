using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using NUnit.Framework;
using Sample.API.Controller.WeatherForecast.Authorization;

namespace Sample.API.Tests.Controller.WeatherForecast.Authorization
{
    [TestFixture]
    internal class WeatherForecastGetAuthorizationTests
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
        public void Allowed()
        {
            //Arrange
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, "SomeKey")
            };
            var identity = new ClaimsIdentity(claims, "ApiKey");
            var principal = new GenericPrincipal(identity, new string[] { "Controller" });
            AuthorizationFilterContext.HttpContext.User = new ClaimsPrincipal(principal);

            //Act
            var obj = new WeatherForecastGetAuthorization();
            obj.OnAuthorization(AuthorizationFilterContext);

            //Assert
            Assert.IsNull(AuthorizationFilterContext.Result);
        }

        [Test]
        public void Failed()
        {
            //Arrange
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, "SomeKey")
            };
            var identity = new ClaimsIdentity(claims, "ApiKey");
            var principal = new GenericPrincipal(identity, new string[] { "Satan" });
            AuthorizationFilterContext.HttpContext.User = new ClaimsPrincipal(principal);

            //Act
            var obj = new WeatherForecastGetAuthorization();
            obj.OnAuthorization(AuthorizationFilterContext);

            //Assert
            Assert.NotNull(AuthorizationFilterContext.Result);
        }

        [Test]
        public void FailedNoRoles()
        {
            //Arrange
            //Act
            var obj = new WeatherForecastGetAuthorization();
            obj.OnAuthorization(AuthorizationFilterContext);

            //Assert
            Assert.NotNull(AuthorizationFilterContext.Result);
        }
    }
}
