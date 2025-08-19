using Hackton.Domain.Interfaces.VideoResult.Repository;
using Hackton.Domain.Interfaces.VideoResult.UseCase;
using Hackton.Domain.VideoResult.Entity;
using Hackton.Domain.VideoResult.Exceptions;

namespace Hackton.Domain.VideoResult.UseCases
{
    public class GetVideoResultsUseCaseHandler : IGetVideoResultsUseCaseHandler<Guid, VideoResultEntity>
    {
        private readonly IVideoResultRepository _videoResultRepository;

        public GetVideoResultsUseCaseHandler(IVideoResultRepository videoResultRepository)
        {
            _videoResultRepository = videoResultRepository;
        }

        public async Task<VideoResultEntity> Handle(Guid command, CancellationToken cancellationToken = default)
        {
            var result = await _videoResultRepository.GetByVideoId(command).ConfigureAwait(false);

            return result ?? throw new VideoResultNotFound();
        }
    }
}
