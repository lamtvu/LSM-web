using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using LmsBeApp_Group06.Data;
using LmsBeApp_Group06.Data.Repositories.InformationRepo;
using LmsBeApp_Group06.Data.Repositories.UserRepos;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Helpers;
using LmsBeApp_Group06.Models;
using LmsBeApp_Group06.Options;
using LmsBeApp_Group06.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace LmsBeApp_Group06.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepo _userRepos;
        private readonly IInformationRepo _inforRepos;
        private readonly IMapper _mapper;
        private readonly JwtService _jwtService;
        private readonly MailService _mailService;

        private readonly string _TokenSecretKey;
        private readonly int _TokenLifetime;
        private readonly int _verificationTokenLifetime;
        private readonly string _verificationSecrecKey;

        public AuthController(IUserRepo userRepos, IMapper mapper,
                              JwtService jwtService, MailService mailService,
                              IOptions<JwtOption> jwtOption, IInformationRepo inforRepos)
        {
            this._mailService = mailService;
            this._jwtService = jwtService;
            this._mapper = mapper;
            this._userRepos = userRepos;
            this._inforRepos = inforRepos;
            var jwtOptionValue = jwtOption.Value;
            this._TokenLifetime = jwtOptionValue.TokenLifetime;
            this._TokenSecretKey = jwtOptionValue.TokenSecretKey;
            this._verificationTokenLifetime = jwtOptionValue.VerificationLifetime;
            this._verificationSecrecKey = jwtOptionValue.VerifictionSecretKey;
        }

        ///<summary>đăng nhập</summary>
        ///<response code="200">thành công</response>
        ///<response code="400">thất bại, sai mk hoặc tk</response>
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<Response<TokenDtos>>> Login(UserLoginDto userLogin)
        {
            var user = await _userRepos.GetByUsername(userLogin.Username);
            if (user == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "invalid username" });
            }

            if (user.Password != userLogin.Password)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "invalid password" });
            }
            if (user.IsLock == true)
            {
                return BadRequest(new Response<string> { StatusCode = 400, Messager = "Username is loocked", Data = "" });
            }
            var claims = new[]{
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.RoleName),
                new Claim("Verify", user.Verify.ToString()),
                new Claim("UserId", user.Id.ToString())
            };
            var token = _jwtService.GenerateToken(claims, _TokenSecretKey, _TokenLifetime);
            var tokenDtos = new TokenDtos { Token = token, Role = user.Role.RoleName };
            return Ok(new Response<TokenDtos> { StatusCode = 200, Messager = "success", Data = tokenDtos });
        }

        ///<summary>lấy mã xác nhập qua email</summary>
        ///<response code="200">thành công</response>
        ///<response code="400">thất bại, không tìm thấy email, email đã xác nhập</response>
        [HttpPost]
        [Route("getVerify", Name = nameof(GetVerifyGmail))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Response<string>>> GetVerifyGmail(string email)
        {
            var userModel = await _userRepos.GetByEmail(email);
            if (userModel == null)
            {
                return BadRequest(new Response<string> { Data = "err", StatusCode = 400, Messager = "email not found" });
            }
            if (userModel.Verify == true)
            {
                return BadRequest(new Response<string> { Data = "err", StatusCode = 400, Messager = "email already verify" });
            }
            var claims = new[]{
                new Claim(ClaimTypes.Name, userModel.Username),
                new Claim(ClaimTypes.Email, userModel.Email)
            };
            var token = _jwtService.GenerateToken(claims, _verificationSecrecKey, _verificationTokenLifetime);
            var mailContent = new MailContent
            {
                To = userModel.Email,
                Subject = "Verify email",
                Body = MailHtmlForm.VetifyForm(userModel.Username, "techedu.com", " https://lmstechbe.azurewebsites.net" + "/api/auth/verify?token=" + token, "techedu.ltv@gmail.com")
            };
            await _mailService.SendMail(mailContent);
            return Ok(new Response<string> { StatusCode = 200, Data = "", Messager = "success" });
        }

        ///<summary>xác nhập gmail</summary>
        ///<response code="200">thành công</response>
        ///<response code="400">thất bại, không tìm thấy email, token không hợp lệ, không chứa token</response>
        [HttpGet]
        [Route("verify")]
        public async Task<ActionResult> VerifyEmail(string token)
        {
            if (token == null || token.Length == 0)
            {
                return BadRequest(new Response<string> { Data = "err", StatusCode = 400, Messager = "requied token" });
            }
            var Claims = _jwtService.GetClaimsPrincipal(token, _verificationSecrecKey);
            var email = Claims?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value.ToString();
            if (email == null)
            {
                return BadRequest(new Response<string> { StatusCode = 400, Messager = "invalid token", Data = "" });
            }
            var userModel = await _userRepos.GetByEmail(email);
            if (userModel == null)
            {
                return BadRequest(new Response<string> { StatusCode = 400, Messager = "gmail not found", Data = "" });
            }
            userModel.Verify = true;
            await _userRepos.SaveChange();
            return Redirect("https://lmsfe.azurewebsites.net/login");
        }

        ///<summary>đăng ký</summary>
        ///<response code="200">thành công</response>
        ///<response code="400">thất bại, usernamse, email đã tồn tại, dữ liệu đầu vào không hợp lệ</response>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser account)
        {
            var userEmail = await _userRepos.GetByEmail(account.Email);
            if (userEmail != null)
            {
                return BadRequest(new Response<string> { StatusCode = 400, Messager = "Email is existed", Data = "" });
            }
            var userName = await _userRepos.GetByUsername(account.Username);
            if (userName != null)
            {
                return BadRequest(new Response<string> { StatusCode = 400, Messager = "Username is existed", Data = "" });
            }


            //Mapper user
            var userModel = _mapper.Map<User>(account);
            userModel.IsLock = false;
            userModel.Verify = false;
            userModel.RoleId = 4;
            userModel.CreateDate = DateTime.UtcNow;

            await _userRepos.CreateUser(userModel);
            await _userRepos.SaveChange();
            return Ok(new Response<User> { StatusCode = 200, Messager = "success", Data = null });
        }
    }
}