using Hackton.Infrastructure.Context;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Hackton.Tests.IntegrationTests.Setup
{
    public class BaseIntegrationTest : IClassFixture<HacktonApplicationFactory>, IDisposable
    {
        private readonly IServiceScope _scope;
        protected readonly JsonSerializerOptions _jsonSerializerOptions;
        //  protected readonly IRegionService _regionService;
        protected readonly HacktonContext _dbContext;

        public BaseIntegrationTest(HacktonApplicationFactory factory)
        {
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            _scope = factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<HacktonContext>();
            // _regionService = _scope.ServiceProvider.GetRequiredService<IRegionService>();
        }

        public void Dispose()
        {
            _scope?.Dispose();
        }
    }
}
