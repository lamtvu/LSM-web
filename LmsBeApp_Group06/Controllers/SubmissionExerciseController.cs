using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LmsBeApp_Group06.Data.Repositories.ExerciseRepo;
using LmsBeApp_Group06.Data.Repositories.SubmisstionExerciseRepo;
using LmsBeApp_Group06.Data.Repositories.UserRepos;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Filters;
using LmsBeApp_Group06.Models;
using LmsBeApp_Group06.Services.BlobService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace LmsBeApp_Group06.Controllers
{
    [ApiController]
    [Route("api/submission-exercise")]
    public class SubmissionExerciseController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISubmissionExerciseRepo _submissionExerciseRepo;
        private readonly IUserRepo _userRepo;
        private readonly IExerciseRepo _exerciseRepo;
        private readonly IBlobService _blobService;
        public SubmissionExerciseController(IUserRepo userRepo, ISubmissionExerciseRepo submissionExerciseRepo, IMapper mapper, IExerciseRepo exerciseRepo, IBlobService blobService)
        {
            this._blobService = blobService;
            this._exerciseRepo = exerciseRepo;
            this._userRepo = userRepo;
            this._submissionExerciseRepo = submissionExerciseRepo;
            this._mapper = mapper;
        }

        [HttpPost("{exerciseId}")]
        [Authorize]
        [ServiceFilter(typeof(UserCheck))]
        public async Task<ActionResult<Response<object>>> CreateSubmissionExercise(int exerciseId)
        {
            var exercise = await _exerciseRepo.GetById(exerciseId);
            if (exercise == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "exercise dosen't exist" });
            }
            if (Request.Form.Files.Count == 0)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "required file" });
            }
            var submission = new SubmissionExercise();
            var file = Request.Form.Files[0];
            submission.Exercise = exercise;
            submission.StudentId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            submission.SubmitDate = System.DateTime.UtcNow;
            submission.FileType = file.FileName;
            await _submissionExerciseRepo.Create(submission);
            await _submissionExerciseRepo.SaveChange();
            //save file
            await _submissionExerciseRepo.SaveChange();
            await _blobService.UploadFileBlobAsync(file, "exercise" + submission.Id + submission.FileType, "exercisecontainer");
            return Ok(new Response<object> { Data = null, StatusCode = 200, Messager = "succes" });
        }

        [HttpDelete("{submisstionExerciseId}")]
        [Authorize]
        [ServiceFilter(typeof(UserCheck))]
        public async Task<ActionResult<Response<object>>> DeleteSubmissionExercise(int submisstionExerciseId)
        {
            var submission = await _submissionExerciseRepo.GetById(submisstionExerciseId);
            if (submission == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "submisstionExerciseId dosen't exist" });
            }
            //delete file
            await _blobService.DeleteBlobAsync("exercise" + submission.Id + submission.FileType, "exercisecontainer");
            //
            await _submissionExerciseRepo.Delete(submission);
            await _submissionExerciseRepo.SaveChange();
            return Ok(new Response<object> { Data = null, StatusCode = 200, Messager = "success" });
        }

        [HttpPut("{submisstionExerciseId}")]
        [Authorize]
        [ServiceFilter(typeof(UserCheck))]
        public async Task<ActionResult<Response<object>>> ChangeSubmissionExercise(int submisstionExerciseId)
        {
            var submissModel = await _submissionExerciseRepo.GetById(submisstionExerciseId);
            if (submissModel == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "submisstionExerciseId dosen't exist" });
            }
            if (Request.Form.Files.Count == 0)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "required file" });
            }
            submissModel.SubmitDate = System.DateTime.UtcNow;
            //delete file
            await _blobService.DeleteBlobAsync("exercise" + submissModel.Id + submissModel.FileType, "exercisecontaner");
            //save new file
            var file = Request.Form.Files[0];
            submissModel.FileType = file.FileName;
            await _submissionExerciseRepo.SaveChange();
            await _blobService.UploadFileBlobAsync(file, "exercise" + submissModel.Id + submissModel.FileType, "exercisecontainer");

            return Ok(new Response<object> { Data = null, StatusCode = 200, Messager = "success" });
        }


        [HttpGet("by-page/{exerciseId}")]
        [Authorize]
        [ServiceFilter(typeof(UserCheck))]
        public async Task<ActionResult<Response<PageDataDto<SubmissionExerciseReadDto>>>> GetSubmissionByExerciseId(int exerciseId, int page, int limit, string searchValue)
        {
            var submisstion = await _submissionExerciseRepo.GetByExerciseId(exerciseId, page * limit, limit, searchValue);
            var submisstionRead = new PageDataDto<SubmissionExerciseReadDto>
            {
                Data = _mapper.Map<IEnumerable<SubmissionExerciseReadDto>>(submisstion.Data),
                Count = submisstion.Count
            };
            return Ok(new Response<PageDataDto<SubmissionExerciseReadDto>> { Data = submisstionRead, StatusCode = 200, Messager = "success" });
        }

        [HttpGet("{exerciseId}")]
        [Authorize]
        [ServiceFilter(typeof(UserCheck))]
        public async Task<ActionResult<Response<SubmissionExerciseReadDto>>> GetSubmisstion(int exerciseId)
        {
            var username = User.Identity.Name;
            var user = await _userRepo.GetByUsername(username);
            if (user == null)
            {
                return Unauthorized(new Response<object> { Data = null, Messager = "invalue token", StatusCode = 401 });
            }
            var submisstion = await _submissionExerciseRepo.GetByExerciseIdAndUserID(user.Id, exerciseId);
            var submissionRead = _mapper.Map<SubmissionExerciseReadDto>(submisstion);
            return Ok(new Response<SubmissionExerciseReadDto> { Data = submissionRead, Messager = "success", StatusCode = 200 });
        }

        [HttpPut("change-score/{submissionId}")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult<Response<object>>> ChangeScore(int submissionId, SubmissionExerciseChangeDto changeDto)
        {
            var submission = await _submissionExerciseRepo.GetById(submissionId);
            if (submission == null)
            {
                return BadRequest(new Response<object> { Data = null, Messager = "not found submission", StatusCode = 200 });
            }
            _mapper.Map(changeDto, submission);
            await _submissionExerciseRepo.SaveChange();
            return Ok(new Response<object> { Data = null, Messager = "Success", StatusCode = 200 });
        }

        [HttpGet("file/{submissionId}")]
        [Authorize]
        [ServiceFilter(typeof(UserCheck))]
        public async Task<ActionResult> GetFile(int submissionId)
        {
            var submission = await _submissionExerciseRepo.GetById(submissionId);
            var fileName = "exercise" + submission.Id + submission.FileType;
            var streamBlobDowload = await _blobService.GetBlobStreamAsync(fileName, "exercisecontainer");
            string contentType;
            new FileExtensionContentTypeProvider().TryGetContentType(submission.FileType, out contentType);
            return File(streamBlobDowload.Content, contentType, submission.FileType);
        }


        [HttpGet("owned-in-class/{classId}")]
        [Authorize]
        [ServiceFilter(typeof(UserCheck))]
        public async Task<ActionResult<IEnumerable<SubmissionExerciseReadDto>>> GetSubmissInClass(int classId)
        {
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            var submissions = await _submissionExerciseRepo.GetOwnedInClass(classId, userId);
            if (submissions == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "not found submissions"});
            }
            var submissionsRead = _mapper.Map<IEnumerable<SubmissionExerciseReadDto>>(submissions);
            return Ok(new Response<IEnumerable<SubmissionExerciseReadDto>> { Data = submissionsRead, StatusCode = 200, Messager = "success" });
        }
    }
}
