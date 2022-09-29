using AutoMapper;
using LmsBeApp_Group06.Data.Repositories.ClassRepo;
using LmsBeApp_Group06.Data.Repositories.CourseRepos;
using LmsBeApp_Group06.Data.Repositories.UserRepos;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Dtos.RequestStudent;
using LmsBeApp_Group06.Models;
using LmsBeApp_Group06.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LmsBeApp_Group06.Controllers
{
    [ApiController]
    [Route("api/request-student")]
    public class RequestStudentController : ControllerBase
    {
        private readonly IRequestStudentRepos _studentRepos;
        private readonly IUserRepo _userRepo;
        private readonly IClassRepo _classRepo;
        private readonly IMapper _mapper;
        private readonly AuthorizeService _authorizeService;
        public RequestStudentController(IRequestStudentRepos studentRepos, IMapper mapper,
                                        IUserRepo userRepo, IClassRepo classRepo,
                                        AuthorizeService authorizeService)
        {
            this._mapper = mapper;
            this._studentRepos = studentRepos;
            this._userRepo = userRepo;
            this._classRepo = classRepo;
            this._authorizeService = authorizeService;
        }

        ///<summary>Lấy danh sách khóa học của teacher theo token</summary>
        [HttpGet("class-request")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PageDataDto<ClassRequestReadDto>>> GetRequestClass(string searchValue)
        {

            var username = User.Identity.Name;
            var allClass = await _classRepo.getAll(searchValue);

            if(allClass.Count()==0)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "There is no class." });
            }

            var classList = new List<ClassRequestReadDto>();
            foreach(var item in allClass)
            {
                var check = await _classRepo.CheckStuding(item.Id, username);
                if(check==false)
                {
                    var data = _mapper.Map<ClassReadDto>(item);
                    var result = await _studentRepos.CheckIfRequested(item.Id, username);
                    var temp = new ClassRequestReadDto();
                    temp.ClassReadDto = data;
                    temp.IsRequest = result;
                    classList.Add(temp);
                }
            }

            if(classList.Count()==0)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "There is no class." });
            } 
            
            var classData=classList as IEnumerable<ClassRequestReadDto>;

            var classReadData = new PageDataDto<ClassRequestReadDto>
            {
                Count = classData.Count(),
                Data = classData
            };
            return Ok(new Response<PageDataDto<ClassRequestReadDto>> { Data = classReadData, StatusCode = 200, Messager = "success" });
        }

        ///<summary>Delete class request </summary>
        ///<response code="201">thành công, tao thành công</response>
        ///<response code="400">thất bại, dữ liệu vào không hợp lệ</response>
        ///<response code="401">token không hợp lệ, không phải student</response>
        [HttpDelete("delete-class-request/{classid}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<ActionResult<Response<string>>> DeleteClassRequest(int classid)
        {
            //Authorize
            var username = User.Identity.Name;
            var user = await _userRepo.GetByUsername(username);
            if (user == null)
            {
                return BadRequest(new Response<string> { StatusCode = 401, Data = "", Messager = "User isn't exist." });
            }

            //check request model 
            var id = await _studentRepos.GetByClassId(classid,username);
            var requestStudentModel= await _studentRepos.GetById(id);

            if (requestStudentModel == null)
            {
                return BadRequest(new Response<string> { StatusCode = 400, Data = "", Messager = "Request is invalid." });
            }

            await _studentRepos.RemoveStudentRequest(requestStudentModel);
            await _studentRepos.SaveChange();

            return Ok(new Response<string> { StatusCode = 200, Messager = "success", Data = "" });
        }

        ///<summary>Lấy hết danh sách yêu cầu tham gia lớp học</summary>
        ///<response code="200">thành công, có yêu cầu</response>
        ///<response code="204">thành công, không có yêu cầu</response>
        ///<response code="401">token không hợp lệ</response>
        [HttpGet("by-class")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Student, Teacher")]
        public async Task<ActionResult<Response<IEnumerable<RequestStudent>>>> GetAll([FromQuery] string searchQuery, int classid)
        {
            //Authorize
            var username = User.Identity.Name;
            var checkAuth = await _authorizeService.IsTeacher(username, classid);
            if (!checkAuth)
            {
                checkAuth = await _authorizeService.IsClassAdmin(username, classid);
                if (!checkAuth)
                {
                    return BadRequest(new Response<string> { StatusCode = 401, Data = "", Messager = "User isn't exist." });
                }
            }

            //get all requests
            var requestStudent = await _studentRepos.GetAll(searchQuery, classid);
            if (requestStudent.Count() == 0)
            {
                return Ok(new Response<string> { StatusCode = 204, Data = "", Messager = "There is no request." });
            }

            var requestStudentReadModel = _mapper.Map<IEnumerable<RequestStudentReadDto>>(requestStudent);
            return Ok(new Response<IEnumerable<RequestStudentReadDto>> { StatusCode = 200, Data = requestStudentReadModel, Messager = "success" });
        }

        ///<summary>Tạo yêu cầu join to class</summary>
        ///<response code="201">thành công, tao thành công</response>
        ///<response code="400">thất bại, dữ liệu vào không hợp lệ</response>
        ///<response code="401">token không hợp lệ, không phải student</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Student, Teacher")]
        public async Task<ActionResult<Response<string>>> CreateRequest(int classid)
        {
            //Authorize
            var username = User.Identity.Name;
            var user = await _userRepo.GetByUsername(username);
            if (user == null)
            {
                return BadRequest(new Response<string> { StatusCode = 401, Data = "", Messager = "User isn't exist." });
            }
             if (user.Verify ==false)
            {
                return BadRequest(new Response<string> { StatusCode = 401, Data = "", Messager = "User doesn't accept." });
            }

            //Get class to join 
            var classToJoin = await _classRepo.GetById(classid);
            Console.WriteLine(classid);
           
            //Create request
            var requestStudentModel = new RequestStudent();
            requestStudentModel.Class = classToJoin;
            requestStudentModel.Sender = user;
            requestStudentModel.CreateDate = DateTime.UtcNow;

            await _studentRepos.CreateStudentRequest(requestStudentModel);
            await _studentRepos.SaveChange();

           // var requestStudentReadDto = _mapper.Map<RequestStudentReadDto>(requestStudentModel);
           return Ok(new Response<string> { StatusCode = 200, Messager = "success", Data = "" });
        }

        ///<summary>Delete request </summary>
        ///<response code="201">thành công, tao thành công</response>
        ///<response code="400">thất bại, dữ liệu vào không hợp lệ</response>
        ///<response code="401">token không hợp lệ, không phải student</response>
        [HttpDelete("{requestId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<ActionResult<Response<string>>> DeleteRequest(int requestId)
        {
            //Authorize
            var username = User.Identity.Name;
            var user = await _userRepo.GetByUsername(username);
            if (user == null)
            {
                return BadRequest(new Response<string> { StatusCode = 401, Data = "", Messager = "User isn't exist." });
            }

            //check request model 
            var requestStudentModel = await _studentRepos.GetById(requestId);
            if (requestStudentModel == null)
            {
                return BadRequest(new Response<string> { StatusCode = 400, Data = "", Messager = "Request is invalid." });
            }

            await _studentRepos.RemoveStudentRequest(requestStudentModel);
            await _studentRepos.SaveChange();

            return Ok(new Response<string> { StatusCode = 200, Messager = "success", Data = "" });
        }

        ///<summary>Delete all requests </summary>
        ///<response code="201">thành công, tao thành công</response>
        ///<response code="400">thất bại, dữ liệu vào không hợp lệ</response>
        ///<response code="401">token không hợp lệ, không phải student</response>
        [HttpDelete("delete-all/{classId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<ActionResult<Response<string>>> DeleteAllRequests(int classId)
        {
            //Authorize
            var username = User.Identity.Name;
            var checkAuth = await _authorizeService.IsTeacher(username, classId);
            if (!checkAuth)
            {
                checkAuth = await _authorizeService.IsClassAdmin(username, classId);
                if (!checkAuth)
                {
                    return BadRequest(new Response<string> { StatusCode = 401, Data = "", Messager = "User isn't exist." });
                }
            }

            //check request model 
            var requestStudentModel = await _studentRepos.GetAll(classId);
            if (requestStudentModel.Count() == 0)
            {
                return BadRequest(new Response<string> { StatusCode = 400, Data = "", Messager = "There is no request." });
            }

            await _studentRepos.RemoveStudentAllRequest(requestStudentModel);
            await _studentRepos.SaveChange();

            return Ok(new Response<string> { StatusCode = 200, Messager = "success", Data = "" });
        }


        ///<summary>Lấy danh sách yêu cầu theo page, limit</summary>
        ///<response code="200">thanh cong</response>
        ///<response code="204">thanh cong, khong co du lieu</response>
        [HttpGet("get-by-page/{classId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize]
        public async Task<ActionResult<Response<PageDataDto<RequestStudentReadDto>>>> GetByPage(int classId, int page, int limit)
        {
            //Authorize
            var username = User.Identity.Name;
            var checkAuth = await _authorizeService.IsTeacher(username, classId);
            if (!checkAuth)
            {
                checkAuth = await _authorizeService.IsClassAdmin(username, classId);
                if (!checkAuth)
                {
                    return BadRequest(new Response<string> { StatusCode = 401, Data = "", Messager = "User isn't exist." });
                }
            }
            //get all requests
            var requestStudent = await _studentRepos.GetByPage(classId, page * limit, limit);
            var pageData = new PageDataDto<RequestStudentReadDto>
            {
                Data = _mapper.Map<IEnumerable<RequestStudentReadDto>>(requestStudent.Data),
                Count = requestStudent.Count
            };
            return Ok(new Response<PageDataDto<RequestStudentReadDto>> { StatusCode = 200, Data = pageData, Messager = "success" });
        }
    }
}
