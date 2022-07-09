using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiBase.Controller.Filter
{
    internal class BaseUrlFilter : CustomResourceFilter
    {
        protected override void afterRun(ResourceExecutedContext context)
        {
            //nothing to do
        }

        protected override void beforeRun(ResourceExecutingContext context)
        {
            //set current baseUrl, this is with the controller included
            var request = context.HttpContext.Request;
            //set the Uri, this is just the base uri without the controller part
            context.HttpContext.Items.Add("BaseUri", new Uri(request.Scheme + "://" + request.Host.Value + request.PathBase));
        }
    }
}
