using System;
using System.Threading;
using NServiceBus;

public class BigStuffHandler : IHandleMessages<TriggerBigStuff>
{
    IBus _bus;
    readonly IStatusStoreClient _statusStoreClient;

    public BigStuffHandler(IBus bus, IStatusStoreClient statusStoreClient)
    {
        _bus = bus;
        _statusStoreClient = statusStoreClient;
    }

    public void Handle(TriggerBigStuff message)
    {
        Console.WriteLine("Handling some big stuff.");
        for (var index = 0; index <= message.HowMuchStuff; index++)
        {
            var messageId = DoSomeWorkForABatch(message, index);
            _statusStoreClient.AddCompletedCommandToBatch(batchId: message.Id, messageId: messageId);
        }
    }

    private Guid DoSomeWorkForABatch(TriggerBigStuff message, int index)
    {
        Thread.Sleep(10);
        Console.WriteLine("Batch Id: " + message.Id + " - Handling message " + index + " of " + message.HowMuchStuff);
        //Just pretend we did some stuff and sent a message
        return Guid.NewGuid();
    }
}
