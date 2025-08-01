namespace Domain.Common;

public abstract class BaseEntity<TId> : IEquatable<BaseEntity<TId>>
    where TId : ValueObject
{
    private readonly List<IDomainEvent> _domainEvents = new();

    protected BaseEntity(TId id)
    {
        Id = id;
        CreatedAt = DateTime.UtcNow;
    }

    protected BaseEntity() { } // For ORM

    public TId Id { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void MarkAsUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    public bool Equals(BaseEntity<TId>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as BaseEntity<TId>);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(BaseEntity<TId>? left, BaseEntity<TId>? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(BaseEntity<TId>? left, BaseEntity<TId>? right)
    {
        return !Equals(left, right);
    }
}