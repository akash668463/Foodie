using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace MessageBus
{
    public class MessageBus : IMessageBus
    {
        private readonly string _connectionString;

        public MessageBus(IConfiguration configuration)
        {
            _connectionString = configuration["ServiceBus:ConnectionString"];
        }

        public async Task PublishMessage(object message, string topic_queue_Name)
        {
            await using var client = new ServiceBusClient(_connectionString);

            var sender = client.CreateSender(topic_queue_Name);

            var jsonMessage = JsonConvert.SerializeObject(message);

            var finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString(),
            };

            await sender.SendMessageAsync(finalMessage);
        }
    }
}