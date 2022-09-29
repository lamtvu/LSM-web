using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LmsBeApp_Group06.Data.Repositories.ClassRepo;
using LmsBeApp_Group06.Data.Repositories.QuizRepo;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Filters;
using LmsBeApp_Group06.Models;
using LmsBeApp_Group06.Services.TimeService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LmsBeApp_Group06.Controllers
{
    [ApiController]
    [Route("api/quiz")]
    public class QuizController : ControllerBase
    {
        private readonly IQuizRepo _quizRepo;
        private readonly IMapper _mapper;
        private readonly IClassRepo _classRepo;
        private readonly ITimeService _timeService;
        public QuizController(IQuizRepo quizRepo, IClassRepo classRepo, IMapper mapper, ITimeService timeService)
        {
            this._timeService = timeService;
            this._classRepo = classRepo;
            this._mapper = mapper;
            this._quizRepo = quizRepo;
        }

        ///<summary>tao quiz cho class co classid</summary>
        [HttpPost("{classId}")]
        [ServiceFilter(typeof(OwnedClassCheck))]
        public async Task<ActionResult<Response<object>>> CreateQuiz(int classId, QuizCreateDto quizCreate)
        {
            var _class = await _classRepo.GetById(classId);
            if (_class == null)
            {
                return BadRequest(new Response<object> { Data = null, Messager = "classId does not exist", StatusCode = 400 });

            }
            var quizModel = _mapper.Map<Quiz>(quizCreate);
            var time = quizCreate.StartTime.Split(':');
            var dueDate = quizCreate.StartDate;
            quizModel.StartDate = new DateTime(dueDate.Year, dueDate.Month, dueDate.Day, Convert.ToInt32(time[0]), Convert.ToInt32(time[1]), 0).AddHours(-7);
            quizModel.CreateDate = DateTime.UtcNow;
            quizModel._Class = _class;
            await _quizRepo.Create(quizModel);
            await _quizRepo.SaveChange();
            return Ok(new Response<object> { Data = null, Messager = "success", StatusCode = 200 });
        }

        ///<summary>xoa quiz</summary>
        [HttpDelete("{quizId}")]
        [Authorize]
        public async Task<ActionResult<Response<object>>> DeleteQuiz(int quizId)
        {
            var quiz = await _quizRepo.GetById(quizId);
            if (quiz == null)
            {
                return BadRequest(new Response<object> { Data = null, Messager = "quiz does not exist", StatusCode = 400 });
            }
            await _quizRepo.Remove(quiz);
            await _quizRepo.SaveChange();
            return Ok(new Response<object> { Data = null, Messager = "success", StatusCode = 200 });
        }


        ///<summary>chinh sua quiz</summary>
        [HttpPut("{quizId}")]
        [Authorize]
        public async Task<ActionResult<Response<object>>> ChangeQuiz(int quizId, QuizCreateDto quizChange)
        {
            var quizModel = await _quizRepo.GetById(quizId);
            if (quizModel == null)
            {
                return BadRequest(new Response<object> { Data = null, Messager = "quiz does not exist", StatusCode = 400 });
            }
            _mapper.Map(quizChange, quizModel);
            var timeValue = quizChange.StartTime.Split(':');
            var dueDate = quizChange.StartDate;
            quizModel.StartDate =new DateTime(dueDate.Year, dueDate.Month, dueDate.Day, Convert.ToInt32(timeValue[0]), Convert.ToInt32(timeValue[1]), 0).AddHours(-7);
            quizModel.CreateDate = DateTime.UtcNow;
            await _quizRepo.SaveChange();
            return Ok(new Response<object> { Data = null, Messager = "success", StatusCode = 200 });
        }

        ///<summary>lay quiz theo id</summary>
        [HttpGet("{quizId}")]
        [Authorize]
        public async Task<ActionResult<Response<QuizReadDto>>> GetById(int quizId)
        {
            var quizModel = await _quizRepo.GetById(quizId);
            if (quizModel == null)
            {
                return BadRequest(new Response<object> { Data = null, Messager = "quiz does not exist", StatusCode = 400 });
            }
            var quizRead = _mapper.Map<QuizReadDto>(quizModel);
            return Ok(new Response<QuizReadDto> { Data = quizRead, Messager = "success", StatusCode = 200 });
        }

        ///<summary>lay quiz theo class id</summary>
        [HttpGet("by-class/{classId}")]
        [Authorize]
        public async Task<ActionResult<Response<IEnumerable<QuizReadDto>>>> GetbyClassId(int classId)
        {
            var quizzes = await _quizRepo.GetByClassId(classId);
            var quizzesRead = _mapper.Map<IEnumerable<QuizReadDto>>(quizzes);
            return Ok(new Response<IEnumerable<QuizReadDto>> { Data = quizzesRead, StatusCode = 200, Messager = "success" });
        }

    }
}
