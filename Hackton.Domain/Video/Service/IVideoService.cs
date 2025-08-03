using Hackton.Domain.Video.Entity;
using Microsoft.AspNetCore.Http;

namespace Hackton.Domain.Video.Service
{
    public interface IVideoService
    {
        Task PostNewVideo(VideoEntity videoEntity, IFormFile file);
        Task<VideoEntity> GetVideo(Guid guid);
        Task UpdateVideo(VideoEntity videoEntity);
    }
}
