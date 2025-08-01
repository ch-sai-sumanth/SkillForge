using Domain.Common;

namespace Domain.ValueObjects;

public sealed class GoalTitle : ValueObject
{
    public const int MaxLength = 200;
    public const int MinLength = 3;

    public string Value { get; }

    private GoalTitle(string value)
    {
        Value = value;
    }

    public static GoalTitle Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Goal title cannot be null or empty", nameof(value));

        var trimmedValue = value.Trim();
        
        if (trimmedValue.Length < MinLength)
            throw new ArgumentException($"Goal title must be at least {MinLength} characters long", nameof(value));

        if (trimmedValue.Length > MaxLength)
            throw new ArgumentException($"Goal title cannot exceed {MaxLength} characters", nameof(value));

        return new GoalTitle(trimmedValue);
    }

    public static implicit operator string(GoalTitle title)
    {
        return title.Value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}