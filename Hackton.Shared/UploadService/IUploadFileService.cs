namespace Hackton.Shared.UploadService
{
    public interface IUploadFileService
    {
        Task<string> UploadVideoAsync(Stream videoFile, string blobName = null);
    }
}
