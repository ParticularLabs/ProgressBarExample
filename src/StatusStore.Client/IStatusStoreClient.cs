using System;

public interface IStatusStoreClient
{
    void AddCompletedCommandToBatch(Guid batchId, Guid messageId);
    BatchStatus GetBatchStatus(string batchId);
}