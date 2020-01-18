using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Codity.Services.ResponseModels;

namespace Codity.WebApi.Filters
{
    public class BaseFilter : ActionFilterAttribute, IActionFilter, IResultFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var response = ModelStateErrors(filterContext);
            if (response.IsError)
            {
                filterContext.Result = new BadRequestObjectResult(response);
            }
            base.OnActionExecuting(filterContext);
        }

        protected BaseResponse ModelStateErrors(ActionExecutingContext filterContext)
        {
            var response = new BaseResponse();

            foreach (var key in filterContext.ModelState.Keys)
            {
                var value = filterContext.ModelState[key];
                foreach (var error in value.Errors)
                {
                    response.AddError(new Error { Message = error.ErrorMessage });
                }
            }
            return response;
        }
    }
}
