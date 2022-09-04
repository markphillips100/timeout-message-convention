using System;
using System.Threading.Tasks;
using Common;
using NServiceBus;
using NServiceBus.Logging;
using SampleDomain.Messages;

class Program
{
    static async Task Main()
    {
        var defaultFactory = LogManager.Use<DefaultFactory>();
        defaultFactory.Level(LogLevel.Debug);

        Console.Title = "SampleClient";
        var endpointConfiguration = new EndpointConfiguration("SampleClient");

        endpointConfiguration.ConfigureEndpoint();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine();
        Console.WriteLine("Storage locations:");
        Console.WriteLine($"Learning Persister: {LearningLocationHelper.SagaDirectory}");
        Console.WriteLine($"Learning Transport: {LearningLocationHelper.TransportDirectory}");

        Console.WriteLine();
        Console.WriteLine("Press 'Enter' to send a StartOrder message");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            Console.WriteLine();
            if (Console.ReadKey().Key != ConsoleKey.Enter)
            {
                break;
            }
            var orderId = Guid.NewGuid();
            var startOrder = new StartOrder
            {
                OrderId = orderId
            };
            await endpointInstance.Send("SampleEndpoint", startOrder)
                .ConfigureAwait(false);
            Console.WriteLine($"Sent StartOrder with OrderId {orderId}.");
        }

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}