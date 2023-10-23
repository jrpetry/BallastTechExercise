using Microsoft.AspNetCore.Mvc.Filters;

namespace MagazineManager.Server.Controllers.Filters
{    

    [AttributeUsage(AttributeTargets.All)]
    public sealed class AuthenticateActionFilter : ActionFilterAttribute
    {
        public AuthenticateActionFilter()
        {
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var a = 1;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var a = 1;
        }
    }
}