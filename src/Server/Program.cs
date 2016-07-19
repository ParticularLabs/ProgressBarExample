using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        HostEndpoint().GetAwaiter().GetResult();
    }

    static async Task HostEndpoint()
    {
        var endpointConfiguration = new EndpointConfiguration("Samples.ProgressBar.Endpoint");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.LimitMessageProcessingConcurrencyTo(1);
        endpointConfiguration.RegisterComponents(c => c.ConfigureComponent<StatusStoreClient>(DependencyLifecycle.InstancePerCall));

        var endpoint = await Endpoint.Start(endpointConfiguration);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpoint.Stop();
    }
}