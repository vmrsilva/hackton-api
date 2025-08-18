using Hackton.Domain.Interfaces.Abstractions.UseCaseAbstraction;
using Hackton.Domain.Video.Entity;
using Hackton.Domain.Video.UseCases;
using Hackton.Domain.Video.UseCases.CommandDtos;
using Hackton.Domain.VideoResult.Entity;
using Hackton.Domain.VideoResult.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace Hackton.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            //ConfigureVideo(services);
            ConfigureUseCases(services);

            return services;
        }

        private static void ConfigureUseCases(IServiceCollection services)
        {
            services.AddScoped<IUseCaseCommandHandler<PostNewVideoCommandDto>, PostNewVideoUseCaseHandler>();
            services.AddScoped<IUseCaseQueryHandler<Guid, VideoEntity>, GetVideoUseCaseHandler>();
            services.AddScoped<IUseCaseQueryHandler<Guid, VideoResultEntity>, GetVideoResultsUseCaseHandler>();
        }
    }
}
