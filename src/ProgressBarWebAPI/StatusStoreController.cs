using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ProgressBarWebAPI
{
    public class StatusStoreController : ApiController
    {
        readonly Dictionary<Guid, List<Guid>> _completedItems = new Dictionary<Guid, List<Guid>>();

        [HttpPost]
        [Route("CompleteMessage")]
        public void CompleteMessage(CompletedMessage message)
        {
            if (!_completedItems.ContainsKey(message.BatchId))
            {
                _completedItems.Add(message.BatchId, new List<Guid>());
            }

            _completedItems[message.BatchId].Add(message.MessageId);

            Console.WriteLine($"Message {message.MessageId} marked as completed.");
        }

        [HttpGet]
        [Route("Batch")]
        public BatchStatus Status(string id)
        {
            Console.WriteLine($"Query for status of {id}.");

            var batchId = Guid.Parse(id);

            if (!_completedItems.ContainsKey(batchId))
            {
                return null;
            }

            return new BatchStatus()
            {
                Id = batchId,
                ItemsCompleted = _completedItems[batchId]
            };
        }
    }
}