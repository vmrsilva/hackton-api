using Hackton.Domain.Video.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Hackton.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            ConfigureVideo(services);

            return services;
        }

        private static void ConfigureVideo(IServiceCollection services)
        {
            services.AddScoped<IVideoService, VideoService>();
        }
    }
}
