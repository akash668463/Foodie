namespace RewardAPI.Message
{
    public interface IAzureServiceBusConsumer
    {
        Task Start();
        Task Stop();
    }
}
