using Hackton.Domain.Interfaces.Abstractions.UseCaseAbstraction;
using Hackton.Domain.Interfaces.Video.Repository;
using Hackton.Domain.Interfaces.Video.UseCases;
using Hackton.Domain.Video.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackton.Domain.Video.UseCases
{
    public class GetVideoUseCaseHandler : IGetVideoUseCaseHandler<Guid, VideoEntity>
    {
        private readonly IVideoRepository _videoRepository;

        public GetVideoUseCaseHandler(IVideoRepository videoRepository)
        {
            _videoRepository = videoRepository;
        }

        public async Task<Entity.VideoEntity> Handle(Guid command, CancellationToken cancellationToken = default)
        {

            var videoDb = await _videoRepository.GetById(command).ConfigureAwait(false);

            return videoDb;
        }
    }
}
