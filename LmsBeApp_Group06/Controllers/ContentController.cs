using System.Threading.Tasks;
using AutoMapper;
using LmsBeApp_Group06.Data.Repositories.ContentRepo;
using LmsBeApp_Group06.Data.Repositories.SectionRepo;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;
using LmsBeApp_Group06.Services.BlobService;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Microsoft.AspNetCore.StaticFiles;
using System;

namespace LmsBeApp_Group06.Controllers
{
    [ApiController]
    [Route("api/Content")]
    public class ContentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IContentRepo _contentRepo;
        private readonly ISectionRepo _sectionRepo;
        private readonly IBlobService _blobService;
        public ContentController(IContentRepo contentRepo, ISectionRepo sectionRepo, IMapper mapper, IBlobService blobService)
        {
            this._blobService = blobService;
            this._sectionRepo = sectionRepo;
            this._contentRepo = contentRepo;
            this._mapper = mapper;
        }

        [HttpPost("{sectionId}")]
        public async Task<ActionResult<Response<object>>> CreateContent([FromForm] ContentCreateDto contentCreate, int sectionId)
        {
            var sectionModel = await _sectionRepo.GetById(sectionId);
            if (sectionModel == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "section does not exist" });
            }
            if (Request.Form.Files.Count == 0)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "required file" });
            }
            var contentModel = _mapper.Map<Content>(contentCreate);
            await _contentRepo.Create(sectionModel, contentModel);
            var file = Request.Form.Files[0];
            contentModel.FileType = file.FileName;
            await _contentRepo.SaveChange();
            //save file
            await _blobService.UploadFileBlobAsync(file, "content" + contentModel.Id + contentModel.FileType, "lmscontainer");
            return Ok(new Response<object> { Data = null, StatusCode = 200, Messager = "success" });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<object>>> DeleteContent(int id)
        {
            var contentModel = await _contentRepo.GetById(id);
            if (contentModel == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "content does not exist" });
            }
            await _contentRepo.Delete(contentModel);
            await _contentRepo.SaveChange();
            await _blobService.DeleteBlobAsync("content" + contentModel.Id + contentModel.FileType, "lmscontainer");
            return Ok(new Response<object> { Data = null, StatusCode = 200, Messager = "success" });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Response<object>>> ChangeContent(int id, ContentCreateDto contentChange)
        {
            var contentModel = await _contentRepo.GetById(id);
            if (contentModel == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "content does not exist" });
            }
            _mapper.Map(contentChange, contentModel);
            await _contentRepo.SaveChange();
            return Ok(new Response<object> { Data = null, StatusCode = 200, Messager = "success" });
        }
        
        [HttpPut("file/{contentId}")]
        public async Task<ActionResult<Response<object>>> ChangeFile(int contentId)
        {
            var contentModel = await _contentRepo.GetById(contentId);
            if (contentModel == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "content does not exist" });
            }
            if (Request.Form.Files.Count == 0)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "required file" });
            }
            var file = Request.Form.Files[0];
            //removle file cu
            await _blobService.DeleteBlobAsync("content" + contentModel.Id + contentModel.FileType, "lmscontainer");
            contentModel.FileType = file.FileName;
            await _contentRepo.SaveChange();
            //save file moi
            await _blobService.UploadFileBlobAsync(file, "content" + contentModel.Id + contentModel.FileType, "lmscontainer");
            return Ok(new Response<object> { Data = null, StatusCode = 200, Messager = "success" });
        }

        [HttpGet("file/{contentId}")]
        public async Task<ActionResult> GetFile(int contentId)
        {
            var content = await _contentRepo.GetById(contentId);
            if (content == null)
            {
                return BadRequest(new Response<object> { Data = null, StatusCode = 400, Messager = "content does not exist" });
            }
            var blobResult = await _blobService.GetBlobStreamAsync("content" + content.Id + content.FileType, "lmscontainer");
            string contentType;
            new FileExtensionContentTypeProvider().TryGetContentType(content.FileType, out contentType);
            return new FileStreamResult(blobResult.Content, contentType);
        }
    }
}
