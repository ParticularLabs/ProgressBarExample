using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Web.Http;

namespace ProgressBarWebAPI
{
    public class StatusStoreController : ApiController
    {
        static readonly ConcurrentDictionary<Guid, ConcurrentBag<Guid>> _completedItems = new ConcurrentDictionary<Guid, ConcurrentBag<Guid>>();

        [HttpPost]
        [Route("CompleteMessage")]
        public void CompleteMessage(CompletedMessage message)
        {
            var completed = _completedItems.GetOrAdd(message.BatchId, id => new ConcurrentBag<Guid>());
            completed.Add(message.MessageId);
            Console.WriteLine($"Message {message.MessageId} marked as completed.");
        }

        [HttpGet]
        [Route("Batch")]
        public BatchStatus Status(string id)
        {
            Console.WriteLine($"Query for status of {id}.");

            var batchId = Guid.Parse(id);

            ConcurrentBag<Guid> list;
            if (!_completedItems.TryGetValue(batchId, out list))
            {
                return null;
            }

            return new BatchStatus
            {
                Id = batchId,
                ItemsCompleted = list.ToArray()
            };
        }
    }
}