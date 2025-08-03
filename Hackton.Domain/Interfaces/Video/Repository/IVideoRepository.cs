using Hackton.Domain.Video.Entity;

namespace Hackton.Domain.Interfaces.Video.Repository
{
    public interface IVideoRepository
    {
        Task Create(VideoEntity videoEntity);

        Task<VideoEntity> GetById(Guid id);
    }
}
