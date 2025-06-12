namespace Application.DTOs;

public class CreatePaymentDto
{
    public string MenteeId { get; set; } = null!;
    public string UTRNumber { get; set; } = null!;
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? Notes { get; set; }
}