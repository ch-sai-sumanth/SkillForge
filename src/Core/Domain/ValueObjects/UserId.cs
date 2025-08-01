using Domain.Common;

namespace Domain.ValueObjects;

public sealed class UserId : ValueObject
{
    public string Value { get; }

    private UserId(string value)
    {
        Value = value;
    }

    public static UserId Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("User ID cannot be null or empty", nameof(value));

        return new UserId(value);
    }

    public static UserId NewId()
    {
        return new UserId(Guid.NewGuid().ToString());
    }

    public static implicit operator string(UserId userId)
    {
        return userId.Value;
    }

    public static explicit operator UserId(string value)
    {
        return Create(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}