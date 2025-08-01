# LearningGoal - Proper DDD Implementation

## Overview

The LearningGoal has been transformed from an anemic domain model into a rich DDD entity with proper encapsulation, business rules, and domain events.

## Key Improvements

### ✅ **Before (Anemic Model)**
```csharp
public class LearningGoal
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = null!;
    public string Status { get; set; } = "InProgress";
    public int ProgressPercentage { get; set; } = 0;
    // ... other properties
}
```

### ✅ **After (Rich Domain Entity)**
```csharp
public sealed class LearningGoal : BaseEntity<LearningGoalId>
{
    // Private setters + validation
    public UserId MenteeId { get; private set; }
    public GoalTitle Title { get; private set; }
    public Progress Progress { get; private set; }
    
    // Factory method with validation
    public static LearningGoal Create(UserId menteeId, GoalTitle title, ...)
    
    // Business methods with rules
    public void UpdateProgress(int newProgressPercentage)
    public void CompleteGoal()
    public void ExtendTargetDate(TargetDate newTargetDate, string reason)
    
    // Domain events automatically raised
    // Business invariants enforced
}
```

## New Components

### 1. **Value Objects** 
Strong typing and validation:
- `LearningGoalId` - Strongly typed ID
- `GoalTitle` - Title with length validation (3-200 chars)
- `GoalDescription` - Description with max length (2000 chars)
- `Progress` - Progress percentage (0-100) with business methods
- `TargetDate` - Future date validation with utility methods
- `UserId` - Strongly typed user reference

### 2. **Domain Events**
Automatic event raising for:
- `LearningGoalCreated` - When goal is created
- `LearningGoalProgressUpdated` - When progress changes
- `LearningGoalCompleted` - When goal reaches 100% or is manually completed
- `LearningGoalAbandoned` - When goal is abandoned with reason
- `LearningGoalTargetDateExtended` - When target date is extended

### 3. **Business Rules Enforced**
- ✅ Title must be 3-200 characters
- ✅ Description max 2000 characters  
- ✅ Progress must be 0-100%
- ✅ Target date must be in the future
- ✅ Completed/abandoned goals cannot be modified
- ✅ Reasons required for abandonment and date extensions
- ✅ Auto-completion when progress reaches 100%

### 4. **Status Management**
Proper state transitions:
```csharp
public enum LearningGoalStatus
{
    InProgress = 1,
    Completed = 2, 
    Abandoned = 3,
    OnHold = 4
}
```

## Usage Examples

### Creating a Goal
```csharp
// In command handler
var menteeId = UserId.Create(request.MenteeId);
var title = GoalTitle.Create(request.Title);
var description = GoalDescription.Create(request.Description);
var targetDate = TargetDate.Create(request.TargetDate);

var goal = LearningGoal.Create(menteeId, title, description, targetDate);
// Domain event: LearningGoalCreated is automatically raised

await _repository.AddAsync(goal);
```

### Updating Progress
```csharp
// In command handler
var goal = await _repository.GetByIdAsync(goalId);
goal.UpdateProgress(75, "Completed advanced course module");
// Domain event: LearningGoalProgressUpdated is automatically raised

await _repository.UpdateAsync(goal);
```

### Completing a Goal
```csharp
var goal = await _repository.GetByIdAsync(goalId);
goal.CompleteGoal();
// Domain event: LearningGoalCompleted is automatically raised
// Progress automatically set to 100%
// Status changed to Completed
```

### Business Rule Violations
```csharp
// These will throw domain exceptions:
var title = GoalTitle.Create(\"\"); // Too short
var progress = Progress.Create(150); // Out of range
var targetDate = TargetDate.Create(DateTime.Now.AddDays(-1)); // Past date

// These will throw business rule violations:
completedGoal.UpdateProgress(50); // Cannot modify completed goal
goal.ExtendTargetDate(newDate, \"\"); // Reason required
```

## Repository Interface Updates

The repository interface should work with the new DDD entity:

```csharp
public interface IGoalRepository
{
    Task<LearningGoal?> GetByIdAsync(LearningGoalId id);
    Task<IEnumerable<LearningGoal>> GetByMenteeIdAsync(UserId menteeId);
    Task<IEnumerable<LearningGoal>> GetOverdueGoalsAsync();
    Task AddAsync(LearningGoal goal);
    Task UpdateAsync(LearningGoal goal);
    Task DeleteAsync(LearningGoalId id);
}
```

## Migration Strategy

### Phase 1: Parallel Implementation
1. Keep existing `LearningGoal` as `LearningGoalLegacy`
2. Implement new DDD `LearningGoal` 
3. Create new endpoints using DDD version
4. Run both versions in parallel

### Phase 2: Gradual Migration
1. Update existing endpoints one by one
2. Use API validation tests to ensure compatibility
3. Migration script to convert legacy data

### Phase 3: Complete Migration
1. Remove legacy implementation
2. Update all references
3. Clean up old code

## Benefits Achieved

### 🎯 **Rich Business Logic**
- Business rules enforced at domain level
- No invalid states possible
- Clear business operations

### 🔒 **Encapsulation**
- Private setters prevent invalid modifications
- Factory methods ensure valid creation
- Business methods are the only way to change state

### 📢 **Domain Events**
- Automatic event publication
- Loose coupling between bounded contexts
- Audit trail and integration support

### 🛡️ **Type Safety**
- Strong typing prevents errors
- Value objects ensure validation
- Impossible to use wrong IDs

### 📖 **Self-Documenting**
- Code expresses business intent
- Domain language used throughout
- Business rules visible in code

### 🧪 **Testability**
- Easy to unit test business logic
- Domain events can be verified
- No infrastructure dependencies

## Next Steps

1. **Update Repository Implementation**: Modify MongoDB repository to work with value objects
2. **Implement Event Handlers**: Create handlers for domain events
3. **Update API Endpoints**: Modify controllers to use new command/query handlers  
4. **Migration Scripts**: Create data migration from old to new format
5. **Update Tests**: Create comprehensive unit tests for domain logic

The LearningGoal is now a proper DDD entity that encapsulates business logic, enforces invariants, and publishes domain events - making it maintainable, testable, and aligned with business requirements! 🎉