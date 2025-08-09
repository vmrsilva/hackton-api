using Bogus;
using Hackton.Domain.Enums;
using Hackton.Domain.Interfaces.Video.Repository;
using Hackton.Domain.Interfaces.Video.UseCases;
using Hackton.Domain.Video.Entity;
using Hackton.Domain.Video.Exceptions;
using Hackton.Domain.Video.UseCases;
using Hackton.Domain.Video.UseCases.CommandDtos;
using Hackton.Shared.Dto.Video;
using Hackton.Shared.Messaging;
using Hackton.Shared.Messaging.Settings;
using Hackton.Shared.UploadService;
using Microsoft.Extensions.Options;
using Moq;

namespace Hackton.Tests.IntegrationTests.Domain.Video.UseCases
{
    public class PostNewVideoUseCaseHandlerTests
    {
        private readonly Mock<IUploadFileService> _uploadFileService;
        private readonly Mock<IVideoRepository> _videoRepository;
        private readonly Mock<IMessagingService> _messagingService;
        private readonly IPostNewVideoUseCaseHandler<PostNewVideoCommandDto> _postNewVideoUseCaseHandler;

        public PostNewVideoUseCaseHandlerTests()
        {
            _uploadFileService = new Mock<IUploadFileService>();
            _videoRepository = new Mock<IVideoRepository>();
            _messagingService = new Mock<IMessagingService>();

            var massTransitSettings = new MassTransitSettings
            {
                QueueVideos = "test-queue"
            };
            var options = Options.Create(massTransitSettings);

            _postNewVideoUseCaseHandler = new PostNewVideoUseCaseHandler(
                _uploadFileService.Object,
                _videoRepository.Object,
                _messagingService.Object,
                options
            );
        }

        [Fact(DisplayName = "Should Post Video With Success")]
        public async Task ShouldPostVideoWithSuccess()
        {
            var mockPostNewVideoCommandDto = Generate(".mp4");

            _videoRepository.Setup(x => x.Create(It.IsAny<VideoEntity>()))
                .Returns(Task.CompletedTask);

            _uploadFileService.Setup(x => x.UploadVideoAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                    .ReturnsAsync("https://blob.fake.com/video.mp4");

            _messagingService.Setup(x => x.SendMessage(It.IsAny<string>(), It.IsAny<VideoMessageDto>()))
                .ReturnsAsync(true);

            await _postNewVideoUseCaseHandler.Handle(mockPostNewVideoCommandDto).ConfigureAwait(false);

            _videoRepository.Verify(x => x.Create(It.IsAny<VideoEntity>()), Times.Once);
            _uploadFileService.Verify(x => x.UploadVideoAsync(It.IsAny<Stream>(), It.IsAny<string>()), Times.Once);
            _messagingService.Verify(x => x.SendMessage(It.IsAny<string>(), It.IsAny<VideoMessageDto>()), Times.Once);
        }

        [Fact(DisplayName = "Should Return VideoFilePathEmptyException When Video Path Is Empty")]
        public async Task ShouldReturnVideoFilePathEmptyExceptionWhenVideoPathIsEmpty()
        {
            var mockPostNewVideoCommandDto = Generate(".mp4");

            _videoRepository.Setup(x => x.Create(It.IsAny<VideoEntity>()))
                .Returns(Task.CompletedTask);

            _uploadFileService.Setup(x => x.UploadVideoAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                    .ReturnsAsync(string.Empty);

            _messagingService.Setup(x => x.SendMessage(It.IsAny<string>(), It.IsAny<VideoMessageDto>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<VideoFilePathEmptyException>(() => _postNewVideoUseCaseHandler.Handle(mockPostNewVideoCommandDto));

            _videoRepository.Verify(x => x.Create(It.IsAny<VideoEntity>()), Times.Never);
            _uploadFileService.Verify(x => x.UploadVideoAsync(It.IsAny<Stream>(), It.IsAny<string>()), Times.Once);
            _messagingService.Verify(x => x.SendMessage(It.IsAny<string>(), It.IsAny<VideoMessageDto>()), Times.Never);
        }

        [Fact(DisplayName = "Should Return VideoBrokerMessageFailException When Broker Does Not Return Success")]
        public async Task ShouldReturnVideoBrokerMessageFailExceptionWhenBrokerDoesNotReturnSuccess()
        {
            var mockPostNewVideoCommandDto = Generate(".mp4");

            _videoRepository.Setup(x => x.Create(It.IsAny<VideoEntity>()))
                .Returns(Task.CompletedTask);

            _uploadFileService.Setup(x => x.UploadVideoAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                    .ReturnsAsync("https://blob.fake.com/video.mp4");

            _messagingService.Setup(x => x.SendMessage(It.IsAny<string>(), It.IsAny<VideoMessageDto>()))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<VideoBrokerMessageFailException>(() => _postNewVideoUseCaseHandler.Handle(mockPostNewVideoCommandDto));

            _videoRepository.Verify(x => x.Create(It.IsAny<VideoEntity>()), Times.Once);
            _uploadFileService.Verify(x => x.UploadVideoAsync(It.IsAny<Stream>(), It.IsAny<string>()), Times.Once);
            _messagingService.Verify(x => x.SendMessage(It.IsAny<string>(), It.IsAny<VideoMessageDto>()), Times.Once);
        }

        #region Private Methods
        private PostNewVideoCommandDto Generate(string fileExtesion)
        {
            var faker = new Faker("pt_BR");

            var videoEntityFaker = new Faker<VideoEntity>("pt_BR")
                .RuleFor(v => v.Id, _ => Guid.NewGuid())
                .RuleFor(v => v.Title, f => f.Lorem.Sentence(3))
                .RuleFor(v => v.Description, f => f.Lorem.Paragraph())
                .RuleFor(v => v.Status, _ => VideoStatusEnum.NaFila)
                .RuleFor(v => v.Active, _ => true)
                .RuleFor(v => v.FilePath, f => f.Internet.Url());

            var videoEntity = videoEntityFaker.Generate();

            var fileBytes = faker.Random.Bytes(256);
            var fileStream = new MemoryStream(fileBytes);

            return new PostNewVideoCommandDto
            {
                VideoEntity = videoEntity,
                FileStream = fileStream,
                FileName = faker.System.FileName(fileExtesion)
            };
        }
        #endregion
    }
}
