using AutoMapper;
using LmsBeApp_Group06.Data.Repositories.AnnouncementRepos;
using LmsBeApp_Group06.Data.Repositories.ClassRepo;
using LmsBeApp_Group06.Data.Repositories.UserRepos;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Dtos.Announcement;
using LmsBeApp_Group06.Models;
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
    [Route("api/announcement")]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementRepos _announcementRepos;
        private readonly IUserRepo _userRepo;
        private readonly IClassRepo _classRepo;
        private readonly IMapper _mapper;
        public AnnouncementController(IAnnouncementRepos announcementRepos, IMapper mapper,
                                        IUserRepo userRepo, IClassRepo classRepo)
        {
            this._mapper = mapper;
            this._announcementRepos = announcementRepos;
            this._userRepo = userRepo;
            this._classRepo = classRepo;
        }


        ///<summary>Lấy hết danh sách thong bao cua hoc sinh</summary>
        ///<response code="200">thành công, có yêu cầu</response>
        ///<response code="204">thành công, không có yêu cầu</response>
        ///<response code="401">token không hợp lệ</response>
        [HttpGet("get-all-notify")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<ActionResult<Response<IEnumerable<AnnouncementReadDto>>>> GetAllNotifyOfStudent()
        {
            var username = User.Identity.Name;
            var user = await _userRepo.GetByUsername(username);
            if (user == null)
            {
                return BadRequest(new Response<string> { StatusCode = 401, Data = "", Messager = "User isn't exist." });
            }

            var announcements = await _announcementRepos.GetAllByNotifyOfStudent(user);
            if (announcements.Count() == 0)
            {
                return Ok(new Response<string> { StatusCode = 204, Data = "", Messager = "There is no announcement." });
            }

            var announcementReadModel = _mapper.Map<IEnumerable<AnnouncementReadDto>>(announcements);
            return Ok(new Response<IEnumerable<AnnouncementReadDto>> { StatusCode = 200, Data = announcementReadModel, Messager = "success" });
        }


        ///<summary>Lấy hết thong bao cua lop</summary>
        ///<response code="200">thành công, có yêu cầu</response>
        ///<response code="204">thành công, không có yêu cầu</response>
        ///<response code="401">token không hợp lệ</response>
        [HttpGet("get-all-notify/{classId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Student,Teacher")]
        public async Task<ActionResult<Response<IEnumerable<AnnouncementReadDto>>>> GetAllNotifyOfClass(int classId)
        {
            var announcements = await _announcementRepos.GetAllByNotifyOfClass(classId);
            if (announcements.Count() == 0)
            {
                return Ok(new Response<string> { StatusCode = 204, Data = "", Messager = "There is no announcement." });
            }

            var announcementReadModel = _mapper.Map<IEnumerable<AnnouncementReadDto>>(announcements);
            return Ok(new Response<IEnumerable<AnnouncementReadDto>> { StatusCode = 200, Data = announcementReadModel, Messager = "success" });
        }


        ///<summary>Lấy hết thong bao cua lop</summary>
        ///<response code="200">thành công, có yêu cầu</response>
        ///<response code="204">thành công, không có yêu cầu</response>
        ///<response code="401">token không hợp lệ</response>
        [HttpGet("get-all-program/{classId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Student,Teacher")]
        public async Task<ActionResult<Response<IEnumerable<AnnouncementReadDto>>>> GetAllByProgram(int classId)
        {
            var announcements = await _announcementRepos.GetAllByProgram(classId);
            if (announcements.Count() == 0)
            {
                return Ok(new Response<string> { StatusCode = 204, Data = "", Messager = "There is no announcement." });
            }

            var announcementReadModel = _mapper.Map<IEnumerable<AnnouncementReadDto>>(announcements);
            return Ok(new Response<IEnumerable<AnnouncementReadDto>> { StatusCode = 200, Data = announcementReadModel, Messager = "success" });
        }

        ///<summary>Tạo chương trình</summary>
        ///<response code="201">thành công, tao thành công</response>
        ///<response code="400">thất bại, dữ liệu vào không hợp lệ</response>
        ///<response code="401">token không hợp lệ, không phải student</response>
        [HttpPost("program/{classId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
         [Authorize]
        public async Task<ActionResult<Response<AnnouncementReadDto>>> CreateProgram(int classId, AnnouncementCreateDto announcementCreate)
        {
            //Get class
            var _class = await _classRepo.GetById(classId);

            var announcementModel = _mapper.Map<Announcement>(announcementCreate);
            announcementModel.CreateDate = DateTime.UtcNow;
            announcementModel.Class = _class;
            announcementModel.Type = "Program";
            await _announcementRepos.CreateAnnouncement(announcementModel);
            await _announcementRepos.SaveChange();

            var announcementReadDto = _mapper.Map<AnnouncementReadDto>(announcementModel);
            return Created("", new Response<AnnouncementReadDto> { StatusCode = 201, Messager = "success", Data = announcementReadDto });
        }

        ///<summary>Tạo chương trình</summary>
        ///<response code="201">thành công, tao thành công</response>
        ///<response code="400">thất bại, dữ liệu vào không hợp lệ</response>
        ///<response code="401">token không hợp lệ, không phải student</response>
        [HttpPost("notify/{classId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult<Response<AnnouncementReadDto>>> CreateNotify(int classId, AnnouncementCreateDto announcementCreate)
        {
            //Get class
            var _class = await _classRepo.GetById(classId);

            var announcementModel = _mapper.Map<Announcement>(announcementCreate);
            announcementModel.CreateDate = DateTime.UtcNow;
            announcementModel.Class = _class;
            announcementModel.Type = "Notify";
            await _announcementRepos.CreateAnnouncement(announcementModel);
            await _announcementRepos.SaveChange();

            var announcementReadDto = _mapper.Map<AnnouncementReadDto>(announcementModel);
            return Created("", new Response<AnnouncementReadDto> { StatusCode = 201, Messager = "success", Data = announcementReadDto });
        }

        ///<summary>Detail thong bao</summary>
        ///<response code="201">thành công, tao thành công</response>
        ///<response code="400">thất bại, dữ liệu vào không hợp lệ</response>
        ///<response code="401">token không hợp lệ, không phải student</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<ActionResult<Response<AnnouncementReadDto>>> DetailAnnouncement(int announcementid)
        {
            var announcement = await _announcementRepos.GetById(announcementid);
            var announcementReadDto = _mapper.Map<AnnouncementReadDto>(announcement);
            return Created("", new Response<AnnouncementReadDto> { StatusCode = 201, Messager = "success", Data = announcementReadDto });
        }

        ///<summary>Edit thong bao</summary>
        ///<response code="201">thành công, tao thành công</response>
        ///<response code="400">thất bại, dữ liệu vào không hợp lệ</response>
        ///<response code="401">token không hợp lệ, không phải student</response>
        [HttpPut("{announcementId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult<Response<AnnouncementReadDto>>> EditAnnouncement(int announcementId, AnnouncementEditDto model)
        {
            if (model == null)
            {
                return BadRequest(new Response<string> { StatusCode = 400, Data = "", Messager = "Announcement is invalid." });
            }
            var announcement = await _announcementRepos.GetById(announcementId);
            _mapper.Map(model, announcement);
            await _announcementRepos.UpdateAnnouncement(announcement);
            await _announcementRepos.SaveChange();
            var announcementReadDto = _mapper.Map<AnnouncementReadDto>(announcement);
            return Created("", new Response<AnnouncementReadDto> { StatusCode = 201, Messager = "success", Data = announcementReadDto });
        }


        ///<summary>delete announcement </summary>
        ///<response code="202">thành công, xóa thành công</response>
        ///<response code="404">thất bại, không tìm thấy yêu cầu</response>
        ///<response code="401">token không hợp lệ, không phải admin</response>
        [HttpDelete("{announcementId}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult<Response<string>>> DeleteAnnouncement(int announcementId)
        {
            var announcementModel = await _announcementRepos.GetById(announcementId);
            if (announcementModel == null)
            {
                return BadRequest(new Response<string> { StatusCode = 400, Data = "", Messager = "Announcement isn't existed." });
            }
            await _announcementRepos.RemoveAnnouncement(announcementModel);
            await _announcementRepos.SaveChange();
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
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult<Response<string>>> DeleteAllAnnouncement(IEnumerable<Announcement> announcements)
        {
            if (announcements == null)
            {
                return BadRequest(new Response<string> { StatusCode = 400, Data = "", Messager = "There is no announcement." });
            }

            await _announcementRepos.RemoveAllAnnouncement(announcements);
            await _announcementRepos.SaveChange();

            return Accepted(new Response<string> { StatusCode = 202, Messager = "success", Data = "" });
        }


        ///<summary>Lấy danh sách thong bao của học sinh theo page, limit</summary>
        ///<response code="200">thanh cong</response>
        ///<response code="204">thanh cong, khong co du lieu</response>
        [HttpGet("get-by-page-notify-student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize]
        public async Task<ActionResult<Response<IEnumerable<object>>>> GetByPageNotifyOfStudent(int page, int limit)
        {
            var username = User.Identity.Name;
            var user = await _userRepo.GetByUsername(username);
            if (user == null)
            {
                return BadRequest(new Response<string> { StatusCode = 401, Data = "", Messager = "User isn't exist." });
            }

            var announcements = await _announcementRepos.GetByPageNotifyOfStudent(user, page * limit, limit);
            if (announcements.Count() == 0)
            {
                return Ok(new Response<string> { StatusCode = 204, Data = "", Messager = "There is no announcement." });
            }

            var announcementReadModel = _mapper.Map<IEnumerable<AnnouncementReadDto>>(announcements);
            return Ok(new Response<IEnumerable<AnnouncementReadDto>> { StatusCode = 200, Data = announcementReadModel, Messager = "success" });
        }

        ///<summary>Lấy danh sách thong bao của lớp học theo page, limit</summary>
        ///<response code="200">thanh cong</response>
        ///<response code="204">thanh cong, khong co du lieu</response>
        [HttpGet("get-by-page-notify-class")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = "Student,Teacher")]
        public async Task<ActionResult<Response<IEnumerable<AnnouncementReadDto>>>> GetByPageNotifyOfClass(int classid, int page, int limit)
        {
            var username = User.Identity.Name;
            var user = await _userRepo.GetByUsername(username);
            if (user == null)
            {
                return BadRequest(new Response<string> { StatusCode = 401, Data = "", Messager = "User isn't exist." });
            }

            var announcements = await _announcementRepos.GetByPageNotifyOfClass(classid, page * limit, limit);
            if (announcements.Count() == 0)
            {
                return Ok(new Response<string> { StatusCode = 204, Data = "", Messager = "There is no announcement." });
            }

            var announcementReadModel = _mapper.Map<IEnumerable<AnnouncementReadDto>>(announcements);
            return Ok(new Response<IEnumerable<AnnouncementReadDto>> { StatusCode = 200, Data = announcementReadModel, Messager = "success" });
        }

        ///<summary>Lấy danh sách program của lớp học theo page, limit</summary>
        ///<response code="200">thanh cong</response>
        ///<response code="204">thanh cong, khong co du lieu</response>
        [HttpGet("get-by-page-program/{classId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = "Student,Teacher,")]
        public async Task<ActionResult<Response<IEnumerable<Invitation>>>> GetByPageProgram(int classId, int page, int limit)
        {
            var username = User.Identity.Name;
            var user = await _userRepo.GetByUsername(username);
            if (user == null)
            {
                return BadRequest(new Response<string> { StatusCode = 401, Data = "", Messager = "User isn't exist." });
            }

            var announcements = await _announcementRepos.GetByPageProgram(classId, page * limit, limit);
            if (announcements.Count() == 0)
            {
                return Ok(new Response<string> { StatusCode = 204, Data = "", Messager = "There is no announcement." });
            }

            var announcementReadModel = _mapper.Map<IEnumerable<AnnouncementReadDto>>(announcements);
            return Ok(new Response<IEnumerable<AnnouncementReadDto>> { StatusCode = 200, Data = announcementReadModel, Messager = "success" });
        }
    }
}