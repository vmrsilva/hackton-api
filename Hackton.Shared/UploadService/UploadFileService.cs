using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Hackton.Shared.UploadService.Settings;
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

        public async Task<string> UploadVideoAsync(Stream file, string blobName)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Arquivo de vídeo inválido");

            var blobServiceClient = new BlobServiceClient(_options.ConnectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_options.VideoContainerName);

            await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);

            //blobName ??= Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var blobClient = containerClient.GetBlobClient(blobName);

            await blobClient.UploadAsync(file, overwrite: true);

            return blobClient.Uri.ToString();
        }
    }
}
