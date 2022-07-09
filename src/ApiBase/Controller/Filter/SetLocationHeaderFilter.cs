using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ApiBase.Controller.Response;

namespace ApiBase.Controller.Filter
{
    internal class SetLocationHeaderFilter : CustomActionFilter
    {
        protected override void afterRun(ActionExecutedContext context)
        {
            if (!context.HttpContext.Items.TryGetValue("BaseUri", out var baseUri))
            {
                return;
            }

            if (context.Result is OkObjectResult {Value: IObjectResponse obj})
            {
                context.HttpContext.Response.Headers.Add("Location", new Uri(baseUri + "/" + obj.GetID()).ToString());
            }
        }

        protected override void beforeRun(ActionExecutingContext context)
        {
            //nothing to do
        }
    }
}
