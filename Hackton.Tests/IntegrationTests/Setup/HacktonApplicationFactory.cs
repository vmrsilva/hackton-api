using Bogus;
using DotNet.Testcontainers.Builders;
using Hackton.Domain.Video.Entity;
using Hackton.Infrastructure.Context;
using Hackton.Shared.Messaging;
using Hackton.Shared.UploadService;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Runtime.InteropServices;
using Testcontainers.MsSql;
using Testcontainers.RabbitMq;

namespace Hackton.Tests.IntegrationTests.Setup
{
    public class HacktonApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MsSqlContainer _msSqlContainer;
        private readonly RabbitMqContainer _rabbitMqContainer;
        //private readonly RedisContainer _redisContainer;
        private readonly string _rabbitPwd = "guest";
        private readonly string _rabbitUser = "guest";
        private readonly Mock<IUploadFileService> _uploadFileService;
        private readonly Mock<IMessagingService> _messagingService;

        public HacktonApplicationFactory()
        {
            _uploadFileService = new Mock<IUploadFileService>();
            _messagingService = new Mock<IMessagingService>();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                _msSqlContainer = new MsSqlBuilder()
                    .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
                      .WithPassword("password(!)Strong")
                             .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
                             .Build();
            }
            else
            {
                _msSqlContainer = new MsSqlBuilder().Build();
            }

            _rabbitMqContainer = new RabbitMqBuilder()
                    .WithImage("masstransit/rabbitmq:latest")
                    .WithUsername(_rabbitPwd)
                    .WithPassword(_rabbitUser)
                    .WithPortBinding(5672, 5672)
                    .WithPortBinding(15672, 15672)
                    .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5672))
                    .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(15672))
                    .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                ConfigureDbContext(services);
                //ConfigureCache(services);
                MockServices(services);
                ConfigureRabbitMq(services);
            });

            base.ConfigureWebHost(builder);
        }

        private void ConfigureDbContext(IServiceCollection services)
        {
            var context = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(HacktonContext));
            if (context != null)
            {
                services.Remove(context);
                var options = services.Where(r => r.ServiceType == typeof(DbContextOptions)
                  || r.ServiceType.IsGenericType && r.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>)).ToArray();
                foreach (var option in options)
                {
                    services.Remove(option);
                }
            }

            services.AddDbContext<HacktonContext>(options =>
            {
                options.UseSqlServer(_msSqlContainer.GetConnectionString());

                var connectionString = _msSqlContainer.GetConnectionString();

                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(); // Habilita retry automático
                });

            });


            using (var serviceProvider = services.BuildServiceProvider())
            {
                var dbContext = serviceProvider.GetRequiredService<HacktonContext>();
                // var created = dbContext.Database.EnsureCreated();
                //if (created)
                dbContext.Database.Migrate();

                InitialSeed(dbContext);
            }
        }

        private void ConfigureRabbitMq(IServiceCollection services)
        {
            var rabbitMq = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IBus));
            if (rabbitMq != null)
            {
                services.Remove(rabbitMq);
            }

            var descriptorsToRemove = services
                .Where(d => d.ServiceType.FullName.Contains("MassTransit"))
                .ToList();

            foreach (var descriptor in descriptorsToRemove)
            {
                services.Remove(descriptor);
            }

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(_rabbitMqContainer.Hostname, "/", h =>
                    {
                        h.Username(_rabbitUser);
                        h.Password(_rabbitPwd);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });
        }

        private void MockServices(IServiceCollection services)
        {
            var uploadService = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IUploadFileService));
            if (uploadService != null)
            {
                services.Remove(uploadService);
            }

            //services.AddSingleton<IUploadFileService>(_uploadFileService.Object);
            services.AddScoped<IUploadFileService>(_ => _uploadFileService.Object);

            var messageService = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IMessagingService));
            if (messageService != null)
            {
                services.Remove(messageService);
            }

            //            services.AddSingleton<IMessagingService>(_messagingService.Object);
            services.AddScoped<IMessagingService>(_ => _messagingService.Object);
        }

        public Mock<IUploadFileService> GetUploadFileServiceMocked()
        {
            return _uploadFileService;
        }

        public Mock<IMessagingService> GetMessagingServiceMocked()
        {
            return _messagingService;
        }

        public async Task InitializeAsync()
        {
            await _msSqlContainer.StartAsync();

            //  await _redisContainer.StartAsync();

            await _rabbitMqContainer.StartAsync();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _msSqlContainer.StopAsync();
            //  await _redisContainer.StopAsync();
            await _rabbitMqContainer.StopAsync();
        }

        private void InitialSeed(HacktonContext context)
        {
            var video = new Faker<VideoEntity>()
                .RuleFor(f => f.Active, true)
                .RuleFor(f => f.Description, f => f.Lorem.Word())
                .RuleFor(f => f.FilePath, f => f.System.FileName())
                .RuleFor(f => f.Title, f => f.Lorem.Word())
                .Generate();

            context.Video.AddRange(video);

            context.SaveChanges();
        }
    }
}
