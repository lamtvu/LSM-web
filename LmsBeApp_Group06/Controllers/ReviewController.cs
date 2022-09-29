using AutoMapper;
using LmsBeApp_Group06.Data.Repositories.ClassRepo;
using LmsBeApp_Group06.Data.Repositories.ReviewRepos;
using LmsBeApp_Group06.Data.Repositories.UserRepos;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Dtos.Reivew;
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
    [Route("api/review")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepo _reviewRepos;
        private readonly IUserRepo _userRepo;
        private readonly IClassRepo _classRepo;
        private readonly AuthorizeService _authorizeService;
        private readonly IMapper _mapper;
        public ReviewController(IReviewRepo reviewRepos, IMapper mapper,
                                        IUserRepo userRepo, IClassRepo classRepo,
                                          AuthorizeService authorizeService)
        {
            this._mapper = mapper;
            this._reviewRepos = reviewRepos;
            this._userRepo = userRepo;
            this._classRepo = classRepo;
            this._authorizeService = authorizeService;
        }

        ///<summary>Lấy danh sách reviews </summary>
        ///<response code="200">thành công, có yêu cầu</response>
        ///<response code="204">thành công, không có yêu cầu</response>
        ///<response code="401">token không hợp lệ</response>
        [HttpGet("get-all")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<ActionResult<Response<IEnumerable<ReviewReadDto>>>> GetAll(int courseid)
        {
            //Get all reviews of course
            var reviews = await _reviewRepos.GetAll(courseid);
            if (reviews.Count() == 0)
            {
                return Ok(new Response<string> { StatusCode = 204, Data = "", Messager = "There is no review." });
            }

            var reviewsReadModel = _mapper.Map<IEnumerable<ReviewReadDto>>(reviews);
            return Ok(new Response<IEnumerable<ReviewReadDto>> { StatusCode = 200, Data = reviewsReadModel, Messager = "success" });
        }

        ///<summary>Check if use reviewed or not </summary>
        ///<response code="200">thành công, có yêu cầu</response>
        ///<response code="204">thành công, không có yêu cầu</response>
        ///<response code="401">token không hợp lệ</response>
        [HttpGet("check-user")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<Response<IEnumerable<ReviewReadDto>>>> CheckReviewed(int courseid)
        {
            var username = User.Identity.Name;
            var user = _userRepo.GetByUsername(username);
            if (user == null)
            {
                return BadRequest(new Response<string> { StatusCode = 401, Data = "", Messager = "User isn't existed." });
            }

            //Get review
            var review = await _reviewRepos.GetReview(user.Id, courseid);
            if (review == null)
            {
                return Ok(new Response<string> { StatusCode = 200, Data = "", Messager = "There is no review." });
            }

            var reviewsReadModel = _mapper.Map<IEnumerable<ReviewReadDto>>(review);
            return Ok(new Response<IEnumerable<ReviewReadDto>> { StatusCode = 200, Data = reviewsReadModel, Messager = "success" });
        }


        ///<summary>Tạo review</summary>
        ///<response code="201">thành công, tao thành công</response>
        ///<response code="400">thất bại, dữ liệu vào không hợp lệ</response>
        ///<response code="401">token không hợp lệ, không phải student</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<Response<ReviewCreateDto>>> CreateReview(int courseid, ReviewCreateDto reviewCreateDto)
        {
            var username = User.Identity.Name;
            var user = await _userRepo.GetByUsername(username);
            if (user == null)
            {
                return BadRequest(new Response<string> { StatusCode = 401, Data = "", Messager = "User isn't existed." });
            }

            if (reviewCreateDto == null)
            {
                return BadRequest(new Response<string> { StatusCode = 401, Data = "", Messager = "Report is invalid." });
            }
            var reviewModel = _mapper.Map<Review>(reviewCreateDto);
            reviewModel.CourseId = courseid;
            reviewModel.SenderId = user.Id;
            reviewModel.CreateDate = DateTime.UtcNow;

            await _reviewRepos.Create(reviewModel);
            await _reviewRepos.SaveChange();

            var reviewReadDto = _mapper.Map<ReviewReadDto>(reviewModel);
            return Created("", new Response<ReviewReadDto> { StatusCode = 201, Messager = "success", Data = reviewReadDto });
        }


        ///<summary>Edit review</summary>
        ///<response code="202">thành công, xóa thành công</response>
        ///<response code="404">thất bại, không tìm thấy yêu cầu</response>
        ///<response code="401">token không hợp lệ, không phải admin</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Student, Teacher")]
        public async Task<ActionResult<Response<ReviewReadDto>>> EditReview(ReviewEditDto reviewEdit)
        {
            var reviewModel = await _reviewRepos.GetReviewById(reviewEdit.Id);

            if (reviewEdit == null || reviewModel.Star <= 0)
            {
                return BadRequest(new Response<string> { StatusCode = 400, Data = "", Messager = "Review is invalid." });
            }

            if (reviewModel == null)
            {
                return BadRequest(new Response<string> { StatusCode = 400, Data = "", Messager = "Review isn't existed." });
            }

            _mapper.Map(reviewEdit, reviewModel);
            await _reviewRepos.Update(reviewModel);
            await _reviewRepos.SaveChange();

            var reviewReadModel = _mapper.Map<ReviewReadDto>(reviewModel);

            return Accepted(new Response<ReviewReadDto> { StatusCode = 202, Messager = "success", Data = reviewReadModel });
        }


        ///<summary>get detail of review</summary>
        ///<response code="202">thành công, xóa thành công</response>
        ///<response code="404">thất bại, không tìm thấy yêu cầu</response>
        ///<response code="401">token không hợp lệ, không phải admin</response> 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Response<ReportReadDto>>> Detail(int reviewid)
        {
            //get review
            var review = await _reviewRepos.GetReviewById(reviewid);
            if (review == null)
            {
                return BadRequest(new Response<string> { StatusCode = 400, Data = "", Messager = "There is no review." });
            }

            var reviewReadModel = _mapper.Map<ReviewReadDto>(review);
            return Ok(new Response<ReviewReadDto> { StatusCode = 200, Data = reviewReadModel, Messager = "success" });
        }


        ///<summary>Lấy danh sách review theo page, limit</summary>
        ///<response code="200">thanh cong</response>
        ///<response code="204">thanh cong, khong co du lieu</response>
        [HttpGet("get-by-page/{courseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<Response<PageDataDto<ReviewReadDto>>>> GetByPage(int courseId, int page, int limit)
        {
            if (limit == 0)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "invalid limit" });
            }
            var reviews = await _reviewRepos.GetByPage(courseId, page * limit, limit);
            var reviewReadModel = _mapper.Map<IEnumerable<ReviewReadDto>>(reviews.Data);
            var pageData = new PageDataDto<ReviewReadDto>
            {
                Count = reviews.Count,
                Data = reviewReadModel
            };
            return Ok(new Response<PageDataDto<ReviewReadDto>> { StatusCode = 200, Data = pageData, Messager = "success" });
        }
    }
}
