using Domain.Common;
using Domain.ValueObjects;

namespace Domain.DomainEvents;

public sealed class LearningGoalAbandoned : DomainEvent
{
    public LearningGoalAbandoned(
        LearningGoalId goalId,
        UserId menteeId,
        GoalTitle title,
        string reason,
        DateTime abandonedAt)
    {
        GoalId = goalId;
        MenteeId = menteeId;
        Title = title;
        Reason = reason;
        AbandonedAt = abandonedAt;
    }

    public LearningGoalId GoalId { get; }
    public UserId MenteeId { get; }
    public GoalTitle Title { get; }
    public string Reason { get; }
    public DateTime AbandonedAt { get; }
}