using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LmsBeApp_Group06.Data.Repositories.AnswerRepo;
using LmsBeApp_Group06.Data.Repositories.QuestionRepo;
using LmsBeApp_Group06.Data.Repositories.QuizRepo;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;
using Microsoft.AspNetCore.Mvc;

namespace LmsBeApp_Group06.Controllers
{
    [ApiController]
    [Route("api/question")]
    public class QuestionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IQuestionRepo _questionRepo;
        private readonly IQuizRepo _quizRepo;
        private readonly IAnswerRepo _answerRepo;
        public QuestionController(IMapper mapper, IQuestionRepo questionRepo, IQuizRepo quizRepo, IAnswerRepo answerRepo)
        {
            this._answerRepo = answerRepo;
            this._quizRepo = quizRepo;
            this._questionRepo = questionRepo;
            this._mapper = mapper;
        }

        [HttpPost("{quizId}")]
        public async Task<ActionResult<Response<object>>> CreateQuestion(int quizId,QuestionCreateDto questionCreate)
        {
            var quiz = await _quizRepo.GetById(quizId);
            if (quiz == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "quiz does not exist" });
            }
            var question = _mapper.Map<Question>(questionCreate);
            question.Quiz = quiz;
            await _questionRepo.Create(question);
            await _questionRepo.SaveChange();

            return Ok(new Response<object> { Data = null, StatusCode = 200, Messager = "success" });
        }

        [HttpDelete("{questionId}")]
        public async Task<ActionResult<Response<object>>> DeleteQuestion(int questionId)
        {
            var question = await _questionRepo.GetById(questionId);
            if (question == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "question does not exist" });
            }
            await _questionRepo.Delete(question);
            await _questionRepo.SaveChange();
            return Ok(new Response<object> { Data = null, StatusCode = 200, Messager = "success" });
        }

        [HttpPut("{questionId}")]
        public async Task<ActionResult<Response<QuestionReadDto>>> ChangeQuestion(int questionId, QuestionChangeDto questionChange)
        {
            var question = await _questionRepo.GetById(questionId);
            if (question == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "question does not exist" });
            }
            await _questionRepo.Delete(question);
            var questionModel = _mapper.Map<Question>(questionChange);
            await _questionRepo.Create(questionModel);
            questionModel.QuizId = question.QuizId;
            await _questionRepo.SaveChange();
            var questionRead = _mapper.Map<QuestionReadDto>(questionModel);
            return Ok(new Response<QuestionReadDto> { Data = questionRead, StatusCode = 400, Messager = "question does not exist" });
        }

        [HttpGet("{questionId}")]
        public async Task<ActionResult<Response<QuestionReadDto>>> GetById(int questionId)
        {
            var question = await _questionRepo.GetDetail(questionId);
            if (question == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "question does not exist" });
            }
            var questionRead = _mapper.Map<QuestionReadDto>(question);
            return Ok(new Response<QuestionReadDto> { StatusCode = 200, Data = questionRead, Messager = "success" });
        }

        [HttpGet("by-quiz-id/{quizId}")]
        public async Task<ActionResult<Response<IEnumerable<QuestionReadIdDto>>>> GetIdByQuiz(int quizId)
        {
            var quiz = await _quizRepo.GetById(quizId);
            if (quiz == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "quiz does not exist" });
            }
            var question = await _questionRepo.GetByQuizId(quizId); 
            var questionReadId = _mapper.Map<IEnumerable<QuestionReadIdDto>>(question);
            return Ok(new Response<IEnumerable<QuestionReadIdDto>>{Data = questionReadId, Messager = "success", StatusCode = 200});
        }

    }
}
