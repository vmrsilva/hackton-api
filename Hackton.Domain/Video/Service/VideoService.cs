using Hackton.Domain.Video.Entity;

namespace Hackton.Domain.Video.Service
{
    public class VideoService : IVideoService
    {
        public Task<VideoEntity> GetVideo(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task PostNewVideo(VideoEntity videoEntity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateVideo(VideoEntity videoEntity)
        {
            throw new NotImplementedException();
        }
    }
}
