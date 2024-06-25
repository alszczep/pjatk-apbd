namespace Api.DTOs;

public class SubscriptionPaymentDTO
{
    public Guid SubscriptionId { get; set; }
    public decimal PaymentAmountInPln { get; set; }
}
