using Hackton.Domain.Interfaces.VideoResult.Repository;
using Hackton.Domain.Interfaces.VideoResult.UseCase;
using Hackton.Domain.VideoResult.Entity;
using Hackton.Domain.VideoResult.Exceptions;
using Hackton.Domain.VideoResult.UseCases;
using Moq;

namespace Hackton.Tests.UnitTests.Domain.VideoResult.UseCases
{
    public class GetVideoResultsUseCaseHandlerTests
    {
        private readonly Mock<IVideoResultRepository> _videoResultRepositoryMock;
        private readonly IGetVideoResultsUseCaseHandler<Guid, VideoResultEntity> _getVideoResultUseCaseHandler;


        public GetVideoResultsUseCaseHandlerTests()
        {
            _videoResultRepositoryMock = new Mock<IVideoResultRepository>();

            _getVideoResultUseCaseHandler = new GetVideoResultsUseCaseHandler(_videoResultRepositoryMock.Object);
        }

        [Fact(DisplayName = "Should Return Video Result")]
        public async Task ShouldReturnVideoResult()
        {
            var mockVideoId = Guid.NewGuid();

            var resultMock = new List<ResultItem>()
            {
                new ResultItem{
                    Description = "QrCode",
                    Time = TimeSpan.FromSeconds(100)
                },
            };

            _videoResultRepositoryMock.Setup(x => x.GetByVideoId(It.IsAny<Guid>()))
                .ReturnsAsync(new VideoResultEntity(mockVideoId, null));

            var result = await _getVideoResultUseCaseHandler.Handle(mockVideoId);

            _videoResultRepositoryMock.Verify(x => x.GetByVideoId(It.IsAny<Guid>()), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(mockVideoId, result.VideoId);
        }

        [Fact(DisplayName = "Should Return VideoResultNotFound When Result Does Not Exist")]
        public async Task ShouldReturnVideoResultNotFoundWhenResultDoesNotExist()
        {
            var mockVideoId = Guid.NewGuid();

            _videoResultRepositoryMock.Setup(x => x.GetByVideoId(It.IsAny<Guid>()))
                .ReturnsAsync((VideoResultEntity)null);

            var result = await Assert.ThrowsAsync<VideoResultNotFound>(() => _getVideoResultUseCaseHandler.Handle(mockVideoId));

            _videoResultRepositoryMock.Verify(x => x.GetByVideoId(It.IsAny<Guid>()), Times.Once);
        }
    }
}
