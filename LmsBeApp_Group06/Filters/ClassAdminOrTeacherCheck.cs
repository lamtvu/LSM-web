using System.Threading.Tasks;
using LmsBeApp_Group06.Data.Repositories.ClassRepo;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LmsBeApp_Group06.Filters
{
    public class ClassAdminOrTeacherCheck : ActionFilterAttribute
    {
        private readonly IClassRepo _classRepo;
        public ClassAdminOrTeacherCheck(IClassRepo classRepo)
        {
            this._classRepo = classRepo;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var username = context.HttpContext.User.Identity.Name;

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
