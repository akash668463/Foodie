using EmailAPI.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EmailAPI.Extension
{
    public static class ApplicationBuilderExtensions
    {
        private static IAzureServiceBusConsumer? ServiceBusConsumer { get; set; }

        public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app)
        {
            // read config safely first — if no Service Bus connection is configured, skip resolving the consumer
            var config = app.ApplicationServices.GetService<IConfiguration>();
            var connectionString = config?
                .GetValue<string>("ServiceBusConnectionString")
                ?? config?.GetValue<string>("ServiceBus:ConnectionString")
                ?? config?.GetValue<string>("ServiceBusConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                // no connection string -> do not attempt to resolve or start the consumer
                Console.WriteLine("Azure Service Bus connection string not configured. Skipping Service Bus consumer.");
                return app;
            }

            // try to resolve the consumer; guard against invalid connection string or other runtime errors
            try
            {
                ServiceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to resolve IAzureServiceBusConsumer: {ex.Message}. Service Bus consumer will be disabled.");
                ServiceBusConsumer = null;
                return app;
            }

            var hostApplicationLife = app.ApplicationServices.GetService<IHostApplicationLifetime>();
            if (hostApplicationLife == null)
            {
                Console.WriteLine("IHostApplicationLifetime not found. Skipping Service Bus consumer lifecycle registration.");
                return app;
            }

            hostApplicationLife.ApplicationStarted.Register(OnStart);
            hostApplicationLife.ApplicationStopping.Register(OnStop);

            return app;
        }

        private static void OnStop()
        {
            try
            {
                if (ServiceBusConsumer != null)
                {
                    // fire-and-forget stop
                    _ = ServiceBusConsumer.Stop();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error stopping Service Bus consumer: {ex.Message}");
            }
        }

        private static void OnStart()
        {
            try
            {
                if (ServiceBusConsumer != null)
                {
                    // start in background — do not block application start
                    _ = ServiceBusConsumer.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting Service Bus consumer: {ex.Message}");
            }
        }
    }
}
