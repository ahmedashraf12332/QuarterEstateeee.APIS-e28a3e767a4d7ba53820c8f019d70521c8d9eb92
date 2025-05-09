using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Service.Service
{
    public class BlobService : IBlobService
    {
        private readonly IConfiguration _configuration;

        public BlobService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string web)
        {
            var connectionString = _configuration.GetConnectionString("AzureBlobStorage");
            var container = new BlobContainerClient(connectionString, web);
            await container.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var fileName = Path.GetFileName(file.FileName);
            var blobClient = container.GetBlobClient(fileName);

            var blobHttpHeaders = new BlobHttpHeaders
            {
                ContentType = file.ContentType // 👈 ده بيخلي المتصفح يعرف إن دي صورة
            };

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new BlobUploadOptions
                {
                    HttpHeaders = blobHttpHeaders
                });
            }

            return blobClient.Uri.ToString();
        }
    }
}
