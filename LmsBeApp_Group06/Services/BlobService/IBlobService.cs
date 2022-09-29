using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;

namespace LmsBeApp_Group06.Services.BlobService
{
    public interface IBlobService
    {
        Task<BlobDownloadResult> GetBlobAsync(string name, string container);
        Task<IEnumerable<string>> ListBlogsAsync();
        Task UploadFileBlobAsync(Microsoft.AspNetCore.Http.IFormFile file, string fileName, string container);
        Task UploadContentBlobAsync(byte[] content, string fileName, string container);
        public Task DeleteBlobAsync(string blobName, string container);
        Task<BlobDownloadStreamingResult> GetBlobStreamAsync(string name,string container);
    }
}
