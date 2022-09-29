using System;
using System.Linq;
using System.Threading.Tasks;
using LmsBeApp_Group06.Data;
using LmsBeApp_Group06.Data.Repositories.ClassRepo;
using LmsBeApp_Group06.Data.Repositories.UserRepos;
using LmsBeApp_Group06.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LmsBeApp_Group06.Filters
{
    public class OwnedClassCheck : ActionFilterAttribute
    {
        private readonly IClassRepo _classRepo;
        private readonly IUserRepo _userRepo;
        public OwnedClassCheck(IClassRepo classRepo, IUserRepo userRepo)
        {
            this._userRepo = userRepo;
            this._classRepo = classRepo;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ActionArguments.ContainsKey("classId"))
            {
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(new Response<string>
                {
                    Data = null,
                    StatusCode = 400,
                    Messager = "required id"
                });
                return;
            }

            var id = context.ActionArguments?.ToList().FirstOrDefault(x => x.Key == "classId").Value;
            var _class = await _classRepo.GetDetail(Convert.ToInt32(id));
            if (_class == null)
            {
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(new Response<string>
                {
                    Data = null,
                    StatusCode = 400,
                    Messager = "class id already exist"
                });
                return;
            }
            var username = context.HttpContext.User.Identity.Name;
            var user = await _userRepo.GetByUsername(username);
            if (_class.TeacherId != user?.Id)
            {
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(new Response<string>
                {
                    Data = null,
                    StatusCode = 403,
                    Messager = "do not own this class"
                });
                return;
            }
            await base.OnActionExecutionAsync(context, next);
        }
    }
}