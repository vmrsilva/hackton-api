using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Hackton.Shared.UploadService
{
    public class UploadFileService : IUploadFileService
    {
        private readonly AzureBlobOptions _options;

        public UploadFileService(IOptions<AzureBlobOptions> options)
        {
            _options = options.Value;
        }

        public async Task<string> UploadVideoAsync(IFormFile videoFile, string blobName = null)
        {
            if (videoFile == null || videoFile.Length == 0)
                throw new ArgumentException("Arquivo de vídeo inválido");

            var blobServiceClient = new BlobServiceClient(_options.ConnectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_options.VideoContainerName);

            await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);

            blobName ??= Guid.NewGuid().ToString() + Path.GetExtension(videoFile.FileName);
            var blobClient = containerClient.GetBlobClient(blobName);

            using var stream = videoFile.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            return blobClient.Uri.ToString();
        }
    }
}
