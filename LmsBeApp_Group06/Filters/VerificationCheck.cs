using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LmsBeApp_Group06.Filters
{
    public class VerificationCheck : ActionFilterAttribute
    {
        public VerificationCheck() { }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var verify = context.HttpContext.User.Claims.ToList().FirstOrDefault(c => c.Type == "Verify").Value;
            if (bool.Parse(verify))
            {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new ContentResult()
                {
                    Content = "unverified gmail account"
                };
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}