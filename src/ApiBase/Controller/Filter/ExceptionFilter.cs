using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ApiBase.Error;

namespace ApiBase.Controller.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var t = context.Exception.GetType();
            if (t.GetInterfaces().Contains(typeof(IApiException)))
            {
                var error = t.GetMethod("GetError").Invoke(context.Exception, Array.Empty<object>());
                context.Result = new JsonResult(error);
                context.HttpContext.Response.StatusCode = (int)(error as IGeneralApiError)?.HttpStatusCode;
            }
        }
    }
}
