using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LmsBeApp_Group06.Data.Repositories.CourseRepos;
using LmsBeApp_Group06.Data.Repositories.SectionRepo;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Filters;
using LmsBeApp_Group06.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LmsBeApp_Group06.Controllers
{
    [Route("api/Section")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        private readonly ISectionRepo _sectionRepo;
        private readonly IMapper _mapper;
        private readonly ICourseRepo _courseRepo;
        public SectionController(ISectionRepo sectionRepo, ICourseRepo courseRepo, IMapper mapper)
        {
            this._courseRepo = courseRepo;
            this._mapper = mapper;
            this._sectionRepo = sectionRepo;

        }

        ///<summary>lấy section theo class id</summary>
        [HttpGet("get-by-courseid/{id}")]
        [Authorize]
        [ServiceFilter(typeof(UserCheck))]
        public async Task<ActionResult<Response<IEnumerable<SectionReadDto>>>> GetByCourseId(int id)
        {
            var sections = await _sectionRepo.GetByCourseId(id);
            var sectionRead = _mapper.Map<IEnumerable<SectionReadDto>>(sections);
            return Ok(new Response<IEnumerable<SectionReadDto>> { StatusCode = 200, Data = sectionRead, Messager = "success" });
        }

        ///<summary>tạo section theo course id</summary>
        [HttpPost("{id}")]
        [Authorize]
        [ServiceFilter(typeof(UserCheck))]
        public async Task<ActionResult<Response<object>>> Create(SectionCreateDto sectionCreateDto, int id)
        {
            var sectionModel = _mapper.Map<Section>(sectionCreateDto);
            var courseModel = await _courseRepo.GetById(id);
            if (courseModel == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "not found course" });
            }
            await _sectionRepo.Create(sectionModel, courseModel);
            await _sectionRepo.SaveChange();
            return Ok(new Response<object> { Data = null, StatusCode = 200, Messager = "success" });
        }

        ///<summary> xóa section theo section id </summary>
        [HttpDelete("{id}")]
        [Authorize]
        [ServiceFilter(typeof(UserCheck))]
        public async Task<ActionResult<Response<object>>> Delete(int id)
        {
            var section = await _sectionRepo.GetById(id);
            await _sectionRepo.Delete(section);
            await _sectionRepo.SaveChange();
            return Ok(new Response<object> { Data = null, StatusCode = 200, Messager = "success" });
        }

        ///<summary>sửa section theo class id</summary>
        [HttpPut("{id}")]
        [Authorize]
        [ServiceFilter(typeof(UserCheck))]
        public async Task<ActionResult<Response<object>>> Change(SectionCreateDto sectionChange, int id)
        {
            var sectionModel = await _sectionRepo.GetById(id);
            _mapper.Map(sectionChange, sectionModel);
            await _sectionRepo.SaveChange();
            return Ok(new Response<object> { Data = null, StatusCode = 200, Messager = "success" });
        }
    }
}
