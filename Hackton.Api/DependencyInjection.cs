using Hackton.Api.Mapping;
using Mapster;
using MapsterMapper;

namespace Hackton.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApi(this IServiceCollection services)
        {
            services.AddSingleton(TypeAdapterConfig.GlobalSettings);
            services.AddScoped<IMapper, ServiceMapper>();

            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(VideoMappingConfig).Assembly);

            return services;
        }
    }
}
