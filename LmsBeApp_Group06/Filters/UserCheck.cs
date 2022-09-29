using System;
using System.Linq;
using System.Threading.Tasks;
using LmsBeApp_Group06.Data.Repositories.ClassRepo;
using LmsBeApp_Group06.Data.Repositories.SubmisstionExerciseRepo;
using LmsBeApp_Group06.Data.Repositories.UserRepos;
using LmsBeApp_Group06.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LmsBeApp_Group06.Filters
{
    public class UserCheck : ActionFilterAttribute
    {
        private readonly IUserRepo _userRepo;
        public UserCheck(IUserRepo userRepo)
        {
            this._userRepo = userRepo;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = await _userRepo.GetByUsername(context.HttpContext.User.Identity.Name);
            if (user == null)
            {
                context.HttpContext.Response.StatusCode = 403;
                context.Result = new JsonResult(new Response<string>
                {
                    Data = null,
                    StatusCode = 403,
                    Messager = "invalid token"
                });
                return;
            }
            await base.OnActionExecutionAsync(context, next);
        }

    }
}
