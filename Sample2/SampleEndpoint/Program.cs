using System;
using System.Threading.Tasks;
using Common;
using NServiceBus;
using NServiceBus.Logging;

class Program
{
    static async Task Main()
    {
        var defaultFactory = LogManager.Use<DefaultFactory>();
        defaultFactory.Level(LogLevel.Debug);

        Console.Title = "SampleEndpoint";
        var endpointConfiguration = new EndpointConfiguration("SampleEndpoint");

        endpointConfiguration.ConfigureEndpoint();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine();
        Console.WriteLine("Storage locations:");
        Console.WriteLine($"Learning Persister: {LearningLocationHelper.SagaDirectory}");
        Console.WriteLine($"Learning Transport: {LearningLocationHelper.TransportDirectory}");

        Console.WriteLine();
        Console.WriteLine("Press 'Enter' to  exit");

        while (true)
        {
            Console.WriteLine();
            if (Console.ReadKey().Key != ConsoleKey.Enter)
            {
                break;
            }
        }

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}