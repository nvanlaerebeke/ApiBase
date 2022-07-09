using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiBase.Authorization.ApiKey
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public abstract class ApiKeyAuthorization : BaseAuthorization
    {
        public override void OnAuthorization(AuthorizationFilterContext context)
        {
            base.OnAuthorization(context);

            var roles = ((ClaimsIdentity)context.HttpContext.User.Identity).Claims.Where(c => c.Type == ClaimTypes.Role).Select(r => r.Value).ToArray();
            if (!Authorize(roles))
            {
                context.ModelState.AddModelError("UnAuthorized", "You are not authorized");
                context.Result = new UnauthorizedObjectResult(context.ModelState);
            }
        }

        protected abstract bool Authorize(IEnumerable<string> roles);
    }
}
