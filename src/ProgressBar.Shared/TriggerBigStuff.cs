using System;
using NServiceBus;

[Serializable]
public class TriggerBigStuff : IMessage
{
    public Guid Id { get; set; }
    public int HowMuchStuff { get; set; }
}