using Bogus;
using Hackton.Domain.Enums;
using Hackton.Domain.Interfaces.Video.Repository;
using Hackton.Domain.Interfaces.Video.UseCases;
using Hackton.Domain.Video.Entity;
using Hackton.Domain.Video.Exceptions;
using Hackton.Domain.Video.UseCases;
using Moq;

namespace Hackton.Tests.IntegrationTests.Domain.Video.UseCases
{
    public class GetVideoUseCaseHandlerTests
    {
        private readonly Mock<IVideoRepository> _videoRepository;
        private readonly IGetVideoUseCaseHandler<Guid, VideoEntity> _getVideoUseCaseHandler;



        public GetVideoUseCaseHandlerTests()
        {
            _videoRepository = new Mock<IVideoRepository>();

            _getVideoUseCaseHandler = new GetVideoUseCaseHandler(_videoRepository.Object);
        }

        [Fact(DisplayName = "Should Return Video With Success")]
        public async Task ShouldReturnVideoWithSuccess()
        {
            var videoId = Guid.NewGuid();

            var mockVideoEntity = new Faker<VideoEntity>()
                .RuleFor(f => f.Id, videoId)
                .RuleFor(f => f.Title, f => f.Lorem.Word())
                .RuleFor(f => f.Description, f => f.Lorem.Sentence())
                .RuleFor(f => f.FilePath, f => f.Internet.Url())
                .RuleFor(f => f.Status, VideoStatusEnum.NaFila)
                .RuleFor(f => f.Active, true)
                .RuleFor(f => f.CreateAt, DateTime.UtcNow)
                .Generate();

            _videoRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(mockVideoEntity);

            var result = await _getVideoUseCaseHandler.Handle(videoId);

            Assert.NotNull(result);
            Assert.Equal(mockVideoEntity.Id, result.Id);
            _videoRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
        }

        [Fact(DisplayName = "Should Return VideoNotFoundException When Video Does Not Exist")]
        public async Task ShouldReturnVideoNotFoundExceptionWhenVideoDoesNotExist()
        {
            var videoId = Guid.NewGuid();

            _videoRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync((VideoEntity)null);

            var result = await Assert.ThrowsAsync<VideoNotFoundException>(() => _getVideoUseCaseHandler.Handle(videoId));

            _videoRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
        }
    }
}
