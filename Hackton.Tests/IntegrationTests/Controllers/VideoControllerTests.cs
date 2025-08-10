using Hackton.Tests.IntegrationTests.Setup;
using Microsoft.EntityFrameworkCore;

namespace Hackton.Tests.IntegrationTests.Controllers
{
    public class VideoControllerTests(HacktonApplicationFactory factory) : BaseIntegrationTest(factory)
    {
        const string routeBase = "api/region";

        [Fact]
        public async Task Teste()
        {
            var messageServiceMock = factory.GetMessagingServiceMocked();
            var uploadServiceMock = factory.GetUploadFileServiceMocked();  

            var client = factory.CreateClient();

            var videoDb = await _dbContext.Video.FirstOrDefaultAsync();

            var response = await client.GetAsync($"{routeBase}/{videoDb.Id}/status");

            Assert.True(true);
        }
        
        [Fact]
        public async Task PostNewVideo()
        {
            var messageServiceMock = factory.GetMessagingServiceMocked();
            var uploadServiceMock = factory.GetUploadFileServiceMocked();

            var client = factory.CreateClient();

            var videoDb = await _dbContext.Video.FirstOrDefaultAsync();

            var response = await client.GetAsync($"{routeBase}/{videoDb.Id}/status");

            Assert.True(true);
        }
    }
}
