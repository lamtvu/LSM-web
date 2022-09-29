using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LmsBeApp_Group06.Data.Repositories.AnswerRepo;
using LmsBeApp_Group06.Data.Repositories.ClassRepo;
using LmsBeApp_Group06.Data.Repositories.QuestionRepo;
using LmsBeApp_Group06.Data.Repositories.QuizRepo;
using LmsBeApp_Group06.Data.Repositories.SubmissionQuizRepo;
using LmsBeApp_Group06.Data.Repositories.UserRepos;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Filters;
using LmsBeApp_Group06.Models;
using LmsBeApp_Group06.Services.TimeService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LmsBeApp_Group06.Controllers
{
    [ApiController]
    [Route("api/submission-quiz")]
    public class SubmissionQuizController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISubmissionQuizRepo _submissionQuizRepo;
        private readonly IQuizRepo _quizRepo;
        private readonly IUserRepo _userRepo;
        private readonly IAnswerRepo _answerRepo;
        private readonly IQuestionRepo _questionRepo;
        private readonly ITimeService _timeService;
        private readonly IClassRepo _classRepo;
        public SubmissionQuizController(ISubmissionQuizRepo submissionQuizRepo, IMapper mapper, IQuizRepo quizRepo, IClassRepo classRepo,
        IUserRepo userRepo, IAnswerRepo answerRepo, IQuestionRepo questionRepo, ITimeService timeService)
        {
            this._classRepo = classRepo;
            this._timeService = timeService;
            this._questionRepo = questionRepo;
            this._answerRepo = answerRepo;
            this._userRepo = userRepo;
            this._quizRepo = quizRepo;
            this._submissionQuizRepo = submissionQuizRepo;
            this._mapper = mapper;
        }

        [HttpPost("{quizId}")]
        [Authorize]
        [ServiceFilter(typeof(UserCheck))]
        public async Task<ActionResult<Response<object>>> CreateSubmissionQuiz(int quizId)
        {
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            var quiz = await _quizRepo.GetById(quizId);
            var classModel = await _classRepo.GetDetail(quiz.ClassId);
            if (!classModel.students.Any(x => x.Id == userId))
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "you don't take this class" });
            }

            if (quiz == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "quiz does not exist" });
            }
            var submissionQuiz = await _submissionQuizRepo.GetByUsernameAndQuizId(userId, quizId);
            if (submissionQuiz != null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "submission already exist" });
            }
            //check time
            if (DateTime.UtcNow < quiz.StartDate)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "the quiz hasn't started" });
            }
            DateTime dueTime = quiz.StartDate.AddMinutes(quiz.Duration);
            if (DateTime.UtcNow > dueTime)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "time out for quiz" });
            }
            var submiss = new SubmissionQuiz();
            submiss.Core = 0;
            submiss.StartTime = DateTime.UtcNow;
            submiss.FinishTime = DateTime.UtcNow;
            submiss.IsFinish = false;
            submiss.Quiz = quiz;
            submiss.StudentId = userId;
            await _submissionQuizRepo.Create(submiss);
            await _submissionQuizRepo.SaveChange();
            return Ok(new Response<object> { Data = null, StatusCode = 200, Messager = "success" });
        }

        [HttpPut("submit/{submissionQuizId}")]
        [Authorize]
        [ServiceFilter(typeof(UserCheck))]
        public async Task<ActionResult<Response<object>>> SubmitSubmisstionQuiz(int submissionQuizId)
        {
            var submiss = await _submissionQuizRepo.GetDetail(submissionQuizId);
            if (submiss == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "submiss does not exist" });
            }
            if (submiss.StudentId != Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value))
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "dont own this submission" });
            }
            if (submiss.IsFinish)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "submission had finished" });
            }
            submiss.IsFinish = true;
            submiss.FinishTime = DateTime.UtcNow;
            var quiz = await _questionRepo.GetByQuizId(submiss.QuizId);
            var total = quiz.ToList().Count;
            var correctCount = submiss.Answers.Count(x => x.IsCorrect);
            submiss.Core = ((float)correctCount / total) * 10;
            await _submissionQuizRepo.SaveChange();
            return Ok(new Response<object> { Data = null, StatusCode = 200, Messager = "success" });
        }

        [HttpPost("answer/{submissionQuizId}")]
        [Authorize]
        [ServiceFilter(typeof(UserCheck))]
        public async Task<ActionResult<Response<object>>> SubmitAnswer(int submissionQuizId, int answerId)
        {
            var submiss = await _submissionQuizRepo.GetById(submissionQuizId);
            var answer = await _answerRepo.GetDetail(answerId);
            if (answer.Question.QuizId != submiss.QuizId)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "the answer does not exist in the quiz" });
            }
            if (submiss == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "submission does not exist" });
            }
            if (submiss.IsFinish)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "submission had finished" });
            }
            //check deadline
            var quiz = await _quizRepo.GetById(submiss.QuizId);
            DateTime dueTime = quiz.StartDate.AddMinutes(quiz.Duration);
            if (DateTime.UtcNow > dueTime)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "time out for quiz" });
            }

            if (answer == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "answer does not exist" });
            }

            var answerExist = submiss.Answers.FirstOrDefault(x => x.QuestionId == answer.QuestionId);
            if (answerExist != null)
            {
                submiss.Answers.Remove(answerExist);
            }
            submiss.Answers.Add(answer);
            await _submissionQuizRepo.SaveChange();
            return Ok(new Response<object> { Data = null, StatusCode = 200, Messager = "success" });
        }

        [HttpGet("my-quiz/{quizId}")]
        [Authorize]
        [ServiceFilter(typeof(UserCheck))]
        public async Task<ActionResult<Response<SubmissionQuizReadDto>>> GetByQuizIdAndUser(int quizId)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == "UserId").Value;
            var submissionQuiz = await _submissionQuizRepo.GetByUsernameAndQuizId(Convert.ToInt32(userId), quizId);
            var submissionQuizRead = _mapper.Map<SubmissionQuizReadDto>(submissionQuiz);
            return Ok(new Response<SubmissionQuizReadDto> { Data = submissionQuizRead, StatusCode = 200, Messager = "Success" });
        }

        [HttpGet("by-quiz/{quizId}")]
        [Authorize]
        [ServiceFilter(typeof(UserCheck))]
        public async Task<ActionResult<Response<PageDataDto<SubmissionQuizReadDto>>>> GetByQuizIdAndUser(int quizId, int page, int limit, string searchValue)
        {
            var quiz = await _quizRepo.GetById(quizId);
            if (quiz == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "quizid does not exist" });
            }
            var submissionQuizzes = await _submissionQuizRepo.GetByQuizId(quizId, page * limit, limit, searchValue);
            var submissionQuizzesRead = _mapper.Map<IEnumerable<SubmissionQuizReadDto>>(submissionQuizzes.Data);
            var PageDataDto = new PageDataDto<SubmissionQuizReadDto> { Data = submissionQuizzesRead, Count = submissionQuizzes.Count };
            return Ok(new Response<PageDataDto<SubmissionQuizReadDto>> { Data = PageDataDto, StatusCode = 200, Messager = "success" });
        }

        [HttpGet("{submissionQuizId}")]
        [Authorize]
        [ServiceFilter(typeof(UserCheck))]
        public async Task<ActionResult<Response<SubmissionQuiz>>> GetById(int submissionQuizId)
        {
            var submission = await _submissionQuizRepo.GetDetail(submissionQuizId);
            if (submission == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "submission does not exist" });
            }
            var submissionRead = _mapper.Map<SubmissionQuizReadDto>(submission);
            return Ok(new Response<SubmissionQuizReadDto> { Data = submissionRead, StatusCode = 200, Messager = "success" });
        }

        [HttpGet("my-quiz-in-class/{classId}")]
        [Authorize]
        [ServiceFilter(typeof(UserCheck))]
        public async Task<ActionResult<Response<IEnumerable<SubmissionQuizReadDto>>>> GetOwnedQuiz(int classId)
        {
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            var submissions = await _submissionQuizRepo.GetOwnedSubmissions(classId, userId);
            var submissionRead = _mapper.Map<IEnumerable<SubmissionQuizReadDto>>(submissions);
            return Ok(new Response<IEnumerable<SubmissionQuizReadDto>> { Data = submissionRead, StatusCode = 200, Messager = "success" });
        }
    }

}
