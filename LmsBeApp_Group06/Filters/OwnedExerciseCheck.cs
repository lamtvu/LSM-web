using System;
using System.Linq;
using System.Threading.Tasks;
using LmsBeApp_Group06.Data.Repositories.ExerciseRepo;
using LmsBeApp_Group06.Data.Repositories.UserRepos;
using LmsBeApp_Group06.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LmsBeApp_Group06.Filters
{
    public class OwnedExerciseCheck : ActionFilterAttribute
    {
        private readonly IExerciseRepo _exerciseRepo;
        private readonly IUserRepo _userRepo;
        public OwnedExerciseCheck(IExerciseRepo exerciseRepo, IUserRepo userRepo)
        {
            this._userRepo = userRepo;
            this._exerciseRepo = exerciseRepo;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ActionArguments.ContainsKey("exerciseId"))
            {
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(new Response<string>
                {
                    Data = null,
                    StatusCode = 400,
                    Messager = "required classid"
                });
                return;
            }
            var userId = context.HttpContext.User.Claims.ToList().FirstOrDefault(c => c.Type == "UserId").Value;
            var classId = context.ActionArguments.ToList().FirstOrDefault(x => x.Key == "exerciseId").Value;
            var exercise = await _exerciseRepo.GetDetail(Convert.ToInt32(classId));
            if (exercise == null)
            {
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(new Response<string>
                {
                    Data = null,
                    StatusCode = 400,
                    Messager = "classid dosen't exist"
                });
                return;
            }
            if (exercise._Class.TeacherId != Convert.ToInt32(userId))
            {
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(new Response<string>
                {
                    Data = null,
                    StatusCode = 400,
                    Messager = "you dont own this exercise"
                });
                return;
            }
            await base.OnActionExecutionAsync(context, next);
        }
    }
}
