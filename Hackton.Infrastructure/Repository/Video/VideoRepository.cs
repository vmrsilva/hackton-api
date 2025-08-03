using Hackton.Domain.Interfaces;
using Hackton.Domain.Interfaces.Video.Repository;
using Hackton.Domain.Video.Entity;

namespace Hackton.Infrastructure.Repository.Video
{
    public class VideoRepository : IVideoRepository
    {
        private readonly IBaseRepository<VideoEntity> _repository;

        public VideoRepository(IBaseRepository<VideoEntity> repository)
        {
            _repository = repository;
        }

        public async Task Create(VideoEntity videoEntity)
        {
            await _repository.AddAsync(videoEntity).ConfigureAwait(false);
        }

        public async Task<VideoEntity> GetById(Guid id)
        {
            return await _repository.GetByIdAsync(id).ConfigureAwait(false);
        }
    }
}
