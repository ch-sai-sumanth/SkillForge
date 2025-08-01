using Domain.Common;
using Domain.ValueObjects;

namespace Domain.DomainEvents;

public sealed class LearningGoalCreated : DomainEvent
{
    public LearningGoalCreated(
        LearningGoalId goalId,
        UserId menteeId,
        GoalTitle title,
        TargetDate targetDate)
    {
        GoalId = goalId;
        MenteeId = menteeId;
        Title = title;
        TargetDate = targetDate;
    }

    public LearningGoalId GoalId { get; }
    public UserId MenteeId { get; }
    public GoalTitle Title { get; }
    public TargetDate TargetDate { get; }
}