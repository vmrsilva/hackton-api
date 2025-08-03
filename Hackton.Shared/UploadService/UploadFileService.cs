using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace Hackton.Shared.UploadService
{
    public class UploadFileService : IUploadFileService
    {
        private readonly string _connectionString;
        private readonly string _containerName;

        public UploadFileService(string connectionString, string containerName)
        {
            _connectionString = connectionString;
            _containerName = containerName;
        }

        public async Task<string> UploadVideoAsync(IFormFile videoFile, string blobName = null)
        {
            if (videoFile == null || videoFile.Length == 0)
                throw new ArgumentException("Arquivo de vídeo inválido");

            var blobServiceClient = new BlobServiceClient(_connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

            // Cria o container se não existir
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);

            blobName ??= Guid.NewGuid().ToString() + Path.GetExtension(videoFile.FileName);
            var blobClient = containerClient.GetBlobClient(blobName);

            using var stream = videoFile.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            return blobClient.Uri.ToString();
        }
    }
}
