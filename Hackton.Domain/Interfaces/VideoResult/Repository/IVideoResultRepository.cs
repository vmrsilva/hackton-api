using Hackton.Domain.VideoResult.Entity;

namespace Hackton.Domain.Interfaces.VideoResult.Repository
{
    public interface IVideoResultRepository
    {
        Task<VideoResultEntity> GetByVideoId(Guid VideoId);
    }
}
