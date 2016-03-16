using System;

namespace ProgressBarMVC.Models
{
    public class StartBatchModel
    {
        public Guid BatchId { get; set; }
        public int NumberOfThingsToDo { get; set; }
    }
}