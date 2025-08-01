using Domain.Common;
using Domain.ValueObjects;

namespace Domain.DomainEvents;

public sealed class LearningGoalProgressUpdated : DomainEvent
{
    public LearningGoalProgressUpdated(
        LearningGoalId goalId,
        UserId menteeId,
        Progress previousProgress,
        Progress newProgress)
    {
        GoalId = goalId;
        MenteeId = menteeId;
        PreviousProgress = previousProgress;
        NewProgress = newProgress;
    }

    public LearningGoalId GoalId { get; }
    public UserId MenteeId { get; }
    public Progress PreviousProgress { get; }
    public Progress NewProgress { get; }
}