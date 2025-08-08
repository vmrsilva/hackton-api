using MassTransit;
using Microsoft.Extensions.Configuration;

namespace Hackton.Shared.Messaging
{
    public class MessagingService : IMessagingService
    {
        private readonly IBus _bus;
        private readonly IConfiguration _configuration;
        public MessagingService(IBus bus, IConfiguration configuration)
        {
            _bus = bus;
            _configuration = configuration;
        }
        public async Task<bool> SendMessage<T>(string queueName, T message)
        {
            try
            {
                if (message == null)
                    return false;

                var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{queueName}"));

                await endpoint.Send(message);


                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
