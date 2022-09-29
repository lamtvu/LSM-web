using System;
using System.Linq;
using System.Threading.Tasks;
using LmsBeApp_Group06.Data.Repositories.CourseRepos;
using LmsBeApp_Group06.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LmsBeApp_Group06.Filters
{
    public class OwnedCourseCheck : ActionFilterAttribute
    {
        private readonly ICourseRepo _courseRepo;
        public OwnedCourseCheck(ICourseRepo courseRepo)
        {
            this._courseRepo = courseRepo;

        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var username = context.HttpContext.User.Identity.Name;

            if (!context.ActionArguments.ContainsKey("courseId"))
            {
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(new Response<object>
                {
                    StatusCode = 400,
                    Data = null,
                    Messager = "id required"
                });
                return;
            }
            var id = context.ActionArguments.ToList().FirstOrDefault(x => x.Key == "courseId").Value;
            var course = await _courseRepo.GetById(Convert.ToInt32(id));
            if (course == null)
            {
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(new Response<object>
                {
                    StatusCode = 400,
                    Data = null,
                    Messager = "course not found"
                });
                return;
            }

            if (course.Instructor.Username != username)
            {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new JsonResult(new Response<object>
                {
                    StatusCode = 401,
                    Data = null,
                    Messager = "you dont owned this course"
                });
                return;
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}