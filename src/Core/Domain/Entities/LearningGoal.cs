using Domain.Common;
using Domain.ValueObjects;
using Domain.DomainEvents;
using Domain.Enums;

namespace Domain.Entities;

public sealed class LearningGoal : BaseEntity<LearningGoalId>
{
    private LearningGoal() : base() { } // For ORM

    private LearningGoal(
        LearningGoalId id,
        UserId menteeId,
        GoalTitle title,
        GoalDescription description,
        TargetDate targetDate) : base(id)
    {
        MenteeId = menteeId;
        Title = title;
        Description = description;
        TargetDate = targetDate;
        Status = LearningGoalStatus.InProgress;
        Progress = Progress.Zero;

        AddDomainEvent(new LearningGoalCreated(id, menteeId, title, targetDate));
    }

    public UserId MenteeId { get; private set; } = null!;
    public GoalTitle Title { get; private set; } = null!;
    public GoalDescription Description { get; private set; } = null!;
    public TargetDate TargetDate { get; private set; } = null!;
    public LearningGoalStatus Status { get; private set; }
    public Progress Progress { get; private set; } = null!;
    public DateTime? CompletedAt { get; private set; }
    public DateTime? AbandonedAt { get; private set; }
    public string? AbandonReason { get; private set; }

    // Factory method for creating new learning goals
    public static LearningGoal Create(
        UserId menteeId,
        GoalTitle title,
        GoalDescription description,
        TargetDate targetDate)
    {
        ValidateGoalCreation(menteeId, title, targetDate);
        
        var goalId = LearningGoalId.NewId();
        return new LearningGoal(goalId, menteeId, title, description, targetDate);
    }

    // Business method: Update progress
    public void UpdateProgress(int newProgressPercentage, string? updateReason = null)
    {
        GuardAgainstFinishedGoal();
        
        var newProgress = Progress.Create(newProgressPercentage);
        
        if (newProgress.Equals(Progress))
            return; // No change needed

        var previousProgress = Progress;
        Progress = newProgress;
        MarkAsUpdated();

        AddDomainEvent(new LearningGoalProgressUpdated(Id, MenteeId, previousProgress, newProgress));

        // Auto-complete if progress reaches 100%
        if (Progress.IsComplete && Status == LearningGoalStatus.InProgress)
        {
            CompleteGoal();
        }
    }

    // Business method: Complete the goal
    public void CompleteGoal()
    {
        GuardAgainstFinishedGoal();
        
        if (!Progress.IsComplete)
        {
            Progress = Progress.Complete;
        }

        Status = LearningGoalStatus.Completed;
        CompletedAt = DateTime.UtcNow;
        MarkAsUpdated();

        AddDomainEvent(new LearningGoalCompleted(Id, MenteeId, Title, CompletedAt.Value));
    }

    // Business method: Abandon the goal
    public void Abandon(string reason)
    {
        GuardAgainstFinishedGoal();
        
        if (string.IsNullOrWhiteSpace(reason))
            throw new BusinessRuleViolationException(
                "Goal abandonment requires reason",
                "A reason must be provided when abandoning a learning goal");

        Status = LearningGoalStatus.Abandoned;
        AbandonedAt = DateTime.UtcNow;
        AbandonReason = reason.Trim();
        MarkAsUpdated();

        AddDomainEvent(new LearningGoalAbandoned(Id, MenteeId, Title, reason, AbandonedAt.Value));
    }

    // Business method: Extend target date
    public void ExtendTargetDate(TargetDate newTargetDate, string reason)
    {
        GuardAgainstFinishedGoal();
        
        if (string.IsNullOrWhiteSpace(reason))
            throw new BusinessRuleViolationException(
                "Target date extension requires reason",
                "A reason must be provided when extending the target date");

        if (newTargetDate.Value <= TargetDate.Value)
            throw new BusinessRuleViolationException(
                "Invalid target date extension",
                "New target date must be later than the current target date");

        var previousTargetDate = TargetDate;
        TargetDate = newTargetDate;
        MarkAsUpdated();

        AddDomainEvent(new LearningGoalTargetDateExtended(
            Id, MenteeId, previousTargetDate, newTargetDate, reason));
    }

    // Business method: Update goal details
    public void UpdateDetails(GoalTitle newTitle, GoalDescription newDescription)
    {
        GuardAgainstFinishedGoal();
        
        bool hasChanges = false;

        if (!Title.Equals(newTitle))
        {
            Title = newTitle;
            hasChanges = true;
        }

        if (!Description.Equals(newDescription))
        {
            Description = newDescription;
            hasChanges = true;
        }

        if (hasChanges)
        {
            MarkAsUpdated();
        }
    }

    // Business method: Put goal on hold
    public void PutOnHold(string reason)
    {
        if (Status != LearningGoalStatus.InProgress)
            throw new InvalidDomainOperationException(
                "Only goals in progress can be put on hold");

        if (string.IsNullOrWhiteSpace(reason))
            throw new BusinessRuleViolationException(
                "Goal hold requires reason",
                "A reason must be provided when putting a goal on hold");

        Status = LearningGoalStatus.OnHold;
        MarkAsUpdated();
    }

    // Business method: Resume goal from hold
    public void Resume()
    {
        if (Status != LearningGoalStatus.OnHold)
            throw new InvalidDomainOperationException(
                "Only goals on hold can be resumed");

        Status = LearningGoalStatus.InProgress;
        MarkAsUpdated();
    }

    // Query methods
    public bool IsOverdue => !Status.IsFinished() && TargetDate.IsOverdue;
    public bool IsDueToday => !Status.IsFinished() && TargetDate.IsDueToday;
    public bool IsDueSoon(int daysThreshold = 7) => !Status.IsFinished() && TargetDate.IsDueSoon(daysThreshold);
    public int DaysRemaining => Status.IsFinished() ? 0 : TargetDate.DaysRemaining;
    public bool CanBeModified => !Status.IsFinished();

    // Private validation methods
    private static void ValidateGoalCreation(UserId menteeId, GoalTitle title, TargetDate targetDate)
    {
        if (menteeId == null)
            throw new ArgumentNullException(nameof(menteeId));
        
        if (title == null)
            throw new ArgumentNullException(nameof(title));
        
        if (targetDate == null)
            throw new ArgumentNullException(nameof(targetDate));
    }

    private void GuardAgainstFinishedGoal()
    {
        if (Status.IsFinished())
            throw new InvalidDomainOperationException(
                $"Cannot modify a {Status.ToFriendlyString().ToLower()} learning goal");
    }
}