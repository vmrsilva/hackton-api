using Hackton.Shared.Messaging;
using Hackton.Shared.UploadService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hackton.Shared
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddShared(this IServiceCollection services)
        {
            ConfigureMessaging(services);
            ConfigureUploadService(services);

            return services;
        }

        private static void ConfigureMessaging(IServiceCollection services)
        {
            services.AddScoped<IMessagingService, MessagingService>();
        }

        private static void ConfigureUploadService(IServiceCollection services)
        {
            services.AddScoped<IUploadFileService, UploadFileService>();
        }
    }
}
