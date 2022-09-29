using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LmsBeApp_Group06.Data.Repositories.RequestTeacherRepo;
using LmsBeApp_Group06.Data.Repositories.UserRepos;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LmsBeApp_Group06.Controllers
{
    [ApiController]
    [Route("api/request-teacher")]
    public class RequestTeacherController : ControllerBase
    {
        private readonly IRequestTeacherRepo _teacherRepo;
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        public RequestTeacherController(IRequestTeacherRepo teacherRepo, IMapper mapper,
                                        IUserRepo userRepo)
        {
            this._mapper = mapper;
            this._teacherRepo = teacherRepo;
            this._userRepo = userRepo;
        }


        ///<summary>Lấy hết danh sách yêu cầu làm giáo viên</summary>
        ///<response code="200">thành công, có yêu cầu</response>
        ///<response code="204">thành công, không có yêu cầu</response>
        ///<response code="401">token không hợp lệ, không phải admin</response>
        [HttpGet]
        [Route("get-all")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Response<IEnumerable<RequestTeacher>>>> GetAll([FromQuery] string searchQuery)
        {
            var requestTeachers = await _teacherRepo.GetAll(searchQuery);
            if (requestTeachers == null)
            {
                return Ok(new Response<string> { StatusCode = 204, Data = "", Messager = "There is no request." });
            }

            return Ok(new Response<IEnumerable<RequestTeacher>> { StatusCode = 200, Data = requestTeachers, Messager = "success" });
        }

        ///<summary>Tạo yêu cầu giáo viên</summary>
        ///<response code="201">thành công, tao thành công</response>
        ///<response code="400">thất bại, dữ liệu vào không hợp lệ</response>
        ///<response code="401">token không hợp lệ, không phải student</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<ActionResult<Response<RequestTeacher>>> CreateRequest(RequestTeacherCreateDto requestTeacherCreate)
        {
            var username = User.Identity.Name;
            var user = await _userRepo.GetByUsername(username);
            if (user == null)
            {
                return Ok(new Response<string> { StatusCode = 401, Data = "", Messager = "User isn't exist." });
            }

            var requestTeacherModel = _mapper.Map<RequestTeacher>(requestTeacherCreate);                    
            requestTeacherModel.User = user;
            requestTeacherModel.CreateDate = user.CreateDate;

            await _teacherRepo.CreateTeacherRequest(requestTeacherModel);
            await _teacherRepo.SaveChange();

            var requestTeacherReadDto = _mapper.Map<RequestTeacherReadDto>(requestTeacherModel);
            return Created("", new Response<RequestTeacherReadDto> { StatusCode = 201, Messager = "success", Data = requestTeacherReadDto });
        }


        ///<summary>xóa yêu cầu giáo viên</summary>
        ///<response code="202">thành công, xóa thành công</response>
        ///<response code="404">thất bại, không tìm thấy yêu cầu</response>
        ///<response code="401">token không hợp lệ, không phải admin</response>
        [HttpDelete()]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Response<string>>> DeleteRequest(int id)
        {
            var requestTeacherModel = await _teacherRepo.GetById(id);
            if(requestTeacherModel==null)
            {
                return Accepted(new Response<string> { StatusCode = 404, Messager = "Request isn't existed", Data = "" });
            }           
          
            //remove request
            await _teacherRepo.RemoveTeacherRequest(requestTeacherModel);
            await _teacherRepo.SaveChange();

            return Accepted(new Response<string> { StatusCode = 202, Messager = "success", Data = "" });
        }


        ///<summary>Lấy danh sách yêu cầu theo page, limit</summary>
        ///<response code="200">thanh cong</response>
        ///<response code="204">thanh cong, khong co du lieu</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<Response<IEnumerable<RequestTeacher>>>> GetByPage(int page, int limit)
        {
            var requestTeachers = await _teacherRepo.GetByPage(page * limit, limit);
            // no content
            return Ok(new Response<IEnumerable<RequestTeacher>> { StatusCode = 200, Messager = "success", Data = requestTeachers });
        }
    }
}
