using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiBase.Authorization.ApiKey
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public abstract class BaseAuthorization : Attribute, IAuthorizationFilter
    {
        protected AuthorizationFilterContext Context;

        public virtual void OnAuthorization(AuthorizationFilterContext context)
        {
            Context = context;
        }

        protected IQueryCollection GetQuery()
        {
            return Context?.HttpContext?.Request?.Query;
        }
    }
}
