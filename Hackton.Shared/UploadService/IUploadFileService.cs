
using Microsoft.AspNetCore.Http;


namespace Hackton.Shared.UploadService
{
    public interface IUploadFileService
    {
        Task<string> UploadVideoAsync(IFormFile videoFile, string blobName = null);
    }
}
