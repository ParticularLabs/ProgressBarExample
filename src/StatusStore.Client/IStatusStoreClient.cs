using System;
using System.Threading.Tasks;

public interface IStatusStoreClient
{
    Task AddCompletedCommandToBatch(Guid batchId, Guid messageId);
    Task<BatchStatus> GetBatchStatus(string batchId);
}