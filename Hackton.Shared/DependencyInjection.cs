
using Hackton.Shared.Dto.Video;
using Hackton.Shared.Messaging;
using Hackton.Shared.Messaging.Settings;
using Hackton.Shared.UploadService;
using Hackton.Shared.UploadService.Settings;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hackton.Shared
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddShared(this IServiceCollection services, IConfiguration configuration)
        {
            ConfigureMessaging(services, configuration);
            ConfigureUploadService(services, configuration);

            return services;
        }

        private static void ConfigureMessaging(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MassTransitSettings>(configuration.GetSection("MassTransit"));

            var servidor = configuration.GetSection("MassTransit")["Server"] ?? string.Empty;
            var usuario = configuration.GetSection("MassTransit")["User"] ?? string.Empty;
            var senha = configuration.GetSection("MassTransit")["Password"] ?? string.Empty;

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(servidor, "/", h =>
                    {
                        h.Username(usuario);
                        h.Password(senha);
                    });

                    cfg.Message<VideoMessageDto>(m =>
                    {
                        m.SetEntityName("video-posted-exchange");
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddScoped<IMessagingService, MessagingService>();
        }

        private static void ConfigureUploadService(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AzureBlobOptions>(configuration.GetSection("AzureBlob"));
            services.AddScoped<IUploadFileService, UploadFileService>();
        }
    }
}
