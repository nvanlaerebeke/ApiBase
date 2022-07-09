using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiBase.Controller.Filter
{
    /// <summary>
    /// Class that makes it easier to implement custom action filters
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public abstract class CustomActionFilter : Attribute, IActionFilter
    {
        protected abstract void beforeRun(ActionExecutingContext context);

        protected abstract void afterRun(ActionExecutedContext context);

        public void OnActionExecuting(ActionExecutingContext context)
        {
            beforeRun(context);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            afterRun(context);
        }
    }
}
