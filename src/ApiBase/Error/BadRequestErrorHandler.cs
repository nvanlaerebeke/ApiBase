using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace ApiBase.Error
{
    internal class BadRequestErrorHandler
    {
        private readonly IBadRequestErrorCodeProvider BadRequestErrorProvider;

        public BadRequestErrorHandler(IBadRequestErrorCodeProvider badRequestErrorProvider)
        {
            BadRequestErrorProvider = badRequestErrorProvider;
        }

        public BadRequestObjectResult Handle(ActionContext context)
        {
            return new BadRequestObjectResult(
                context.ModelState
                    .Where(modelError => modelError.Value.Errors.Count > 0)
                    .Select(modelError =>
                    {
                        var code = BadRequestErrorProvider.GetCode(modelError.Key);
                        var msg = (modelError.Value.Errors?.Count > 0) ? modelError.Value.Errors[0].ErrorMessage : BadRequestErrorProvider.GetMessageForCode(code);
                        return new BadRequestApiError(code, msg, System.Net.HttpStatusCode.BadRequest);
                    }
                ).FirstOrDefault()
            );
        }
    }
}
