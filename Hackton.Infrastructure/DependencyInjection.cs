using Hackton.Domain.Interfaces;
using Hackton.Domain.Interfaces.Video.Repository;
using Hackton.Infrastructure.Context;
using Hackton.Infrastructure.Repository.Base;
using Hackton.Infrastructure.Repository.Video;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Hackton.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            ConfigureContext(services, config);
            ConfigureRepositories(services);

            return services;
        }

        private static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IVideoRepository, VideoRepository>();
        }
        private static void ConfigureContext(IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("Database");

            services.AddDbContext<HacktonContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            using (var serviceProvider = services.BuildServiceProvider())
            {
                var dbContext = serviceProvider.GetRequiredService<HacktonContext>();
            }
        }
    }
}
