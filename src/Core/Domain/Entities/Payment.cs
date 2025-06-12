using Domain;

namespace User.Domain.Entities;

public class Payment
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string MenteeId { get; set; } = null!;
    public decimal Amount { get; set; }
    public string UTRNumber { get; set; } = null!;
    public DateTime PaymentDate { get; set; }
    public string Status { get; set; } = PaymentStatus.Pending.ToString(); // Pending, Confirmed, Failed
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime RecordedAt { get; set; }
}