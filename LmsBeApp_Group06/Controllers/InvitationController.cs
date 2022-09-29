using AutoMapper;
using LmsBeApp_Group06.Data.Repositories.ClassRepo;
using LmsBeApp_Group06.Data.Repositories.InvitationRepos;
using LmsBeApp_Group06.Data.Repositories.UserRepos;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Dtos.Invitation;
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
    [Route("api/invitation")]
    public class InvitationController : ControllerBase
    {
        private readonly IInvitationRepos _invitationRepos;
        private readonly IUserRepo _userRepo;
        private readonly IClassRepo _classRepo;
        private readonly AuthorizeService _authorizeService;
        private readonly IMapper _mapper;
        public InvitationController (IInvitationRepos invitationRepos, IMapper mapper,
                                        IUserRepo userRepo, IClassRepo classRepo,
                                        AuthorizeService authorizeService)
        {
            this._mapper = mapper;
            this._invitationRepos = invitationRepos;
            this._userRepo = userRepo;
            this._classRepo = classRepo;
            this._authorizeService = authorizeService;
        }


        ///<summary>Lấy hết danh sách lời mời tham gia lớp học cua hoc sinh</summary>
        ///<response code="200">thành công, có yêu cầu</response>
        ///<response code="204">thành công, không có yêu cầu</response>
        ///<response code="401">token không hợp lệ</response>
        [HttpGet("get-all-of-student")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<Response<IEnumerable<Invitation>>>> GetAllOfStudent([FromQuery] string searchQuery)
        {
            var username = User.Identity.Name;
            var user = await _userRepo.GetByUsername(username);
            if (user == null)
            {
                return BadRequest(new Response<string> { StatusCode = 401, Data = "", Messager = "User isn't exist." });
            }

            var invitation = await _invitationRepos.GetAllOfStudent(searchQuery, user.Id);
            if (invitation.Count() == 0)
            {
                return Ok(new Response<string> { StatusCode = 204, Data = "", Messager = "There is no request." });
            }

            var invitationReadModel = _mapper.Map<IEnumerable<InvitationReadDto>>(invitation);
            return Ok(new Response<IEnumerable<InvitationReadDto>> { StatusCode = 200, Data = invitationReadModel, Messager = "success" });
        }

        ///<summary>Lấy hết danh sách lời mời tham gia lớp học cua lop</summary>
        ///<response code="200">thành công, có yêu cầu</response>
        ///<response code="204">thành công, không có yêu cầu</response>
        ///<response code="401">token không hợp lệ</response>
        [HttpGet("get-all-of-class")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Student,Teacher")]
        public async Task<ActionResult<Response<IEnumerable<Invitation>>>> GetAllOfClass([FromQuery] string searchQuery, int classid)
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

            var invitation = await _invitationRepos.GetAllOfClass(searchQuery, classid);
            if (invitation.Count() == 0)
            {
                return Ok(new Response<string> { StatusCode = 204, Data = "", Messager = "There is no request." });
            }

            var invitationReadModel = _mapper.Map<IEnumerable<InvitationReadDto>>(invitation);
            return Ok(new Response<IEnumerable<InvitationReadDto>> { StatusCode = 200, Data = invitationReadModel, Messager = "success" });
        }

        ///<summary>Tạo lời mời</summary>
        ///<response code="201">thành công, tao thành công</response>
        ///<response code="400">thất bại, dữ liệu vào không hợp lệ</response>
        ///<response code="401">token không hợp lệ, không phải student</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Student,Teacher")]
        public async Task<ActionResult<Response<InvitationReadDto>>> CreateInvitation(int classid, int receiverid)
        {
            //Authorize
            var username = User.Identity.Name;
            var checkAuth = await _authorizeService.IsTeacher(username, classid);
            if (!checkAuth)
            {
                checkAuth = await _authorizeService.IsClassAdmin(username, classid);
                if (!checkAuth)
                {
                    return BadRequest(new Response<string> { StatusCode = 401, Data = "", Messager = "User can't accept." });
                }
            }

            //Get class, receiver
            var classToJoin = await _classRepo.GetById(classid);
            var reciever = await _userRepo.GetById(receiverid);

            //Check if user has joined the class or not
            if (classToJoin.students.Contains(reciever))
            {
                return BadRequest(new Response<string> { StatusCode = 400, Data = "", Messager = "Student has really joined to class " });
            }

            //Check if sender is admin or teacher owned class
            if(receiverid==classToJoin.TeacherId)
            {
                return BadRequest(new Response<string> { StatusCode = 400, Data = "", Messager = "Student is invalid " });
            }

            var invitationtModel = new Invitation();
            invitationtModel.Class = classToJoin;
            invitationtModel.Receiver = reciever;

            await _invitationRepos.CreateInvitation(invitationtModel);
            await _invitationRepos.SaveChange();

            var invitationReadDto = _mapper.Map<InvitationReadDto>(invitationtModel);
            return Created("", new Response<InvitationReadDto> { StatusCode = 201, Messager = "success", Data = invitationReadDto });
        }


        ///<summary>delete invitation </summary>
        ///<response code="202">thành công, xóa thành công</response>
        ///<response code="404">thất bại, không tìm thấy yêu cầu</response>
        ///<response code="401">token không hợp lệ, không phải admin</response>
        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Student, Teacher")]
        public async Task<ActionResult<Response<string>>> DeleteInvitation(int invitationid)
        {
            var invitationModel = await _invitationRepos.GetById(invitationid);
            if (invitationModel == null)
            {
                return BadRequest(new Response<string> { StatusCode = 400, Data = "", Messager = "Invitation isn't existed." });
            }

            await _invitationRepos.RemoveInvitation(invitationModel);
            await _invitationRepos.SaveChange();

            return Accepted(new Response<string> { StatusCode = 202, Messager = "success", Data = "" });
        }
   
        ///<summary>delete all invitations of students</summary>
        ///<response code="202">thành công, xóa thành công</response>
        ///<response code="404">thất bại, không tìm thấy yêu cầu</response>
        ///<response code="401">token không hợp lệ, không phải admin</response>
        [HttpDelete("delete-all")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Student, Teacher")]
        public async Task<ActionResult<Response<string>>> DeleteAllInvitation(IEnumerable<Invitation> invitations)
        {
            if (invitations == null)
            {
                return BadRequest(new Response<string> { StatusCode = 400, Data = "", Messager = "There is no invitation." });
            }

            await _invitationRepos.RemoveAllInvitation(invitations);
            await _invitationRepos.SaveChange();

            return Accepted(new Response<string> { StatusCode = 202, Messager = "success", Data = "" });
        }


        ///<summary>Lấy danh sách lời mời của học sinh theo page, limit</summary>
        ///<response code="200">thanh cong</response>
        ///<response code="204">thanh cong, khong co du lieu</response>
        [HttpGet("get-by-page-student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<Response<IEnumerable<Invitation>>>> GetByPageForStudent(int page, int limit)
        {
            var username = User.Identity.Name;
            var user = await _userRepo.GetByUsername(username);
            if(user==null)
            {
                return BadRequest(new Response<string> { StatusCode = 400, Data = "", Messager = "Request isn't existed." });
            }

            var invitations = await _invitationRepos.GetByPageForStudent(user.Id, page * limit, limit);
            if (invitations.Count() == 0)
            {
                return Ok(new Response<string> { StatusCode = 204, Data = "", Messager = "There is no request." });
            }

            var invitationReadDto = _mapper.Map<IEnumerable<InvitationReadDto>>(invitations);
            return Ok(new Response<IEnumerable<InvitationReadDto>> { StatusCode = 200, Messager = "success", Data = invitationReadDto });
        }

        ///<summary>Lấy danh sách lời mời của lớp học theo page, limit</summary>
        ///<response code="200">thanh cong</response>
        ///<response code="204">thanh cong, khong co du lieu</response>
        [HttpGet("get-by-page-class")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = "Student, Teacher")]
        public async Task<ActionResult<Response<IEnumerable<Invitation>>>> GetByPageForClass(string searchQuery, int classid, int page, int limit)
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

            var invitations = await _invitationRepos.GetByPageForClass(searchQuery,classid, page * limit, limit);
            if (invitations.Count() == 0)
            {
                return Ok(new Response<string> { StatusCode = 204, Data = "", Messager = "There is no request." });
            }

            var invitationReadDto = _mapper.Map<IEnumerable<InvitationReadDto>>(invitations);
            return Ok(new Response<IEnumerable<InvitationReadDto>> { StatusCode = 200, Messager = "success", Data = invitationReadDto });
        }
    }
}
