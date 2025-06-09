namespace User.Domain.Entities;

public class ActivityLog
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; }
    public string Action { get; set; }          
    public string Description { get; set; }    
    public DateTime Timestamp { get; set; } 
}