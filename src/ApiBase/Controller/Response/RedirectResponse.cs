using System;
using ApiBase.Object;

namespace ApiBase.Controller.Response
{
    public class RedirectResponse<T> : IObjectResponse where T : IAPIObject
    {
        private readonly T NewObject;
        private readonly IAPIController Controller;
        private readonly Type TargetController;

        public RedirectResponse(IAPIController controller, T newObject) : this(controller, controller.GetType(), newObject) { }

        public RedirectResponse(IAPIController controller, Type targetController, T newObject)
        {
            Controller = controller;
            NewObject = newObject;
            TargetController = targetController;
        }

        public string GetID()
        {
            return NewObject.GetID();
        }

        public Uri Uri
        {
            get
            {
                return Controller.GetBaseUri().Append(TargetController.Name.Replace("Controller", "") + "/" + GetID());
            }
        }
    }
}
