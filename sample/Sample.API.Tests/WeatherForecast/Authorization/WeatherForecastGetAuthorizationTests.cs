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

namespace ApiBase.Tests.Authentication.ApiKey
{
    public class WeatherForecastGetAuthorizationTests
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

            //Act
            var obj = new WeatherForecastGetAuthorization();
            obj.OnAuthorization(AuthorizationFilterContext);

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
        public void OnAuthorizationIncorrectRole()
        {
            //Arrange
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, "SomeKey")
            };
            var identity = new ClaimsIdentity(claims, "ApiKey");
            var principal = new GenericPrincipal(identity, new string[] { "Storage" });
            //var ticket = new AuthenticationTicket(principal, "ApiKey");
            AuthorizationFilterContext.HttpContext.User = new ClaimsPrincipal(principal);

            //Act
            var obj = new WeatherForecastGetAuthorization();
            obj.OnAuthorization(AuthorizationFilterContext);

            //Assert
            Assert.That(AuthorizationFilterContext.Result.GetType().Equals(typeof(UnauthorizedObjectResult)));
            var result = AuthorizationFilterContext.Result as UnauthorizedObjectResult;
            Assert.NotNull(result);
            Assert.AreEqual(401, result.StatusCode);
        }
    }
}
