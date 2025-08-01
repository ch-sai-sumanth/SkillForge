using Domain.Common;

namespace Domain.ValueObjects;

public sealed class LearningGoalId : ValueObject
{
    public string Value { get; }

    private LearningGoalId(string value)
    {
        Value = value;
    }

    public static LearningGoalId Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("LearningGoal ID cannot be null or empty", nameof(value));

        return new LearningGoalId(value);
    }

    public static LearningGoalId NewId()
    {
        return new LearningGoalId(Guid.NewGuid().ToString());
    }

    public static implicit operator string(LearningGoalId learningGoalId)
    {
        return learningGoalId.Value;
    }

    public static explicit operator LearningGoalId(string value)
    {
        return Create(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}