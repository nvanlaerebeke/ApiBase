using System;
using Microsoft.AspNetCore.Mvc;
using ApiBase.Controller.Filter;

namespace ApiBase.Controller
{
    [BaseUrlFilter]
    [SetLocationHeaderFilter]
    [ExceptionFilter]
    public abstract class APIController : ControllerBase, IAPIController
    {
        [NonAction]
        public Uri GetBaseUri()
        {
            if (HttpContext.Items.TryGetValue("BaseUri", out var baseUri))
            {
                return baseUri as Uri;
            }
            return null;
        }
    }
}
