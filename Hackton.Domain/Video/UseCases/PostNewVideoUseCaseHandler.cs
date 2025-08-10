using Hackton.Domain.Interfaces.Video.Repository;
using Hackton.Domain.Interfaces.Video.UseCases;
using Hackton.Domain.Video.Exceptions;
using Hackton.Domain.Video.UseCases.CommandDtos;
using Hackton.Shared.Dto.Video;
using Hackton.Shared.Messaging;
using Hackton.Shared.Messaging.Settings;
using Hackton.Shared.UploadService;
using Microsoft.Extensions.Options;

namespace Hackton.Domain.Video.UseCases
{
    public class PostNewVideoUseCaseHandler : IPostNewVideoUseCaseHandler<PostNewVideoCommandDto>
    {
        private readonly IUploadFileService _uploadFileService;
        private readonly IVideoRepository _videoRepository;
        private readonly IMessagingService _messagingService;
        private readonly MassTransitSettings _massTransitSettings;

        public PostNewVideoUseCaseHandler(IUploadFileService uploadFileService,
                                          IVideoRepository videoRepository,
                                          IMessagingService messagingService,
                                          IOptions<MassTransitSettings> massTransitOptions)
        {
            _uploadFileService = uploadFileService;
            _videoRepository = videoRepository;
            _messagingService = messagingService;
            _massTransitSettings = massTransitOptions.Value;
        }

        public async Task Handle(PostNewVideoCommandDto command, CancellationToken cancellation = default)
        {


            var filePath = await _uploadFileService.UploadVideoAsync(command.FileStream, command.FileName);

            if (string.IsNullOrEmpty(filePath))
                throw new VideoFilePathEmptyException();

            var newEntity = command.VideoEntity;

            newEntity.FilePath = filePath;
            newEntity.Status = Enums.VideoStatusEnum.NaFila;

            await _videoRepository.Create(newEntity).ConfigureAwait(false);

            var message = new VideoMessageDto
            {
                FileUrl = filePath,
                VideoId = newEntity.Id
            };

            var queueName = _massTransitSettings.QueueVideos;

            var messageSent = await _messagingService.SendMessage(queueName, new VideoMessageDto { FileUrl = filePath, VideoId = newEntity.Id }).ConfigureAwait(false);

            if (!messageSent)
                throw new VideoBrokerMessageFailException();
        }
    }
}
