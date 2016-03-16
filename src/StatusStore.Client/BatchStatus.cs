using System;
using System.Collections.Generic;

public class BatchStatus
{
    public Guid Id { get; set; }
    public ICollection<Guid> ItemsCompleted { get; set; }
}