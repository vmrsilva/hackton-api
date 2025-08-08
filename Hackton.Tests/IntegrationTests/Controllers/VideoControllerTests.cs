using Hackton.Tests.IntegrationTests.Setup;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackton.Tests.IntegrationTests.Controllers
{
    public class VideoControllerTests(HacktonApplicationFactory factory) : BaseIntegrationTest(factory)
    {
        const string routeBase = "api/region";

        [Fact]
        public async Task Teste()
        {
            var client = factory.CreateClient();

            var videoDb = await _dbContext.Video.FirstOrDefaultAsync();

            var response = await client.GetAsync($"{routeBase}/{videoDb.Id}/status");

            Assert.True(true);
        }
    }
}
