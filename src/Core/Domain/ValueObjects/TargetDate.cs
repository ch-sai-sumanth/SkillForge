using Domain.Common;

namespace Domain.ValueObjects;

public sealed class TargetDate : ValueObject
{
    public DateTime Value { get; }

    private TargetDate(DateTime value)
    {
        Value = value;
    }

    public static TargetDate Create(DateTime value)
    {
        if (value.Kind != DateTimeKind.Utc)
            value = value.ToUniversalTime();

        if (value <= DateTime.UtcNow)
            throw new ArgumentException("Target date must be in the future", nameof(value));

        return new TargetDate(value);
    }

    public static TargetDate CreateFromLocal(DateTime localDateTime)
    {
        var utcDateTime = localDateTime.ToUniversalTime();
        return Create(utcDateTime);
    }

    public bool IsOverdue => DateTime.UtcNow > Value;
    public bool IsDueToday => Value.Date == DateTime.UtcNow.Date;
    public bool IsDueSoon(int daysThreshold = 7) => 
        (Value - DateTime.UtcNow).TotalDays <= daysThreshold && !IsOverdue;

    public int DaysRemaining => Math.Max(0, (int)(Value.Date - DateTime.UtcNow.Date).TotalDays);
    public TimeSpan TimeRemaining => IsOverdue ? TimeSpan.Zero : Value - DateTime.UtcNow;

    public TargetDate ExtendBy(TimeSpan duration)
    {
        if (duration <= TimeSpan.Zero)
            throw new ArgumentException("Extension duration must be positive", nameof(duration));

        return new TargetDate(Value.Add(duration));
    }

    public TargetDate ExtendByDays(int days)
    {
        if (days <= 0)
            throw new ArgumentException("Days must be positive", nameof(days));

        return new TargetDate(Value.AddDays(days));
    }

    public static implicit operator DateTime(TargetDate targetDate)
    {
        return targetDate.Value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString("yyyy-MM-dd HH:mm:ss UTC");
}