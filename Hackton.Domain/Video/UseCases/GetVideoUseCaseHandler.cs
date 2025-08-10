using Hackton.Domain.Interfaces.Video.Repository;
using Hackton.Domain.Interfaces.Video.UseCases;
using Hackton.Domain.Video.Entity;
using Hackton.Domain.Video.Exceptions;

namespace Hackton.Domain.Video.UseCases
{
    public class GetVideoUseCaseHandler : IGetVideoUseCaseHandler<Guid, VideoEntity>
    {
        private readonly IVideoRepository _videoRepository;

        public GetVideoUseCaseHandler(IVideoRepository videoRepository)
        {
            _videoRepository = videoRepository;
        }

        public async Task<VideoEntity> Handle(Guid command, CancellationToken cancellationToken = default)
        {

            var videoDb = await _videoRepository.GetById(command).ConfigureAwait(false);

            if (videoDb is null)
                throw new VideoNotFoundException();

            return videoDb;
        }
    }
}
