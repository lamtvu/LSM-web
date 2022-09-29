using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LmsBeApp_Group06.Data.Repositories.ClassRepo;
using LmsBeApp_Group06.Data.Repositories.ExerciseRepo;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Filters;
using LmsBeApp_Group06.Models;
using LmsBeApp_Group06.Services.TimeService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LmsBeApp_Group06.Controllers
{
    [ApiController]
    [Route("api/exercise")]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseRepo _exerciseRepo;
        private readonly IMapper _mapper;
        private readonly IClassRepo _classRepo;
        private readonly ITimeService _timeService;
        public ExerciseController(IExerciseRepo exerciseRepo, IClassRepo classRepo, IMapper mapper, ITimeService timeService)
        {
            this._timeService = timeService;
            this._classRepo = classRepo;
            this._mapper = mapper;
            this._exerciseRepo = exerciseRepo;
        }

        ///<summary>them bai tap theo classid</summary>
        [HttpPost("{classId}")]
        [Authorize]
        //  [ServiceFilter(typeof(UserCheck))]
        //  [ServiceFilter(typeof(OwnedClassCheck))]
        public async Task<ActionResult<Response<object>>> CreateExercise(ExerciseCreateDto exerciseCreate, int classId)
        {
            var exercise = _mapper.Map<Exercise>(exerciseCreate);
            var _class = await _classRepo.GetById(classId);
            if (_class == null)
            {
                return BadRequest(new Response<object> { StatusCode = 400, Data = null, Messager = "class does't exist" });
            }

            exercise._Class = _class;
            var time = exerciseCreate.DueTime.Split(':');
            var dueDate = exerciseCreate.DueDate;
            exercise.DueDate = new DateTime(dueDate.Year, dueDate.Month, dueDate.Day, Convert.ToInt32(time[0]), Convert.ToInt32(time[1]), 0).AddHours(-7);
            exercise.CreateDate = DateTime.UtcNow;
            await _exerciseRepo.Create(exercise);
            await _exerciseRepo.SaveChange();
            return Ok(new Response<object> { Data = null, StatusCode = 200, Messager = "success" });
        }

        ///<summary>xoa bai tap theo id</summary>
        [HttpDelete("{exerciseId}")]
        [Authorize]
        [ServiceFilter(typeof(UserCheck))]
        [ServiceFilter(typeof(OwnedExerciseCheck))]
        public async Task<ActionResult<Response<object>>> DeleteExercise(int exerciseId)
        {
            var exercise = await _exerciseRepo.GetById(exerciseId);
            if (exercise == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "exercise dosen't exist" });
            }
            await _exerciseRepo.Delete(exercise);
            await _exerciseRepo.SaveChange();
            return Ok(new Response<object> { Data = null, StatusCode = 200, Messager = "success" });
        }

        ///<summary>sua bai tap theo id</summary>
        [HttpPut("{exerciseId}")]
        [Authorize]
        [ServiceFilter(typeof(UserCheck))]
        [ServiceFilter(typeof(OwnedExerciseCheck))]
        public async Task<ActionResult<Response<object>>> ChangeExercise(int exerciseId, ExerciseChangeDto exerciseChange)
        {
            var exercise = await _exerciseRepo.GetById(exerciseId);
            if (exercise == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "exercise dosen't exist" });
            }
            _mapper.Map(exerciseChange, exercise);
            var time = exerciseChange.DueTime.Split(':');
            var dueDate = exerciseChange.DueDate;
            exercise.DueDate = new DateTime(dueDate.Year, dueDate.Month, dueDate.Day, Convert.ToInt32(time[0]), Convert.ToInt32(time[1]), 0).AddHours(-7);
            await _exerciseRepo.SaveChange();
            return Ok(new Response<object> { Data = null, StatusCode = 200, Messager = "success" });
        }

        ///<summary>Lay bai tap theo id</summary>
        [HttpGet("detail/{exerciseId}")]
        [Authorize]
        // [ServiceFilter(typeof(UserCheck))]
        public async Task<ActionResult<ExerciseReadDto>> GetExerciesById(int exerciseId)
        {
            var exercise = await _exerciseRepo.GetDetail(exerciseId);
            if (exercise == null)
            {
                return BadRequest(new Response<ExerciseReadDto> { Data = null, Messager = "id dosen't exist", StatusCode = 400 });
            }
            var exerciseRead = _mapper.Map<ExerciseReadDto>(exercise);
            return Ok(new Response<ExerciseReadDto> { Data = exerciseRead, Messager = "success", StatusCode = 200 });
        }

        ///<summary>Lay bai tap theo id lop</summary>
        [HttpGet("{classId}")]
        [Authorize]
        // [ServiceFilter(typeof(UserCheck))]
        public async Task<ActionResult<IEnumerable<ExerciseReadDto>>> GetExerciesByClassId(int classId)
        {
            var exercises = await _exerciseRepo.GetByClassId(classId);
            if (exercises == null)
            {
                return BadRequest(new Response<ExerciseReadDto> { Data = null, Messager = "id dosen't exist", StatusCode = 400 });
            }
            var exerciseRead = _mapper.Map<IEnumerable<ExerciseReadDto>>(exercises);
            return Ok(new Response<IEnumerable<ExerciseReadDto>> { Data = exerciseRead, Messager = "success", StatusCode = 200 });
        }

    }
}