using Domain.Common;

namespace Domain.ValueObjects;

public sealed class GoalDescription : ValueObject
{
    public const int MaxLength = 2000;

    public string Value { get; }

    private GoalDescription(string value)
    {
        Value = value;
    }

    public static GoalDescription Create(string? value)
    {
        var trimmedValue = value?.Trim() ?? string.Empty;
        
        if (trimmedValue.Length > MaxLength)
            throw new ArgumentException($"Goal description cannot exceed {MaxLength} characters", nameof(value));

        return new GoalDescription(trimmedValue);
    }

    public static GoalDescription Empty => new(string.Empty);

    public bool IsEmpty => string.IsNullOrEmpty(Value);

    public static implicit operator string(GoalDescription description)
    {
        return description.Value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}