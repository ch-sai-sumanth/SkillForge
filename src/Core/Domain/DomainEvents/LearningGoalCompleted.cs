using Domain.Common;
using Domain.ValueObjects;

namespace Domain.DomainEvents;

public sealed class LearningGoalCompleted : DomainEvent
{
    public LearningGoalCompleted(
        LearningGoalId goalId,
        UserId menteeId,
        GoalTitle title,
        DateTime completedAt)
    {
        GoalId = goalId;
        MenteeId = menteeId;
        Title = title;
        CompletedAt = completedAt;
    }

    public LearningGoalId GoalId { get; }
    public UserId MenteeId { get; }
    public GoalTitle Title { get; }
    public DateTime CompletedAt { get; }
}