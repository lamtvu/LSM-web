using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using LmsBeApp_Group06.Data.Repositories.ClassRepo;
using LmsBeApp_Group06.Data.Repositories.CourseRepos;
using LmsBeApp_Group06.Data.Repositories.UserRepos;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Filters;
using LmsBeApp_Group06.Models;
using LmsBeApp_Group06.Options;
using LmsBeApp_Group06.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LmsBeApp_Group06.Controllers
{
    [ApiController]
    [Route("api/class")]
    public class ClassController : ControllerBase
    {
        private readonly IClassRepo _classRepo;
        private readonly IMapper _mapper;
        private readonly IUserRepo _userRepo;
        private readonly JwtService _jwtService;
        private readonly string _inviteSecretKey;
        private readonly int _inviteLifetime;
        private readonly MailService _mailService;
        private readonly ICourseRepo _courseRepo;
        public ClassController(IClassRepo classRepo, IUserRepo userRepo, IMapper mapper, JwtService jwtService,
        IOptions<JwtOption> jwtOption, MailService mailService, ICourseRepo courseRepo)
        {
            this._courseRepo = courseRepo;
            this._mailService = mailService;
            this._jwtService = jwtService;
            this._userRepo = userRepo;
            this._mapper = mapper;
            this._classRepo = classRepo;
            this._inviteLifetime = jwtOption.Value.InivteLifetime;
            this._inviteSecretKey = jwtOption.Value.InviteSecretKey;
        }

        ///<summary>Lấy danh sách khóa học của teacher theo token</summary>
        [HttpGet("owned")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PageDataDto<ClassReadDto>>> GetOwnedClass(int page, int limit, string searchValue)
        {
            if (limit == 0)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "invalid limit" });
            }
            var username = User.Identity.Name;
            var classData = await _classRepo.GetOwnedClass(page * limit, limit, username, searchValue);
            var classReadData = new PageDataDto<ClassReadDto>
            {
                Count = classData.Count,
                Data = _mapper.Map<IEnumerable<Class>, IEnumerable<ClassReadDto>>(classData.Data)
            };
            return Ok(new Response<PageDataDto<ClassReadDto>> { Data = classReadData, StatusCode = 200, Messager = "success" });
        }

        ///<summary>Lấy hết danh sách lớp học theo page</summary>
        [HttpGet()]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PageDataDto<ClassReadDto>>> getAll(int page, int limit, string searchValue)
        {
            if (limit == 0)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "invalid limit" });
            }
            var classData = await _classRepo.GetAll(page * limit, limit, searchValue);
            var classReadData = new PageDataDto<ClassReadDto>
            {
                Count = classData.Count,
                Data = _mapper.Map<IEnumerable<Class>, IEnumerable<ClassReadDto>>(classData.Data)
            };
            return Ok(new Response<PageDataDto<ClassReadDto>> { Data = classReadData, StatusCode = 200, Messager = "success" });
        }
        ///<summary>Lấy danh sách lớp học đang học theo token</summary>
        [HttpGet("studing")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PageDataDto<ClassReadDto>>> GetStudingClass(int page, int limit)
        {
            var username = User.Identity.Name;
            var classes = await _classRepo.GetStudingClass(page * limit, limit, username);
            var classReadData = new PageDataDto<ClassReadDto>
            {
                Count = classes.Count,
                Data = _mapper.Map<IEnumerable<Class>, IEnumerable<ClassReadDto>>(classes.Data)

            };
            return Ok(new Response<PageDataDto<ClassReadDto>> { Data = classReadData, StatusCode = 200, Messager = "success" });
        }

        ///<summary>Lấy danh sách lớp học đang học theo token</summary>
        [HttpGet("studing-search")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PageDataDto<ClassReadDto>>> GetStudingClassById(int page, int limit, string searchValue)
        {
            var username = User.Identity.Name;
            var classData = await _classRepo.GetStudingClass(page * limit, limit, searchValue);
            var ClassReadData = new PageDataDto<ClassReadDto>
            {
                Count = classData.Count,
                Data = _mapper.Map<IEnumerable<Class>, IEnumerable<ClassReadDto>>(classData.Data)
            };
            return Ok(new Response<PageDataDto<ClassReadDto>> { Data = ClassReadData, StatusCode = 200, Messager = "success" });
        }

        ///<summary>Lấy theo id</summary>
        [HttpGet("{classId}")]
        [Authorize]
        public async Task<ActionResult<Response<ClassReadDto>>> GetById(int classId)
        {
            var _class = await _classRepo.GetById(classId);
            if (_class == null)
            {
                return BadRequest(new Response<object> { Data = null, Messager = "not found", StatusCode = 400 });
            }
            var classRead = _mapper.Map<ClassReadDto>(_class);
            return Ok(new Response<ClassReadDto> { Data = classRead, StatusCode = 200, Messager = "success" });
        }

        ///<summary> Tạo lớp</summary>
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ServiceFilter(typeof(UserCheck))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Response<ClassReadDto>>> CreateClass(ClassCreateDtos ClassCreate)
        {
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            var classModel = _mapper.Map<Class>(ClassCreate);
            classModel.TeacherId = userId;
            classModel.CreateDate = DateTime.UtcNow;
            await _classRepo.Create(classModel);
            await _classRepo.SaveChange();
            var classRead = _mapper.Map<ClassReadDto>(classModel);
            return Created("", new Response<ClassReadDto> { Data = classRead, StatusCode = 200, Messager = "success" });
        }

        ///<summary>Xóa lớp</summary>
        [HttpDelete("{classId}")]
        [Authorize(Roles = "Teacher")]
        [ServiceFilter(typeof(OwnedClassCheck))]
        public async Task<ActionResult<Response<string>>> DeleteClass(int classId)
        {
            var classModel = await _classRepo.GetById(classId);
            await _classRepo.Delete(classModel);
            await _classRepo.SaveChange();
            var classReadDtos = _mapper.Map<ClassReadDto>(classModel);
            return Accepted(new Response<ClassReadDto> { Data = classReadDtos, StatusCode = 200, Messager = "success" });
        }

        ///<summary>Chỉnh sủa thông tin lớp</summary>
        [HttpPut("{classId}")]
        [Authorize]
        [ServiceFilter(typeof(OwnedClassCheck))]
        public async Task<ActionResult<Response<string>>> ChangeInforClass(int classId, ClassChangeDtos classChangeDtos)
        {
            var classModel = await _classRepo.GetById(classId);
            _mapper.Map(classChangeDtos, classModel);
            await _classRepo.SaveChange();
            return BadRequest(new Response<string> { Data = null, StatusCode = 200, Messager = "success" });
        }

        [HttpPost("assistant/{classId}")]
        [Authorize]
        [ServiceFilter(typeof(OwnedClassCheck))]
        public async Task<ActionResult<Response<ClassReadDto>>> ChooseAssistant(int classId, UsernameDtos user)
        {
            var classModel = await _classRepo.GetDetail(classId);
            var student = classModel.students.FirstOrDefault(x => x.Username == user.username);
            if (student == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = user.username + " not in class" });
            }
            classModel.AssistantId = student.Id;
            await _classRepo.SaveChange();
            var classRead = _mapper.Map<ClassReadDto>(classModel);
            return Ok(new Response<ClassReadDto> { Data = classRead, StatusCode = 200, Messager = "success" });
        }

        [HttpPost("class-admin/{classId}")]
        [Authorize]
        [ServiceFilter(typeof(OwnedClassCheck))]
        public async Task<ActionResult<Response<ClassReadDto>>> ChooseClassAdmin(int classId, UsernameDtos user)
        {
            var classModel = await _classRepo.GetDetail(classId);
            var student = classModel.students.FirstOrDefault(x => x.Username == user.username);
            if (student == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = user.username + " not in class" });
            }
            classModel.ClassAdminId = student.Id;
            await _classRepo.SaveChange();
            var classRead = _mapper.Map<ClassReadDto>(classModel);
            return Ok(new Response<ClassReadDto> { Data = classRead, StatusCode = 200, Messager = "success" });
        }

        ///<summary>Thêm học sinh vào lớp</summary>
        [HttpPost("students/{classId}")]
        [Authorize]
        public async Task<ActionResult<Response<string>>> ChangeClassAdmin(int classId, UsernameDtos usernameDtos)
        {
            var classModel = await _classRepo.GetDetail(classId);
            var student = classModel.students.FirstOrDefault(x => x.Username == usernameDtos.username);
            if (student != null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = usernameDtos.username + " already exist in class" });
            }
            var user = await _userRepo.GetByUsername(usernameDtos.username);
            classModel.students.Add(user);
            await _classRepo.SaveChange();
            return Ok(new Response<string> { Data = null, Messager = "success", StatusCode = 200 });
        }

        ///<summary>Xóa học sinh trong lớp</summary>
        [HttpPut("students/{classId}")]
        [Authorize]
        public async Task<ActionResult<Response<string>>> DeleteClassAdmin(int classId, UsernameDtos usernameDtos)
        {
            var user = await _userRepo.GetByUsername(usernameDtos.username);
            var _class = await _classRepo.GetById(classId);
            _class.students.Remove(user);
            await _classRepo.SaveChange();
            return Ok(new Response<string> { Data = null, Messager = "success", StatusCode = 200 });
        }

        ///<summary>lấy học sinh trong lớp </summary>
        [HttpGet("students/{classId}")]
        [Authorize]
        public async Task<ActionResult<Response<PageDataDto<UserReadDto>>>> GetStudent(int classId, int page, int limit)
        {
            var users = await _classRepo.GetStudents(classId, page * limit, limit);
            var userData = new PageDataDto<UserReadDto>
            {
                Count = users.Count,
                Data = _mapper.Map<IEnumerable<User>, IEnumerable<UserReadDto>>(users.Data)
            };
            return Ok(new Response<PageDataDto<UserReadDto>> { Data = userData, Messager = "success", StatusCode = 200 });
        }

        [HttpPost("students/invite/{classId}")]
        // [Authorize]
        public async Task<ActionResult<Response<object>>> InviteStudent(int classId, UsernameDtos user)
        {
            var userModel = await _userRepo.GetByUsername(user.username);
            if (userModel == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "username not found" });
            }
            var classModel = await _classRepo.GetById(classId);
            if (classModel == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "class not found" });
            }
            var Claims = new Claim[]{
                new Claim(ClaimTypes.Name, user.username),
                new Claim("classId", classId.ToString())
            };
            var token = _jwtService.GenerateToken(Claims, this._inviteSecretKey, this._inviteLifetime);
            var urlToken = "https://lmsfe.azurewebsites.net/students/accept?token=" + token;
            var mailContent = new MailContent
            {
                To = userModel.Email,
                Subject = "Class Invitation",
                Body = MailHtmlForm.InviteForm(userModel, classModel, urlToken, "techedu.ltv@gmail.com")
            };
            await _mailService.SendMail(mailContent);
            return Ok(new Response<object> { Data = null, Messager = "success", StatusCode = 200 });
        }

        [HttpGet("/students/accept")]
        public async Task<ActionResult<Response<object>>> AcceptIvitation(string token)
        {
            if (token == null || token.Length == 0)
            {
                return BadRequest(new Response<string> { Data = "err", StatusCode = 400, Messager = "requied token" });
            }
            var claims = _jwtService.GetClaimsPrincipal(token, this._inviteSecretKey);
            var username = claims?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            var classId = claims?.Claims.FirstOrDefault(x => x.Type == "classId").Value;
            var user = await _userRepo.GetByUsername(username);
            var classModel = await _classRepo.GetDetail(Convert.ToInt32(classId));
            if (user == null || classModel == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "invalid token" });
            }
            var student = classModel.students.FirstOrDefault(x => x.Username == username);
            if (student != null)
            {
                return Redirect("https://lmsfe.azurewebsites.net/student/class/" + classId);
            }
            classModel.students.Add(user);
            await _classRepo.SaveChange();
            return Redirect("https://lmsfe.azurewebsites.net/student/class/" + classId);
        }

        [HttpPost("courses/{classId}")]
        [Authorize]
        [ServiceFilter(typeof(OwnedClassCheck))]
        public async Task<ActionResult<Response<object>>> AddCourseToClass(int classId, int courseId)
        {
            var classModel = await _classRepo.GetDetail(classId);
            if (classModel == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "not found class" });
            }
            var course = await _courseRepo.GetById(courseId);
            if (course == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "not found course" });
            }
            classModel.Courses.Add(course);
            await _classRepo.SaveChange();
            return Ok(new Response<object> { Data = null, StatusCode = 200, Messager = "success" });
        }

        [HttpDelete("courses/{classId}")]
        [Authorize]
        [ServiceFilter(typeof(OwnedClassCheck))]
        public async Task<ActionResult<Response<object>>> DeleteCourseInClass(int classId, int courseId)
        {
            var classModel = await _classRepo.GetDetail(classId);
            if (classModel == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "not found class" });
            }
            var course = classModel.Courses.FirstOrDefault(x=>x.Id == courseId);
            if (course == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "not found course" });
            }
            classModel.Courses.Remove(course);
            await _classRepo.SaveChange();
            return Ok(new Response<object> { Data = null, StatusCode = 200, Messager = "success" });
        }
    }
}
