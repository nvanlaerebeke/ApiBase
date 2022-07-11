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
            if (!t.GetInterfaces().Contains(typeof(IApiException)))
            {
                return;
            }

            var error = t.GetMethod("GetError")?.Invoke(context.Exception, Array.Empty<object>());
            if (error == null)
            {
                return;
            }
                
            context.Result = new JsonResult(error);
            var httpStatusCode = (error as IGeneralApiError)?.HttpStatusCode;
            if (httpStatusCode != null)
            {
                context.HttpContext.Response.StatusCode = (int) httpStatusCode;
            }
        }
    }
}
