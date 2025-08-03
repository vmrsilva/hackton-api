using Hackton.Domain.Interfaces.Video.Repository;
using Hackton.Domain.Interfaces.Video.UseCases;
using Hackton.Domain.Video.UseCases.CommandDtos;
using Hackton.Shared.UploadService;

namespace Hackton.Domain.Video.UseCases
{
    public class PostNewVideoUseCaseHandler : IPostNewVideoUseCaseHandler<PostNewVideoCommandDto>
    {
        private readonly IUploadFileService _uploadFileService;
        private readonly IVideoRepository _videoRepository;

        public PostNewVideoUseCaseHandler(IUploadFileService uploadFileService, IVideoRepository videoRepository)
        {
            _uploadFileService = uploadFileService;
            _videoRepository = videoRepository;
        }

        public async Task Handle(PostNewVideoCommandDto command, CancellationToken cancellation = default)
        {
            var filePath = await _uploadFileService.UploadVideoAsync(command.FileStream, command.FileName);

            if (filePath is null)
                throw new Exception("");

            var newEntity = command.VideoEntity;

            newEntity.FilePath = filePath;
            newEntity.Status = Enums.VideoStatusEnum.NaFila;

            await _videoRepository.Create(newEntity).ConfigureAwait(false);
        }
    }
}
