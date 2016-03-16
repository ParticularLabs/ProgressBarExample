using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.ProgressBar.Endpoint");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        busConfiguration.RegisterComponents(c => c.ConfigureComponent<StatusStoreClient>(DependencyLifecycle.InstancePerCall));

        var excludesBuilder =
            AllAssemblies
                .Except("System.*");

        busConfiguration.AssembliesToScan(excludesBuilder);

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}