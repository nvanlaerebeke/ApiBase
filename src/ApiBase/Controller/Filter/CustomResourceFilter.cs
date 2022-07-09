using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiBase.Controller.Filter
{
    /// <summary>
    /// Class that makes it easier to implement custom resource filters
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public abstract class CustomResourceFilter : Attribute, IResourceFilter
    {
        protected abstract void beforeRun(ResourceExecutingContext context);

        protected abstract void afterRun(ResourceExecutedContext context);

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            afterRun(context);
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            beforeRun(context);
        }
    }
}
