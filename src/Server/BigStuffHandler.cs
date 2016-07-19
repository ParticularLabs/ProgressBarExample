using System;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;

public class BigStuffHandler : IHandleMessages<TriggerBigStuff>
{
    public BigStuffHandler(IStatusStoreClient statusStoreClient)
    {
        _statusStoreClient = statusStoreClient;
    }

    public async Task Handle(TriggerBigStuff message, IMessageHandlerContext context)
    {
        Console.WriteLine("Handling some big stuff.");
        for (var index = 0; index <= message.HowMuchStuff; index++)
        {
            var workItemId = await DoSomeWorkForABatch(message, index);
            await _statusStoreClient.AddCompletedCommandToBatch(batchId: message.Id, messageId: workItemId);
        }
    }

    private async Task<Guid> DoSomeWorkForABatch(TriggerBigStuff message, int index)
    {
        await Task.Delay(100);
        Console.WriteLine("Batch Id: " + message.Id + " - Handling message " + index + " of " + message.HowMuchStuff);
        //Just pretend we did some stuff and sent a message
        return Guid.NewGuid();
    }

    readonly IStatusStoreClient _statusStoreClient;
}
