using AutoMapper;
using LmsBeApp_Group06.Data.Repositories.ClassRepo;
using LmsBeApp_Group06.Data.Repositories.ReportsRepo;
using LmsBeApp_Group06.Data.Repositories.UserRepos;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Dtos.Report;
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
    [Route("api/report")]
    public class ReportController : ControllerBase
    {
        private readonly IReportsRepo _reportsRepos;
        private readonly IUserRepo _userRepo;
        private readonly IClassRepo _classRepo;
        private readonly AuthorizeService _authorizeService;
        private readonly IMapper _mapper;
        public ReportController (IReportsRepo reportsRepos, IMapper mapper,
                                        IUserRepo userRepo, IClassRepo classRepo,
                                          AuthorizeService authorizeService)
        {
            this._mapper = mapper;
            this._reportsRepos = reportsRepos;
            this._userRepo = userRepo;
            this._classRepo = classRepo;
            this._authorizeService = authorizeService;
        }

        ///<summary>Lấy danh sách reports </summary>
        ///<response code="200">thành công, có yêu cầu</response>
        ///<response code="204">thành công, không có yêu cầu</response>
        ///<response code="401">token không hợp lệ</response>
        [HttpGet("get-all/{classid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Student, Teacher")]
        public async Task<ActionResult<Response<IEnumerable<ReportReadDto>>>> GetAll(int classid)
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

            //Get all reports
            var reports = await _reportsRepos.GetAll(classid);
            if (reports.Count() == 0)
            {
                return Ok(new Response<string> { StatusCode = 204, Data = "", Messager = "There is no report." });
            }

            var reportsReadModel = _mapper.Map<IEnumerable<ReportReadDto>>(reports);
            return Ok(new Response<IEnumerable<ReportReadDto>> { StatusCode = 200, Data = reportsReadModel, Messager = "success" });
        }
   

        ///<summary>Tạo report</summary>
        ///<response code="201">thành công, tao thành công</response>
        ///<response code="400">thất bại, dữ liệu vào không hợp lệ</response>
        ///<response code="401">token không hợp lệ, không phải student</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<Response<ReportCreateDto>>> CreateReport(int classid,  ReportCreateDto reportCreateDto)
        {
            var username = User.Identity.Name;
            var user = _userRepo.GetByUsername(username);
            if (user == null)
            {
                return BadRequest(new Response<string> { StatusCode = 401, Data = "", Messager = "User isn't existed." });
            }

            if(reportCreateDto==null)
            {
                return BadRequest(new Response<string> { StatusCode = 401, Data = "", Messager = "Report is invalid." });
            }
            var reportModel = _mapper.Map<Report>(reportCreateDto);
            reportModel.ClassId = classid;
            reportModel.SenderId = user.Id;
         
            await _reportsRepos.SaveChange();

            var reportReadDto = _mapper.Map<ReportReadDto>(reportModel);
            return Created("", new Response<ReportReadDto> { StatusCode = 201, Messager = "success", Data = reportReadDto });
        }


        ///<summary>delete report</summary>
        ///<response code="202">thành công, xóa thành công</response>
        ///<response code="404">thất bại, không tìm thấy yêu cầu</response>
        ///<response code="401">token không hợp lệ, không phải admin</response>
        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Student, Teacher")]
        public async Task<ActionResult<Response<string>>> DeleteReport(int reportid)
        {
            var reportModel = await _reportsRepos.GetReport(reportid);
            if (reportModel == null)
            {
                return Ok(new Response<string> { StatusCode = 400, Data = "", Messager = "Report isn't existed." });
            }

            await _reportsRepos.Delete(reportModel);
            await _reportsRepos.SaveChange();

            return Accepted(new Response<string> { StatusCode = 202, Messager = "success", Data = "" });
        }

        ///<summary>delete all reports</summary>
        ///<response code="202">thành công, xóa thành công</response>
        ///<response code="404">thất bại, không tìm thấy yêu cầu</response>
        ///<response code="401">token không hợp lệ, không phải admin</response> 
        [HttpDelete("delete-all")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Teacher, Student")]
        public async Task<ActionResult<Response<string>>> DeleteAll(int classid)
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

            //get list reports
            var reports = await _reportsRepos.GetAll(classid);
            if(reports.Count() == 0)
            {
                return Ok(new Response<string> { StatusCode = 400, Data = "", Messager = "There is no report." });
            }

            //delete all reports
            await _reportsRepos.DeleteAll(reports);
            await _reportsRepos.SaveChange();

            return Accepted(new Response<string> { StatusCode = 202, Messager = "success", Data = "" });
        }


        ///<summary>get detail of report</summary>
        ///<response code="202">thành công, xóa thành công</response>
        ///<response code="404">thất bại, không tìm thấy yêu cầu</response>
        ///<response code="401">token không hợp lệ, không phải admin</response> 
        [HttpGet("{reportid}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Teacher, Student")]
        public async Task<ActionResult<Response<ReportReadDto>>> Detail(int reportid)
        {
            //get report
            var report = await _reportsRepos.GetReport(reportid);
            if (report == null)
            {
                return BadRequest(new Response<string> { StatusCode = 400, Data = "", Messager = "There is no report." });
            }

            var reportsReadModel = _mapper.Map<ReportReadDto>(report);
            return Ok(new Response<ReportReadDto> { StatusCode = 200, Data = reportsReadModel, Messager = "success" });
        }


        ///<summary>Lấy danh sách report theo page, limit</summary>
        ///<response code="200">thanh cong</response>
        ///<response code="204">thanh cong, khong co du lieu</response>
        [HttpGet("get-by-page")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = "Student, Teacher")]
        public async Task<ActionResult<Response<IEnumerable<ReportReadDto>>>> GetByPage(string searchQuery,int classid, int page, int limit)
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

            var reports = await _reportsRepos.GetByPage(searchQuery, classid, page * limit, limit);
            if (reports.Count() == 0)
            {
                return Ok(new Response<string> { StatusCode = 204, Data = "", Messager = "There is no report." });
            }

            var reportsReadModel = _mapper.Map<IEnumerable<ReportReadDto>>(reports);
            return Ok(new Response<IEnumerable<ReportReadDto>> { StatusCode = 200, Data = reportsReadModel, Messager = "success" });
        }

               
    }
}
