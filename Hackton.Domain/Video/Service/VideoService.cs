using Hackton.Domain.Video.Entity;
using Hackton.Shared.UploadService;
using Microsoft.AspNetCore.Http;

namespace Hackton.Domain.Video.Service
{
    public class VideoService : IVideoService
    {
        private readonly IUploadFileService _uploadFileService;

        public VideoService(IUploadFileService uploadFileService)
        {
            _uploadFileService = uploadFileService;
        }

        public Task<VideoEntity> GetVideo(Guid guid)
        {
            throw new NotImplementedException();
        }

        public async Task PostNewVideo(VideoEntity videoEntity, IFormFile file)
        {
            await _uploadFileService.UploadVideoAsync(file, "videos");
        

        }

        public Task UpdateVideo(VideoEntity videoEntity)
        {
            throw new NotImplementedException();
        }
    }
}
