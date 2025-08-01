namespace Domain.Common;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
    Guid EventId { get; }
}

public abstract class DomainEvent : IDomainEvent
{
    protected DomainEvent()
    {
        EventId = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }

    public Guid EventId { get; }
    public DateTime OccurredOn { get; }
}