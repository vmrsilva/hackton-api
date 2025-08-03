using Hackton.Domain.Video.Entity;

namespace Hackton.Domain.Video.Service
{
    public interface IVideoService
    {
        Task PostNewVideo(VideoEntity videoEntity);
        Task<VideoEntity> GetVideo(Guid guid);
        Task UpdateVideo(VideoEntity videoEntity);
    }
}
