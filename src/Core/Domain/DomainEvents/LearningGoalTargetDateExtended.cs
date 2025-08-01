using Domain.Common;
using Domain.ValueObjects;

namespace Domain.DomainEvents;

public sealed class LearningGoalTargetDateExtended : DomainEvent
{
    public LearningGoalTargetDateExtended(
        LearningGoalId goalId,
        UserId menteeId,
        TargetDate previousTargetDate,
        TargetDate newTargetDate,
        string reason)
    {
        GoalId = goalId;
        MenteeId = menteeId;
        PreviousTargetDate = previousTargetDate;
        NewTargetDate = newTargetDate;
        Reason = reason;
    }

    public LearningGoalId GoalId { get; }
    public UserId MenteeId { get; }
    public TargetDate PreviousTargetDate { get; }
    public TargetDate NewTargetDate { get; }
    public string Reason { get; }
}