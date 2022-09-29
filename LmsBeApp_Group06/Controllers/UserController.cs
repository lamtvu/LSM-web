using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LmsBeApp_Group06.Data.Repositories.UserRepos;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Filters;
using LmsBeApp_Group06.Models;
using LmsBeApp_Group06.Services;
using LmsBeApp_Group06.Services.BlobService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace LmsBeApp_Group06.Controllers
{

    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly IBlobService _blobService;

        public UserController(IUserRepo userRepo, IMapper mapper, IBlobService blobService)
        {
            this._blobService = blobService;
            this._mapper = mapper;
            this._userRepo = userRepo;

        }

        ///<summary>Thay đổi role người dùng</summary>
        ///<response code="202">thay đổi thành công</response>
        ///<response code="400">role id không hợp lệ, id user không tồn tại</response>
        [HttpPut("change-role/{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<Response<User>>> ChangeRole(int id, [FromQuery] int RoleId)
        {
            var userModel = await _userRepo.GetById(id);
            if (userModel == null)
            {
                return Unauthorized(new Response<string> { StatusCode = 401, Data = "", Messager = "User isn't exist." });
            }

            userModel.RoleId = RoleId;
            await _userRepo.Change(userModel);
            await _userRepo.SaveChange();

            return Ok(new Response<User> { StatusCode = 202, Data = null, Messager = "success" });
        }

        ///<summary>Thay đổi isLook người dùng</summary>
        ///<response code="202">thay đổi thành công</response>
        ///<response code="400">id user không hợp lệ</response>
        [HttpPut("change-lock/{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response<string>>> ChangeLock(int id)
        {
            var userModel = await _userRepo.GetById(id);
            if (userModel == null)
            {
                return Unauthorized(new Response<string> { StatusCode = 401, Data = null, Messager = "User isn't exist." });
            }

            if (userModel.IsLock == true)
            {
                userModel.IsLock = false;
                await _userRepo.Change(userModel);
                await _userRepo.SaveChange();
                return Ok(new Response<string> { StatusCode = 202, Data = null, Messager = "User is unlocked." });
            }

            userModel.IsLock = true;
            await _userRepo.Change(userModel);
            await _userRepo.SaveChange();
            return Ok(new Response<string> { StatusCode = 202, Data = "", Messager = "User is locked." });
        }

        ///<summary>Lấy hết danh sách người dùng</summary>
        ///<response code="200">thành công</response>
        ///<response code="204">thành công, nhưng ko có dữ liệu</response>
        [HttpGet("get-all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response<PageDataDto<UserReadDto>>>> GetAll([FromQuery] string searchValue)
        {
            var users = await _userRepo.GetAll(searchValue);
            var userReadData = new PageDataDto<UserReadDto>
            {
                Count = users.Count,
                Data = _mapper.Map<IEnumerable<User>, IEnumerable<UserReadDto>>(users.Data)
            };
            return Ok(new Response<PageDataDto<UserReadDto>> { StatusCode = 200, Data = userReadData, Messager = "success" });
        }

        ///<summary>Thay đổi infor người dùng theo token</summary>
        ///<response code="202">thay đổi thành công</response>
        ///<response code="400">input không hợp lệ, user không tồn tại</response>
        [HttpPut("change-infor")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<UserReadDto>>> ChangeInfor(ChangeUserInforDto changeUserInfor)
        {
            var username = User.Identity.Name;
            var userModel = await _userRepo.GetByUsername(username);
            if (userModel == null)
            {
                return Unauthorized(new Response<string> { StatusCode = 401, Data = null, Messager = "User isn't exist." });
            }
            _mapper.Map(changeUserInfor, userModel);
            await _userRepo.SaveChange();
            var userRead = _mapper.Map<UserReadDto>(userModel);
            return Ok(new Response<UserReadDto> { StatusCode = 200, Data = userRead, Messager = "success" });
        }

        [HttpPut("change-password")]
        [Authorize]
        public async Task<ActionResult<Response<object>>> ChangePassword(UserChangePasswordDto userChangePassword)
        {
            var username = User.Identity.Name;
            var user = await _userRepo.GetByUsername(username);
            if (user == null)
            {
                return Unauthorized(new Response<string> { StatusCode = 401, Data = null, Messager = "User isn't exist." });
            }
            if (user.Password != userChangePassword.Oldpass)
            {
                return BadRequest(new Response<string> { StatusCode = 400, Data = null, Messager = "invalid old password." });
            }
            if (user.Password == userChangePassword.Newpass)
            {
                return BadRequest(new Response<string> { StatusCode = 400, Data = null, Messager = "The new password must be different from the old password." });
            }
            if (userChangePassword.Confirmpass != userChangePassword.Newpass)
            {
                return BadRequest(new Response<string> { StatusCode = 400, Data = null, Messager = "The new password and Confirm password are invalid." });
            }
            user.Password = userChangePassword.Newpass;
            await _userRepo.SaveChange();
            return Ok(new Response<object> { Data = null, StatusCode = 200, Messager = "success" });
        }

        ///<summary>lấy thông tin người dùng theo id</summary>
        ///<response code="200">thành công</response>
        ///<response code="400">user không tồn tại</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<UserReadDto>>> GetDetail(int id)
        {
            var userModel = await _userRepo.GetDetail(id);
            if (userModel == null)
            {
                return Unauthorized(new Response<string> { StatusCode = 401, Data = null, Messager = "User isn't exist." });
            }
            var userRead = _mapper.Map<UserReadDto>(userModel);

            return Ok(new Response<UserReadDto> { StatusCode = 200, Data = userRead, Messager = "success" });
        }

        ///<summary>lấy thông tin người dùng theo token</summary>
        ///<response code="200">thành công</response>
        ///<response code="400">user không tồn tại</response>
        [HttpGet("detail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<Response<UserReadDto>>> GetDetailByToken()
        {
            var username = User.Identity.Name;

            var userModel = await _userRepo.GetDetail(username);
            if (userModel == null)
            {
                return Unauthorized(new Response<string> { StatusCode = 401, Data = null, Messager = "User isn't exist." });
            }
            var userRead = _mapper.Map<UserReadDto>(userModel);
            return Ok(new Response<UserReadDto> { StatusCode = 200, Data = userRead, Messager = "success" });
        }


        ///<summary>Lấy danh sách theo page</summary>
        ///<response code="200">thành công</response>
        ///<response code="204">thành công, nhưng ko có dữ liệu</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<Response<PageDataDto<UserReadDto>>>> GetByPage([FromQuery] int page, [FromQuery] int limit, string searchValue)
        {
            var users = await _userRepo.GetByPage(page * limit, limit, searchValue);
            // nocontent
            var userReads = _mapper.Map<IEnumerable<User>, IEnumerable<UserReadDto>>(users.Data);
            var pageData = new PageDataDto<UserReadDto>
            {
                Data = userReads,
                Count = users.Count
            };
            return Ok(new Response<PageDataDto<UserReadDto>> { StatusCode = 200, Data = pageData, Messager = "success" });
        }

        [HttpPut("change-avatar")]
        [Authorize]
        public async Task<ActionResult<Response<object>>> ChangeAvatar()
        {
            var username = User.Identity.Name;
            var user = await _userRepo.GetByUsername(username);
            if (user == null)
            {
                return Unauthorized(new Response<object> { Data = null, StatusCode = 401, Messager = "invalid Token" });
            }
            if (Request.Form.Files.Count == 0)
            {
                return Ok(new Response<object> { Data = null, StatusCode = 400, Messager = "requied file" });
            }
            var file = Request.Form.Files[0];
            var fileType = file.FileName;
            if (!String.IsNullOrWhiteSpace(user.Image))
            {
                await _blobService.DeleteBlobAsync("user" + user.Id + user.Image, "usercontainer");
                user.Image = "";
            }
            user.Image = fileType;
            await _userRepo.SaveChange();
            await _blobService.UploadFileBlobAsync(file, "user" + user.Id + user.Image, "usercontainer");
            return Ok(new Response<object> { Data = null, StatusCode = 200, Messager = "success" });
        }

        [HttpGet("avatar/{userId}")]
        public async Task<ActionResult> GetAvatar(int userId)
        {
            var user = await _userRepo.GetById(userId);
            if (user == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "userid does not exist" });
            }
            if (String.IsNullOrWhiteSpace(user.Image))
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "has not avatar" });
            }
            var fileName = "user" + user.Id + user.Image;
            var stream = await _blobService.GetBlobStreamAsync(fileName, "usercontainer");
            string contentType;
            new FileExtensionContentTypeProvider().TryGetContentType(user.Image, out contentType);
            return new FileStreamResult(stream.Content, contentType);
        }
    }
}

