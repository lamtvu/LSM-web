using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LmsBeApp_Group06.Data.Repositories.AnswerRepo;
using LmsBeApp_Group06.Data.Repositories.QuestionRepo;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LmsBeApp_Group06.Controllers
{
    [ApiController]
    [Route("api/answer")]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerRepo _answerRepo;
        private readonly IMapper _mapper;
        private readonly IQuestionRepo _questionRepo;
        public AnswerController(IAnswerRepo answerRepo, IMapper mapper, IQuestionRepo questionRepo)
        {
            this._questionRepo = questionRepo;
            this._mapper = mapper;
            this._answerRepo = answerRepo;
        }

        ///<summary> xoa answer theo id</summary>
        [HttpDelete("{answerId}")]
        [Authorize]
        public async Task<ActionResult<Response<object>>> RemoveAnswer(int answerId)
        {
            var answer = await _answerRepo.GetById(answerId);
            if (answer == null)
            {
                return BadRequest(new Response<object> { StatusCode = 400, Data = null, Messager = "answerId does not exist" });
            }
            await _answerRepo.Delete(answer);
            await _answerRepo.SaveChage();
            return Ok(new Response<object> { StatusCode = 200, Data = null, Messager = "success" });
        }

        ///<summary> toa answer theo question id</summary>
        [HttpPost("{questionId}")]
        [Authorize]
        public async Task<ActionResult<Response<object>>> CreateAnswer(AnswerCreateDto answerCreate, int questionId)
        {
            var question = await _questionRepo.GetById(questionId);
            if (question == null)
            {
                return BadRequest(new Response<object> { StatusCode = 400, Data = null, Messager = "questionid does not exist" });
            }
            var answer = _mapper.Map<Answer>(answerCreate);
            answer.Question = question;
            await _answerRepo.Create(answer);
            await _answerRepo.SaveChage();
            return Ok(new Response<object> { StatusCode = 200, Data = null, Messager = "success" });
        }

        ///<summary>lấy danh sach answer cua question id</summary>
        [HttpGet("questionId")]
        [Authorize]
        public async Task<ActionResult<Response<IEnumerable<AnswerReadDto>>>> GetByQuestionId(int questionId)
        {
            var answers = await _answerRepo.GetByQuestionId(questionId);
            var answerReads = _mapper.Map<IEnumerable<AnswerReadDto>>(answers);
            return Ok(new Response<IEnumerable<AnswerReadDto>> { StatusCode = 200, Data = answerReads, Messager = "success" });
        }

        ///<summary>thay doi answer</summary>
        [HttpPut("answerId")]
        [Authorize]
        public async Task<ActionResult<Response<object>>> ChangeAnswer(int answerId, AnswerCreateDto answerChange)
        {
            var answerModel = await _answerRepo.GetById(answerId);
            if (answerModel == null)
            {
                return BadRequest(new Response<object> { StatusCode = 400, Data = null, Messager = "answerId does not exist" });
            }
            _mapper.Map(answerChange, answerModel);
            await _answerRepo.SaveChage();
            return Ok(new Response<object> { StatusCode = 200, Data = null, Messager = "success" });
        }
    }
}
