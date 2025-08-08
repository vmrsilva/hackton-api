namespace Hackton.Shared.Messaging
{
    public interface IMessagingService
    {
        Task<bool> SendMessage<T>(string queueName, T message);
    }
}
