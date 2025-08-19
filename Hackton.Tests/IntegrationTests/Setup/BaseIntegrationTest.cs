using Hackton.Infrastructure.Context;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Text.Json;

namespace Hackton.Tests.IntegrationTests.Setup
{
    public class BaseIntegrationTest : IClassFixture<HacktonApplicationFactory>, IDisposable
    {
        private readonly IServiceScope _scope;
        protected readonly JsonSerializerOptions _jsonSerializerOptions;
        protected readonly HacktonContext _dbContext;
        protected readonly IMongoDatabase _hacktonMongoContext;

        public BaseIntegrationTest(HacktonApplicationFactory factory)
        {
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            _scope = factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<HacktonContext>();
            _hacktonMongoContext = _scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
        }

        public void Dispose()
        {
            _scope?.Dispose();
        }
    }
}
