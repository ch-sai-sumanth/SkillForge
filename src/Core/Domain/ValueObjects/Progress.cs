using Domain.Common;

namespace Domain.ValueObjects;

public sealed class Progress : ValueObject
{
    public const int MinValue = 0;
    public const int MaxValue = 100;

    public int Percentage { get; }

    private Progress(int percentage)
    {
        Percentage = percentage;
    }

    public static Progress Create(int percentage)
    {
        if (percentage < MinValue || percentage > MaxValue)
            throw new ArgumentOutOfRangeException(nameof(percentage), 
                $"Progress percentage must be between {MinValue} and {MaxValue}");

        return new Progress(percentage);
    }

    public static Progress Zero => new(0);
    public static Progress Complete => new(100);

    public bool IsComplete => Percentage == MaxValue;
    public bool IsStarted => Percentage > MinValue;

    public Progress Increase(int amount)
    {
        if (amount < 0)
            throw new ArgumentException("Cannot increase progress by negative amount", nameof(amount));

        var newPercentage = Math.Min(Percentage + amount, MaxValue);
        return new Progress(newPercentage);
    }

    public Progress Decrease(int amount)
    {
        if (amount < 0)
            throw new ArgumentException("Cannot decrease progress by negative amount", nameof(amount));

        var newPercentage = Math.Max(Percentage - amount, MinValue);
        return new Progress(newPercentage);
    }

    public Progress SetTo(int percentage)
    {
        return Create(percentage);
    }

    public static implicit operator int(Progress progress)
    {
        return progress.Percentage;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Percentage;
    }

    public override string ToString() => $"{Percentage}%";
}