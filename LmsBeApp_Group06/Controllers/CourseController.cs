using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using LmsBeApp_Group06.Data.Repositories.CourseRepos;
using LmsBeApp_Group06.Data.Repositories.UserRepos;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Filters;
using LmsBeApp_Group06.Models;
using LmsBeApp_Group06.Services.BlobService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace LmsBeApp_Group06.Controllers
{
    [ApiController]
    [Route("api/course")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepo _courseRepo;
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly IBlobService _blobService;
        public CourseController(ICourseRepo courseRepo, IUserRepo userRepo, IMapper mapper
        , IBlobService blobService)
        {
            this._blobService = blobService;
            this._mapper = mapper;
            this._userRepo = userRepo;
            this._courseRepo = courseRepo;

        }

        ///<summary>Tạo khóa học</summary>
        ///<response code="403">Role khong hop le</response>
        ///<response code="401">token khong hop le</response>
        [HttpPost]
        [Authorize(Roles = "Instructor")]
        public async Task<ActionResult<Response<object>>> CreateCourse([FromForm] CourseCreateDto courseCreate)
        {
            var username = User.Identity.Name;
            var user = await _userRepo.GetByUsername(username);
            if (user == null)
            {
                return Unauthorized(new Response<object> { Data = null, StatusCode = 401, Messager = "user does not exist" });
            }
            if (Request.Form.Files.Count < 0)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "required image file" });
            }
            var file = Request.Form.Files[0];
            var filename = file.FileName;
            var fileExt = Path.GetExtension(filename).Substring(1);
            if (fileExt != "png" && fileExt != "jpg" && fileExt != "jpeg")
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "invalid type file (png, jpg, jpeg)" });
            }

            var courseModel = _mapper.Map<Course>(courseCreate);
            courseModel.Image = filename;
            courseModel.Instructor = user;
            await _courseRepo.Create(courseModel);
            await _courseRepo.SaveChange();
            await _blobService.UploadFileBlobAsync(file, "courseimage" + courseModel.Id + courseModel.Image, "coursecontainer");
            return Created("", new Response<object> { Data = null, StatusCode = 201, Messager = "success" });
        }

        ///<summary>Xóa khóa học</summary>
        ///<response code="403">Role khong hop le</response>
        ///<response code="401">token khong hop le</response>
        [HttpDelete("{courseId}")]
        [Authorize(Roles = "Instructor")]
        [ServiceFilter(typeof(OwnedCourseCheck))]
        public async Task<ActionResult<Response<object>>> DeleteCourse(int courseId)
        {
            var courseModel = await _courseRepo.GetById(courseId);
            if (courseModel == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "course not found" });
            }
            await _courseRepo.Delete(courseModel);
            await _courseRepo.SaveChange();
            return Accepted(new Response<object> { Data = null, StatusCode = 202, Messager = "success" });
        }

        ///<summary>Lấy danh sách khóa học đã tạo</summary>
        ///<response code="403">Role khong hop le</response>
        ///<response code="401">token khong hop le</response>
        [HttpGet("get-owned")]
        [Authorize(Roles = "Instructor")]
        public async Task<ActionResult<Response<PageDataDto<CourseReadDto>>>> GetOwned(string searchValue, int page, int limit)
        {
            var username = User.Identity.Name;
            var courses = await _courseRepo.GetByOwner(searchValue, username, page * limit, limit);
            var pageData = new PageDataDto<CourseReadDto>
            {
                Data = _mapper.Map<IEnumerable<CourseReadDto>>(courses.Data),
                Count = courses.Count
            };
            return Ok(new Response<PageDataDto<CourseReadDto>> { Data = pageData, StatusCode = 200, Messager = "success" });
        }

        ///<summary>Lấy danh sách khóa học có trong lớp</summary>
        ///<response code="403">Role khong hop le</response>
        ///<response code="401">token khong hop le</response>
        [HttpGet("get-by-class/{classId}")]
        [Authorize]
        public async Task<ActionResult<Response<PageDataDto<CourseReadDto>>>> GetByClassInUse(string searchValue, int page, int limit, int classId)
        {
            var username = User.Identity.Name;
            var courses = await _courseRepo.GetByClassInUse(searchValue, classId, page * limit, limit);
            var pageData = new PageDataDto<CourseReadDto>
            {
                Data = _mapper.Map<IEnumerable<Course>, IEnumerable<CourseReadDto>>(courses.Data),
                Count = courses.Count
            };
            return Ok(new Response<PageDataDto<CourseReadDto>> { Data = pageData, StatusCode = 200, Messager = "success" });
        }

        ///<summary>Lấy hết danh sách khóa học</summary>
        ///<response code="403">Role khong hop le</response>
        ///<response code="401">token khong hop le</response>
        [HttpGet]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Response<PageDataDto<CourseReadDto>>>> GetAll(string searchValue, int page, int limit)
        {
            var username = User.Identity.Name;
            var courses = await _courseRepo.GetAll(searchValue, page * limit, limit);
            var pageData = new PageDataDto<CourseReadDto>
            {
                Data = _mapper.Map<IEnumerable<Course>, IEnumerable<CourseReadDto>>(courses.Data),
                Count = courses.Count
            };
            return Ok(new Response<PageDataDto<CourseReadDto>> { Data = pageData, StatusCode = 200, Messager = "success" });
        }

        [HttpGet("{courseId}")]
        [Authorize]
        public async Task<ActionResult<Response<CourseReadDto>>> GetById(int courseId)
        {
            var course = await _courseRepo.GetById(courseId);
            if (course == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "course not found" });
            }
            var courseReadDto = _mapper.Map<CourseReadDto>(course);
            return Ok(new Response<CourseReadDto> { Data = courseReadDto, StatusCode = 200, Messager = "success" });
        }

        ///<summary>Chinh sua khoa hoc</summary>
        [HttpPut("{courseId}")]
        [Authorize(Roles = "Instructor")]
        [ServiceFilter(typeof(OwnedCourseCheck))]
        public async Task<ActionResult<Response<PageDataDto<object>>>> ChangeCourse([FromForm] CourseChangeDto courseChangeDto, int courseId)
        {
            if (Request.Form.Files.Count < 0)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "required image file" });
            }
            var file = Request.Form.Files[0];
            var filename = file.FileName;
            var fileExt = Path.GetExtension(filename).Substring(1);
            if (fileExt != "png" && fileExt != "jpg" && fileExt != "jpeg")
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "invalid type file (png, jpg, jpeg)" });
            }
            var course = await _courseRepo.GetById(courseId);
            if (course == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "not found course" });
            }
            if (!String.IsNullOrWhiteSpace(course.Image))
            {
                await _blobService.DeleteBlobAsync("courseimage" + courseId + course.Image, "coursecontainer");
                course.Image = "";
            }
            _mapper.Map(courseChangeDto, course);
            course.Image = filename;
            await _blobService.UploadFileBlobAsync(file, "courseimage" + courseId + course.Image, "coursecontainer");
            await _courseRepo.SaveChange();
            return Ok(new Response<PageDataDto<object>> { Data = null, StatusCode = 200, Messager = "success" });
        }

        [HttpGet("image/{courseId}")]
        public async Task<ActionResult<Response<object>>> GetImage(int courseId)
        {
            var course = await _courseRepo.GetById(courseId);
            if (course == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "not found course" });
            }
            if (String.IsNullOrWhiteSpace(course.Image))
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "this course has not image" });
            }
            var stream = await _blobService.GetBlobStreamAsync("courseimage" + courseId + course.Image, "coursecontainer");
            string contentType;
            new FileExtensionContentTypeProvider().TryGetContentType(course.Image, out contentType);
            return new FileStreamResult(stream.Content, contentType);
        }

        //tui them vo
        
        ///<summary>Thay đổi isLook </summary>
        ///<response code="202">thay đổi thành công</response>
        ///<response code="400">id user không hợp lệ</response>
        [HttpPut("change-lock/{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response<string>>> ChangeLock(int id)
        {           
            var courseModel =await _courseRepo.GetById(id);
            if (courseModel == null)
            {
                return Unauthorized(new Response<string> { StatusCode = 401, Data = null, Messager = "Course isn't exist." });
            }

           if (courseModel.IsLook==true)
            {
                courseModel.IsLook = false;
                await _courseRepo.Change(courseModel);
                await _userRepo.SaveChange();
                return Ok(new Response<string> { StatusCode = 202, Data = null, Messager = "Course is unlocked." });
            }                

            courseModel.IsLook = true;
            await _courseRepo.Change(courseModel);
            await _courseRepo.SaveChange();
            return Ok(new Response<string> { StatusCode = 202, Data = "", Messager = "Course is locked." });
        }

        ///<summary>Lấy hết danh sách courses</summary>
        ///<response code="200">thành công</response>
        ///<response code="204">thành công, nhưng ko có dữ liệu</response>
        [HttpGet("get-all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response<PageDataDto<CourseReadDto>>>> GetAll([FromQuery] string searchValue)
        {
            var courses = await _courseRepo.GetAll(searchValue);
            var courseReadData = new PageDataDto<CourseReadDto>
            {
                Count = courses.Count,
                Data = _mapper.Map<IEnumerable<Course>, IEnumerable<CourseReadDto>>(courses.Data)
            };
            return Ok(new Response<PageDataDto<CourseReadDto>> { StatusCode = 200, Data = courseReadData, Messager = "success" });
        }
    }
}
