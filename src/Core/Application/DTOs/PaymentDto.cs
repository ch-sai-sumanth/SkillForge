namespace Application.DTOs;

public class PaymentDto
{
    public string Id { get; set; } = null!;
    public string MenteeId { get; set; } = null!;
    public string UTRNumber { get; set; } = null!;
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string Status { get; set; } = null!;
    public string? Notes { get; set; }
    public DateTime RecordedAt { get; set; }
}