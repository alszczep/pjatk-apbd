namespace api.Models;

public class SubscriptionPayment
{
    public Guid Id { get; init; }
    public decimal AmountPaid { get; set; }
    public DateOnly PeriodLastDay { get; set; }

    public Subscription Subscription { get; set; } = null!;
}
