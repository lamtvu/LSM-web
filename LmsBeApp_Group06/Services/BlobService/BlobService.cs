using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;

namespace LmsBeApp_Group06.Services.BlobService
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        public BlobService(BlobServiceClient blobServiceClient)
        {
            this._blobServiceClient = blobServiceClient;
        }
        public async Task DeleteBlobAsync(string blobName, string container)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(container);
            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.DeleteAsync();
        }

        public async Task<BlobDownloadResult> GetBlobAsync(string name, string container)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(container);
            var blobClient = containerClient.GetBlobClient(name);
            var blobDowload = await blobClient.DownloadContentAsync();
            return blobDowload;
        }
        public async Task<BlobDownloadStreamingResult> GetBlobStreamAsync(string name, string container)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(container);
            var blobClient = containerClient.GetBlobClient(name);
            var blobDowload = await blobClient.DownloadStreamingAsync();
            return blobDowload;
        }
        public Task<IEnumerable<string>> ListBlogsAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task UploadContentBlobAsync(byte[] content, string fileName, string container)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(container);
            var blobClient = containerClient.GetBlobClient(fileName);
            await using var memoryStream = new MemoryStream(content);
            string contentType;
            new FileExtensionContentTypeProvider().TryGetContentType(fileName, out contentType);
            await blobClient.UploadAsync(memoryStream, new BlobHttpHeaders { ContentType = contentType});
        }

        public async Task UploadFileBlobAsync(IFormFile file , string fileName, string container)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(container);
            var blobClient = containerClient.GetBlobClient(fileName);
            await using var memoryStream = file.OpenReadStream();
            await blobClient.UploadAsync(memoryStream, new BlobHttpHeaders { ContentType = file.ContentType});
        }
    }
}
