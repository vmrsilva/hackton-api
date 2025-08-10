using Bogus;
using Hackton.Api.Controllers.Video.Dto;
using Hackton.Domain.Video.Entity;
using Mapster;

namespace Hackton.Tests.UnitTests.Domain.Video.Entity
{
    public class VideoEntityTests
    {
        [Fact(DisplayName = "Should Create New Video Entity With Sucess")]
        public void ShouldCreateNewVideoEntityWithSucess()
        {
            var videoMockDto = new Faker<CreateVideoDto>()
                .RuleFor(f => f.Title, f => f.Lorem.Word())
                .RuleFor(f => f.Description, f => f.Lorem.Sentence())
                .Generate();

            var videoEntity = videoMockDto.Adapt<VideoEntity>();

            Assert.NotNull(videoEntity);
            Assert.Equal(videoMockDto.Title, videoEntity.Title);
            Assert.Equal(videoMockDto.Description, videoEntity.Description);
            Assert.NotEqual(Guid.Empty, videoEntity.Id);
            Assert.True(videoEntity.Active);
            Assert.True(videoEntity.CreateAt.Year == DateTime.UtcNow.Year);
        }

        [Fact(DisplayName = "Should Change Active to Disable")]
        public void ShouldChangeActiveToDisable()
        {
            var videoMockDto = new Faker<CreateVideoDto>()
                .RuleFor(f => f.Title, f => f.Lorem.Word())
                .RuleFor(f => f.Description, f => f.Lorem.Sentence())
                .Generate();

            var videoEntity = videoMockDto.Adapt<VideoEntity>();

            videoEntity.MarkAsDeleted();

            Assert.False(videoEntity.Active);
        }

    }
}
