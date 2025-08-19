using Hackton.Domain.Interfaces.VideoResult.Repository;
using Hackton.Domain.VideoResult.Entity;
using Hackton.Infrastructure.Context;
using MongoDB.Driver;

namespace Hackton.Infrastructure.MongoRepository
{
    public class VideoResultRepository : IVideoResultRepository
    {
        private readonly HacktonMongoContext _mongoContext;

        public VideoResultRepository(HacktonMongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        public async Task Create(VideoResultEntity videoResultEntity)
        {
            await _mongoContext.VideoResults.InsertOneAsync(videoResultEntity).ConfigureAwait(false);
        }

        public async Task<VideoResultEntity> GetByVideoId(Guid VideoId)
        {
            return await _mongoContext.VideoResults
                .Find(x => x.VideoId == VideoId)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);
        }
    }
}
