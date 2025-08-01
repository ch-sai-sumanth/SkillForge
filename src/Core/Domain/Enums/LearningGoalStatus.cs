namespace Domain.Enums;

public enum LearningGoalStatus
{
    InProgress = 1,
    Completed = 2,
    Abandoned = 3,
    OnHold = 4
}

public static class LearningGoalStatusExtensions
{
    public static bool IsActive(this LearningGoalStatus status)
    {
        return status == LearningGoalStatus.InProgress;
    }

    public static bool IsFinished(this LearningGoalStatus status)
    {
        return status == LearningGoalStatus.Completed || status == LearningGoalStatus.Abandoned;
    }

    public static bool CanTransitionTo(this LearningGoalStatus currentStatus, LearningGoalStatus newStatus)
    {
        return currentStatus switch
        {
            LearningGoalStatus.InProgress => newStatus != LearningGoalStatus.InProgress,
            LearningGoalStatus.OnHold => newStatus == LearningGoalStatus.InProgress || 
                                        newStatus == LearningGoalStatus.Abandoned,
            LearningGoalStatus.Completed => false, // Completed goals cannot change status
            LearningGoalStatus.Abandoned => false, // Abandoned goals cannot change status
            _ => false
        };
    }

    public static string ToFriendlyString(this LearningGoalStatus status)
    {
        return status switch
        {
            LearningGoalStatus.InProgress => "In Progress",
            LearningGoalStatus.Completed => "Completed",
            LearningGoalStatus.Abandoned => "Abandoned",
            LearningGoalStatus.OnHold => "On Hold",
            _ => status.ToString()
        };
    }
}